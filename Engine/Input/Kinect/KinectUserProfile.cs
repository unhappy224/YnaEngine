using System;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace Yna.Engine.Input.Kinect
{
    public class KinectUserProfile
    {
        public int Id { get; set; }
        public bool Available { get; protected set; }
        public Vector3 Head { get; protected set; }
        public Vector3 HandLeft { get; protected set; }
        public Vector3 HandRight { get; protected set; }

        public KinectUserProfile()
        {
            Available = false;
            Head = Vector3.Zero;
            HandLeft = Vector3.Zero;
            HandRight = Vector3.Zero;
        }

        public void SetVector3(JointType jointType, Vector3 position)
        {
            // Hands
            if (jointType == JointType.HandLeft)
                HandLeft = position;

            else if (jointType == JointType.HandRight)
                HandRight = position;

            // Head
            else if (jointType == JointType.Head)
                Head = position;
        }

        public void SetAvailable(bool available)
        {
            Available = available;
        }
    }
}
