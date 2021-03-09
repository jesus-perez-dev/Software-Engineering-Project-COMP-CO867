using System;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public class Trapdoor : GameEntity
    {
        private RectangleShape _trapdoor;

        public bool Open { get; private set; }
        public float RotationAngle { get; set; }

        /// <summary>
        /// Rotatable part of the conveyer belt that drops marbles onto buckets below
        /// </summary>
        public Trapdoor()
        {
            _trapdoor = new RectangleShape();
            _trapdoor.Size = Dimensions;
            _trapdoor.Position = Position;

            this.RotationAngle = 0f;
            this.Open = false;
        }

        /// <summary>
        /// Rotates trapdoor at an angle
        /// </summary>
        /// <param name="angleChange"></param>
        public void Rotate(float angleChange)
        {
            this.RotationAngle += angleChange;
            _trapdoor.Rotation = RotationAngle;
        }

        /// <summary>
        /// Opens the trapdoor if already closed, and closes trapdoor if already open
        /// Open/close describe state of whether marble can 'roll' over trapdoor or not
        /// </summary>
        public void Toggle()
        {
            this.Open = !this.Open;
        }

        public void Render(RenderWindow window)
        {
            window.Draw(_trapdoor);
        }
    }
}
