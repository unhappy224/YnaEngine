using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine;
using Yna.Engine.Graphics2D;
using Yna.Engine.State;

namespace Yna.Engine.Winforms
{
    public class YnGameControl : GraphicsDeviceControl
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        /// <summary>
        /// Initializes the control, creating the ContentManager
        /// and using it to load a SpriteFont.
        /// </summary>
        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            YnG.Content = content;
            YnG.GraphicsDevice = GraphicsDevice;
        }

        /// <summary>
        /// Disposes the control, unloading the ContentManager.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                content.Unload();
            
            base.Dispose(disposing);
        }

        /// <summary>
        /// Draws the control, using SpriteBatch and SpriteFont.
        /// </summary>
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }
    }
}
