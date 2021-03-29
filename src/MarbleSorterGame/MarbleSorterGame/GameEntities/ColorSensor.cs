using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    // Sensor that detects color of marble rolling past it
    public class ColorSensor : Sensor
    {
        Color Value;

        public ColorSensor(Vector2f size, Vector2f position) : base(size, position)
        {
            //_sensorSprite.Position = position;
        }
    }
}
