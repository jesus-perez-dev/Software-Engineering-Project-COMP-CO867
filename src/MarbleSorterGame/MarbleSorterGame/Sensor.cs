using System;
using SFML.Audio;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public abstract class Sensor : GameEntity
    {
        private Sprite _sensorSprite;
        private SoundBuffer _sensorActivateBuffer;
        private Sound _sensorActivate;

        public String SensorType;

        public Sensor()
        {
        }

        //inherted members might also call override
        public virtual void Sense(Marble m)
        {
            //write to PLC
        }

        public void Render(RenderWindow window)
        {
            window.Draw(_sensorSprite);
        }
        public void Load(IAssetBundle bundle)
        {
            _sensorSprite.Texture = bundle.SensorTexture;
            //_sensorActivateBuffer = bundle.SensorActivateBuffer;
        }

        public void PlayAudio()
        {
            //if activates
            //multiple sounds can be stored in the same buffer, option could be to pass in the buffer
            _sensorActivate.SoundBuffer = _sensorActivateBuffer;
            _sensorActivate.Play();
        }

        public event EventHandler SenseCallback;
        
    }
}
