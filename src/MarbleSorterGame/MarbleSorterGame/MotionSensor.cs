using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Sensor that detects movement of any marble that passes through it
    /// </summary>
    public class MotionSensor : Sensor
    {

        private int _maxCapacity;

        public MotionSensor()
        {
        }

        public MotionSensor(Vector2f position, Vector2f size): base(position, size)
        {
        }

    }
}
