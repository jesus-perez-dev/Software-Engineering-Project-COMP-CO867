using System;
using MarbleSorterGame.Utilities;
using SFML.System;
using SFML.Graphics;

namespace MarbleSorterGame.GameEntities
{
	/// <summary>
	/// label is simply a centered text
	/// Centered text
	/// </summary>
	public class Label : Drawable
	{
		public String Text
        {
			get => _text.DisplayedString;
			set => _text.DisplayedString = value;
        }

		private Text _text;
		public Text LabelText { get => _text; }
		public Vector2f _labelPosition { get; }
		private String _labelText;
		private int _labelSize;
		private SFML.Graphics.Color _labelColor;
		private Font _labelFont;

		/// <summary>
		/// Use when creating stand-alone label
		/// </summary>
		/// <param name="labelText"></param>
		/// <param name="labelPosition"></param>
		/// <param name="labelSize"></param>
		/// <param name="labelColor"></param>
		/// <param name="labelFont"></param>
		public Label(String labelText, Vector2f? labelPosition, int labelSize, SFML.Graphics.Color labelColor, Font labelFont)
		{
			_labelText = labelText;
			if (labelPosition != null)
			{
				_labelPosition = (Vector2f) labelPosition;
			}

			_labelSize = labelSize;
			_labelColor = labelColor;
			_labelFont = labelFont;

			_text = new Text(_labelText, _labelFont, (uint) _labelSize);
			_text.FillColor = _labelColor;

			_text.Origin = _text.CenterOrigin();
			_text.Position = _labelPosition;
		}

		public void Draw(RenderWindow window)
		{
			window.Draw(_text);
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_text);
		}
	}
}