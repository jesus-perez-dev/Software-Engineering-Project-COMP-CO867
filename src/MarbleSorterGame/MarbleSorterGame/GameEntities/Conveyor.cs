using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    public class Conveyor : GameEntity
    {
        private RectangleShape _conveyor;
        private Vector2f _conveyorSpeed;

        public Conveyor(Vector2f position, Vector2f size, Vector2f conveyorSpeed) : base(position, size)
        {
            _conveyor = Box;
            _conveyorSpeed = conveyorSpeed;

            _conveyor.OutlineColor = Color.Black;
            _conveyor.FillColor = Color.Black;

            _conveyor.Position = position;
        }

        public override void Load(IAssetBundle bundle)
        {
        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_conveyor);
        }
    }

}
