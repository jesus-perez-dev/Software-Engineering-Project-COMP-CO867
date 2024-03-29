﻿using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    // Being inline with the conveyer, drops marbles onto buckets below
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
        public bool IsFullyOpen => RotationAngle == 90;
        public bool IsFullyClosed => RotationAngle == 0;

        public float RotationAngle => _trapdoor.Rotation;
        
        // Sets state of signal light to be turned on or off
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

        // Constructor for trapdoor
        public Trapdoor(Vector2f position, Vector2f size) : base(position, size)
        {
            _trapdoor = Box;
            var sizeWithoutOutline = Size;
            sizeWithoutOutline.Y -= 4;
            _trapdoor.Size = sizeWithoutOutline;
            _trapdoor.FillColor = Color.Red;
            _trapdoor.OutlineColor = Color.Black;
            _trapdoor.OutlineThickness = 2;
            var centeringWithOutline = position;
            centeringWithOutline.Y += 2;
            _trapdoor.Position = centeringWithOutline;

            _indicateConveyorDrop = new RectangleShape();
            var sizeIncludingOutline = Size;
            sizeIncludingOutline.Y += 4;
            _indicateConveyorDrop.Size = sizeIncludingOutline;
            _indicateConveyorDrop.FillColor = Color.White;
            _indicateConveyorDrop.Position = position;
        }

        // Rotational movement operation called on every tick
        public void Update()
        {
            float newRotation = Math.Min(Math.Max(_CLOSE_MAX_ANGLE, RotationAngle + _rotateStep), _OPEN_MAX_ANGLE);
            _trapdoor.Rotation = newRotation;
        }

        // Draws trapdoor
        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            if (_trapdoor == null)
                return;
            if (IsOpen)
                window.Draw(_indicateConveyorDrop);
            
            window.Draw(_trapdoor);
        }

        // Loads assets for trapdoor
        public override void Load(IAssetBundle bundle)
        {
            _trapDoorPeriod = bundle.GameConfiguration.TrapDoorPeriod;
        }
    }
}
