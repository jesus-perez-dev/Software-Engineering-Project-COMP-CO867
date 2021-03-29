using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    /// <summary>
    /// A signal light which indicates whether a gate/trapdoor are full open or closed
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
        /// Controls the state of the signal light
        /// </summary>
        /// <param name="signalOn">Whether the signal light should be on</param>
        public void SetState(bool signalOn)
        {
            SignalState = signalOn;
        }
        
        /// <summary>
        /// Loads any assets used (empty since we are using the built in circle shape)
        /// </summary>
        /// <param name="bundle">Assets to load</param>
        public override void Load(IAssetBundle bundle)
        {
            
        }

        /// <summary>
        /// Renders the signal light
        /// </summary>
        /// <param name="window">Window to draw the signal light</param>
        public override void Render(RenderWindow window)
        {
            _signalLight.FillColor = SignalState ? Color.Yellow : Color.Black;
            
            window.Draw(_signalLight);
        }
    }

}
