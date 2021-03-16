namespace MarbleSorterGame.Enums
{
    public enum Weight : int
    {
        Small,
        Medium,
        Large
    };
    
    public static class WeightExtensions 
    {
        public static string ToGameLabel(this Weight weight)
        {
            return weight switch
            {
                Weight.Small => "S",
                Weight.Medium => "M",
                Weight.Large => "L",
            };
        }
    }
}
