using SFML.Graphics;
using SFML.System;
using System;
using SFML.System;

namespace MarbleSorterGame
{
    public class PressureSensor : Sensor
    {
        Weight Value;

        public PressureSensor(Vector2f position, Vector2f size) : base(position, size)
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
