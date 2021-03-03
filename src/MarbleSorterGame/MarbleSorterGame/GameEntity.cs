using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    public class GameEntity
    {
        public String Name { get; set; }

        public int X;
        public int Y;
        public int Width;
        public int Height;

        public GameEntity()
        {

        }

        public void SetDimensions(int width, int height)
        {
            this.Width = width;
            this.Height = height;
        }

        public void SetPosition(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public void setSpacial(int x, int y, int width, int height)
        {
            this.SetPosition(x, y);
            this.SetDimensions(width, height);
        }

        public bool Overlaps(GameEntity entity)
        {
            //use .intersects and .getGlobalBounds (only for Shapes)
            return false;
        }

        public bool Inside(GameEntity entity)
        {
            //if inside bucket?
            return false;
        }

        public void Render(RenderWindow window)
        {

        }

        public void Load(IAssetBundle bundle)
        {

        }

    }
}
