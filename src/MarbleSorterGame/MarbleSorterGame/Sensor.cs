using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public abstract class Sensor : GameEntity
    {
        private SoundBuffer _sensorActivateBuffer;
        private Sound _sensorActivate;
        public string SensorType;
        
        // Perform all IO with the PLC Simulator in SenseCallback handler
        public event EventHandler SenseCallback;
        
        protected Sensor(Vector2f position, Vector2f size) : base(position, size) { }

        public void PlayAudio()
        {
            //if activates
            //multiple sounds can be stored in the same buffer, option could be to pass in the buffer
            _sensorActivate.SoundBuffer = _sensorActivateBuffer;
            _sensorActivate.Play();
        }
    }
}
