﻿using System;
using MarbleSorterGame.Enums;
using MarbleSorterGame.GameEntities;
using MarbleSorterGame.Utilities;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Color = MarbleSorterGame.Enums.Color;

//using Color = SFML.Graphics.Color;

namespace MarbleSorterGame
{

    /// <summary>
    /// Object that is being sorted in the game
    /// Rolls right on to the conveyer and is dropped via trapdoors onto the buckets below
    /// </summary>
    public class Marble : GameEntity
    {
        private Sprite _sprite;
        public float Radius { get; }
        public Color Color { get; }
        public Weight Weight { get; }
        private Vector2f _velocity;

        private float _marblePeriod = 30f;
        private float StepX => GameLoop.WINDOW_WIDTH / _marblePeriod / GameLoop.FPS;
        private float StepY => GameLoop.WINDOW_HEIGHT / _marblePeriod / GameLoop.FPS * 2;

        public static readonly float MarbleSizeLarge = GameLoop.WINDOW_WIDTH / 20f;
        public static readonly float MarbleSizeMedium = GameLoop.WINDOW_WIDTH / 30f;
        public static readonly float MarbleSizeSmall = GameLoop.WINDOW_WIDTH / 40f;


        /// <summary>
        /// Constructor for marble
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="weight"></param>
        public Marble(RectangleShape screen, Vector2f position, Color color, Weight weight)//: base(position, size)
        {
            Color = color;
            Weight = weight;

            Vector2f size = new Vector2f(0, 0);

            if (weight == Weight.Large)
                size = new Vector2f(MarbleSizeLarge, MarbleSizeLarge); //sizer.Percent(5 ,5);

            if (weight == Weight.Medium)
                size = new Vector2f(MarbleSizeMedium, MarbleSizeMedium); //sizer.Percent(4.5f, 4.5f);
            
            if (weight == Weight.Small)
                size = new Vector2f(MarbleSizeSmall,MarbleSizeSmall); //sizer.Percent(3.5f, 3.5f);
            
            Position = position;
            Radius = size.X / 2;
            base.Size = size;
        }

        /// <summary>
        /// Sets state of movement, whether it is moving/falling/stopped
        /// </summary>
        /// <param name="state"></param>
        public void SetState(MarbleState state)
        {
            if (state == MarbleState.Still)
                _velocity = new Vector2f(0,0);
            if (state == MarbleState.Rolling)
                _velocity = new Vector2f(StepX,0);
            if (state == MarbleState.Falling)
                _velocity = new Vector2f(0,StepY);
        }
        
        /// <summary>
        /// Sets marble size
        /// </summary>
        public override Vector2f Size
        {
            get => Box.Size;
            set
            {
                _sprite.Scale = RescaleSprite(value, _sprite);
                Box.Size = value;
            }
        }

        /// <summary>
        /// Increment marble position by velocity and rotate accordingly
        /// </summary>
        public void Update()
        {
            Position = new Vector2f(Position.X + _velocity.X, Position.Y + _velocity.Y);
            _sprite.Position = new Vector2f(Position.X + Size.X/2, Position.Y + Size.Y/2);
            _sprite.Rotation += _velocity.X;
        }

        /// <summary>
        /// Render method for marble
        /// </summary>
        /// <param name="window"></param>
        public override void Render(RenderWindow window)
        {
            //base.Render(window);
            //Box.OutlineColor = SFML.Graphics.Color.Green;
            //Box.OutlineThickness = 2;
            window.Draw(_sprite);
        }

        /// <summary>
        /// Extracts marble texture from bundle from chosen color and scales it correctly to marble dimension
        /// </summary>
        /// <param name="bundle"></param>
        public override void Load(IAssetBundle bundle)
        {
            _marblePeriod = bundle.GameConfiguration.MarblePeriod;
                
            Texture texture = null;
            switch (Color)
            {
                case Color.Red:
                    texture = bundle.MarbleRedTexture;
                    break;
                case Color.Blue:
                    texture = bundle.MarbleBlueTexture;
                    break;
                case Color.Green:
                    texture = bundle.MarbleGreenTexture;
                    break;
                default:
                    throw new ArgumentException("Undefined texture for marble color: " + Color);
            }
            
            //_marble.TextureRect = new IntRect(0, 0, 100, 100);
            texture.Smooth = false;

            _sprite = new Sprite(texture);
            _sprite.Position = Position;

            //center marble origin
            _sprite.Origin = new Vector2f(_sprite.Texture.Size.X / 2, _sprite.Texture.Size.Y / 2);
            //EntityRectOrigin = _sprite.Origin;

            //scale texture to correct dimensions
            _sprite.Scale = ScaleEntity(texture);
        }
    }
}
