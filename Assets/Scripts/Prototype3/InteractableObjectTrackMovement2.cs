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
        protected float timeStep;
        protected int numFrames = 30;
        protected int currentFrame = 0;
        protected float[] movementArray;
        protected float[] movementArrayTemp;
        protected float avgMovement = 0f;
        [Tooltip("Number of frames that will be skipped each time velocity or acceleration is calculated")]
        protected int skipFrames = 3;
        protected float lastFrameTime = 0f;
        protected Shake shakeScript;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            shakeScript = GetComponent<Shake>();
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
            UpdateAngularDrag();

        }
        /// <summary>
        /// The ProcessFixedUpdate method is run in every FixedUpdate method on the interactable object. It applies velocity to the object to ensure it is tracking the grabbing object.
        /// </summary>
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsGrabbed()) {
              if (currentFrame%skipFrames == 0) {
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
                  //vleocity or acc
                  if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical) {
                    UpdateMovementArray(speed);
                  } else {
                    UpdateMovementArray(acceleration);
                  }
                  //Debug.Log("frame " + currentFrame);
                  avgMovement = CalculateAverageMovement();
                  CheckMovementSpeed(avgMovement);
                  // always velocity
                  lastSpeed = speed;
                  lastPosition = transform.position;
                  lastFrameTime = Time.time;
                }
              }
              currentFrame++;
              if (currentFrame == numFrames) {
                currentFrame = 0;
              }
            }

        }
        //Updates speed array
        protected void UpdateMovementArray(float movement) {
          movementArray[currentFrame] = movement;
        }
        protected float CalculateAverageMovement() {
          float total = 0f;
          int j = 0;
          for (int i=0; i<currentFrame+1; i=i+skipFrames) {
            total += movementArray[i];
            j++;
          }
          float averageMovement = total / j;
          //Debug.Log("averageMovement "+ averageMovement);
          return averageMovement;
        }
        // Calculates the speed or acceleration in any direction
        protected void CalculateMovementAnyDirection() {
          speed = CalculateGeneralVelocity(lastPosition);
          //Debug.Log("general Velocity and last velocity " + speed + " " + lastSpeed);
          if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection)
          {
              acceleration = CalculateAccelerationAny(speed);
              //Debug.Log("acceleration " + acceleration);
              accelerationTotal += acceleration;
              UpdateMovementArray(acceleration);
          }
          else {
              speedTotal += speed;
              //Debug.Log("velocity " + speed);
              UpdateMovementArray(speed);
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
              UpdateMovementArray(acceleration);
          }
          else {
              speedTotal += speed;
              Debug.Log("velocity " + speed);
              UpdateMovementArray(speed);
          }
        }
        // Calculates the velocity based on objects position
        protected override float CalculateGeneralVelocity(Vector3 lastPos)
        {
            //Debug.Log("general velocity " + (((transform.position - lastPos).magnitude) / (Time.time - lastFrameTime)));
            return (((transform.position - lastPos).magnitude) / (Time.time - lastFrameTime));
        }
        // Calculates movement velocity in vertical
        protected override float CalculateVerticalVelocity(Vector3 lastPos)
        {
            return (Mathf.Abs(transform.position.y - lastPos.y)) / (Time.time - lastFrameTime);
        }
        // Calculates the acceleration in any direction
        protected override float CalculateAccelerationAny(float speed)
        {
            //Debug.Log("velocty and lastSpeed " + speed + " " + lastSpeed);
            //Debug.Log("time " + (Time.time - lastFrameTime));
            return ((Mathf.Abs(speed - lastSpeed)) / (Time.time - lastFrameTime));
        }
        // Calculates the acceleration in vertical direction
        protected override float CalculateAccelerationVertical(float speed)
        {
            return ((Mathf.Abs(speed-lastSpeed)) / (Time.time - lastFrameTime));
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
            //Debug.Log("----------------- grab start ---------------");
            //Debug.Log("movement limiation type " + movementLimitType);
            timeGrabStart = Time.time;
            startPosition = transform.position;
            currentFrame = 0;
            movementArray = Enumerable.Repeat(0f, numFrames).ToArray();
            movementArrayTemp = Enumerable.Repeat(0f, numFrames).ToArray();
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
                // speedLimit = ((150 / (interactableRigidbody.mass+5))+0.2f);
                speedLimit = (20f / (interactableRigidbody.mass+5));
            }
        }
        public override void UpdateAngularDrag() {
          interactableRigidbody.angularDrag = (0.1f * (interactableRigidbody.mass*interactableRigidbody.mass));
        }
    }
}
