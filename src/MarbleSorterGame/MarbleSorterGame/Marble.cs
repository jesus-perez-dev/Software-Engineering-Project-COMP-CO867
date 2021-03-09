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
        private RectangleShape _marbleShape;
        private Texture _texture;

        public float Radius;
        public Color Color;
        public Weight Weight;

        public float RotationAngle { get; set; }
        public Vector2f Velocity { get; set; }

        public Marble(float radius, Color color, Weight weight)
        {
            this.Radius = radius;
            this.Color = color;
            this.Weight = weight;
            this.RotationAngle = 0f;

            _marbleShape = new RectangleShape();
            _marbleShape.Size = Dimensions;
            _marbleShape.Position = Position;

            _marble = new Sprite();
            //_marble.Origin = new Vector2f(Dimensions.X / 2f, Dimensions.Y / 2f);
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
        /// Draws marble onto render target RenderWindow
        /// </summary>
        /// <param name="window">RenderWindow for marble to be drawn onto</param>
        public void Render(RenderWindow window)
        {
            window.Draw(_marble);
        }

        /// <summary>
        /// Extracts marble texture from bundle from chosen color and scales it correctly to marble dimension
        /// </summary>
        /// <param name="bundle"></param>
        public void Load(IAssetBundle bundle)
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

            //scale texture to correct dimensions
            Vector2u textureSize = _texture.Size;
            Vector2f scaleRatio = new Vector2f(Dimensions.X / textureSize.X, Dimensions.Y / textureSize.Y);
            _marble.Scale = scaleRatio;
        }
    }
}
