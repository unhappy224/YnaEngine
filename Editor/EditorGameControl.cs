﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yna.Engine;
using Yna.Engine.Graphics2D;
using Yna.Engine.Graphics2D.Scene;
using Yna.Engine.Winforms;
using System.Diagnostics;
using Yna.Engine.Graphics3D;
using Yna.Engine.Graphics3D.Camera;

namespace Yna.Editor
{
    public class EditorGameControl : YnGameControl
    {
        public Color ClearColor;

        private GameTime gameTime;
        private Stopwatch _stopWatch;
        private TimeSpan _lastUpdate;

        private bool _is3DScene;
        private BaseCamera camera;
        private List<YnEntity> _gameObjects;
        private List<YnEntity3D> _gameObjects3D;

        protected override void Initialize()
        {
            base.Initialize();
            ClearColor = Color.Black;

            camera = new FixedCamera();
            _gameObjects = new List<YnEntity>();
            _gameObjects3D = new List<YnEntity3D>();

            _lastUpdate = new TimeSpan(DateTime.Now.Ticks);
            _stopWatch = new Stopwatch(); 
            _stopWatch.Start();

            _is3DScene = false;

            gameTime = new GameTime();
        }

        protected override void Update()
        {
            UpdateTime();

            foreach (YnEntity go in _gameObjects)
                go.Update(gameTime);

            foreach (YnEntity3D go3 in _gameObjects3D)
                go3.Update(gameTime);
        }

        protected void UpdateTime()
        {
            TimeSpan total = _stopWatch.Elapsed;
            TimeSpan elapsed = total - _lastUpdate;
            gameTime.ElapsedGameTime = elapsed;
            gameTime.TotalGameTime = total;
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(ClearColor);

            if (_is3DScene)
            {
                foreach (YnEntity go in _gameObjects)
                    go.Draw(gameTime, spriteBatch);

                YnG3.RestoreGraphicsDeviceStates();

                foreach (YnEntity3D go3 in _gameObjects3D)
                    go3.Draw(gameTime, GraphicsDevice, camera);
            }

            if (_gameObjects.Count > 0)
            {
                spriteBatch.Begin();

                foreach (YnEntity go in _gameObjects)
                    go.Draw(gameTime, spriteBatch);
                
                spriteBatch.End();
            }
        }
    }
}