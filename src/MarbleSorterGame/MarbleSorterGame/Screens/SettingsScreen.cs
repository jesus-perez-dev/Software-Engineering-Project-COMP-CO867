using System;
using System.IO;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;

namespace MarbleSorterGame.Screens
{
    // The settings screen
    public class SettingsScreen : Screen
    {
        private Button _returnButton;
        private Button[] _buttons;
        private RenderWindow _window;

        private Text _title;
        private Label _editNote;
        private Label _settings;
        
        // Constructor
        public SettingsScreen(RenderWindow window, IAssetBundle bundle)
        {
            var screen = GameLoop.WINDOW_RECT;
            
            _window = window;
            _window.MouseMoved += MouseHoverOverButton;
            _window.MouseButtonPressed += MenuMousePressed;

            var titlePosition = screen.PositionRelative(Joint.Middle, Joint.Start);
            _title = new Text("Settings", bundle.Font);
            _title.CharacterSize = 48;
            _title.FillColor = Color.Black;
            _title.Position = titlePosition.ShiftX(-_title.GetGlobalBounds().Width/2).ShiftY(30);

            var buttonSize = screen.Percent(20, 8);
            var buttonPosition = screen.PositionRelative(Joint.End, Joint.Start).ShiftX(-buttonSize.X).ShiftY(buttonSize.Y);
            
            _returnButton = new Button("Return to Menu", 0.5f, bundle.Font, buttonPosition, buttonSize);
            _returnButton.ClickEvent += ReturnButtonClickHandler;
            _buttons = new[] { _returnButton } ;

            string dirPath = ((AssetBundleLoader) bundle).AbsoluteAssetDirectoryPath;
            string gamePath = Path.Join(dirPath, "game.json");
            var editNotePosition = screen
                .PositionRelative(Joint.Middle, Joint.Start)
                .ShiftY(_returnButton.Box.Position.Y + 60 + buttonSize.Y);
            _editNote = new Label($"Edit the following file to change game settings:\n\n{gamePath.ColumnWrap(60).Replace("\t", "")}", editNotePosition, 16, Color.Red, bundle.Font);
            
            var conf = bundle.GameConfiguration;
            string settingsText = string.Join("\n", new[]
            {
                $"Screen Width: {conf.ScreenWidth}",
                $"Screen Height: {conf.ScreenHeight}",
                "",
                $"Driver: {conf.Driver}",
                $"Simulation Name: {conf.DriverOptions?.SimulationName}",
                $"Update Interval: {conf.DriverOptions?.UpdateInterval}",
                "",
                $"Gate Period: {conf.GatePeriod}",
                $"Marble Period: {conf.MarblePeriod}",
                $"Trap Door Period: {conf.TrapDoorPeriod}",
            });
            
            var settingsPosition = screen.PositionRelative(Joint.Middle, Joint.Middle);
            settingsPosition.Y = _editNote.LabelText.Position.Y + _editNote.LabelText.GetGlobalBounds().Height + 30;
            _settings = new Label(settingsText, settingsPosition, 26, Color.Black, bundle.Font);
            _settings.LabelText.Position += new Vector2f(0, _settings.LabelText.GetGlobalBounds().Height / 2);
        }

        private void ReturnButtonClickHandler(object? sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            _window.MouseMoved -= MouseHoverOverButton;
            _window.MouseButtonPressed -= MenuMousePressed;
            MarbleSorterGame.ActiveMenu = Menu.Main;
        }
        
         private void MenuMousePressed(object? sender, MouseButtonEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }
         
        private void MouseHoverOverButton(object? sender, MouseMoveEventArgs mouse)
        {
            MarbleSorterGame.UpdateButtonsFromMouseEvent(_window, _buttons, mouse);
        }

        // Updates any game states changed by the setting
        public override void Update()
        {
        }

        // Method that gets called when the screen is to be redrawn
        public override void Draw(RenderWindow window)
        {
            foreach (var button in _buttons)
                button.Render(window);
            
            window.Draw(_title);
            window.Draw(_editNote);
            window.Draw(_settings);
        }
    }
}