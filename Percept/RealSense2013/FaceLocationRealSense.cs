using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class FaceLocationRealSense : IFace
    {

        private int faceId;
        private float positionX;
        private float positionY;
        private float positionZ;
        private ViewAngleEnum viewAngle;

        public enum ViewAngleEnum
        {
            AngleFrontal = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_FRONTAL,
            Angle90Left = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_0,
            Angle90Right = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_180,
            Angle90All = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_MULTI,
            Angle45Left = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_45,
            Angle45Right = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_135,
            Angle45All = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_HALF_MULTI,
            InclinedFrontal = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ANGLE_FRONTALROLL,
            Inclined30Left = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ROLL_30N,
            Inclined30Right = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ROLL_30,
            Inclined60Left = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ROLL_60N,
            Inclined60Right = PXCMFaceAnalysis.Detection.ViewAngle.VIEW_ROLL_60,
            None
        }

        public FaceLocationRealSense(int FaceId, float PositionX, float PositionY, float PositionZ, ViewAngleEnum ViewAngle) 
        {
            faceId = FaceId;
            positionX = PositionX;
            positionY = PositionY;
            positionZ = PositionZ;
            viewAngle = ViewAngle;
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
        

        public ViewAngleEnum ViewAngle
        {
            get { return viewAngle; }
        }

    }
}
