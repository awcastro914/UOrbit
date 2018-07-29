using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The CameraFollow script is designed to allow a camera to follow a specific satellite
   and cycle through them using the up and down arrow keys. This camera is defaulted to
   display 3 in game.

    Created by : Antonio Castro
 */
public class CameraFollow : MonoBehaviour {
    private Vector3 offset;                        // Offset to allow for a better camera angle for viewing
    private int currentSat = 1;                    // Initialize the cammera to start at satellite 1

    // Update is called once per frame
    void LateUpdate () {
        if (Input.GetKeyDown(KeyCode.UpArrow))     // Moves up to next satellite upon up arrow event
        {
            if (currentSat + 1 <= ReadFile.numOfSats-1)
            {
                currentSat++;
            }
            else
            {
                currentSat = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))   // Moves down to previous satellite upon down arrow event
        {
            if(currentSat - 1 >= 0)
            {
                currentSat--;
            }
            else
            {
                currentSat = ReadFile.numOfSats-1;
            }
        }
        //Debug.Log("SatFollow " + currentSat);
        Transform target = GameObject.Find(currentSat.ToString()).transform; // Finds a specific sat by name
        offset[0] = -50;
        transform.position = target.position + offset;                       // Tranforms camera to location of satellite + offset
    }


}
