using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class GestureRealSense : IGestus
    {

        private string nameGestus;
        private int idGestus;
        private Boolean active;
        private int idJoint;

        public enum GestureEnum
        {
            Swipe_Left = PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_LEFT,
            Swipe_Right = PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_RIGHT,
            Swipe_Up = PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_UP,
            Swipe_Down = PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN,
            HandWave = PXCMGesture.Gesture.Label.LABEL_HAND_WAVE,
            HandCircle = PXCMGesture.Gesture.Label.LABEL_HAND_CIRCLE,
            ThumbUp = PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP,
            ThumbDown = PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN,
            Peace = PXCMGesture.Gesture.Label.LABEL_POSE_PEACE,
            Big5 = PXCMGesture.Gesture.Label.LABEL_POSE_BIG5,
            None
        }

        public GestureRealSense(string name,GestureEnum id,Boolean isActive,JointRealSense.JointEnum IdJoint) 
        {

            nameGestus = name;
            idGestus = (int)id;
            active = isActive;
            idJoint = (int)IdJoint;
        }

        public string GetName()
        {
            return nameGestus;
        }

        public int GetId()
        {
            return idGestus;
        }

        public Boolean IsActive() 
        {
            return active;
        }

        public int IdJointOrigin()
        {
            return idJoint;
        }
    }
}
