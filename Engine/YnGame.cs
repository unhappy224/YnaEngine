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
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Registry globals objects
            YnG.Game = this;
            YnG.GraphicsDevice = GraphicsDevice;
            YnG.GraphicsDeviceManager = graphics;
            YnG.Keys = new YnKeyboard(this);
            YnG.Mouse = new YnMouse(this);
            YnG.Gamepad = new YnGamepad(this);
            YnG.Touch = new YnTouch(this);
            YnG.AudioManager = new AudioManager();
            YnG.Content = Content;
            YnG.StateManager = new YnStateManager(this); ;
            YnG.StorageManager = StorageManager.Instance;

            Components.Add(YnG.Keys);
            Components.Add(YnG.Mouse);
            Components.Add(YnG.Gamepad);
            Components.Add(YnG.Touch);
            Components.Add(YnG.StateManager);

            YnScreen.Setup(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false);
            Window.Title = string.Format("{0} {1}", GameTitle, GameVersion);

#if WINDOWS_PHONE_7
            // 30 FPS for Windows Phone 7
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Battery saving when screen suspended
            InactiveSleepTime = TimeSpan.FromSeconds(1);
#endif
        }

        public YnGame(YnBootstrap boot)
            : this()
        {
            boot.Load();
            boot.Apply();
        }

        public YnGame(int width, int height, string title)
            : this(width, height, width, height, title) 
        {
        }

        public YnGame(int width, int height, int referenceWidth, int referenceHeight, string title)
            : this()
        {
#if !WINDOWS_PHONE_7 && !WINDOWS_PHONE_8 && !ANDROID
            YnScreen.Setup(width, height, referenceWidth, referenceHeight, true);
            this.Window.Title = title;
#endif
        }

        #endregion

        #region GameState pattern

        protected override void Initialize()
        {
            if (YnG.GraphicsDevice == null)
                YnG.GraphicsDevice = GraphicsDevice;

            base.Initialize();
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

