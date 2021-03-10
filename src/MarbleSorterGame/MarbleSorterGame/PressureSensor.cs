using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class PressureSensor : Sensor
    {
        Weight Value;

        public PressureSensor()
        {
        }

        public PressureSensor(Vector2f position, Vector2f size) : base(position, size)
        {
        }


    }
}
