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
        
        // TODO: Pass a game configuration structure in here instead of width/height uints
        public GameScreen(AssetBundleLoader bundle, uint screenWidth, uint screenHeight)
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

            //================= Game Entities ====================//

            String instructionsText = "Use the Input/Output Addresses shown below to create a working PLC for the marble sorter, \nbased on the requirements on the buckets below.";
            Label instructions = new Label(
                instructionsText,
                sizer.Percent(35, 3),
                10,
                SFML.Graphics.Color.Black,
                font);

            //instructions.Draw(window);

            Conveyor conveyor1 = new Conveyor(
                sizer.Percent(0, 60),
                sizer.Percent(100, 1),
                new Vector2f(1, 0)
                );

            Trapdoor trapdoor1 = new Trapdoor(
                sizer.Percent(27, 60),
                sizer.Percent(6, 1)
                );

            Trapdoor trapdoor2 = new Trapdoor(
                sizer.Percent(51, 60),
                sizer.Percent(6, 1)
                );

            Trapdoor trapdoor3 = new Trapdoor(
                sizer.Percent(76, 60),
                sizer.Percent(6, 1)
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
                sizer.Percent(13, 50),
                sizer.Percent(1, 7)
                );

            PressureSensor startSensorPressure = new PressureSensor(
                sizer.Percent(5, 55),
                sizer.Percent(3, 3)
                );

            ColorSensor startSensorColor = new ColorSensor(
                sizer.Percent(7, 55),
                sizer.Percent(3, 3)
                );

            MotionSensor endSensorMotion = new MotionSensor(
                sizer.Percent(20, 100),
                sizer.Percent(0, 0)
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

            Vector2f signalSize = sizer.Percent(3, 8);
            SignalLight gateOpen = new SignalLight(
                sizer.Percent(10, 20),
                signalSize
                );
            
            SignalLight gateClosed = new SignalLight(
                sizer.Percent(gateOpen.Position.X + signalSize.X, gateOpen.Position.Y),
                signalSize
                );

            SignalLight conveyerOn = new SignalLight(
                sizer.Percent(15, 80),
                signalSize
                );

            SignalLight bucketDropped = new SignalLight(
                sizer.Percent(50, 80),
                signalSize
                );

            SignalLight trapdoorOpen1 = new SignalLight(
                new Vector2f(trapdoor1.Position.X, trapdoor1.Position.Y - 20),
                signalSize
                );

            SignalLight trapdoorClosed1 = new SignalLight(
                new Vector2f (trapdoorOpen1.Position.X + signalSize.X, trapdoorOpen1.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen2 = new SignalLight(
                new Vector2f(trapdoor2.Position.X, trapdoor2.Position.Y - 20),
                signalSize
                );

            SignalLight trapdoorClosed2 = new SignalLight(
                new Vector2f(trapdoor2.Position.X + signalSize.X, trapdoorOpen2.Position.Y),
                signalSize
                );

            SignalLight trapdoorOpen3 = new SignalLight(
                new Vector2f(trapdoor3.Position.X, trapdoor3.Position.Y - 20),
                signalSize
                );

            SignalLight trapdoorClosed3 = new SignalLight(
                new Vector2f(trapdoorOpen3.Position.X + signalSize.X, trapdoorOpen3.Position.Y),
                signalSize
                );

            SignalLight marblePassed = new SignalLight(
                sizer.Percent(90, 70),
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
                startSensorColor,
                startSensorPressure,
                endSensorMotion,
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
                marblePassed
            };

            foreach (GameEntity entity in _entities)
            {
                entity.Load(bundle);

            }
            // Game Instructions
            //Text instructions = QuickShape.Label("sample instructions", sizer.Percent(0,0), font, SFML.Graphics.Color.Black);
            
            _drawables = new Drawable[]
            {
                menuBarBackground,
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

            //============ Menu buttons event handlers ============
            EventHandler<SFML.Window.MouseButtonEventArgs> Game_MousePressed = (Object sender, SFML.Window.MouseButtonEventArgs mouse) =>
            {
                Button buttonStart = (Button)_entities[0];
                Button buttonReset = (Button)_entities[1];
                Button buttonExit = (Button)_entities[2];
                if (buttonStart.IsPressed(mouse.X, mouse.Y))
                {
                }
                else if (buttonReset.IsPressed(mouse.X, mouse.Y))
                {
                    window.Close();
                }
                else if (buttonExit.IsPressed(mouse.X, mouse.Y))
                {
                    window.Close();
                }
            };

            window.MouseButtonPressed += Game_MousePressed;
        }
    }
}