// Position Rewind|Presence|70070
namespace VRTK
{
    using UnityEngine;

	public class PositionRewind : VRTK_PositionRewind {

		protected Vector3 originalStandingPosition;
		protected Vector3 originalHeadsetPosition;

			protected override void OnEnable() {
				base.OnEnable();
				SetOriginalPosition();
			}

				/// <summary>
		    /// The SetLastGoodPosition method stores the current valid play area and headset position.
		    /// </summary>
		    public virtual void SetOriginalPosition()
		    {
		        if (playArea != null && headset != null)
		        {
		            lastGoodPositionSet = true;
		            originalStandingPosition = playArea.position;
		            originalHeadsetPosition = headset.position;
		        }
		    }

				/// <summary>
        /// The RewindPosition method resets the play area position to the last known good position of the play area.
        /// </summary>
        public virtual void RewindPositionToOriginal()
        {
            if (headset != null)
            {
                Vector3 storedPosition = playArea.position;
                Vector3 resetVector = originalHeadsetPosition - headset.position;
                Vector3 moveOffset = resetVector.normalized * pushbackDistance;
                playArea.position = originalHeadsetPosition;
                if (bodyPhysics != null)
                {
                    bodyPhysics.ResetVelocities();
                }
                OnPositionRewindToSafe(SetEventPayload(storedPosition));
            }
        }
	}
}
