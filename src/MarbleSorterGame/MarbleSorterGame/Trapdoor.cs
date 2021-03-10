using System;
using SFML.Graphics;
using SFML.System;

namespace MarbleSorterGame
{
    public class Trapdoor : GameEntity
    {
        private Sprite _sprite;
        public bool Open;

        public Trapdoor(Vector2f position, Vector2f size) : base(position, size)
        {
            Open = false;
        }

        public void Toggle()
        {
            Open = !Open;
        }

        public override void Render(RenderWindow window)
        {
            if (_sprite == null)
                return;
            
            window.Draw(_sprite);
        }

        public override void Load(IAssetBundle bundle)
        {
        }
    }
}
