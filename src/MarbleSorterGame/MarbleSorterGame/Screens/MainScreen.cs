using System;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The main page where users can go to the setting screen or the game
    /// </summary>
    public class MainScreen
    {
        public MainScreen(RenderWindow window, AssetBundleLoader bundle, uint screenWidth, uint screenHeight)
        {
            Font font = bundle.Font;
            Sizer sizer = new Sizer(screenWidth, screenHeight);

        }
        
        public void Update(RenderWindow window, Font font)
        {
            
        }

        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public void Draw(RenderWindow window, Font font)
        {
            var sizer = new Sizer(window.Size.X,window.Size.Y);

            Label menuTitle = new Label("Marble Sorter Game", sizer.Percent(50, 30), 50, SFML.Graphics.Color.Black, font);
            Label copyright = new Label("Copyright 2021 - Mohawk College", sizer.Percent(80, 95), 15, SFML.Graphics.Color.Black, font);
            /**
            Text menuTitle = QuickShape.Label(
                "Marble Sorter Game",
                sizer.Percent(50, 30),
                font,
                SFML.Graphics.Color.Red);
            
            Text copyright = QuickShape.Label(
                "Copyright 2021 - Mohawk College",
                sizer.Percent(60, 95),
                font,
                SFML.Graphics.Color.Black);
            */

            Vector2f buttonSize = sizer.Percent(15f, 10f); // new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            Button buttonStart = new Button("Start", 1f, font, sizer.Percent(30f, 70f), buttonSize);
            Button buttonSettings = new Button("Settings",1f,  font, sizer.Percent(50f, 70f), buttonSize);
            Button buttonExit = new Button("Exit", 1f, font, sizer.Percent(70f, 70f), buttonSize);

            buttonStart.Render(window);
            buttonSettings.Render(window);
            buttonExit.Render(window);

            // Draw text on-top
            menuTitle.Draw(window);
            copyright.Draw(window);

            //============ Menu buttons event handlers ============
            EventHandler<SFML.Window.MouseButtonEventArgs> Game_MousePressed = (Object sender, SFML.Window.MouseButtonEventArgs mouse) =>
            {
                if (buttonStart.IsPressed(mouse.X, mouse.Y))
                {
                    MarbleSorterGame.ActiveMenu = Menu.Game;
                }
                else if (buttonSettings.IsPressed(mouse.X, mouse.Y))
                {
                    MarbleSorterGame.ActiveMenu = Menu.Settings;

                }
                else if (buttonExit.IsPressed(mouse.X, mouse.Y))
                {
                    window.Close();
                }
            };

            window.MouseButtonPressed += Game_MousePressed;
        }
    }
}