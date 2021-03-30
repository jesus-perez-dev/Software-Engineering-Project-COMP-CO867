using SFML.Graphics;
using SFML.System;
using System;
using MarbleSorterGame.Enums;

namespace MarbleSorterGame.GameEntities
{
    // Sensor that detects weight of marble that passes through it
    public class PressureSensor : Sensor
    {
        Weight Value;

        // Constructor for pressure sensor
        public PressureSensor(Vector2f position, Vector2f size) : base(position, size)
        {
            //_sensorSprite.Position = position;
        }

    }
}
