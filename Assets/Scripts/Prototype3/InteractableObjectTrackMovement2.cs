namespace VRTK
{
    using UnityEngine;
    using System.Linq;

    public class InteractableObjectTrackMovement2 : InteractableObjectCustom
    {
        // calculates movement every x frame

        protected Vector3 startPosition;
        public float safeZoneDistance = 0f;
        public bool globalDangerZoneWarning = true;
        public bool warnInDangerZone = true;
        protected float speedDangerZone = 0.15f;
        protected bool permanentGrab = false;
        protected HingeJoint hingeJoint;
        protected JointSpring jointSpring;
        protected float timeStep;
        protected int numFrames = 40;
        protected int currentFrame = 0;
        protected float[] movementArray;
        protected float[] movementArrayTemp;
        protected float avgMovement = 0f;
        [Tooltip("Number of frames that will be skipped each time velocity or acceleration is calculated")]
        protected int skipFrames = 4;
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
            if (globalDangerZoneWarning) {
              warnInDangerZone = globalControl.warnInDangerZone;
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
                    HandleCurrentMovment(speed);
                  } else {
                    HandleCurrentMovment(acceleration);
                  }
                  // Debug.Log("frame " + currentFrame);
                  //avgMovement = CalculateAverageMovement();
                  //CheckMovementSpeed(avgMovement);
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
          for (int i=0; i<numFrames; i=i+skipFrames) {
            total += movementArray[i];
            Debug.Log("movementcalc " + i + " - " + movementArray[i]);
            j++;
          }
          float averageMovement = total / j;
          Debug.Log("averageMovement "+ averageMovement);
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
              //UpdateMovementArray(acceleration);
          }
          else {
              speedTotal += speed;
              //Debug.Log("velocity " + speed);
              //UpdateMovementArray(speed);
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
              //UpdateMovementArray(acceleration);
          }
          else {
              speedTotal += speed;
              //Debug.Log("velocity " + speed);
              //UpdateMovementArray(speed);
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
        protected override void CheckMovementSpeed(float speed) {
          if (speed > speedLimit)
          {
              //Debug.Log(" *************** Too fast! speed limit " + speedLimit + " speed " + speed + " angular drag " + interactableRigidbody.angularDrag);
              ForceReleaseGrab();
          } else if (warnInDangerZone && speed > speedLimit-speedDangerZone && !shakeScript.IsShaking()) {
            //Debug.Log(" ******* ALMOST Too fast! danger zone speed limit " + speedLimit + " speed " + speed);
            shakeScript.EnableShake();
          } else if (warnInDangerZone && speed <= speedLimit-speedDangerZone && shakeScript.IsShaking()){
            shakeScript.DisableShake();
          }
        }
        protected void CheckDistanceFromOrigin() {
          Vector3 distance = transform.position - startPosition;
          if (distance.y > 0.5f && Mathf.Abs(distance.x) < 0.1f && Mathf.Abs(distance.z) < 0.1f) {
            permanentGrab = true;
          }
        }
        protected void HandleCurrentMovment(float movement) {
          //UpdateMovementArray(movement);
          // check speed here when not using average movement
          CheckDistanceFromOrigin();
          if (!permanentGrab) {
            CheckMovementSpeed(movement);
          }
        }
        // Updates movement limit
        public override void UpdateMovementLimitValue()
        {
          // Calculates movement limit depending on what movementLimitationType is chosen
          if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical)
          {
            if (interactableRigidbody.mass > 9.83f) {
              speedLimit = (1.9f / interactableRigidbody.mass + 0.06f);
            } else if (interactableRigidbody.mass > 2.36f) {
              // speedLimit = ((2 / (interactableRigidbody.mass))+0.1f); works good
               //speedLimit = ((1.5f / (interactableRigidbody.mass))+0.07f); // works good as well. A bit frustrating but I managed to get the first four scenes correct
              //speedLimit = ((2.3f / (interactableRigidbody.mass))+0.07f); // used for user study1
              //speedLimit = (4/(interactableRigidbody.mass+0.45f)-0.1f); //wip
              // 4/(x+0.9)-0.2 //wip
              //speedLimit = (1.9f / interactableRigidbody.mass + 0.06f); // prototype 3A
              //speedLimit = (-0.08f * interactableRigidbody.mass + 1.1f); // combining linear
              speedLimit = (-0.08f * interactableRigidbody.mass + 1.05f); // combining linear
            } else {
              speedLimit = (-0.8f * interactableRigidbody.mass + 2.75f); // prototype 3A or cobining linear
              //speedLimit = (-1.8f * interactableRigidbody.mass + 3.6f);
            }

          }
          else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationVertical)
          {
              // speedLimit = ((150 / (interactableRigidbody.mass+5))+0.2f);
              speedLimit = (30f / (interactableRigidbody.mass+5));
          }
        }
        protected override void ForceReleaseGrab()
        {
            GameObject grabbingObject = GetGrabbingObject();
            if (grabbingObject != null)
            {
                timeForceRelease = Time.time;
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
            if (shakeScript != null) {
                shakeScript.DisableShake();
            }
            numberOfGrabs += 1;
            statManager.localSceneStats.timeGrabbingObj += timeGrabbed;
            statManager.localSceneStats.totalGrabs += 1;
            string hand = "";
            permanentGrab = false;
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

    }
}
