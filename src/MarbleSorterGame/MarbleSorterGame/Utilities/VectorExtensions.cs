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

    public static class FloatRectExtensions
    {
        // rectangle "other" fits entirely inside this recangle "self"
        public static bool ContainsRect(this FloatRect self, FloatRect other)
        {
            return self.Contains(other.Left, other.Top) &&
                   self.Contains(other.Left + other.Width, other.Top + other.Height);
        }
        
        public static Vector2f PositionRelative(this FloatRect self, Joint xJoint, Joint yJoint)
        {
            return new Vector2f()
                .PositionRelativeToRectX(self, 0, xJoint)
                .PositionRelativeToRectY(self, 0, yJoint);
        }
    }

    public static class ShapeExtensions
    {
        public static Vector2f Percent(this Shape self, float percentX, float percentY) =>
            new Vector2f().PercentOf(self, percentX, percentY);
        
        public static Vector2f PositionRelative(this Shape self, Joint xJoint, Joint yJoint)
        {
            return new Vector2f()
                .PositionRelativeToShapeX(self, xJoint)
                .PositionRelativeToShapeY(self, yJoint);
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

        private static float PositionRelativeScalar(float start, float adjust, float dimension, Joint targetJoint)
            => Align(start + adjust, dimension, targetJoint);
        
        public static Vector2f PositionRelativeToRectX(this Vector2f self, FloatRect target, float adjustOffsetX, Joint targetJoint)
            => new Vector2f(PositionRelativeScalar(target.Left, adjustOffsetX, target.Width, targetJoint), self.Y);
        
        public static Vector2f PositionRelativeToRectY(this Vector2f self, FloatRect target, float adjustOffsetY, Joint targetJoint)
            => new Vector2f(self.X, PositionRelativeScalar(target.Top, adjustOffsetY, target.Height, targetJoint));
        
        public static Vector2f PositionRelativeToShapeY(this Vector2f self, Shape target, Joint targetJoint)
            => self.PositionRelativeToRectY(target.GetGlobalBounds(), target.Origin.Y + target.OutlineThickness, targetJoint);

        public static Vector2f PositionRelativeToShapeX(this Vector2f self, Shape target, Joint targetJoint)
            => self.PositionRelativeToRectX(target.GetGlobalBounds(), target.Origin.X + target.OutlineThickness, targetJoint);
        
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
    }
}