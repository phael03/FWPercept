using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class SkeletonKinect : ISkeleton
    {

        private Microsoft.Kinect.Skeleton mySkeleton;
        private List<IJoint> joints = new List<IJoint>();        

        public SkeletonKinect(Microsoft.Kinect.Skeleton skeleton)
        {
            mySkeleton = skeleton;
            addJoints();
        }

        private void addJoints() 
        {
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.AnkleLeft], "AnkleLeft", JointKinect.JointEnum.AnkleLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.AnkleRight], "AnkleRight", JointKinect.JointEnum.AnkleRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.ElbowLeft], "ElbowLeft", JointKinect.JointEnum.ElbowLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.ElbowRight], "ElbowRight", JointKinect.JointEnum.ElbowRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.FootLeft], "FootLeft", JointKinect.JointEnum.FootLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.FootRight], "FootRight", JointKinect.JointEnum.FootRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.HandLeft], "HandLeft", JointKinect.JointEnum.HandLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.HandRight], "HandRight", JointKinect.JointEnum.HandRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.Head], "Head", JointKinect.JointEnum.Head));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.HipCenter], "HipCenter", JointKinect.JointEnum.HipCenter));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.HipLeft], "HipLeft", JointKinect.JointEnum.HipLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.HipRight], "HipRight", JointKinect.JointEnum.HipRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.KneeLeft], "KneeLeft", JointKinect.JointEnum.KneeLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.KneeRight], "KneeRight", JointKinect.JointEnum.KneeRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.ShoulderCenter], "ShoulderCenter", JointKinect.JointEnum.ShoulderCenter));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.ShoulderLeft], "ShoulderLeft", JointKinect.JointEnum.ShoulderLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.ShoulderRight], "ShoulderRight", JointKinect.JointEnum.ShoulderRight));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.Spine], "Spine", JointKinect.JointEnum.Spine));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.WristLeft], "WristLeft", JointKinect.JointEnum.WristLeft));
            joints.Add(new JointKinect(GetId(), mySkeleton.Joints[Microsoft.Kinect.JointType.WristRight], "WristRight", JointKinect.JointEnum.WristRight));
        }

        public List<IJoint> GetJoints()
        {
            return joints;
        }

        public JointKinect GetJoint(JointKinect.JointEnum idJoint)
        {
            return (JointKinect)joints[(int)idJoint];
        }

        public int GetId()
        {
            return mySkeleton.TrackingId;
        }

        public float PositionX()
        {
            return mySkeleton.Position.X;
        }

        public float PositionY()
        {
            return mySkeleton.Position.Y;
        }

        public float PositionZ()
        {
            return mySkeleton.Position.Z;
        }

        public Boolean TrackingStatus()
        {
            if (mySkeleton.TrackingState == Microsoft.Kinect.SkeletonTrackingState.Tracked)
                return true;
            else 
                return false;
        }
        
    }
}
