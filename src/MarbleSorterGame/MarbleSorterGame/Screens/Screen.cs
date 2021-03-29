using System.Data;
using SFML.Graphics;

namespace MarbleSorterGame.Screens
{
    // General window state that points to other screens
    public abstract class Screen
    {
        // Updates any game entity positions, game logic and driver states
        public abstract void Update();
        
        // Draws any game entities to the current window
        public abstract void Draw(RenderWindow window);
    }
}