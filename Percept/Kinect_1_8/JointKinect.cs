using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace FwPercept
{
    public sealed class JointKinect : IJoint
    {

        private Microsoft.Kinect.Joint myJoint;
        private string nameJoint;
        private int idJoint;
        private int idSkeleton;

        public enum JointEnum 
        {
            AnkleLeft,
            AnkleRight,
            ElbowLeft,
            ElbowRight,
            FootLeft,
            FootRight,
            HandLeft,
            HandRight,
            Head,
            HipCenter,
            HipLeft,
            HipRight,
            KneeLeft,
            KneeRight,
            ShoulderCenter,
            ShoulderLeft,
            ShoulderRight,
            Spine,
            WristLeft,
            WristRight
        }

        public JointKinect(int IdSkeleton,Microsoft.Kinect.Joint joint, string name, JointEnum id) 
        {
            myJoint = joint;
            nameJoint = name;
            idJoint = (int)id;
            idSkeleton = IdSkeleton;
        }

        public string GetName()
        {
            return nameJoint;
        }

        public int GetId()
        {
            return idJoint;
        }

        public int GetIdSkeletonOrigin()
        {
            return idSkeleton;
        }

        public int MapJointPositionXToScreen(object sensor)
        {
            CoordinateMapper mapping = ((Microsoft.Kinect.KinectSensor)sensor).CoordinateMapper;
            var point = mapping.MapSkeletonPointToColorPoint(myJoint.Position, ((Microsoft.Kinect.KinectSensor)sensor).ColorStream.Format);

            if (point == null)
                return 0;
            return point.X;
        }

        public int MapJointPositionYToScreen(object sensor)
        {
            CoordinateMapper mapping = ((Microsoft.Kinect.KinectSensor)sensor).CoordinateMapper;
            var point = mapping.MapSkeletonPointToColorPoint(myJoint.Position, ((Microsoft.Kinect.KinectSensor)sensor).ColorStream.Format);

            if (point == null)
                return 0;
            return point.Y;
        }

        public Boolean TrackingStatus() 
        {
            if (myJoint.TrackingState == Microsoft.Kinect.JointTrackingState.Tracked)
                return true;
            else
                return false;
        }

        public float PositionX
        {
            get { return myJoint.Position.X; }
        }

        public float PositionY
        {
            get { return myJoint.Position.Y; }
        }

        public float PositionZ
        {
            get { return myJoint.Position.Z; }
        }

        
    }
}
