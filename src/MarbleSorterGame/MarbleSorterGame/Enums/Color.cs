namespace MarbleSorterGame.Enums
{
    public enum Color
    {
        Red,
        Green,
        Blue
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
