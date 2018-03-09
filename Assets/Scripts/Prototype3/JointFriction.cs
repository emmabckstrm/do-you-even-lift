namespace VRTK {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class JointFriction : MonoBehaviour {

		[Tooltip("mulitiplier for the angular velocity for the torque to apply.")]
	  public float friction = 0.8f;

	  private HingeJoint hinge;
		protected Joint joint;
	  private Rigidbody _thisBody;
		private GameObject connectedObject;
	  private Rigidbody connectedBody;
	  private Vector3 _axis;  //local space
		private float angularV;
		private Vector3 worldAxis;
		private Vector3 worldTorque;

		protected GrabAttachMechanics.VRTK_CustomJointGrabAttach customJointScript;
		protected InteractableObjectCustom interactableObjScript;

	  // Use this for initialization
	  void Start () {
			customJointScript = GetComponent<GrabAttachMechanics.VRTK_CustomJointGrabAttach>();
			interactableObjScript = GetComponent<InteractableObjectCustom>();
	    hinge = (HingeJoint)customJointScript.customJoint;
	    connectedObject = interactableObjScript.GetGrabbingObject();
			connectedBody = connectedObject.GetComponent<Rigidbody>();
	    _axis = hinge.axis;

	    _thisBody = GetComponent<Rigidbody>();
	  }

	  // Update is called once per frame
	  void FixedUpdate () {
			if (interactableObjScript.IsGrabbed()) {
				angularV = hinge.velocity;
		    Debug.Log("angularV " + angularV);
		    worldAxis = transform.TransformVector(_axis);
		    worldTorque = friction * angularV * worldAxis;

		    _thisBody.AddTorque(-worldTorque);
		    connectedBody.AddTorque(worldTorque);
			}

	  }
	}
}
