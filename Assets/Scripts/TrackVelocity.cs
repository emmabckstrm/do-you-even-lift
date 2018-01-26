using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackVelocity : MonoBehaviour {

    public Rigidbody rb;
    private Vector3 nullVector;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        nullVector = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void LogVelocity()
    {
        if (rb.velocity != nullVector)
        {
            Debug.Log(rb.velocity);
        }
    }
}
