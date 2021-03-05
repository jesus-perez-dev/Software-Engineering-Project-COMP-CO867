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

            _font = new Font("Assets/OpenSans-Regular.ttf");

            //========= creating window ===========
            //if decide on fullscreen mode, will have to figure out new way to place entites other than hardcoding coordinates
            _videoMode = new VideoMode(800, 600);
            _windowTitle = "PLC Training Simulator - Marble Sorter Game";
            _windowStyles = Styles.Default;
            _window = new RenderWindow(_videoMode, _windowTitle, _windowStyles);
            //set framerate to monitor refresh rate (if graphics driver allows) or hardcode to 60
            //_window.SetVerticalSyncEnabled(true);
            _window.SetFramerateLimit(60);


            //hardcoding sample requirements (CHANGE once PLC solution known/ config file created)
            //make seperate class/interface for requirements?
            int[] bucketsCapacity = new int[3] { 5, 5, 5 };
            Weight[] bucketsReqWeight = new Weight[3] { Weight.Large, Weight.Medium, Weight.Small };
            Color[] bucketsReqColor = new Color[3] { Color.Red, Color.Green, Color.Blue };

            //========= Game Menu Entities ===========
            _entities = new List<GameEntity>();

            Sensor colorSensor = new ColorSensor();
            Sensor pressureSensor = new PressureSensor();
            Sensor motionSensor = new MotionSensor();

            Bucket bucket1 = new Bucket(bucketsReqColor[0], bucketsReqWeight[0], bucketsCapacity[0]);
            Bucket bucket2 = new Bucket(bucketsReqColor[1], bucketsReqWeight[1], bucketsCapacity[1]);
            Bucket bucket3 = new Bucket(bucketsReqColor[2], bucketsReqWeight[2], bucketsCapacity[2]);

            Trapdoor trapdoor1 = new Trapdoor();
            Trapdoor trapdoor2 = new Trapdoor();
            Trapdoor trapdoor3 = new Trapdoor();

            Marble marbleRedCorrect = new Marble(15, Color.Red, Weight.Large);
            Marble marbleRedIncorrect = new Marble(15, Color.Red, Weight.Small);

            _entities.Add(colorSensor);
            _entities.Add(pressureSensor);
            _entities.Add(motionSensor);

            _entities.Add(bucket1);
            _entities.Add(bucket2);
            _entities.Add(bucket3);

            _entities.Add(trapdoor1);
            _entities.Add(trapdoor2);
            _entities.Add(trapdoor3);

            _entities.Add(marbleRedCorrect);
            _entities.Add(marbleRedIncorrect);
        }

        public void Run()
        {
            MainMenu();
        }

        private void MainMenu()
        {
            //============ Main Menu buttons/text ============
            //default menu button size
            Vector2f buttonSize = new Vector2f(_window.Size.X / 7, _window.Size.Y / 11);
            var buttonColor = SFML.Graphics.Color.Black;
            var labelSize = 20;

            //button/text positions
            var buttonStartPosition = new Vector2f(_window.Size.X / 3, _window.Size.Y - 200);
            var buttonSettingsPosition = new Vector2f(_window.Size.X / 2, _window.Size.Y - 200);
            var buttonExitPosition = new Vector2f(_window.Size.X / 3f * 2, _window.Size.Y - 200);
            var menuTitlePosition = new Vector2f(_window.Size.X / 3 - 100, _window.Size.Y / 5);

            Text menuTitle = new Text("Marble Sorter Game", _font, 50);
            Text copyrightNotice = new Text("Copyright 2021 - Mohawk College", _font, 10);

            Button buttonStart = new Button(buttonStartPosition, buttonSize, "Start", labelSize, _font, buttonColor);
            Button buttonSettings = new Button(buttonSettingsPosition, buttonSize, "Settings", labelSize, _font, buttonColor);
            Button buttonExit = new Button(buttonExitPosition, buttonSize, "Exit", labelSize, _font, buttonColor);

            menuTitle.Position = new Vector2f(_window.Size.X/3 - 100, _window.Size.Y/5);
            menuTitle.FillColor = SFML.Graphics.Color.White;
            copyrightNotice.Position = new Vector2f(_window.Size.X - 175, _window.Size.Y - 25);
            copyrightNotice.FillColor = SFML.Graphics.Color.White;


            _window.Draw(menuTitle);
            _window.Draw(copyrightNotice);

            buttonStart.Draw(_window);
            buttonSettings.Draw(_window);
            buttonExit.Draw(_window);

            //============ Menu buttons event listeners ============

            //============ Menu loop ============
            _window.KeyPressed += Window_KeyPressed;
            _window.MouseButtonPressed += Window_MousePressed;

            while (_window.IsOpen)
            {
                _window.WaitAndDispatchEvents();

                _window.Display();

                //=================== KEYBOARD FEEDBACK ================
                if (Keyboard.IsKeyPressed(Keyboard.Key.R))
                    _window.Close();
                {
                }
            }
        }

        private void SettingsMenu()
        {
            //============ Settings Menu buttons/text ============
            RectangleShape buttonSoundIncrease = new RectangleShape();
            RectangleShape buttonSoundDecrease = new RectangleShape();
            RectangleShape buttonResolution = new RectangleShape();
            RectangleShape buttonBack = new RectangleShape();

            //default menu button size
            Vector2f buttonSize = new Vector2f(_window.Size.X / 15, _window.Size.Y / 20);

            buttonSoundIncrease.Size = buttonSize;
            buttonSoundDecrease.Size = buttonSize;
            buttonResolution.Size = buttonSize;
            buttonBack.Size = buttonSize;

            buttonSoundIncrease.Position = new Vector2f(50, 50);
            buttonSoundDecrease.Position=  new Vector2f(100, 50);
            buttonResolution.Position = new Vector2f(150, 50);
            buttonBack.Position = new Vector2f(170, 50);

            _window.Draw(buttonSoundIncrease);
            _window.Draw(buttonSoundDecrease);
            _window.Draw(buttonResolution);
            _window.Draw(buttonBack);
        }

        private void GameMenu()
        {
            //============ Game Menu buttons/text ============
            RectangleShape buttonStart = new RectangleShape();
            RectangleShape buttonReset = new RectangleShape();
            RectangleShape buttonExit = new RectangleShape();

            RectangleShape signalBucket1 = new RectangleShape();
            RectangleShape signalBucket2 = new RectangleShape();
            RectangleShape signalBucket3 = new RectangleShape();

            Text instructions = new Text("sample instructions", _font, 25);
            Text labelTrapdoor1 = new Text();
            Text labelTrapdoor2 = new Text();
            Text labelTrapdoor3 = new Text();
            Text labelBucketReq1 = new Text("Bucket 1 Requirements", _font);
            Text labelBucketReq2 = new Text("Bucket 2 Requirements", _font);
            Text labelBucketReq3 = new Text("Bucket 3 Requirements", _font);

            //var sample = new Dictionary<GameEntity, Text>();

            instructions.Position = new Vector2f(0, 0);
            instructions.FillColor = SFML.Graphics.Color.White;

            //default menu button size
            Vector2f buttonSize = new Vector2f(_window.Size.X / 15, _window.Size.Y / 20);

            buttonStart.Size = buttonSize;
            buttonReset .Size = buttonSize;
            buttonExit.Size = buttonSize;

            buttonStart.Position = new Vector2f(300, 30);
            buttonReset.Position = new Vector2f(350, 30);
            buttonExit.Position = new Vector2f(00, 30);

            _window.Draw(buttonStart);
            _window.Draw(buttonReset);
            _window.Draw(buttonExit);

        }

        private void switchMenu(Menu menu)
        {
            _activeMenu = menu;
            _window.Clear();

            switch (menu)
            {
                case Menu.Main:
                    MainMenu();
                    break;
                case Menu.Settings:
                    SettingsMenu();
                    break;
                case Menu.Game:
                    GameMenu();
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

        /// <summary>
        /// Function called when mouse key is pressed
        /// </summary>
        private void Window_MousePressed(object sender, SFML.Window.MouseButtonEventArgs mouse)
        {
            Console.Write(mouse.X + " " + mouse.Y);
        }
    }
}