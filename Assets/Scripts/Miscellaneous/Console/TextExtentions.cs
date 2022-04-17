using Treasury.Miscellaneous.Optimization.MonoCache;

namespace Treasury.Miscellaneous.Console
{
	public static class TextExtentions
	{
		public const string WHITE_COLOR = "FFFFFF";
		public const string BLUE_COLOR = "00FFF7";
		public const string ORANGE_COLOR = "F4CA16";
		public const string RED_COLOR = "E22121";
		
		public static string GetExceptionBaseText(string methodName, string className)
		{
			var classNameColored = GetColoredHTMLText(RED_COLOR, className);
			var monoCacheNameColored = GetColoredHTMLText(ORANGE_COLOR, nameof(MonoCache));
			var methodNameColored = GetColoredHTMLText(RED_COLOR, methodName);
			var baseTextColored = GetColoredHTMLText(WHITE_COLOR,
				$"can't be implemented in subclass {classNameColored} of {monoCacheNameColored}. Use ");
            
			return $"{methodNameColored} {baseTextColored}";
		}
		
		public static string GetWarningBaseText(string methodName, string recommendedMethod, string className)
		{
			var coloredClass = GetColoredHTMLText(ORANGE_COLOR, className);
			var monoCacheNameColored = GetColoredHTMLText(ORANGE_COLOR, nameof(MonoCache));
			var coloredMethod = GetColoredHTMLText(ORANGE_COLOR, methodName);
            
			var coloredRecommendedMethod =
				GetColoredHTMLText(BLUE_COLOR, "protected override void ") + 
				GetColoredHTMLText(ORANGE_COLOR, recommendedMethod);
            
			var coloredBaseText = GetColoredHTMLText(WHITE_COLOR, 
				$"It is recommended to replace {coloredMethod} method with {coloredRecommendedMethod} " +
				$"in subclass {coloredClass} of {monoCacheNameColored}");
            
			return coloredBaseText;
		}

		public static string GetColoredHTMLText(string color, string text)
		{
			if (color.IndexOf('#') == -1)
				color = '#' + color;

			return $"<color={color}>{text}</color>";
		}
	}
}