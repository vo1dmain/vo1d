using System.Collections;
using UnityEngine;

///	<summary>
///	Отвечает за представление объекта как персонажа с характеристиками и возможными действиями.
/// </summary>

[RequireComponent(typeof(CharacterController))]
internal class Character : MonoBehaviour
{
	#region Internal class properties

	//	bool

	/// <summary>
	/// Возвращает <c>true</c>, если персонаж жив.
	/// </summary>
	internal bool Alive { get; private set; } = true;
	
	/// <summary>
	/// Возвращает <c>true</c>, если количество очков жизни выше нормального максимума.
	/// </summary>
	internal bool GotHealthBoost => healthPoints > normalHPMaximum;
	
	/// <summary>
	/// Возвращает <c>true</c>, если количество очков жизни равно superHPMaximum.
	/// </summary>
	internal bool GotSuperHealth => healthPoints == superHPMaximum;
	
	/// <summary>
	/// Возвращает <c>true</c>, если персонаж просматривает камеры на карте.
	/// </summary>
	internal bool InCameraMode { get; private set; } = false;

	//	uint

	/// <summary>
	/// Количество очков здоровья персонажа.
	/// </summary>
	internal uint HealthPoints => healthPoints;
	
	/// <summary>
	/// Количество очков брони персонажа.
	/// </summary>
	internal uint ArmorPoints => armorPoints;
	
	/// <summary>
	/// Количество патронов.
	/// </summary>
	internal uint Ammo => ammo;

	#endregion

	#region Private Unity messages

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	private void Update()
	{
		if (Alive)
		{
			motion = transform.TransformDirection(motion);

			_ = controller.SimpleMove(motion);
		}
	}

	private void LateUpdate()
	{
		if (Alive)
		{
			NormalizeIfHealthBoosted();
		}
	}

	#endregion

	#region Internal class methods

	/// <summary>
	/// Переключает персонажа в режим просмотра камер.
	/// </summary>
	internal void EnterCameraMode()
	{
		InCameraMode = true;
	}

	/// <summary>
	/// Выводит персонажа из режима просмотра камер.
	/// </summary>
	internal void ExitCameraMode()
	{
		InCameraMode = false;
	}

	/// <summary>
	/// Принимает и обрабатывает нанесённый персонажу урон.
	/// </summary>
	/// <param name="damagePoints">Количество полученного урона.</param>
	internal void ReceiveDamage(uint damagePoints)
	{
		if (Alive)
		{
			if (normalizeHealthRoutine != null)
			{
				StopCoroutine(normalizeHealthRoutine);
			}

			healthPoints -= damagePoints;

			if (healthPoints == 0)
			{
				Die();
			}
		}
	}

	/// <summary>
	/// Принимает и обрабатывает лечение, применённое к персонажу.
	/// </summary>
	/// <param name="healPoints">Количество восстанавливаемых очков здоровья</param>
	internal void ReceiveHeal(uint healPoints)
	{
		if (Alive)
		{
			takingHeal = healPoints > 0u;

			if (!GotSuperHealth)
			{
				healthPoints += healPoints;
			}
		}
	}

	/// <summary>
	/// Задаёт перемещение персонажа вдоль осей X и Z в одном из возможных режимов.
	/// </summary>
	/// <param name="deltaX">Смещение вдоль оси X. Отвечает за перемещение влево/вправо.</param>
	/// <param name="deltaZ">Смещение вдоль оси Z. Отвечает за перемещение вперёд/назад.</param>
	/// <param name="moveMode">Режим перемещения. Отвечает за скорость перемещения.</param>
	internal void Move(float deltaX, float deltaZ, CharacterMoveMode moveMode)
	{
		if (Grounded() && Alive)
		{
			switch (moveMode)
			{
				case CharacterMoveMode.SlowWalk:
					currentSpeed = walkingSpeed;
					break;
				case CharacterMoveMode.Walk:
					currentSpeed = movingSpeed;
					break;
				case CharacterMoveMode.Run:
					currentSpeed = runningSpeed;
					break;
				default:
					currentSpeed = 0;
					break;
			}

			motion = new Vector3(deltaX * currentSpeed, gravity, deltaZ * currentSpeed);

			motion = Vector3.ClampMagnitude(motion, currentSpeed);
		}
	}

	/// <summary>
	/// Заставляет персонажа выполнить прыжок.
	/// </summary>
	internal void Jump()
	{
		if (Grounded() && Alive)
		{
			StartCoroutine(JumpCoroutine());
		}
	}

