using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using com.ootii.Base;
using com.ootii.Cameras;
using com.ootii.Geometry;
using com.ootii.Helpers;
using com.ootii.Input;
using com.ootii.Utilities.Debug;

namespace com.ootii.AI.Controllers
{
    /// <summary>
    /// The sneak is a slow move that keeps the character facing forward.
    /// 
    /// This motion will force the camera into the third-person-fixed mode.
    /// </summary>
    [MotionTooltip("A forward motion that looks like the avatar is sneaking. The motion is slower than a walk and has the " +
                   "avatar always facing forward.")]
    public class Sneak : MotionControllerMotion
    {
        // Enum values for the motion
        public const int PHASE_UNKNOWN = 0;
        public const int PHASE_START = 600;

		protected Vector3 mLaunchForward = Vector3.zero;

		/// <summary>
		/// Keeps track the the camera mode to revert to
		/// </summary>
		private int mSavedCameraMode = EnumCameraMode.THIRD_PERSON_FOLLOW;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Sneak()
            : base()
        {
            _Priority = 100;
            mIsStartable = true;
            mIsGroundedExpected = true;
        }

        /// <summary>
        /// Controller constructor
        /// </summary>
        /// <param name="rController">Controller the motion belongs to</param>
        public Sneak(MotionController rController)
            : base(rController)
        {
            _Priority = 1;
            mIsStartable = true;
            mIsGroundedExpected = true;
        }

        /// <summary>
        /// Preprocess any animator data so the motion can use it later
        /// </summary>
        public override void LoadAnimatorData()
        {
            mController.AddAnimatorName("AnyState");

            mController.AddAnimatorName("AnyState -> Sneak-SM.SneakIdle");
            mController.AddAnimatorName("AnyState -> Sneak-SM.SneakForward");

            mController.AddAnimatorName("Sneak-SM.SneakIdle");
            mController.AddAnimatorName("Sneak-SM.SneakForward");
            mController.AddAnimatorName("Sneak-SM.SneakLeft");
            mController.AddAnimatorName("Sneak-SM.SneakRight");
            mController.AddAnimatorName("Sneak-SM.SneakBackward");
            mController.AddAnimatorName("Sneak-SM.SneakBlend -> Idle-SM.Idle_Casual");
        }

        /// <summary>
        /// Tests if this motion should be started. However, the motion
        /// isn't actually started.
        /// </summary>
        /// <returns></returns>
        public override bool TestActivate()
        {
            if (!mIsStartable) { return false; }
            if (!mController.IsGrounded) { return false; }

            // Only move in if the stance is set or it's time to move in
            if (mController.State.Stance == EnumControllerStance.SNEAK ||
//                mController.State.Stance == EnumControllerStance.TRAVERSAL && InputManager.IsJustPressed("ChangeStance"))
			    InputManager.IsJustPressed("ChangeStance") || (GlobalVariables.Instance != null && GlobalVariables.Instance.crawling))
            {
                return true;
            }
            
            // If we get here, we should not be in the stance
            return false;
        }

        /// <summary>
        /// Tests if the motion should continue. If it shouldn't, the motion
        /// is typically disabled
        /// </summary>
        /// <returns></returns>
        public override bool TestUpdate()
        {
            if (mIsActivatedFrame) { return true; }

			if(GlobalVariables.Instance != null && !GlobalVariables.Instance.crawling){
				return false;
			}

            if (!IsInSneakState) { return false; }
//            if (!mController.IsGrounded) { return false; }
            if (mController.State.Stance != EnumControllerStance.SNEAK) { return false; }
            if (InputManager.IsJustPressed("ChangeStance")) { return false; }

            return true;
        }

        /// <summary>
        /// Called to start the specific motion. If the motion
        /// were something like 'jump', this would start the jumping process
        /// </summary>
        /// <param name="rPrevMotion">Motion that this motion is taking over from</param>
        public override bool Activate(MotionControllerMotion rPrevMotion)
        {

			// Store the last camera mode and force it to a fixed view.
			// We do this to always keep the camera behind the player
			if (mController.UseInput && mController.CameraRig != null)
			{
				mSavedCameraMode = mController.CameraRig.Mode;				
				mController.CameraRig.TransitionToMode(EnumCameraMode.CRAWLING);
			}

            // Force the character's stance to change
            mController.Stance = EnumControllerStance.SNEAK;

			mLaunchForward = mController.transform.forward;
			mController.SetColliderHeightAtBase(0.5f*mController.BaseColliderHeight);
//			mController.SetColliderRadiusAtCenter(2f*mController.BaseColliderRadius);

            // Trigger the change in the animator
            mController.SetAnimatorMotionPhase(mMotionLayer.AnimatorLayerIndex, Sneak.PHASE_START, true);

            // Allow the base to finish
            return base.Activate(rPrevMotion);
        }

        /// <summary>
        /// Called to stop the motion. If the motion is stopable. Some motions
        /// like jump cannot be stopped early
        /// </summary>
        public override void Deactivate()
        {

			// Return the camera to what it was
			if (mController.CameraRig.Mode == EnumCameraMode.CRAWLING)
			{
				mController.CameraRig.TransitionToMode(mSavedCameraMode);
			}

            // If we're still flagged as in the sneak stance, move out
            if (mController.Stance == EnumControllerStance.SNEAK)
            {
                mController.Stance = EnumControllerStance.TRAVERSAL;
            }

			//TESTI
			mController.SetColliderHeightAtBase(mController.BaseColliderHeight);
			mController.SetColliderRadiusAtCenter(mController.BaseColliderRadius);

            // Deactivate
            base.Deactivate();
        }

