using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    sealed public class FaceTrackingFramesSenseEventArgs : EventArgs
    {

        private List<PXCMFaceAnalysis.Detection.Data> arrayDetection = null;
        private List<PXCMFaceAnalysis.Landmark.LandmarkData[]> arrayLandmark = null;
        private PXCMImage objImage;

        public FaceTrackingFramesSenseEventArgs(List<PXCMFaceAnalysis.Detection.Data> ArrayDetection, List<PXCMFaceAnalysis.Landmark.LandmarkData[]> ArrayLandmark, PXCMImage ObjImage) 
        {
            arrayDetection = ArrayDetection;
            arrayLandmark = ArrayLandmark;
            objImage = ObjImage;
        }


        public List<PXCMFaceAnalysis.Detection.Data> ArrayDetection
        {
            get { return arrayDetection; }
        }

        public List<PXCMFaceAnalysis.Landmark.LandmarkData[]> ArrayLandmark
        {
            get { return arrayLandmark; }
        }

        public PXCMImage ObjImage
        {
            get { return objImage; }
        }
    }
}
