using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

class RubicCube : MonoBehaviour
{
	[SerializeField] CubeRaycaster raycaster;
	[SerializeField] Transform angleRotator;
	[SerializeField] float autoRotateTime = 0.2f;
	[SerializeField] int scrambleCount = 10;

	public event Action OnSolved;

	public RubicSubBlock[] subBlocks;
	public Pose[] defaultPoses;

	bool autoRotate = false;
	bool manualRotate = false;
	Vector3 angularVelocity = Vector3.zero;

	void Awake()
	{
		subBlocks = GetComponentsInChildren<RubicSubBlock>();
		defaultPoses = new Pose[subBlocks.Length];
		for (int i = 0; i < subBlocks.Length; i++)
		{
			Transform subBlock = subBlocks[i].transform;
			defaultPoses[i] = new Pose(subBlock.position, subBlock.rotation);
		}

		raycaster.OnAxisFixed += OnAxisFixed;
		raycaster.OnMouseDrag += OnMouseDrag;
		raycaster.OnMouseUp += OnMouseUp;
	}

	readonly List<Transform> selected = new();

	void OnAxisFixed()
	{
		if (autoRotate) return;
		if (!enabled) return;

		manualRotate = true;
		angleRotator.rotation = Quaternion.identity;

		Vector3Int clicked = raycaster.GetClickedObject().GetComponentInParent<RubicSubBlock>().CurrentLocalIndex;
		Vector3 axis = raycaster.RotationAxis;

		SetBlocksParent(clicked, axis);
	}

	void FindSubCubesOnAxis(Vector3Int clickedLocalIndex, Vector3 globalAxis, List<Transform> selected)
	{ 
		Vector3Int localAxis = Vector3Int.RoundToInt(transform.InverseTransformDirection(globalAxis));

		Vector3Int center = clickedLocalIndex;
		if (localAxis.x == 0)
			center.x = 0;
		if (localAxis.y == 0)
			center.y = 0;
		else if (localAxis.z == 0)
			center.z = 0;

		foreach (RubicSubBlock subCube in subBlocks)
		{
			if (IsSubCubeOnAxis(center, localAxis, subCube.CurrentLocalIndex))
				selected.Add(subCube.transform);
		}
	}

	bool IsSubCubeOnAxis(Vector3Int center, Vector3Int axis, Vector3Int subCubePosition)
	{
		Vector3Int relativePosition = subCubePosition - center;
		return Vector3.Dot(relativePosition, axis) == 0;
	}


	void OnMouseDrag()
	{
		if (manualRotate)
			angleRotator.rotation = Quaternion.AngleAxis(raycaster.RotationAngle, raycaster.RotationAxis);
	}

	void OnMouseUp()
	{
		autoRotate = true;
		manualRotate = false;
	}

	void Update()
	{
		if (!autoRotate) return;

		autoRotate = CubeRotator.AutoRotate(angleRotator, ref angularVelocity, autoRotateTime);
		if (!autoRotate)
			TurnDone();
	}

	void TurnDone()
	{
		ResetBlocksParent();

		if (TestRubicCube())
			OnSolved?.Invoke();
	}

	void SetBlocksParent(Vector3Int clicked, Vector3 axis)
	{
		selected.Clear();
		FindSubCubesOnAxis(clicked, axis, selected);

		foreach (Transform subCube in selected)
			subCube.SetParent(angleRotator);
	}

	void ResetBlocksParent()
	{
		for (int i = angleRotator.childCount - 1; i >= 0; i--)
		{
			Transform child = angleRotator.GetChild(i);
			child.SetParent(transform);
		}
	}

	bool TestRubicCube()
	{
		foreach (Direction dir in DirectionHelper.allDirections)
		{
			if (!TestRubicCubeSide(dir))
				return false;
		}
		return true;
	}

	bool TestRubicCubeSide(Direction dir)
	{
		RubicColor color = RubicColor.None;
		foreach (RubicSubBlock subCube in subBlocks)
		{
			RubicColor subCubeColor = subCube.GetColorByGlobalDirection(dir);
			if (color == RubicColor.None)
				color = subCubeColor;
			else if (subCubeColor == RubicColor.None)
				continue;
			else if (color != subCubeColor)
				return false;

		}
		return true;
	}

	public void Solve() 
	{
		for (int i = 0; i < subBlocks.Length; i++)
		{
			Transform subBlock = subBlocks[i].transform;
			subBlock.SetPositionAndRotation(defaultPoses[i].position, defaultPoses[i].rotation);
		}
	}

	readonly Vector3[] allArises = new Vector3[] { Vector3.up, Vector3.right, Vector3.forward };

	public Pose[] scramblesPoses;

	public void Scramble()
	{
		Solve();
		for (int i = 0; i < scrambleCount; i++)
		{
			Vector3 axis = allArises[Random.Range(0, allArises.Length)];
			Vector3Int vector3Int = new(
				Random.Range(-1, 2),
				Random.Range(-1, 2),
				Random.Range(-1, 2));

			SetBlocksParent(vector3Int, axis);
			angleRotator.RotateAround(Vector3.zero, axis, Random.Range(1, 4) * 90);
			ResetBlocksParent();
		}

		scramblesPoses = new Pose[subBlocks.Length];
		for (int i = 0; i < subBlocks.Length; i++)
		{
			Transform subBlock = subBlocks[i].transform;
			scramblesPoses[i] = new Pose(subBlock.position, subBlock.rotation);
		}
	}
}