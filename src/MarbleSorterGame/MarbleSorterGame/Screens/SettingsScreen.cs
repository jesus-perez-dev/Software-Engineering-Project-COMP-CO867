using System;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Screens
{
    /// <summary>
    /// The settings screen
    /// </summary>
    public class SettingsScreen : Screen
    {
        public SettingsScreen(RenderWindow window, IAssetBundle bundle)
        {
            Font font = bundle.Font;
        }
        
        public override void Update()
        {
            
        }

        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public override void Draw(RenderWindow window)
        {
            //default sizes
            Vector2f buttonSize = new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            var labelColor = SFML.Graphics.Color.White;
            var labelSize = 20;

            var buttonSoundIncreasePosition = new Vector2f(50, 50);
            var buttonSoundDecreasePosition=  new Vector2f(100, 50);
            var buttonResolutionPosition = new Vector2f(150, 50);
            var buttonBackPosition = new Vector2f(170, 50);

            //============ Settings Menu buttons/text ============
            /**
            var buttonSoundIncrease = new Button(buttonSoundIncreasePosition, buttonSize, "Volume +");
            var buttonSoundDecrease = new Button(buttonSoundIncreasePosition, buttonSize, "Volume -");
            var buttonResolution = new Button(buttonResolutionPosition, buttonSize, "Fullscreen");
            var buttonBack = new Button(buttonBackPosition, buttonSize, "Back");

            buttonSoundIncrease.Draw(menu);
            buttonSoundDecrease.Draw(menu);
            buttonResolution.Draw(menu);
            buttonBack.Draw(menu);
            */
        }
    }
}