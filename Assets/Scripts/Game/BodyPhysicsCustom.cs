// Body Physics|Presence|70060
namespace VRTK
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

	public class BodyPhysicsCustom : VRTK_BodyPhysics {


		protected override void StartFall(GameObject targetFloor)
        {
            if (IsLeaning())
            {
                OnStopLeaning(SetBodyPhysicsEvent(null, null));
            }
            if (OnGround())
            {
                OnStopTouchingGround(SetBodyPhysicsEvent(null, null));
            }
            isFalling = true;
            isMoving = false;
            isLeaning = false;
            onGround = false;
						Debug.Log("Fall start");
            fallMinTime = Time.time + (Time.fixedDeltaTime * 3.0f); // Wait at least 3 fixed update frames before declaring falling finished
            OnStartFalling(SetBodyPhysicsEvent(targetFloor, null));
        }

        protected override void StopFall()
        {
            bool wasFalling = isFalling;
            if (!OnGround())
            {
                OnStartTouchingGround(SetBodyPhysicsEvent(currentValidFloorObject, null));
            }
            isFalling = false;
            onGround = true;
            enableBodyCollisions = enableBodyCollisionsStartingValue;
						Debug.Log("Fall end. Body collisions status: " + enableBodyCollisions);
            if (wasFalling)
            {
                OnStopFalling(SetBodyPhysicsEvent(null, null));
            }
        }

        protected override void GravityFall(RaycastHit rayCollidedWith)
        {
            StartFall(rayCollidedWith.collider.gameObject);
            TogglePhysics(true);
            ApplyBodyVelocity(Vector3.zero);
        }
	}
}
