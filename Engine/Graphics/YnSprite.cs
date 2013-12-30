﻿// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yna.Engine.Graphics.Controller;
using System.Collections.Generic;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;
using Microsoft.Xna.Framework.Input;
using Yna.Engine.Graphics.Component;

namespace Yna.Engine.Graphics
{
    /// <summary>
    /// Define the origin of an object
    /// </summary>
    public enum SpriteOrigin
    {
        TopLeft = 0, Top, TopRight, Left, Center, Right, BottomLeft, Bottom, BottomRight
    }

    /// <summary>
    /// Define a drawable 2D object
    /// </summary>
    public class YnSprite : YnGameObject, ICollidable2
    {
        #region Protected declarations

        // Texture
        protected Texture2D _texture;
        protected string _assetName;
        protected bool _dirty;

        // Sprite position, direction and bounds
        protected Vector2 _position;
        protected Vector2 _screenPosition;
        protected Rectangle _bounds;
        protected Vector2 _distance;
        protected Vector2 _direction;
        protected Vector2 _lastPosition;
        protected Vector2 _lastDistance;

        // Draw params
        protected Rectangle? _sourceRectangle;
        protected Color _color;
        protected float _rotation;
        protected Vector2 _origin;
        protected Vector2 _scale;
        protected SpriteEffects _effects;
        protected float _layerDepth;
        protected float _alpha;

        // Define the position of the sprite relative to its parent
        protected YnSprite _parent;
        protected bool _hovered;
        protected bool _clicked;

        // Components
        protected List<SpriteComponent> _components;

        #endregion

        #region Fields

        /// <summary>
        /// Flags who determine if this object must be cleaned and removed
        /// </summary>
        public bool Dirty
        {
            get { return _dirty; }
            set
            {
                _dirty = value;
                _enabled = !value;
                _visible = !value;
            }
        }

        /// <summary>
        /// Gets or sets the Texture2D used by the object
        /// </summary>
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// Gets or sets the texture name used when the content is loaded
        /// </summary>
        public string AssetName
        {
            get { return _assetName; }
            set
            {
                if (_assetName != value)
                {
                    _assetLoaded = false;
                    _assetName = value;
                }
            }
        }

        /// <summary>
        /// Gets the absolute screen position
        /// </summary>
        public Vector2 ScreenPosition
        {
            get { return _screenPosition; }
        }

