using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*The SpawnInfo script spawns text meshes to display data of the satellites.
 *The number of textmeshes spawned is related the amount of satellites.
   
   Created by : Antonio Castro
 */


public class SpawnInfo : MonoBehaviour {
    public GameObject infoInstance;                                                                                     //Instance of a satellite 
    GameObject infoInstanceClone;                                                                                       //Clone of satellite
    void Start () {
        int numOfSats = ReadFile.numOfSats;                                                                             //Get number of satellites
        int satCount = 0;                                                                                               //Counter for amount of textmeshes spawned

        for (int i = 0; i < numOfSats; i++)                                                                             //Loop through to spawn Textmeshed
        {
            transform.position = new Vector3(0, 0, 0);
            infoInstanceClone = Instantiate(infoInstance, transform.position, Quaternion.identity) as GameObject;       //Create Textmesh
            infoInstanceClone.name = satCount.ToString() + "_info";                                                     //Name Textmesh as int from 0 to n where n is number of satellites
            satCount++;

        }
    }
}


