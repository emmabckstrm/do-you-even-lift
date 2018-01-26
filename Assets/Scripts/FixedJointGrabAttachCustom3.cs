// Fixed Joint Grab Attach|GrabAttachMechanics|50040
namespace VRTK.GrabAttachMechanics
{
    using UnityEngine;
    public class FixedJointGrabAttachCustom3 : VRTK_FixedJointGrabAttach
    {
        public float speedLimit = 10f;
        private Vector3 nullVector = new Vector3(0.0f, 0.0f, 0.0f);
        private float speed = 0;
        private Vector3 lastPosition = Vector3.zero;

        // Different types of movement limitations
        public enum MovementLimitationTypes {
            VelocityAnyDirection,
            VelocityVertical,
            AccelerationAnyDirection,
            AccelerationVertical
        }
        [Tooltip("Determines in what direction the movement limit is")]
        public MovementLimitationTypes movementLimitType = MovementLimitationTypes.VelocityAnyDirection;

        // Use this for initialization
        void Start()
        {
            if (grabbedObjectRigidBody) {
                // Calculates movement limit depending on what movementLimitationType is chosen
                if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical)
                {
                    speedLimit = (1 / (grabbedObjectRigidBody.mass));
                }
                else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationVertical)
                {
                    speedLimit = (50 / (grabbedObjectRigidBody.mass));
                }
            }
            
        }

        /// <summary>
        /// The ProcessFixedUpdate method is run in every FixedUpdate method on the interactable object. It applies velocity to the object to ensure it is tracking the grabbing object.
        /// </summary>
        public override void ProcessFixedUpdate()
        {
            base.ProcessFixedUpdate();

            // Calls different functions depending on what movementlimiation type has been chosen
            if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection)
            {
                speed = CalculateGeneralVelocity(lastPosition);
            }
            else if (movementLimitType == MovementLimitationTypes.VelocityVertical)
            {
                speed = CalculateVerticalVelocity(lastPosition);
            }
            else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
            {
                speed = CalculateAccelerationAny(lastPosition);
            }
            else if (movementLimitType == MovementLimitationTypes.AccelerationVertical) {
                speed = CalculateAccelerationVertical(lastPosition);
            }
            CheckMovementSpeed(speed);
            lastPosition = transform.position;
        }

        // Calculates the velocity based on objects position
        private float CalculateGeneralVelocity(Vector3 lastPos) {
            return (((transform.position - lastPos).magnitude) / Time.deltaTime);
        }

        // Calculates movement velocity in vertical
        private float CalculateVerticalVelocity(Vector3 lastPos) {
            return (Mathf.Abs(transform.position.y - lastPos.y)) / Time.deltaTime;
        }

        // Calculates the acceleration in any direction
        private float CalculateAccelerationAny(Vector3 lastPos) {
            return (CalculateGeneralVelocity(lastPos) / Time.deltaTime);
        }

        // Calculates the acceleration in vertical direction
        private float CalculateAccelerationVertical(Vector3 lastPos)
        {
            return (CalculateVerticalVelocity(lastPos) / Time.deltaTime);
        }

        // Checks if the velocity exceeds the limit
        private void CheckMovementSpeed(float speed)
        {
            Debug.Log("Thiss is the speed  " + speed);
            Debug.Log("This is the limit " + speedLimit);
            Debug.Log("This is the mass " + grabbedObjectRigidBody);
            if (speed > speedLimit)
            {
                Debug.Log("Grabb attachment says TOO FAST! Limit; " + speedLimit);
                ForceReleaseGrab();
            }
        }
    }
}