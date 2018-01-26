// Fixed Joint Grab Attach|GrabAttachMechanics|50040
namespace VRTK.GrabAttachMechanics
{
    using UnityEngine;
    public class FixedJointGrabAttachCustom2 : VRTK_FixedJointGrabAttach
    {
        Rigidbody rb;
        public float velocityLimit = 0.1f;
        private Vector3 nullVector = new Vector3(0.0f, 0.0f, 0.0f);



        // Use this for initialization
        void Start()
        {

            rb = grabbedObjectRigidBody;
        }

        /// <summary>
        /// The ProcessFixedUpdate method is run in every FixedUpdate method on the interactable object. It applies velocity to the object to ensure it is tracking the grabbing object.
        /// </summary>
        public override void ProcessFixedUpdate()
        {
            base.ProcessFixedUpdate();
            if (grabbedObjectRigidBody.velocity != nullVector)
            {
                Debug.Log("testing to print velocity" + grabbedObjectRigidBody.velocity);
            }
            CheckVelocity();
        }

        // Checks the velocity 
        private void CheckVelocity()
        {
            if (Mathf.Abs(grabbedObjectRigidBody.velocity.y) > velocityLimit)
            {
                Debug.Log("Grabb attachment says TOO FAST!");
                ForceReleaseGrab();
            }
        }
    }
}