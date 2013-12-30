using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yna.Engine.Graphics.Component
{
    public class SpritePhysics : SpriteComponent
    {
        protected Vector2 _acceleration;
        protected Vector2 _velocity;
        protected float _maxVelocity;

        // Collide with screen
        protected bool _forceInsideScreen;
        protected bool _forceAllowAcrossScreen;

        protected Rectangle _gameViewport;

        /// <summary>
        /// Gets or sets Acceleration
        /// </summary>
        public Vector2 Acceleration
        {
            get { return _acceleration; }
            set { _acceleration = value; }
        }

        /// <summary>
        /// Gets or sets Velocity
        /// </summary>
        public Vector2 Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        /// <summary>
        /// Gets or sets the X value of the Velocity
        /// </summary>
        public float VelocityX
        {
            get { return _velocity.X; }
            set { _velocity.X = value; }
        }

        /// <summary>
        /// Gets or sets set Y value of the Velocity
        /// </summary>
        public float VelocityY
        {
            get { return _velocity.Y; }
            set { _velocity.Y = value; }
        }

        /// <summary>
        /// Gets or sets the VelocityMax
        /// </summary>
        public float VelocityMax
        {
            get { return _maxVelocity; }
            set { _maxVelocity = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle Viewport used for this sprite. Default is the size of the screen
        /// </summary>
        public Rectangle Viewport
        {
            get { return _gameViewport; }
            set { _gameViewport = value; }
        }

        /// <summary>
        /// Force or not the sprite to stay in screen
        /// </summary>
        public bool ForceInsideScreen
        {
            get { return _forceInsideScreen; }
            set
            {
                _forceInsideScreen = value;

                if (_forceInsideScreen)
                    _forceAllowAcrossScreen = false;
            }
        }

        /// <summary>
        /// Authorizes or not the object across the screen and appear on the opposite
        /// </summary>
        public bool AllowAcrossScreen
        {
            get { return _forceAllowAcrossScreen; }
            set
            {
                _forceAllowAcrossScreen = value;

                if (_forceAllowAcrossScreen)
                    _forceInsideScreen = false;
            }
        }

        public SpritePhysics()
            : base()
        {
            _forceInsideScreen = false;
            _forceAllowAcrossScreen = false;
            _acceleration = Vector2.One;
            _velocity = Vector2.Zero;
            _maxVelocity = 1.0f;
        }

        public override void Initialize()
        {
            _gameViewport = new Rectangle(0, 0, YnG.Width, YnG.Height);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            Sprite.Translate(_velocity * _acceleration);
            _velocity *= _maxVelocity;
        }

        public override void PostUpdate(GameTime gameTime)
        {
            if (_forceInsideScreen)
            {
                if (Sprite.X - Sprite.Origin.X < _gameViewport.X)
                {
                    Sprite.Move(_gameViewport.X + Sprite.Origin.X, Sprite.Y);
                    _velocity *= 0.0f;
                }
                else if (Sprite.X + (Sprite.Width - Sprite.Origin.X) > _gameViewport.Width)
                {
                    Sprite.Move(_gameViewport.Width - (Sprite.Width - Sprite.Origin.X), Sprite.Y);
                    _velocity *= 0.0f;
                }

                if (Sprite.Y - Sprite.Origin.Y < _gameViewport.Y)
                {
                    Sprite.Move(Sprite.X, _gameViewport.Y + Sprite.Origin.Y);
                    _velocity *= 0.0f;
                }
                else if (Sprite.Y + (Sprite.Height - Sprite.Origin.Y) > _gameViewport.Height)
                {
                    Sprite.Move(Sprite.X, _gameViewport.Height - (Sprite.Height - Sprite.Origin.Y));
                    _velocity *= 0.0f;
                }
            }
            else if (_forceAllowAcrossScreen)
            {
                if (Sprite.X + (Sprite.Width - Sprite.Origin.X) < _gameViewport.X)
                    Sprite.Move(_gameViewport.Width - Sprite.Origin.X, Sprite.Y);
                else if (Sprite.X > _gameViewport.Width)
                    Sprite.Move(_gameViewport.X, Sprite.Y);

                if (Sprite.Y + Sprite.Height < _gameViewport.Y)
                    Sprite.Move(Sprite.X, _gameViewport.Height);
                else if (Sprite.Y > _gameViewport.Height)
                    Sprite.Move(Sprite.X, _gameViewport.Y);
            }
        }
    }
}
