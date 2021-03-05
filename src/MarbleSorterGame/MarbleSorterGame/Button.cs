using System;
using SFML.Graphics;
using SFML.System;
using UtilityCentering;

public class Button
{

	public Label Label;
    private RectangleShape _button;
	private Vector2f _buttonPosition;
	private Vector2f _buttonSize;

	/// <summary>
	/// full constructor of Button class, with all parameters
	/// </summary>
	/// <param name="buttonPosition">vector coordinate of center of button</param>
	/// <param name="buttonSize">vector size of button</param>
	/// <param name="label">string label on button</param>
	/// <param name="font">font of button label</param>
	public Button(Vector2f buttonPosition, Vector2f buttonSize, Label label)
	{
		_buttonPosition = buttonPosition;
		_buttonSize = buttonSize;
		Label = label;

		_button = new RectangleShape(buttonSize);

		//set transform origins of text/button to its center 
		_button.Origin = _button.CenterOrigin();

		var textBounds = Label.Text.GetGlobalBounds();
		var buttonBounds = _button.GetGlobalBounds();
		//set coordinates to center if text bounds do not exceed button bounds
		if (textBounds.Height > buttonBounds.Height || textBounds.Width > buttonBounds.Width)
        {
            //if text bounds exceeds button bounds, return some exception?
        }
        else
        {
            _button.Position = buttonPosition;
			Label.Text.Position = buttonPosition;
        }
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

    public void Draw(RenderWindow window)
    {
		window.Draw(_button);
		Label.Draw(window);
    }


}
