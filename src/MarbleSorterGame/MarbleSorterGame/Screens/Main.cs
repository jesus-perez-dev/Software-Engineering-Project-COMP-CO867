using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The main page where users can go to the setting screen or the game
    /// </summary>
    public class Main
    {
        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public static void Draw(RenderWindow window, Font font)
        {
            //============ Main Menu buttons/text ============

            //default menu button size
            Vector2f buttonSize = new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            int menuTitleSize = 30;
            int menuButtonSize = 15;

            //button/text positions
            var buttonStartPosition = new Vector2f(window.Size.X / 3, window.Size.Y - 200);
            var buttonSettingsPosition = new Vector2f(window.Size.X / 2, window.Size.Y - 200);
            var buttonExitPosition = new Vector2f(window.Size.X / 3f * 2, window.Size.Y - 200);
            var menuTitlePosition = new Vector2f(window.Size.X / 2, window.Size.Y / 5);
            var copyrightPosition = new Vector2f(window.Size.X - 100, window.Size.Y - 20);

            var menuTitle = new Label("Marble Sorter Game", menuTitlePosition, menuTitleSize, SFML.Graphics.Color.Red, font);
            var copyright= new Label("Copyright 2021 - Mohawk College", copyrightPosition, 10, SFML.Graphics.Color.Black, font);

            var buttonStartLabel = new Label("Start", null, menuButtonSize, SFML.Graphics.Color.Black, font);
            var buttonSettingsLabel = new Label("Settings", null, menuButtonSize, SFML.Graphics.Color.Black, font);
            var buttonExitLabel = new Label("Exit", null, menuButtonSize, SFML.Graphics.Color.Black, font);

            Button buttonStart = new Button(buttonStartPosition, buttonSize, buttonStartLabel);
            Button buttonSettings = new Button(buttonSettingsPosition, buttonSize, buttonSettingsLabel);
            Button buttonExit = new Button(buttonExitPosition, buttonSize, buttonExitLabel);

            buttonStart.Draw(window);
            buttonSettings.Draw(window);
            buttonExit.Draw(window);
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