using System.Collections;
using UnityEngine;

namespace Character
{
	internal class CharacterView : MonoBehaviour
	{
		private void Awake()
		{
			characterModel = GetComponent<CharacterModel>();

			MoveCharacterAction += MoveCharacter;

			characterModel.Jump += Jump;

			characterModel.Death += Die;
		}

		private void Update()
		{
			MoveCharacterAction?.Invoke();
		}



		private void Jump()
		{
			if (characterModel.IsGrounded)
			{
				StartCoroutine(JumpCoroutine());
			}
		}

		private void MoveCharacter()
		{
			_ = characterModel.Controller.SimpleMove(characterModel.Motion);
		}

		private void Die()
		{

		}



		private IEnumerator JumpCoroutine()
		{
			characterModel.Controller.slopeLimit = 90f;

			float timeInAir = 0.0f;

			do
			{
				float jumpHeight = jumpCurve.Evaluate(timeInAir);

				Vector3 jumpVector = Vector3.up * jumpHeight * characterModel.JumpForce * Time.deltaTime;

				_ = characterModel.Controller.Move(jumpVector);

				timeInAir += Time.deltaTime;

				yield return null;
			}
			while (!characterModel.IsGrounded && characterModel.Controller.collisionFlags != CollisionFlags.Above);

			characterModel.Controller.slopeLimit = 45f;
		}



		[SerializeField] private AnimationCurve jumpCurve;
		[SerializeField] private CharacterModel characterModel;



		private System.Action MoveCharacterAction = null;
	}
}
