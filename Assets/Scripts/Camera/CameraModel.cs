using UnityEngine;
using Controls;

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

		transform.localEulerAngles = new Vector3(rotationVertical, rotationHorizontal, 0);
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