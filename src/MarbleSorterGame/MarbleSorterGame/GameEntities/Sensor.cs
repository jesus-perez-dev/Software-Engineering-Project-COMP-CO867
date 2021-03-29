using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    /// <summary>
    /// Detects characteristics of marble such as weight, color and motion
    /// </summary>
    public abstract class Sensor : GameEntity
    {
        protected Sprite _sensorSprite;
        protected Sound _sensorActivate;
        public string SensorType;
        
        // Perform all IO with the PLC Simulator in SenseCallback handler
        public event EventHandler SenseCallback;
        
        /// <summary>
        /// Constructor for sensor
        /// </summary>
        /// <param name="position">Global vector position</param>
        /// <param name="size">Global vector size</param>
        protected Sensor(Vector2f position, Vector2f size) : base(position, size) {
            _sensorSprite = new Sprite();
            _sensorActivate = new Sound();

            _sensorSprite.Position = Position;
            // Console.WriteLine(_sensorSprite.Position);
        }

        /// <summary>
        /// Whether sensor has detected a marble passing through it
        /// </summary>
        /// <param name="m"></param>
        public void Sense(Marble m)
        {
        }
        
        public override Vector2f Size
        {
            get => Box.Size;
            set
            {
                _sensorSprite.Scale = RescaleSprite(value, _sensorSprite);
                Box.Size = value;
            }
        }

        public override Vector2f Position
        {
            get => Box.Position;
            set => _sensorSprite.Position = Box.Position = value;
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
        /// <param name="window">Current window to draw</param>
        public override void Render(RenderWindow window)
        {
            window.Draw(_sensorSprite);
        }

        /// <summary>
        /// extracts sensor assets from assets bundle
        /// </summary>
        /// <param name="bundle">reference to bundle object containing asset references</param>
        public override void Load(IAssetBundle bundle)
        {
            _sensorSprite.Texture = bundle.SensorTexture;
            _sensorSprite.Scale = ScaleEntity(bundle.SensorTexture);
        }
    }
}
