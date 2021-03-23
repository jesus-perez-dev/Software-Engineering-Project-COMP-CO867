using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame.Utilities
{
    public enum Joint 
    {
        Start,  // Top if vertical, far-left if horizontal
        Middle, 
        End     // Bottom if vertical, far-right if horizontal
    }
    
    public static class ShapeExtensions
    {
        public static Vector2f Percent(this Shape self, float percentX, float percentY) =>
            new Vector2f().PercentOf(self, percentX, percentY);
        
        public static Vector2f PositionRelative(this Shape self, Joint xJoint, Joint yJoint)
        {
            return new Vector2f()
                .PositionRelativeToX(self, xJoint)
                .PositionRelativeToY(self, yJoint);
        }
    }
    
    public static class VectorExtensions
    {
        private static float Align(float start, float dimension, Joint joint)
        {
            return joint switch
            {
                Joint.Start => start,
                Joint.Middle => start + dimension / 2,
                Joint.End => start + dimension
            };
        }

        public static Vector2f PositionRelativeToX(this Vector2f self, Shape target, Joint targetJoint)
        {
            float x = Align(target.Position.X, target.GetGlobalBounds().Width - target.Origin.X, targetJoint);
            return new Vector2f(x, self.Y);
        }

        public static Vector2f PositionRelativeToY(this Vector2f self, Shape target, Joint targetJoint)
        {
            float y = Align(target.Position.Y, target.GetGlobalBounds().Height - target.Origin.Y, targetJoint);
            return new Vector2f(self.X, y);
        }

        public static Vector2f PositionRelativeTo(this Vector2f self, Shape target, Joint xJoint, Joint yJoint)
        {
            return self
                .PositionRelativeToX(target, xJoint)
                .PositionRelativeToY(target, yJoint);
        }
        
        public static Vector2f ShiftY(this Vector2f self, float amount) => new Vector2f(self.X, self.Y + amount);
        public static Vector2f ShiftX(this Vector2f self, float amount) => new Vector2f(self.X + amount, self.Y);
        public static Vector2f Shift(this Vector2f self, Vector2f amount) => new Vector2f(self.X + amount.X, self.Y + amount.Y);

        public static Vector2f PercentOfX(this Vector2f self, Shape containerShape, float percent)
        {
            float width = containerShape.GetGlobalBounds().Width;
            return new Vector2f(width * (percent / 100.0f), self.Y);
        }
        
        public static Vector2f PercentOfY(this Vector2f self, Shape containerShape, float percent)
        {
            float height = containerShape.GetGlobalBounds().Height;
            return new Vector2f(self.X, height * (percent / 100.0f));
        }
        
        public static Vector2f PercentOf(this Vector2f self, Shape containerShape, float percentX, float percentY)
            => self
                .PercentOfX(containerShape, percentX)
                .PercentOfY(containerShape, percentY);

        public static Vector2f PercentOfPositionX(this Vector2f self, Shape containerShape, float percent)
            => self.PercentOfX(containerShape, percent) + new Vector2f(containerShape.Position.X, 0);
        
        public static Vector2f PercentOfPositionY(this Vector2f self, Shape containerShape, float percent)
            => self.PercentOfY(containerShape, percent) + new Vector2f(0, containerShape.Position.Y);

        public static Vector2f PercentOfPosition(this Vector2f self, Shape containerShape, float percentX, float percentY)
            => self
                .PercentOfPositionX(containerShape, percentX)
                .PercentOfPositionY(containerShape, percentY);
    }
}