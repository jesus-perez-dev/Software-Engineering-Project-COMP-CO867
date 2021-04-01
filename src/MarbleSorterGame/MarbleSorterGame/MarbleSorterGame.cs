#nullable enable
using System;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.Screens;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    // The implementation of the abstract game loop 
    public class MarbleSorterGame : GameLoop
    {
        private static string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        private static GameScreen _activeGameScreen;
        private static IAssetBundle? _bundle;
        private static IIODriver? _driver;
        public static string? Error;

        public static Menu ActiveMenu
        {
            set
            {
                _activeGameScreen = value switch
                {
                    Menu.Game => new MarbleSorterGameScreen(WINDOW, _bundle, _driver),
                    Menu.Main => new MainMenuGameScreen(WINDOW, _bundle),
                    Menu.Settings => new SettingsGameScreen(WINDOW, _bundle),
                    Menu.Error => new ErrorGameScreen(WINDOW, Error) 
                };
            }
        }
        
        public MarbleSorterGame(IAssetBundle? bundle, IIODriver? driver, Exception? maybeLoadException) : base(bundle, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            _bundle = bundle;
            _driver = driver;

            if (maybeLoadException == null)
                ActiveMenu = Menu.Main;
            else 
                ShowErrorScreen(maybeLoadException);
        }

        private void ShowErrorScreen(Exception exception)
        {
            Console.WriteLine(exception);
            Error = exception.ToString();
            ActiveMenu = Menu.Error;
        }

        // Update any data for the game
        public override void Update()
        {
            try
            {
                _activeGameScreen.Update();
            }
            catch (Exception exception)
            {
                ShowErrorScreen(exception);
            }
        }

        // Draw method for the game. Each of the screens call their draw method depending on the active menu
        public override void Draw()
        {
            _activeGameScreen.Draw(WINDOW);
        }
    }
}