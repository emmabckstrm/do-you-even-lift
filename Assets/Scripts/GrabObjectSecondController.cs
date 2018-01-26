namespace VRTK.SecondaryControllerGrabActions
{
    using UnityEngine;

    public class GrabObjectSecondController : VRTK_BaseGrabAction
    {

        public override void Initialise(VRTK_InteractableObject currentGrabbdObject, VRTK_InteractGrab currentPrimaryGrabbingObject, VRTK_InteractGrab currentSecondaryGrabbingObject, Transform primaryGrabPoint, Transform secondaryGrabPoint)
        {
            base.Initialise(currentGrabbdObject, currentPrimaryGrabbingObject, currentSecondaryGrabbingObject, primaryGrabPoint, secondaryGrabPoint);
            Debug.Log("Second grab!");
        }
    }

}