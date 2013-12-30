// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine
{
    /// <summary>
    /// A screen helper who provide some methods to get scaled coordinates and scale
    /// </summary>
    public class YnScreen
    {
        private static Rectangle _screenRectangle;

        /// <summary>
        /// Show or hide the mouse cursor.
        /// </summary>
        public static bool ShowMouse
        {
            get { return YnG.Game.IsMouseVisible; }
            set { YnG.Game.IsMouseVisible = value; }
        }

        /// <summary>
        /// Enable or disable fullscreen mode.
        /// </summary>
        public static bool IsFullScreen
        {
            get { return YnG.GraphicsDeviceManager.IsFullScreen; }
            set { YnG.GraphicsDeviceManager.IsFullScreen = value; }
        }

        /// <summary>
        /// The screen width
        /// </summary>
        public static int Width { get; internal set; }

        /// <summary>
        /// The screen height
        /// </summary>
        public static int Height { get; internal set; }

        /// <summary>
        /// Gets the rectangle that represent the screen size
        /// </summary>
        public static Rectangle ScreenRect
        {
            get { return _screenRectangle; }
        }

        /// <summary>
        /// Gets the center of the screen on X axis
        /// </summary>
        public static int ScreenCenterX
        {
            get { return Width / 2; } 
        }

        /// <summary>
        /// Gets the center of the screen on Y axis
        /// </summary>
        public static int ScreenCenterY
        {
            get { return Height / 2; }
        }

        /// <summary>
		/// Base reference for scaling width
        /// </summary>
        public static int ReferenceWidth { get; internal set; }

        /// <summary>
		/// Base reference for scaling height
        /// </summary>
        public static int ReferenceHeight { get; internal set; }

		/// <summary>
		/// Setup the specified width, height, referenceWidth and referenceHeight.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="referenceWidth">Reference width.</param>
		/// <param name="referenceHeight">Reference height.</param>
		public static void Setup(int width, int height, int referenceWidth, int referenceHeight, bool applyResolution)
        {
            ReferenceWidth = referenceWidth;
            ReferenceHeight = referenceHeight;
            
			Width = width;
            Height = height;

            _screenRectangle = new Rectangle(0, 0, Width, Height);

			if (applyResolution) 
			{
                YnG.GraphicsDeviceManager.PreferredBackBufferWidth = width;
                YnG.GraphicsDeviceManager.PreferredBackBufferHeight = height;
                YnG.GraphicsDeviceManager.ApplyChanges();
			}
        }

		/// <summary>
		/// Setup the specified width, height and applyResolution.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="applyResolution">If set to <c>true</c> apply resolution.</param>
		public static void Setup(int width, int height, bool applyResolution)
		{
			Setup (width, height, ReferenceWidth, ReferenceHeight, applyResolution);
		}

		/// <summary>
		/// Setup the specified width and height.
		/// </summary>
		/// <param name="width">Screen width.</param>
		/// <param name="height">Screen height.</param>
		public static void Setup(int width, int height)
		{
			Setup (width, height, ReferenceWidth, ReferenceWidth, true);
		}

        /// <summary>
        /// Get the scaled X coordinate relative to the reference width
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float GetScaleX(float value)
        {
            return (((float)Width * value) / (float)ReferenceWidth);
        }

        /// <summary>
        /// Get the scaled Y coordinate relative to the reference height
        /// </summary>
        /// <param name="value">The default Y coordinate used with the reference height</param>
        /// <returns>A scaled Y coordinate</returns>
        public static float GetScaleY(float value)
        {
            return (((float)Height * value) / (float)ReferenceHeight);
        }

        /// <summary>
        /// Get the scale relative to the reference width and height
        /// </summary>
        /// <returns>The scale difference between the current resolution and the reference resolution of the screen</returns>
        public static Vector2 GetScale()
        {
            return new Vector2(
                (float)((float)Width / (float)ReferenceWidth),
                (float)((float)Height / (float)ReferenceHeight));
        }

		/// <summary>
		/// Determines the max resolution.
		/// </summary>
		/// <returns>A collection of supported display mode.</returns>
		/// <param name="fullscreen">If set to <c>true</c> fullscreen.</param>
		public static DisplayModeCollection DetermineBestResolution(bool fullscreen)
		{
			DisplayModeCollection modes = YnG.GraphicsDevice.Adapter.SupportedDisplayModes;

			int width = 640;
			int height = 480;

			foreach (DisplayMode mode in modes)
			{
				width = (mode.Width > width) ? mode.Width : width;
				height = (mode.Height > height) ? mode.Height : height;
			}

			Setup (width, height, true);

			if (!IsFullScreen && fullscreen)
				YnG.GraphicsDeviceManager.ToggleFullScreen();

			return modes;
		}

		/// <summary>
		/// Toggles in fullscreen/windowed mode.
		/// </summary>
		public static void ToggleFullscreen()
		{
			YnG.GraphicsDeviceManager.ToggleFullScreen();
		}
    }
}
