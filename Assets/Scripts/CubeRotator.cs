using UnityEngine; 

public class CubeRotator : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] CubeRaycaster raycaster;
	[SerializeField] float autoRotateTime = 0.2f; 
	
	Vector3 angularVelocity = Vector3.zero;
	Quaternion originalRotation;
	bool autoRotate = false;

	private void Start()
	{
		raycaster.OnMouseDown += OnMouseDown;
		raycaster.OnMouseDrag += OnMouseDrag;
		raycaster.OnMouseUp += OnMouseUp;
	}

	void OnMouseDown()
	{
		originalRotation = target.rotation;
	}
	void OnMouseDrag()
	{
		target.rotation = Quaternion.AngleAxis(raycaster.RotationAngle, raycaster.RotationAxis) * originalRotation;
	}

	void OnMouseUp()
	{
		autoRotate = true;
	}

	void Update()
	{
		if (autoRotate)
			autoRotate = AutoRotate(target, ref angularVelocity, autoRotateTime);
	}


	public static bool AutoRotate(Transform target, ref Vector3 angularVelocity, float rotateTime)
	{
		Vector3 currentEuler = target.rotation.eulerAngles;
		float targetEulerX = Mathf.Round(currentEuler.x / 90) * 90;
		float targetEulerY = Mathf.Round(currentEuler.y / 90) * 90;
		float targetEulerZ = Mathf.Round(currentEuler.z / 90) * 90;
		Vector3 targetEuler = new(targetEulerX, targetEulerY, targetEulerZ);

		const float epsilon = 0.1f;
		bool closeEnough =
			Mathf.Abs(targetEulerX - currentEuler.x) < epsilon &&
			Mathf.Abs(targetEulerY - currentEuler.y) < epsilon &&
			Mathf.Abs(targetEulerZ - currentEuler.z) < epsilon;

		if (closeEnough)
		{ 
			target.rotation = Quaternion.Euler(targetEuler);
			return false;
		}

		Vector3 euler = Vector3.SmoothDamp(currentEuler, targetEuler, ref angularVelocity, rotateTime, float.MaxValue, Time.deltaTime);
		target.rotation = Quaternion.Euler(euler);

		return true;
	}
}
