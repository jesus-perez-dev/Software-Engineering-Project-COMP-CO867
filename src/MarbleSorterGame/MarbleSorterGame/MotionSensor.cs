using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// Sensor that detects movement of any marble that passes through it
    /// </summary>
    public class MotionSensor : Sensor
    {
        private int _maxCapacity;

        private int _maxCapacity;
        public MotionSensor(Vector2f position, Vector2f size): base(position, size)
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
