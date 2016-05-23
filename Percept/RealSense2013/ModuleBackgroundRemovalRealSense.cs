using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;

namespace FwPercept
{
    public sealed class ModuleBackgroundRemovalRealSense : IModule, IModuleBackground
    {
        private BackgroundModeEnum backgroundMode;
        private string imgSourceFile = null;
        
        public enum BackgroundModeEnum 
        {
            BgWhite = PXCMSegmentation.BlendMode.BLEND_BG_WHITE,
            BgBlue = PXCMSegmentation.BlendMode.BLEND_BG_BLUE,
            BgImage = PXCMSegmentation.BlendMode.BLEND_BG_IMAGE,
            BgAlpha = PXCMSegmentation.BlendMode.BLEND_ALPHA_CHANNEL
        }

        public ModuleBackgroundRemovalRealSense(BackgroundModeEnum Mode) 
        {
            backgroundMode = Mode;
        }

        public ModuleBackgroundRemovalRealSense(BackgroundModeEnum Mode, string ImgSourceFile)
        {
            backgroundMode = Mode;
            if (Mode == BackgroundModeEnum.BgImage)
            {
                imgSourceFile = ImgSourceFile;
            }            
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "BackgroundRemovalRealSense";
        }

        public int GetIdModule()
        {
            return 7;
        }

        private Bitmap GetImageFromEvent(PXCMImage image) 
        {
            PXCMImage.ImageData data;
            Bitmap img = null;
            pxcmStatus status = image.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, out data);

            if (status == pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                img = data.ToBitmap(image.info.width, image.info.height);
                image.ReleaseAccess(ref data);
            }
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return img;
        }

        public byte[] GetDataFromImageFrame(EventArgs e)
        {
            byte[] bytes = null;
            Bitmap image = GetImageFromEvent(((BackgroundRemovalFramesSenseEventArgs)e).ObjImage);

            if (image == null)
                return null;
            BitmapSource bmpSource = ConvertBitmapToBitmapSource(image);

            // Calculate stride of source
            int stride = bmpSource.PixelWidth * (bmpSource.Format.BitsPerPixel / 8);

            // Create data array to hold source pixel data
            bytes = new byte[stride * bmpSource.PixelHeight];

            // Copy source image pixels to the data array
            bmpSource.CopyPixels(bytes, stride, 0);

            return bytes;
        }

        public WriteableBitmap BuildWriteableBitmap(EventArgs e)
        {
            byte[] bytes = null;
            Bitmap image = GetImageFromEvent(((BackgroundRemovalFramesSenseEventArgs)e).ObjImage);

            if (image == null)
                return null;
            BitmapSource bmpSource = ConvertBitmapToBitmapSource(image);
            //BitmapSource bmpSource = BitmapToBitmapSource(e.Image);

            // Calculate stride of source
            int stride = bmpSource.PixelWidth * (bmpSource.Format.BitsPerPixel / 8);

            // Create data array to hold source pixel data
            bytes = new byte[stride * bmpSource.PixelHeight];

            // Copy source image pixels to the data array
            bmpSource.CopyPixels(bytes, stride, 0);

            // Create WriteableBitmap to copy the pixel data to.      
            WriteableBitmap target = new WriteableBitmap(
              bmpSource.PixelWidth,
              bmpSource.PixelHeight,
              bmpSource.DpiX, bmpSource.DpiY,
              bmpSource.Format, null);

            // Write the pixel data to the WriteableBitmap.
            target.WritePixels(
              new Int32Rect(0, 0, bmpSource.PixelWidth, bmpSource.PixelHeight),
              bytes, stride, 0);

            return target;
        }

        public WriteableBitmap BuildWriteableBitmap2(BackgroundRemovalFramesSenseEventArgs e)
        {
            Bitmap image = GetImageFromEvent(e.ObjImage);
            if (image == null)
                return null;
            WriteableBitmap colorBitmap = new WriteableBitmap(ConvertBitmapToBitmapSource(image));
            //WriteableBitmap colorBitmap = new WriteableBitmap(BitmapToBitmapSource(e.Image));
            return colorBitmap;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern Int32 DeleteObject(IntPtr hGDIObj);

        private BitmapSource ConvertBitmapToBitmapSource(Bitmap bmp)
        {
            if (bmp == null)
            {
                return null;
            }
            lock (bmp)
            {
                IntPtr hBitmap = bmp.GetHbitmap();
                System.Windows.Media.Imaging.BitmapSource bs =
                    System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                      hBitmap,
                      IntPtr.Zero,
                      System.Windows.Int32Rect.Empty,
                      System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(hBitmap);
                bmp.Dispose();
                return bs;
            }
        }

        private Bitmap BitmapSourceToBitmap(BitmapSource bitmapsource)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(stream);

                using (var tempBitmap = new Bitmap(stream))
                {
                    // According to MSDN, one "must keep the stream open for the lifetime of the Bitmap."
                    // So we return a copy of the new bitmap, allowing us to dispose both the bitmap and the stream.
                    return new Bitmap(tempBitmap);
                }
            }
        }

        private BitmapSource BitmapToBitmapSource(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public BackgroundModeEnum BackgroundMode
        {
            get { return backgroundMode; }
        }

        public string ImgSourceFile
        {
            get { return imgSourceFile; }
        }
    }
}
