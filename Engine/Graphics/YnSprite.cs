// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Animation;
using Yna.Engine.Graphics.Component;
using System.Collections.Generic;

namespace Yna.Engine.Graphics
{
    public class YnSprite : YnEntity
    {
        #region Private declarations

        // Moving the sprite
        protected Vector2 _distance;
        protected Vector2 _direction;
        protected Vector2 _lastPosition;
        protected Vector2 _lastDistance;
        
        // Position
        protected Rectangle? _sourceRectangle;
		
        // Animations
        protected List<SpriteComponent> _components;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the direction.
        /// </summary>
        public Vector2 Direction
        {
            get { return _direction; }
        }

        /// <summary>
        /// Gets the previous position.
        /// </summary>
        public Vector2 LastPosition
        {
            get { return _lastPosition; }
        }

        /// <summary>
        /// Gets or sets the distance of the sprite
        /// </summary>
        public Vector2 Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }

        /// <summary>
        /// Gets the previous direction.
        /// </summary>
        public Vector2 LastDistance
        {
            get { return _lastDistance; }
        }

        /// <summary>
        /// Gets or sets the Source rectangle
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle;  }
            set { _sourceRectangle = value; }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Create a sprite with default values
        /// </summary>
        public YnSprite() 
            : base ()
        {
            _sourceRectangle = null;
            
            _distance = Vector2.One;
            _direction = Vector2.Zero;
            _lastPosition = Vector2.Zero;
            _lastDistance = Vector2.Zero;

            _components = new List<SpriteComponent>();
        }

        private YnSprite(Vector2 position)
            : this()
        {
            _position = position;
            _lastPosition = _position;
        }

        /// <summary>
        /// Create a sprite
        /// </summary>
        /// <param name="assetName">Image name that will loaded from the content manager</param>
        public YnSprite(string assetName)
            : this(Vector2.Zero)
        {
            _assetName = assetName;
        }

        /// <summary>
        /// Create a sprite
        /// </summary>
        /// <param name="position">Position of the sprite</param>
        /// <param name="assetName">Image name that will loaded from the content manager</param>
        public YnSprite(Vector2 position, string assetName) 
            : this(position)
        {
            _assetName = assetName;
        }

        /// <summary>
        /// Create a sprite without asset
        /// </summary>
        /// <param name="rectangle">Size of the sprite</param>
        /// <param name="color">Color of the sprite</param>
        public YnSprite(Rectangle rectangle, Color color)
            : this()
        {
            Rectangle = rectangle;
            _texture = YnGraphics.CreateTexture(color, rectangle.Width, rectangle.Height);
            _assetLoaded = true;
            _position = new Vector2(rectangle.X, rectangle.Y);
            _rectangle = rectangle;
        }

        #endregion

        public T AddComponent<T>() where T : SpriteComponent, new()
        {
            T component = new T();
            component.Sprite = this;
            component.Initialize();
            _components.Add(component);
            return component;
        }

        public T GetComponent<T>() where T : SpriteComponent
        {
            SpriteComponent result = null;

            int size = _components.Count;
            int i = 0;

            while (i < size && result == null)
            {
                result = _components[i] is T ? _components[i] : result;
                i++;
            }

            return (T)result;
        }

        #region GameState patterns

        public override void Initialize()
        {
            base.Initialize();
            _initialized = true;
        }

        /// <summary>
        /// Load the texture of the sprite.
        /// </summary>
        public override void LoadContent()
        {
            if (!_assetLoaded)
            {
                if (_texture == null && _assetName != String.Empty)
                {
                    _texture = YnG.Content.Load<Texture2D>(_assetName);

                    if (GetComponent<SpriteAnimator>() != null)
                    {
                        _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                        _rectangle = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
                    }
                }
                
                _assetLoaded = true;
            }
        }

        /// <summary>
        /// Load content with a specific texture
        /// if a texture is already loaded, it's replaced by the new
        /// </summary>
        /// <param name="textureName">Texture name</param>
        public virtual void LoadContent(string textureName)
        {
            _assetName = textureName;
            _assetLoaded = false;
            _texture = null;
            LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Enabled)
            {
                _lastPosition.X = _position.X;
                _lastPosition.Y = _position.Y;
                _lastDistance.X = _distance.X;
                _lastDistance.Y = _distance.Y;

                for (int i = 0, l = _components.Count; i < l; i++)
                {
                    if (_components[i].Enabled)
                        _components[i].Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Post updates for sprite collide and direction
        /// Called after Update() and before Draw() 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime)
        {
            // Update the direction
            _distance.X = _position.X - _lastPosition.X;
            _distance.Y = _position.Y - _lastPosition.Y;
            _direction.X = _distance.X;
            _direction.Y = _distance.Y;

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
            
            if (_direction.X != 0 && _direction.Y != 0)
                _direction.Normalize();

            for (int i = 0, l = _components.Count; i < l; i++)
            {
                if (_components[i].Enabled)
                    _components[i].PostUpdate(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Enabled)
                PostUpdate(gameTime);
           
            if (Visible)
                spriteBatch.Draw(_texture, _position, _sourceRectangle, _color * _alpha, _rotation, _origin, _scale, _effects, _layerDepth);
        }

        #endregion
    }
}
