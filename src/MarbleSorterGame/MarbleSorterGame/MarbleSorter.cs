using System;
using System.Collections.Generic;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;

using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace MarbleSorterGame
{
    class MarbleSorter
    {
        private List<GameEntity> _entities;
        //private SimulationClient _client;
        private IAssetBundle _assets;

        private RenderWindow _window;
        private VideoMode _videoMode;
        private String _windowTitle;
        private Styles _windowStyles;
        private Menu _activeMenu;

        //have in assets loader?
        private static Font _font;

        //add parameter SimulationClient client when library implemented
        public MarbleSorter(IAssetBundle assetBundle)
        {
            //this._client = client;
            _assets = assetBundle;

            _activeMenu = Menu.Main;
            _font = new Font("assets/OpenSans-Regular.ttf");

            //========= creating window ===========
            //if decide on fullscreen mode, will have to figure out new way to place entites other than hardcoding coordinates
            _videoMode = new VideoMode(800, 600);
            _windowTitle = "PLC Training Simulator - Marble Sorter Game";
            _windowStyles = Styles.Default;
            _window = new RenderWindow(_videoMode, _windowTitle, _windowStyles);
            _window.Clear(SFML.Graphics.Color.White);

            //set framerate to monitor refresh rate (if graphics driver allows) or hardcode to 60
            //_window.SetVerticalSyncEnabled(true);
            _window.SetFramerateLimit(60);

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
            Sensor colorSensor = new ColorSensor();
            Sensor pressureSensor = new PressureSensor();
            Sensor motionSensor = new MotionSensor();
            var sensors = new List<Sensor>() { colorSensor, pressureSensor, motionSensor };


            Bucket bucket1 = new Bucket(bucketsReqColor[0], bucketsReqWeight[0], bucketsCapacity[0]);
            Bucket bucket2 = new Bucket(bucketsReqColor[1], bucketsReqWeight[1], bucketsCapacity[1]);
            Bucket bucket3 = new Bucket(bucketsReqColor[2], bucketsReqWeight[2], bucketsCapacity[2]);
            var Buckets = new List<Bucket>() {bucket1, bucket2, bucket3};

            Trapdoor trapdoor1 = new Trapdoor();
            Trapdoor trapdoor2 = new Trapdoor();
            Trapdoor trapdoor3 = new Trapdoor();
            var trapdoors = new List<Trapdoor>() { trapdoor1, trapdoor2, trapdoor3 };

            Marble marbleRedCorrect = new Marble(15, Color.Red, Weight.Large);
            Marble marbleRedIncorrect = new Marble(15, Color.Red, Weight.Small);
            var marbles = new List<Marble>() { marbleRedCorrect, marbleRedIncorrect } ;

            var bucketSignal1= new CircleShape();
            var bucketSignal2= new CircleShape();
            var bucketSignal3= new CircleShape();
            var bucketSignals = new List<CircleShape>() { bucketSignal1, bucketSignal2, bucketSignal3 };

            _entities = new List<GameEntity>()
            {
                colorSensor,
                pressureSensor,
                motionSensor,
                bucket1,
                bucket2,
                bucket3,
                trapdoor1,
                trapdoor2,
                trapdoor3,
                marbleRedCorrect,
                marbleRedIncorrect
            };
        }

        public void Run()
        {
            switchMenu();

            //============ Main loop ============
            while (_window.IsOpen) {
                _window.WaitAndDispatchEvents();
                _window.Display();
            }
        }

        private void MainMenu(RenderWindow menu)
        {
            //============ Main Menu buttons/text ============

            //default menu button size
            Vector2f buttonSize = new Vector2f(menu.Size.X / 7, menu.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            int menuTitleSize = 30;
            int menuButtonSize = 15;

            //button/text positions
            var buttonStartPosition = new Vector2f(menu.Size.X / 3, menu.Size.Y - 200);
            var buttonSettingsPosition = new Vector2f(menu.Size.X / 2, menu.Size.Y - 200);
            var buttonExitPosition = new Vector2f(menu.Size.X / 3f * 2, menu.Size.Y - 200);
            var menuTitlePosition = new Vector2f(menu.Size.X / 2, menu.Size.Y / 5);
            var copyrightPosition = new Vector2f(menu.Size.X - 100, menu.Size.Y - 20);

            var menuTitle = new Label("Marble Sorter Game", menuTitlePosition, menuTitleSize, SFML.Graphics.Color.Red, _font);
            var copyright= new Label("Copyright 2021 - Mohawk College", copyrightPosition, 10, SFML.Graphics.Color.White, _font);

            var buttonStartLabel = new Label("Start", null, menuButtonSize, SFML.Graphics.Color.Black, _font);
            var buttonSettingsLabel = new Label("Settings", null, menuButtonSize, SFML.Graphics.Color.Black, _font);
            var buttonExitLabel = new Label("Exit", null, menuButtonSize, SFML.Graphics.Color.Black, _font);

            Button buttonStart = new Button(buttonStartPosition, buttonSize, buttonStartLabel);
            Button buttonSettings = new Button(buttonSettingsPosition, buttonSize, buttonSettingsLabel);
            Button buttonExit = new Button(buttonExitPosition, buttonSize, buttonExitLabel);

            buttonStart.Draw(menu);
            buttonSettings.Draw(menu);
            buttonExit.Draw(menu);
            menuTitle.Draw(menu);
            copyright.Draw(menu);

            //============ Menu buttons event handlers ============
            EventHandler<SFML.Window.MouseButtonEventArgs> Game_MousePressed = (Object sender, SFML.Window.MouseButtonEventArgs mouse) =>
            {
                if (buttonStart.IsPressed(mouse.X, mouse.Y))
                {
                    _activeMenu = Menu.Game;
                    switchMenu();
                }
                else if (buttonSettings.IsPressed(mouse.X, mouse.Y))
                {
                    _activeMenu = Menu.Settings;
                    switchMenu();

                }
                else if (buttonExit.IsPressed(mouse.X, mouse.Y))
                {
                    menu.Close();
                }
            };

            menu.MouseButtonPressed += Game_MousePressed;
        }

        private void SettingsMenu(RenderWindow menu)
        {
            //default sizes
            Vector2f buttonSize = new Vector2f(menu.Size.X / 7, menu.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            var labelColor = SFML.Graphics.Color.White;
            var labelSize = 20;

            var buttonSoundIncreasePosition = new Vector2f(50, 50);
            var buttonSoundDecreasePosition=  new Vector2f(100, 50);
            var buttonResolutionPosition = new Vector2f(150, 50);
            var buttonBackPosition = new Vector2f(170, 50);

            //============ Settings Menu buttons/text ============
            /**
            var buttonSoundIncrease = new Button(buttonSoundIncreasePosition, buttonSize, "Volume +");
            var buttonSoundDecrease = new Button(buttonSoundIncreasePosition, buttonSize, "Volume -");
            var buttonResolution = new Button(buttonResolutionPosition, buttonSize, "Fullscreen");
            var buttonBack = new Button(buttonBackPosition, buttonSize, "Back");

            buttonSoundIncrease.Draw(menu);
            buttonSoundDecrease.Draw(menu);
            buttonResolution.Draw(menu);
            buttonBack.Draw(menu);
            */

        }

        private void GameMenu(RenderWindow menu)
        {
            //default menu button size
            Vector2f buttonSize = new Vector2f(menu.Size.X / 7, menu.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            var labelColor = SFML.Graphics.Color.White;
            var labelSize = 15;

            //button/text positions
            var buttonStartSimulationPosition = new Vector2f(menu.Size.X / 3f * 2, menu.Size.Y / 10);
            var buttonBackPosition = new Vector2f(menu.Size.X / 3f * 2, menu.Size.Y / 10);
            var buttonResetPosition = new Vector2f(menu.Size.X / 4f * 2, menu.Size.Y / 10);
            var instructionsPosition = new Vector2f(0, 0);

            //initial marble position
            var marbleActivePosition = new Vector2f(0, menu.Size.Y / 2 );

            //============ Game Menu buttons/text ============
            var buttonStartSimulationLabel = new Label("Start", null, 10, SFML.Graphics.Color.Black, _font);
            var buttonBackLabel = new Label("Back", null, 10, SFML.Graphics.Color.Black, _font);
            var buttonResetLabel = new Label("Reset", null, 10, SFML.Graphics.Color.Black, _font);

            var buttonStartSimulation = new Button(buttonStartSimulationPosition, buttonSize, buttonStartSimulationLabel);
            var buttonBack = new Button(buttonBackPosition, buttonSize, buttonBackLabel);
            var buttonReset= new Button(buttonResetPosition, buttonSize, buttonResetLabel);

            Text instructions = new Text("sample instructions", _font, 25);
            Text labelBucketReq1 = new Text("Bucket 1 Requirements", _font);
            Text labelBucketReq2 = new Text("Bucket 2 Requirements", _font);
            Text labelBucketReq3 = new Text("Bucket 3 Requirements", _font);

            //var sample = new Dictionary<GameEntity, Text>();

            //=============== Setting Positions ===================
            instructions.Position = instructionsPosition;

            menu.Draw(instructions);
        }

        private void switchMenu()
        {
            //HACK, will close window and make new one, eventhandlers need to be refreshed in a smoother way
            _window.Clear(SFML.Graphics.Color.White);
            _window.Close();
            _window = new RenderWindow(_videoMode, _windowTitle, _windowStyles);

            switch (_activeMenu)
            {
                case Menu.Main:
                    MainMenu(_window);
                    break;
                case Menu.Settings:
                    SettingsMenu(_window);
                    break;
                case Menu.Game:
                    GameMenu(_window);
                    break;
            }
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
}