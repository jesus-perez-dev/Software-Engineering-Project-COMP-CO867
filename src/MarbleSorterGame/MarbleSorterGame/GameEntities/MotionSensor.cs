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
    /// <summary>
    /// Sensor that detects movement of any marble that passes through it
    /// </summary>
    public class MotionSensor : Sensor
    {
        public bool Detected { get; set; }

        private RectangleShape _laser;
        
        // Whether the lazer is facing left relative to the position of the sensor
        private bool _left;
        
        // Note: If laserRange, is negative it detects marbles from the left, vice-versa if positive
        private float _laserRange;
        
        public MotionSensor(float maxLaserRange, Vector2f position, Vector2f size): base(position, size)
        {
            //_sensorSprite.Position = position;
            _laser = new RectangleShape();
            _laserRange = Math.Abs(maxLaserRange);
            _left = maxLaserRange < 0;
            
            // height will be 10% of the motion detector box
            _laser.Size = new Vector2f(_laserRange, size.Y * 0.1f);
            _laser.FillColor = Color.Red;

            float laserY = position.Y + (size.Y/2) - (_laser.Size.Y/2);

            if (_left)
                _laser.Position = new Vector2f(position.X - _laserRange + size.X / 2, laserY);
            else
                throw new ArgumentException("Motion Sensor: Detecting motion to right sensor not currently supported!");
        }

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
            }
            else
            {
                Detected = false;
                _laser.FillColor = Color.Red;
            }

            // Get marble closest to sensor horizontally
            //var minDistance = withinRange.Min(m => Math.Abs(m.Position.X - Position.X));
            //var marble = withinRange.First(m => Math.Abs(m.Position.X - Position.X) == minDistance);
        }
    }
}
