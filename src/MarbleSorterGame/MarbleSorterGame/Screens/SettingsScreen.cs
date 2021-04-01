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
    public class SettingsGameScreen : GameScreen
    {
        private Button[] _buttons;

        private Text _title;
        private Label _editNote;
        private Label _settings;
        
        // Constructor
        public SettingsGameScreen(RenderWindow window, IAssetBundle bundle) : base(window, bundle)
        {
            Window.MouseMoved += MouseHoverOverButton;
            Window.MouseButtonPressed += MenuMousePressed;

            var titlePosition = Screen.PositionRelative(Joint.Middle, Joint.Start);
            _title = new Text("Settings", bundle.Font);
            _title.CharacterSize = 48;
            _title.FillColor = Color.Black;
            _title.Position = titlePosition.ShiftX(-_title.GetGlobalBounds().Width/2).ShiftY(30);

            var buttonSize = Screen.Percent(20, 8);
            var buttonPosition = Screen.PositionRelative(Joint.End, Joint.Start).ShiftX(-buttonSize.X).ShiftY(buttonSize.Y);
            var returnButton = new Button("Return to Menu", 0.5f, bundle.Font, buttonPosition, buttonSize);
            returnButton.ClickEvent += ReturnButtonClickHandler;
            _buttons = new[] { returnButton } ;

            string dirPath = ((AssetBundleLoader) bundle).AbsoluteAssetDirectoryPath;
            string gamePath = Path.Join(dirPath, "game.json");
            var editNotePosition = Screen
                .PositionRelative(Joint.Middle, Joint.Start)
                .ShiftY(returnButton.Box.Position.Y + 60 + buttonSize.Y);
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
            
            var settingsPosition = Screen.PositionRelative(Joint.Middle, Joint.Middle);
            settingsPosition.Y = _editNote.LabelText.Position.Y + _editNote.LabelText.GetGlobalBounds().Height + 30;
            _settings = new Label(settingsText, settingsPosition, 26, Color.Black, bundle.Font);
            _settings.LabelText.Position += new Vector2f(0, _settings.LabelText.GetGlobalBounds().Height / 2);
        }

        private void ReturnButtonClickHandler(object? sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Window.MouseMoved -= MouseHoverOverButton;
            Window.MouseButtonPressed -= MenuMousePressed;
            MarbleSorterGame.ActiveMenu = Menu.Main;
        }
        
         private void MenuMousePressed(object? sender, MouseButtonEventArgs mouse)
        {
            UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }
         
        private void MouseHoverOverButton(object? sender, MouseMoveEventArgs mouse)
        {
            UpdateButtonsFromMouseEvent(Window, _buttons, mouse);
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