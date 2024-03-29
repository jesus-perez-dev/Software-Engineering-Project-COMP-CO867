using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = SFML.Graphics.Color;
using MarbleSorterGame.Drivers;

namespace MarbleSorterGame.Screens
{
    // The game itself
    public class MarbleSorterGameScreen : GameScreen, IDisposable
    {
        private Drawable[] _drawables;
        private GameEntity[] _entities;
        
        private IIODriver _driver;

        // Sensors
        private MotionSensor _motionSensorConveyor;
        private MotionSensor _motionSensorBucket;
        private ColorSensor _colorSensor;
        private PressureSensor _pressureSensor;

        // Other Entities
        private Gate _gateEntrance;
        private SignalLight _gateOpen;
        private SignalLight _gateClosed;
        private Conveyor _conveyor;
        private Marble[] _marbles;
        private Trapdoor[] _trapDoors;
        private SignalLight[] _trapDoorsOpen;
        private SignalLight[] _trapDoorsClosed;
        private Bucket[] _buckets;
        private List<GameEntity> _sensors;
        private List<GameEntity> _hoverable;
        private List<GameEntity> _mappable;
        private List<GameEntity> _signalLights;
        private SignalLight[] _signalLightsColors;
        private SignalLight[] _signalLightsWeights;

        // UI Buttons
        private Button _buttonPause;
        private Button _buttonReset;
        private Button _buttonMainMenu;
        private Button[] _buttons;
        private RectangleShape _menuBarBackground;

        // Legends
        private Legend _legendHovered;
        private GameEntity _hoveredEntity;
        private Dictionary<string, string> _hoveredEntityData;

        private Legend _legendGameState; // reflects _gameState
        
        private Legend _legendGame;
        private Dictionary<string, string> _legendGameData;
        
        private Legend _legendIoMap;
        private Dictionary<string, string> _ioMapData;
        
        private readonly uint _legendPadding = 20;
        private readonly float _legendSpacing;

        // Game state
        private GameState _gameState;
        private List<Marble> _marblesRemaining;
        
        public MarbleSorterGameScreen(RenderWindow window, IAssetBundle bundle, IIODriver driver) : base(window, bundle)
        {
            _legendSpacing = Screen.Percent(0, 1).Y; // 1% vertical distance between other legends and menu bar background
            _driver = driver;
            Reset();
        }

