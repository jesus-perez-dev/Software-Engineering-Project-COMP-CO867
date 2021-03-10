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
        private CircleShape _marbleShape;
        private Texture _texture;

        public float Radius;
        public Color Color;
        public Weight Weight;

        //parameter should be int instead? otherwise main game would have to reference enum class instead of this one
        public Marble(float radius, Color color, Weight weight, Vector2f position, Vector2f size) : base(position, size)
        {
            this.Radius = radius;
            //add enum.isDefined for more strict type check? color/weight must be < 3
            this.Color = color;
            this.Weight = weight;

            _marbleShape = new CircleShape(Radius);
        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_marble);
        }

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

            //update marble textures so they look circular
            _marble.TextureRect = new IntRect(0, 0, 100, 100);
            //_texture.Update();
            _marble.Texture = _texture;
        }
    }
}
