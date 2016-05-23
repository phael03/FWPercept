using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace FwPercept
{
    public sealed class ModuleBackgroundRemovalKinect : IModule, IModuleBackground
    {

        private ModuleColorKinect.ImageFormatEnum imgColorFormat;

        public ModuleBackgroundRemovalKinect(ModuleColorKinect.ImageFormatEnum ImgColorFormat) 
            : base()
        {
            imgColorFormat = ImgColorFormat;
        }
        
        public Type GetTypeTargetCamera()
        {
            return typeof(Kinect);
        }

        public string GetNameModule()
        {
            return "BackgroundRemovalKinect";
        }

        public int GetIdModule()
        {
            return 5;
        }

        public byte[] GetDataFromImageFrame(EventArgs e)
        {
            byte[] bytes = null;

            using (var backgroundRemovedFrame = ((BackgroundRemovalFramesKinectEventArgs)e).ObjFrame.OpenBackgroundRemovedColorFrame())
            {
                if (backgroundRemovedFrame == null)
                    return null;

                bytes = new byte[backgroundRemovedFrame.PixelDataLength];

                backgroundRemovedFrame.CopyPixelDataTo(bytes);
            }

            return bytes;
        }

        public WriteableBitmap BuildWriteableBitmap(EventArgs e)
        {
            byte[] bytes = null;
            using (var backgroundRemovedFrame = ((BackgroundRemovalFramesKinectEventArgs)e).ObjFrame.OpenBackgroundRemovedColorFrame())
            {
                if (backgroundRemovedFrame == null)
                    return null;

                bytes = new byte[backgroundRemovedFrame.PixelDataLength];
                backgroundRemovedFrame.CopyPixelDataTo(bytes);
                
                WriteableBitmap foregroundBitmap = new WriteableBitmap(backgroundRemovedFrame.Width, backgroundRemovedFrame.Height, 96.0, 96.0, PixelFormats.Bgra32, null);

                // Write the pixel data into our bitmap
                foregroundBitmap.WritePixels(
                    new Int32Rect(0, 0, foregroundBitmap.PixelWidth, foregroundBitmap.PixelHeight),
                    //backgroundRemovedFrame.GetRawPixelData(),
                    bytes,
                    foregroundBitmap.PixelWidth * sizeof(int),
                    0);
                
                return foregroundBitmap;
            }
        }

        public ModuleColorKinect.ImageFormatEnum ImgColorFormat
        {
            get { return imgColorFormat; }
        }

    }
}
