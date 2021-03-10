using SFML.Audio;
using SFML.Graphics;
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

        //PLC logic here
        public override void Sense(Marble m)
        {
        }

    }
}
