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

        private Vector2f defaultSize;
        private Vector2f defaultPosition;
        
        // constructor for motion sensor
        public MotionSensor(float maxLaserRange, Vector2f position, Vector2f size): base(position, size)
        {
            //_sensorSprite.Position = position;
            _laser = new RectangleShape();
            _laserRange = Math.Abs(maxLaserRange);
            _left = maxLaserRange < 0;
            
            // height will be 10% of the motion detector box
            defaultSize = new Vector2f(_laserRange, size.Y * 0.1f);
            _laser.Size = defaultSize;
            _laser.FillColor = Color.Red;

            float laserY = position.Y + (size.Y/2) - (_laser.Size.Y/2);

            if (_left)
            {
                defaultPosition = new Vector2f(position.X - _laserRange + size.X / 2, laserY);
                _laser.Position = defaultPosition;
            }
            else
                throw new ArgumentException("Motion Sensor: Detecting motion to right sensor not currently supported!");
        }

        // Render method for motion sensor
        public override void Render(RenderWindow window)
        {
            base.Render(window);
            window.Draw(_laser);
        }

        // Perform marble detection and adjust the appearance of the laser
        public void Update(IEnumerable<Marble> marbles)
        {
            if (marbles.Any(m => m.GlobalBounds.Intersects(_laser.GetGlobalBounds())))
            {
                Detected = true;
                _laser.FillColor = Color.Green;
                
                // Find rightmost intersecting marble
                var intersectingMarble = marbles.OrderByDescending(x => x.Position.X)
                    .FirstOrDefault(m => m.GlobalBounds.Intersects(_laser.GetGlobalBounds()));
                
                if (intersectingMarble != null)
                {
                    // Change laser size and position to rightmost intersecting marble
                    _laser.Size = new Vector2f(_laserRange - intersectingMarble.Position.X + 170, _laser.Size.Y);
                    _laser.Position = new Vector2f(defaultPosition.X + intersectingMarble.Position.X - 170, _laser.Position.Y);
                }
            }
            else
            {
                Detected = false;
                _laser.FillColor = Color.Red;
                
                // Reset laser to default size and position to check again
                _laser.Size = defaultSize;
                _laser.Position = defaultPosition;
            }
        }
    }
}
