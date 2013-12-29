using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Component
{
    public class SpriteComponent
    {
        public bool Enabled { get; set; }
        internal YnSprite Sprite { get; set; }

        public SpriteComponent()
        {
            Enabled = true;
        }

        internal virtual void Initialize() { }
        internal virtual void Update(GameTime gameTime) { }
        internal virtual void PostUpdate(GameTime gameTime) { }
    }
}
