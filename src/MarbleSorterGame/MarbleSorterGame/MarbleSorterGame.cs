using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// The implementation of the abstract game loop 
    /// </summary>
    public class MarbleSorterGame : GameLoop
    {
        public static string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        //public static uint WINDOW_WIDTH = 1280;
        //public static uint WINDOW_HEIGHT = 720;

        public static uint WINDOW_WIDTH = 1920;
        public static uint WINDOW_HEIGHT = 1080;
        
        private GameScreen _gameScreen;
        private SettingsScreen _settingsScreen;
        private MainScreen _mainScreen;
        public static Menu ActiveMenu { get; set; }

        public MarbleSorterGame() : base(WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            var bundle = new AssetBundleLoader("assets/");
            _gameScreen = new GameScreen(Window, bundle, WINDOW_WIDTH, WINDOW_HEIGHT, new KeyboardIODriver());
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