using System;
using System.Dynamic;
using System.Runtime.CompilerServices;
using SFML.System;
using SFML.Graphics;
using MarbleSorterGame;

/// <summary>
/// Centered text with basic rectangle shape surrounding it
/// </summary>
public class Label : GameEntity
{
	protected Text _label { get; set; }
	protected RectangleShape _labelBox { get; set; }
	public String Text {
		get => _label.DisplayedString;
		set => _label.DisplayedString = value;
	}

	/// <summary>
	/// Generates a label with a rectangle shape surrounding it
	/// </summary>
	/// <param name="text"></param>
	/// <param name="font"></param>
	/// <param name="textSize"></param>
	/// <param name="textColor"></param>
	/// <param name="labelBoxColor"></param>
	/// <param name="labelPadding"></param>
	/// <param name="position"></param>
	/// <param name="size">Will not matter, size of label will be determined by text size and label padding</param>
	public Label(String text, Font font, uint textSize, float labelPadding, SFML.Graphics.Color textColor, SFML.Graphics.Color ?labelBoxColor, Vector2f position, Vector2f size) :
		base(position, size)
	{
		_label = new Text(text, font, textSize);
		_label.FillColor = textColor;

		//resize label according to its displayed text size
		FloatRect textBounds = _label.GetLocalBounds();
		Size = new Vector2f(textBounds.Width, textBounds.Height);
		_label.Origin = CenterOrigin();
		_label.Position = position;

		//resize labelbox to surround text
		_labelBox = new RectangleShape();
		if (!labelBoxColor.Equals(null))
        {
            _labelBox.FillColor = (SFML.Graphics.Color)labelBoxColor;
        }

		_labelBox.Size = new Vector2f(textBounds.Width + labelPadding * 2, textBounds.Height + labelPadding * 2);
		_labelBox.Origin = _label.Origin;
		_labelBox.Position = position;

		//resize of label to include label box
		FloatRect labelBounds = _labelBox.GetLocalBounds();
		Size = new Vector2f(labelBounds.Width, labelBounds.Height);
	}

    public override void Render(RenderWindow window)
    {
		window.Draw(_labelBox);
		window.Draw(_label);
    }

    public override void Load(IAssetBundle bundle)
    {
    }
}
