using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    public class PressureSensor : Sensor
    {
        Weight Value;

        public PressureSensor()
        {

        }

        //PLC logic here
        public override void Sense(Marble m)
        {
            this.Value = m.Weight;
        }
    }
}
