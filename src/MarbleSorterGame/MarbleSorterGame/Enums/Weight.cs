namespace MarbleSorterGame.Enums
{
    public enum Weight : int
    {
        // 0 = no marble in front
        Small = 1,
        Medium = 2,
        Large = 3
    };
    
    public static class WeightExtensions 
    {
        public static string ToGameLabel(this Weight weight)
        {
            return weight switch
            {
                Weight.Small => "Small",
                Weight.Medium => "Medium",
                Weight.Large => "Large",
            };
        }
    }
}
