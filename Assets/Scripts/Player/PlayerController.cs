using Controls;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Контроллер ввода, отвечающий за передвижение персонажа, к которому прикреплён.
	/// </summary>
	internal class PlayerController : MonoBehaviour, Character.ICharacterController
	{
		#region Public class methods

		public void ReceiveDamage(uint dmg)
		{
			characterModel.ChangeHP(-(int)dmg);
		}

		public void ReceiveHeal(uint heal)
		{
			characterModel.ChangeHP((int)heal);
		}

		#endregion

		#region Private Unity messages

		private void Awake()
		{
			characterModel = GetComponent<Character.CharacterModel>();

			KeyboardInputHandler += MotionInput;
			KeyboardInputHandler += MenuInput;
			MouseInputHandler = RotationInput;
		}

		private void Update()
		{
			MouseInputHandler?.Invoke();
			KeyboardInputHandler?.Invoke();
		}

		#endregion

		#region Private class methods

		/// <summary>
		/// Перехват и обработка ввода с клавиатуры, отвечающего за перемещение.
		/// </summary>
		private void MotionInput()
		{
			float deltaX = 0; // Смещение вдоль оси X
			float deltaZ = 0; // Смещение вдоль оси Z

			if (Input.GetKey(Keys.MoveForward))
			{
				deltaZ = FORWARD;
			}
			else if (Input.GetKey(Keys.MoveBackward))
			{
				deltaZ = BACKWARD;
			}

			if (Input.GetKey(Keys.MoveLeft))
			{
				deltaX = LEFT;
			}
			else if (Input.GetKey(Keys.MoveRight))
			{
				deltaX = RIGHT;
			}

			if (Input.GetKey(Keys.Sprint))
			{
				characterModel.SetNewMotionDirection(deltaX, deltaZ, Character.CharacterMotionMode.Sprint);
			}
			else if (Input.GetKey(Keys.Walk))
			{
				characterModel.SetNewMotionDirection(deltaX, deltaZ, Character.CharacterMotionMode.Walk);
			}
			else
			{
				characterModel.SetNewMotionDirection(deltaX, deltaZ, Character.CharacterMotionMode.Run);
			}

			if (Input.GetKeyDown(Keys.Jump))
			{
				characterModel.InvokeJump();
			}
		}

		/// <summary>
		/// Перехват и обработка ввода с мыши, отвечающего за поворот объекта.
		/// </summary>
		private void RotationInput()
		{
			RotateHorizontal(Input.GetAxis("Mouse Horizontal"));
		}

		/// <summary>
		/// Перехват и обработка ввода с клавиатуры, отвечающего за открытие меню. (Тестовый код)
		/// </summary>
		private void MenuInput()
		{
			if (Input.GetKeyDown(Keys.Decline))
			{
				switch (motionBlocked)
				{
					case true:
						EnableMotionInputHandling();
						break;
					default:
						DisableMotionInputHandling();
						break;
				}
			}
		}

		/// <summary>
		/// Поворачивает объект на <paramref name="deltaHor"/> градусов по горизонтали.
		/// </summary>
		/// <param name="deltaHor">Смещение текущего угла поворота.</param>
		private void RotateHorizontal(float deltaHor)
		{
			// Получаем текущий угол поворота объекта
			float rotationHor = transform.localEulerAngles.y;

			// Добавляем к нему полученное смещение, умноженное на чувствительность
			rotationHor += deltaHor * SensitivitySettings.SensHor;

			// Заменяем угол поворота объекта новым
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rotationHor, 0);
		}

		private void DisableMotionInputHandling()
		{
			KeyboardInputHandler -= MotionInput;
			MouseInputHandler = null;
			motionBlocked = true;
		}

		private void EnableMotionInputHandling()
		{
			KeyboardInputHandler += MotionInput;
			MouseInputHandler = RotationInput;
			motionBlocked = false;
		}

		#endregion

		#region Private serialized class fields

		/// <summary>
		/// Ссылка на модель персонажа, за обработку ввода которого отвечает данный класс.
		/// </summary>
		[SerializeField] private Character.CharacterModel characterModel;

		[SerializeField] private bool motionBlocked = false;

		#endregion

		private const float LEFT = 1f;      // Соответствует движению влево
		private const float RIGHT = -1f;	// Соответствует движению вправо

		private const float FORWARD = 1f;   // Соответствует движению вперёд
		private const float BACKWARD = -1f; // Соответствует движению назад

		private delegate void InputHandler();

		private InputHandler KeyboardInputHandler = null;
		private InputHandler MouseInputHandler = null;
	}
}