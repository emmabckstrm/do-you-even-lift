namespace VRTK
{
    using UnityEngine;
    using System.IO;

    public class InteractableObjectTrackMovement : VRTK_InteractableObject
    {
        // Different types of movement limitations
        public enum MovementLimitationTypes
        {
            VelocityAnyDirection,
            VelocityVertical,
            AccelerationAnyDirection,
            AccelerationVertical
        }
        [Tooltip("Determines in what direction the movement limit is")]
        public MovementLimitationTypes movementLimitType = MovementLimitationTypes.VelocityAnyDirection;
        protected float speedLimit = 10f;
        private float speed = 0f;
        private float lastSpeed = 0f;
        private Vector3 lastPosition = Vector3.zero;
        protected string serializedData;
        protected StreamWriter writer;
        private float timestamp = 0.0f;
        private float timeGrabbed = 0.0f;
        private int numberOfGrabs = 0;
        private int numberOfTouches = 0;

        // Use this for initialization
        protected void Start()
        {
            //writer = new StreamWriter("MyPath.txt", true);
            Rigidbody rb = interactableRigidbody;
            // Calculates movement limit depending on what movementLimitationType is chosen
            if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical)
            {
                speedLimit = (3 / (interactableRigidbody.mass));
            }
            else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationVertical)
            {
                speedLimit = (150 / (interactableRigidbody.mass));
            }
        }

        /// <summary>
        /// The ProcessFixedUpdate method is run in every FixedUpdate method on the interactable object. It applies velocity to the object to ensure it is tracking the grabbing object.
        /// </summary>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

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
            else if (movementLimitType == MovementLimitationTypes.AccelerationVertical)
            {
                speed = CalculateAccelerationVertical(lastPosition);
            }
            CheckMovementSpeed(speed);
            lastPosition = transform.position;
        }

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
        private float CalculateAccelerationAny(Vector3 lastPos)
        {
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
            serializedData = "Thiss is the speed  " + speed;
            //Debug.Log("This is the limit " + speedLimit);
            //Debug.Log("This is the mass " + interactableRigidbody.mass);
            // Write to disk
            
            //writeString(serializedData);
            if (speed > speedLimit)
            {
                //Debug.Log("Grabb attachment says TOO FAST! Limit; " + speedLimit);
                ForceReleaseGrab();
            }
        }

        // Overridden to log start time of grab
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            Debug.Log("Yey, it was grabbed! It's grabbed by " + currentGrabbingObject.name);
            timestamp = Time.time;
            base.Grabbed(currentGrabbingObject);
        }

        // Overridden to log end time of grab
        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
        { 
            timeGrabbed = Time.time - timestamp;
            numberOfGrabs += 1;
            Debug.Log("Ungrabbed! " + previousGrabbingObject.name + " after this time " + timeGrabbed);
            Debug.Log(this.name + " Total grabs " + numberOfGrabs);
            base.Ungrabbed(previousGrabbingObject);
        }

        public override void StartTouching(VRTK_InteractTouch currentTouchingObject)
        {
            numberOfTouches += 1;
            Debug.Log("total touches to this obj: " + numberOfTouches);
            base.StartTouching(currentTouchingObject);
        }

        static void writeString(string str)
        {
            string path = "MyPath.txt";

            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            //writer.WriteLine("TEST");
            //writer.Close();
        }


    }
}