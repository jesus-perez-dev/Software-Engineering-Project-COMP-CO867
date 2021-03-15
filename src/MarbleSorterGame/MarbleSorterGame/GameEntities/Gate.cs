using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class Gate : GameEntity
    {
        public bool IsFullyOpen => _gate.Position.Y <= _minGateY;
        public bool IsFullyClosed => _gate.Position.Y >= _maxGateY;
        
        // Assuming 60 FPS: Tick = 16.6ms per frame
        // Game ticks in 5 seconds = 5000 / 16.6 = 300 game ticks
        private static int StepTicks = 100;

        /// How much to in/decrement gate Y-position per-step
        private float _step;
        private RectangleShape _gate;

        private float _maxGateY;
        private float _minGateY;

        public void SetState(bool opening)
        {
            if (opening)
                _step = - (Size.Y / StepTicks);
            else
                _step = (Size.Y / StepTicks);
        }

        public Gate(Vector2f position, Vector2f size) : base(position, size)
        {
            _gate = new RectangleShape(size);
            _gate.FillColor = SFML.Graphics.Color.Black;
            _gate.Position = position;
            
            // Assuming 60 FPS: Tick = 16.6ms per frame
            // Game ticks in 5 seconds = 5000 / 16.6 = 300 game ticks
            _step = size.Y / StepTicks;
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
        }
    }
}
