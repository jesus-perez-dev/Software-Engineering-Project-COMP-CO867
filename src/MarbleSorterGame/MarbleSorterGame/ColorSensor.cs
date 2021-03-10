using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Sensor that detects color of marble rolling past it
    /// </summary>
    public class ColorSensor : Sensor
    {
        Color Value;

        public ColorSensor(Vector2f position, Vector2f size) : base(position, size)
        {
        }

        //PLC logic here
        public override void Sense(Marble m)
        {
            this.Value = m.Color;
        }
    }
}
