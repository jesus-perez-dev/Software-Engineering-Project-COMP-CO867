using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    /// <summary>
    /// Sensor that detects weight of marble that passes through it
    /// </summary>
    public class PressureSensor : Sensor
    {
        Weight Value;

        public PressureSensor(Vector2f position, Vector2f size) : base(position, size)
        {
            //_sensorSprite.Position = position;
        }

    }
}
