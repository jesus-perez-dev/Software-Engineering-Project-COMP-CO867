using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    public class SignalLight : GameEntity
    {
        private CircleShape _signalLight;
        public bool SignalState { get; protected set; }

        public SignalLight(Vector2f position, Vector2f size) : base(position, size)
        {
            SignalState = true;

            _signalLight = new CircleShape(10.0f)
            {
                Position = position, 
                FillColor = Color.Black, 
                OutlineColor = Color.Black, 
                OutlineThickness = 5
            };
        }

        public void SetState(bool signalOn)
        {
            SignalState = signalOn;
        }
        
        public override void Load(IAssetBundle bundle)
        {
            
        }

        public override void Render(RenderWindow window)
        {
            _signalLight.FillColor = SignalState ? Color.Yellow : Color.Black;
            
            window.Draw(_signalLight);
        }
    }

}
