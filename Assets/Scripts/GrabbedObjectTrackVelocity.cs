using VRTK;
using System.Collections;
using UnityEngine;

public class GrabbedObjectTrackVelocity : VRTK_InteractableObject
{
	float spinSpeed = 0f;
	Transform rotator;

	public float maxVelocity = 1f;
	private Rigidbody rb;
	private Vector3 nullVector;
    private float maxVelocityRelMass;

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
        CheckVelocity();

    }

	private void CheckVelocity() {
        if (rb.velocity != nullVector)
        {
            LogVelocity();
            if (rb.velocity.y > maxVelocity)
            {
                Debug.Log("TOO FAST! track velocity script");
                ForceReleaseGrab();
            }
        }
    }

	private void LogVelocity()
	{
		
		Debug.Log("Velcotiy.. " + rb.velocity);
		
	}


}

