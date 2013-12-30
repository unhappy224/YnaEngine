// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Yna.Engine.Graphics.Animation;
using Yna.Engine.Graphics.Component;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// A simple camera used on the scene to make different type of effects.
    /// Position, Rotation and Zoom can be applied on the scene.
    /// </summary>
    public class ViewportCamera
    {
        // Avoid garbage generation because we use it on each update.
        private Matrix _originMatrix;
        private Matrix _rotationMatrix;
        private Matrix _zoomMatrix;
        private Matrix _translationMatrix;
        private Matrix _cacheResult;
        private Vector2 _screenCenter;

        /// <summary>
        /// X position
        /// </summary>
        public int X;

        /// <summary>
        /// Y position
        /// </summary>
        public int Y;

        /// <summary>
        /// Rotation in degrees.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Zoom in/out the scene.
        /// </summary>
        public float Zoom;

        /// <summary>
        /// Create a camera for the scene
        /// </summary>
        public ViewportCamera()
        {
            X = 0;
            Y = 0;
            Rotation = 0.0f;
            Zoom = 1.0f;
            _screenCenter = new Vector2(YnG.Width / 2, YnG.Height / 2);
        }

        /// <summary>
        /// Get the transformed matrix
        /// </summary>
        /// <returns></returns>
        public Matrix GetTransformMatrix()
        {
            _originMatrix = Matrix.CreateTranslation(X + (-YnG.Width / 2), Y + (-YnG.Height / 2), 0);
            _rotationMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation));
            _zoomMatrix = Matrix.CreateScale(Zoom);
            _translationMatrix = Matrix.CreateTranslation(X + (YnG.Width / 2), Y + (YnG.Height / 2), 0);
            _cacheResult = _zoomMatrix * _originMatrix * _rotationMatrix * _translationMatrix;

            return _cacheResult;
        }
    }
}
