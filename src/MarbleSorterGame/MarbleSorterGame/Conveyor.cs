using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class Conveyor : GameEntity
    {
        private RectangleShape _conveyor;
        private Vector2f _conveyorSpeed;

        public Conveyor(Vector2f position, Vector2f size, Vector2f conveyorSpeed) : base(position, size)
        {
            _conveyorSpeed = conveyorSpeed;
            _conveyor = new RectangleShape(size);
            _conveyor.Position = position;

            _conveyor.OutlineColor = SFML.Graphics.Color.Black;
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
