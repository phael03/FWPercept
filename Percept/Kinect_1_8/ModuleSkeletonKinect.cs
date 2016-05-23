using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FwPercept
{
    public class ModuleSkeletonKinect : IModule, IModuleSkeleton
    {

        public event EventHandler<JointEventArgs> AnkleLeftMoved;
        public event EventHandler<JointEventArgs> AnkleRightMoved;
        public event EventHandler<JointEventArgs> ElbowLeftMoved;
        public event EventHandler<JointEventArgs> ElbowRightMoved;
        public event EventHandler<JointEventArgs> FootLeftMoved;
        public event EventHandler<JointEventArgs> FootRightMoved;
        public event EventHandler<JointEventArgs> HandLeftMoved;
        public event EventHandler<JointEventArgs> HandRightMoved;
        public event EventHandler<JointEventArgs> HeadMoved;
        public event EventHandler<JointEventArgs> HipCenterMoved;
        public event EventHandler<JointEventArgs> HipLeftMoved;
        public event EventHandler<JointEventArgs> HipRightMoved;
        public event EventHandler<JointEventArgs> KneeLeftMoved;
        public event EventHandler<JointEventArgs> KneeRightMoved;
        public event EventHandler<JointEventArgs> ShoulderCenterMoved;
        public event EventHandler<JointEventArgs> ShoulderLeftMoved;
        public event EventHandler<JointEventArgs> ShoulderRightMoved;
        public event EventHandler<JointEventArgs> SpinetMoved;
        public event EventHandler<JointEventArgs> WristLeftMoved;
        public event EventHandler<JointEventArgs> WristRightMoved;

        private Dictionary<int, JointKinect> CopyAnkleLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyAnkleRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyElbowLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyElbowRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyFootLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyFootRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHandLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHandRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHead = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHipCenter = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHipLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyHipRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyKneeLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyKneeRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyShoulderCenter = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyShoulderLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyShoulderRight = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopySpine = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyWristLeft = new Dictionary<int, JointKinect>();
        private Dictionary<int, JointKinect> CopyWristRight = new Dictionary<int, JointKinect>();

        public ModuleSkeletonKinect()
            : base()
        {
            
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(Kinect);
        }

        public string GetNameModule()
        {
            return "SkeletonKinect";
        }

        public int GetIdModule()
        {
            return 3;
        }

        public IGestus GetDataGesture(EventArgs e)
        {
            return null;
        }

        public ISkeleton GetDataSkeleton(EventArgs e)
        {
            Skeleton[] skeletons = null;
            SkeletonKinect myskeletonsKinect = null;

            using (SkeletonFrame sFrame = ((AllFramesKinectEventArgs)e).ObjFrame.OpenSkeletonFrame())
            {
                if (sFrame == null)
                    return null;

                skeletons = new Skeleton[sFrame.SkeletonArrayLength];
                sFrame.CopySkeletonDataTo(skeletons);
            }

            int i = 0;

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    myskeletonsKinect = new SkeletonKinect(skeleton);
                    break;
                }
            }

            return myskeletonsKinect;
        }

        public SkeletonKinect[] GetArrayDataSkeletons(AllFramesKinectEventArgs e)
        {
            Skeleton[] skeletons = null;
            SkeletonKinect[] skeletonsKinect = null;

            using (SkeletonFrame sFrame = e.ObjFrame.OpenSkeletonFrame())
            {
                if (sFrame == null)
                    return null;

                skeletons = new Skeleton[sFrame.SkeletonArrayLength];
                skeletonsKinect = new SkeletonKinect[sFrame.SkeletonArrayLength];
                sFrame.CopySkeletonDataTo(skeletons);
            }

            int i = 0;

            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState == SkeletonTrackingState.Tracked)
                {
                    skeletonsKinect[i] = new SkeletonKinect(skeleton);
                }
            }

            return skeletonsKinect;
        }

        public SkeletonKinect GetDataSkeletonByPlayer(AllFramesKinectEventArgs e, int index)
        {
            Skeleton[] skeletons = null;
            SkeletonKinect myskeletonsKinect = null;

            using (SkeletonFrame sFrame = e.ObjFrame.OpenSkeletonFrame())
            {
                if (sFrame == null)
                    return null;

                skeletons = new Skeleton[sFrame.SkeletonArrayLength];
                sFrame.CopySkeletonDataTo(skeletons);
            }

            int i = 0;

            if (index < 0 || index > (skeletons.Length - 1))
                return null;

            if (skeletons[index].TrackingState == SkeletonTrackingState.Tracked)
                myskeletonsKinect = new SkeletonKinect(skeletons[index]);
            else
                return null;
                                 
            return myskeletonsKinect;
        }

        public void EnableTrackingJoint(SkeletonKinect skeleton, JointKinect.JointEnum idJoint)
        {
            JointMoved(skeleton, idJoint);
        }

        public void DisableTrackingJoint(SkeletonKinect skeleton, JointKinect.JointEnum idJoint)
        {
            switch (idJoint)
            {
                case JointKinect.JointEnum.AnkleLeft:
                    CopyAnkleLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.AnkleRight:
                    CopyAnkleRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.ElbowLeft:
                    CopyElbowLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.ElbowRight:
                    CopyElbowRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.FootLeft:
                    CopyFootLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.FootRight:
                    CopyFootRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.HandLeft:
                    CopyHandLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.HandRight:
                    CopyHandRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.Head:
                    CopyHead[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.HipCenter:
                    CopyHipCenter[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.HipLeft:
                    CopyHipLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.HipRight:
                    CopyHipRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.KneeLeft:
                    CopyKneeLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.KneeRight:
                    CopyKneeRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.ShoulderCenter:
                    CopyShoulderCenter[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.ShoulderLeft:
                    CopyShoulderLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.ShoulderRight:
                    CopyShoulderRight[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.Spine:
                    CopySpine[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.WristLeft:
                    CopyWristLeft[skeleton.GetId()] = null;
                    break;
                case JointKinect.JointEnum.WristRight:
                    CopyWristRight[skeleton.GetId()] = null;
                    break;
            }
        }
        
        private void JointMoved(SkeletonKinect skeleton, JointKinect.JointEnum idJoint)
        {
            JointEventArgs args = new JointEventArgs();
            JointKinect joint = (JointKinect)skeleton.GetJoints()[(int)idJoint];
            JointKinect jointAux = null;

            float basePositionX = 0;
            float basePositionY = 0;
            float basePositionZ = 0;
            Boolean moved = false;

            if (idJoint.Equals(JointKinect.JointEnum.HandLeft))
            {
                if (!CopyHandLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHandLeft.Add(skeleton.GetId(), joint);
                }
                else 
                {
                    basePositionX = CopyHandLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyHandLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHandLeft[skeleton.GetId()].PositionZ;
                }                
            }
            else if (idJoint.Equals(JointKinect.JointEnum.HandRight))
            {
                if (!CopyHandRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHandRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyHandRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyHandRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHandRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.AnkleLeft))
            {
                if (!CopyAnkleLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyAnkleLeft.Add(skeleton.GetId(),joint);
                }
                else
                {
                    basePositionX = CopyAnkleLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyAnkleLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyAnkleLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.AnkleRight))
            {
                if (!CopyAnkleRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyAnkleRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyAnkleRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyAnkleRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyAnkleRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.ElbowLeft))
            {
                if (!CopyElbowLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyElbowLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyElbowLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyElbowLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.ElbowRight))
            {
                if (!CopyElbowRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyElbowRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyElbowRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyElbowRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyElbowRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.FootLeft))
            {
                if (!CopyFootLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyFootLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyFootLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyFootLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyFootLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.FootRight))
            {
                if (!CopyFootRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyFootRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyFootRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyFootRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyFootRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.Head))
            {
                if (!CopyHead.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHead.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyHead[skeleton.GetId()].PositionX;
                    basePositionY = CopyHead[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHead[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.HipCenter))
            {
                if (!CopyHipCenter.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHipCenter.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyHipCenter[skeleton.GetId()].PositionX;
                    basePositionY = CopyHipCenter[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHipCenter[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.HipLeft))
            {
                if (!CopyHipLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHipLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyHipLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyHipLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHipLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.HipRight))
            {
                if (!CopyHipRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyHipRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyHipRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyHipRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyHipRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.KneeLeft))
            {
                if (!CopyKneeLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyKneeLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyKneeLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyKneeLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyKneeLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.KneeRight))
            {
                if (!CopyKneeRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyKneeRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyKneeRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyKneeRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyKneeRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.ShoulderCenter))
            {
                if (!CopyShoulderCenter.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyShoulderCenter.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyShoulderCenter[skeleton.GetId()].PositionX;
                    basePositionY = CopyShoulderCenter[skeleton.GetId()].PositionY;
                    basePositionZ = CopyShoulderCenter[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.ShoulderLeft))
            {
                if (!CopyShoulderLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyShoulderLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyShoulderLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyShoulderLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyShoulderLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.ShoulderRight))
            {
                if (!CopyShoulderRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyShoulderRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyShoulderRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyShoulderRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyShoulderRight[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.Spine))
            {
                if (!CopySpine.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopySpine.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopySpine[skeleton.GetId()].PositionX;
                    basePositionY = CopySpine[skeleton.GetId()].PositionY;
                    basePositionZ = CopySpine[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.WristLeft))
            {
                if (!CopyWristLeft.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyWristLeft.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyWristLeft[skeleton.GetId()].PositionX;
                    basePositionY = CopyWristLeft[skeleton.GetId()].PositionY;
                    basePositionZ = CopyWristLeft[skeleton.GetId()].PositionZ;
                }
            }
            else if (idJoint.Equals(JointKinect.JointEnum.WristRight))
            {
                if (!CopyWristRight.TryGetValue(skeleton.GetId(), out jointAux))
                {
                    basePositionX = joint.PositionX;
                    basePositionY = joint.PositionY;
                    basePositionZ = joint.PositionZ;

                    CopyWristRight.Add(skeleton.GetId(), joint);
                }
                else
                {
                    basePositionX = CopyWristRight[skeleton.GetId()].PositionX;
                    basePositionY = CopyWristRight[skeleton.GetId()].PositionY;
                    basePositionZ = CopyWristRight[skeleton.GetId()].PositionZ;
                }
            }

            if (joint.TrackingStatus())
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
                    OnJointMoved(args,idJoint);
                moved = false;
            }
        }

        private void OnJointMoved(JointEventArgs e,JointKinect.JointEnum idJoint)
        {
            EventHandler<JointEventArgs> handler = null;
            switch (idJoint)
            {
                case JointKinect.JointEnum.AnkleLeft:
                    handler = AnkleLeftMoved;
                    break;
                case JointKinect.JointEnum.AnkleRight:
                    handler = AnkleRightMoved;
                    break;
                case JointKinect.JointEnum.ElbowLeft:
                    handler = ElbowLeftMoved;
                    break;
                case JointKinect.JointEnum.ElbowRight:
                    handler = ElbowRightMoved;
                    break;
                case JointKinect.JointEnum.FootLeft:
                    handler = FootLeftMoved;
                    break;
                case JointKinect.JointEnum.FootRight:
                    handler = FootRightMoved;
                    break;
                case JointKinect.JointEnum.HandLeft:
                    handler = HandLeftMoved;
                    break;
                case JointKinect.JointEnum.HandRight:
                    handler = HandRightMoved;
                    break;
                case JointKinect.JointEnum.Head:
                    handler = HeadMoved;
                    break;
                case JointKinect.JointEnum.HipCenter:
                    handler = HipCenterMoved;
                    break;
                case JointKinect.JointEnum.HipLeft:
                    handler = HipLeftMoved;
                    break;
                case JointKinect.JointEnum.HipRight:
                    handler = HipRightMoved;
                    break;
                case JointKinect.JointEnum.KneeLeft:
                    handler = KneeLeftMoved;
                    break;
                case JointKinect.JointEnum.KneeRight:
                    handler = KneeRightMoved;
                    break;
                case JointKinect.JointEnum.ShoulderCenter:
                    handler = ShoulderCenterMoved;
                    break;
                case JointKinect.JointEnum.ShoulderLeft:
                    handler = ShoulderLeftMoved;
                    break;
                case JointKinect.JointEnum.ShoulderRight:
                    handler = ShoulderRightMoved;
                    break;
                case JointKinect.JointEnum.Spine:
                    handler = SpinetMoved;
                    break;
                case JointKinect.JointEnum.WristLeft:
                    handler = WristLeftMoved;
                    break;
                case JointKinect.JointEnum.WristRight:
                    handler = WristRightMoved;
                    break;
            }            
            if (handler != null)
            {
                handler(this, e);
            }
        }

    }
}
