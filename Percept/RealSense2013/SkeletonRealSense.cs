using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class SkeletonRealSense : ISkeleton
    {

        private PXCMGesture mySkeleton;
        private List<IJoint> joints = new List<IJoint>(); 

        public SkeletonRealSense(PXCMGesture gesture) 
        {
            mySkeleton = gesture;
            addJoints();
        }

        private void addJoints()
        {
            PXCMGesture.GeoNode node;

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_ELBOW_PRIMARY, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "ElbowPrimary", JointRealSense.JointEnum.ElbowPrimary));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_ELBOW_SECONDARY, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "ElbowSecondary", JointRealSense.JointEnum.ElbowSecondary));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_ELBOW_LEFT, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "ElbowLeft", JointRealSense.JointEnum.ElbowLeft));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_ELBOW_RIGHT, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "ElbowRight", JointRealSense.JointEnum.ElbowRight));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "HandPrimary", JointRealSense.JointEnum.HandPrimary));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "HandSecondary", JointRealSense.JointEnum.HandSecondary));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "HandLeft", JointRealSense.JointEnum.HandLeft));

            mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT, out node);
            joints.Add(new JointRealSense(GetId(), mySkeleton, node, "HandRight", JointRealSense.JointEnum.HandRight));
        }

        public List<IJoint> GetJoints()
        {
            return joints;
        }

        public JointRealSense GetJoint(JointRealSense.JointEnum idJoint)
        {
            return (JointRealSense)joints[(int)idJoint];
        }

        public int GetId()
        {
            return 1;
        }
    }
}
