using System.Collections;
using UnityEngine;

[AddComponentMenu(menuName: "Scene Controller")]
internal class SceneController : MonoBehaviour
{
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	internal static IEnumerator SphereIndicator(Vector3 point)
	{
		GameObject sphereIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);

		sphereIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

		sphereIndicator.GetComponent<SphereCollider>().enabled = false;

		sphereIndicator.transform.position = point;

		sphereIndicator.layer = 2;

		yield return new WaitForSeconds(1f);

		Destroy(sphereIndicator);
	}

	[SerializeField] private Camera[] mapCams;

	private static Camera currentCamera;

	private static int lastCamIndex = 0;
}
