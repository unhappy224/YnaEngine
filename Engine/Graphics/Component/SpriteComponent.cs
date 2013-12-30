using Microsoft.Xna.Framework;

namespace Yna.Engine.Graphics.Component
{
    /// <summary>
    /// Define a base class to make sprite component.
    /// </summary>
    public class SpriteComponent
    {
        protected bool _enabled;
        internal YnSprite Sprite { get; set; }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Create the component.
        /// </summary>
        public SpriteComponent()
        {
            _enabled = true;
        }

        /// <summary>
        /// Default initialization
        /// </summary>
        public virtual void Initialize() { }

        /// <summary>
        /// Update the component. Called after the base update of the Sprite.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Second update called after the first update and just before Draw.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime) { }
    }
}
