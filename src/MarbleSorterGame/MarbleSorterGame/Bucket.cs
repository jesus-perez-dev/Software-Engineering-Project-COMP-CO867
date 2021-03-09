using System;
using SFML.Audio;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public class Bucket : GameEntity
    {
        private Sprite _bucket;
        private Texture _bucketTexture;
        private Sound _dropSound;

        private Color _requiredColor;
        private Weight _requiredWeight;

        public int Capacity;
        public int Accepted;
        public bool Fail { get; }

        /// <summary>
        /// Bucket that holds dropped marbles, containing requirements for color, weight and capacity
        /// </summary>
        /// <param name="requiredColor"></param>
        /// <param name="requiredWeight"></param>
        /// <param name="capacity"></param>
        public Bucket(Color requiredColor, Weight requiredWeight, int capacity)
        {
            this._requiredColor = requiredColor;
            this._requiredWeight = requiredWeight;
            this.Capacity = capacity;

            this._bucket = new Sprite();
        }

        /// <summary>
        /// Plays sound of marble dropping into the bucket
        /// </summary>
        public void playDropSound()
        {
            _dropSound.Play();
        }

        /// <summary>
        /// Draws the bucket onto render target RenderWindow
        /// </summary>
        /// <param name="window">RenderWindow for marble to be drawn onto</param>
        public new void Render(RenderWindow window)
        {
            window.Draw(_bucket);
        }

        /// <summary>
        /// Extracts bucket assets, such as texture and sound, from bundle
        /// </summary>
        /// <param name="bundle"></param>
        public new void Load(IAssetBundle bundle)
        {
            _bucketTexture = bundle.BucketTexture;
            _dropSound = bundle.BucketDrop;

            _bucket.Texture = _bucketTexture;
        }
    }
}
