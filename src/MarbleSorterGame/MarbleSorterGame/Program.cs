namespace MarbleSorterGame
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var bundle = new AssetBundleLoader("assets/");
            MarbleSorterGame msg = new MarbleSorterGame(bundle);
            msg.Run();
        }
    }
}
