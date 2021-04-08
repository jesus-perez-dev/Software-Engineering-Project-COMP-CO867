using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MarbleSorterGame.Utilities;

namespace MarbleSorterGame.GameEntities
{
    // Sensor that detects movement of any marble that passes through it
    public class MotionSensor : Sensor
    {
        public bool Detected { get; set; }

        private RectangleShape _laser;
        
        // Whether the laser is facing left relative to the position of the sensor
        private bool _left;
        
        // Note: If laserRange, is negative it detects marbles from the left, vice-versa if positive
        private float _laserRange;

        private readonly Vector2f _defaultSize;
        private readonly Vector2f _defaultPosition;
        private readonly FloatRect _defaultLaserGlobalBounds;
        
        // constructor for motion sensor
        public MotionSensor(float maxLaserRange, Vector2f position, Vector2f size): base(position, size)
        {
            //_sensorSprite.Position = position;
            _laser = new RectangleShape();
            _laserRange = Math.Abs(maxLaserRange);
            _left = maxLaserRange < 0;
            
            // height will be 10% of the motion detector box
            _defaultSize = new Vector2f(_laserRange, size.Y * 0.1f);
            _laser.Size = _defaultSize;
            _laser.FillColor = Color.Red;

            float laserY = position.Y + (size.Y/2) - (_laser.Size.Y/2);

            if (_left)
            {
                _defaultPosition = new Vector2f(position.X - _laserRange + size.X / 2, laserY);
                _laser.Position = _defaultPosition;
            }
            else
                throw new ArgumentException("Motion Sensor: Detecting motion to right sensor not currently supported!");

            _defaultLaserGlobalBounds = _laser.GetGlobalBounds();
        }

        // Render method for motion sensor
        public override void Render(RenderWindow window)
        {
            base.Render(window);
            window.Draw(_laser);
        }

        // Perform marble detection and adjust the appearance of the laser
        // Gate needed to get proper laser size
        public void Update(IEnumerable<Marble> marbles, Gate gate)
        {
            float endOfLaser = _laser.Position.X + _laser.Size.X;
            
            // Find rightmost intersecting marble that does not pass the sensor and intersects laser default bounds
            Marble? intersectingMarble = marbles.OrderByDescending(m => m.Position.X)
                .Where(m => m.GlobalBounds.Intersects(_defaultLaserGlobalBounds))
                .FirstOrDefault(m => m.Position.X < endOfLaser);
            
            Detected = false;
            _laser.FillColor = Color.Red;
            _laser.Size = _defaultSize;
            _laser.Position = _defaultPosition;

            if (intersectingMarble != null)
            {
                Detected = true;
                _laser.FillColor = Color.Green;
                // Change laser size and position to rightmost intersecting marble
                float centerOfMarble = intersectingMarble.Position.X + intersectingMarble.Size.X / 2;
                _laser.Size = new Vector2f(_laserRange - centerOfMarble + gate.Position.X, _laser.Size.Y);
                _laser.Position = new Vector2f(_defaultPosition.X + centerOfMarble - gate.Position.X, _laser.Position.Y);
            }
        }
    }
}
