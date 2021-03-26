using System.Data;
using SFML.Graphics;

namespace MarbleSorterGame.Screens
{
    public abstract class Screen
    {
        public abstract void Update();
        public abstract void Draw(RenderWindow window);
    }
}