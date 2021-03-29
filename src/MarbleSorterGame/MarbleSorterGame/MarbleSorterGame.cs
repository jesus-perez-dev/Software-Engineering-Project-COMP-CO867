using System;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.Screens;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    // The implementation of the abstract game loop 
    public class MarbleSorterGame : GameLoop
    {
        private static string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        private static Screen _activeScreen;
        private static IAssetBundle _bundle;

        public static Menu ActiveMenu
        {
            set
            {
                _activeScreen = value switch
                {
                    Menu.Game => new GameScreen(WINDOW, _bundle),
                    Menu.Main => new MainScreen(WINDOW, _bundle),
                    Menu.Settings => new SettingsScreen(WINDOW, _bundle) 
                };
            }
        }
        
        public MarbleSorterGame(IAssetBundle bundle) : base(bundle, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            _bundle = bundle;
            ActiveMenu = Menu.Main;
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

        // Update any data for the game
        public override void Update()
        {
            _activeScreen.Update();
        }

        // Draw method for the game. Each of the screens call their draw method depending on the active menu
        public override void Draw()
        {
            _activeScreen.Draw(WINDOW);
        }
    }
}