using Controls;
using UnityEngine;

/// <summary>
/// Контроллер ввода, отвечающий за передвижение персонажа, к которому прикреплён.
/// </summary>

[RequireComponent(typeof(Character))]
internal class MotionController : MonoBehaviour
{
	#region Private Unity messages

	private void Awake()
	{
		character = GetComponent<Character>();
	}

	private void Update()
	{
		MouseInput();
		KeyboardInput();
	}

	#endregion

	#region Private class methods

	/// <summary>
	/// Перехват и обработка ввода с клавиатуры.
	/// </summary>
	private void KeyboardInput()
	{
		//
		// Если персонаж жив и не использует камеры на карте
		//
		if (character.Alive && !character.InCameraMode)
		{
			float deltaX = 0; // Смещение вдоль оси X
			float deltaZ = 0; // Смещение вдоль оси Z

			if (Input.GetKey(Keys.MoveForward))
			{
				deltaZ = 1f; // Соответствует движению вперёд
			}
			else if (Input.GetKey(Keys.MoveBackward))
			{
				deltaZ = -1f; // Соответствует движению назад
			}

			if (Input.GetKey(Keys.MoveLeft))
			{
				deltaX = 1f; // Соответствует движению влево
			}
			else if (Input.GetKey(Keys.MoveRight))
			{
				deltaX = -1f; // Соответствует движению вправо
			}

			if (Input.GetKey(Keys.Run))
			{
				character.Move(deltaX, deltaZ, CharacterMoveMode.Run);
			}
			else if (Input.GetKey(Keys.Walk))
			{
				character.Move(deltaX, deltaZ, CharacterMoveMode.SlowWalk);
			}
			else
			{
				character.Move(deltaX, deltaZ, CharacterMoveMode.Walk);
			}

			if (Input.GetKeyDown(Keys.Jump))
			{
				character.Jump();
			}
		}
	}

	/// <summary>
	/// Перехват и обработка ввода с мыши.
	/// </summary>
	private void MouseInput()
	{
		//
		// Если персонаж жив и не использует камеры на карте
		//
		if (character.Alive && !character.InCameraMode)
		{
			RotateHorizontal(Input.GetAxis("Mouse Horizontal"));
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

	#endregion

	#region Private serialized class fields

	/// <summary>
	/// Ссылка на компонент типа Character, за передвижение которого отвечает данный контроллер.
	/// </summary>
	[SerializeField] private Character character;

	#endregion
}
