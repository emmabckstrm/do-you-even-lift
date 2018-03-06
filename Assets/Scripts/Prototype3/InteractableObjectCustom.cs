namespace VRTK
{
    using UnityEngine;
    using System.Linq;

    public class InteractableObjectCustom : VRTK_InteractableObject
    {
        // Different types of movement limitations
        public enum MovementLimitationTypes
        {
            VelocityAnyDirection,
            VelocityVertical,
            AccelerationAnyDirection,
            AccelerationVertical,
        }
        [Tooltip("Determines in what direction the movement limit is")]
        public MovementLimitationTypes movementLimitType = MovementLimitationTypes.VelocityAnyDirection;
        [Tooltip("If checked, the global movement limitation type overrides the one chosen for this object")]
        public bool GlobalMovementLimit = true;
        protected float speedLimit = 10f;
        private float speedTotal = 0f;
        private float accelerationTotal = 0f;
        private float acceleration = 0f;
        private float speed = 0f;
        private float lastSpeed = 0f;
        private Vector3 lastPosition = Vector3.zero;
        protected string serializedData;
        //static protected StreamWriter writer = new StreamWriter("MyPath.txt", true);
        private float timeGrabStart = 0.0f;
        private float timeGrabbed = 0.0f;
        private int numberOfGrabs = 0;
        private int numberOfTouches = 0;
        private int numberOfForceReleases = 0;
        private bool forceRelease = false;
        protected StatManager statManager;
        protected GlobalControl globalControl;


        // Calculates the velocity based on objects position
        private float CalculateGeneralVelocity(Vector3 lastPos)
        {
            return (((transform.position - lastPos).magnitude) / Time.deltaTime);
        }
        // Calculates movement velocity in vertical
        private float CalculateVerticalVelocity(Vector3 lastPos)
        {
            return (Mathf.Abs(transform.position.y - lastPos.y)) / Time.deltaTime;
        }
        // Calculates the acceleration in any direction
        private float CalculateAccelerationAny(float speed)
        {
            return ((Mathf.Abs(speed - lastSpeed)) / Time.deltaTime);
        }
        // Calculates the acceleration in vertical direction
        private float CalculateAccelerationVertical(float speed)
        {
            return ((Mathf.Abs(speed-lastSpeed)) / Time.deltaTime);
        }

        // Checks if the velocity exceeds the limit
        private void CheckMovementSpeed(float speed)
        {

        }
        // Overridden to log start time of grab


        // Updates movement limit
        public void UpdateMovementLimitValue()
        {

        }



    }
}
