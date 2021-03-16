using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    public class Bucket : GameEntity
    {
        private Sprite _bucket;
        //private Sound _dropSound;
        private Sound _failSound;
        private Sound _successSound;
        private Color? _requiredColor;
        private Weight? _requiredWeight;

        private Text _capacityLabel;
        private Text _requiredSizeLabel;
        private CircleShape _requiredColorLabel;

        public int Capacity;
        public int Accepted;

        /// Bucket that holds dropped marbles, containing requirements for color, weight and capacity
        public Bucket(Vector2f position, Vector2f size, Color? requiredColor, Weight? requiredWeight, int capacity) :  base (position, size)
        {
            _requiredColor = requiredColor;
            _requiredWeight = requiredWeight;
            Capacity = capacity;
            
            _bucket = new Sprite();
            Size =  new Vector2f(MarbleSorterGame.WINDOW_WIDTH/19, MarbleSorterGame.WINDOW_HEIGHT/10); // Note: Largest marble cannot be larger than this
            _bucket.Position = Position - new Vector2f(0, Size.Y);
            
            _requiredColorLabel = new CircleShape()
            {
                FillColor = requiredColor?.ToSfmlColor() ?? SFML.Graphics.Color.White,
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = (requiredColor == null) ? 0 : 3,
                Position = new Vector2f(Position.X + Size.X/2, Position.Y - Size.Y/2),
                Radius = Size.X/8
            };
            
            _requiredColorLabel.Origin = new Vector2f(_requiredColorLabel.Radius, _requiredColorLabel.Radius);
        }

        /// Insert marble into the bucket, return true/false depening on whether marble meets its requirements
        public bool InsertMarble(Marble m)
        {
            bool marbleOk = (_requiredColor == null || m.Color == _requiredColor)  &&
                            (_requiredColor == null || m.Weight == _requiredWeight) &&
                            Accepted < Capacity;

            if (marbleOk)
            {
                Accepted++;
                _successSound.Play();
            }
            else
            {
                _failSound.Play();
            }

            _capacityLabel.DisplayedString = $"{Accepted}/{Capacity}";
            return marbleOk;
        }

        /// Draws the bucket onto render target RenderWindow
        public override void Render(RenderWindow window)
        {
           // base.Render(window);
            if (_bucket == null)
                return;
            
            window.Draw(_bucket);
            window.Draw(_requiredColorLabel);
            window.Draw(_requiredSizeLabel);
            window.Draw(_capacityLabel);
        }
        
        /// Extracts bucket assets, such as texture and sound, from bundle
        public override void Load(IAssetBundle bundle)
        {
            _capacityLabel = new Text($"0/{Capacity}", bundle.Font);
            // TODO: Find a better place for this
            _capacityLabel.Position = Position + new Vector2f(-1 * _capacityLabel.GetGlobalBounds().Width, (Size.Y/3f));
            _capacityLabel.FillColor = SFML.Graphics.Color.Black;
            
            _requiredSizeLabel = new Text(_requiredWeight?.ToGameLabel() ?? "", bundle.Font);
            _requiredSizeLabel.Position = Position + new Vector2f((-1) *_capacityLabel.GetGlobalBounds().Width, (Size.Y/3f) + _capacityLabel.GetGlobalBounds().Height + 5);
            _requiredSizeLabel.FillColor = SFML.Graphics.Color.Black;
            
            // _dropSound = bundle.BucketDrop;
            _successSound = bundle.BucketDropSuccess;
            _failSound = bundle.BucketDropFail;

            _bucket.Texture = bundle.BucketTexture;
            _bucket.Scale = ScaleEntity(bundle.BucketTexture);
        }
    }
}
