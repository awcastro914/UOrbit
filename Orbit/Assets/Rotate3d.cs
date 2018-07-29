using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEditor;

/* The Rotate3d script will rotate a cube a certain amount of degrees which
   is designated by the log file. The rotation is done using the Slerp function
   which interpolates between two angles.
   
   Created by : Antonio Castro
 */
public class Rotate3d : MonoBehaviour {

    private string ourName;                                                               // Name of current satellite
    private int readline = 1;                                                             // Line to read in log file for rotation
    int ourLines;                                                                         // Total lines in file
    int iterations;                                                                       // Amount of times information is logged for each satellite
    double[] ourCommandArray;                                                             // Contains all of our rotation commands
    Quaternion from;                                                                      // Our starting rotation
    Quaternion to;                                                                        // Our finishing rotaiton
    float speed = 1.0F;                                                                   // Adjusts rotation speed lower = slower
    private void Start()
    {
        ourName = gameObject.name;                                                        // Retrieves the name of our current sat
        readline = int.Parse(ourName) + 1;                                                // Command for sat is located at line name + 1
        ourLines = ReadFile.countLinesInFileCut(ReadFile.logFile);                           // Gets total lines in file                                       
        int i = 0;
        if (ReadFile.numOfSats == 1)                                                      // Fixes case where iterations will be 2 for 1 sat total always due to a minimum of 2 lines 
        {
            iterations = ourLines - 1;
        }
        else                                                                              // base case where we have more than 1 sat
        {
            iterations = ourLines / ReadFile.numOfSats;                                   // Iterations are based on (amount of lines in file) / (amount of satelites)
        }
        ourCommandArray = new double[iterations];                                         // Create an array to hold the commands
        while (readline <= ourLines - 1)                                                  // Cycles through lines in file to get command
        {
            string[] tempArray = ReadFile.ReadCommandLine(readline, ReadFile.logFile);    // Reads line based on name of sat.(Ex. Sat 5 will read line 6.) and breaks it up by commas. 
            ourCommandArray[i] = Convert.ToDouble(tempArray[ReadFile.yawLocation]);                         // The rotate command is located at position 41 so we take it and add it to command array.
            i++;                                                                          // Increment place in command array
            readline = readline + ReadFile.numOfSats;                                     // Next line that is read is based on current line + number of sats
        }
        StartCoroutine("rotate3d");
    }

    IEnumerator rotate3d()                                                                
    {
        int j = 0;
        int turnComplete = 1;                                                            // Check if we have completed our rotation
        int done = 1;                                                                    // Check if we are done with all rotations
        int firstRotation = 1;                                                           // Check if it is first rotation
        float counter = 0;                                                               // Keeps track of time to be used in the Slerp function to rotate the satellite
        while (done == 1)                                                                 // run commands until done
        {                                                                                 // If already at target destination wait heartbeat length
            while (counter <= ReadFile.heartBeat) {
                counter += Time.deltaTime;
            if (turnComplete == 1 && j < iterations)                                      // If rotation is complete 
            {
                Vector3 temp = new Vector3(0, (float)ourCommandArray[j], 0);              // Create a 3D vector to represent the destination
                turnComplete = 0;

                from = transform.rotation;                                                // Our initial rotation be our current rotational position
                to = Quaternion.Euler(temp);
            }
            if (firstRotation == 1)                                                         // checks to initialize sat at correct rotation
            {
                transform.rotation = Quaternion.Slerp(from, to, Time.time * speed * 10000); // Snap to initial angle when initializing
                yield return new WaitForSeconds(0.0f);
                counter = 0;
                if (to.eulerAngles == gameObject.transform.rotation.eulerAngles)
                {
                    firstRotation = 0;
                }
            }
            else {
                if (from.eulerAngles == to.eulerAngles)                                                                      // Check if Destination is equal to our initial
                {
                    yield return new WaitForSeconds((ReadFile.heartBeat)/(ReadFile.time));                                                        // If already at target destination wait heartbeat length
                        counter = 0;
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(from, to, (counter/ ReadFile.heartBeat) * ReadFile.time);             // Rotate CHANGE TO HEARTBEAT speed/ReadFile.heartBeat
                    yield return new WaitForSeconds(0.0f);
                }
            }
            if (gameObject.transform.rotation.eulerAngles == to.eulerAngles)                                                            // if our rotation is done
            {
                turnComplete = 1;
                j++;
                counter = 0;
                }
            if(j >= ourCommandArray.Length)
                {
                    counter = 100000;
                    done = 0;
                }
            }
    }
    }
}
