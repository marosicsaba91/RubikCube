using UnityEngine;

class RubicSubBlock : MonoBehaviour
{
	[SerializeField] RubicColor upColor;
	[SerializeField] RubicColor downColor;
	[SerializeField] RubicColor leftColor;
	[SerializeField] RubicColor rightColor;
	[SerializeField] RubicColor frontColor;
	[SerializeField] RubicColor backColor;

	[SerializeField] Vector3Int startIndex;

	public Vector3Int CurrentLocalIndex => Vector3Int.RoundToInt(transform.localPosition);

	void OnValidate()
	{
		// Setup Colors
		for (int i = 0; i < transform.childCount; i++) 
		{
			Transform child = transform.GetChild(i);
			string name = child.name;
			RubicColor color = RubicColorHelper.FromName(name);
			if (color == RubicColor.None)
				continue;


			Vector3 localVector = child.localPosition.normalized;
			Direction localDirection = DirectionHelper.ClosestDirection(localVector);

			if (localDirection == Direction.Up)
				upColor = color;
			else if (localDirection == Direction.Down)
				downColor = color;
			else if (localDirection == Direction.Left)
				leftColor = color;
			else if (localDirection == Direction.Right)
				rightColor = color;
			else if (localDirection == Direction.Forward)
				frontColor = color;
			else if (localDirection == Direction.Back)
				backColor = color;
		}		
	}

	public RubicColor GetColorByGlobalDirection(Direction globalDirection)
	{
		Vector3 worldVector = DirectionHelper.ToVector(globalDirection);
		Vector3 localVector = transform.InverseTransformDirection(worldVector);
		Direction localDirection = DirectionHelper.ClosestDirection(localVector);
		return GetColorByLocalDirection(localDirection);
	}

	RubicColor GetColorByLocalDirection(Direction localDirection) 
	{
		switch(localDirection)
		{
			case Direction.Forward:
				return frontColor;
			case Direction.Right:
				return rightColor;
			case Direction.Back:
				return backColor;
			case Direction.Left:
				return leftColor;
			case Direction.Up:
				return upColor;
			case Direction.Down:
				return downColor;

			default:
				return RubicColor.None;
		}
	}
}