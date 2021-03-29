using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{

    /// <summary>
    /// Visual indicator of certain toggle-able game entities (such as trapdoors and gates) being in a complete "on" or "off position
    /// </summary>
    public class SignalLight : GameEntity
    {
        /// <summary>
        /// The circle to display on the window
        /// </summary>
        private readonly CircleShape _signalLight;
        
        /// <summary>
        /// Whether the light should be on or off
        /// </summary>
        private bool SignalState { get; set; }

        /// <summary>
        /// Constructor for signal light
        /// </summary>
        /// <param name="position">The position on the window</param>
        /// <param name="size">The size of the light (unused)</param>
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

        /// <summary>
        /// Sets state of signal light to be turned on or off
        /// </summary>
        /// <param name="state">bool on or off</param>
        public void SetState(bool state)
        {
            SignalState = state;
        }
        
        /// <summary>
        /// Loads any assets used (empty since we are using the built in circle shape)
        /// </summary>
        /// <param name="bundle">Assets to load</param>
        public override void Load(IAssetBundle bundle)
        {
        }

        /// <summary>
        /// Draws signal light
        /// </summary>
        /// <param name="window">Current window to draw</param>
        public override void Render(RenderWindow window)
        {
            _signalLight.FillColor = SignalState ? Color.Yellow : Color.Black;
            
            window.Draw(_signalLight);
        }
    }

}
