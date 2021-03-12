using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public abstract class Sensor : GameEntity
    {
        protected Sprite _sensorSprite;
        protected Sound _sensorActivate;
        public string SensorType;
        
        // Perform all IO with the PLC Simulator in SenseCallback handler
        public event EventHandler SenseCallback;
        
        protected Sensor(Vector2f position, Vector2f size) : base(position, size) {
            _sensorSprite = new Sprite();
            _sensorActivate = new Sound();

            _sensorSprite.Position = Position;
            // Console.WriteLine(_sensorSprite.Position);
        }

        //inherted members might also call override
        public void Sense(Marble m)
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
        public override void Render(RenderWindow window)
        {
            window.Draw(_sensorSprite);
        }

        /// <summary>
        /// extracts sensor assets from assets bundle
        /// </summary>
        /// <param name="bundle"></param>
        public override void Load(IAssetBundle bundle)
        {
            _sensorSprite.Texture = bundle.SensorTexture;
            _sensorSprite.Scale = ScaleEntity(bundle.SensorTexture);

            _sensorActivate = bundle.SensorActivate;
        }
    }
}
