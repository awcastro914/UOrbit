using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* The CameraSnap script provides a field to allow the camera to snap to a specific
   satellite by dragging said satellite into the field in the editor. This camera
   is defaulted to display 4 in game.
    Created by : Antonio Castro
 */
public class CameraSnap : MonoBehaviour {

    public Transform target;                           // Field to allow for a satellite to be snapped to  
    public Vector3 offset;                             // Offset to allow for a better camera angle for viewing

    // Update is called once per frame
    void LateUpdate () {
        offset[0] = -50;
        transform.position = target.position + offset; // Tranforms camera to location of satellite + offset 
    }
}
