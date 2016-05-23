using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class FaceLandmarkRealSense : IFace
    {

        private int faceId;
        private float positionX;
        private float positionY;
        private float positionZ;
        private LandmarkJointEnum landmarkJoint;

        public enum LandmarkJointEnum
        {
            CornerInLeftEye = PXCMFaceAnalysis.Landmark.Label.LABEL_LEFT_EYE_INNER_CORNER,
            CornerOutLeftEye = PXCMFaceAnalysis.Landmark.Label.LABEL_LEFT_EYE_OUTER_CORNER,
            CornerInRightEye = PXCMFaceAnalysis.Landmark.Label.LABEL_RIGHT_EYE_INNER_CORNER,
            CornerOutRightEye = PXCMFaceAnalysis.Landmark.Label.LABEL_RIGHT_EYE_OUTER_CORNER,
            CornerLeftMouth = PXCMFaceAnalysis.Landmark.Label.LABEL_MOUTH_LEFT_CORNER,
            CornerRightMouth = PXCMFaceAnalysis.Landmark.Label.LABEL_MOUTH_RIGHT_CORNER,
            Nose = PXCMFaceAnalysis.Landmark.Label.LABEL_NOSE_TIP,
            None
        }

        public FaceLandmarkRealSense(int FaceId, float PositionX, float PositionY, float PositionZ, LandmarkJointEnum LandmarkJoint)             
        {
            faceId = FaceId;
            positionX = PositionX;
            positionY = PositionY;
            positionZ = PositionZ;
            landmarkJoint = LandmarkJoint;
        }

        public int GetFaceId()
        {
            return faceId;
        }

        public float GetPositionX()
        {
            return positionX;
        }

        public float GetPositionY()
        {
            return positionY;
        }

        public float GetPositionZ()
        {
            return positionZ;
        }

        public LandmarkJointEnum LandmarkJoint
        {
            get { return landmarkJoint; }
        }
    }
}
