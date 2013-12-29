using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics.Component
{
    public class TouchEventDispatcher : SpriteComponent
    {
        private int _fingerTarget;
        private bool _testAllFingers;
        private bool _hovered;
        private Vector2 _cachePosition;
        private Rectangle _cacheScreenBounds;

        public int FingerTarget 
        {
            get { return _fingerTarget; }
            set { _fingerTarget = value; } 
        }

        public bool TestAllFingers
        {
            get { return _testAllFingers; }
            set { _testAllFingers = value; }
        }

        #region Events

        /// <summary>
        /// Triggered when touch are detected over the object
        /// </summary>
        public event EventHandler<TouchActionEventArgs> TouchHover = null;

        /// <summary>
        /// Triggered when touch are detected over the object
        /// </summary>
        public event EventHandler<TouchActionEventArgs> TouchReleased = null;

        private void OnTouchHover(TouchActionEventArgs e)
        {
            _hovered = true;

            if (TouchHover != null)
                TouchHover(Sprite, e);
        }

        private void OnTouchReleased(TouchActionEventArgs e)
        {
            _hovered = false;

            if (TouchReleased != null)
                TouchReleased(Sprite, e);
        }
        
        #endregion

        public TouchEventDispatcher()
            : base()
        {
            _fingerTarget = 0;
            _testAllFingers = false;
            _hovered = false;
            _cachePosition = Vector2.Zero;
            _cacheScreenBounds = Rectangle.Empty;
        }

        internal override void Update(GameTime gameTime)
        {
            _cacheScreenBounds.X = (int)(Sprite.ScreenPosition.X - Sprite.Origin.X);
            _cacheScreenBounds.Y = (int)(Sprite.ScreenPosition.Y - Sprite.Origin.Y);
            _cacheScreenBounds.Width = (int)(Sprite.ScaledWidth);
            _cacheScreenBounds.Height = (int)(Sprite.ScaledHeight);

            if (_testAllFingers)
            {
                for (int i = 0; i < YnG.Touch.MaxFingerPoints; i++)
                    UpdateTouchInputOn(i);
            }
            else
                UpdateTouchInputOn(_fingerTarget);
        }

        private void UpdateTouchInputOn(int fingerId)
        {
            _cachePosition = YnG.Touch.GetPosition(fingerId);

            if (_cacheScreenBounds.Contains(_cachePosition))
            {
                Sprite.Clicked |= YnG.Touch.Pressed(fingerId);

                OnTouchHover(new TouchActionEventArgs(
                    _cachePosition.X,
                    _cachePosition.Y,
                    fingerId,
                    Sprite.Clicked,
                    YnG.Touch.Moved(fingerId),
                    false,
                    YnG.Touch.GetPressureLevel(fingerId)));
            }
            else if (_hovered)
            {
                OnTouchReleased(new TouchActionEventArgs(
                    _cachePosition.X,
                    _cachePosition.Y,
                    fingerId,
                    false,
                    false,
                    true,
                    YnG.Touch.GetPressureLevel(fingerId)));
            }
        }
    }
}
