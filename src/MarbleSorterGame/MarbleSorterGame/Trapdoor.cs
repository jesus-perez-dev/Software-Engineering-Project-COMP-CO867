using System;
using SFML.Graphics;

namespace MarbleSorterGame
{
    public class Trapdoor
    {
        private Texture _texture;
        public bool Open;

        public Trapdoor()
        {
            this.Open = false;

        }

        public void Toggle()
        {
            if (this.Open)
            {
                this.Open = false;
            }
            else
            {
                this.Open = true;
            }
        }


        public void Render(RenderWindow window)
        {
        }

        public void Load(IAssetBundle bundle)
        {
            //trapdoor should be just line

        }
    }
}
