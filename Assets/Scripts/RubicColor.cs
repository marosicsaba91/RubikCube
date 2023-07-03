using System;

enum RubicColor
{
	None = 0,
	Red,
	Green,
	Blue,
	Yellow,
	White,
	Orange
}

static class RubicColorHelper
{
	public static RubicColor[] values;
	public static string[] names;

	// static constructor
	static RubicColorHelper()
	{
		values = Enum.GetValues(typeof(RubicColor)) as RubicColor[];
		names = Enum.GetNames(typeof(RubicColor));
	}

	public static RubicColor FromName(string name)
	{
		int index = Array.IndexOf(names,name);
		if (index >= 0)
			return values[index];
		
		return RubicColor.None;
		
	}
}