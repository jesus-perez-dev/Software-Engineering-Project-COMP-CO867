using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class SignalLight : GameEntity
    {
        private Sprite _signalLightOn;
        private Sprite _signalLightOff;
        public bool SignalState { get; protected set; }

        public SignalLight(Vector2f position, Vector2f size) : base(position, size)
        {
            SignalState = false;
        }

        public void Toggle()
        {
            SignalState = !SignalState;
        }

        public override void Load(IAssetBundle bundle)
        {
            _signalLightOff = new Sprite(bundle.SensorSignalOffTexture);
            _signalLightOn = new Sprite(bundle.SensorSignalOnTexture);

            _signalLightOff.Scale = ScaleEntity(bundle.SensorSignalOffTexture);
            _signalLightOn.Scale = ScaleEntity(bundle.SensorSignalOnTexture);

            _signalLightOff.Origin = CenterOrigin(bundle.SensorSignalOffTexture);
            _signalLightOn.Origin = CenterOrigin(bundle.SensorSignalOnTexture);
        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_signalLightOff);

            _signalLightOn.Position = Position;
            _signalLightOff.Position = Position;
            if (SignalState)
            {
                window.Draw(_signalLightOn);
            } else
            {
                window.Draw(_signalLightOff);
            }
        }
    }

}
