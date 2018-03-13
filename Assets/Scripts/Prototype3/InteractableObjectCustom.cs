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
      [Header("Movement limitation", order = 4)]
      [Tooltip("Determines in what direction the movement limit is")]
      public MovementLimitationTypes movementLimitType = MovementLimitationTypes.VelocityAnyDirection;

        [Tooltip("If checked, the global movement limitation type overrides the one chosen for this object")]
        public bool GlobalMovementLimit = true;
        protected float speedLimit = 10f;
        protected float speedTotal = 0f;
        protected float accelerationTotal = 0f;
        protected float acceleration = 0f;
        protected float speed = 0f;
        protected float lastSpeed = 0f;
        protected Vector3 lastPosition = Vector3.zero;
        protected string serializedData;
        //static protected StreamWriter writer = new StreamWriter("MyPath.txt", true);
        protected float timeGrabStart = 0.0f;
        protected float timeGrabbed = 0.0f;
        protected int numberOfGrabs = 0;
        protected int numberOfTouches = 0;
        protected int numberOfForceReleases = 0;
        protected bool forceRelease = false;
        protected StatManager statManager;
        protected GlobalControl globalControl;

        // Calculates the velocity based on objects position
        protected virtual float CalculateGeneralVelocity(Vector3 lastPos)
        {
            Debug.Log(((transform.position - lastPos).magnitude) / Time.deltaTime);
            return (((transform.position - lastPos).magnitude) / Time.deltaTime);
        }
        // Calculates movement velocity in vertical
        protected virtual float CalculateVerticalVelocity(Vector3 lastPos)
        {
            return (Mathf.Abs(transform.position.y - lastPos.y)) / Time.deltaTime;
        }
        // Calculates the acceleration in any direction
        protected virtual float CalculateAccelerationAny(float speed)
        {
            return ((Mathf.Abs(speed - lastSpeed)) / Time.deltaTime);
        }
        // Calculates the acceleration in vertical direction
        protected virtual float CalculateAccelerationVertical(float speed)
        {
            return ((Mathf.Abs(speed-lastSpeed)) / Time.deltaTime);
        }

        // Checks if the velocity exceeds the limit
        protected virtual void CheckMovementSpeed(float speed)
        {
            if (speed > speedLimit)
            {
                Debug.Log(" *************** Too fast! speed limit " + speedLimit + " speed " + speed + " angular drag " + interactableRigidbody.angularDrag);
                ForceReleaseGrab();
            }
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
        public virtual void UpdateMovementLimitValue()
        {
          // Calculates movement limit depending on what movementLimitationType is chosen
          if (movementLimitType == MovementLimitationTypes.VelocityAnyDirection || movementLimitType == MovementLimitationTypes.VelocityVertical)
          {
            if (interactableRigidbody.mass > 1) {
              // speedLimit = ((2 / (interactableRigidbody.mass))+0.1f); works good
               //speedLimit = ((1.5f / (interactableRigidbody.mass))+0.07f); // works good as well. A bit frustrating but I managed to get the first four scenes correct
              //speedLimit = ((2.3f / (interactableRigidbody.mass))+0.07f); // used for user study1
              //speedLimit = (4/(interactableRigidbody.mass+0.45f)-0.1f); //wip
              // 4/(x+0.9)-0.2 //wip
              speedLimit = (1.9f / interactableRigidbody.mass + 0.06f);
            } else {
               speedLimit = (-0.8f * interactableRigidbody.mass + 2.75f);
              //speedLimit = (-1.8f * interactableRigidbody.mass + 3.6f);
            }

          }
          else if (movementLimitType == MovementLimitationTypes.AccelerationAnyDirection || movementLimitType == MovementLimitationTypes.AccelerationVertical)
          {
              // speedLimit = ((150 / (interactableRigidbody.mass+5))+0.2f);
              speedLimit = (30f / (interactableRigidbody.mass+5));
          }
        }
        public virtual void UpdateAngularDrag() {
          interactableRigidbody.angularDrag = (0.1f * (interactableRigidbody.mass*interactableRigidbody.mass*(interactableRigidbody.mass*0.5f)));
        }



    }
}
