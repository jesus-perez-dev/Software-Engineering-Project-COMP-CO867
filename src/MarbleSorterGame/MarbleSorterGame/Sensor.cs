using System;
using SFML.Audio;

namespace MarbleSorterGame
{
    public abstract class Sensor : GameEntity
    {
        private Sound _activate;
        public String SensorType;

        public Sensor()
        {
        }

        //inherted members might also call override
        public virtual void Sense(Marble m)
        {
            //write to PLC
        }

        public event EventHandler SenseCallback;
        
    }
}
