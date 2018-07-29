using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* The EasyOrbit script is used to make the satellites orbit around a central point.
   Currently we are rotating around the center of the earth.

   Created by : Antonio Castro
 */

public class EasyOrbit : MonoBehaviour
{
   private float width = 250;                                                    //Width and height represent size of orbit 
   private float height = 250;                                                   
   private string ourName;                                                       //Name of our current satellite
   private int ourNameint;                                                       //Integer representation of our satellite  
   private float finalDegreesint;                                                //Degrees to move to on the next update
   private float initDegreesint;                                                 //Degrees started on
   public static float loops;                                                    //Amount of heartbeats recieved
   private float timeToCompletion;                                               //Time to run
   private float inclinationint;                                                 //Integer representation of inclination
   private float semimajorAxisint;                                               //Integer representation of semimajor axis
   private TrailRenderer setTime;
    void Start()
    {
        ourName = gameObject.name;                                        
        ourNameint = (int)CheckString(ourName);
        string[] logLines = ReadFile.ReadCommandLinecsv(ourNameint + 1);                         //Reads at line equal to name of satellite to get initial degrees, needs plus 1 offest to adjust for initial information line
        string initDegrees = logLines[ReadFile.trueAnomalyLocation];                             //Reads degrees at init line
        string inclination = logLines[ReadFile.inclinaitionLocation];                            //Reads inclination
        logLines = ReadFile.ReadCommandLinecsv(ourNameint + ReadFile.numOfSats * 3 + 1);         //Reads at line equal to name of satellite plus offset to get final degrees
        string finalDegrees = logLines[ReadFile.trueAnomalyLocation];                            //Reads degrees at final line
        string semimajorAxis = logLines[ReadFile.semimajorAxisLocation];                         //Reads semimajor axis
        finalDegreesint = CheckString(finalDegrees);                                             //Through line 37 is parsing string information into int
        initDegreesint = CheckString(initDegrees);
        semimajorAxisint = CheckString(semimajorAxis);
        inclinationint = CheckString(inclination);
        timeToCompletion = 360 / ((finalDegreesint - initDegreesint)/(float)ReadFile.heartBeat); //Time to finish a rotation 
        StartCoroutine("Orbit");
    }

    IEnumerator Orbit()
    {
        float rotation = 0;                                                                      //Current spot in terms of true anomaly
        float timeCounter = 0;                                                                   //Keeps track of time since start
        loops = (((ReadFile.countLinesInFileCut(ReadFile.logFile)-1)/ReadFile.numOfSats)-1) * (float)ReadFile.heartBeat;  //Number of hearbeats
        while (Time.time < (loops / ReadFile.time))                                              // Continues running until end of simulation
        {
            yield return new WaitForSeconds(0.01f);
            if (Time.time < (ReadFile.heartBeat * 2))
            {
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
                timeCounter += Time.deltaTime * 1;                                                   //Update current time
                rotation = ((timeCounter / timeToCompletion) * 360) * ReadFile.time;                 //Get incremental change in the form of a precent

                float x = (Mathf.Cos(((rotation + initDegreesint) * Mathf.PI) / 180) * width) + 335.54f;                  //Update x location
                float y = (Mathf.Cos(((rotation + initDegreesint) * Mathf.PI) / 180) * inclinationint) + 326.42f;         //Update y location
                float z = (Mathf.Sin(((rotation + initDegreesint) * Mathf.PI) / 180) * height) + 549.32f;                 //Update z location

                transform.position = new Vector3(x, y, z);                                                                //Set new position
                if (Time.time < ReadFile.heartBeat * 2 + 1)
                {
                    setTime = GameObject.Find(name).GetComponent(typeof(TrailRenderer)) as TrailRenderer;
                    setTime.time = 1;
                }
                else
                {
                    setTime.time = 300;
                }
            }

        }
    }

    public static float CheckString(string String)                            // ensures never add null
    {
        float ourint;
        if (String.Equals("0") || String.Equals("0.000000"))
        {
            ourint = 0.0f;
        }
        else
        {
            ourint = float.Parse(String);
        }
        return ourint;
    }


}
