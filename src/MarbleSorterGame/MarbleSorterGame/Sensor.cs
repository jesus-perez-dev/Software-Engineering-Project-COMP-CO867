using System;
using SFML.Audio;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public abstract class Sensor : GameEntity
    {
        private Sprite _sensorSprite;
        private Sound _sensorActivate;

        //make enum?
        public String SensorType;

        public Sensor()
        {
        }

        //inherted members might also call override
        public virtual void Sense(Marble m)
        {
            //write to PLC
        }


        /// <summary>
        /// Plays sensor activation sound
        /// </summary>
        public void playActivateSound()
        {
            _sensorActivate.Play();
        }

        /// <summary>
        /// Draws sensor onto render target window
        /// </summary>
        /// <param name="window"></param>
        public new void Render(RenderWindow window)
        {
            window.Draw(_sensorSprite);
        }

        /// <summary>
        /// extracts sensor assets from assets bundle
        /// </summary>
        /// <param name="bundle"></param>
        public new void Load(IAssetBundle bundle)
        {
            _sensorSprite.Texture = bundle.SensorTexture;
            _sensorActivate = bundle.SensorActivate;
        }

        public event EventHandler SenseCallback;
        
    }
}
