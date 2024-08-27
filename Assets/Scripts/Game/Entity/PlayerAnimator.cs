using UnityEngine;

namespace Dogabeey
{
    public class PlayerAnimator : EntityAnimator
    {
		public PlayerJoystickController joystickController;
        public string rotationText = "rotation_movement_difference";

        public override void AnimationController()
        {
            base.AnimationController();

			// Find the rotation difference between player's rotation and the joystick's rotation.
			float angle = Vector3.SignedAngle(transform.forward, joystickController.direction, Vector3.up);

			// Set the angle between -1 and 1.
			float value = angle / 180f;

			animator.SetFloat(rotationText, value);
        }
    }
}