using System;
using Microsoft.Xna.Framework;
using Yna.Engine.Input.Kinect;

namespace Yna.Engine.Input.Service
{
    public interface IKinectSensorService
    {
        Vector3 Head(KinectPlayerIndex index);

        Vector3 HandLeft(KinectPlayerIndex index);

        Vector3 HandRight(KinectPlayerIndex index);

        Vector3 FootLeft(KinectPlayerIndex index);

        Vector3 FootRight(KinectPlayerIndex index);

        bool IsAvailable();
    }
}
