using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics;

namespace Yna.Engine.Window.Winforms
{
    public abstract class YnWinformsControl : GraphicsDeviceControl
    {
        protected SpriteBatch spriteBatch;
        // TODO: Move GameTime here

        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            YnG.Content = new ContentManager(Services, "Content"); ;
            YnG.GraphicsDevice = GraphicsDevice;
            YnScreen.Width = Width;
            YnScreen.Height = Height;
        }

        /// <summary>
        /// Disposes the control, unloading the ContentManager.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                YnG.Content.Unload();

            base.Dispose(disposing);
        }
    }
}