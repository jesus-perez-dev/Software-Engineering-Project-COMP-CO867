using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using Newtonsoft.Json;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// The main page where users can go to the setting screen or the game
    /// </summary>
    public class MainScreen : Screen, IDisposable
    {
        private Font _font;
        private RenderWindow _window;
        private RectangleShape _background;

        private Button _buttonStart;
        private Button _buttonSettings;
        private Button _buttonExit;
        private Label _menuTitle;
        private Label _copyright;

        private List<Marble> _aestheticMarbles = new List<Marble>();
        private Button[] _buttons;

        private static float _marbleDimension = GameLoop.WINDOW_RECT.Size.Y / 6;
        private static Vector2f _marbleSize = new Vector2f(_marbleDimension, _marbleDimension);
        private int _updateCounter = 0;

        private Text _errorText;
        private bool _errorMode;

        public MainScreen(RenderWindow window, IAssetBundle bundle)
        {
            var screen = GameLoop.WINDOW_RECT;
            if (bundle.Error != null)
            {
                _errorMode = true;
                _errorText = new Text(string.Join("\n", bundle.Error), BackupFont.LoadBackupFont())
                {
                    FillColor = Color.Red,
                    Position = new Vector2f(0, 0),
                    CharacterSize = 18
                };
                // Shrink the text until it fits the screen resolution
                while (!screen.GetGlobalBounds().ContainsRect(_errorText.GetGlobalBounds()))
                {
                    _errorText.Scale = _errorText.Scale - new Vector2f(0.01f, 0.01f);
                    _errorText.Position = new Vector2f(1f, 1f);
                }
            }
            else
            {
                _errorMode = false;
                _font = bundle.Font;
                _window = window;

                _background = new RectangleShape { Size = screen.Size };
                _menuTitle = new Label("Marble Sorter Game", screen.Percent(50, 30), 50, Color.Black, _font);
                _copyright = new Label("Copyright 2021 - Mohawk College", screen.Percent(80, 95), 15, Color.Black, _font);
                
                Vector2f buttonSize = screen.Percent(15f, 10f); // new Vector2f(window.Size.X / 7, window.Size.Y / 11);
                _buttonStart = new Button("Start", 0.7f, _font, screen.Percent(30f, 70f), buttonSize);
                _buttonSettings = new Button("Settings",0.7f,  _font, screen.Percent(50f, 70f), buttonSize);
                _buttonSettings.Disabled = true;
                _buttonExit = new Button("Exit", 0.7f, _font, screen.Percent(70f, 70f), buttonSize);
                _buttons = new [] { _buttonStart, _buttonSettings, _buttonExit };

                Enums.Color[] colors = {Enums.Color.Red, Enums.Color.Green, Enums.Color.Blue};
                for (int i = 0; i < 3; i++)
                {
                    var marble = new Marble(screen, default, colors[i], Weight.Large);
                    marble.SetState(MarbleState.Rolling);
                    marble.Load(bundle);
                    marble.Size = _marbleSize;
                    marble.Update();
                    _aestheticMarbles.Add(marble);
                }
                SetupInputHandlers();
            }
            
        }
        /// <summary>
        /// Initializes all event handlers associated with main screen
        /// </summary>
        private void SetupInputHandlers()
        {
            _window.MouseButtonPressed += MenuMousePressed;
            _window.MouseMoved += MouseHoverOverButton;
            _buttonStart.ClickEvent += StartButtonClickHandler;
            _buttonSettings.ClickEvent += SettingsButtonClickHandler;
            _buttonExit.ClickEvent += ExitButtonClickHandler;
        }

        /// <summary>
        /// Event handler for start button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void StartButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.ActiveMenu = Menu.Game;
            Dispose();
        }

        /// <summary>
        /// Event handler for settings button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void SettingsButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.ActiveMenu = Menu.Settings;
            Dispose();
        }

        /// <summary>
        /// Event handler for exit button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void ExitButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            _window.Close();
        }

        /// <summary>
        /// Event handler for mouse click, check if any of menu buttons were clicked from that
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
         private void MenuMousePressed(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }
         
        /// <summary>
        /// Event handler for mouse hover-over 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="mouse"></param>
        private void MouseHoverOverButton(object? sender, MouseMoveEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromMouseEvent(_window, _buttons, mouse);
        }

        /// <summary>
        /// Updates positions of game entities and game state logic
        /// </summary>
        public override void Update()
        {
            float xOffset = default;
            foreach (var marble in _aestheticMarbles)
            {
                marble.Update();
                marble.Position = (GameLoop.WINDOW_RECT.Size / 2) - (marble.Size / 2);
                marble.Position = marble.Position
                    .ShiftX(-_marbleDimension*2)
                    .ShiftX(xOffset);
                xOffset += _marbleDimension*2;
            }
        }

        /// <summary>
        /// Redraw screen in preparation for next update loop
        /// </summary>
        /// <param name="window"></param>
        public override void Draw(RenderWindow window)
        {
            if (_errorMode)
            {
                window.Draw(_errorText);
            }
            else
            {
                window.Draw(_background);
                
                foreach (var marble in _aestheticMarbles)
                    marble.Render(window);
                
                _buttonStart.Render(window);
                _buttonSettings.Render(window);
                _buttonExit.Render(window);
                    
                // Draw text on-top
                _menuTitle.Draw(window);
                _copyright.Draw(window);
            }
        }

        /// <summary>
        /// Dispose all event handlers associated with the menu screen
        /// </summary>
        public void Dispose()
        {
            _window.MouseButtonPressed -= MenuMousePressed;
            _window.MouseMoved -= MouseHoverOverButton;
            _buttonStart.ClickEvent -= StartButtonClickHandler;
            _buttonExit.ClickEvent -= ExitButtonClickHandler;
            _buttonSettings.ClickEvent -= SettingsButtonClickHandler;
        }
    }
}