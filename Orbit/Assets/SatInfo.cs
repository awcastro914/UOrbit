using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatInfo : MonoBehaviour {
    private GameObject TrackObject;
    private Vector3 Offset;

    // Update is called once per frame
    void Update () {
        TrackObject = GameObject.Find("0");
        Transform target = GameObject.Find("0").transform;
        if (TrackObject.name != null)
            gameObject.transform.position = Camera.main.WorldToScreenPoint(TrackObject.transform.position) + Offset;
    }
}
