using SFML.System;
using SFML.Graphics;
using System;

namespace MarbleSorterGame
{
    public abstract class GameEntity
    {
        public Label HelperPopup { get; set; }
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
        public FloatRect LocalBounds => _rect.GetLocalBounds();

        public GameEntity(Vector2f position, Vector2f size)
        {
            _rect = new RectangleShape
            {
                Position = position,
                Size = size,
            };
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
        /// centers origin of game entity give it's asset loaded texture
        /// </summary>
        /// <param name="texture"></param>
        /// <returns></returns>
        public Vector2f CenterOrigin(Texture texture)
        {
            FloatRect bounds = GlobalBounds;

            return (
                new Vector2f(
                        bounds.Left + texture.Size.X / 2f,
                        bounds.Top + texture.Size.Y / 2f
                    )
                );
        }

        /// <summary>
        /// centers origin of any game entity 
        /// </summary>
        /// <returns></returns>
        public Vector2f CenterOrigin()
        {
            Vector2f entityCenter = new Vector2f(
                    LocalBounds.Left + LocalBounds.Width / 2.0f,
                    LocalBounds.Top + LocalBounds.Height / 2.0f
                );

            return entityCenter;
        }

        /// <summary>
        /// Scales the game entity texture so that it fits its dimensions
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>New dimension vector correctly ratio'd for the game entity dimension</returns>
        public Vector2f ScaleEntity(Texture texture)
        {
            Vector2u textureSize = texture.Size;
            Vector2f scaleRatio = new Vector2f(Size.X / textureSize.X, Size.Y / textureSize.Y);
            return scaleRatio;
        }

        /// <summary>
        /// Checks to see whether colliding entity intersects with game entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Overlaps(GameEntity entity)
        {
            return _rect.GetGlobalBounds().Intersects(entity.GlobalBounds);
        }

        /// <summary>
        /// checks whether button has been pressed with mouse coordinates
        /// </summary>
        /// <param name="X">Mouse pressed x-coordinate</param>
        /// <param name="Y">Mouse pressed y-coordinate</param>
        /// <returns></returns>
        public bool IsPressed(int X, int Y)
        {
            return (
                X > GlobalBounds.Left && X < GlobalBounds.Left + GlobalBounds.Width &&
                Y > GlobalBounds.Top && Y < GlobalBounds.Top + GlobalBounds.Height);
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

        public void RenderHelperPopup(RenderWindow window, Vector2f mousePosition)
        {
            if (HelperPopup is null) return;

            HelperPopup.Position = mousePosition;
            //why is helper popup null here?
            HelperPopup.Render(window);

            Console.WriteLine(HelperPopup.Position);
            Console.WriteLine(HelperPopup.Size);
            Console.WriteLine(HelperPopup);
        }

        public bool MouseHovered(Vector2f mousePosition)
        {
            return (
                mousePosition.X > GlobalBounds.Left && mousePosition.X < GlobalBounds.Left + GlobalBounds.Width &&
                mousePosition.Y > GlobalBounds.Top && mousePosition.Y < GlobalBounds.Top + GlobalBounds.Height
                );
        }

        public abstract void Render(RenderWindow window);
        public abstract void Load(IAssetBundle bundle);
    }
}
