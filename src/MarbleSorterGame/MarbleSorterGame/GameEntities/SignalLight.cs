using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{

    /// <summary>
    /// Visual indicator of certain toggle-able game entities (such as trapdoors and gates) being in a complete "on" or "off position
    /// </summary>
    public class SignalLight : GameEntity
    {
        private CircleShape _signalLight;
        public bool SignalState { get; protected set; }

        /// <summary>
        /// Constructor of signal light
        /// </summary>
        /// <param name="position">Global vector position</param>
        /// <param name="size">Global vector size</param>
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
        /// Loads assets for signal light
        /// </summary>
        /// <param name="bundle">reference to bundle object containing asset references</param>
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
