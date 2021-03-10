using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public class Bucket : GameEntity
    {
        private Sprite _sprite;
        private Sound _failSound;
        private Sound _successSound;
        private Color _requiredColor;
        private Weight _requiredWeight;
        public int Capacity;
        public int Accepted;
        public bool Fail;

        public Bucket(Color requiredColor, Weight requiredWeight, int capacity, Vector2f position, Vector2f size) 
            : base(position, size)
        {
            _requiredColor = requiredColor;
            _requiredWeight = requiredWeight;
            Capacity = capacity;
        }

        public override void Render(RenderWindow window)
        {
            if (_sprite == null)
                return;
            
            window.Draw(_sprite);
        }

        public override void Load(IAssetBundle bundle)
        {
            _sprite = new Sprite(bundle.BucketTexture);
        }
    }
}
