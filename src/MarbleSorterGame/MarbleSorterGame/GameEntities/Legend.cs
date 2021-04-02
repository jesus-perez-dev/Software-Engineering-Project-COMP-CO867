using System;
using System.Runtime.InteropServices.WindowsRuntime;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.GameEntities
{
    // is text centered on a colored background (rectangle) with padding. the background will grow/shrink to match currently loaded text
    public class Legend : GameEntity
    {
        private Text _text;
        private uint _padding;
        private uint _outlineThickness;
        private RectangleShape _background;
        public bool Hidden { get; set; }
        
        // get or set text displayed in legend and resize background to fit
        public string DisplayedText
        {
            get => _text.DisplayedString;
            set {  
                _text.DisplayedString = value;
                Refit();
            }
        }

        // get or set background color of legend
        public SFML.Graphics.Color BackgroundColor
        {
            get => _background.FillColor;
            set => _background.FillColor = value;
        }

        public override Vector2f Size
        {
            get => Box.Position;
            set => throw new NotImplementedException("Cannot resize legend - operation unimplemented");
        }
        public override Vector2f Position 
        {
            get => base.Position;
            set
            {
                 base.Position = value;
                 Refit();
            } 
        }

        // adjust the size of background to accommodate text content
        private void Refit()
        {
            _text.Position = base.Position;
            _background.Position = base.Position;
            _background.Size = new Vector2f(_text.GetGlobalBounds().Width, _text.GetGlobalBounds().Height);
            _background.Size += new Vector2f(_padding + _outlineThickness, _padding + _outlineThickness);
            _text.Position = _background.Position
                .ShiftX(_padding/2f)
                .ShiftY(_padding/2f);
        }

        public Legend(string text, uint characterSize, uint padding, Color backgroundColor, Color outlineColor, uint outlineThickness, Font font, Vector2f position)
        {
            _padding = padding;
            _outlineThickness = outlineThickness;
            _background = Box;
            _background.FillColor = backgroundColor;
            _background.OutlineThickness = outlineThickness;
            _background.OutlineColor = outlineColor;

            _text = new Text(text, font, characterSize);
            _text.FillColor = Color.Black;
            base.Position = position;
            Refit();
        }
        
        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            if (!Hidden)
            {
                window.Draw(_background);
                window.Draw(_text);
            }
        }

        public override void Load(IAssetBundle bundle)
        {
        }
    }
}