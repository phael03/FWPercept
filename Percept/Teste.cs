using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FwPercept
{
    public class Teste
    {
        Camera device = null;
        ModuleColorKinect colorKinect = null;
        ModuleDepthKinect depthKinect = null;
        ModuleSkeletonKinect skeleton = null;
        WriteableBitmap colorBitmap = null;

        Camera camRealSense = null;
        ModuleImageRealSense imageSense = null;        

        public void main()
        {
            Percept framework = Percept.GetInstance();
            device = framework.GetCamera(Percept.KINECT);

            colorKinect = new ModuleColorKinect(ModuleColorKinect.Rgb640_480_30fps);
            depthKinect = new ModuleDepthKinect(ModuleDepthKinect.Depth640_480_30fps);
            skeleton = new ModuleSkeletonKinect();

            device.EnableModule(colorKinect);
            /*device.EnableModule(depthKinect);*/
            device.EnableModule(skeleton);
            device.InitCamera();

            ((Kinect)device).GetAllFrames += Teste_GetAllFrames;
            
        }

        void Teste_GetAllFrames(object sender, AllFramesKinectEventArgs e)
        {
            colorBitmap = colorKinect.BuildWriteableBitmap(e);

            //Um ou Outro

            /*colorBitmap = depthKinect.BuildWriteableBitmap(e);*/

            SkeletonKinect mySkeleton = skeleton.GetFirstDataSkeleton(e);
            JointKinect head = (JointKinect)mySkeleton.GetJoints()[(int)JointKinect.JointEnum.Head];

            float x = head.PositionX;
        }

    }
}
