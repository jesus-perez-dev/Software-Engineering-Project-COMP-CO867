using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public class Game
    {
        public static void Draw(RenderWindow window, Font font)
        {
            //CONVEYOR
            RectangleShape conveyor = new RectangleShape();
            conveyor.Position = new Vector2f(0, 350);
            conveyor.Size = new Vector2f(1000, 0);
            conveyor.OutlineColor = SFML.Graphics.Color.Red;
            conveyor.OutlineThickness = 2f;
            window.Draw(conveyor);
            
            //default menu button size
            Vector2f buttonSize = new Vector2f(window.Size.X / 7, window.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            var labelColor = SFML.Graphics.Color.White;
            var labelSize = 15;

            //button/text positions
            var buttonStartSimulationPosition = new Vector2f(window.Size.X / 3f * 2, window.Size.Y / 10);
            var buttonBackPosition = new Vector2f(window.Size.X / 3f * 2, window.Size.Y / 10);
            var buttonResetPosition = new Vector2f(window.Size.X / 4f * 2, window.Size.Y / 10);
            var instructionsPosition = new Vector2f(120, 25);

            //initial marble position
            var marbleActivePosition = new Vector2f(0, window.Size.Y / 2 );

            //============ Game Menu buttons/text ============
            var buttonStartSimulationLabel = new Label("Start", null, 10, SFML.Graphics.Color.Black, font);
            var buttonBackLabel = new Label("Back", null, 10, SFML.Graphics.Color.Black, font);
            var buttonResetLabel = new Label("Reset", null, 10, SFML.Graphics.Color.Black, font);

            var buttonStartSimulation = new Button(buttonStartSimulationPosition, buttonSize, buttonStartSimulationLabel);
            var buttonBack = new Button(buttonBackPosition, buttonSize, buttonBackLabel);
            var buttonReset= new Button(buttonResetPosition, buttonSize, buttonResetLabel);

            Label instructions = new Label("sample instructions", instructionsPosition, 25, SFML.Graphics.Color.Black, font);
            Text labelBucketReq1 = new Text("Bucket 1 Requirements", font);
            Text labelBucketReq2 = new Text("Bucket 2 Requirements", font);
            Text labelBucketReq3 = new Text("Bucket 3 Requirements", font);

            //var sample = new Dictionary<GameEntity, Text>();

            //=============== Setting Positions ===================

            instructions.Draw(window);
        }

    }
}