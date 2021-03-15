using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    public class Trapdoor : GameEntity
    {
        private const float _OPEN_MAX_ANGLE = 90f;
        private const float _CLOSE_MAX_ANGLE = 0f;
        private const float _DROP_ANGLE = 45f;
        // At 60fps, 5 seconds = 300 game ticks
        private const float _ROTATE_STEP_TICKS = 300;
        private float _rotateStep;
        
        private RectangleShape _trapdoor;
        private RectangleShape _indicateConveyorDrop;
        
        public bool IsOpen => RotationAngle > _DROP_ANGLE && RotationAngle <= _OPEN_MAX_ANGLE + 1f;
        public float RotationAngle => _trapdoor.Rotation;
        
        public void SetState(bool opening)
        {
            if (opening)
                _rotateStep = (_OPEN_MAX_ANGLE / _ROTATE_STEP_TICKS);
            else
                _rotateStep = - (_OPEN_MAX_ANGLE / _ROTATE_STEP_TICKS);
        }

        /// <summary>
        /// Rotatable part of the conveyer belt that drops marbles onto buckets below
        /// </summary>
        public Trapdoor(Vector2f position, Vector2f size) : base(position, size)
        {
            _trapdoor = new RectangleShape();
            _trapdoor.Size = Size;
            _trapdoor.FillColor = SFML.Graphics.Color.Red;
            _trapdoor.Position = position;

            _indicateConveyorDrop = new RectangleShape();
            _indicateConveyorDrop.Size = Size;
            _indicateConveyorDrop.FillColor = new SFML.Graphics.Color(181, 181, 181);
            _indicateConveyorDrop.Position = position;
        }

        public void Update()
        {
            float newRotation = Math.Min(Math.Max(_CLOSE_MAX_ANGLE, RotationAngle + _rotateStep), _OPEN_MAX_ANGLE);
            _trapdoor.Rotation = newRotation;
        }

        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            if (_trapdoor == null)
                return;
            
            window.Draw(_trapdoor);
            if (IsOpen)
                window.Draw(_indicateConveyorDrop);
        }

        public override void Load(IAssetBundle bundle)
        {
        }
    }
}
