using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace MarbleSorterGame
{
    public class Marble : GameEntity
    {
        private Texture _texture;
        public float Radius;
        public Color Color;
        public Weight Weight;

        //parameter should be int instead? otherwise main game would have to reference enum class instead of this one
        public Marble(float radius, Color color, Weight weight)
        {
            this.Radius = radius;
            //add enum.isDefined for more strict type check? color/weight must be < 3
            this.Color = color;
            this.Weight = weight;
        }

        public void Render(Window RenderWindow)
        {
            RenderWindow.Display();
        }

        public void Load()
        {

        }
    }
}
