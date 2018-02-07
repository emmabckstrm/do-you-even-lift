namespace VRTK
{
    using UnityEngine;
    using System.Linq;

    public class InteractableObjectTrackMovement : VRTK_InteractableObject
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

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            Rigidbody rb = interactableRigidbody;
            statManager = GameObject.Find("AppManager").GetComponent<StatManager>();
            globalControl = GameObject.Find("AppManager").GetComponent<GlobalControl>();
            if (GlobalMovementLimit) {
                movementLimitType = (MovementLimitationTypes)globalControl.movementLimitType;
            }
            // Calculates movement limit depending on what movementLimitationType is chosen
            UpdateMovementLimitValue();
            
        }

        /// <summary>
        /// The ProcessFixedUpdate method is run in every FixedUpdate method on the interactable object. It applies velocity to the object to ensure it is tracking the grabbing object.
        /// </summary>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            // Calls different functions depending on what movementlimiation type has been chosen
            // If any direction
            // Since the acceleration is dependant on the speed, we always want to calculate the speed
            if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
            {
                speed = CalculateGeneralVelocity(lastPosition);
                if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
                {
                    acceleration = CalculateAccelerationAny(speed);
                    accelerationTotal += acceleration;
                    CheckMovementSpeed(acceleration);
                }
                else {
                    speedTotal += speed;
                    CheckMovementSpeed(speed);
                }
            }
            else 
            {
                // if acceleration or speed vertically
                speed = CalculateVerticalVelocity(lastPosition);
                if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
                {
                    acceleration = CalculateAccelerationVertical(speed);
                    accelerationTotal += acceleration;
                    CheckMovementSpeed(acceleration);
                }
                else {
                    speedTotal += speed;
                    CheckMovementSpeed(speed);
                }
            }

            lastSpeed = speed;
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
            serializedData = "Thiss is the speed  " + speed;
            // Write to disk
            
            //writeString(serializedData);
            if (speed > speedLimit)
            {
                //Debug.Log("Grabb attachment says TOO FAST! Limit; " + speedLimit);
                forceRelease = true;
                numberOfForceReleases++;
                statManager.localSceneStats.totalForceReleases += 1;
                ForceReleaseGrab();
            } 
        }
        // Overridden to log start time of grab
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            timeGrabStart = Time.time;
            base.Grabbed(currentGrabbingObject);
        }
        // Overridden to log end time of grab
        public override void Ungrabbed(VRTK_InteractGrab previousGrabbingObject = null)
        {
            float timeGrabEnd = Time.time;
            timeGrabbed = timeGrabEnd - timeGrabStart;
            numberOfGrabs += 1;
            statManager.localSceneStats.timeGrabbingObj += timeGrabbed;
            statManager.localSceneStats.totalGrabs += 1;
            string hand = "";
            if (previousGrabbingObject.name.Contains("right"))
            {
                statManager.localSceneStats.totalGrabsRight += 1;
                statManager.localSceneStats.timeGrabbingRight += timeGrabbed;
                hand = "right";
            }
            else {
                statManager.localSceneStats.totalGrabsLeft += 1;
                statManager.localSceneStats.timeGrabbingLeft += timeGrabbed;
                hand = "left";
            }
            statManager.AddCSVStatPerGrab(timeGrabStart, timeGrabEnd, interactableRigidbody.mass, hand, forceRelease);
            forceRelease = false;

            base.Ungrabbed(previousGrabbingObject);
        }
        //Overridden to count number of touches to object
        public override void StartTouching(VRTK_InteractTouch currentTouchingObject)
        {
            numberOfTouches += 1;
            statManager.localSceneStats.totalTouches += 1;
            if (currentTouchingObject.name.Contains("right"))
            {
                statManager.localSceneStats.totalTouchesRight += 1;
            }
            else
            {
                statManager.localSceneStats.totalTouchesLeft += 1;
            }
            //Debug.Log("total touches to this obj: " + numberOfTouches);
            base.StartTouching(currentTouchingObject);
        }
        // Updates movement limit
        public void UpdateMovementLimitValue()
        {
            // Calculates movement limit depending on what movementLimitationType is chosen
            if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical)
            {
                // speedLimit = ((2 / (interactableRigidbody.mass))+0.1f); works good
                // speedLimit = ((1.5f / (interactableRigidbody.mass))+0.07f); // works good as well. A bit frustrating but I managed to get the first four scenes correct
                speedLimit = ((2.3f / (interactableRigidbody.mass))+0.07f);
            }
            else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationVertical)
            {
                speedLimit = (150 / (interactableRigidbody.mass));
            }
        }
        


    }
}