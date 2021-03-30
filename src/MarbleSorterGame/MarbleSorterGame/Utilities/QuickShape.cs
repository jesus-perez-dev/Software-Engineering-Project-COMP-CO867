using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Utilities
{
    // Contains convenience methods for constructing commonly use game shapes/drawables
    public static class QuickShape
    {
        public static Text Label(string displayString, Vector2f position, Font font, SFML.Graphics.Color color)
        {
			return new Text
			{
				Position = position,
				DisplayedString = displayString,
				Font = font,
				FillColor = color
			};
        }
    }
}