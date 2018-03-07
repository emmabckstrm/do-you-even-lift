namespace VRTK
{
    using UnityEngine;
    using System.Linq;

    public class InteractableObjectTrackMovement2 : InteractableObjectCustom
    {

        protected Vector3 startPosition;
        public float safeZoneDistance = 0f;
        protected HingeJoint hingeJoint;
        protected JointSpring jointSpring;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            safeZoneDistance = 0.067f;
            Rigidbody rb = interactableRigidbody;
            // modifies the hingeJoint
            hingeJoint = GetComponent<HingeJoint>();
            if (hingeJoint != null) {
              hingeJoint.useSpring = true;
              jointSpring = hingeJoint.spring;
              float newDamper = interactableRigidbody.mass / 100;
              jointSpring.damper = newDamper;
            }

            // gets other scripts
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
            if (safeZoneDistance == 0 || (transform.position - startPosition).magnitude > safeZoneDistance ) {
              // Calls different functions depending on what movementlimiation type has been chosen
              // If any direction
              // Since the acceleration is dependant on the speed, we always want to calculate the speed
              if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
              {
                  CalculateMovementAnyDirection();
              }
              else
              {
                  CalculateMovementVertical();
              }

              lastSpeed = speed;
              lastPosition = transform.position;
            }
        }
        // Calculates the speed or acceleration in any direction
        protected void CalculateMovementAnyDirection() {
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
        // Calculates the velocity or acceleration vertically
        protected void CalculateMovementVertical() {
          // if acceleration or speed vertically
          speed = CalculateVerticalVelocity(lastPosition);
          if (movementLimitType == MovementLimitationTypes.AccelerationVertical)
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
        // Calculates the acceleration in any direction
        protected override float CalculateAccelerationAny(float speed)
        {
            return ((Mathf.Abs(speed - lastSpeed)) / Time.deltaTime);
        }
        // Calculates the acceleration in vertical direction
        protected override float CalculateAccelerationVertical(float speed)
        {
            return ((Mathf.Abs(speed-lastSpeed)) / Time.deltaTime);
        }
        protected override void ForceReleaseGrab()
        {
            GameObject grabbingObject = GetGrabbingObject();
            if (grabbingObject != null)
            {
                grabbingObject.GetComponent<VRTK_InteractGrab>().ForceRelease();
                forceRelease = true;
                numberOfForceReleases++;
                statManager.localSceneStats.totalForceReleases += 1;
            }
        }

        // Overridden to log start time of grab
        public override void Grabbed(VRTK_InteractGrab currentGrabbingObject = null)
        {
            timeGrabStart = Time.time;
            startPosition = transform.position;
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

        // Updates movement limit
        public override void UpdateMovementLimitValue()
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
