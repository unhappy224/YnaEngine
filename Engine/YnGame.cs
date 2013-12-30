// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Audio;
using Yna.Engine.State;
using Yna.Engine.Input;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    /// <summary>
    /// The game class
    /// </summary>
    public class YnGame : Game
    {
        protected GraphicsDeviceManager graphics = null;
        protected SpriteBatch spriteBatch = null;
        protected YnStateManager stateManager = null;
        public static string GameTitle = "Yna Game";
        public static string GameVersion = "1.0.0.0";

        #region Constructors

        /// <summary>
        /// Create and setup the game engine
        /// Graphics, Services and helpers are initialized
        /// </summary>
        public YnGame()
            : base()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.stateManager = new YnStateManager(this);

            YnKeyboard keyboardComponent = new YnKeyboard(this);
            YnMouse mouseComponent = new YnMouse(this);
            YnGamepad gamepadComponent = new YnGamepad(this);
            YnTouch touchComponent = new YnTouch(this);

            Components.Add(keyboardComponent);
            Components.Add(mouseComponent);
            Components.Add(gamepadComponent);
            Components.Add(touchComponent);
            Components.Add(stateManager);

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDeviceManager = this.graphics;
            YnG.Keys = keyboardComponent;
            YnG.Mouse = mouseComponent;
            YnG.Gamepad = gamepadComponent;
            YnG.Touch = touchComponent;
            YnG.StateManager = stateManager;
            YnG.StorageManager = new StorageManager();
            YnG.AudioManager = new AudioManager();

#if !ANDROID
            this.Window.Title = String.Format("{0} - v{1}", GameTitle, GameVersion);
#endif
            YnScreen.Setup(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false);

#if WINDOWS_PHONE_7
            // 30 FPS for Windows Phone 7
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Battery saving when screen suspended
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif
        }

        public YnGame(int width, int height, string title)
            : this(width, height, width, height, title) 
        {

        }

        public YnGame(int width, int height, int referenceWidth, int referenceHeight, string title)
            : this()
        {
#if !WINDOWS_PHONE && !ANDROID
            YnScreen.Setup(width, height, referenceWidth, referenceHeight, true);
            this.Window.Title = title;
#endif
        }

        #endregion

        #region GameState pattern

        /// <summary>
        /// Load assets from content manager
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsDevice.Viewport = new Viewport(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
        }

        /// <summary>
        /// Unload assets off content manager and suspend managers
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            YnG.AudioManager.Dispose();
        }

        #endregion
    }
}

