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
    public class ModuleDepthKinect : IModule, IModuleImage
    {
        private ImageFormatEnum imgFormat;

        public enum ImageFormatEnum
        {
            Depth640_480_30fps = DepthImageFormat.Resolution640x480Fps30
        }

        public ModuleDepthKinect(ImageFormatEnum ImageFormat) 
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
            return "DepthKinect";
        }

        public int GetIdModule()
        {
            return 2;
        }

        public byte[] GetDataFromImageFrame(EventArgs e)
        {
            byte[] bytes = null;

            using (DepthImageFrame dFrame = ((AllFramesKinectEventArgs)e).ObjFrame.OpenDepthImageFrame())
            {
                if (dFrame == null)
                    return null;
                bytes = convertDepthToRGB(dFrame);
            }

            return bytes;	
        }

        private byte[] convertDepthToRGB(DepthImageFrame depthFrame)
        {
            //B G R Vazio
            //16 Bits
            short[] depthRaw = new short[depthFrame.PixelDataLength];
            byte[] colorPixels = null;

            depthFrame.CopyPixelDataTo(depthRaw);

            // RGB vazio
            colorPixels = new byte[depthFrame.Height * depthFrame.Width * sizeof(int)];

            //Profundidade e o Jogador
            int depth = 0;

            byte blue, red, green;

            // Convert the depth to RGB
            int colorPixelIndex = 0;
            for (int i = 0; i < depthRaw.Length; i++)
            {
                //shift pra direita de 3 para pegar apenas a parte da profundidade
                depth = (int)(depthRaw[i] >> 3);

                blue = (byte)(255 - ((depth - 800) * 255 / (4096 - 800)));
                green = (byte)(255 - ((depth - 800) * 255 / (4096 - 800)));
                red = (byte)(255 - ((depth - 800) * 255 / (4096 - 800)));

                //Conversão de um ponto para os 4 bytes
                colorPixels[colorPixelIndex++] = blue;//1º byte
                colorPixels[colorPixelIndex++] = green;//2º byte
                colorPixels[colorPixelIndex++] = red;//3º byte
                colorPixelIndex++;//4º byte vazio
            }
            return colorPixels;
        }

        public short[] GetDepthFrame(AllFramesKinectEventArgs e)
        {
            short[] depthRaw = null;

            using (DepthImageFrame dFrame = e.ObjFrame.OpenDepthImageFrame())
            {
                if (dFrame == null)
                    return null;
                depthRaw = new short[dFrame.PixelDataLength];                
            }

            return depthRaw;
        }

        public WriteableBitmap BuildWriteableBitmap(EventArgs e)
        {
            byte[] bytes = null;

            using (DepthImageFrame dFrame = ((AllFramesKinectEventArgs)e).ObjFrame.OpenDepthImageFrame())
            {
                if (dFrame == null)
                    return null;

                bytes = convertDepthToRGB(dFrame);

                WriteableBitmap colorBitmap = new WriteableBitmap(dFrame.Width, dFrame.Height, 96, 96, PixelFormats.Bgr32, null);

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
