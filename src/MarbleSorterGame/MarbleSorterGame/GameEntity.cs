using SFML.System;
using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    public class GameEntity
    {
        public String Name { get; set; }
        public Vector2f Dimensions { get; set; }
        public Vector2f Position { get; set; }

        public GameEntity()
        {

        }

        /// <summary>
        /// Provides layout for basic game entity object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dimensions"></param>
        /// <param name="position"></param>
        public GameEntity(String name, Vector2f dimensions, Vector2f position)
        {
            this.Name = name;
            this.Dimensions = dimensions;
            this.Position = position;
        }

        /// <summary>
        /// Sets new position of gameentity based on current position and new position vector
        /// </summary>
        /// <param name="position">vector describing change in position</param>
        public void UpdatePosition(Vector2f position)
        {
            this.Position = new Vector2f(this.Position.X + position.X, this.Position.Y + position.Y);
        }

        /// <summary>
        /// Checks to see whether colliding entity intersects with game entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Overlaps(Shape entity)
        {
            //use .intersects and .getGlobalBounds (only for Shapes)
            return false;
        }

        public bool Inside(GameEntity entity)
        {
            //if inside bucket?
            return false;
        }

        /// <summary>
        /// Renders GameEntity object onto target window
        /// </summary>
        /// <param name="window">RenderWindow target</param>
        public virtual void Render(RenderWindow window)
        {
        }

        /// <summary>
        /// Loads assets bundle such as textures and sounds onto the game entity
        /// </summary>
        /// <param name="bundle"></param>
        public virtual void Load(IAssetBundle bundle)
        {

        }

    }
}
