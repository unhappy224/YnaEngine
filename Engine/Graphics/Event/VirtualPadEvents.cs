// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Yna.Engine.Graphics.Controller;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Event for virtul pad controller who're used to notify the direction.
    /// </summary>
    public class VirtualPadEventArgs : EventArgs
    {
        public PadButtons Direction { get; protected set; }

        /// <summary>
        /// Create an empty event.
        /// </summary>
        public VirtualPadEventArgs()
        {
            Direction = PadButtons.None;
        }

        /// <summary>
        /// Create an event with a direction.
        /// </summary>
        /// <param name="direction"></param>
        public VirtualPadEventArgs(PadButtons direction)
        {
            Direction = direction;
        }
    }
}
