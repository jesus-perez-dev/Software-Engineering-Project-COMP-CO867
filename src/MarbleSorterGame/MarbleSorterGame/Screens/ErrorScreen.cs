using System;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Screens
{
    public class ErrorGameScreen : GameScreen
    {
        private Text _errorText;
        
        public ErrorGameScreen(RenderWindow window, string error) : base(window, null)
        {
            var font = BackupFont.LoadBackupFont();
            float ySpacer = Screen.Percent(0f, 3f).Y;

            _errorText = new Text(string.Join("\n", error.ColumnWrap(69)), BackupFont.LoadBackupFont())
            {
                FillColor = Color.Red,
                CharacterSize = 18,
            };
            _errorText.Origin = _errorText.CenterOrigin();
            _errorText.Position = Screen.PositionRelative(Joint.Middle, Joint.Middle);
            
            // Shrink the text until it fits the screen resolution
            while (!Screen.GetGlobalBounds().ContainsRect(_errorText.GetGlobalBounds()))
            {
                _errorText.Scale = _errorText.Scale - new Vector2f(0.01f, 0.01f);
                _errorText.Position = Screen.PositionRelative(Joint.Middle, Joint.Middle);
            }
        }

        public override void Update()
        {
        }

        public override void Draw(RenderWindow window)
        {
            window.Draw(_errorText);
        }
    }
}