using System;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    // The general structure of how the game is implemented
    public abstract class GameLoop
    {
        public static int FPS = 60;
        
        // Update resolution from config file
        public static uint WINDOW_WIDTH;
        public static uint WINDOW_HEIGHT;
        public static RectangleShape WINDOW_RECT;
        protected static RenderWindow WINDOW;
        
        // Mouse cursor types
        public static class Cursors
        {
            public static readonly Cursor NotAllowed = new Cursor(Cursor.CursorType.NotAllowed);
            public static readonly Cursor Hand = new Cursor(Cursor.CursorType.Hand);
            public static readonly Cursor Arrow = new Cursor(Cursor.CursorType.Arrow);
        }
        
        // Color for when the window get cleared
        public  SFML.Graphics.Color WindowClearColor
        {
            get;
            protected set;
        }

        // Game structure that determines how entities are updated and timed
        protected GameLoop(IAssetBundle bundle, string windowTitle, Color windowClearColor)
        {
            WINDOW_WIDTH = bundle?.GameConfiguration?.ScreenWidth ?? 800;
            WINDOW_HEIGHT = bundle?.GameConfiguration?.ScreenHeight ?? 600;
            WINDOW_RECT = new RectangleShape {Size = new Vector2f(WINDOW_WIDTH, WINDOW_HEIGHT)};
            
            Styles styles = Styles.Close;
            ContextSettings settings = default;
            settings.AntialiasingLevel = 8;
            
            WINDOW = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle, styles, settings);

            WindowClearColor = windowClearColor;
            
            WINDOW.Closed += WINDOW_Closed;
            WINDOW.Resized += WINDOW_Resized;
        }

        // Structure for the game. Assets are first loaded then objects that do not need to change get initialized. The game then loops.
        // Inside the loop events get handled, then data gets updated followed by a window clear, redrawing all the object, and then
        // displaying new ones. 
        public void Run()
        {
            while (WINDOW.IsOpen)
            {
                WINDOW.DispatchEvents();
                Thread.Sleep(1000 / FPS);
                Update();
                WINDOW.Clear(WindowClearColor);
                Draw();
                WINDOW.Display();
            }
        }

        // Abstract method for inheritors to implement
        public abstract void Update();
        public abstract void Draw();

        // Event handler for window closing (through clicking the X button)
        private void WINDOW_Closed(object sender, EventArgs e)
        {
            WINDOW.Close();
        }
        
        // Event handler for window resizing
        private void WINDOW_Resized(object sender, SizeEventArgs e)
        {
            WINDOW.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }
    }
}