        /// <summary>
        /// Updates the motion over time. This is called by the controller
        /// every update cycle so animations and stages can be updated.
        /// </summary>
        public override void UpdateMotion()
        {
            if (!TestUpdate())
            {
                Deactivate();
                return;
            }

            
            

			//TESTI
//			mController.SetColliderHeightAtCenter(0.8f);
//			mController.SetColliderHeightAtBase(0.5f);

            // Trend data allows us to wait for the speed to peak or bottom-out before we send it to
            // the animator. This is important for pivots that need to be very precise.
            mUseTrendData = true;

			DetermineVelocity();
			DetermineAngularVelocity();

        }

		/// <summary>
		/// Returns the current velocity of the motion
		/// </summary>
		protected override Vector3 DetermineVelocity()
		{
			ControllerState lState = mController.State;
			

			// If were in the midst of jumping, we want to add velocity based on 
			// the magnitude of the controller. However, we don't add it we're heading back to idle

				Vector3 lBaseForward = mController.CameraTransform.forward;
				if (!mController.UseInput) { lBaseForward = mController.transform.forward; }
				
				// Direction of the camera
				Vector3 lCameraForward = lBaseForward;
				lCameraForward.y = 0f;
				lCameraForward.Normalize();
				
				// Create a quaternion that gets us from our world-forward to our camera direction.
				// FromToRotation creates a quaternion using the shortest method which can sometimes
				// flip the angle. LookRotation will attempt to keep the "up" direction "up".
				Quaternion lFromCamera = Quaternion.LookRotation(lCameraForward);
				
				// Determine the avatar displacement direction. This isn't just
				// normal movement forward, but includes movement to the side
				Vector3 lMoveDirection = lFromCamera * lState.InputForward;
				
				// Allow the player to create an initial launch velocity if there isn't one
				//if (!mControlEnabled && lState.InputMagnitudeTrend.Value != 0f && lState.GroundLaunchVelocity.magnitude == 0f)
				//{
				//    lState.GroundLaunchVelocity = lFromCamera * (lBaseForward * mMovementSpeed);
				//}
				
				// Determine the max air speed
//				Vector3 lMomentum = lState.GroundLaunchVelocity;
				
				float mMovementSpeed = 1f;

				// Determine the air speed. We want the max of the momentum or control
				// speed. This gives us smooth movement while running and jumping
				float lControlSpeed =  mMovementSpeed * lState.InputMagnitudeTrend.Value;
				
//				float lAirSpeed = lMomentum.magnitude * lState.InputMagnitudeTrend.Value;
//				lAirSpeed = Mathf.Max(lAirSpeed, lControlSpeed);
				
				// Combine our control velocity with momentum
//				Vector3 lAirVelocity = Vector3.zero;
				
//				lAirVelocity += lMoveDirection * lAirSpeed;
				
				// Return the final velocity
//				mVelocity = lAirVelocity;
				mVelocity = lMoveDirection;

			
			return mVelocity;
		}
		
		/// <summary>
		/// Returns the current angular velocity of the motion
		/// </summary>
		protected override Vector3 DetermineAngularVelocity()
		{
//			mAngularVelocity = Vector3.zero;

			ControllerState lState = mController.State;
			
			// Clear the rotation value
			mAngularVelocity = Vector3.zero;
			
			float mRotationMin = 0.1f;
			float mRotationSpeed = 90f;

			Quaternion lBaseRotation = mController.CameraTransform.rotation;
				
			if (!mController.UseInput) { lBaseRotation = mController.transform.rotation; }
					
			// Determine the direction of the input relative to the camera
			Vector3 lInputDirection = lBaseRotation * lState.InputForward;
						
			// Check how much we're rotating vs. the original launch forward. We'll only 
			// rotate if we're changing direction drastically.
			float lDeltaAngle = Mathf.Abs(NumberHelper.GetHorizontalAngle(mLaunchForward, lInputDirection));
			if (mRotationMin > 0 && (lDeltaAngle > mRotationMin || Mathf.Abs(lState.InputFromCameraAngle) < 5f))
			{
				//mAngularVelocity.y = lState.InputFromAvatarAngle * mRotationSpeed;
				mAngularVelocity.y = (lState.InputFromAvatarAngle / 90f) * mRotationSpeed;
			}

			return mAngularVelocity;
		}
		
		/// <summary>
		/// Allows the motion to modify the velocity before it is applied.
		/// </summary>
		/// <returns></returns>
		public override void CleanRootMotion(ref Vector3 rVelocityDelta, ref Quaternion rRotationDelta)
        {
            rRotationDelta = Quaternion.identity;
        }

        /// <summary>
        /// Raised when the animator's state has changed
        /// </summary>
        /// <param name="rLastStateID">State the animator is leaving</param>
        /// <param name="rNewStateID">State the animator is now at</param>
        public override void OnAnimatorStateChange(int rLastStateID, int rNewStateID)
        {
        }

        /// <summary>
        /// Test to see if we're currently in the state
        /// </summary>
        public bool IsInSneakState
        {
            get
            {
                string lState = mController.GetAnimatorStateName(mAnimatorLayerIndex);
                string lTransition = mController.GetAnimatorStateTransitionName(mAnimatorLayerIndex);

                if (lTransition == "AnyState -> Sneak-SM.SneakIdle" ||
                    lTransition == "AnyState -> Sneak-SM.SneakForward" ||
                    lState == "Sneak-SM.SneakIdle" ||
                    lState == "Sneak-SM.SneakForward" ||
                    lState == "Sneak-SM.SneakLeft" ||
                    lState == "Sneak-SM.SneakRight" ||
                    lState == "Sneak-SM.SneakBackward"
                    )
                {
                    return true;
                }

                return false;
            }
        }
    }
}
