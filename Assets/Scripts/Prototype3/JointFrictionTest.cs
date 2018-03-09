using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointFrictionTest : MonoBehaviour {

	[Tooltip("mulitiplier for the angular velocity for the torque to apply.")]
  public float Friction = 0.4f;

  private HingeJoint _hinge;
  private Rigidbody _thisBody;
  private Rigidbody _connectedBody;
  private Vector3 _axis;  //local space
	private float angularV;
	private Vector3 worldAxis;
	private Vector3 worldTorque;

  // Use this for initialization
  void Start () {
    _hinge = GetComponent<HingeJoint>();
    _connectedBody = _hinge.connectedBody;
    _axis = _hinge.axis;

    _thisBody = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void FixedUpdate () {
    angularV = _hinge.velocity;
    //Debug.Log("angularV " + angularV);
    worldAxis = transform.TransformVector(_axis);
    worldTorque = Friction * angularV * worldAxis;

    //_thisBody.AddTorque(-worldTorque);
    //_connectedBody.AddTorque(worldTorque);
  }
}
