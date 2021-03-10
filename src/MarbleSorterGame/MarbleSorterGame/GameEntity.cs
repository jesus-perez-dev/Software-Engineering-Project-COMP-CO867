using SFML.Graphics;
using System;
using SFML.System;

namespace MarbleSorterGame
{
    public abstract class GameEntity
    {
        private RectangleShape _rect;
        
        public string Name { get; set; }

        /// <summary>
        /// Position of the game entity
        /// </summary>
        public Vector2f Position
        {
            get => _rect.Position;
            set => _rect.Position = value;
        }

        /// <summary>
        /// Size of the game entity
        /// </summary>
        public Vector2f Size
        {
            get => _rect.Size;
            set => _rect.Size = value;
        }

        public FloatRect GlobalBounds => _rect.GetGlobalBounds();
        

        public GameEntity(Vector2f position, Vector2f size)
        {
            _rect = new RectangleShape
            {
                Position = position,
                Size = size,
            };
        }

        /// <summary>
        /// Does any point on the entity fit inside this entity
        /// </summary>
        public bool Overlaps(GameEntity entity)
        {
            return _rect.GetGlobalBounds().Intersects(entity.GlobalBounds);
        }

        /// <summary>
        /// Does every point on the entity fit inside this entity
        /// </summary>
        public bool Inside(GameEntity entity)
        {
            var corners = new Vector2f[]
            {
                new Vector2f(entity.Position.X, entity.Position.Y), // Top-Left corner
                new Vector2f(entity.Position.X + entity.Size.X, entity.Position.Y), // Top-Right corner
                new Vector2f(entity.Position.X + entity.Size.X, entity.Position.Y + entity.Size.Y), // Bottom-Right corner
                new Vector2f(entity.Position.X, entity.Position.Y + entity.Size.Y), // Bottom-Left corner
            };

            foreach (var corner in corners)
            {
                if (!_rect.GetGlobalBounds().Contains(corner.X, corner.Y))
                    return false;
            }

            return true;
        }

        public abstract void Render(RenderWindow window);
        public abstract void Load(IAssetBundle bundle);
    }
}
