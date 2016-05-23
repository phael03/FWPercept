using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public class ModuleGestusRealSense : IModule, IModuleSkeleton
    {

        public event EventHandler<JointEventArgs> ElbowPrimaryMoved;
        public event EventHandler<JointEventArgs> ElbowSecondaryMoved;
        public event EventHandler<JointEventArgs> ElbowLeftMoved;
        public event EventHandler<JointEventArgs> ElbowRightMoved;
        public event EventHandler<JointEventArgs> HandPrimaryMoved;
        public event EventHandler<JointEventArgs> HandSecondaryMoved;
        public event EventHandler<JointEventArgs> HandLeftMoved;
        public event EventHandler<JointEventArgs> HandRightMoved;
        public event EventHandler<JointEventArgs> FingerThumbMoved;
        public event EventHandler<JointEventArgs> FingerIndexMoved;
        public event EventHandler<JointEventArgs> FingerMiddleMoved;
        public event EventHandler<JointEventArgs> FingerRingMoved;
        public event EventHandler<JointEventArgs> FingerPinkyMoved;

        private JointRealSense CopyElbowPrimary = null;
        private JointRealSense CopyElbowSecondary = null;
        private JointRealSense CopyElbowLeft = null;
        private JointRealSense CopyElbowRight = null;
        private JointRealSense CopyHandPrimary = null;
        private JointRealSense CopyHandSecondary = null;
        private JointRealSense CopyHandLeft = null;
        private JointRealSense CopyHandRight = null;
        private JointRealSense CopyFingerThumb = null;
        private JointRealSense CopyFingerIndex = null;
        private JointRealSense CopyFingerMiddle = null;
        private JointRealSense CopyFingerRing = null;
        private JointRealSense CopyFingerPinky = null;

        public ModuleGestusRealSense()
            : base()
        {

        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "GestureRealSense";
        }

        public int GetIdModule()
        {
            return 3;
        }

        public ISkeleton GetDataSkeleton(EventArgs e)
        {
            SkeletonRealSense mySkeleton = null;
            mySkeleton = new SkeletonRealSense(((SkeletonFramesSenseEventArgs)e).ObjSkeleton);

            return mySkeleton;
        }

        public IGestus GetDataGesture(EventArgs e)
        {
            GestureRealSense myGesture = null;
            GestureRealSense.GestureEnum gestureAux = GestureRealSense.GestureEnum.None;
            JointRealSense.JointEnum jointAux = JointRealSense.JointEnum.None;
            string nameAux = "";
            Boolean statusAux = false;

            switch (((GestureSenseEventArgs)e).ObjGesture.label)
            {
                case PXCMGesture.Gesture.Label.LABEL_HAND_CIRCLE:
                    gestureAux = GestureRealSense.GestureEnum.HandCircle;
                    nameAux = "HandCircle";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_HAND_WAVE:
                    gestureAux = GestureRealSense.GestureEnum.HandWave;
                    nameAux = "HandWave";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN:
                    gestureAux = GestureRealSense.GestureEnum.Swipe_Down;
                    nameAux = "Swipe_Down";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_LEFT:
                    gestureAux = GestureRealSense.GestureEnum.Swipe_Left;
                    nameAux = "Swipe_Left";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_RIGHT:
                    gestureAux = GestureRealSense.GestureEnum.Swipe_Right;
                    nameAux = "Swipe_Right";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_UP:
                    gestureAux = GestureRealSense.GestureEnum.Swipe_Up;
                    nameAux = "Swipe_Up";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_BIG5:
                    gestureAux = GestureRealSense.GestureEnum.Big5;
                    nameAux = "Big5";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_PEACE:
                    gestureAux = GestureRealSense.GestureEnum.Peace;
                    nameAux = "Peace";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN:
                    gestureAux = GestureRealSense.GestureEnum.ThumbDown;
                    nameAux = "ThumbDown";
                    break;
                case PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP:
                    gestureAux = GestureRealSense.GestureEnum.ThumbUp;
                    nameAux = "ThumbUp";
                    break;
            }

            switch (((GestureSenseEventArgs)e).ObjGesture.body)
            {
                case PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_PRIMARY:
                    jointAux = JointRealSense.JointEnum.HandPrimary;
                    break;
                case PXCMGesture.GeoNode.Label.LABEL_BODY_HAND_SECONDARY:
                    jointAux = JointRealSense.JointEnum.HandSecondary;
                    break;
            }

            if (((GestureSenseEventArgs)e).ObjGesture.active)
                statusAux = true;

            myGesture = new GestureRealSense(nameAux, gestureAux, statusAux, jointAux);

            return myGesture;
        }

        public void EnableTrackingJoint(SkeletonRealSense skeleton, JointRealSense.JointEnum idJoint)
        {
            JointMoved(skeleton, idJoint, JointRealSense.JointHandEnum.None);
        }

        public void EnableTrackingSubJoint(SkeletonRealSense skeleton, JointRealSense.JointEnum idJoint, JointRealSense.JointHandEnum idSubJoint)
        {
            JointMoved(skeleton, idJoint, idSubJoint);
        }

        public void DisableTrackingJoint(JointRealSense.JointEnum idJoint)
        {
            switch (idJoint)
            {
                case JointRealSense.JointEnum.ElbowPrimary:
                    CopyElbowPrimary = null;
                    break;
                case JointRealSense.JointEnum.ElbowSecondary:
                    CopyElbowSecondary = null;
                    break;
                case JointRealSense.JointEnum.ElbowLeft:
                    CopyElbowLeft = null;
                    break;
                case JointRealSense.JointEnum.ElbowRight:
                    CopyElbowRight = null;
                    break;
                case JointRealSense.JointEnum.HandPrimary:
                    CopyHandPrimary = null;
                    break;
                case JointRealSense.JointEnum.HandSecondary:
                    CopyHandSecondary = null;
                    break;
                case JointRealSense.JointEnum.HandLeft:
                    CopyHandLeft = null;
                    break;
                case JointRealSense.JointEnum.HandRight:
                    CopyHandRight = null;
                    break;
            }
        }

        public void DisableTrackingSubJoint(JointRealSense.JointHandEnum idJoint)
        {
            switch (idJoint)
            {
                case JointRealSense.JointHandEnum.FingerThumb:
                    CopyFingerThumb = null;
                    break;
                case JointRealSense.JointHandEnum.FingerIndex:
                    CopyFingerIndex = null;
                    break;
                case JointRealSense.JointHandEnum.FingerMiddle:
                    CopyFingerMiddle = null;
                    break;
                case JointRealSense.JointHandEnum.FingerRing:
                    CopyFingerRing = null;
                    break;
                case JointRealSense.JointHandEnum.FingerPinky:
                    CopyFingerPinky = null;
                    break;
            }
        }

        private void JointMoved(SkeletonRealSense skeleton, JointRealSense.JointEnum idJoint, JointRealSense.JointHandEnum idSubJoint)
        {
            JointEventArgs args = new JointEventArgs();
            JointRealSense joint = (JointRealSense)skeleton.GetJoints()[(int)idJoint];
            JointRealSense subJoint = null;

            float basePositionX = 0;
            float basePositionY = 0;
            float basePositionZ = 0;
            Boolean moved = false;

            if (!(idSubJoint.Equals(JointRealSense.JointHandEnum.None)))
                subJoint = (JointRealSense)joint.GetSubJoints()[(int)idSubJoint];

            if (idJoint.Equals(JointRealSense.JointEnum.HandLeft))
            {
                if (subJoint != null)
                {
                    if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerThumb))
                    {
                        if (CopyFingerThumb == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerThumb = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerThumb.PositionX;
                            basePositionY = CopyFingerThumb.PositionY;
                            basePositionZ = CopyFingerThumb.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerIndex))
                    {
                        if (CopyFingerIndex == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerIndex = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerIndex.PositionX;
                            basePositionY = CopyFingerIndex.PositionY;
                            basePositionZ = CopyFingerIndex.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerMiddle))
                    {
                        if (CopyFingerMiddle == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerMiddle = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerMiddle.PositionX;
                            basePositionY = CopyFingerMiddle.PositionY;
                            basePositionZ = CopyFingerMiddle.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerRing))
                    {
                        if (CopyFingerRing == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerRing = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerRing.PositionX;
                            basePositionY = CopyFingerRing.PositionY;
                            basePositionZ = CopyFingerRing.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerPinky))
                    {
                        if (CopyFingerPinky == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerPinky = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerPinky.PositionX;
                            basePositionY = CopyFingerPinky.PositionY;
                            basePositionZ = CopyFingerPinky.PositionZ;
                        }
                    }
                }
                else
                {
                    if (CopyHandLeft == null)
                    {
                        basePositionX = joint.PositionX;
                        basePositionY = joint.PositionY;
                        basePositionZ = joint.PositionZ;

                        CopyHandLeft = joint;
                    }
                    else
                    {
                        basePositionX = CopyHandLeft.PositionX;
                        basePositionY = CopyHandLeft.PositionY;
                        basePositionZ = CopyHandLeft.PositionZ;
                    }
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.HandRight))
            {
                if (subJoint != null)
                {
                    if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerThumb))
                    {
                        if (CopyFingerThumb == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerThumb = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerThumb.PositionX;
                            basePositionY = CopyFingerThumb.PositionY;
                            basePositionZ = CopyFingerThumb.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerIndex))
                    {
                        if (CopyFingerIndex == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerIndex = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerIndex.PositionX;
                            basePositionY = CopyFingerIndex.PositionY;
                            basePositionZ = CopyFingerIndex.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerMiddle))
                    {
                        if (CopyFingerMiddle == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerMiddle = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerMiddle.PositionX;
                            basePositionY = CopyFingerMiddle.PositionY;
                            basePositionZ = CopyFingerMiddle.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerRing))
                    {
                        if (CopyFingerRing == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerRing = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerRing.PositionX;
                            basePositionY = CopyFingerRing.PositionY;
                            basePositionZ = CopyFingerRing.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerPinky))
                    {
                        if (CopyFingerPinky == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerPinky = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerPinky.PositionX;
                            basePositionY = CopyFingerPinky.PositionY;
                            basePositionZ = CopyFingerPinky.PositionZ;
                        }
                    }
                }
                else
                {
                    if (CopyHandRight == null)
                    {
                        basePositionX = joint.PositionX;
                        basePositionY = joint.PositionY;
                        basePositionZ = joint.PositionZ;

                        CopyHandRight = joint;
                    }
                    else
                    {
                        basePositionX = CopyHandRight.PositionX;
                        basePositionY = CopyHandRight.PositionY;
                        basePositionZ = CopyHandRight.PositionZ;
                    }
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.HandPrimary))
            {
                if (subJoint != null)
                {
                    if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerThumb))
                    {
                        if (CopyFingerThumb == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerThumb = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerThumb.PositionX;
                            basePositionY = CopyFingerThumb.PositionY;
                            basePositionZ = CopyFingerThumb.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerIndex))
                    {
                        if (CopyFingerIndex == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerIndex = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerIndex.PositionX;
                            basePositionY = CopyFingerIndex.PositionY;
                            basePositionZ = CopyFingerIndex.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerMiddle))
                    {
                        if (CopyFingerMiddle == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerMiddle = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerMiddle.PositionX;
                            basePositionY = CopyFingerMiddle.PositionY;
                            basePositionZ = CopyFingerMiddle.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerRing))
                    {
                        if (CopyFingerRing == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerRing = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerRing.PositionX;
                            basePositionY = CopyFingerRing.PositionY;
                            basePositionZ = CopyFingerRing.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerPinky))
                    {
                        if (CopyFingerPinky == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerPinky = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerPinky.PositionX;
                            basePositionY = CopyFingerPinky.PositionY;
                            basePositionZ = CopyFingerPinky.PositionZ;
                        }
                    }
                }
                else
                {
                    if (CopyHandPrimary == null)
                    {
                        basePositionX = joint.PositionX;
                        basePositionY = joint.PositionY;
                        basePositionZ = joint.PositionZ;

                        CopyHandPrimary = joint;
                    }
                    else
                    {
                        basePositionX = CopyHandPrimary.PositionX;
                        basePositionY = CopyHandPrimary.PositionY;
                        basePositionZ = CopyHandPrimary.PositionZ;
                    }
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.HandSecondary))
            {
                if (subJoint != null)
                {
                    if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerThumb))
                    {
                        if (CopyFingerThumb == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerThumb = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerThumb.PositionX;
                            basePositionY = CopyFingerThumb.PositionY;
                            basePositionZ = CopyFingerThumb.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerIndex))
                    {
                        if (CopyFingerIndex == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerIndex = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerIndex.PositionX;
                            basePositionY = CopyFingerIndex.PositionY;
                            basePositionZ = CopyFingerIndex.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerMiddle))
                    {
                        if (CopyFingerMiddle == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerMiddle = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerMiddle.PositionX;
                            basePositionY = CopyFingerMiddle.PositionY;
                            basePositionZ = CopyFingerMiddle.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerRing))
                    {
                        if (CopyFingerRing == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerRing = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerRing.PositionX;
                            basePositionY = CopyFingerRing.PositionY;
                            basePositionZ = CopyFingerRing.PositionZ;
                        }
                    }
                    else if (idSubJoint.Equals(JointRealSense.JointHandEnum.FingerPinky))
                    {
                        if (CopyFingerPinky == null)
                        {
                            basePositionX = subJoint.PositionX;
                            basePositionY = subJoint.PositionY;
                            basePositionZ = subJoint.PositionZ;

                            CopyFingerPinky = subJoint;
                        }
                        else
                        {
                            basePositionX = CopyFingerPinky.PositionX;
                            basePositionY = CopyFingerPinky.PositionY;
                            basePositionZ = CopyFingerPinky.PositionZ;
                        }
                    }
                }
                else
                {
                    if (CopyHandSecondary == null)
                    {
                        basePositionX = joint.PositionX;
                        basePositionY = joint.PositionY;
                        basePositionZ = joint.PositionZ;

                        CopyHandSecondary = joint;
                    }
                    else
                    {
                        basePositionX = CopyHandSecondary.PositionX;
                        basePositionY = CopyHandSecondary.PositionY;
                        basePositionZ = CopyHandSecondary.PositionZ;
                    }
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.ElbowPrimary))
            {
                if (CopyElbowPrimary == null)
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowPrimary = joint;
                }
                else
                {
                    basePositionX = CopyElbowPrimary.PositionX;
                    basePositionY = CopyElbowPrimary.PositionY;
                    basePositionZ = CopyElbowPrimary.PositionZ;
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.ElbowSecondary))
            {
                if (CopyElbowSecondary == null)
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowSecondary = joint;
                }
                else
                {
                    basePositionX = CopyElbowSecondary.PositionX;
                    basePositionY = CopyElbowSecondary.PositionY;
                    basePositionZ = CopyElbowSecondary.PositionZ;
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.ElbowLeft))
            {
                if (CopyElbowLeft == null)
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowLeft = joint;
                }
                else
                {
                    basePositionX = CopyElbowLeft.PositionX;
                    basePositionY = CopyElbowLeft.PositionY;
                    basePositionZ = CopyElbowLeft.PositionZ;
                }
            }
            else if (idJoint.Equals(JointRealSense.JointEnum.ElbowRight))
            {
                if (CopyElbowRight == null)
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowRight = joint;
                }
                else
                {
                    basePositionX = CopyElbowRight.PositionX;
                    basePositionY = CopyElbowRight.PositionY;
                    basePositionZ = CopyElbowRight.PositionZ;
                }
            }


            if (subJoint != null)
            {
                if (subJoint.PositionX != basePositionX)
                {
                    args.DifPositionX = subJoint.PositionX - basePositionX;
                    args.OrientationX = true;
                    args.CurrentPositionX = subJoint.PositionX;
                    moved = true;
                }
                if (subJoint.PositionY != basePositionY)
                {
                    args.DifPositionY = subJoint.PositionY - basePositionY;
                    args.OrientationY = true;
                    args.CurrentPositionY = subJoint.PositionY;
                    moved = true;
                }
                if (subJoint.PositionZ != basePositionZ)
                {
                    args.DifPositionZ = subJoint.PositionZ - basePositionZ;
                    args.OrientationZ = true;
                    args.CurrentPositionZ = subJoint.PositionZ;
                    moved = true;
                }

                if (moved)
                    OnJointMoved(args, idJoint, idSubJoint);
                moved = false;
            }
            else
            {
                if (joint.PositionX != basePositionX)
                {
                    args.DifPositionX = joint.PositionX - basePositionX;
                    args.OrientationX = true;
                    args.CurrentPositionX = joint.PositionX;
                    moved = true;
                }
                if (joint.PositionY != basePositionY)
                {
                    args.DifPositionY = joint.PositionY - basePositionY;
                    args.OrientationY = true;
                    args.CurrentPositionY = joint.PositionY;
                    moved = true;
                }
                if (joint.PositionZ != basePositionZ)
                {
                    args.DifPositionZ = joint.PositionZ - basePositionZ;
                    args.OrientationZ = true;
                    args.CurrentPositionZ = joint.PositionZ;
                    moved = true;
                }

                if (moved)
                    OnJointMoved(args, idJoint, idSubJoint);
                moved = false;
            }
        }

        private void OnJointMoved(JointEventArgs e, JointRealSense.JointEnum idJoint, JointRealSense.JointHandEnum idSubJoint)
        {
            EventHandler<JointEventArgs> handler = null;

            if (!(idSubJoint.Equals(JointRealSense.JointHandEnum.None)))
            {
                switch (idSubJoint)
                {
                    case JointRealSense.JointHandEnum.FingerThumb:
                        handler = FingerThumbMoved;
                        break;
                    case JointRealSense.JointHandEnum.FingerIndex:
                        handler = FingerIndexMoved;
                        break;
                    case JointRealSense.JointHandEnum.FingerMiddle:
                        handler = FingerMiddleMoved;
                        break;
                    case JointRealSense.JointHandEnum.FingerRing:
                        handler = FingerRingMoved;
                        break;
                    case JointRealSense.JointHandEnum.FingerPinky:
                        handler = FingerPinkyMoved;
                        break;
                }
            }
            else
            {
                switch (idJoint)
                {
                    case JointRealSense.JointEnum.ElbowPrimary:
                        handler = ElbowPrimaryMoved;
                        break;
                    case JointRealSense.JointEnum.ElbowSecondary:
                        handler = ElbowSecondaryMoved;
                        break;
                    case JointRealSense.JointEnum.ElbowLeft:
                        handler = ElbowLeftMoved;
                        break;
                    case JointRealSense.JointEnum.ElbowRight:
                        handler = ElbowRightMoved;
                        break;
                    case JointRealSense.JointEnum.HandPrimary:
                        handler = HandPrimaryMoved;
                        break;
                    case JointRealSense.JointEnum.HandSecondary:
                        handler = HandSecondaryMoved;
                        break;
                    case JointRealSense.JointEnum.HandLeft:
                        handler = HandLeftMoved;
                        break;
                    case JointRealSense.JointEnum.HandRight:
                        handler = HandRightMoved;
                        break;
                }
            }

            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}
