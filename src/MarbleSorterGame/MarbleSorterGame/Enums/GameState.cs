namespace MarbleSorterGame.Enums
{
    public enum GameState
    {
        Lose,
        Win,
        Progress,
        Pause
    }

    public static class GameStateEnumExtensions 
    {
        public static string ToHumanString(this GameState self)
        {
            return self switch
            {
                GameState.Lose => "You Lost!",
                GameState.Win => "You Won!",
                GameState.Progress => "In-Progress",
                GameState.Pause => "Paused",
            };
        }
        
        public static SFML.Graphics.Color ToIndicatorColor(this GameState self)
        {
            return self switch
            {
                GameState.Lose => SFML.Graphics.Color.Red,
                GameState.Win => SFML.Graphics.Color.Green,
                GameState.Progress => SFML.Graphics.Color.Yellow,
                GameState.Pause => SFML.Graphics.Color.Cyan,
            };
        }
    }
}