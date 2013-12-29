using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Yna.Engine.Graphics.Event;
using Yna.Engine.Input;

namespace Yna.Engine.Graphics.Component
{
    public class MouseEventDispatcher : SpriteComponent
    {
        private Rectangle _cacheScreenBounds;
        private bool _hovered;

        #region Events

        /// <summary>
        /// Triggered when the mouse is over the object
        /// </summary>
        public event EventHandler<MouseOverEntityEventArgs> MouseOver = null;

        /// <summary>
        /// Triggered when the mouse leave the object
        /// </summary>
        public event EventHandler<MouseLeaveSpriteEventArgs> MouseLeave = null;

        /// <summary>
        /// Triggered when a click (and just one) is detected over the object
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseClicked = null;
        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseClickSpriteEventArgs> MouseClick = null;
        /// <summary>
        /// Triggered when click are detected over the object
        /// </summary>
        public event EventHandler<MouseReleaseSpriteEventArgs> MouseReleased = null;

        private void MouseOverSprite(MouseOverEntityEventArgs e)
        {
            _hovered = true;

            if (MouseOver != null)
                MouseOver(Sprite, e);
        }

        private void MouseLeaveSprite(MouseLeaveSpriteEventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(Sprite, e);
        }

        private void MouseJustClickedSprite(MouseClickSpriteEventArgs e)
        {
            if (MouseClicked != null)
                MouseClicked(Sprite, e);
        }

        private void MouseClickSprite(MouseClickSpriteEventArgs e)
        {
            if (MouseClick != null)
                MouseClick(Sprite, e);
        }

        private void MouseReleasedSprite(MouseReleaseSpriteEventArgs e)
        {
            _hovered = false;

            if (MouseReleased != null)
                MouseReleased(Sprite, e);
        }

        #endregion

        public MouseEventDispatcher()
            : base()
        {
            _hovered = false;
            _cacheScreenBounds = Rectangle.Empty;
        }

        internal override void Update(GameTime gameTime)
        {
            _cacheScreenBounds.X = (int)(Sprite.ScreenPosition.X - Sprite.Origin.X);
            _cacheScreenBounds.Y = (int)(Sprite.ScreenPosition.Y - Sprite.Origin.Y);
            _cacheScreenBounds.Width = (int)(Sprite.ScaledWidth);
            _cacheScreenBounds.Height = (int)(Sprite.ScaledHeight);

            if (_cacheScreenBounds.Contains(YnG.Mouse.X, YnG.Mouse.Y))
            {
                Sprite.Hovered = true;
                
                // Mouse Over
                MouseOverSprite(new MouseOverEntityEventArgs(YnG.Mouse.X, YnG.Mouse.Y));

                // Just clicked
                if (YnG.Mouse.JustClicked(MouseButton.Left) || YnG.Mouse.JustClicked(MouseButton.Middle) || YnG.Mouse.JustClicked(MouseButton.Right))
                {
                    Sprite.Clicked = true;

                    MouseButton mouseButton = MouseButton.Right;

                    if (YnG.Mouse.JustClicked(MouseButton.Left))
                        mouseButton = MouseButton.Left;
                    else if (YnG.Mouse.JustClicked(MouseButton.Middle))
                        mouseButton = MouseButton.Middle;

                    MouseJustClickedSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, true, false));
                }

                // One click
                else if (YnG.Mouse.ButtonDown(MouseButton.Left, ButtonState.Pressed) || YnG.Mouse.ButtonDown(MouseButton.Middle, ButtonState.Pressed) || YnG.Mouse.ButtonDown(MouseButton.Right, ButtonState.Pressed))
                {
                    MouseButton mouseButton = MouseButton.Right;

                    if (YnG.Mouse.ButtonDown(MouseButton.Left, ButtonState.Pressed))
                        mouseButton = MouseButton.Left;
                    else if (YnG.Mouse.ButtonDown(MouseButton.Middle, ButtonState.Pressed))
                        mouseButton = MouseButton.Middle;

                    MouseClickSprite(new MouseClickSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y, mouseButton, false, false));
                }
                else
                    MouseReleasedSprite(new MouseReleaseSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
            }
            // Mouse leave
            else if (_cacheScreenBounds.Contains(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y))
                MouseLeaveSprite(new MouseLeaveSpriteEventArgs(YnG.Mouse.LastMouseState.X, YnG.Mouse.LastMouseState.Y, YnG.Mouse.X, YnG.Mouse.Y));
            else if (_hovered)
                MouseReleasedSprite(new MouseReleaseSpriteEventArgs(YnG.Mouse.X, YnG.Mouse.Y));
        }
    }
}
