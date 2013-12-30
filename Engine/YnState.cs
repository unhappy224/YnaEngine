// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.State
{
    /// <summary>
    /// A basic state used with the state manager
    /// A state represents a game screen as a menu, a scene or a score screen.
    /// </summary>
    public abstract class YnState : YnGameObject
    {
        private static int ScreenCounter = 0;
        protected SpriteBatch spriteBatch;
        protected StateManager stateManager;


        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
            internal set { spriteBatch = value; }
        }

        /// <summary>
        /// Gets or sets the Screen Manager
        /// </summary>
        public StateManager StateManager
        {
            get { return stateManager; }
            set
            {
                stateManager = value;
                spriteBatch = value.SpriteBatch;
            }
        }

        #region Events

        /// <summary>
        /// Triggered when the state has begin to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingStarted = null;

        /// <summary>
        /// Triggered when the state has finish to load content.
        /// </summary>
        public event EventHandler<EventArgs> ContentLoadingFinished = null;

        /// <summary>
        /// Called when the state has begin to load content.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnContentLoadingStarted(EventArgs e)
        {
            if (ContentLoadingStarted != null)
                ContentLoadingStarted(this, e);
        }

        /// <summary>
        /// Called when the state has finish to load content.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnContentLoadingFinished(EventArgs e)
        {
            if (ContentLoadingFinished != null)
                ContentLoadingFinished(this, e);
        }

        #endregion

        #region Constructors

        public YnState()
            : base()
        {
            _name = "State_" + (ScreenCounter++);
        }

        public YnState(string name)
            : this()
        {
            _name = name;
        }

        #endregion

        /// <summary>
        /// Draw the state on screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Quit the state and remove it from the ScreenManager
        /// </summary>
        public virtual void Kill()
        {
            stateManager.Remove(this);
        }
    }
}