﻿using System;
using MarbleSorterGame;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MarbleSorterGame.GameEntities
{

	/// <summary>
	/// Menu object that, when clicked, links to other screens or features
	/// </summary>
	public class Button : GameEntity
	{
		private RectangleShape _button;
		private Text _text;
		
		private static Color _disabledFillColor = new Color(200,200,200);
		private static Color _disabledTextColor = Color.White;
		
		private static Color _hoveredFillColor = new Color(50, 50, 50);
		private static Color _hoveredTextColor = Color.White;
		
		//private static Color _defaultFillColor = new Color(200, 200, 200);
		private static Color _defaultFillColor = new Color(238, 236, 150);
		private static Color _defaultTextColor = Color.Black;
		public bool Hovered { get; set; }
		public bool Disabled { get; set; }

		public event EventHandler<MouseButtonEventArgs> ClickEvent;

		public Color FillColor
		{
			get => _button.FillColor;
			set => _button.FillColor = value;
		}

		public String LabelText
		{
			get => _text.DisplayedString;
			set => _text.DisplayedString = value;
		}
		
		/// <summary>
		/// Constructor of button
		/// </summary>
		/// <param name="displayText">Text the button will display</param>
		/// <param name="fontScale">Text size of button</param>
		/// <param name="font">Font of button</param>
		/// <param name="position">Global vector position</param>
		/// <param name="size">Global vector size</param>
		public Button(string displayText, float fontScale, Font font, Vector2f position, Vector2f size) :
			base(position, size)
		{
			_text = QuickShape.Label(displayText, position, font, SFML.Graphics.Color.Black);
			_text.Scale = new Vector2f(fontScale, fontScale);
			_text.Position = position; // new Vector2f(position.X + (size.X/2), position.Y + (size.Y/2));
			_text.Origin = _text.CenterOrigin();

			_button = Box; //new RectangleShape(size);
			//_button.FillColor = new SFML.Graphics.Color(200, 200, 200);
			_button.FillColor = new Color(218, 216, 124);
			_button.OutlineColor = Color.Black;
			_button.OutlineThickness = 2f;
			_button.Origin = _button.CenterOrigin(); //set transform origins of text/button to its center 
			_button.Position = position;

			Hovered = false;
			Disabled = false;
		}

		public override Vector2f Position
		{
			get => Position;
			set
			{
				 _text.Position = value;
				 Box.Position = value;
			}
		}

		/// <summary>
		/// checks whether button has been pressed with mouse coordinates
		/// </summary>
		/// <param name="x">Global x-coordinate of mouse position</param>
		/// <param name="y">Global y-coordinate of mouse position</param>
		/// <returns></returns>
		public bool MouseInButton(int x, int y)
		{
			FloatRect buttonBounds = _button.GetGlobalBounds();
			return (
				x > buttonBounds.Left && x < buttonBounds.Left + buttonBounds.Width &&
				y > buttonBounds.Top && y < buttonBounds.Top + buttonBounds.Height);
		}

		/// <summary>
		/// Click event handler for button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void Click(object? sender, MouseButtonEventArgs args) => ClickEvent?.Invoke(sender, args);

		/// <summary>
		/// Draws button
		/// </summary>
		/// <param name="window"></param>
		public override void Render(RenderWindow window)
		{
			if (Disabled)
			{
				_button.FillColor = _disabledFillColor;
				_text.FillColor = _disabledTextColor;
			}
			else if (Hovered)
			{
				_button.FillColor = _hoveredFillColor;
				_text.FillColor = _hoveredTextColor;
			}
			else
			{
				_button.FillColor = _defaultFillColor;
				_text.FillColor = _defaultTextColor;
			}

			window.Draw(_button);
			window.Draw(_text);
		}

		/// <summary>
		/// Loads assets required for button
		/// </summary>
        /// <param name="bundle">reference to bundle object containing asset references</param>
		public override void Load(IAssetBundle bundle)
		{
		}
	}
}