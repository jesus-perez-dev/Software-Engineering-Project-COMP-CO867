using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public class Trapdoor : GameEntity
    {
        private const float _OPEN_MAX_ANGLE = 90f;
        private const float _CLOSE_MAX_ANGLE = 0f;
        private const float _DROP_ANGLE = 45f;

        private RectangleShape _trapdoor;
        private RectangleShape _indicateConveyorDrop;
        private bool _dropPossible;
        private bool _moving;

        public bool OpenStatus { get; private set; }
        public float RotationAngle { get; set; }

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

            RotationAngle = 0f;
            OpenStatus = false;
            _dropPossible = false;
            _moving = false;
        }

        public Trapdoor()
        {
        }

        /// <summary>
        /// Rotates trapdoor at an angle
        /// </summary>
        /// <param name="angleChange"></param>
        public void Rotate(float angleChange)
        {
            RotationAngle += angleChange;
            _trapdoor.Rotation = RotationAngle;

            Console.WriteLine(RotationAngle);
        }

        public void Open(float angleChange)
        {
            if (_moving) return;
            _moving = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                if (RotationAngle >= _OPEN_MAX_ANGLE)
                {
                    timer.Stop();
                    timer.Dispose();

                    OpenStatus = true;
                    _moving = false;
                } else
                {
                    Rotate(angleChange);
                    checkDropPossible();
                }
            };
        }

        public void Close(float angleChange)
        {
            if (_moving) return;
            _moving = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                if (RotationAngle <= _CLOSE_MAX_ANGLE)
                {
                    timer.Stop();
                    timer.Dispose();

                    OpenStatus = false;
                    _moving = false;
                } else
                {
                    Rotate(-angleChange);
                    checkDropPossible();
                }
            };
        }
        private void checkDropPossible()
        {
            if(RotationAngle > _DROP_ANGLE && RotationAngle <= _OPEN_MAX_ANGLE + 1f)
            {
                _dropPossible = true;
            } else
            {
                _dropPossible = false;
            }
        }

        /// <summary>
        /// Opens the trapdoor if already closed, and closes trapdoor if already open
        /// Open/close describe state of whether marble can 'roll' over trapdoor or not
        /// </summary>
        public void Toggle()
        {
            OpenStatus = !OpenStatus;
        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_trapdoor);
            if (_dropPossible)
            {
                window.Draw(_indicateConveyorDrop);
            }
        }

        public override void Load(IAssetBundle bundle)
        {
        }
    }
}
