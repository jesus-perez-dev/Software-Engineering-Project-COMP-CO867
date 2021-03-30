using System;
using MarbleSorterGame.Enums;
using MarbleSorterGame.Utilities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using Color = MarbleSorterGame.Enums.Color;

namespace MarbleSorterGame.GameEntities
{
    // Container for marbles to drop onto, contains requirements that must be fulfilled to complete the marble sorter game
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
        public int TotalMarbles => TotalCorrect + TotalIncorrect;
        public int TotalCorrect;
        public int TotalIncorrect;
        public int Capacity;


        public Bucket(Vector2f position, Vector2f size, Color? requiredColor, Weight? requiredWeight, int capacity) :  base (position, size)
        {
            _requiredColor = requiredColor;
            _requiredWeight = requiredWeight;
            Capacity = capacity;
            
            _bucket = new Sprite();
            base.Size =  new Vector2f(MarbleSorterGame.WINDOW_WIDTH/19, MarbleSorterGame.WINDOW_HEIGHT/10); // Note: Largest marble cannot be larger than this
            _bucket.Position = Position - new Vector2f(0, Size.Y);

            _requiredColorLabel = new CircleShape()
            {
                FillColor = requiredColor?.ToSfmlColor() ?? SFML.Graphics.Color.White,
                OutlineColor = SFML.Graphics.Color.Black,
                OutlineThickness = (requiredColor == null) ? 0 : 2,
                Position = new Vector2f(Position.X + Size.X / 2, Position.Y - Size.Y / 2),
                Radius = Size.X / 8
            };
            
            _requiredColorLabel.Origin = new Vector2f(_requiredColorLabel.Radius, _requiredColorLabel.Radius);

            TotalCorrect = 0;
            TotalIncorrect = 0;
        }

        public override Vector2f Position
        {
            get => Box.Position;
            set => _bucket.Position = Box.Position = value;
        }
        
        public override Vector2f Size
        {
            get => Box.Size;
            set
            {
                _bucket.Scale = RescaleSprite(value, _bucket);
                Box.Size = value;
            }
        }

        // Insert marble into the bucket, return true/false depening on whether marble meets its requirements
        public bool InsertMarble(Marble m)
        {
            bool marbleOk = ValidateMarble(m);

            if (marbleOk)
            {
                TotalCorrect++;
                _successSound.Play();
            }
            else
            {
                TotalIncorrect++;
                _failSound.Play();
            }

            _capacityLabel.DisplayedString = $"{TotalMarbles}/{Capacity}";
            return marbleOk;
        }

        // Checks if marble that was dropped met the requirements
        public bool ValidateMarble(Marble m)
        {
            return (_requiredColor == null || m.Color == _requiredColor)  &&
                    (_requiredWeight == null || m.Weight == _requiredWeight) &&
                    TotalMarbles < Capacity;
        }

        // Draws the bucket onto render target RenderWindow
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
        
        // Extracts bucket assets, such as texture and sound, from bundle
        public override void Load(IAssetBundle bundle)
        {
            _capacityLabel = new Text($"0/{Capacity}", bundle.Font);
            _capacityLabel.Scale -= new Vector2f(0.1f, 0.1f);
            _capacityLabel.FillColor = SFML.Graphics.Color.Black;
            _capacityLabel.Position = Box.PositionRelative(Joint.Start, Joint.Start)
                .ShiftX(-_capacityLabel.GetGlobalBounds().Width - 10);
            
            _requiredSizeLabel = new Text(_requiredWeight?.ToGameLabel() ?? "", bundle.Font);
            _requiredSizeLabel.Scale -= new Vector2f(0.4f, 0.4f);
            _requiredSizeLabel.FillColor = SFML.Graphics.Color.Black;
            _requiredSizeLabel.Position = Box.PositionRelative(Joint.Start, Joint.End)
                .ShiftX(-_requiredSizeLabel.GetGlobalBounds().Width - 10)
                .ShiftY(-_requiredSizeLabel.GetGlobalBounds().Height - 10);
            
            // _dropSound = bundle.BucketDrop;
            _successSound = bundle.BucketDropSuccess;
            _failSound = bundle.BucketDropFail;

            _bucket.Texture = bundle.BucketTexture;
            _bucket.Scale = ScaleEntity(bundle.BucketTexture);
        }
    }
}
