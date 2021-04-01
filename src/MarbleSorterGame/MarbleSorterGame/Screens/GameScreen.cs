using System.Data;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame.Screens
{
    // General window state that points to other screens
    public abstract class GameScreen
    {
        protected readonly RenderWindow Window;
        protected readonly IAssetBundle Bundle;
        protected readonly RectangleShape Screen;

        public GameScreen(RenderWindow window, IAssetBundle bundle)
        {
            Window = window;
            Bundle = bundle;
            Screen = new RectangleShape {Size = new Vector2f(window.Size.X, window.Size.Y)};
        }
        
        // Updates any game entity positions, game logic and driver states
        public abstract void Update();
        
        // Draws any game entities to the current window
        public abstract void Draw(RenderWindow window);

        // Trigger click event(s) on buttons if a click event occured there
        public void UpdateButtonsFromClickEvent(object? sender, GameEntities.Button[] buttons, MouseButtonEventArgs mouse)
        {
            foreach (var button in buttons)
                if (button.MouseInButton(mouse.X, mouse.Y))
                    button.Click(sender, mouse);
        }

        // Update the state of buttons/cursor based on mouse event and list of buttons
        public void UpdateButtonsFromMouseEvent(RenderWindow window, GameEntities.Button[] buttons, MouseMoveEventArgs mouse)
        {
            window.SetMouseCursor(GameLoop.Cursors.Arrow);
            foreach (var button in buttons)
            {
                button.Hovered = false;
                if (button.MouseInButton(mouse.X, mouse.Y))
                {
                    button.Hovered = true;
                    window.SetMouseCursor(button.Disabled ? GameLoop.Cursors.NotAllowed : GameLoop.Cursors.Hand);
                }
            }
        }
    }
}