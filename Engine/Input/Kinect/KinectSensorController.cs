using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Kinect;
using Yna;

namespace Yna.Engine.Input.Kinect
{
    public class KinectSensorController
    {
        private bool _isAvailable;
        private float maxX = 0.4f;
        private float maxY = 0.4f;
        private int _screenWidth;
        private int _screenHeight;
        private KinectUserProfile _userProfil;
        private KinectSensor _kinectSensor;
        private Skeleton[] _cacheSkeletons;
        private Skeleton _cacheSkeleton;

        #region Properties

        /// <summary>
        /// Get or Set the status of the Device
        /// </summary>
        public bool IsAvailable
        {
            get { return _isAvailable; }
            protected set { _isAvailable = value; }
        }

        public KinectUserProfile User
        {
            get { return _userProfil; }
        }

        public float MaxX
        {
            get { return maxX; }
            set { maxX = value; }
        }

        public float MaxY
        {
            get { return maxY; }
            set { maxY = value; }
        }

        #endregion

        public KinectSensorController(int screenWidth, int screenHeight)
        {
            SetScreenSize(screenWidth, screenHeight);
            Initialize();
        }

        public void SetScreenSize(int width, int height)
        {
            _screenWidth = width;
            _screenHeight = height;
        }

        private void Initialize()
        {
            _userProfil = new KinectUserProfile();

            if (KinectSensor.KinectSensors.Count > 0)
            {
                // We take the first (for now)
                _kinectSensor = KinectSensor.KinectSensors[0];

                // We adjust the defaults parameters for the sensor
                TransformSmoothParameters parameters = new TransformSmoothParameters
                {
                    Correction = 0.3f,
                    JitterRadius = 1.0f,
                    MaxDeviationRadius = 0.5f,
                    Prediction = 0.4f,
                    Smoothing = 0.7f
                };

                _kinectSensor.SkeletonStream.Enable(parameters);
                _kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

                try
                {
                    _kinectSensor.Start();
                    _isAvailable = true;
                }
                catch (System.IO.IOException ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    _isAvailable = false;
                }
            }
            else
                _isAvailable = false;
        }

        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                // No player
                if (skeletonFrame == null)
                    return;

                int skeletonsSize = skeletonFrame.SkeletonArrayLength;

                _cacheSkeletons = new Skeleton[skeletonsSize];

                skeletonFrame.CopySkeletonDataTo(_cacheSkeletons);

                for (int i = 0; i < skeletonsSize; i++)
                {
                    _cacheSkeleton = _cacheSkeletons[i];

                    if (_cacheSkeleton.TrackingState == SkeletonTrackingState.Tracked)
                    {

                        if (_cacheSkeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked &&
                            _cacheSkeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                        {
                            Joint jointRight = _cacheSkeleton.Joints[JointType.HandRight];
                            Joint jointLeft = _cacheSkeleton.Joints[JointType.HandLeft];

                            Joint scaledRight = jointRight.ScaleTo(_screenWidth, _screenHeight, maxX, maxY);
                            Joint scaledLeft = jointLeft.ScaleTo(_screenWidth, _screenHeight, maxX, maxY);

                            _userProfil.SetVector3(scaledLeft.JointType, new Vector3(
                                scaledLeft.Position.X,
                                scaledLeft.Position.Y,
                                scaledLeft.Position.Z));

                            _userProfil.SetVector3(scaledRight.JointType, new Vector3(
                               scaledRight.Position.X,
                               scaledRight.Position.Y,
                               scaledRight.Position.Z));
                        }

                        _userProfil.SetVector3(JointType.Head, new Vector3(
                            _cacheSkeleton.Joints[JointType.Head].Position.X,
                            _cacheSkeleton.Joints[JointType.Head].Position.Y,
                            _cacheSkeleton.Joints[JointType.Head].Position.Z));

                        return;
                    }
                }
            }
        }
    }
}