        // Initialize all game entity objects and reset values to default states
        private void Reset()
        {
            // Used for positioning by percentage relative to screen
            var font = Bundle.Font;
            var preset = Bundle.GameConfiguration.Preset;

            _ioMapData = new Dictionary<string, string>();
            _legendGameData = new Dictionary<string, string>();
            _hoveredEntityData = new Dictionary<string, string>();
            
            _gameState = GameState.Progress;
            
            // Enable IO for the driver
            _driver.SetActive(true);

            //================= Event Handlers ====================//
            Window.MouseButtonPressed += GameMouseClickEventHandler;
            Window.KeyPressed += GameKeyEventHandler;
            Window.MouseMoved += GameMouseMoveEventHandler;

            //================= Background widgets ====================//
            // Color of the menu and legend background
            var chromeColor = new SFML.Graphics.Color(218, 224, 226);

            // Menu bar background (slight gray recantgle behind buttons)
            _menuBarBackground = new RectangleShape
            {
                Position = new Vector2f(),
                Size = Screen.Percent(100f, 6f),
                FillColor = chromeColor,
                OutlineColor = Color.Black,
                OutlineThickness = 2
            };

            //================= Buttons ====================//

            Vector2f menuButtonSize = Screen.Percent(8, 4);
            float menuButtonFontScale = 0.4f;
            float menuButtonSpacing = new Vector2f().PercentOfX(Screen, 1f).X;
            
            _buttonPause = new Button("Pause", menuButtonFontScale, font, default, menuButtonSize);
            _buttonReset = new Button("Reset Game", menuButtonFontScale, font, default, menuButtonSize);
            _buttonMainMenu = new Button("Main Menu", menuButtonFontScale, font, default, menuButtonSize);

            // NOTE: Button is centered by *Origin = Center*, which affects our shift values
            _buttonMainMenu.Position = Screen
                .PositionRelative(Joint.End, Joint.Start) // Position top corner of screen
                .ShiftX(-menuButtonSize.X/2) // Shift left so its not over screen edge
                .ShiftX(-menuButtonSpacing) // Add a 1% spacer from screen edge
                .ShiftY((menuButtonSize.Y)/2 + Math.Max(0, _menuBarBackground.Size.Y - menuButtonSize.Y)/2); // Shift down so its vertically centered in menu bar background

            _buttonReset.Position = _buttonMainMenu.Box
                .PositionRelative(Joint.Start, Joint.Start)
                .ShiftX(-menuButtonSize.X)
                .ShiftX(-menuButtonSpacing); // Add a 1% spacer from button edge
            
            _buttonPause.Position = _buttonReset.Box
                .PositionRelative(Joint.Start, Joint.Start)
                .ShiftX(-menuButtonSize.X)
                .ShiftX(-menuButtonSpacing); // Add a 1% spacer from button edge

            _buttonPause.ClickEvent += PauseButtonClickHandler;
            _buttonReset.ClickEvent += ResetButtonClickHandler;
            _buttonMainMenu.ClickEvent += MainMenuButtonClickHandler;
            
            _buttons = new[] { _buttonMainMenu, _buttonPause, _buttonReset};

            var legendGamePosition = _menuBarBackground
                .PositionRelative(Joint.Start, Joint.End)
                .Shift(new Vector2f(_legendSpacing, _legendSpacing));
            
            _legendGame = new Legend(string.Empty, 14, _legendPadding, chromeColor, Color.Black, 2, Bundle.Font, legendGamePosition);

            var legendMappingPosition = _legendGame.Box
                .PositionRelative(Joint.End, Joint.Start);
            _legendIoMap = new Legend(string.Empty, 14, _legendPadding, chromeColor, Color.Black, 2, Bundle.Font, legendMappingPosition);

            var legendHoveredEntityPosition = _menuBarBackground
                .PositionRelative(Joint.End, Joint.Middle)
                //.ShiftX(-_infoBoxBackgroundSize.X - _legendPadding * 2)
                .ShiftY(-_legendSpacing);

            _legendHovered = new Legend(string.Empty, 14, _legendPadding, chromeColor, Color.Black, 2, Bundle.Font, legendHoveredEntityPosition);
            
            _legendGameState = new Legend(_gameState.ToHumanString(), 14, 24, Color.Green, Color.Black, 2, Bundle.Font, default);
            _legendGameState.Position = _menuBarBackground.PositionRelative(Joint.End, Joint.End)
                .ShiftX(-_legendGameState.Box.GetGlobalBounds().Width - _legendSpacing)
                .ShiftY(_legendSpacing);
            
            //================= Game Entities ====================//
            _conveyor = new Conveyor(Screen.Percent(0, 60), Screen.Percent(100, 1), new Vector2f(1, 0));

            _marbles = preset.Marbles
                .Select(mc => new Marble(new Vector2f(40, _conveyor.Position.Y), mc.Color.ToGameColor(), mc.Weight.ToGameWeight()))
                .Reverse()
                .ToArray();
            
            _marblesRemaining = new List<Marble>(_marbles);

            // Set marble initial positions based on screen dimensions
            float offset = 0;
            foreach (var marble in _marbles.Reverse())
            {
                marble.Position = marble.Position
                    .PositionRelativeToShapeY(_conveyor.Box, Joint.Start)
                    .ShiftY(-marble.GlobalBounds.Height)
                    .ShiftX(-offset);
                
                offset += marble.Size.X;
            }

            int bucketCount = preset.Buckets.Count;
            float bucketHorizontalSpaceIncrement = 125.0f / (bucketCount + 2);
            _buckets = preset.Buckets
                .Select((bc, i) => new Bucket(
                    Screen.Percent(bucketHorizontalSpaceIncrement * (i + 1), 100),
                     new Vector2f(Marble.MarbleSizeLarge * 1.5f, Marble.MarbleSizeLarge * 1.5f),
                    bc.Color?.ToGameColor(),
                    bc.Weight?.ToGameWeight(),
                    bc.Capacity
                ))
                .ToArray();

            _trapDoors = _buckets
                .Select((b, i) =>
                    new Trapdoor(
                        Screen.Percent(bucketHorizontalSpaceIncrement * (i + 1), 60),
                        new Vector2f(b.Size.X, _conveyor.Size.Y)))
                .ToArray();

            _trapDoors[0].Name = IOKeys.Q.TrapDoor1;
            _trapDoors[1].Name = IOKeys.Q.TrapDoor2;
            _trapDoors[2].Name = IOKeys.Q.TrapDoor3;

            // Adjust bucket positions to be at very bottom of screen
            foreach (var bucket in _buckets)
                bucket.Position -= new Vector2f(0, bucket.Size.Y);
            
            //Vector2f gateEntranceSize = Screen.Percent(0.75f, 0);
            Vector2f gateEntranceSize = new Vector2f(Marble.MarbleSizeLarge * 0.15f, Marble.MarbleSizeLarge * 1.25f);
            //gateEntranceSize.Y = Marble.MarbleSizeLarge * 1.25f;
            // screen.Percent(13, 52)
            _gateEntrance = new Gate(
                _conveyor.Box
                    .PositionRelative(Joint.Start, Joint.Start)
                    .ShiftX(Screen.Percent(15, 0).X)
                    .ShiftY(-gateEntranceSize.Y), gateEntranceSize);

            Vector2f sensorSize = new Vector2f(Marble.MarbleSizeSmall, Marble.MarbleSizeSmall);
            
            Vector2f gateSensorPosition = _gateEntrance.Box
                .PositionRelative(Joint.Start, Joint.End)
                .ShiftY(-sensorSize.Y)
                .ShiftX(-sensorSize.X);
            var pressureSensorPosition = gateSensorPosition;
            pressureSensorPosition.Y += sensorSize.Y/2;
            var colorSensorPosition = gateSensorPosition;
            colorSensorPosition.Y -= 2;
            
            _pressureSensor = new PressureSensor(pressureSensorPosition, sensorSize/2);
            _colorSensor = new ColorSensor(colorSensorPosition, sensorSize/2);
            
            // Difference between middle of motion sensor and right-edge of gate
            float motionSensorXPosition = MarbleSorterGame.WINDOW_WIDTH - sensorSize.X;
            float motionSensorLaserRange = (_gateEntrance.Position.X + _gateEntrance.Size.X) - (motionSensorXPosition + sensorSize.X / 2);
            _motionSensorConveyor = new MotionSensor(motionSensorLaserRange, new Vector2f(motionSensorXPosition, gateSensorPosition.Y), sensorSize);

            // Position half-way between conveyer and top of buckets
            Vector2f motionSensorPosition = new Vector2f(MarbleSorterGame.WINDOW_WIDTH - sensorSize.X, _buckets[0].Position.Y - sensorSize.Y);
            _motionSensorBucket = new MotionSensor(motionSensorLaserRange, motionSensorPosition, sensorSize);

            // Signal Lights
            var temp = Screen.Percent(2f, 2f);
            Vector2f signalSize = new Vector2f(temp.X, temp.X);

            var trapDoorSizeX = _trapDoors[0].Size.X;
            var signalColor1 = new SignalLight(_gateEntrance.Box.PositionRelative(Joint.Start, Joint.Start).Shift(-signalSize * 1.5f).ShiftX(-signalSize.X * 2f), signalSize);
            var signalColor2 = new SignalLight(signalColor1.Position.ShiftX(-signalSize.X * 2f), signalSize);
            var signalPressure1 = new SignalLight(signalColor1.Position.ShiftY(-signalSize.Y * 3f), signalSize);
            var signalPressure2 = new SignalLight(signalColor2.Position.ShiftY(-signalSize.Y * 3f), signalSize);
            var bucketDropped = new SignalLight(Screen.Percent(115, 100), signalSize );
            var trapdoorOpen1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X + trapDoorSizeX, _trapDoors[0].Position.Y - 50), signalSize);
            var trapdoorOpen2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X + trapDoorSizeX, _trapDoors[1].Position.Y - 50), signalSize);
            var trapdoorOpen3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X + trapDoorSizeX, _trapDoors[2].Position.Y - 50), signalSize);
            var trapdoorClosed1 = new SignalLight(new Vector2f(_trapDoors[0].Position.X - signalSize.X, _trapDoors[0].Position.Y + 50), signalSize);
            var trapdoorClosed2 = new SignalLight(new Vector2f(_trapDoors[1].Position.X - signalSize.X, _trapDoors[1].Position.Y + 50), signalSize);
            var trapdoorClosed3 = new SignalLight(new Vector2f(_trapDoors[2].Position.X - signalSize.X, _trapDoors[2].Position.Y + 50), signalSize);
            _gateOpen = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y - 50), signalSize);
            _gateClosed = new SignalLight(new Vector2f(_gateEntrance.Position.X + gateEntranceSize.X + 10, _trapDoors[0].Position.Y + 25), signalSize);

            signalColor1.Name = IOKeys.I.ColorSensor;
            signalColor2.Name = IOKeys.I.ColorSensor;
            signalPressure1.Name = IOKeys.I.WeightSensor;
            signalPressure2.Name = IOKeys.I.WeightSensor;
            trapdoorOpen1.Name = IOKeys.I.TrapDoor1Open;
            trapdoorOpen2.Name = IOKeys.I.TrapDoor2Open;
            trapdoorOpen3.Name = IOKeys.I.TrapDoor3Open;
            trapdoorClosed1.Name = IOKeys.I.TrapDoor1Closed;
            trapdoorClosed2.Name = IOKeys.I.TrapDoor2Closed;
            trapdoorClosed3.Name = IOKeys.I.TrapDoor3Closed;
            bucketDropped.Name = IOKeys.I.BucketMotionSensor;
            _pressureSensor.Name = IOKeys.I.WeightSensor;
            _colorSensor.Name = IOKeys.I.ColorSensor;
            _motionSensorBucket.Name = IOKeys.I.BucketMotionSensor;
            _motionSensorConveyor.Name = IOKeys.I.ConveyorMotionSensor;
            _gateOpen.Name = IOKeys.I.GateOpen;
            _gateClosed.Name = IOKeys.I.GateClosed;
            _gateEntrance.Name = IOKeys.Q.Gate;
            _conveyor.Name = IOKeys.Q.Conveyor;

            _trapDoorsOpen = new[] {trapdoorOpen1, trapdoorOpen2, trapdoorOpen3};
            _trapDoorsClosed = new[] {trapdoorClosed1, trapdoorClosed2, trapdoorClosed3};

            _sensors = new List<GameEntity>
            {
                _motionSensorBucket,
                _motionSensorConveyor,
                _pressureSensor,
                _colorSensor
            };

            _signalLights = new List<GameEntity>
            {
                trapdoorOpen1,
                trapdoorOpen2,
                trapdoorOpen3,
                trapdoorClosed1,
                trapdoorClosed2,
                trapdoorClosed3,
                bucketDropped,
                _gateOpen,
                _gateClosed,
                signalColor1,
                signalColor2,
                signalPressure1,
                signalPressure2,
            };

            _signalLightsColors = new []
            {
                signalColor1,
                signalColor2,
            };

            _signalLightsWeights = new []
            {
                signalPressure1,
                signalPressure2,
            };

            _entities = new GameEntity[]
            {
                _buttonPause,
                _buttonReset,
                _buttonMainMenu,
                _conveyor,
                _pressureSensor,
                _colorSensor,
                _motionSensorBucket,
                _motionSensorConveyor,
                _gateOpen,
                _gateClosed,
                _legendGame,
                _legendIoMap,
                _legendHovered,
                _legendGameState,
                bucketDropped,
                trapdoorOpen1,
                trapdoorOpen2,
                trapdoorOpen3,
                trapdoorClosed1,
                trapdoorClosed2,
                trapdoorClosed3,
            };

            _entities = _entities.ToList()
                .Concat(_signalLights)
                .Concat(_marbles)
                .Concat(_trapDoors)
                .Concat(_marbles)
                .Concat(_buckets)
                .Concat(new[] { _gateEntrance })
                .ToArray();

            _hoverable = new List<GameEntity>();
            _hoverable.AddRange(_buckets);
            _hoverable.AddRange(_marbles);
            _hoverable.AddRange(_trapDoors);
            _hoverable.AddRange(_signalLights);
            _hoverable.AddRange(_sensors);
            _hoverable.Add(_gateEntrance);

            _mappable = new List<GameEntity>();
            _mappable.AddRange(_trapDoors);
            _mappable.Add(_gateEntrance);
            _mappable.Add(_conveyor);

            _mappable.AddRange(_trapDoorsOpen);
            _mappable.AddRange(_trapDoorsClosed);
            _mappable.Add(_motionSensorBucket);
            _mappable.Add(_gateOpen);
            _mappable.Add(_gateClosed);
            _mappable.Add(_motionSensorConveyor);
            _mappable.Add(_pressureSensor);
            _mappable.Add(_colorSensor);

            //load assets for all entities
            foreach (GameEntity entity in _entities)
                entity.Load(Bundle);

            _drawables = new Drawable[] { _menuBarBackground, };
        }

        // Mouse movement event handler for features involving hover-over
        private void GameMouseMoveEventHandler(object? sender, MouseMoveEventArgs mouse)
        {
            UpdateButtonsFromMouseEvent(Window, _buttons, mouse);
            UpdateHoveredEntityMouseEvent(Window, mouse);
        }

        // Mouse click event handler for button clicking
        private void GameMouseClickEventHandler(object? sender, MouseButtonEventArgs mouse)
        {
            UpdateButtonsFromClickEvent(sender, _buttons, mouse);
        }

        // Keyboard event handlers for keyboard driver, for debug purposes
        private void GameKeyEventHandler(object? sender, KeyEventArgs key)
        {
            if (_driver is KeyboardIODriver kbdriver)
                kbdriver.UpdateByKey(key);
        }

        // Mouse move event for any entity that is hovered over
        private void UpdateHoveredEntityMouseEvent(RenderWindow window, MouseMoveEventArgs mouse)
        {
            _hoveredEntity = null;
            window.SetMouseCursor(MarbleSorterGame.Cursors.Arrow);

            foreach (GameEntity entity in _hoverable)
            {
                if (entity.MouseHovered(new Vector2f(mouse.X, mouse.Y)))
                {
                    _hoveredEntity = entity;
                    window.SetMouseCursor(MarbleSorterGame.Cursors.Help);
                    break;
                }
            }
            
            foreach (Button button in _buttons)
            {
                if (button.MouseHovered(new Vector2f(mouse.X, mouse.Y)))
                {
                    _hoveredEntity = null;
                    window.SetMouseCursor(MarbleSorterGame.Cursors.Hand);
                    break;
                }
            }
        }

        //sets signal state from driver
        public void UpdateSignals()
        {
            _gateEntrance.SetState(_driver.Gate);

            _gateOpen.SetState(_driver.GateOpen);
            _gateClosed.SetState(_driver.GateClosed);
            _conveyor.SetState(_driver.Conveyor);

            _trapDoors[0].SetState(_driver.TrapDoor1);
            _trapDoors[1].SetState(_driver.TrapDoor2);
            _trapDoors[2].SetState(_driver.TrapDoor3);

            _trapDoorsOpen[0].SetState(_driver.TrapDoor1Open);
            _trapDoorsOpen[1].SetState(_driver.TrapDoor2Open);
            _trapDoorsOpen[2].SetState(_driver.TrapDoor3Open);

            _trapDoorsClosed[0].SetState(_driver.TrapDoor1Closed);
            _trapDoorsClosed[1].SetState(_driver.TrapDoor2Closed);
            _trapDoorsClosed[2].SetState(_driver.TrapDoor3Closed);

            //set weight/color sensor lights
            var c = _driver.ColorSensor;
            _signalLightsColors[0].SetState(c == (int)Enums.Color.Blue || c == (int)Enums.Color.Red);
            _signalLightsColors[1].SetState(c == (int)Enums.Color.Blue || c == (int)Enums.Color.Green);

            var w = _driver.PressureSensor;
            _signalLightsWeights[0].SetState(w == (int)Enums.Weight.Large || w == (int)Enums.Weight.Small);
            _signalLightsWeights[1].SetState(w == (int)Enums.Weight.Large || w == (int)Enums.Weight.Medium);
        }

        // Writes driver values to the game and reads in game state 
        private void UpdateDriver()
        {
            _driver.TrapDoor1Open = _trapDoors[0].IsFullyOpen;
            _driver.TrapDoor2Open = _trapDoors[1].IsFullyOpen;
            _driver.TrapDoor3Open = _trapDoors[2].IsFullyOpen;

            _driver.TrapDoor1Closed = _trapDoors[0].IsFullyClosed;
            _driver.TrapDoor2Closed = _trapDoors[1].IsFullyClosed;
            _driver.TrapDoor3Closed = _trapDoors[2].IsFullyClosed;

            _driver.GateClosed = _gateEntrance.IsFullyClosed;
            _driver.GateOpen = _gateEntrance.IsFullyOpen;
            
            //////////////////////////////////////// 
            ///// BEGIN: TODO FIXME HACK

            // This is code that gets the sensors working for the demo presentation
            // It simply checks if a marble overlaps the sensor, if yes, write the value to the driver
            // In the final version this should be re-architected, perhaps using a Sensor.Update(Marble, IIODriver) methods?

            _driver.ConveyorMotionSensor = false;
            _driver.BucketMotionSensor = false;
            _driver.ColorSensor = 0;
            _driver.PressureSensor = 0;
            foreach (var marble in _marbles)
            {
                // Write color of overlapping marble
                if (_colorSensor.Overlaps(marble))
                    _driver.ColorSensor = (byte) marble.Color;
                
                // Write pressure of overlapping marble
                if (_pressureSensor.Overlaps(marble))
                    _driver.PressureSensor = (byte) marble.Weight;
            }
            
            // If marble position is past the gate and its X-value is on the conveyor belt (not falling)
            // write to the conveyor motion sensor
            _motionSensorConveyor.Update(_marbles, _gateEntrance);
            _driver.ConveyorMotionSensor |= _motionSensorConveyor.Detected;

            // If a straight horizontal line drawn from the marble across the screen touches the bucket motion sensor, fire-off the sensor
            _motionSensorBucket.Update(_marbles, _gateEntrance);
            _driver.BucketMotionSensor |= _motionSensorBucket.Detected;
            
            ////// END: TODO FIXME HACK
            ////////////////////////////////////////

            // Update IIODriver instance
            _driver.Update();
        }

        // Update marble positioning
        private void UpdateMarbles()
        {
            foreach (var marble in _marbles)
            {
                // By default, marble should roll right
                marble.SetState(MarbleState.Rolling);

                // If marble is touching gate and gate is closed, do not move
                // Marble can clip through if more than half of marble is past gate
                float marbleRightEdge = marble.Position.X + marble.Size.X;
                if (!_gateEntrance.IsFullyOpen && marbleRightEdge > _gateEntrance.Position.X - _gateEntrance.Size.X*2 //_gateEntrance.Overlaps(marble) 
                                               && marbleRightEdge < _gateEntrance.Position.X + _gateEntrance.Size.X)
                {
                    marble.SetState(MarbleState.Still);
                }
                
                // If the right edge of the marble is passed the left edge of the gate, keep it going
                if (marbleRightEdge > _gateEntrance.Position.X + _gateEntrance.Size.X && !_gateEntrance.IsFullyClosed)
                    marble.SetState(MarbleState.Rolling);
                
                // If conveyor is off, marbles stay still
                if (!_conveyor.ConveyorOn)
                    marble.SetState(MarbleState.Still);
                
                // If marble has started falling, keep it falling
                if (marble.Position.Y + marble.Size.Y > _conveyor.Position.Y)
                    marble.SetState(MarbleState.Falling);
            }

            // If marbles are touching/overlapping each other, the marble farthest to left should stop moving
            // NOTE: This assumes marble order is placed from left-to-right!
            for (var i = 0; i < _marbles.Length - 1; i++)
            {
                var spacing = Marble.MarbleSizeSmall * 2;
                var prevMarbleEdge = _marbles[i].GlobalBounds.Left + _marbles[i].GlobalBounds.Width;
                if (prevMarbleEdge + spacing > _marbles[i + 1].GlobalBounds.Left && _marbles[i+1].GetState() != MarbleState.Falling)
                    _marbles[i].SetState(MarbleState.Still);
            }
                
            
            // If marble is overtop a trap-door, and the trap door is open, switch velocity to falling
            foreach (var marble in _marbles)
                foreach (var trapdoor in _trapDoors)
                {
                    float trapDoorRightEdge = trapdoor.Position.X + trapdoor.Size.X;
                    float marbleCenterX = marble.Position.X + marble.Size.X/2;
                    float marbleRightEdge = marble.Position.X + marble.Size.X;
                    if (trapdoor.IsOpen && marbleCenterX >= trapdoor.Position.X && marbleCenterX <= trapDoorRightEdge && marble.Position.X > trapdoor.Position.X && marbleRightEdge < trapDoorRightEdge)
                        marble.SetState(MarbleState.Falling);
                }

            // Now actually update marble coordinates
            foreach (var m in _marbles)
                m.Update();
            
            // When the marble is complete inside the bucket, insert marble and teleport it offscreen
            foreach (var marble in _marbles)
            {
                foreach (var bucket in _buckets)
                {
                    if (bucket.GlobalBounds.Contains(marble.Position.X + marble.Size.X/2, marble.Position.Y))
                    {
                        // Set marble position offscreen and update bucket counters
                        marble.Position = new Vector2f(50, 50000);
                        marble.SetState(MarbleState.Still);
                        bool success = bucket.InsertMarble(marble);
                        _marblesRemaining.Remove(marble);
                    }
                }
            }
        }

        // Update door positions
        private void UpdateDoors()
        {
            foreach (var trapdoor in _trapDoors)
                trapdoor.Update();

            // Update entrace gate position
            _gateEntrance.Update(_marbles);
        }

        // Update legend text
        private void UpdateLegend()
        {
            //IoMapConfiguration config = Bundle.IoMapConfiguration();
            _legendGameData["Marbles Remaining Total"] = _marblesRemaining.Count.ToString();
            _legendGameData["Marbles Correctly Dropped"] = _buckets.Select(b => b.TotalCorrect).Sum().ToString();
            _legendGameData["Marbles Incorrectly Dropped"] = _buckets.Select(b => b.TotalIncorrect).Sum().ToString();

            _ioMapData[IOKeys.I.TrapDoor1Open] = _driver.TrapDoor1Open.ToString();
            _ioMapData[IOKeys.I.TrapDoor2Open] = _driver.TrapDoor2Open.ToString();
            _ioMapData[IOKeys.I.TrapDoor3Open] = _driver.TrapDoor3Open.ToString();
            _ioMapData[IOKeys.I.TrapDoor1Closed] = _driver.TrapDoor1Closed.ToString();
            _ioMapData[IOKeys.I.TrapDoor2Closed] = _driver.TrapDoor2Closed.ToString();
            _ioMapData[IOKeys.I.TrapDoor3Closed] = _driver.TrapDoor3Closed.ToString();
            _ioMapData[IOKeys.I.BucketMotionSensor] = _driver.BucketMotionSensor.ToString();
            _ioMapData[IOKeys.I.WeightSensor] = _driver.PressureSensor.ToString();
            _ioMapData[IOKeys.I.ColorSensor] = _driver.ColorSensor.ToString();
            _ioMapData[IOKeys.I.ConveyorMotionSensor] = _driver.ConveyorMotionSensor.ToString();
            _ioMapData[IOKeys.I.GateClosed] = _driver.GateClosed.ToString();
            _ioMapData[IOKeys.I.GateOpen] = _driver.GateOpen.ToString();
            _ioMapData[IOKeys.Q.TrapDoor1] = _driver.TrapDoor1.ToString();
            _ioMapData[IOKeys.Q.TrapDoor2] = _driver.TrapDoor2.ToString();
            _ioMapData[IOKeys.Q.TrapDoor3] = _driver.TrapDoor3.ToString();
            _ioMapData[IOKeys.Q.Gate] = _driver.Gate.ToString();
            _ioMapData[IOKeys.Q.Conveyor] = _driver.Conveyor.ToString();

            var legendGameBuilder = new StringBuilder();
            var legendMappingBuilder = new StringBuilder();

            foreach (var stat in _legendGameData)
                legendGameBuilder.AppendLine(string.Format("{0,-40}: {1}", stat.Key, stat.Value));

            foreach (var entity in _mappable)
            {
                var entityConf = Bundle.IoMapConfiguration.Find(e => e.EntityName == entity.Name);
                if (entityConf != null)
                    legendMappingBuilder.AppendLine($"{entityConf.ToAddressString()}-{entityConf.Description,-30}: {_ioMapData[entity.Name]}");
            }

            _legendGame.DisplayedText = legendGameBuilder.ToString();
            _legendIoMap.DisplayedText = legendMappingBuilder.ToString();

            _legendIoMap.Position = _legendGame.Box
                .PositionRelative(Joint.End, Joint.Start)
                .ShiftX(_legendSpacing);
            
            _legendHovered.Hidden = true;
            if (_hoveredEntity != null)
            {
                var hoveredItemStringBuilder = new StringBuilder();
                var config = Bundle.IoMapConfiguration.Find(e => e.EntityName == _hoveredEntity.Name);
                _hoveredEntityData["Description"] = config?.Description ?? _hoveredEntity.Name;
                _hoveredEntityData["IO Address"] = config?.ToAddressString();
                
                foreach (var (key,value) in _hoveredEntityData) 
                    if (!string.IsNullOrEmpty(value))
                        hoveredItemStringBuilder.AppendLine($"{key}: {value}");
                
                _legendHovered.DisplayedText = hoveredItemStringBuilder.ToString();
                _legendHovered.Hidden = false;
                _legendHovered.Position = _legendIoMap.Box
                    .PositionRelative(Joint.End, Joint.Start)
                    .ShiftX(_legendSpacing);
            }
            
            _legendGameState.DisplayedText = _gameState.ToHumanString();
            _legendGameState.BackgroundColor = _gameState.ToIndicatorColor();
            _legendGameState.Position = _menuBarBackground.PositionRelative(Joint.End, Joint.End)
                .ShiftX(-_legendGameState.Box.GetGlobalBounds().Width - _legendSpacing)
                .ShiftY(_legendSpacing);
        }

        // Update game state from key events such as bucket handling and win state
        public void UpdateGameState()
        {
            // Once game state is win or lose, this method cannot change it
            if (_gameState == GameState.Win || _gameState == GameState.Lose)
                return;
            
            // If there are no marbles remaining, and there are no buckets with incorrect marbles, set win state
            if (_marblesRemaining.Count == 0 && _buckets.Select(b => b.TotalIncorrect).Sum() == 0)
            {
                _gameState = GameState.Win;
                return;
            }

            // if any buckets have gained an incorrect marble, set state to lose
            if (_buckets.Select(b => b.TotalIncorrect).Sum() > 0)
            {
                _gameState = GameState.Lose;
                return;
            }

            // Maybe find a marble that has gone past screen edge
            Marble? maybeOverEdgeMarble = _marblesRemaining.Find(m => m.Position.X - m.Radius > Window.Size.X);

            if (maybeOverEdgeMarble == null)
                return;

            // Remove it from marbles remaining list
            _marblesRemaining.Remove(maybeOverEdgeMarble);

            // If edge marble marble could have successfully entered a bucket, set lose state
            if (_buckets.Any(b => b.ValidateMarble(maybeOverEdgeMarble)))
            {
                _gameState = GameState.Lose;
            }
        }

        // Update all game entity positions, game state and driver input/output
        public override void Update()
        {
            UpdateLegend();

            if (_gameState != GameState.Pause)
            {
                UpdateDriver();
                UpdateSignals();
                UpdateMarbles();
                UpdateDoors();
                UpdateGameState(); // Win, lose, or pause
            }
        }

        // Redraw screen in preparation for next update loop
        public override void Draw(RenderWindow window)
        {
            foreach (var drawable in _drawables)
                window.Draw(drawable);
            
            foreach (var entity in _entities) 
                entity.Render(window);
        }

        // Event handler for pause button click
        private void PauseButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            if (_gameState == GameState.Pause)
                _gameState = GameState.Progress;
            else if (_gameState == GameState.Progress)
                _gameState = GameState.Pause;
        }

        // Event handler for reset button click
        private void ResetButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            Dispose();
            Reset();
        }

        // Event handler for main menu button click
        private void MainMenuButtonClickHandler(object? sender, MouseButtonEventArgs args)
        {
            Dispose();
            MarbleSorterGame.ActiveMenu = Menu.Main;
        }

        // Dispose all event handlers associated with the game screen
        public void Dispose()
        {
            _buttonPause.ClickEvent -= PauseButtonClickHandler;
            _buttonReset.ClickEvent -= ResetButtonClickHandler;
            _buttonMainMenu.ClickEvent -= MainMenuButtonClickHandler;
            Window.MouseButtonPressed -= GameMouseClickEventHandler;
            Window.KeyPressed -= GameKeyEventHandler;
            Window.MouseMoved -= GameMouseMoveEventHandler;
        }
    }
}
