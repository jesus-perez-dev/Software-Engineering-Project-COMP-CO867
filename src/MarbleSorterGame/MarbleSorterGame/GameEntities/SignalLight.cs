using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{

    // Visual indicator of certain toggle-able game entities (such as trapdoors and gates) being in a complete "on" or "off position
    public class SignalLight : GameEntity
    {
        // The circle to display on the window
        private readonly CircleShape _signalLight;
        
        // Whether the light should be on or off
        private bool SignalState { get; set; }

        // Constructor for signal light
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

        // Sets state of signal light to be turned on or off
        public void SetState(bool state)
        {
            SignalState = state;
        }
        
        // Loads any assets used (empty since we are using the built in circle shape)
        public override void Load(IAssetBundle bundle)
        {
        }

        // Draws signal light
        public override void Render(RenderWindow window)
        {
            _signalLight.FillColor = SignalState ? Color.Yellow : Color.Black;
            
            window.Draw(_signalLight);
        }
    }

}
