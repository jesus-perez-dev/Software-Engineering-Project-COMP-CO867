using System;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

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
            IIODriver driver = bundle.GameConfiguration.Driver switch
            {
                DriverType.Keyboard => new KeyboardIODriver(),
                DriverType.Simulation => new S7IODriver(bundle.GameConfiguration.DriverOptions),
                _ => throw new ArgumentException($"Unknown IO driver: {bundle.GameConfiguration.Driver}")
            };
            
            _gameScreen = new GameScreen(Window, bundle, driver, 0);
            _mainScreen = new MainScreen(Window, bundle);
            _settingsScreen = new SettingsScreen(Window, bundle);
        }

        // Trigger click event(s) on buttons if a click event occured there
        public static void UpdateButtonsFromClickEvent(object? sender, GameEntities.Button[] buttons, MouseButtonEventArgs mouse)
        {
            foreach (var button in buttons)
                if (button.MouseInButton(mouse.X, mouse.Y))
                    button.Click(sender, mouse);
        }

        // Update the state of buttons/cursor based on mouse event and list of buttons
        public static void UpdateButtonsFromMouseEvent(RenderWindow window, GameEntities.Button[] buttons, MouseMoveEventArgs mouse)
        {
            window.SetMouseCursor(Cursors.Arrow);
            foreach (var button in buttons)
            {
                button.Hovered = false;
                if (button.MouseInButton(mouse.X, mouse.Y))
                {
                    button.Hovered = true;
                    window.SetMouseCursor(button.Disabled ? Cursors.NotAllowed : Cursors.Hand);
                }
            }
        }

        /// <summary>
        /// Update any data for the game
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