using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    public class Gate : GameEntity
    {
        public bool IsFullyOpen => _gate.Position.Y <= _minGateY;
        public bool IsFullyClosed => _gate.Position.Y >= _maxGateY;
        
        private float _gatePeriod = 30f; // Default: Take 30 seconds to open
        
        private float Step => Size.Y / GameLoop.FPS / _gatePeriod;

        /// How much to in/decrement gate Y-position per-step
        private float _step;
        private RectangleShape _gate;

        private float _maxGateY;
        private float _minGateY;

        public void SetState(bool opening)
        {
            if (opening)
                _step = Step * -1;
            else
                _step = Step;
        }

        public Gate(Vector2f position, Vector2f size) : base(position, size)
        {
            _gate = Box;
            _gate.FillColor = SFML.Graphics.Color.Black;
            _gate.Position = position;
            
            _step = size.Y / Step;
            _minGateY = position.Y - size.Y;
            _maxGateY = position.Y;
        }

        public void Update()
        {
            float newY = Math.Min(Math.Max(_minGateY, _gate.Position.Y + _step), _maxGateY);
            _gate.Position = new Vector2f(_gate.Position.X, newY);
            Position = _gate.Position;
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
