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
    public sealed class ModuloFaceTrackingRealSense : IModule, IModuleFace
    {
        public ModuloFaceTrackingRealSense()
            : base()
        {
            
        }

        public Type GetTypeTargetCamera()
        {
            return typeof(RealSense);
        }

        public string GetNameModule()
        {
            return "FaceTrackingRealSense";
        }

        public int GetIdModule()
        {
            return 5;
        }

        public IFace GetFaceFromFrame(EventArgs e)
        {
            List<PXCMFaceAnalysis.Detection.Data> arrayDetection = ((FaceTrackingFramesSenseEventArgs)e).ArrayDetection;
            FaceLocationRealSense loc = null;
            FaceLocationRealSense.ViewAngleEnum viewAngleAux = FaceLocationRealSense.ViewAngleEnum.None;

            if (arrayDetection != null && arrayDetection.Count > 0)
            {
                viewAngleAux = (FaceLocationRealSense.ViewAngleEnum)arrayDetection[0].viewAngle;
                loc = new FaceLocationRealSense(arrayDetection[0].fid, arrayDetection[0].rectangle.x, arrayDetection[0].rectangle.y, arrayDetection[0].rectangle.w, viewAngleAux);
            }
            return loc;
        }

        public FaceLocationRealSense GetFaceLocationByIdPerson(int idPerson,FaceTrackingFramesSenseEventArgs e) 
        {
            List<PXCMFaceAnalysis.Detection.Data> arrayDetection = e.ArrayDetection;
            FaceLocationRealSense loc = null;
            FaceLocationRealSense.ViewAngleEnum viewAngleAux = FaceLocationRealSense.ViewAngleEnum.None;

            if (arrayDetection != null && arrayDetection.Count > 0 && idPerson >= 1 && idPerson <= arrayDetection.Count) 
            {                
                viewAngleAux = (FaceLocationRealSense.ViewAngleEnum)arrayDetection[idPerson - 1]. viewAngle;                
                loc = new FaceLocationRealSense(arrayDetection[idPerson-1].fid,arrayDetection[idPerson-1].rectangle.x,arrayDetection[idPerson-1].rectangle.y,arrayDetection[idPerson-1].rectangle.w, viewAngleAux);
            }
            return loc;
        }

        public List<FaceLocationRealSense> GetListFaceLocation(FaceTrackingFramesSenseEventArgs e)
        {
            List<PXCMFaceAnalysis.Detection.Data> arrayDetection = e.ArrayDetection;
            List<FaceLocationRealSense> list = new List<FaceLocationRealSense>();
            FaceLocationRealSense loc = null;
            FaceLocationRealSense.ViewAngleEnum viewAngleAux = FaceLocationRealSense.ViewAngleEnum.None;

            if (arrayDetection != null && arrayDetection.Count > 0)
            {
                foreach (var item in arrayDetection)
                {
                    viewAngleAux = (FaceLocationRealSense.ViewAngleEnum)item.viewAngle;                    
                    loc = new FaceLocationRealSense(item.fid, item.rectangle.x, item.rectangle.y, item.rectangle.w, viewAngleAux);
                    list.Add(loc);
                    loc = null;
                }                
            }
            return list;
        }

        public List<FaceLandmarkRealSense> GetFaceLandmarksByIdPerson(int idPerson, FaceTrackingFramesSenseEventArgs e)
        {
            List<PXCMFaceAnalysis.Landmark.LandmarkData[]> arrayLandmark = e.ArrayLandmark;
            PXCMFaceAnalysis.Landmark.LandmarkData[] arraAux = null;
            List<FaceLandmarkRealSense> list = new List<FaceLandmarkRealSense>();
            FaceLandmarkRealSense land = null;
            FaceLandmarkRealSense.LandmarkJointEnum jointAux = FaceLandmarkRealSense.LandmarkJointEnum.None;

            if (arrayLandmark != null && arrayLandmark.Count > 0 && idPerson >= 1 && idPerson <= arrayLandmark.Count)
            {
                arraAux = arrayLandmark[idPerson - 1];

                for (int i = 0; i < arraAux.Length; i++)
                {
                    jointAux = (FaceLandmarkRealSense.LandmarkJointEnum)arraAux[i].label;                    
                    land = new FaceLandmarkRealSense(arraAux[i].fid, arraAux[i].position.x, arraAux[i].position.y, arraAux[i].position.z, jointAux);
                    list.Add(land);
                    land = null;
                }
            }
            return list;
        }

        public List<List<FaceLandmarkRealSense>> GetListFaceLandmarks(FaceTrackingFramesSenseEventArgs e)
        {
            List<PXCMFaceAnalysis.Landmark.LandmarkData[]> arrayLandmark = e.ArrayLandmark;
            PXCMFaceAnalysis.Landmark.LandmarkData[] arraAux = null;
            List<FaceLandmarkRealSense> preList = new List<FaceLandmarkRealSense>();
            List<List<FaceLandmarkRealSense>> list = new List<List<FaceLandmarkRealSense>>();
            FaceLandmarkRealSense land = null;
            FaceLandmarkRealSense.LandmarkJointEnum jointAux = FaceLandmarkRealSense.LandmarkJointEnum.None;

            if (arrayLandmark != null && arrayLandmark.Count > 0)
            {
                foreach (var item in arrayLandmark)
                {
                    arraAux = item;

                    for (int i = 0; i < arraAux.Length; i++)
                    {
                        jointAux = (FaceLandmarkRealSense.LandmarkJointEnum)arraAux[i].label;                        
                        land = new FaceLandmarkRealSense(arraAux[i].fid, arraAux[i].position.x, arraAux[i].position.y, arraAux[i].position.z, jointAux);
                        preList.Add(land);
                        list.Add(preList);
                        land = null;
                    }   
                }
            }
            return list;
        }

        public List<byte[]> SaveAllFacesActives(object obj) 
        {
            List<byte[]> list = new List<byte[]>();
            Dictionary<int, object> lista = (Dictionary<int, object>)obj;
            PXCMSession s = (PXCMSession)lista[1];

            PXCMFaceAnalysis face;
            s.CreateImpl<PXCMFaceAnalysis>(PXCMFaceAnalysis.CUID, out face);

            PXCMFaceAnalysis.ProfileInfo pinfo;
            face.QueryProfile(0, out pinfo);

            UtilMCapture capture = new UtilMCapture(s);
            capture.LocateStreams(ref pinfo.inputs);
            face.SetProfile(ref pinfo);

            PXCMImage[] images=new PXCMImage[PXCMCapture.VideoStream.STREAM_LIMIT];
            PXCMScheduler.SyncPoint[] sps = new PXCMScheduler.SyncPoint[2];
            // Get samples from input device and pass to the module
            capture.ReadStreamAsync(images,out sps[0]);
            face.ProcessImageAsync(images,out sps[1]);
            PXCMScheduler.SyncPoint.SynchronizeEx(sps);
            // Tracking or recognition results are ready. Now process them

            // Dispose
            PXCMScheduler.SyncPoint.Dispose(sps);
            PXCMImage.Dispose(images);

            for (uint i = 0; ; i++)
            {
                int fid; ulong ts;
                pxcmStatus sts = face.QueryFace(i, out fid, out ts);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Retrieve face model data */
                PXCMFaceAnalysis.Recognition rec = face.DynamicCast<PXCMFaceAnalysis.Recognition>(PXCMFaceAnalysis.Recognition.CUID);
                PXCMFaceAnalysis.Recognition.ProfileInfo dinfo;
                rec.QueryProfile(0, out dinfo);
                rec.SetProfile(ref dinfo);

                PXCMFaceAnalysis.Recognition.Model m1;
                rec.CreateModel(fid, out m1);

                if (m1 != null) 
                {
                    byte[] buffer = new byte[dinfo.modelSize];
                    m1.Serialize(buffer);

                    list.Add(buffer);
                }                      
            }
            return list;
        }

        public int RecognizeFace(object obj, byte[] buffer) 
        {
            Dictionary<int, object> lista = (Dictionary<int, object>)obj;
            PXCMSession s = (PXCMSession)lista[1];
            List<PXCMFaceAnalysis.Recognition.Model> listCompare = new List<PXCMFaceAnalysis.Recognition.Model>();
            PXCMFaceAnalysis.Recognition.Model[] arrayCompare;

            PXCMFaceAnalysis face;
            s.CreateImpl<PXCMFaceAnalysis>(PXCMFaceAnalysis.CUID, out face);

            PXCMFaceAnalysis.ProfileInfo pinfo;
            face.QueryProfile(0, out pinfo);

            UtilMCapture capture = new UtilMCapture(s);
            capture.LocateStreams(ref pinfo.inputs);
            face.SetProfile(ref pinfo);

            PXCMImage[] images = new PXCMImage[PXCMCapture.VideoStream.STREAM_LIMIT];
            PXCMScheduler.SyncPoint[] sps = new PXCMScheduler.SyncPoint[2];
            // Get samples from input device and pass to the module
            capture.ReadStreamAsync(images, out sps[0]);
            face.ProcessImageAsync(images, out sps[1]);
            PXCMScheduler.SyncPoint.SynchronizeEx(sps);
            // Tracking or recognition results are ready. Now process them

            // Dispose
            PXCMScheduler.SyncPoint.Dispose(sps);
            PXCMImage.Dispose(images);

            for (uint i = 0; ; i++)
            {
                int fid; ulong ts;
                pxcmStatus sts = face.QueryFace(i, out fid, out ts);
                if (sts < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Retrieve face model data */
                PXCMFaceAnalysis.Recognition rec = face.DynamicCast<PXCMFaceAnalysis.Recognition>(PXCMFaceAnalysis.Recognition.CUID);
                PXCMFaceAnalysis.Recognition.ProfileInfo dinfo;
                rec.QueryProfile(0, out dinfo);
                rec.SetProfile(ref dinfo);

                PXCMFaceAnalysis.Recognition.Model m1 = null;
                rec.CreateModel(fid, out m1);

                if(m1 != null)
                    listCompare.Add(m1);
            }

            if (listCompare.Count > 0) 
            {
                arrayCompare = new PXCMFaceAnalysis.Recognition.Model[listCompare.Count];

                for (int i = 0; i < listCompare.Count; i++)
                {
                    arrayCompare[i] = listCompare[i];
                }

                /* Retrieve face model data */
                PXCMFaceAnalysis.Recognition rec = face.DynamicCast<PXCMFaceAnalysis.Recognition>(PXCMFaceAnalysis.Recognition.CUID);
                PXCMFaceAnalysis.Recognition.ProfileInfo dinfo;
                rec.QueryProfile(0, out dinfo);
                rec.SetProfile(ref dinfo);

                PXCMFaceAnalysis.Recognition.Model m2 = null;
                pxcmStatus sts = rec.DeserializeModel(buffer, out m2);

                if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR) 
                {
                    uint index;
                    pxcmStatus sts2 = m2.Compare(arrayCompare, out index);
                    if (sts2 >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                        return (int)index;
                }
            }            
            return -1;
        }

        public WriteableBitmap PrintFaceLocation(FaceTrackingFramesSenseEventArgs e) 
        {
            PXCMImage img = e.ObjImage;
            PXCMImage.ImageData data;
            Bitmap bitmap = null;

            if (img.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, out data) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                bitmap = data.ToBitmap(img.info.width, img.info.height);
            }

            if (bitmap != null) 
            {
                foreach (var item in e.ArrayDetection)
                {
                    DrawLocation(item, ref bitmap);
                }
            }
            return BuildWriteableBitmap(bitmap);
        }

        public WriteableBitmap PrintFaceLandmark(FaceTrackingFramesSenseEventArgs e)
        {
            PXCMImage img = e.ObjImage;
            PXCMImage.ImageData data;
            Bitmap bitmap = null;

            if (img.AcquireAccess(PXCMImage.Access.ACCESS_READ, PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, out data) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                bitmap = data.ToBitmap(img.info.width, img.info.height);
            }

            if (bitmap != null)
            {
                foreach (var item in e.ArrayLandmark)
                {
                    DrawLandmark(item, ref bitmap);
                }
            }
            return BuildWriteableBitmap(bitmap);
        }

        private void DrawLocation(PXCMFaceAnalysis.Detection.Data data, ref Bitmap bitmap)
        {
            lock (this)
            {
                if (bitmap == null) return;
                Graphics g = Graphics.FromImage(bitmap);
                System.Drawing.Pen red = new System.Drawing.Pen(System.Drawing.Color.Red, 3.0f);
                System.Drawing.Brush brush = new SolidBrush(System.Drawing.Color.Red);

                System.Drawing.Point[] points4 = new System.Drawing.Point[]{
                        new System.Drawing.Point((int)data.rectangle.x,(int)data.rectangle.y),
                        new System.Drawing.Point((int)data.rectangle.x+(int)data.rectangle.w,(int)data.rectangle.y),
                        new System.Drawing.Point((int)data.rectangle.x+(int)data.rectangle.w,(int)data.rectangle.y+(int)data.rectangle.h),
                        new System.Drawing.Point((int)data.rectangle.x,(int)data.rectangle.y+(int)data.rectangle.h),
                        new System.Drawing.Point((int)data.rectangle.x,(int)data.rectangle.y)};
                g.DrawLines(red, points4);
                g.DrawString(data.fid.ToString(), new Font("Arial", 16), brush, (float)data.rectangle.x, (float)data.rectangle.y);
                
                brush.Dispose();
                red.Dispose();
                g.Dispose();
            }
        }

        private void DrawLandmark(PXCMFaceAnalysis.Landmark.LandmarkData[] data, ref Bitmap bitmap)
        {
            lock (this)
            {
                if (bitmap == null) return;
                Graphics g = Graphics.FromImage(bitmap);
                using (System.Drawing.Pen red = new System.Drawing.Pen(System.Drawing.Color.Red, 3.0f))
                {                    
                        foreach (PXCMFaceAnalysis.Landmark.LandmarkData ld in data)
                        {
                            if (ld.label > 0) g.DrawEllipse(red, ld.position.x - 3, ld.position.y - 3, 6, 6);
                        }                    
                }
                g.Dispose();
            }
        }

        private WriteableBitmap BuildWriteableBitmap(Bitmap image)
        {
            byte[] bytes = null;

            if (image == null)
                return null;
            //BitmapSource bmpSource = ConvertBitmapToBitmapSource(image);
            BitmapSource bmpSource = BitmapToBitmapSource(image);

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
    }
}
