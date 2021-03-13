using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    public class Marble : GameEntity
    {
        private Sprite _marble;
        // private IntRect _marbleShape;
        private Texture _texture;

        public float Radius;
        public Color Color;
        public Weight Weight;

        public float RotationAngle { get; set; }
        public Vector2f Velocity { get; set; }

        /// <summary>
        /// Marble that rolls across the conveyer, contains data about color and weight that needs to be dropped in the right buckets
        /// </summary>
        /// <param name="color">Color of marble</param>
        /// <param name="weight">Weight of marble</param>
        public Marble(Vector2f position, Vector2f size, Color color, Weight weight) : base(position, size)
        {
            this.Color = color;
            this.Weight = weight;
            this.RotationAngle = 0f;

            // _marbleShape = new IntRect();
            // _marbleShape.Size = Size;
            // _marbleShape.Position = Position;

            _marble = new Sprite(_texture);
            _marble.Position = position;
            // _marble.TextureRect = _marbleShape;
        }

        /// <summary>
        /// Rotates the marble at a given angle
        /// </summary>
        /// <param name="angleChange"></param>
        public void Rotate(float angleChange)
        {
            this.RotationAngle += angleChange;
            _marble.Rotation = this.RotationAngle;
        }

        /// <summary>
        /// Adds a new velocity vector on top of the exist marble velocity vector
        /// </summary>
        /// <param name="velocityChange"></param>
        public void VelocityAdd(Vector2f velocityChange)
        {
            this.Velocity = new Vector2f(this.Velocity.X + velocityChange.X, this.Velocity.Y + velocityChange.Y);
        }

        /// <summary>
        /// Moves marble in direction of current marble velocity vector
        /// </summary>
        public void Move()
        {
            Position = new Vector2f(Position.X + Velocity.X, Position.Y + Velocity.Y);
        }
        
        /// <summary>
        /// Moves marble in direction of current marble velocity vector
        /// </summary>
        public void Move(float directionX, float directionY)
        {
            _marble.Position = new Vector2f( _marble.Position.X + directionX, _marble.Position.Y + directionY);
        }

        /// <summary>
        /// Draws marble onto render target RenderWindow
        /// </summary>
        /// <param name="window">RenderWindow for marble to be drawn onto</param>
        public override void Render(RenderWindow window)
        {
            window.Draw(_marble);
        }

        /// <summary>
        /// Extracts marble texture from bundle from chosen color and scales it correctly to marble dimension
        /// </summary>
        /// <param name="bundle"></param>
        public override void Load(IAssetBundle bundle)
        {
            switch (this.Color)
            {
                case Color.Red:
                    _texture = bundle.MarbleRedTexture;
                    break;

                case Color.Blue:
                    _texture = bundle.MarbleBlueTexture;
                    break;

                case Color.Green:
                    _texture = bundle.MarbleGreenTexture;
                    break;
            }

            _marble.Texture = _texture;

            //_marble.TextureRect = new IntRect(0, 0, 100, 100);
            _texture.Smooth = true;

            //center marble origin
            _marble.Origin = new Vector2f(_marble.Texture.Size.X / 2, _marble.Texture.Size.Y / 2);

            //scale texture to correct dimensions
            _marble.Scale = ScaleEntity(_texture);
        }
    }
}
