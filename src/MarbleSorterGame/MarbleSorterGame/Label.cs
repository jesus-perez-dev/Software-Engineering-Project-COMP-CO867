using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using SFML.System;
using SFML.Graphics;
using UtilityCentering;

/// <summary>
/// label is simply a centered text
/// </summary>

public class Label
{
	public Text Text { get; }
	public Vector2f _labelPosition { get; }
	private String _labelText;
	private int _labelSize;
	private Color _labelColor;
	private Font _labelFont;

	/// <summary>
	/// Use when creating stand-alone label
	/// </summary>
	/// <param name="labelText"></param>
	/// <param name="labelPosition"></param>
	/// <param name="labelSize"></param>
	/// <param name="labelColor"></param>
	/// <param name="labelFont"></param>
	public Label(String labelText, Vector2f ?labelPosition, int labelSize, Color labelColor, Font labelFont)
	{
		_labelText = labelText;
		if (labelPosition != null) {
			_labelPosition = (Vector2f)labelPosition;
        }

		_labelSize = labelSize;
		_labelColor = labelColor;
		_labelFont = labelFont;

		Text = new Text(_labelText, _labelFont, (uint)_labelSize);
		Text.FillColor = _labelColor;

		Text.Origin = Text.CenterOrigin();
		Text.Position = _labelPosition;
	}

	public static Text Create(string displayString, Vector2f position, Font font, Color color)
	{
		/**
		return new Text
		{
			Position = position,
			DisplayedString = displayString,
			Font = font,
			FillColor = color,
		};
		*/
		return null;
	}
	
	public void Draw(RenderWindow window)
    {
		window.Draw(Text);
    }
}
