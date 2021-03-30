using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    // Detects characteristics of marble such as weight, color and motion
    public abstract class Sensor : GameEntity
    {
        protected Sprite _sensorSprite;
        protected Sound _sensorActivate;
        public string SensorType;
        
        // Perform all IO with the PLC Simulator in SenseCallback handler
        public event EventHandler SenseCallback;
        
        // Constructor for sensor
        protected Sensor(Vector2f position, Vector2f size) : base(position, size) {
            _sensorSprite = new Sprite();
            _sensorActivate = new Sound();

            _sensorSprite.Position = Position;
            // Console.WriteLine(_sensorSprite.Position);
        }

        // Whether sensor has detected a marble passing through it
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

        // Plays sensor activation sound
        public void playActivateSound()
        {
            _sensorActivate.Play();
        }

        // Draws sensor onto render target window
        public override void Render(RenderWindow window)
        {
            window.Draw(_sensorSprite);
        }

        // extracts sensor assets from assets bundle
        public override void Load(IAssetBundle bundle)
        {
            _sensorSprite.Texture = bundle.SensorTexture;
            _sensorSprite.Scale = ScaleEntity(bundle.SensorTexture);
        }
    }
}
