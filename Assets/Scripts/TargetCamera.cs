using System;
using UnityEngine;

[ExecuteAlways]
class TargetCamera : MonoBehaviour
{
	[SerializeField] Transform target;

	[SerializeField] float verticalAngle = 10;
	[SerializeField] float horizontalAngle = 10;

	[SerializeField] float maxHorizontalAngle = 85;
	[SerializeField] float minHorizontalAngle = 5;

	[SerializeField] float maxVerticalAngle = 85;
	[SerializeField] float minVerticalAngle = 5;

	[SerializeField] float fieldOfView = 45;
	[SerializeField] float targetSize = 10;

	[SerializeField] float horizontalSensitivity = 1f;
	[SerializeField] float verticalSensitivity = 1f;

	[SerializeField] bool switchHorizontal = true;
	[SerializeField] bool switchVertical = true; 

	[SerializeField] int mouseButton = 2;
	
	void Update()
	{
		Vector3 targetPos = target.position;

		HandleInput();

		float distance = targetSize / (2 * MathF.Tan(Mathf.Deg2Rad * fieldOfView / 2));

		float horizontalDistance = Mathf.Cos(verticalAngle * Mathf.Deg2Rad) * distance;
		float verticalDistance = Mathf.Sin(verticalAngle * Mathf.Deg2Rad) * distance;

		float xDistance = Mathf.Cos(horizontalAngle * Mathf.Deg2Rad) * horizontalDistance;
		float zDistance = Mathf.Sin(horizontalAngle * Mathf.Deg2Rad) * horizontalDistance;

		Vector3 offsetVector = new(xDistance, verticalDistance, zDistance);

		transform.position = targetPos + offsetVector;
		transform.LookAt(targetPos);

		Camera.main.fieldOfView = fieldOfView;
	}

	void HandleInput()
	{
		if(!Input.GetMouseButton(mouseButton))
			return;

		float x = Input.GetAxis("Mouse X");
		float y = Input.GetAxis("Mouse Y");

		if (switchHorizontal)
			x = -x;
		if (switchVertical)
			y = -y;

		horizontalAngle += x * horizontalSensitivity;
		verticalAngle += y * verticalSensitivity;

		horizontalAngle = Mathf.Clamp(horizontalAngle, minHorizontalAngle, maxHorizontalAngle);
		verticalAngle = Mathf.Clamp(verticalAngle, minVerticalAngle, maxVerticalAngle);

	}
}
