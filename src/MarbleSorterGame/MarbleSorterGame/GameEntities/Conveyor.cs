using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame.GameEntities
{
    /// <summary>
    /// Moves marble towards the exit, contains trapdoors that drop the marbles
    /// </summary>
    public class Conveyor : GameEntity
    {
        private RectangleShape _conveyor;
        private Vector2f _conveyorSpeed;

        /// <summary>
        /// Constructor for conveyer
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="conveyorSpeed"></param>
        public Conveyor(Vector2f position, Vector2f size, Vector2f conveyorSpeed) : base(position, size)
        {
            _conveyor = Box;
            _conveyorSpeed = conveyorSpeed;

            _conveyor.OutlineColor = Color.Black;
            _conveyor.FillColor = Color.Black;

            _conveyor.Position = position;
        }

        /// <summary>
        /// Loads assets for conveyer
        /// </summary>
        /// <param name="bundle">reference to bundle object containing asset references</param>
        public override void Load(IAssetBundle bundle)
        {
        }

        /// <summary>
        /// Draws conveyer onto window
        /// </summary>
        /// <param name="window">Current window to draw</param>
        public override void Render(RenderWindow window)
        {
            window.Draw(_conveyor);
        }
    }

}
