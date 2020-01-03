using UnityEngine;

namespace Character
{
	///	<summary>
	///	Отвечает за хранение характеристик персонажа.
	/// </summary>
	internal class CharacterModel : MonoBehaviour
	{
		public event System.Action Death;
		public event System.Action Jump;
		public event System.Action DamageReceived;
		public event System.Action HealReceived;
		public event System.Action HealthChanged;



		public bool IsGrounded => GroundedCheck();



		public float JumpForce => jumpForce;



		public uint HealthPoints => HP;



		public CharacterController Controller => controller;



		public Vector3 Motion { get; private set; } = Vector3.zero;



		public void ChangeHP(int deltaHP)
		{
			int newHP = (int)HP;
			newHP += deltaHP;

			if (newHP > HP)
			{
				HealReceived?.Invoke();
			}
			else if (newHP < HP)
			{
				DamageReceived?.Invoke();
			}
			else if (newHP <= 0)
			{
				Death?.Invoke();
			}

			HP = (uint)newHP;

			HealthChanged?.Invoke();

		}
		
		/// <summary>
		/// Задаёт новый вектор движения персонажа.
		/// </summary>
		/// <param name="deltaX">Сдвиг вдоль оси "X".</param>
		/// <param name="deltaZ">Сдвиг вдоль оси "Z".</param>
		/// <param name="motionMode">Режим движения (шаг, бег, спринт и т.д.).</param>
		public void SetNewMotionDirection(float deltaX, float deltaZ, CharacterMotionMode motionMode)
		{
			switch (motionMode)
			{
				case CharacterMotionMode.Walk:
					currentSpeed = walkSpeed;
					break;
				case CharacterMotionMode.Run:
					currentSpeed = runSpeed;
					break;
				case CharacterMotionMode.Sprint:
					currentSpeed = sprintSpeed;
					break;
			}

			deltaX *= currentSpeed;
			deltaZ *= currentSpeed;

			Motion = new Vector3(deltaX, gravity, deltaZ);

			Motion = Vector3.ClampMagnitude(Motion, currentSpeed);

			Motion = transform.TransformDirection(Motion);
		}

		public void InvokeJump()
		{
			Jump?.Invoke();
		}


		/// <summary>
		/// Проверяет, стоит ли персонаж на твёрдой поверхности.
		/// </summary>
		/// <returns>True - если на расстоянии 0.1 или меньше под персонажем находится твёрдая поверхность.</returns>
		private bool GroundedCheck()
		{
			_ = Physics.SphereCast(controller.bounds.center, 0.5f, Vector3.down, out RaycastHit hitInfo);

			return hitInfo.distance <= .6f;
		}



		[Header("HP")]
		[SerializeField] private uint HP = 100;
		[SerializeField] private uint maxHP = 100;

		[Header("Speed")]
		[SerializeField] private float walkSpeed;
		[SerializeField] private float runSpeed;
		[SerializeField] private float sprintSpeed;
		[SerializeField] private float currentSpeed;

		[SerializeField] private float jumpForce;
		[SerializeField] private float gravity = -9.87f;

		[SerializeField] private CharacterController controller;
	}

	internal enum CharacterMotionMode : byte
	{
		Walk,
		Run,
		Sprint
	}
}