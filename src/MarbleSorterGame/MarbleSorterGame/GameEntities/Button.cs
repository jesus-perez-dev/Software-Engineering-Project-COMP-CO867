using System;
using MarbleSorterGame;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame.GameEntities
{
	public class Button : GameEntity
	{
		private RectangleShape _button;
		private Text _text;

		public bool Hovered { get; set; }
		public bool Disabled { get; set; }
		
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
		/// Full constructor of Button class, with all parameters
		/// </summary>
		/// <param name="displayText">String label on button</param>
		/// <param name="fontScale">Size of the font</param>
		/// <param name="font">Font of button label</param>
		/// <param name="position">Vector coordinate of center of button</param>
		/// <param name="size">Vector size of button</param>
		public Button(string displayText, float fontScale, Font font, Vector2f position, Vector2f size) :
			base(position, size)
		{
			_text = QuickShape.Label(displayText, position, font, SFML.Graphics.Color.Black);
			_text.Scale = new Vector2f(fontScale, fontScale);
			_text.Position = position; // new Vector2f(position.X + (size.X/2), position.Y + (size.Y/2));
			_text.Origin = _text.CenterOrigin();

			_button = Box; //new RectangleShape(size);
			_button.FillColor = new SFML.Graphics.Color(200, 200, 200);
			_button.OutlineColor = SFML.Graphics.Color.Black;
			_button.OutlineThickness = 1f;
			_button.Origin = _button.CenterOrigin(); //set transform origins of text/button to its center 
			_button.Position = position;

			Hovered = false;
			Disabled = false;
		}

		/// <summary>
		/// checks whether button has been pressed with mouse coordinates
		/// </summary>
		/// <param name="X">Mouse pressed x-coordinate</param>
		/// <param name="Y">Mouse pressed y-coordinate</param>
		/// <returns></returns>
		public bool MouseInButton(int X, int Y)
		{
			FloatRect buttonBounds = _button.GetGlobalBounds();
			return (
				X > buttonBounds.Left && X < buttonBounds.Left + buttonBounds.Width &&
				Y > buttonBounds.Top && Y < buttonBounds.Top + buttonBounds.Height);
		}

		public delegate bool IsPressedDelegate(int X, int Y);

		public override void Render(RenderWindow window)
		{
			if (Disabled)
			{
				_button.FillColor = new Color(200,200,200);
				_text.FillColor = Color.White;
			}
			else if (Hovered)
			{
				_button.FillColor = new Color(50, 50, 50);
				_text.FillColor = Color.White;
			}
			else
			{
				_button.FillColor = new Color(150, 150, 150);
				_text.FillColor = Color.Black;
			}

			window.Draw(_button);
			window.Draw(_text);
		}

		public override void Load(IAssetBundle bundle)
		{
		}
	}
}