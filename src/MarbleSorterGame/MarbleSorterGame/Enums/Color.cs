namespace MarbleSorterGame.Enums
{
    public enum Color
    {
        // 0 = no marble in front
        Red = 1,
        Green = 2,
        Blue = 3
    }

    public static class ColorExtensions
    {
        public static SFML.Graphics.Color ToSfmlColor(this Color col)
        {
            return col switch
            {
                Color.Red => SFML.Graphics.Color.Red,
                Color.Green => SFML.Graphics.Color.Green,
                Color.Blue => SFML.Graphics.Color.Blue,
            };
        }
    }
}