        /// <summary>
        /// Gets or sets the parent object of this object (null if don't have a parent)
        /// </summary>
        public YnSprite Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the X position of the object.
        /// Note: The rectangle is updated when a value is setted
        /// </summary>
        public float X
        {
            get { return (int)_position.X; }
            set
            {
                _position.X = value;
                _bounds.X = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the Y position of the object.
        /// Note: The rectangle value is updated when a value is setted
        /// </summary>
        public int Y
        {
            get { return (int)_position.Y; }
            set
            {
                _position.Y = value;
                _bounds.Y = (int)value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the rectangle
        /// Note: who is the texture2D.Height value
        /// </summary>
        public int Height
        {
            get { return _bounds.Height; }
            set
            {
                if (_texture != null)
                    _scale.Y = (float)((float)value / (float)_texture.Height);

                _bounds.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the rectangle
        /// Note: who is the texture2D.Width value
        /// </summary>
        public int Width
        {
            get { return _bounds.Width; }
            set
            {
                if (_texture != null)
                    _scale.X = (float)((float)value / (float)_texture.Width);

                _bounds.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the position of the object
        /// Note: The rectangle values are updated
        /// </summary>
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position.X = value.X;
                _position.Y = value.Y;
                _bounds.X = (int)value.X;
                _bounds.Y = (int)value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the Rectangle (Bounding box) of the object
        /// Note: The position values are updated
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = value;
                _position.X = value.X;
                _position.Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the Source rectangle
        /// </summary>
        public Rectangle? SourceRectangle
        {
            get { return _sourceRectangle; }
            set { _sourceRectangle = value; }
        }

        /// <summary>
        /// Gets or sets the rotation of the object in radians
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// Gets or sets the scale of the object.Default is Vector2.One
        /// Note: The rectangle of the object is updated
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float ScaledWidth
        {
            get { return _bounds.Width * _scale.X; }
        }

        public float ScaledHeight
        {
            get { return _bounds.Height * _scale.Y; }
        }

        /// <summary>
        /// Gets or sets the origin of the object. Default is Vector2.Zero
        /// </summary>
        public Vector2 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Gets or sets the color applied to the object
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the Alpha applied to the object. Value between 0.0f and 1.0f
        /// </summary>
        public float Alpha
        {
            get { return _alpha; }
            set
            {
                _alpha = value > 1.0f ? 1.0f : value;
                _alpha = _alpha < 0.0f ? 0.0f : _alpha;
            }
        }

        /// <summary>
        /// Gets or sets the SpriteEffect used for drawing the sprite
        /// </summary>
        public SpriteEffects Effects
        {
            get { return _effects; }
            set { _effects = value; }
        }

        /// <summary>
        /// Gets or sets the layer depth
        /// </summary>
        public float LayerDepth
        {
            get { return _layerDepth; }
            set { _layerDepth = value; }
        }

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
        /// Flag indicating that the entity is currently hovered (touch or mouse)
        /// </summary>
        public bool Hovered
        {
            get { return _hovered; }
            internal set { _hovered = value; }
        }

        /// <summary>
        /// Flag indicating that the entity is currently beeing clicked (touch or mouse)
        /// </summary>
        public bool Clicked
        {
            get { return _clicked; }
            internal set { _clicked = value; }
        }

        #endregion

        #region Events for life

        /// <summary>
        /// Triggered when the object was killed
        /// </summary>
        public event EventHandler<EventArgs> Killed = null;

        /// <summary>
        /// Triggered when the object was revived
        /// </summary>
        public event EventHandler<EventArgs> Revived = null;

        private void KillSprite(EventArgs e)
        {
            if (Killed != null)
                Killed(this, e);
        }

        private void ReviveSprite(EventArgs e)
        {
            if (Revived != null)
                Revived(this, e);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a sprite with default values
        /// </summary>
        public YnSprite()
            : base()
        {
            _dirty = false;
            _position = Vector2.Zero;
            _bounds = Rectangle.Empty;
            _sourceRectangle = null;
            _texture = null;
            _assetName = String.Empty;
            _assetLoaded = false;
            _color = Color.White;
            _rotation = 0.0f;
            _origin = Vector2.Zero;
            _scale = Vector2.One;
            _alpha = 1.0f;
            _effects = SpriteEffects.None;
            _layerDepth = 1.0f;
            _parent = null;
            _distance = Vector2.One;
            _direction = Vector2.Zero;
            _lastPosition = Vector2.Zero;
            _lastDistance = Vector2.Zero;
            _components = new List<SpriteComponent>();
        }

        /// <summary>
        /// Create a sprite
        /// </summary>
        /// <param name="assetName">Image name that will loaded from the content manager</param>
        public YnSprite(string assetName)
            : this()
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
            Bounds = rectangle;
            _texture = YnGraphics.CreateTexture(color, rectangle.Width, rectangle.Height);
            _assetLoaded = true;
            _position = new Vector2(rectangle.X, rectangle.Y);
            _bounds = rectangle;
        }

        public YnSprite(Texture2D texture)
            : this()
        {
            _texture = texture;

            if (_texture != null)
            {
                _bounds.Width = texture.Width;
                _bounds.Height = texture.Height;
                _assetLoaded = true;
            }
        }

        #endregion

        #region Components management

        /// <summary>
        /// Add a new component. Note that you can just add one component of each type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Return the added component.</returns>
        public T AddComponent<T>() where T : SpriteComponent, new()
        {
            T component = GetComponent<T>();

            if (component == null)
            {
                component = new T();
                component.Sprite = this;
                component.Initialize();
                _components.Add(component);
            }

            return component;
        }

        /// <summary>
        /// Gets a component by its type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Return the component if exits, otherwise return null.</returns>
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

        /// <summary>
        /// Remove a component
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Return the removed component if exits, otherwise return null.</returns>
        public T RemoveComponent<T>() where T : SpriteComponent
        {
            SpriteComponent result = null;

            int size = _components.Count;
            int i = 0;

            while (i < size && result == null)
            {
                if (_components[i] is T)
                {
                    result = _components[i] as T;
                    _components.Remove(result);
                }
                i++;
            }

            return (T)result;
        }

        #endregion

        #region Transform methods

        /// <summary>
        /// Ease the positionning. Sets the position of the entity.
        /// </summary>
        /// <param name="x">Position on X axis.</param>
        /// <param name="y">Position on Y axis.</param>
        public virtual void Move(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
            _bounds.X = (int)x;
            _bounds.Y = (int)y;
        }

        public virtual void Move(ref Vector2 position)
        {
            Move(position.X, position.Y);
        }

        public virtual void Move(Vector2 position)
        {
            Move(ref position);
        }

        public virtual void Translate(Vector2 position)
        {
            Translate(ref position);
        }

        public virtual void Translate(ref Vector2 position)
        {
            Translate(position.X, position.Y);
        }

        public virtual void Translate(float x, float y)
        {
            _position.X += x;
            _position.Y += y;
            _bounds.X += (int)x;
            _bounds.Y += (int)y;
        }

        /// <summary>
        /// Sets the size of the bouding rectangle.
        /// </summary>
        /// <param name="width">Desired width.</param>
        /// <param name="height">Desired height</param>
        public virtual void SetSize(float width, float height, bool updateScale)
        {
            _bounds.Width = (int)width;
            _bounds.Height = (int)height;

            if (updateScale && _texture != null)
            {
                _scale.X = (float)((float)width / (float)_texture.Width);
                _scale.Y = (float)((float)height / (float)_texture.Height);
            }
        }

        public virtual void SetSize(float width, float height)
        {
            SetSize(width, height, true);
        }

        /// <summary>
        /// Update the screen position relative to its parent
        /// </summary>
        public void UpdateScreenPosition()
        {
            _screenPosition = _position;

            // Relative position to it's parent
            if (_parent != null)
                _screenPosition += _parent.ScreenPosition;
        }

        #endregion

        #region Methods for changing origin and size

        /// <summary>
        /// Change the origin of the object. Note that when initializing the object origin
        /// with this method, the origin point will be computed once. If you change the object's
        /// bounds afterwards, old origin will be kept as is and may not reflect the initially wanted 
        /// origin point.
        /// </summary>
        /// <param name="spriteOrigin">Determinated point of origin</param>
        public void SetOrigin(SpriteOrigin spriteOrigin)
        {
            switch (spriteOrigin)
            {
                case SpriteOrigin.TopLeft: _origin = Vector2.Zero; break;
                case SpriteOrigin.Top: _origin = new Vector2(ScaledWidth / 2, 0); break;
                case SpriteOrigin.TopRight: _origin = new Vector2(ScaledWidth, 0); break;
                case SpriteOrigin.Left: _origin = new Vector2(0, ScaledHeight / 2); break;
                case SpriteOrigin.Center: _origin = new Vector2(ScaledWidth / 2, ScaledHeight / 2); break;
                case SpriteOrigin.Right: _origin = new Vector2(ScaledWidth, ScaledHeight / 2); break;
                case SpriteOrigin.BottomLeft: _origin = new Vector2(0, ScaledHeight); break;
                case SpriteOrigin.Bottom: _origin = new Vector2(ScaledWidth / 2, ScaledHeight); break;
                case SpriteOrigin.BottomRight: _origin = new Vector2(ScaledWidth, ScaledHeight); break;
            }
        }

        public void SetOrigin(Vector2 origin)
        {
            _origin = origin;
        }

        public void SetOrigin(ref Vector2 origin)
        {
            _origin = origin;
        }

        /// <summary>
        /// Adapt the scale of the entity for taking fullscreen
        /// </summary>
        public void SetFullScreen()
        {
            _position = Vector2.Zero;
            _bounds.X = 0;
            _bounds.Y = 0;
            SetSize(YnG.Width, YnG.Height);
        }

        #endregion

        #region Life cycle

        /// <summary>
        /// Flag the object for a purge action
        /// </summary>
        public virtual void Die()
        {
            Dirty = true;
        }

        /// <summary>
        /// Kill the object, it's no more updated and drawed
        /// Pause is setted on true and Visible is setted to false
        /// </summary>
        public virtual void Kill()
        {
            Active = false;
            KillSprite(EventArgs.Empty);
        }

        /// <summary>
        /// Revive the object, it's updated and drawed
        /// Pause is setted on false and Visible is setted to true
        /// </summary>
        public virtual void Revive()
        {
            Active = true;
            ReviveSprite(EventArgs.Empty);
        }

        #endregion

        #region GameState pattern

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
            if (!_assetLoaded && _assetName != String.Empty)
            {
                _texture = YnG.Content.Load<Texture2D>(_assetName);
                _bounds.Width = _texture.Width;
                _bounds.Height = _texture.Height;

                if (GetComponent<SpriteAnimator>() != null)
                {
                    _sourceRectangle = new Rectangle(0, 0, _texture.Width, _texture.Height);
                    _bounds = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
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

        /// <summary>
        /// Dispose the Texture2D object if the Dirty property is set to true
        /// </summary>
        public override void UnloadContent()
        {
            if (_texture != null && _dirty)
                _texture.Dispose();
        }

        public override void Update(GameTime gameTime)
        {
            // Reset flags
            _clicked = false;
            _hovered = false;

            // Update last position/direction
            _lastPosition.X = _position.X;
            _lastPosition.Y = _position.Y;
            _lastDistance.X = _distance.X;
            _lastDistance.Y = _distance.Y;

            UpdateScreenPosition();

            for (int i = 0, l = _components.Count; i < l; i++)
            {
                if (_components[i].Enabled)
                    _components[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Post updates for sprite collide and direction
        /// Called after Update() and before Draw() 
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void PostUpdate(GameTime gameTime)
        {
            _bounds.X = (int)_position.X;
            _bounds.Y = (int)_position.Y;

            // Update the direction
            _distance.X = _position.X - _lastPosition.X;
            _distance.Y = _position.Y - _lastPosition.Y;
            _direction.X = _distance.X;
            _direction.Y = _distance.Y;

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
