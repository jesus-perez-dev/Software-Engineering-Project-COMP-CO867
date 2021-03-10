using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public class Bucket : GameEntity
    {
        private Sprite _bucket;
        private Sound _dropSound;
        private Sound _failSound;
        private Sound _successSound;
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
        public Bucket(Vector2f position, Vector2f size, Color requiredColor, Weight requiredWeight, int capacity) :  base (position, size)
        {
            _requiredColor = requiredColor;
            _requiredWeight = requiredWeight;
            Capacity = capacity;
        }

        public Bucket()
        {
        }


        /// <summary>
        /// Plays sound of marble dropping into the bucket
        /// </summary>
        public void PlayDropSound()
        {
            _dropSound.Play();
        }

        /// <summary>
        /// Plays sound that indicates marble correctly fits all of the bucket requirements
        /// </summary>
        public void PlaySuccessSound()
        {
            _successSound.Play();
        }

        /// <summary>
        /// Plays sound that indicates marble did not fit bucket requirements
        /// </summary>
        public void PlayFailSound()
        {
            _failSound.Play();
        }

        /// <summary>
        /// Draws the bucket onto render target RenderWindow
        /// </summary>
        /// <param name="window">RenderWindow for marble to be drawn onto</param>
        public override void Render(RenderWindow window)
        {
            if (_bucket == null)
                return;

            _bucket.Position = Position;
            window.Draw(_bucket);
        }

        /// <summary>
        /// Extracts bucket assets, such as texture and sound, from bundle
        /// </summary>
        /// <param name="bundle"></param>
        public override void Load(IAssetBundle bundle)
        {
            _dropSound = bundle.BucketDrop;
            _successSound = bundle.BucketDropSuccess;
            _failSound = bundle.BucketDropFail;

            _bucket = new Sprite(bundle.BucketTexture);
            _bucket.Scale = ScaleEntity(bundle.BucketTexture);
        }

    }
}
