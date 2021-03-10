using System;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The game itself
    /// </summary>
    public class Game
    {

        public Game()
        {
            
        }
        
        
        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public static void Draw(RenderWindow window, Font font)
        {
            Sizer sizer = new Sizer(window.Size.X, window.Size.Y);
            
            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = sizer.Percent(0f, 0f),
                Size = sizer.Percent(100f, 6f),
                FillColor = new SFML.Graphics.Color(89, 105, 115) // dark-blue-gray ish color
            };

            // Start Button
            Button startButton = new Button(
                "Start Simulation",
                0.8f,
                font,
                sizer.Percent(91, 3),
                sizer.Percent(8, 5)
            );
            
            // Reset Button
            Button resetButton = new Button(
                "Reset Game",
                0.8f,
                font,
                sizer.Percent(81, 3),
                sizer.Percent(8, 5)
                );
            
            // Exit Button
            Button exitButton = new Button(
                "Exit Game",
                0.8f,
                font,
                sizer.Percent(71, 3),
                sizer.Percent(8, 5)
            );
            
            // Bucket #1

            Bucket bucket1 = new Bucket();
            
            
            
            // Conveyer Belt Line
            RectangleShape conveyor = new RectangleShape();
            conveyor.Position = sizer.Percent(0, 60); //new Vector2f(0, 350);
            conveyor.Size = sizer.Percent(100, 0);
            conveyor.OutlineColor = SFML.Graphics.Color.Black;
            conveyor.OutlineThickness = 2f;
            window.Draw(conveyor);
            
            Text instructions = QuickShape.Label("sample instructions", sizer.Percent(0,0), font, SFML.Graphics.Color.Black);

            Drawable[] drawables =
            {
                menuBarBackground,
                instructions
            };

            GameEntity[] entities =
            {
                startButton,
                resetButton,
                exitButton
            };
            
            foreach (var drawable in drawables)
                window.Draw(drawable);
            
            foreach (var entity in entities)
                entity.Render(window);
        }
    }
}