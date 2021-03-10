using System;
using MarbleSorterGame;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using UtilityCentering;
using Color = SFML.Graphics.Color;

public class Button : GameEntity
{
    private RectangleShape _button;
    private Text _text;

	/// <summary>
	/// full constructor of Button class, with all parameters
	/// </summary>
	/// <param name="position">vector coordinate of center of button</param>
	/// <param name="size">vector size of button</param>
	/// <param name="label">string label on button</param>
	/// <param name="font">font of button label</param>
	public Button(string displayText, float fontScale, Font font, Vector2f position, Vector2f size) :
		base(position, size)
	{
		_text = QuickShape.Label(displayText, position, font, Color.Black);
		_text.Scale = new Vector2f(fontScale, fontScale);
		_text.Position = position; // new Vector2f(position.X + (size.X/2), position.Y + (size.Y/2));
		_text.Origin = _text.CenterOrigin();
		
		_button = new RectangleShape(size);
		_button.FillColor = new Color(200, 200, 200);
		_button.OutlineColor = Color.Black;
		_button.OutlineThickness = 1f;
		_button.Origin = _button.CenterOrigin(); //set transform origins of text/button to its center 
		_button.Position = position;
	}

	/// <summary>
	/// checks whether button has been pressed with mouse coordinates
	/// </summary>
	/// <param name="X">Mouse pressed x-coordinate</param>
	/// <param name="Y">Mouse pressed y-coordinate</param>
	/// <returns></returns>
	public bool IsPressed(int X, int Y)
    {
		FloatRect buttonBounds = _button.GetGlobalBounds();
		return (
			X > buttonBounds.Left && X < buttonBounds.Left + buttonBounds.Width &&
			Y > buttonBounds.Top && Y < buttonBounds.Top + buttonBounds.Height);
    }

	public delegate bool IsPressedDelegate(int X, int Y);

	public override void Render(RenderWindow window)
	{
		window.Draw(_button);
		window.Draw(_text);
	}

	public override void Load(IAssetBundle bundle)
	{
	}
}
