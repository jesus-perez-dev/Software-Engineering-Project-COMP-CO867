using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    // Moves marble towards the exit, contains trapdoors that drop the marbles
    public class Conveyor : GameEntity
    {
        private RectangleShape _conveyor;
        private Vector2f _conveyorSpeed;
        public bool ConveyorOn;

        // Constructor for conveyor
        public Conveyor(Vector2f position, Vector2f size, Vector2f conveyorSpeed) : base(position, size)
        {
            _conveyor = Box;
            _conveyor.FillColor = Color.Black;
            _conveyor.Position = position;
            
            _conveyorSpeed = conveyorSpeed;

            ConveyorOn = true;
        }
        
        // Sets state of signal light to be turned on or off
        public void SetState(bool state)
        {
            ConveyorOn = state;
        }

        // Loads assets for conveyor
        public override void Load(IAssetBundle bundle)
        {
        }

        // Draws conveyor onto window
        public override void Render(RenderWindow window)
        {
            _conveyor.FillColor = ConveyorOn ? new Color(20, 150, 0): Color.Red;
            
            window.Draw(_conveyor);
        }
    }

}
