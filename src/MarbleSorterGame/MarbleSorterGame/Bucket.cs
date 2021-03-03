using System;
using SFML.Audio;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public class Bucket
    {
        private Texture _texture;
        private Sound _failSound;
        private Sound _successSound;
        private Color _requiredColor;
        private Weight _requiredWeight;
        public int Capacity;
        public int Accepted;
        public bool Fail;

        public Bucket(Color requiredColor, Weight requiredWeight, int capacity)
        {
            this._requiredColor = requiredColor;
            this._requiredWeight = requiredWeight;
            this.Capacity = capacity;
        }
    }
}
