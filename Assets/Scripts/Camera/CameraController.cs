using UnityEngine;
using Controls;

internal class CameraController : MonoBehaviour
{
	internal Ray RayToScreenCenter { get; private set; }

	private void Awake()
	{
		currentCamera = GetComponent<Camera>();
	}

	private void Start()
	{
		screenCenterPoint = new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 2, 0);
	}

	private void Update()
	{
		if (currentCamera.enabled && rotationAxes != CameraRotationAxes.None)
		{
			MouseInput();

			RayToScreenCenter = Camera.main.ScreenPointToRay(screenCenterPoint);
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

	[SerializeField] private CameraRotationAxes defaultRotationAxes = CameraRotationAxes.HorAndVert;
	[SerializeField] private CameraRotationAxes rotationAxes = CameraRotationAxes.HorAndVert;

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