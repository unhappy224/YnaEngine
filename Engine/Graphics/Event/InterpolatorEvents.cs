// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;

namespace Yna.Engine.Graphics.Event
{
    /// <summary>
    /// Event class used when an interpolator object done its job.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InterpolatorEventArgs<T> : EventArgs
    {
        public T InterpolatedValue;
        public float ElapsedTime;

        public InterpolatorEventArgs()
        {
            ElapsedTime = 0.0f;
            InterpolatedValue = default(T);
        }

        public InterpolatorEventArgs(float elapsedTime, T value)
        {
            ElapsedTime = elapsedTime;
            InterpolatedValue = value;
        }
    }
}
