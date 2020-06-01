using Controls;
using System.Collections;
using UnityEngine;

internal class CameraModel : MonoBehaviour
{
	internal Ray RayToScreenCenter { get; private set; }

	private void Awake()
	{
		screenCenterPoint = new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 2, 0);
	}

	private void FixedUpdate()
	{
		if (currentCamera.enabled && rotationAxes != CameraRotationAxes.None)
		{
			RayToScreenCenter = Camera.main.ScreenPointToRay(screenCenterPoint);
		}
	}

	private void Update()
	{
		if (currentCamera.enabled && rotationAxes != CameraRotationAxes.None)
		{
			MouseInput();
			KeyboardInput();

			transform.localEulerAngles = new Vector3(rotationVertical, rotationHorizontal, leanAngle);
		}
	}

	private void KeyboardInput()
	{
		if (Input.GetKeyDown(Keys.LeanLeft))
		{
			switch (currentLeanState)
			{
				case LeanStates.Left:
					_ = StartCoroutine(Lean(0f, LeanStates.None));
					break;
				default:
					_ = StartCoroutine(Lean(10f, LeanStates.Left));
					break;
			}
		}

		if (Input.GetKeyDown(Keys.LeanRight))
		{
			switch (currentLeanState)
			{
				case LeanStates.Right:
					_ = StartCoroutine(Lean(0f, LeanStates.None));
					break;
				default:
					_ = StartCoroutine(Lean(-10f, LeanStates.Right));
					break;
			}
		}
	}

	private void MouseInput()
	{
		if (rotationAxes == CameraRotationAxes.HorizontalOnly)
		{
			rotationHorizontal += Input.GetAxis("Mouse Horizontal") * SensitivitySettings.SensHor;

			rotationHorizontal = Mathf.Clamp(rotationHorizontal, minHorRotationAngle, maxHorRotationAngle);
		}

		if (rotationAxes == CameraRotationAxes.VerticalOnly)
		{
			rotationVertical -= Input.GetAxis("Mouse Vertical") * SensitivitySettings.SensVert;

			rotationVertical = Mathf.Clamp(rotationVertical, minVertRotationAngle, maxVertRotationAngle);
		}
	}

	private IEnumerator Lean(float newAngle, LeanStates newState)
	{
		if (newAngle > leanAngle)
		{
			do
			{
				leanAngle++;
				yield return new WaitForSeconds(.0001f);
			} while (leanAngle != newAngle);
		}
		else if (newAngle < leanAngle)
		{
			do
			{
				leanAngle--;
				yield return new WaitForSeconds(.0001f);
			} while (leanAngle != newAngle);
		}

		currentLeanState = newState;
	}

	internal void Unlock()
	{
		rotationAxes = defaultRotationAxes;
	}

	internal void Lock()
	{
		rotationAxes = CameraRotationAxes.None;
	}

	[SerializeField] private Camera currentCamera;

	[Header("Rotation axes")]
	[SerializeField] private CameraRotationAxes defaultRotationAxes = CameraRotationAxes.HorAndVert;
	[SerializeField] private CameraRotationAxes rotationAxes = CameraRotationAxes.HorAndVert;

	[Header("Rotation angles")]
	[SerializeField] private float minVertRotationAngle = -60f;
	[SerializeField] private float maxVertRotationAngle = 60f;
	[SerializeField] private float minHorRotationAngle = -360f;
	[SerializeField] private float maxHorRotationAngle = 360f;

	[SerializeField] private float leanAngle = 0f;
	[SerializeField] private LeanStates currentLeanState = LeanStates.None;

	private Vector3 screenCenterPoint;
	private float rotationVertical = 0f;
	private float rotationHorizontal = 0f;
}

internal enum CameraRotationAxes : byte
{
	HorAndVert,
	VerticalOnly,
	HorizontalOnly,
	None
}

internal enum LeanStates : byte
{
	None,
	Right,
	Left
}