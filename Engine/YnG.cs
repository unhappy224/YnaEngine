// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Audio;
using Yna.Engine.Input;
using Yna.Engine.State;
using Yna.Engine.Storage;

namespace Yna.Engine
{
    /// <summary>
    /// Static class that expose important object relative to the current context like
    /// Game, GraphicsDevice, Input, etc...
    /// </summary>
    public static class YnG
    {
        /// <summary>
        /// Gets or Set the Game instance
        /// </summary>
        public static Game Game { get; set; }

        /// <summary>
        /// Gets the GraphicsDevice instance relative to the Game object
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get { return Game.GraphicsDevice; }
        }

        #region Managers

        /// <summary>
        /// Gets the GraphicsDeviceManager relative to the Game object
        /// </summary>
        public static GraphicsDeviceManager GraphicsDeviceManager { get; set; }

        /// <summary>
        /// Gets the ContentManager instance relative to the Game object
        /// </summary>
        public static ContentManager Content
        {
            get { return Game.Content; }
        }

        /// <summary>
        /// Gets or Set the State Manager
        /// </summary>
        public static YnStateManager StateManager { get; set; }

        /// <summary>
        /// Gets or Set the audio manager
        /// </summary>
        public static AudioManager AudioManager { get; set; }

        /// <summary>
        /// Gets or sets the storage manager
        /// </summary>
        public static StorageManager StorageManager { get; set; }

        #endregion

        #region Inputs

        /// <summary>
        /// Gets or Set the keyboard states
        /// </summary>
        public static YnKeyboard Keys { get; set; }

        /// <summary>
        /// Gets or Set the mouse states
        /// </summary>
        public static YnMouse Mouse { get; set; }

        /// <summary>
        /// Gets or Set the Gamepad states
        /// </summary>
        public static YnGamepad Gamepad { get; set; }

        /// <summary>
        /// Gets or Set the Touch states
        /// </summary>
        public static YnTouch Touch { get; set; }

        #endregion

        #region Screen size

        /// <summary>
        /// Gets the width of the current viewport
        /// </summary>
        public static int Width
        {
            get { return YnScreen.Width; }
        }

        /// <summary>
        /// Gets the height of the current viewport
        /// </summary>
        public static int Height
        {
            get { return YnScreen.Height; }
        }

        #endregion

        #region StateManager

        public static void SetStateActive(string name, bool desactiveOtherStates)
        {
            if (StateManager != null)
                StateManager.SetActive(name, desactiveOtherStates);
        }

        #endregion

        /// <summary>
        /// Close the game
        /// </summary>
        public static void Exit()
        {
            Game.Exit();
        }
    }
}
