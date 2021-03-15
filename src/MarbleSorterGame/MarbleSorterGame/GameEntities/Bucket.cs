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
        private Color _requiredColor;
        private Weight _requiredWeight;

        private Text _capacityLabel;
        private RectangleShape _requiredColorLabel;

        public int Capacity;
        public int Accepted;

        /// Bucket that holds dropped marbles, containing requirements for color, weight and capacity
        public Bucket(Vector2f position, Vector2f size, Color requiredColor, Weight requiredWeight, int capacity) :  base (position, size)
        {
            _requiredColor = requiredColor;
            _requiredWeight = requiredWeight;
            Capacity = capacity;

            _bucket = new Sprite();

            Vector2f bucketSize = requiredWeight switch
            {
                Weight.Large => new Vector2f(250, 250),
                Weight.Medium => new Vector2f(200, 200),
                Weight.Small => new Vector2f(150, 150)
            };
            
            Size = bucketSize;
            _bucket.Position = Position - new Vector2f(0, Size.Y);
            
            _requiredColorLabel = new RectangleShape()
            {
                FillColor = requiredColor.ToSfmlColor(),
                Position = new Vector2f(Position.X + Size.X/2, Position.Y - Size.Y/2),
                Size = new Vector2f(40, 40)
            };
            
            _requiredColorLabel.Origin = new Vector2f(_requiredColorLabel.Size.X / 2, _requiredColorLabel.Size.Y / 2);
        }

        /// Insert marble into the bucket, return true/false depening on whether marble meets its requirements
        public bool InsertMarble(Marble m)
        {
            if (m.Color == _requiredColor && m.Weight == _requiredWeight && Accepted < Capacity)
            {
                Accepted++;
                _successSound.Play();
                _capacityLabel.DisplayedString = $"{Accepted}/{Capacity}";
                return true;
            }
            else
            {
                _capacityLabel.DisplayedString = $"{Accepted}/{Capacity}";
                _failSound.Play();
                return false;
            }
        }

        /// Draws the bucket onto render target RenderWindow
        public override void Render(RenderWindow window)
        {
           // base.Render(window);
            if (_bucket == null)
                return;
            
            window.Draw(_bucket);
            window.Draw(_requiredColorLabel);
            window.Draw(_capacityLabel);
        }
        
        /// Extracts bucket assets, such as texture and sound, from bundle
        public override void Load(IAssetBundle bundle)
        {
            _capacityLabel = new Text("", bundle.Font);
            // TODO: Find a better place for this
            _capacityLabel.Position = Position + new Vector2f(Size.X/2, 10);
            _capacityLabel.DisplayedString = $"0/{Capacity}";
            _capacityLabel.FillColor = SFML.Graphics.Color.Black;
            
            // _dropSound = bundle.BucketDrop;
            _successSound = bundle.BucketDropSuccess;
            _failSound = bundle.BucketDropFail;

            _bucket.Texture = bundle.BucketTexture;
            _bucket.Scale = ScaleEntity(bundle.BucketTexture);
        }
    }
}
