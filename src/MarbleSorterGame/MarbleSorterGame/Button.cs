using System;
using SFML.Graphics;
using SFML.System;

public class Button
{

	private RectangleShape _button;
	private Text _text;
	private String _label;
	private int _labelSize;

	private Vector2f _buttonPosition { get; set; }
	private Vector2f _buttonSize { get; set; }
	private Font _font { get; set; }
	private Color _labelColor { get; set; }

	public Button()
    {

    }

	/// <summary>
	/// full constructor of Button class, with all parameters
	/// </summary>
	/// <param name="buttonPosition">vector coordinate of center of button</param>
	/// <param name="buttonSize">vector size of button</param>
	/// <param name="label">string label on button</param>
	/// <param name="font">font of button label</param>
	public Button(Vector2f buttonPosition, Vector2f buttonSize, String label, int labelSize, Font font, Color labelColor)
	{
		_buttonPosition = buttonPosition;
		_buttonSize = buttonSize;
		_label = label;
		_labelSize = labelSize;
		_font = font;
		_labelColor = labelColor;

		_text = new Text(label, font, (uint)labelSize);
		_text.FillColor = labelColor;

		_button = new RectangleShape(buttonSize);

		//set transform origins of text/button to rectangle center 
		FloatRect buttonBounds = _button.GetLocalBounds();
		Vector2f buttonCenter = new Vector2f(
				buttonBounds.Left + buttonBounds.Width / 2.0f,
				buttonBounds.Top + buttonBounds.Height / 2.0f
			);
		_button.Origin = buttonCenter;

		FloatRect textBounds = _text.GetLocalBounds();
		Vector2f textCenter = new Vector2f(
				textBounds.Left + textBounds.Width / 2.0f,
				textBounds.Top + textBounds.Height / 2.0f
			);
		_text.Origin = textCenter;

		//set coordinates to center if text bounds do not exceed button bounds
		if (textBounds.Height > buttonBounds.Height || textBounds.Width > buttonBounds.Width)
        {
            //if text bounds exceeds button bounds, return some exception?
        }
        else
        {
            _button.Position = buttonPosition;
			_text.Position = buttonPosition;
        }
	}

	//try to make this generic to both Text and Shape, since both have GetLocalBounds?
	//or implement interface that has this entity centering tool
	public static void CenterOriginText(Text entity)
    {
		FloatRect entityBounds = entity.GetLocalBounds();
		Vector2f entityCenter = new Vector2f(
				entityBounds.Left + entityBounds.Width / 2.0f,
				entityBounds.Top + entityBounds.Height / 2.0f
			);

		entity.Origin = entityCenter;
    }

	/// <summary>
	/// checks whether button has been pressed with mouse coordinates
	/// </summary>
	/// <param name="X">Mouse pressed x-coordinate</param>
	/// <param name="Y">Mouse pressed y-coordinate</param>
	/// <returns></returns>
	public bool IsPressed(int X, int Y)
    {
		FloatRect buttonBounds = _button.GetLocalBounds();
		if (
			X > buttonBounds.Left && X < buttonBounds.Left + buttonBounds.Width &&
			Y > buttonBounds.Top && X < buttonBounds.Top + buttonBounds.Height)
        {
			return true;
        }
		return false;
    }

	public delegate bool IsPressedDelegate(int X, int Y);

    public void Draw(RenderWindow window)
    {
		window.Draw(_button);
		window.Draw(_text);
    }


}
