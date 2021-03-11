using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class Gate : GameEntity
    {
        public bool OpenStatus;
        public bool Moving;

        private RectangleShape _gate;
        private float _gate_open_max;
        private float _gate_closed_max;

        public Gate(Vector2f position, Vector2f size) : base(position,size)
        {
            _gate = new RectangleShape(size);
            _gate.FillColor = SFML.Graphics.Color.Black;
            _gate.Position = position;

            _gate_open_max = position.Y - size.Y;
            _gate_closed_max = position.Y;

            Moving = false;
        }

        /// <summary>
        /// Toggles gate switches indicates whether it is open (true) or closed (false)
        /// </summary>
        public void Toggle()
        {
            OpenStatus = !OpenStatus;
        }

        public void Move(float moveChange)
        {
            _gate.Position = new Vector2f(_gate.Position.X, _gate.Position.Y + moveChange);
        }

        public void Open(float moveChange)
        {
            if (Moving) return;
            Moving = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                if (_gate.Position.Y <= _gate_open_max)
                {
                    timer.Stop();
                    timer.Dispose();

                    OpenStatus = true;
                    Moving = false;
                } else
                {
                    Move(-moveChange);
                }
            };
        }

        public void Close(float moveChange)
        {
            if (Moving) return;
            Moving = true;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Elapsed += (Object source, System.Timers.ElapsedEventArgs e) =>
            {
                if (_gate.Position.Y >= _gate_closed_max)
                {
                    timer.Stop();
                    timer.Dispose();

                    OpenStatus = true;
                    Moving = false;
                } else
                {
                    Move(moveChange);
                }
            };

        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_gate);
        }

        public override void Load(IAssetBundle bundle)
        {
        }

    }

}
