using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class JointRealSense : IJoint
    {
        private PXCMGesture.GeoNode myJoint;
        private PXCMGesture mySkeleton = null;
        private string nameJoint;
        private int idJoint;
        private List<IJoint> subJoints = new List<IJoint>();
        private int idSkeleton;

        public enum JointEnum
        {
            ElbowPrimary,
            ElbowSecondary,
            ElbowLeft,
            ElbowRight,
            HandPrimary,
            HandSecondary,
            HandLeft,
            HandRight,
            None
        }

        public enum JointHandEnum
        {
            FingerThumb,
            FingerIndex,
            FingerMiddle,
            FingerRing,
            FingerPinky,
            None
        }

        public enum OpennessState
        {
            Open,
            Close,
            Unknow
        }

        public JointRealSense(int IdSkeleton, PXCMGesture gesture, PXCMGesture.GeoNode joint, string name, JointEnum id)
        {
            myJoint = joint;
            nameJoint = name;
            idJoint = (int)id;
            mySkeleton = gesture;
            addSubJoints(id);
            idSkeleton = IdSkeleton;
        }

        public JointRealSense(int IdSkeleton, PXCMGesture.GeoNode joint, string name, JointHandEnum id)
        {
            myJoint = joint;
            nameJoint = name;
            idJoint = (int)id;
            mySkeleton = null;
            idSkeleton = IdSkeleton;
        }

        private void addSubJoints(JointEnum id)
        {
            PXCMGesture.GeoNode node;

            switch (id)
            {
                case JointEnum.HandPrimary:
                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerThumb", JointRealSense.JointHandEnum.FingerThumb));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerIndex", JointRealSense.JointHandEnum.FingerIndex));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerMiddle", JointRealSense.JointHandEnum.FingerMiddle));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerRing", JointRealSense.JointHandEnum.FingerRing));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerPinky", JointRealSense.JointHandEnum.FingerPinky));
                    break;
                case JointEnum.HandSecondary:
                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerThumb", JointRealSense.JointHandEnum.FingerThumb));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerIndex", JointRealSense.JointHandEnum.FingerIndex));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerMiddle", JointRealSense.JointHandEnum.FingerMiddle));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerRing", JointRealSense.JointHandEnum.FingerRing));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerPinky", JointRealSense.JointHandEnum.FingerPinky));
                    break;
                case JointEnum.HandLeft:
                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerThumb", JointRealSense.JointHandEnum.FingerThumb));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT | PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerIndex", JointRealSense.JointHandEnum.FingerIndex));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerMiddle", JointRealSense.JointHandEnum.FingerMiddle));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerRing", JointRealSense.JointHandEnum.FingerRing));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_LEFT | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerPinky", JointRealSense.JointHandEnum.FingerPinky));
                    break;
                case JointEnum.HandRight:
                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT | PXCMGesture.GeoNode.Label.LABEL_FINGER_THUMB, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerThumb", JointRealSense.JointHandEnum.FingerThumb));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT | PXCMGesture.GeoNode.Label.LABEL_FINGER_INDEX, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerIndex", JointRealSense.JointHandEnum.FingerIndex));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT | PXCMGesture.GeoNode.Label.LABEL_FINGER_MIDDLE, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerMiddle", JointRealSense.JointHandEnum.FingerMiddle));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT | PXCMGesture.GeoNode.Label.LABEL_FINGER_RING, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerRing", JointRealSense.JointHandEnum.FingerRing));

                    mySkeleton.QueryNodeData(0, PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_RIGHT | PXCMGesture.GeoNode.Label.LABEL_FINGER_PINKY, out node);
                    subJoints.Add(new JointRealSense(GetIdSkeletonOrigin(), node, "FingerPinky", JointRealSense.JointHandEnum.FingerPinky));
                    break;
            }
        }

        public List<IJoint> GetSubJoints()
        {
            return subJoints;
        }

        public JointRealSense GetSubJoint(JointRealSense.JointEnum idJoint)
        {
            return (JointRealSense)subJoints[(int)idJoint];
        }

        public Boolean IsSubJoint()
        {
            if (subJoints.Count > 0)
                return true;
            else
                return false;
        }

        public OpennessState HandIsOpen()
        {
            if (idJoint.Equals(JointEnum.HandPrimary) || idJoint.Equals(JointEnum.HandSecondary) || idJoint.Equals(JointEnum.HandLeft) || idJoint.Equals(JointEnum.HandRight))
            {
                if (myJoint.opennessState == PXCMGesture.GeoNode.Openness.LABEL_OPEN)
                    return OpennessState.Open;
                else if (myJoint.opennessState == PXCMGesture.GeoNode.Openness.LABEL_CLOSE)
                    return OpennessState.Close;
            }
            return OpennessState.Unknow;
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

        public int[] MapJointPositionsXYToScreen(object dictionary, float x, float y,float z)
        {
            int[] array = null;
            Dictionary<int, object> lista = (Dictionary<int, object>)dictionary;
            MyPipeline pepiline = (MyPipeline)lista[2];

            PXCMImage depth = pepiline.QueryImage(PXCMImage.ImageType.IMAGE_TYPE_DEPTH);
            PXCMImage.ImageInfo dinfo = depth.info;

            PXCMImage color = pepiline.QueryImage(PXCMImage.ImageType.IMAGE_TYPE_COLOR);
            PXCMImage.ImageInfo cinfo = color.info;

            PXCMImage.ImageData ddata;
            depth.AcquireAccess(PXCMImage.Access.ACCESS_READ, out ddata);
            
            float[] uvmap = ddata.ToFloatArray(2, (int)(2 * dinfo.width * dinfo.height));

            int index = ((int)y) * (int)dinfo.width + (int)x;
            x = uvmap[index * 2] * cinfo.width;
            y = uvmap[index * 2 + 1] * cinfo.height;
            
            depth.ReleaseAccess(ref ddata);            

            array = new int[] { (int)x, (int)y };

            return array;
        }

        public float PositionX
        {
            get { return myJoint.positionWorld.x; }
        }

        public float PositionY
        {
            get { return myJoint.positionWorld.y; }
        }

        public float PositionZ
        {
            get { return myJoint.positionWorld.z; }
        }

        public float PositionPixelX
        {
            get { return myJoint.positionImage.x; }
        }

        public float PositionPixelY
        {
            get { return myJoint.positionImage.y; }
        }

        public float PositionPixelZ
        {
            get { return myJoint.positionImage.z; }
        }
    }
}
