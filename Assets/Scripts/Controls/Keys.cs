using UnityEngine;

namespace Controls
{
	internal static class Keys
	{
		internal static void SetNewKey(Action action, KeyCode key)
		{
			action.SetNewKey(key);
		}

		internal static KeyCode MoveForward => moveForward.Key;
		internal static KeyCode MoveBackward => moveBackward.Key;

		internal static KeyCode MoveLeft => moveLeft.Key;
		internal static KeyCode MoveRight => moveRight.Key;

		internal static KeyCode Attack => attack.Key;
		internal static KeyCode Aim => aim.Key;
		internal static KeyCode Reload => reload.Key;

		internal static KeyCode Run => run.Key;
		internal static KeyCode Walk => walk.Key;
		internal static KeyCode Jump => jump.Key;

		internal static KeyCode LeanRight => leanRight.Key;
		internal static KeyCode LeanLeft => leanLeft.Key;

		internal static KeyCode Submit => submit.Key;
		internal static KeyCode Decline => decline.Key;

		internal static KeyCode CameraModeToggle => cameraModeToggle.Key;



		private static readonly Action moveForward = new Action("Move Forward", KeyCode.W);
		private static readonly Action moveBackward = new Action("Move Backward", KeyCode.S);

		private static readonly Action moveLeft = new Action("Move Left", KeyCode.D);
		private static readonly Action moveRight = new Action("Move Right", KeyCode.A);

		private static readonly Action attack = new Action("Attack", KeyCode.Mouse0);
		private static readonly Action aim = new Action("Aim", KeyCode.Mouse1);
		private static readonly Action reload = new Action("Reload", KeyCode.R);

		private static readonly Action run = new Action("Run", KeyCode.LeftAlt);
		private static readonly Action walk = new Action("Walk", KeyCode.LeftShift);
		private static readonly Action jump = new Action("Jump", KeyCode.Space);

		private static readonly Action leanRight = new Action("Lean Right", KeyCode.E);
		private static readonly Action leanLeft = new Action("Lean Left", KeyCode.Q);

		private static readonly Action submit = new Action("Submit", KeyCode.Return);
		private static readonly Action decline = new Action("Decline", KeyCode.Escape);

		private static readonly Action cameraModeToggle = new Action("Camera-mode Toggle", KeyCode.Alpha5);
	}
}