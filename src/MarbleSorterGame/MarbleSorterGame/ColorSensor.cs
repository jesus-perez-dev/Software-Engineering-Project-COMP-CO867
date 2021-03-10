using SFML.Graphics;
using System;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Sensor that detects color of marble rolling past it
    /// </summary>
    public class ColorSensor : Sensor
    {
        Color Value;

        public ColorSensor(Vector2f size, Vector2f position) : base(size, position)
        {
        }

        public override void Render(RenderWindow window)
        {
            throw new NotImplementedException();
        }

        public override void Load(IAssetBundle bundle)
        {
            throw new NotImplementedException();
        }
    }
}
