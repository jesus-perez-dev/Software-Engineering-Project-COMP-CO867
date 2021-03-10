using System;
using System.Collections.Generic;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    /// <summary>
    /// The game itself
    /// </summary>
    public class GameScreen
    {
        private Drawable[] _drawables = { };
        private GameEntity[] _entities = { };

        private EventHandler<SFML.Window.MouseButtonEventArgs> _game_mouseEvent;
        private EventHandler<SFML.Window.KeyEventArgs> _game_keyEvent;

        // TODO: Pass a game configuration structure in here instead of width/height uints
        public GameScreen(RenderWindow window, AssetBundleLoader bundle, uint screenWidth, uint screenHeight)
        {
            Font font = bundle.Font;
            Sizer sizer = new Sizer(screenWidth, screenHeight);
            
            //================= Background widgets ====================//
            // Menu bar background (slight gray recantgle behind buttons)
            RectangleShape menuBarBackground = new RectangleShape
            {
                Position = sizer.Percent(0f, 0f),
                Size = sizer.Percent(100f, 6f),
                FillColor = new SFML.Graphics.Color(89, 105, 115) // dark-blue-gray ish color
            };

            //================= Buttons ====================//
            Button buttonStart;
            Button buttonReset;
            Button buttonExit;

            buttonStart = new Button(
                "Start Simulation",
                0.4f,
                font,
                sizer.Percent(60, 3),
                sizer.Percent(13, 5)
            );

            buttonReset = new Button(
                "Reset Game",
                0.4f,
                font,
                sizer.Percent(75, 3),
                sizer.Percent(13, 5)
                );

            buttonExit = new Button(
                "Exit Game",
                0.4f,
                font,
                sizer.Percent(90, 3),
                sizer.Percent(13, 5)
            );

            //================= Labels ====================//
            String instructionsText = "Use the Input/Output Addresses shown below to create a working \nPLC for the marble sorter, based on the requirements on the buckets below.";

            Label instructions = new Label(
                instructionsText,
                sizer.Percent(30, 3),
                10,
                SFML.Graphics.Color.Black,
                font);

            Label infobar = new Label(
                instructionsText,
                sizer.Percent(30, 3),
                10,
                SFML.Graphics.Color.Black,
                font);


            //================= Game Entities ====================//

            Vector2f trapdoorSize = sizer.Percent(8, 1);
            Vector2f gateEntranceSize = sizer.Percent(1, 9);
            Vector2f signalSize = sizer.Percent(3, 8);
            Vector2f sensorSize = new Vector2f(20, 20);

            Conveyor conveyor1 = new Conveyor(
                sizer.Percent(0, 60),
                sizer.Percent(100, 1),
                new Vector2f(1, 0)
                );

            Trapdoor trapdoor1 = new Trapdoor(
                sizer.Percent(27, 60),
                trapdoorSize
                );

            Trapdoor trapdoor2 = new Trapdoor(
                sizer.Percent(51, 60),
                trapdoorSize
                );

            Trapdoor trapdoor3 = new Trapdoor(
                sizer.Percent(76, 60),
                trapdoorSize
                );

            Bucket bucket1 = new Bucket(
                sizer.Percent(25, 80),
                sizer.Percent(10, 20),
                Color.Red,
                Weight.Large,
                20
                );

            Bucket bucket2 = new Bucket(
                sizer.Percent(50, 80),
                sizer.Percent(10, 20),
                Color.Red,
                Weight.Large,
                20
                );

            Bucket bucket3 = new Bucket(
                sizer.Percent(75, 80),
                sizer.Percent(10, 20),
                Color.Red,
                Weight.Large,
                20
                );

            Gate gateEntrance = new Gate(
                sizer.Percent(13, 52),
                gateEntranceSize
                );

            PressureSensor sensorPressureStart = new PressureSensor(
                sizer.Percent(3, 55),
                sensorSize
                );

            ColorSensor sensorColorStart = new ColorSensor(
                sizer.Percent(6, 55),
                sensorSize
                ) ;

            MotionSensor sensorMotionEnd = new MotionSensor(
                sizer.Percent(94, 55),
                sensorSize
                );

            MotionSensor sensorMotionBucket1 = new MotionSensor(
                sizer.Percent(25, 100),
                sizer.Percent(0, 0)
                );

            MotionSensor sensorMotionBucket2 = new MotionSensor(
                sizer.Percent(45, 100),
                sizer.Percent(0, 0)
                );

            MotionSensor sensorMotionBucket3 = new MotionSensor(
                sizer.Percent(65, 100),
                sizer.Percent(0, 0)
                );

            SignalLight signalColor1 = new SignalLight(
                sizer.Percent(30, 20),
                signalSize
                );

            SignalLight signalColor2 = new SignalLight(
                new Vector2f(signalColor1.Position.X + signalSize.X, signalColor1.Position.Y),
                signalSize
                );

            SignalLight signalPressure1 = new SignalLight(
                new Vector2f(signalColor1.Position.X + 100, signalColor1.Position.Y),
                signalSize
                );

            SignalLight signalMotion1 = new SignalLight(
                sizer.Percent(95, 50),
                signalSize
                );

            SignalLight gateOpen = new SignalLight(
                sizer.Percent(10, 40),
                signalSize
                );
            
            SignalLight gateClosed = new SignalLight(
                new Vector2f(gateOpen.Position.X + signalSize.X, gateOpen.Position.Y),
                signalSize
                );

            SignalLight conveyerOn = new SignalLight(
                sizer.Percent(5, 75),
                signalSize
                );

            SignalLight bucketDropped = new SignalLight(
                sizer.Percent(95, 90),
                signalSize
                );

            SignalLight trapdoorOpen1 = new SignalLight(
                new Vector2f(trapdoor1.Position.X, trapdoor1.Position.Y - 60),
                signalSize
                );

            SignalLight trapdoorClosed1 = new SignalLight(
                new Vector2f (trapdoorOpen1.Position.X + signalSize.X, trapdoorOpen1.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen2 = new SignalLight(
                new Vector2f(trapdoor2.Position.X, trapdoor2.Position.Y - 60),
                signalSize
                );

            SignalLight trapdoorClosed2 = new SignalLight(
                new Vector2f(trapdoor2.Position.X + signalSize.X, trapdoorOpen2.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen3 = new SignalLight(
                new Vector2f(trapdoor3.Position.X, trapdoor3.Position.Y - 60),
                signalSize
                );

            SignalLight trapdoorClosed3 = new SignalLight(
                new Vector2f(trapdoorOpen3.Position.X + signalSize.X, trapdoorOpen3.Position.Y),
                signalSize
                );

            _entities = new GameEntity[]
            {
                buttonStart,
                buttonReset,
                buttonExit,
                conveyor1,
                trapdoor1,
                trapdoor2,
                trapdoor3,
                bucket1,
                bucket2,
                bucket3,
                gateEntrance,
                sensorColorStart,
                sensorPressureStart,
                signalColor1,
                signalColor2,
                signalPressure1,
                sensorMotionEnd,
                sensorMotionBucket1,
                sensorMotionBucket2,
                sensorMotionBucket3,
                gateOpen,
                gateClosed,
                conveyerOn,
                bucketDropped,
                trapdoorOpen1,
                trapdoorClosed1,
                trapdoorOpen2,
                trapdoorClosed2,
                trapdoorOpen3,
                trapdoorClosed3,
                signalMotion1
            };

            foreach (GameEntity entity in _entities)
            {
                entity.Load(bundle);
            }
            
            _drawables = new Drawable[]
            {
                menuBarBackground,
                instructions,
                infobar
            };

            //============ Mouse buttons event handlers ============
            _game_mouseEvent = (Object sender, SFML.Window.MouseButtonEventArgs mouse) =>
            {
                if (buttonStart.IsPressed(mouse.X, mouse.Y))
                {
                    /**
                    foreach(GameEntity entity in _entities)
                    {
                        if (entity is Marble)
                        {
                            Marble marble = (Marble)entity;
                            marble.Move();
                            marble.Rotate(2f);
                        }
                    }
                    */
                }
                else if (buttonReset.IsPressed(mouse.X, mouse.Y))
                {
                }
                else if (buttonExit.IsPressed(mouse.X, mouse.Y))
                {
                    window.Close();
                }
            };

            //============ Keyboard buttons event handlers ============
            _game_keyEvent = (Object sender, SFML.Window.KeyEventArgs key) =>
            {
                if (key.Code == SFML.Window.Keyboard.Key.R)
                {
                    gateEntrance.Open(2f);
                    trapdoor1.Open(2f);
                }
                else if (key.Code == SFML.Window.Keyboard.Key.C)
                {
                    gateEntrance.Close(2f);
                    trapdoor1.Close(2f);
                }
            };

        }
        
        
        /// <summary>
        /// Method that gets called when the screen is to be redrawn
        /// </summary>
        /// <param name="window"></param>
        /// <param name="font"></param>
        public void Draw(RenderWindow window, Font font)
        {
            foreach (var drawable in _drawables)
            {
                window.Draw(drawable);
            }
            
            foreach (var entity in _entities) 
            {
                entity.Render(window);
            }

            window.MouseButtonPressed += _game_mouseEvent;
            window.KeyPressed += _game_keyEvent;
        }
    }
}