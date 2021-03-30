﻿using System;
using MarbleSorterGame.Utilities;
using SFML.System;
using SFML.Graphics;

namespace MarbleSorterGame.GameEntities
{
	// label is simply a centered text
	// Centered text
	public class Label : Drawable
	{
		public String Text
        {
			get => _text.DisplayedString;
			set => _text.DisplayedString = value;
        }

		public Vector2f LabelPosition
		{
			set => _text.Position = value;
			get => _text.Position;
		}

		private Text _text;
		public Text LabelText { get => _text; }
		private String _labelText;
		private int _labelSize;
		private SFML.Graphics.Color _labelColor;
		private Font _labelFont;

		// Constructor for label
		public Label(String labelText, Vector2f? labelPosition, int labelSize, SFML.Graphics.Color labelColor, Font labelFont)
		{
			_labelText = labelText;

			_labelSize = labelSize;
			_labelColor = labelColor;
			_labelFont = labelFont;

			_text = new Text(_labelText, _labelFont, (uint) _labelSize);
			_text.FillColor = _labelColor;

			if (labelPosition != null)
			{
				LabelPosition = (Vector2f) labelPosition;
			}

			_text.Origin = _text.CenterOrigin();
			_text.Position = LabelPosition;
		}

		// Draw method of label
		public void Draw(RenderWindow window)
		{
			window.Draw(_text);
		}

		// Draw method of label
		public void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_text);
		}
	}
}