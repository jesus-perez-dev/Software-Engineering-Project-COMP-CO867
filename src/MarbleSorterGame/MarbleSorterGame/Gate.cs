using SFML.Graphics;
using SFML.System;
using System;

namespace MarbleSorterGame
{
    public class Gate : GameEntity
    {
        public bool ControlState;
        private RectangleShape _gate;

        public Gate(Vector2f position, Vector2f size) : base(position,size)
        {
            _gate = new RectangleShape(size);
            _gate.Position = position;
        }

        /// <summary>
        /// Toggles gate switches indicates whether it is open (true) or closed (false)
        /// </summary>
        public void Toggle()
        {
            ControlState = !ControlState;
        }

        public void Open()
        {

        }

        public void Close()
        {

        }

        public override void Render(RenderWindow window)
        {
            window.Draw(_gate);
        }

        public override void Load(IAssetBundle bundle)
        {
        }

    }

}
