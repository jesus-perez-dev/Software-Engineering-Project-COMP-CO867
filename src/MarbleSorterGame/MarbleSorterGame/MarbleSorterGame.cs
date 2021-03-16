using System;
using System.Collections.Generic;
using MarbleSorterGame.Screens;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The implementation of the abstract game loop 
    /// </summary>
    public class MarbleSorterGame : GameLoop
    {
        public static string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        
        private GameScreen _gameScreen;
        private SettingsScreen _settingsScreen;
        private MainScreen _mainScreen;
        public static Menu ActiveMenu { get; set; }

        public MarbleSorterGame(IAssetBundle bundle) : base(bundle.GameConfiguration.ScreenWidth, bundle.GameConfiguration.ScreenHeight, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            _gameScreen = new GameScreen(Window, bundle, WINDOW_WIDTH, WINDOW_HEIGHT, new KeyboardIODriver(), 0);
            _mainScreen = new MainScreen(Window, bundle, WINDOW_WIDTH, WINDOW_HEIGHT);
            _settingsScreen = new SettingsScreen(Window, bundle, WINDOW_WIDTH, WINDOW_HEIGHT);
        }

        /// <summary>
        /// Update any data for the game
        /// todo: copy draw method and have each of the screen classes handle data changes (eg, marbles moving)
        /// </summary>
        public override void Update()
        {
            switch (ActiveMenu)
            {
                case Menu.Main:
                    _mainScreen.Update();
                    break;
                case Menu.Settings:
                    _settingsScreen.Update();
                    break;
                case Menu.Game:
                    _gameScreen.Update();
                    break;
            }
        }

        /// <summary>
        /// Draw method for the game. Each of the screens call their draw method depending on the active menu
        /// </summary>
        public override void Draw()
        {
            switch (ActiveMenu)
            {
                case Menu.Main:
                    _mainScreen.Draw(Window);
                    break;
                case Menu.Settings:
                    _settingsScreen.Draw(Window);
                    break;
                case Menu.Game:
                    _gameScreen.Draw(Window);
                    break;
            }
        }
        
    }
}