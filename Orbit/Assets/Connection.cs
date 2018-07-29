using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/* The Connection scprit draws lines from satellite to satellite or base
   station to satelite if they are withing a certain proximity of eachother.

   Created by : Antonio Castro
 */
public class Connection : MonoBehaviour {
    private double distanceBase = 200;                                                   // Distance for base station to sat connection
    private double distanceSat = 100;                                                    // Distance for sat to sat connection 
    private Vector3 earthBase = new Vector3(505, 326, 549);                              // Marks base station on earth (x,y,z)
    private Vector3[] gamePiecesLocations;                                               // Holds all locations of satellites

    void Start () {                                              
    gamePiecesLocations = new Vector3[ReadFile.numOfSats];
        StartCoroutine("Connect");
    }

    IEnumerator Connect()
    {
       Vector3 initial;                                                        // Location of our initial point
       Vector3 final;                                                          // Location of our final point
       double distance;                                                        // Distance between our initial and final point
       int run = 1;                                                            // Ensures we draw connections to completion             
        while (run == 1)
        {
            yield return new WaitForSeconds(0.001f);                           // Time between each new drawn line
            for (int i = 0; i <= ReadFile.numOfSats - 1; i++)                  // Outer loop to grab all sats locations
            {
                string temp = i.ToString();                                    
                GameObject tempG = GameObject.Find(temp);                      
                gamePiecesLocations[i] = tempG.transform.position;             // Use a satellite for the initial point
            }

            for (int j = 0; j <= ReadFile.numOfSats - 1; j++)                  // Inner loop which grabs the other sats and relays distance 
            {
                int currentsat = j;                                            
                initial = gamePiecesLocations[j];                              // Use Earth Base coorinates for the final point
                final = earthBase;
                distance = Mathf.Sqrt((Mathf.Pow((initial[0] - earthBase[0]), 2)) + (Mathf.Pow((initial[2] - earthBase[2]), 2)) + (Mathf.Pow((initial[1] - earthBase[1]), 2)));   // Gets distance between earth base and current sat 
                if (distance < distanceBase)                                            
                {
                    Debug.DrawLine(initial, final, Color.red);                 // If distance is in range draw line
                }

                for (int k = 0; k <= ReadFile.numOfSats - 1; k++)              // Grabs the other sats for a distance comparison to our current sat  
                {
                    if (k != currentsat)                                       // ensures we are not drawing lines from and to our current sat
                    {
                         initial = gamePiecesLocations[j];                     // Current sat location
                         final = gamePiecesLocations[k];                       // Other sat location
                         distance = Mathf.Sqrt((Mathf.Pow((initial[0] - final[0]), 2)) + (Mathf.Pow((initial[2] - final[2]), 2)) + (Mathf.Pow((initial[1] - final[1]), 2)));    // Gets distance between two sats
                        if (distance < distanceSat)
                        {
                            Debug.DrawLine(initial,final,Color.red);            // If distance is in range draw line
                        }
                    }
                }
            }
        }

    }


}
