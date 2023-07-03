using System;

enum RubikColor
{
	None = 0,
	Red,
	Green,
	Blue,
	Yellow,
	White,
	Orange
}

static class RubikColorHelper
{
	public static RubikColor[] values;
	public static string[] names;

	// static constructor
	static RubikColorHelper()
	{
		values = Enum.GetValues(typeof(RubikColor)) as RubikColor[];
		names = Enum.GetNames(typeof(RubikColor));
	}

	public static RubikColor FromName(string name)
	{
		int index = Array.IndexOf(names,name);
		if (index >= 0)
			return values[index];
		
		return RubikColor.None;
		
	}
}