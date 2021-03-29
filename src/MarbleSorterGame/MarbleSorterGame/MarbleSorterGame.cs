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
        private static string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        private static Screen _activeScreen;
        private static IAssetBundle _bundle;
        private static IIODriver _driver;

        public static Menu ActiveMenu
        {
            set
            {
                _activeScreen = value switch
                {
                    Menu.Game => new GameScreen(WINDOW, _bundle, _driver, 0),
                    Menu.Main => new MainScreen(WINDOW, _bundle),
                    Menu.Settings => new SettingsScreen(WINDOW, _bundle) 
                };
            }
        }
        
        public MarbleSorterGame(IAssetBundle bundle) : base(bundle.GameConfiguration.ScreenWidth, bundle.GameConfiguration.ScreenHeight, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            _bundle = bundle;
            _driver = bundle.GameConfiguration.Driver switch
            {
                DriverType.Keyboard => new KeyboardIODriver(),
                DriverType.Simulation => new S7IODriver(bundle.GameConfiguration.DriverOptions),
                _ => throw new ArgumentException($"Unknown IO driver: {bundle.GameConfiguration.Driver}")
            };

            ActiveMenu = Menu.Main;
        }

        /// <summary>
        /// Trigger click event(s) on buttons if a click event occured there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="buttons">All buttons that could be clicked on</param>
        /// <param name="mouse">Mouse object</param>
        public static void UpdateButtonsFromClickEvent(object? sender, GameEntities.Button[] buttons, MouseButtonEventArgs mouse)
        {
            foreach (var button in buttons)
                if (button.MouseInButton(mouse.X, mouse.Y))
                    button.Click(sender, mouse);
        }


        /// <summary>
        /// Update the state of buttons/cursor based on mouse event and list of buttons
        /// </summary>
        /// <param name="window">Current window to draw</param>
        /// <param name="buttons">All buttons that could be hovered over on</param>
        /// <param name="mouse">Mouse object</param>
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
            _activeScreen.Update();
        }

        /// <summary>
        /// Draw method for the game. Each of the screens call their draw method depending on the active menu
        /// </summary>
        public override void Draw()
        {
            _activeScreen.Draw(WINDOW);
        }
    }
}