using SFML.Graphics;
using SFML.System;
using System;
using System.Linq;

namespace MarbleSorterGame.GameEntities
{
    public class Gate : GameEntity
    {
        // normal color of gate
        private static readonly Color DefaultGateColor = Color.Black;
        // Color of gate when a marble is stuck underneath
        private static readonly Color JammedGateColor = Color.Red;

        private readonly float _maxGateY;
        private readonly float _minGateY;
        public bool IsFullyOpen => _gate.Position.Y <= _minGateY;
        public bool IsFullyClosed => _gate.Position.Y >= _maxGateY;
        public bool IsOpening { get; private set; }
        public bool IsClosing => !IsOpening && !IsFullyOpen && !IsFullyClosed;
        private float Step => Size.Y / GameLoop.FPS / _gatePeriod * (IsOpening ? -1 : 1);

        /// How much to in/decrement gate Y-position per-step
        private readonly RectangleShape _gate;
        
         // How many seconds takes for the gate to open. By default, 30 seconds, but source this from Config in .Load()
        private float _gatePeriod = 30f;

        public void SetState(bool opening)
        {
            IsOpening = opening;
        }

        public Gate(Vector2f position, Vector2f size) : base(position, size)
        {
            _gate = Box;
            _gate.FillColor = DefaultGateColor;
            _gate.Position = position;
            
            _maxGateY = position.Y;
            _minGateY = position.Y - size.Y;
        }

        public void Update(Marble[] marbles)
        {
            if (marbles.Any(InsideHorizontal) && IsClosing)
            {
                // If there is a marble anywhere underneath the gate, change to red color but dont move anywhere
                _gate.FillColor = JammedGateColor; 
            }
            else
            {
                // Move the gate up or down appropriately (based on Step value)
                _gate.FillColor = DefaultGateColor;
                float newY = Math.Min(Math.Max(_minGateY, _gate.Position.Y + Step), _maxGateY);
                _gate.Position = new Vector2f(_gate.Position.X, newY);
            }
        }

        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            window.Draw(_gate);
        }

        public override void Load(IAssetBundle bundle)
        {
            _gatePeriod = bundle.GameConfiguration.GatePeriod;
        }
    }
}
