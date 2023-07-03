using UnityEngine;

class RubikSubBlock : MonoBehaviour
{
	[SerializeField] RubikColor upColor;
	[SerializeField] RubikColor downColor;
	[SerializeField] RubikColor leftColor;
	[SerializeField] RubikColor rightColor;
	[SerializeField] RubikColor frontColor;
	[SerializeField] RubikColor backColor;

	[SerializeField] Vector3Int startIndex;

	public Vector3Int CurrentLocalIndex => Vector3Int.RoundToInt(transform.localPosition);

	void OnValidate()
	{
		// Setup Colors
		for (int i = 0; i < transform.childCount; i++) 
		{
			Transform child = transform.GetChild(i);
			string name = child.name;
			RubikColor color = RubikColorHelper.FromName(name);
			if (color == RubikColor.None)
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

	public RubikColor GetColorByGlobalDirection(Direction globalDirection)
	{
		Vector3 worldVector = DirectionHelper.ToVector(globalDirection);
		Vector3 localVector = transform.InverseTransformDirection(worldVector);
		Direction localDirection = DirectionHelper.ClosestDirection(localVector);
		return GetColorByLocalDirection(localDirection);
	}

	RubikColor GetColorByLocalDirection(Direction localDirection) 
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
				return RubikColor.None;
		}
	}
}