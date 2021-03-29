using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    /// <summary>
    /// Being inline with the conveyer, drops marbles onto buckets below
    /// </summary>
    public class Trapdoor : GameEntity
    {
        private const float _OPEN_MAX_ANGLE = 90f;
        private const float _CLOSE_MAX_ANGLE = 0f;
        private const float _DROP_ANGLE = 45f;
        // At 60fps, 5 seconds = 300 game ticks
        private float _rotateStep;

        private float _trapDoorPeriod = 30f; // Default: 30 seconds to rotate

        private float RotateStep => _OPEN_MAX_ANGLE / GameLoop.FPS / _trapDoorPeriod;
        
        private RectangleShape _trapdoor;
        private RectangleShape _indicateConveyorDrop;
        
        public bool IsOpen => RotationAngle > _DROP_ANGLE && RotationAngle <= _OPEN_MAX_ANGLE + 1f;
        public bool IsFullyOpen => RotationAngle == 0;
        public bool IsFullyClosed => RotationAngle == 90;

        public float RotationAngle => _trapdoor.Rotation;
        

        /// <summary>
        /// Sets state of signal light to be turned on or off
        /// </summary>
        /// <param name="opening">Whether trapdoor should be moving in opening/closing</param>
        public void SetState(bool opening)
        {
            if (opening)
                _rotateStep = RotateStep;
            else
                _rotateStep = RotateStep * -1;
        }
        
        public override Vector2f Size
        {
            get => Box.Size;
            set => Box.Size = _trapdoor.Size = _indicateConveyorDrop.Size = value;
        }

        public override Vector2f Position
        {
            get => Box.Position;
            set => Box.Position = _trapdoor.Position = _indicateConveyorDrop.Position = value;
        }

        /// <summary>
        /// Constructor for trapdoor
        /// </summary>
        /// <param name="position">Global vector position</param>
        /// <param name="size">Global vector size</param>
        public Trapdoor(Vector2f position, Vector2f size) : base(position, size)
        {
            _trapdoor = Box;
            _trapdoor.Size = Size;
            _trapdoor.FillColor = SFML.Graphics.Color.Red;
            _trapdoor.Position = position;

            _indicateConveyorDrop = new RectangleShape();
            _indicateConveyorDrop.Size = Size;
            _indicateConveyorDrop.FillColor = new SFML.Graphics.Color(181, 181, 181);
            _indicateConveyorDrop.Position = position;
        }

        /// <summary>
        /// Rotational movement operation called on every tick
        /// </summary>
        public void Update()
        {
            float newRotation = Math.Min(Math.Max(_CLOSE_MAX_ANGLE, RotationAngle + _rotateStep), _OPEN_MAX_ANGLE);
            _trapdoor.Rotation = newRotation;
        }

        /// <summary>
        /// Draws trapdoor
        /// </summary>
        /// <param name="window">Current window to draw</param>
        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            if (_trapdoor == null)
                return;
            
            window.Draw(_trapdoor);
            if (IsOpen)
                window.Draw(_indicateConveyorDrop);
        }

        /// <summary>
        /// Loads assets for trapdoor
        /// </summary>
        /// <param name="bundle">reference to bundle object containing asset references</param>
        public override void Load(IAssetBundle bundle)
        {
            _trapDoorPeriod = bundle.GameConfiguration.TrapDoorPeriod;
        }
    }
}
