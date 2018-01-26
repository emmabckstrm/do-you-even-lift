using VRTK;
using VRTK.GrabAttachMechanics;
using System.Collections;
using UnityEngine;

public class GrabbedObjectTrackVelocity2 : VRTK_InteractableObject
{
    float spinSpeed = 0f;
    Transform rotator;

    public float maxVelocity = 1f;
    private Rigidbody rb;
    private Vector3 nullVector;
    private float maxVelocityRelMass;
    //protected VRTK_BaseJointGrabAttach joint;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
    }


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxVelocityRelMass = rb.mass * 2;
        maxVelocity = maxVelocityRelMass;
        nullVector = new Vector3(0, 0, 0);
    }


    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //LogVelocity();
        CheckVelocity();

    }

    private void CheckVelocity()
    {
        if (rb.velocity.y > maxVelocity)
        {
            Debug.Log("TOO FAST! track velocity script 2222");
            ForceReleaseGrab();
        }
    }

    private void LogVelocity()
    {
        if (rb.velocity != nullVector)
        {
            Debug.Log(rb.velocity);
        }
    }


}

