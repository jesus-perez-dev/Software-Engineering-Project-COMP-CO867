using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Sensor that detects color of marble rolling past it
    /// </summary>
    public class ColorSensor : Sensor
    {
        Color Value;

        public ColorSensor()
        {
        }

        //PLC logic here
        public override void Sense(Marble m)
        {
            this.Value = m.Color;
        }
    }
}
