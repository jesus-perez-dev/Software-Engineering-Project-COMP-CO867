using System;
using System.Collections.Generic;
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
        private List<GameEntity> _entities;
        private EventHandler<SFML.Window.MouseButtonEventArgs> _main_MousePressed;

        public MainScreen(RenderWindow window, AssetBundleLoader bundle, uint screenWidth, uint screenHeight)
        {
            Font font = bundle.Font;
            Sizer sizer = new Sizer(screenWidth, screenHeight);
            var buttonColor = new SFML.Graphics.Color(200, 200, 200);

            Label menuTitle = new Label(
                "Marble Sorter Game", 
                font,
                30, 
                2f,
                SFML.Graphics.Color.Black, 
                buttonColor,
                sizer.Percent(50, 30), 
                sizer.Percent(0,0)
                );
            Label copyright = new Label(
                "Copyright 2021 - Mohawk College", 
                font,
                10, 
                5f,
                SFML.Graphics.Color.Black, 
                null,
                sizer.Percent(87, 95), 
                sizer.Percent(0,0)
                );

            Label buttonStart = new Label(
                "Start",
                font,
                15,
                2f,
                SFML.Graphics.Color.Black,
                buttonColor,
                sizer.Percent(35, 60),
                sizer.Percent(0, 0)
                );

            Label buttonSettings = new Label(
                "Reset",
                font,
                15,
                2f,
                SFML.Graphics.Color.Black,
                SFML.Graphics.Color.White,
                sizer.Percent(50, 60),
                sizer.Percent(0, 0)
                );

            Label buttonExit = new Label(
                "Exit",
                font,
                15,
                5f,
                SFML.Graphics.Color.Black,
                SFML.Graphics.Color.White,
                sizer.Percent(65, 60),
                sizer.Percent(0, 0)
                );

            _entities = new List<GameEntity>()
            {
                buttonStart,
                buttonSettings,
                buttonExit,
                menuTitle,
                copyright
            };

            //============ Menu buttons event handlers ============
            _main_MousePressed = (Object sender, SFML.Window.MouseButtonEventArgs mouse) =>
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

            window.MouseButtonPressed += _main_MousePressed;

        }
        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public void Draw(RenderWindow window, Font font)
        {
            foreach (var entity in _entities) 
            {
                entity.Render(window);
            }
        }
        
        public void Update(RenderWindow window, Font font)
        {
            
        }
    }
}