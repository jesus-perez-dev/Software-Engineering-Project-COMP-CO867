using System;
using System.Runtime.CompilerServices;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// The main page where users can go to the setting screen or the game
    /// </summary>
    public class MainScreen : IDisposable
    {
        private Font _font;
        private RenderWindow _window;

        private Button _buttonStart;
        private Button _buttonSettings;
        private Button _buttonExit;
        private Label _menuTitle;
        private Label _copyright;
        
        private void MenuMousePressed(object? sender, SFML.Window.MouseButtonEventArgs mouse)
        {
            if (_buttonStart.IsPressed(mouse.X, mouse.Y))
            {
                MarbleSorterGame.ActiveMenu = Menu.Game;
                Dispose();
            }
            else if (_buttonSettings.IsPressed(mouse.X, mouse.Y))
            {
                MarbleSorterGame.ActiveMenu = Menu.Settings;
                Dispose();
            }
            else if (_buttonExit.IsPressed(mouse.X, mouse.Y))
            {
                _window.Close();
            }
        }
        
        public MainScreen(RenderWindow window, IAssetBundle bundle, uint screenWidth, uint screenHeight)
        {
            _font = bundle.Font;
            _window = window;
            _window.MouseButtonPressed += MenuMousePressed;
            
            Sizer sizer = new Sizer(screenWidth, screenHeight);
            _menuTitle = new Label("Marble Sorter Game", sizer.Percent(50, 30), 50, SFML.Graphics.Color.Black, _font);
            _copyright = new Label("Copyright 2021 - Mohawk College", sizer.Percent(80, 95), 15, SFML.Graphics.Color.Black, _font);

            Vector2f buttonSize = sizer.Percent(15f, 10f); // new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            _buttonStart = new Button("Start", 1f, _font, sizer.Percent(30f, 70f), buttonSize);
            _buttonSettings = new Button("Settings",1f,  _font, sizer.Percent(50f, 70f), buttonSize);
            _buttonExit = new Button("Exit", 1f, _font, sizer.Percent(70f, 70f), buttonSize);
        }
        
        public void Update()
        {
            
        }

        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public void Draw(RenderWindow window)
        {
            //var sizer = new Sizer(window.Size.X,window.Size.Y);
            _buttonStart.Render(window);
            _buttonSettings.Render(window);
            _buttonExit.Render(window);

            // Draw text on-top
            _menuTitle.Draw(window);
            _copyright.Draw(window);
        }

        public void Dispose()
        {
            _window.MouseButtonPressed -= MenuMousePressed;
        }
    }
}