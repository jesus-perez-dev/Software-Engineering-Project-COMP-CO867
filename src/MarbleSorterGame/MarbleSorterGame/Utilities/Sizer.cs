using System.Drawing;
using SFML.System;

namespace MarbleSorterGame.Utilities
{
    /// <summary>
    /// A class to compute SFML size objects
    /// </summary>
    public class Sizer
    {
        private uint _screenWidth;
        private uint _screenHeight;
        
        public Sizer(uint screenWidth, uint screenHeight)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        /// <summary>
        /// Return a Vector object with X and Y equal to the specified screen percentage from the left
        /// </summary>
        public Vector2f Percent(float xScreenPercent, float yScreenPercent)
        {
            return new Vector2f()
            {
                X = (int)((xScreenPercent / 100.0f) * _screenWidth),
                Y = (int)((yScreenPercent / 100.0f) * _screenHeight),
            };
        }
    }
}
