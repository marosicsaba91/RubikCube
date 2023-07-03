using System;
using UnityEngine;

public class CubeRaycaster: MonoBehaviour
{
	enum State { None, MouseDown, AxisFixed }

	[SerializeField] float sensitivity = 30;
	[SerializeField] float distanceToLockAxis = 0.2f;
	[SerializeField] int mouseButton = 0;
	[SerializeField] Color gizmoColor = Color.red;

	public event Action OnMouseDown;
	public event Action OnMouseDrag;
	public event Action OnAxisFixed;
	public event Action OnMouseUp;

	GameObject clickedObject;
	Vector3 mouseDownPosition;
	Vector3 mouseDownNormal;
	Vector3 mousePosition;
	Vector3 mouseAxis;

	State state = State.None;

	public Vector3 RotationAxis { get; private set; } = Vector3.zero;
	public float RotationAngle { get; private set; } = 0;
	public GameObject GetClickedObject() => clickedObject;

	void Update()
	{
		HandleInput();
	}

	void HandleInput()
	{
		if (Input.GetMouseButtonDown(mouseButton) & state == State.None)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				mouseDownNormal = hit.normal;
				mouseDownPosition = hit.point;
				state = State.MouseDown;
				clickedObject = hit.collider.gameObject;
				OnMouseDown?.Invoke();
			}
		}

		if (Input.GetMouseButton(mouseButton) && state is State.MouseDown or State.AxisFixed )
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Plane plane = new(mouseDownNormal, mouseDownPosition);
			if (plane.Raycast(ray, out float distance))
			{
				mousePosition = ray.GetPoint(distance);

				Vector3 fullRotation = mousePosition - mouseDownPosition;
				Vector3 rotationVector = mousePosition - mouseDownPosition;

				if (state == State.MouseDown)
				{
					mouseAxis = OnlyOneAxis(rotationVector);
					mouseAxis.Normalize();
					mouseAxis.x = Mathf.Abs(mouseAxis.x);
					mouseAxis.y = Mathf.Abs(mouseAxis.y);
					mouseAxis.z = Mathf.Abs(mouseAxis.z);
					RotationAxis = Vector3.Cross(mouseDownNormal, mouseAxis);
				}

				if (state == State.MouseDown && fullRotation.magnitude >= distanceToLockAxis)
				{
					OnAxisFixed?.Invoke();
					state = State.AxisFixed;
				}

				Vector3 projection = Vector3.Project(rotationVector, mouseAxis); 
				RotationAngle = (projection.x + projection.y + projection.z) * sensitivity;
				OnMouseDrag?.Invoke();
			}
		}

		if (Input.GetMouseButtonUp(mouseButton) && state == State.AxisFixed)
		{
			clickedObject = null;
			state = State.None;
			OnMouseUp?.Invoke();
		}
	}

	private Vector3 OnlyOneAxis(Vector3 axis)
	{
		float x = Mathf.Abs(axis.x);
		float y = Mathf.Abs(axis.y);
		float z = Mathf.Abs(axis.z);

		if (x > y && x > z)
			return new Vector3(axis.x, 0, 0);
		else if (y > x && y > z)
			return new Vector3(0, axis.y, 0);
		else
			return new Vector3(0, 0, axis.z);
	}

	void OnDrawGizmos()
	{
		if (state == State.None) return;

		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(mouseDownPosition, 0.1f);
		Gizmos.DrawSphere(mousePosition, 0.1f);
		Gizmos.DrawLine(mouseDownPosition, mousePosition);
	}
}
