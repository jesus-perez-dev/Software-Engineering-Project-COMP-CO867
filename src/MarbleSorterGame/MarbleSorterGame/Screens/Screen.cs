using System.Data;
using SFML.Graphics;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// General window state that points to other screens
    /// </summary>
    public abstract class Screen
    {
        /// <summary>
        /// Updates any game entity positions, game logic and driver states
        /// </summary>
        public abstract void Update();
        
        /// <summary>
        /// Draws any game entities to the current window
        /// </summary>
        /// <param name="window">Current window to draw</param>
        public abstract void Draw(RenderWindow window);
    }
}