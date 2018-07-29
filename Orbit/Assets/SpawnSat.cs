using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* The Spawnsat script spawns the satellites at certain areas around the planet
   Depending on where they are initialized in the log file.
   
   Created by : Antonio Castro
 */
public class SpawnSat : MonoBehaviour {

    public GameObject satInstance;                                                                      //Instance of a satellite 
    GameObject satInstanceClone;                                                                        //Clone the satellite to create more   
    void Start () {
        int numOfSats = ReadFile.numOfSats;                                                             //Number of satellites to spawn (from log file)
        int satCount = 0;
        for (int i = 0; i < numOfSats; i++)                                                             //Loop through for amount of satellites needed
        {
            string[] logLines = ReadFile.ReadCommandLinecsv(i + 1);                                     //Get initial log line for satellite
            string initDegrees = logLines[ReadFile.trueAnomalyLocation];                                //Get inital true anomaly
            string initTurn = logLines[41];                                                             //Get initial rotation
            float initDegreesint;                                                                         
            if (initDegrees.Equals("0.000000") || initDegrees.Equals("0"))                                                                //change rotation to integer
            {
                initDegreesint = 0;
            }
            else
            {
                initDegreesint = float.Parse(initDegrees);
            }

            float x = 335.54f + ((Mathf.Cos(initDegreesint))*250); 
            float y = 326.42f;
            float z = 549.32f + ((Mathf.Sin(initDegreesint))*250);  
            transform.position = new Vector3(x, y, z);                                                  //Set transform to intial location
            satInstanceClone = Instantiate(satInstance, transform.position, Quaternion.identity) as GameObject; //spawn satellite clone
            satInstanceClone.name = satCount.ToString();
            satCount++;

        }
    }


}
