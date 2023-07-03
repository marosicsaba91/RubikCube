using System;
using UnityEngine;

enum Direction
{
	Right,
	Left,
	Up,
	Down,
	Forward,
	Back,
};


static class DirectionHelper
{

	public static readonly Direction[] allDirections;

	static DirectionHelper()
	{
		allDirections = Enum.GetValues(typeof(Direction)) as Direction[];
	}
	public static Vector3 ToVector(Direction dir)
	{
		switch (dir)
		{
			case Direction.Forward:
				return Vector3.forward;
			case Direction.Right:
				return Vector3.right;
			case Direction.Back:
				return Vector3.back;
			case Direction.Left:
				return Vector3.left;
			case Direction.Up:
				return Vector3.up;
			case Direction.Down:
				return Vector3.down;
			default:
				return Vector3.zero;
		}
	}

	public static Direction ClosestDirection(Vector3 dir)
	{
		float x = Mathf.Abs(dir.x);
		float y = Mathf.Abs(dir.y);
		float z = Mathf.Abs(dir.z);

		if (x > y && x > z) 
			return dir.x > 0 ? Direction.Right : Direction.Left;

		if (y > x && y > z)
			return dir.y > 0 ? Direction.Up : Direction.Down;

		return dir.z > 0 ? Direction.Forward : Direction.Back;
	}

}