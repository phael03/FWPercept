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
    public sealed class ModuleColorKinect : IModule, IModuleImage
    {
        private ImageFormatEnum imgFormat;

        public enum ImageFormatEnum
        {
            Rgb640_480_30fps = ColorImageFormat.RgbResolution640x480Fps30,
            Yuv640_480_15fps = ColorImageFormat.YuvResolution640x480Fps15,
            Rgb1280_960_12fps = ColorImageFormat.RgbResolution1280x960Fps12,
            Infrared640_480_30fps = ColorImageFormat.InfraredResolution640x480Fps30,
            RawBayer640_480_30fps = ColorImageFormat.RawBayerResolution640x480Fps30,
            RawBayer1280_960_12fps = ColorImageFormat.RawBayerResolution1280x960Fps12,
            RawYuv640_480_15fps = ColorImageFormat.RawYuvResolution640x480Fps15
        }

        public ModuleColorKinect(ImageFormatEnum ImageFormat) 
            : base()
        {
            imgFormat = ImageFormat;            
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(Kinect);
        }

        public string GetNameModule()
        {
            return "ColorKinect";
        }

        public int GetIdModule()
        {
            return 1;
        }

        public byte[] GetDataFromImageFrame(EventArgs e)
        {
            byte[] bytes = null;
            using (ColorImageFrame cFrame = ((AllFramesKinectEventArgs)e).ObjFrame.OpenColorImageFrame())
            {
                if (cFrame == null)
                    return null;

                bytes = new byte[cFrame.PixelDataLength];

                cFrame.CopyPixelDataTo(bytes);
            }
            return bytes;
        }

        public WriteableBitmap BuildWriteableBitmap(EventArgs e)
        {
            byte[] bytes = null;
            using (ColorImageFrame cFrame = ((AllFramesKinectEventArgs)e).ObjFrame.OpenColorImageFrame())
            {
                if (cFrame == null)
                    return null;

                bytes = new byte[cFrame.PixelDataLength];
                cFrame.CopyPixelDataTo(bytes);

                WriteableBitmap colorBitmap = new WriteableBitmap(cFrame.Width, cFrame.Height, 96, 96, PixelFormats.Bgr32, null);
                
                // Write the pixel data into our bitmap
                colorBitmap.WritePixels(
                      new Int32Rect(0, 0, colorBitmap.PixelWidth, colorBitmap.PixelHeight),
                      bytes,
                      colorBitmap.PixelWidth * sizeof(int),
                      0);

                return colorBitmap;
            }
        }

        public ImageFormatEnum ImgFormat
        {
            get { return imgFormat; }
        }

    }
}
