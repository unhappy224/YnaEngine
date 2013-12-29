// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Yna.Engine.Graphics.Component
{
    /// <summary>
    /// Sprite animator class used in Sprite class for animating a sprite with a spritesheet
    /// </summary>
    public class SpriteAnimator : SpriteComponent
    {
        private Dictionary<string, SpriteAnimation> _animations { get; set; }
        private int _spriteWidth;
        private int _spriteHeight;
        private int _textureWidth;
        private int _textureHeight;
        private int _nbSpriteX;
        private int _nbSpriteY;
        private int _spritesheetLenght;
        private string _currentAnimationName;

        public Dictionary<string, SpriteAnimation> Animations
        {
            get { return _animations; }
            protected set { _animations = value; }
        }

        /// <summary>
        /// Gets the current animation name.
        /// </summary>
        public string CurrentAnimationName
        {
            get { return _currentAnimationName; }
        }

        public SpriteAnimator()
            : base()
        {
            _spriteWidth = 0;
            _spriteHeight = 0;
            _textureWidth = 0;
            _textureHeight = 0;
            _nbSpriteX = 0;
            _nbSpriteY = 0;
            _spritesheetLenght = 0;
            _currentAnimationName = String.Empty;
        }

        #region Prepare and add animations

        public void PrepareAnimation(int width, int height)
        {
            Initialize(width, height, Sprite.Texture.Width, Sprite.Texture.Height);

            // The sprite size is now the size of a sprite on the spritesheet
            Sprite.Bounds = new Rectangle((int)Sprite.X, (int)Sprite.Y, width, height);
        }

        /// <summary>
        /// Add an animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="startIndex">The start sprite index (included)</param>
        /// <param name="endIndex">The end sprite index (included)</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, int startIndex, int endIndex, int frameRate, bool reversed)
        {
            // Securize the start and end index
            if (startIndex > endIndex)
            {
                int temp = endIndex;
                endIndex = startIndex;
                startIndex = temp;
            }

            // Build the index array
            int count = endIndex - startIndex;
            int[] indexes = new int[count + 1];
            int currentIntex = startIndex;

            for (int i = 0; i <= count; i++)
            {
                indexes[i] = currentIntex;
                currentIntex++;
            }

            // Call the proper method
            AddAnimation(name, indexes, frameRate, reversed);
        }

        /// <summary>
        /// Add an animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="indexes">Array of index that represent images</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, int[] indexes, int frameRate, bool reversed)
        {
            Add(name, indexes, frameRate, reversed);
            Sprite.SourceRectangle = Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Add an animation
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="rectangles">Array of Rectangles that represents animations on the spritesheet</param>
        /// <param name="frameRate">Framerate for this animation</param>
        /// <param name="reversed">Reverse or not the animation</param>
        public void AddAnimation(string name, Rectangle[] rectangles, int frameRate, bool reversed)
        {
            Add(name, rectangles, frameRate, reversed);
            Sprite.SourceRectangle = Animations[name].Rectangle[0];
        }

        /// <summary>
        /// Get the current animation index
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>Index of the animation</returns>
        public int GetCurrentAnimationIndex(string animationKeyName)
        {
            return Animations[animationKeyName].Index;
        }

        /// <summary>
        /// Get the length of an animation
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>Length of the animation</returns>
        public int GetAnimationLenght(string animationKeyName)
        {
            return Animations[animationKeyName].Count;
        }

        /// <summary>
        /// Get the SpriteAnimation object for a specified animation
        /// </summary>
        /// <param name="animationKeyName">Animation name</param>
        /// <returns>The SpriteAnimation object</returns>
        public SpriteAnimation GetAnimation(string animationKeyName)
        {
            return Animations[animationKeyName];
        }

        /// <summary>
        /// Play an animation
        /// </summary>
        /// <param name="animationName">Animation name</param>
        public void Play(string animationName)
        {
            Sprite.SourceRectangle = PlayAnimation(animationName, Sprite.Effects);
        }


        #endregion

        internal override void Initialize()
        {
            _animations = new Dictionary<string, SpriteAnimation>();
        }

        /// <summary>
        /// Initialization of the animator.
        /// </summary>
        /// <param name="animationWidth"></param>
        /// <param name="animationHeight"></param>
        /// <param name="textureWidth"></param>
        /// <param name="textureHeight"></param>
        private void Initialize(int animationWidth, int animationHeight, int textureWidth, int textureHeight)
        {
            _animations.Clear();

            _spriteWidth = animationWidth;
            _spriteHeight = animationHeight;
            _textureWidth = textureWidth;
            _textureHeight = textureHeight;
            _currentAnimationName = String.Empty;
            _nbSpriteX = _textureWidth / _spriteWidth;
            _nbSpriteY = _textureHeight / _spriteHeight;
            _spritesheetLenght = _nbSpriteX * _nbSpriteY;
        }

        /// <summary>
        /// Add an animation to a sprite.
        /// </summary>
        /// <param name="name">Animation name.</param>
        /// <param name="indexes">Array of index</param>
        /// <param name="frameRate">Desired framerate</param>
        /// <param name="reversed">Sets to true for a reversed animation.</param>
        public void Add(string name, int[] indexes, int frameRate, bool reversed)
        {
            int animationLength = indexes.Length;

            SpriteAnimation animation = new SpriteAnimation(animationLength, frameRate, reversed);

            for (int i = 0; i < animationLength; i++)
            {
                int x = indexes[i] % _nbSpriteX;
                int y = (int)Math.Floor((double)(indexes[i] / _nbSpriteX));

                // Adapt the X value 
                // Ex: For Y = 2, we must have X in range [0... sizeX]
                if (y > 0)
                    x = x % (_nbSpriteX * y);
                else
                    x = x % _nbSpriteX;

                // The source rectangle for the sprite
                animation.Rectangle[i] = new Rectangle(x * _spriteWidth, y * _spriteHeight, _spriteWidth, _spriteHeight);
            }

            _animations.Add(name, animation);
        }

        /// <summary>
        /// Add a new animation.
        /// </summary>
        /// <param name="name">Animation name</param>
        /// <param name="rectangles">Rectangles that compose the animation</param>
        /// <param name="frameRate">The framerate</param>
        /// <param name="reversed">Sets to true to reverse image</param>
        public void Add(string name, Rectangle[] rectangles, int frameRate, bool reversed)
        {
            int animationLength = rectangles.Length;
            SpriteAnimation animation = new SpriteAnimation(animationLength, frameRate, reversed);
            animation.Rectangle = rectangles;
            _animations.Add(name, animation);
        }

        /// <summary>
        /// Play an animation by its name.
        /// </summary>
        /// <param name="animationName">The name of the animation.</param>
        /// <param name="effects">A SpriteEffects if the animation must be reversed.</param>
        /// <returns>The source rectangle of the animation.</returns>
        private Rectangle PlayAnimation(string animationName, ref SpriteEffects effects)
        {
            _currentAnimationName = animationName;
            return _animations[animationName].Next(ref effects);
        }

        private Rectangle PlayAnimation(string animationName, SpriteEffects effects)
        {
            return PlayAnimation(animationName, ref effects);
        }

        private SpriteAnimation GetCurrentAnimation()
        {
            if (_currentAnimationName != String.Empty)
                return _animations[_currentAnimationName];

            return null;
        }

        /// <summary>
        /// Update animator.
        /// </summary>
        /// <param name="gameTime">GameTime object.</param>
        internal override void Update(GameTime gameTime)
        {
            if (_currentAnimationName != String.Empty)
                _animations[_currentAnimationName].Update(gameTime);
        }

        internal override void PostUpdate(GameTime gameTime)
        {
            if (Sprite.LastDistance == Vector2.Zero && CurrentAnimationName != String.Empty)
                Sprite.SourceRectangle = GetCurrentAnimation().Rectangle[0];
        }
    }
}
