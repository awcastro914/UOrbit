using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbiting : MonoBehaviour {
    public Transform EarthTarget;
    public Transform moon;
    private Rigidbody thisRigidBody;

    private void Awake()
    {
        moon = transform;
        this.thisRigidBody = this.GetComponent<Rigidbody>();
        this.thisRigidBody.AddForce(transform.forward * 200);
       // this.thisRigidBody.AddForce(transform.right * 100);
       // this.thisRigidBody.AddForce(transform.up * 100);
    }

    // Use this for initialization
    void Start () {
        GameObject earth = GameObject.FindGameObjectWithTag("earth");
        EarthTarget = earth.transform;

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 line = EarthTarget.position - moon.position;
        line.Normalize();

        float distance = Vector3.Distance(EarthTarget.position, moon.position);
        this.thisRigidBody.AddForce(line * 10 / distance);
        Vector3 pos =this.thisRigidBody.position;
    }
}