	/// <summary>
	/// Заставляет персонажа начать стрельбу.
	/// </summary>
	/// <param name="firingRay">Луч, задающий точку, в которую будет произведён выстрел.</param>
	internal void Fire(Ray firingRay)
	{
		this.firingRay = firingRay;

		if (firingRoutine == null)
		{
			firingRoutine = StartCoroutine(FireCoroutine());
		}
	}

	/// <summary>
	/// Заставляет персонажа остановить стрельбу.
	/// </summary>
	internal void StopFiring()
	{
		if (firingRoutine != null)
		{
			StopCoroutine(firingRoutine);
			firingRoutine = null;
		}
	}

	/// <summary>
	/// Заставляет персонажа выполнить перезарядку.
	/// </summary>
	internal void Reload()
	{
		ammo = 30;
	}

	#endregion

	#region Private class methods

	/// <summary>
	/// Стоит ли персонаж на твёрдой поверхности.
	/// </summary>
	/// <returns>True - если на расстоянии 0.1 или меньше под персонажем находится твёрдая поверхность.</returns>
	private bool Grounded()
	{
		_ = Physics.SphereCast(controller.bounds.center, 0.5f, Vector3.down, out RaycastHit hit);

		return hit.distance <= .6f;
	}

	private IEnumerator JumpCoroutine()
	{
		controller.slopeLimit = 90f;

		float timeInAir = 0.0f;

		do
		{
			float jumpHeight = jumpFallOff.Evaluate(timeInAir);

			Vector3 jumpVector = Vector3.up * jumpHeight * jumpForce * Time.deltaTime;

			_ = controller.Move(jumpVector);

			timeInAir += Time.deltaTime;

			yield return null;
		}
		while (!Grounded() && controller.collisionFlags != CollisionFlags.Above);

		controller.slopeLimit = 45f;
	}

	private IEnumerator FireCoroutine()
	{
		while (ammo > 0)
		{
			if (Physics.Raycast(firingRay, out RaycastHit hit))
			{
				GameObject hittedObject = hit.transform.gameObject;

				Character target = hittedObject.GetComponent<Character>();

				switch (target)
				{
					case null:
						_ = StartCoroutine(SceneController.SphereIndicator(hit.point));
						break;
					default:
						target.ReceiveDamage(100);
						break;
				}
			}

			ammo--;

			yield return new WaitForSeconds(0.1f);
		}

		firingRoutine = null;
	}

	private IEnumerator NormalizeHealthCoroutine()
	{
		while (GotHealthBoost)
		{
			healthPoints--;
			
			yield return new WaitForSeconds(normalizeHealthDelay);
		}

		normalizeHealthRoutine = null;
	}

	/// <summary>
	/// Запускает механизм нормализации очков здоровья, если их количество выше нормального.
	/// </summary>
	private void NormalizeIfHealthBoosted()
	{
		if (GotHealthBoost && !takingHeal && normalizeHealthRoutine == null)
		{
			normalizeHealthRoutine = StartCoroutine(NormalizeHealthCoroutine());
		}
		else if (GotHealthBoost && takingHeal && normalizeHealthRoutine != null)
		{
			StopCoroutine(normalizeHealthRoutine);
			normalizeHealthRoutine = null;
		}
	}

	/// <summary>
	/// Заставляет персонажа умереть.
	/// </summary>
	private void Die()
	{
		StopAllCoroutines();

		Alive = false;
	}

	#endregion

	#region Private serialized class fields

	//Unity classes

	[SerializeField] private AnimationCurve jumpFallOff;
	[SerializeField] private CharacterController controller;
	
	//bool

	[SerializeField] private bool takingHeal = false;

	//float

	[SerializeField] private float walkingSpeed = 5f;
	[SerializeField] private float movingSpeed = 10f;
	[SerializeField] private float runningSpeed = 20f;
	[SerializeField] private float jumpForce = 8f;
	[SerializeField] private float gravity = -9.82f;
	[SerializeField] private float normalizeHealthDelay = 0.5f;

	//uint

	[SerializeField] private uint healthPoints = 100;
	[SerializeField] private uint armorPoints = 100;
	[SerializeField] private uint ammo = 30;
	[SerializeField] private uint normalHPMaximum = 100;
	[SerializeField] private uint superHPMaximum = 150;

	#endregion

	#region Private non-serialized class fields

	private Coroutine firingRoutine;
	private Coroutine normalizeHealthRoutine;

	private Ray firingRay;
	private Vector3 motion = Vector3.zero;

	private float currentSpeed = 0;

	#endregion
}

internal enum CharacterMoveMode : byte
{
	SlowWalk,
	Walk,
	Run
}