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
    /// <summary>
    /// Abstract class for the game loop. This is the general structure of how the game needs to be implemented.
    /// </summary>
    public abstract class GameLoop
    {
        public static int FPS = 60;
        
        // Update resolution from config file
        public static uint WINDOW_WIDTH;
        public static uint WINDOW_HEIGHT;
        public static RectangleShape WINDOW_RECT;
        protected static RenderWindow WINDOW;
        
        public static class Cursors
        {
            public static readonly Cursor NotAllowed = new Cursor(Cursor.CursorType.NotAllowed);
            public static readonly Cursor Hand = new Cursor(Cursor.CursorType.Hand);
            public static readonly Cursor Arrow = new Cursor(Cursor.CursorType.Arrow);
        }
        
        /// <summary>
        /// Color for when the window get cleared
        /// </summary>
        public  SFML.Graphics.Color WindowClearColor
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowWidth"></param>
        /// <param name="windowHeight"></param>
        /// <param name="windowTitle"></param>
        /// <param name="windowClearColor"></param>
        protected GameLoop(uint windowWidth, uint windowHeight, string windowTitle, SFML.Graphics.Color windowClearColor)
        {
            WINDOW_WIDTH = windowWidth;
            WINDOW_HEIGHT = windowHeight;
            WINDOW_RECT = new RectangleShape {Size = new Vector2f(WINDOW_WIDTH, WINDOW_HEIGHT)};
            
            Styles styles = Styles.Close;
            ContextSettings settings = default;
            settings.AntialiasingLevel = 8;
            
            WINDOW = new RenderWindow(new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT), windowTitle, styles, settings);

            WindowClearColor = windowClearColor;
            
            WINDOW.Closed += WINDOW_Closed;
            WINDOW.Resized += WINDOW_Resized;
        }

        /// <summary>
        /// Structure for the game. Assets are first loaded then objects that do not need to change get initialized. The game then loops.
        /// Inside the loop events get handled, then data gets updated followed by a window clear, redrawing all the object, and then
        /// displaying new ones. 
        /// </summary>
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

        /// <summary>
        /// Abstract method for inheritors to implement
        /// </summary>
        public abstract void Update();
        public abstract void Draw();

        /// <summary>
        /// Event for when users click the x button to close the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WINDOW_Closed(object sender, EventArgs e)
        {
            WINDOW.Close();
        }
        
        private void WINDOW_Resized(object sender, SizeEventArgs e)
        {
            WINDOW.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }
    }
}