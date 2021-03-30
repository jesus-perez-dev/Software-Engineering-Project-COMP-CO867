﻿using SFML.System;
using SFML.Graphics;
using System;
using MarbleSorterGame.GameEntities;

namespace MarbleSorterGame.GameEntities
{
    // Describes every entity within the game that can be rendered/updated
    public abstract class GameEntity
    {
        private RectangleShape _rect;
        
        public String InfoText;
        public string Name { get; set; }
        
        // Bounding box of the game entity
        public RectangleShape Box => _rect;
        
        // Position of the game entity
        public virtual Vector2f Position
        {
            get => _rect.Position;
            set => _rect.Position = value;
        }

        // Size of the game entity
        public virtual Vector2f Size
        {
            get => _rect.Size;
            set => _rect.Size = value;
        }

        public FloatRect GlobalBounds => _rect.GetGlobalBounds();

        protected GameEntity() : this(default(Vector2f), default(Vector2f))
        {
        }

        // Constructor for game entity
        public GameEntity(Vector2f position, Vector2f size)
        {
            _rect = new RectangleShape
            {
                Position = position,
                Size = size,
                //FillColor = Color.Yellow,
                //OutlineColor = Color.Yellow,
                //OutlineThickness = 1
            };
        }

        // Return a new scale value appropriate for fitting "sprite" inside of box "size"
        protected static Vector2f RescaleSprite(Vector2f size, Sprite sprite)
        {
            // Actual size of a sprite is: sprite.Scale * sprite.Texture.Size. To set the size of the sprite, we need to adjust the scale
            return new Vector2f(size.X / sprite.Texture.Size.X, size.Y / sprite.Texture.Size.Y);
        }

        // Changes the default transformation origin from top left of entity object to its center
        public Vector2f CenterOrigin(Texture texture)
        {
            FloatRect bounds = this.GlobalBounds;

            return new Vector2f(
                bounds.Left + texture.Size.X / 2f,
                bounds.Top + texture.Size.Y / 2f
            );
        }

        // Scales the game entity texture so that it fits its dimensions
        public Vector2f ScaleEntity(Texture texture)
        {
            Vector2u textureSize = texture.Size;
            Vector2f scaleRatio = new Vector2f(Size.X / textureSize.X, Size.Y / textureSize.Y);
            return scaleRatio;
        }

        // Checks to see if game entity overlaps bounding box of current game entity
        public bool Overlaps(GameEntity entity)
        {
            return _rect.GetGlobalBounds().Intersects(entity.GlobalBounds);
        }
        
        // Check to see if entity at any X position intersects this entities Y position (IE a horizontal straight line touches both)
        public bool OverlapsVertical(GameEntity entity)
        {
            float top = GlobalBounds.Top;
            float bottom = GlobalBounds.Top + GlobalBounds.Height;

            float eTop = entity.GlobalBounds.Top;
            float eBottom = entity.GlobalBounds.Top + entity.GlobalBounds.Height;

            return (bottom > eTop && top < eBottom);
        }
        
        // Checks whether entity X coordinate fits completely inside this entity
        public bool InsideHorizontal(GameEntity entity)
        {
            return entity.Position.X < Position.X &&
                entity.Position.X + entity.Size.X > Position.X + Size.X;
        }

        // Checks if every point on the entity fit inside of given entity
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

        // Checks whether mouse cursor is hovered over the game entity 
        public bool MouseHovered(Vector2f mousePosition)
        {
            return (
                mousePosition.X > GlobalBounds.Left && mousePosition.X < GlobalBounds.Left + GlobalBounds.Width &&
                mousePosition.Y > GlobalBounds.Top && mousePosition.Y < GlobalBounds.Top + GlobalBounds.Height
                );
        }

        // Render method for game entity that draws its bounding box
        public virtual void Render(RenderWindow window)
        {
            window.Draw(_rect);
        }
        // Load method for any assets required for the game entity
        public abstract void Load(IAssetBundle bundle);

        // Shows titlegame entity 
        public override string ToString()
        {
            return InfoText;
        }
    }
}
