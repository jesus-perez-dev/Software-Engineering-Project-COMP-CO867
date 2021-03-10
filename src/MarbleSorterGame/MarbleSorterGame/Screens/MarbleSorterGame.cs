using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The implementation of the abstract game loop 
    /// </summary>
    public class MarbleSorterGame : GameLoop
    {
        public const string WINDOW_TITLE = "PLC Training Simulator - Marble Sorter Game";
        public const uint DEFAULT_WINDOW_WIDTH = 800;
        public const uint DEFAULT_WINDOW_HEIGHT = 600;
        
        private GameScreen _gameScreen;

        public static Menu ActiveMenu
        {
            get;
            set;
        }

        private AssetBundleLoader _bundle;
        private static Font _font;
        private List<GameEntity> _entities;
        private Vector2f _velocityGravity;
        private Vector2f _velocityConveyer;

        public MarbleSorterGame() : base(DEFAULT_WINDOW_WIDTH, DEFAULT_WINDOW_HEIGHT, WINDOW_TITLE, SFML.Graphics.Color.White)
        {
            //IAssetBundle
            var loader = new AssetBundleLoader("assets/");
            _gameScreen = new GameScreen(loader, Window.Size.X, Window.Size.Y);
            _velocityConveyer = new Vector2f(1, 0);
            _velocityGravity= new Vector2f(0, 1);
        }

        /// <summary>
        /// Loading of content
        /// todo: add assets here?
        /// </summary>
        public override void LoadContent()
        {
            _bundle = new AssetBundleLoader("assets/");
            _font = new Font("assets/OpenSans-Regular.ttf");
        }

        /// <summary>
        /// Initializing any objects the game will need
        /// </summary>
        public override void Initialize()
        {
            //========= Game Menu Entities Requirements ===========
            //hardcoding sample requirements (CHANGE once PLC solution known/ config file created)
            //make seperate class/interface for requirements?
            var bucketsCapacity = new List<int>();
            bucketsCapacity.AddRange(new int[3] { 5, 5, 5 });

            var bucketsReqWeight = new List<Weight>();
            bucketsReqWeight.AddRange(new Weight[3] { Weight.Large, Weight.Medium, Weight.Small });

            var bucketsReqColor = new List<Color>();
            bucketsReqColor.AddRange(new Color[3] { Color.Red, Color.Blue, Color.Green});

            //========= Game Menu Entities ===========
            //Sensor colorSensor = new ColorSensor();
            //Sensor pressureSensor = new PressureSensor();
            //Sensor motionSensor = new MotionSensor();
            //var sensors = new List<Sensor>() { colorSensor, pressureSensor, motionSensor };


            /*
            Bucket bucket1 = new Bucket(bucketsReqColor[0], bucketsReqWeight[0], bucketsCapacity[0]);
            Bucket bucket2 = new Bucket(bucketsReqColor[1], bucketsReqWeight[1], bucketsCapacity[1]);
            Bucket bucket3 = new Bucket(bucketsReqColor[2], bucketsReqWeight[2], bucketsCapacity[2]);
            var Buckets = new List<Bucket>() {bucket1, bucket2, bucket3};
            Trapdoor trapdoor1 = new Trapdoor();
            Trapdoor trapdoor2 = new Trapdoor();
            Trapdoor trapdoor3 = new Trapdoor();
            var trapdoors = new List<Trapdoor>() { trapdoor1, trapdoor2, trapdoor3 };
            */

            Marble marbleRedCorrect = new Marble(new Vector2f(30,30), new Vector2f(100,100), Color.Red, Weight.Large);
            Marble marbleRedIncorrect = new Marble(new Vector2f(30,30), new Vector2f(100,100), Color.Red, Weight.Small);
            var marbles = new List<Marble>() { marbleRedCorrect, marbleRedIncorrect } ;

            marbleRedCorrect.Position = new Vector2f(200f, 200f);
            marbleRedCorrect.Size = new Vector2f(50, 50);

            //var marbles = new List<Marble>() { marbleRedCorrect} ;

            var bucketSignal1= new CircleShape();
            var bucketSignal2= new CircleShape();
            var bucketSignal3= new CircleShape();
            var bucketSignals = new List<CircleShape>() { bucketSignal1, bucketSignal2, bucketSignal3 };

            _entities = new List<GameEntity>()
            {
                /*
                colorSensor,
                pressureSensor,
                motionSensor,
                bucket1,
                bucket2,
                bucket3,
                trapdoor1,
                trapdoor2,
                trapdoor3,
                marbleRedIncorrect
                */
                marbleRedCorrect,
            };

            foreach(GameEntity entity in _entities)
            {
                entity.Load(_bundle);
            }
        }

        /// <summary>
        /// Update any data for the game
        /// todo: copy draw method and have each of the screen classes handle data changes (eg, marbles moving)
        /// </summary>
        public override void Update()
        {

            foreach(GameEntity entity in _entities)
            {
                if (entity is Marble)
                {
                    Marble marble = (Marble)entity;
                    marble.Move();
                    marble.Rotate(2f);
                }
            }
        }

        /// <summary>
        /// Draw method for the game. Each of the screens call their draw method depending on the active menu
        /// </summary>
        public override void Draw()
        {
            switch (ActiveMenu)
            {
                case Menu.Main:
                    MainScreen.Draw(Window, _font);
                    break;
                case Menu.Settings:
                    SettingsScreen.Draw(Window, _font);
                    break;
                case Menu.Game:
                    _gameScreen.Draw(Window, _font);
                    break;
            }
        }
        
    }
}