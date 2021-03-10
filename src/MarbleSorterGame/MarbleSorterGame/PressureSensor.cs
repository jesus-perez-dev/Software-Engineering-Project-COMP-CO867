using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class PressureSensor : Sensor
    {
        Weight Value;

        public PressureSensor(Vector2f position, Vector2f size) : base(position, size)
        {
        }


        //PLC logic here
        public override void Sense(Marble m)
        {
            this.Value = m.Weight;
        }
        public override void Load(IAssetBundle bundle)
        {
            throw new NotImplementedException();
        }

        public override void Render(RenderWindow window)
        {
            throw new NotImplementedException();
        }

    }
}
