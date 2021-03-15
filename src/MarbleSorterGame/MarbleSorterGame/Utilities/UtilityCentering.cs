using SFML.System;
using SFML.Graphics;

namespace UtilityCentering
{
    /// <summary>
    /// sets transformable origin of shape/text to its rectangular center
    /// </summary>
    public static class UtilityCentering
    {
        public static Vector2f CenterOrigin(this Shape entity)
        {
            FloatRect entityBounds = entity.GetLocalBounds();
            Vector2f entityCenter = new Vector2f(
                    entityBounds.Left + entityBounds.Width / 2.0f,
                    entityBounds.Top + entityBounds.Height / 2.0f
                );

            return entityCenter;
        }
        public static Vector2f CenterOrigin(this Text entity)
        {
            FloatRect entityBounds = entity.GetLocalBounds();
            Vector2f entityCenter = new Vector2f(
                    entityBounds.Left + entityBounds.Width / 2.0f,
                    entityBounds.Top + entityBounds.Height / 2.0f
                );

            return entityCenter;
        }
    }
}
