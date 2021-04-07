using System;
using System.Collections.Generic;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MarbleSorterGame.Screens
{
    // The main page where users can go to the setting screen or the game
    public class MainMenuGameScreen : GameScreen, IDisposable
    {
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

        public MainMenuGameScreen(RenderWindow window, IAssetBundle bundle) : base(window, bundle)
        {
            var font = Bundle.Font;

            _background = new RectangleShape { Size = Screen.Size };
            _menuTitle = new Label("Marble Sorter Game", Screen.Percent(50, 30), 50, Color.Black, font);
            _copyright = new Label("Copyright 2021 - Mohawk College", Screen.Percent(80, 95), 15, Color.Black, font);
            
            Vector2f buttonSize = Screen.Percent(15f, 10f); // new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            _buttonStart = new Button("Start", 0.7f, font, Screen.Percent(30f, 70f), buttonSize);
            _buttonSettings = new Button("Settings",0.7f,  font, Screen.Percent(50f, 70f), buttonSize);
            _buttonExit = new Button("Exit", 0.7f, font, Screen.Percent(70f, 70f), buttonSize);
            _buttons = new [] { _buttonStart, _buttonSettings, _buttonExit };

            Enums.Color[] colors = {Enums.Color.Red, Enums.Color.Green, Enums.Color.Blue};
            for (int i = 0; i < 3; i++)
            {
                var marble = new Marble(new Vector2f(-1000, -1000), colors[i], Weight.Large);
                marble.SetState(MarbleState.Rolling);
                marble.Load(bundle);
                marble.Size = _marbleSize;
                marble.Update();
                _aestheticMarbles.Add(marble);
            }
            SetupInputHandlers();
        }
        // Initializes all event handlers associated with main screen
        private void SetupInputHandlers()
        {
            Window.MouseButtonPressed += MenuMousePressed;
            Window.MouseMoved += MouseHoverOverButton;
            _buttonStart.ClickEvent += StartButtonClickHandler;
            _buttonSettings.ClickEvent += SettingsButtonClickHandler;
            _buttonExit.ClickEvent += ExitButtonClickHandler;
        }

        // Event handler for start button click
        private void StartButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.ActiveMenu = Menu.Game;
            Dispose();
        }

        // Event handler for settings button click
        private void SettingsButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.ActiveMenu = Menu.Settings;
            Dispose();
        }

        // Event handler for exit button click
        private void ExitButtonClickHandler(object? sender, MouseButtonEventArgs mouse)
        {
            Window.Close();
        }

        // Event handler for mouse click, check if any of menu buttons were clicked from that
         private void MenuMousePressed(object? sender, MouseButtonEventArgs mouse)
        {
            UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }
         
        // Event handler for mouse hover-over 
        private void MouseHoverOverButton(object? sender, MouseMoveEventArgs mouse)
        {
            UpdateButtonsFromMouseEvent(Window, _buttons, mouse);
        }

        // Updates positions of game entities and game state logic
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

        // Redraw screen in preparation for next update loop
        public override void Draw(RenderWindow window)
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

        // Dispose all event handlers associated with the menu screen
        public void Dispose()
        {
            Window.MouseButtonPressed -= MenuMousePressed;
            Window.MouseMoved -= MouseHoverOverButton;
            _buttonStart.ClickEvent -= StartButtonClickHandler;
            _buttonExit.ClickEvent -= ExitButtonClickHandler;
            _buttonSettings.ClickEvent -= SettingsButtonClickHandler;
        }
    }
}