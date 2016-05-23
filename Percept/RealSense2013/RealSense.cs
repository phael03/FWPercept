using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;

namespace FwPercept
{
    public class RealSense : Camera
    {
        public event EventHandler<ImageFramesSenseEventArgs> GetImageFrames;
        public event EventHandler<AudioFramesSenseEventArgs> GetAudioFrames;
        public event EventHandler<SkeletonFramesSenseEventArgs> GetSkeletonFrames;
        public event EventHandler<GestureSenseEventArgs> GetGestureFrames;
        public event EventHandler<VoiceRecognitionSenseEventArgs> GetVoiceRecognitionFrames;
        public event EventHandler<FaceTrackingFramesSenseEventArgs> GetFaceTrackingFrames;
        public event EventHandler<EmotionRecognitionSenseEventArgs> GetEmotionRecognitionFrames;
        public event EventHandler<BackgroundRemovalFramesSenseEventArgs> GetBackgroundRemovalFrames;

        private PXCMSession sensor = null;
        private MyPipeline myPipeline = null;
        private Thread t = null;

        private Boolean stopQuery = false;

        private PXCMSegmentation.BlendMode backgroundMode;
        private string backgroundSourceFile;

        private PXCMVoiceRecognition.ProfileInfo.Language voiceLanguage;
        private ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule voiceModule;
        private string[] voiceCommands;


        public RealSense()
            : base()
        {
            pxcmStatus status = PXCMSession.CreateInstance(out sensor);

            if (status == pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                myPipeline = new MyPipeline(sensor);
            }
        }

        private void StartLoopFrames()
        {
            myPipeline.Init();

            if (myPipeline.BackgroundActive)
                FinishConfigBackgroundRemoval();

            if (myPipeline.GestureActive)
                FinishConfigGesture();

            if (myPipeline.EmotionActive)
                FinishConfigEmotion();

            if (myPipeline.FaceActive)
                FinishConfigFace();

            for (;;)
            {
                // some device hot-plug code omitted.
                if (!myPipeline.AcquireFrame(true)) break;

                if (myPipeline.BackgroundActive)
                    CaptureFrameBackgroundRemoval();

                if (myPipeline.GestureActive)
                    CaptureFrameGesture();

                if (myPipeline.EmotionActive)
                    CaptureFrameEmotion();

                if (myPipeline.FaceActive)
                    CaptureFrameFace();

                if (!myPipeline.ReleaseFrame()) break;
            }
            myPipeline.Close();
        }

        public override void StartCamera()
        {
            if (sensor != null && !StatusCamera)
            {
                //sensor.LoopFrames();
                StatusCamera = true;

                //Habilitando Eventos
                myPipeline.GetImageFrames += myPipeline_GetImageFrames;
                myPipeline.GetAudioFrames += myPipeline_GetAudioFrames;
                myPipeline.GetGestureFrames += myPipeline_GetGestureFrames;
                myPipeline.GetVoiceRecognitionFrames += myPipeline_GetVoiceRecognitionFrames;

                t = new Thread(StartLoopFrames);
                t.Start();
            }
            else
            {
                MessageBox.Show("The device is already started, it's needed to call RestartCamera()", "Erro");
            }
        }

        private void CaptureFrameFace()
        {
            PXCMFaceAnalysis ft = myPipeline.QueryFace();
            List<PXCMFaceAnalysis.Detection.Data> arrayDetection = new List<PXCMFaceAnalysis.Detection.Data>();
            List<PXCMFaceAnalysis.Landmark.LandmarkData[]> arrayLandmark = new List<PXCMFaceAnalysis.Landmark.LandmarkData[]>();

            for (uint i = 0; ; i++)
            {
                int fid; ulong ts;
                if (ft.QueryFace(i, out fid, out ts) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                /* Retrieve face location data */
                PXCMFaceAnalysis.Detection ftd = ft.DynamicCast<PXCMFaceAnalysis.Detection>(PXCMFaceAnalysis.Detection.CUID);
                PXCMFaceAnalysis.Detection.Data ddata;
                if (ftd.QueryData(fid, out ddata) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                    arrayDetection.Add(ddata);

                /* Retrieve face landmark data */
                PXCMFaceAnalysis.Landmark ftl = ft.DynamicCast<PXCMFaceAnalysis.Landmark>(PXCMFaceAnalysis.Landmark.CUID);
                PXCMFaceAnalysis.Landmark.ProfileInfo pinfo;
                ftl.QueryProfile(out pinfo);

                PXCMFaceAnalysis.Landmark.LandmarkData[] ldata = new PXCMFaceAnalysis.Landmark.LandmarkData[(int)(PXCMFaceAnalysis.Landmark.Label.LABEL_7POINTS & PXCMFaceAnalysis.Landmark.Label.LABEL_SIZE_MASK)];
                if (ftl.QueryLandmarkData(fid, PXCMFaceAnalysis.Landmark.Label.LABEL_7POINTS, ldata) >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                    arrayLandmark.Add(ldata);
            }

            PXCMImage img = myPipeline.QueryImage(PXCMImage.ImageType.IMAGE_TYPE_COLOR);
            //invocar evento
            FaceTrackingFramesSenseEventArgs e = new FaceTrackingFramesSenseEventArgs(arrayDetection, arrayLandmark, img);
            OnFaceFrames(e);
        }

        private void CaptureFrameEmotion() 
        {
            PXCMEmotion emotion = myPipeline.QueryEmotion();
            EmotionRecognitionSenseEventArgs e = new EmotionRecognitionSenseEventArgs(emotion);
            OnEmotionFrames(e);
        }

        private void CaptureFrameGesture()
        {
            PXCMGesture gesture = myPipeline.QueryGesture();            
            SkeletonFramesSenseEventArgs e = new SkeletonFramesSenseEventArgs(gesture);
            OnSkeletonFrames(e);
        }

        private void CaptureFrameBackgroundRemoval()
        {
            PXCMSegmentation background = myPipeline.QuerySegmentation();
            PXCMImage image = myPipeline.QuerySegmentationBlendedImage();
            BackgroundRemovalFramesSenseEventArgs e = new BackgroundRemovalFramesSenseEventArgs(image, background);
            OnBackgroundFrames(e);
        }

        private void FinishConfigFace()
        {
            PXCMFaceAnalysis.ProfileInfo pinfo;
            myPipeline.QueryFace().QueryProfile(out pinfo);

            pxcmStatus status = myPipeline.QueryFace().SetProfile(ref pinfo);
            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return;
            }
        }

        private void FinishConfigEmotion()
        {
            PXCMEmotion.ProfileInfo pinfo;
            myPipeline.QueryEmotion().QueryProfile(out pinfo);

            pxcmStatus status = myPipeline.QueryEmotion().SetProfile(ref pinfo);
            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return;
            }
        }

        private void FinishConfigGesture()
        {
            PXCMGesture.ProfileInfo pinfo;
            myPipeline.QueryGesture().QueryProfile(out pinfo);

            pxcmStatus status = myPipeline.QueryGesture().SetProfile(ref pinfo);
            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return;
            }            
        }

        private void FinishConfigBackgroundRemoval()
        {
            PXCMSegmentation.ProfileInfo pinfo;
            myPipeline.QuerySegmentation().QueryProfile(out pinfo);
            pinfo.blendMode = backgroundMode;
            
            pxcmStatus status = myPipeline.QuerySegmentation().SetProfile(ref pinfo);
            if (status < pxcmStatus.PXCM_STATUS_NO_ERROR)
            {
                return;
            }
            myPipeline.SetSegmentationBlendMode(backgroundMode);

            if (backgroundMode == ((PXCMSegmentation.BlendMode)ModuleBackgroundRemovalRealSense.BackgroundModeEnum.BgImage))
            {
                PXCMImage bgImage = null;

                PXCMAccelerator accelerator;
                pxcmStatus msts = this.sensor.CreateAccelerator(out accelerator);
                if (msts < pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    //Lançar Exceção
                }
                ReadImageFromFile(accelerator, out bgImage, backgroundSourceFile);
                myPipeline.SetSegmentationBGImage(bgImage);
            }
        }

        void myPipeline_GetGestureFrames(object sender, GestureSenseEventArgs e)
        {
            OnGestureFrames(e);
        }

        void myPipeline_GetAudioFrames(object sender, AudioFramesSenseEventArgs e)
        {
            OnAudioFrames(e);
        }

        void myPipeline_GetImageFrames(object sender, ImageFramesSenseEventArgs e)
        {
            ImageFramesSenseEventArgs ev = new ImageFramesSenseEventArgs(e.ObjImage);
            OnImageFrames(ev);
        }

        void myPipeline_GetVoiceRecognitionFrames(object sender, VoiceRecognitionSenseEventArgs e)
        {
            OnVoiceRecognitionFrames(e);
        }

        private void OnGestureFrames(GestureSenseEventArgs e)
        {
            EventHandler<GestureSenseEventArgs> handler = GetGestureFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnAudioFrames(AudioFramesSenseEventArgs e)
        {
            EventHandler<AudioFramesSenseEventArgs> handler = GetAudioFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnImageFrames(ImageFramesSenseEventArgs e)
        {
            EventHandler<ImageFramesSenseEventArgs> handler = GetImageFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnVoiceRecognitionFrames(VoiceRecognitionSenseEventArgs e)
        {
            EventHandler<VoiceRecognitionSenseEventArgs> handler = GetVoiceRecognitionFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnSkeletonFrames(SkeletonFramesSenseEventArgs e)
        {
            EventHandler<SkeletonFramesSenseEventArgs> handler = GetSkeletonFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnFaceFrames(FaceTrackingFramesSenseEventArgs e)
        {
            EventHandler<FaceTrackingFramesSenseEventArgs> handler = GetFaceTrackingFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnEmotionFrames(EmotionRecognitionSenseEventArgs e)
        {
            EventHandler<EmotionRecognitionSenseEventArgs> handler = GetEmotionRecognitionFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnBackgroundFrames(BackgroundRemovalFramesSenseEventArgs e)
        {
            EventHandler<BackgroundRemovalFramesSenseEventArgs> handler = GetBackgroundRemovalFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void RestartCamera()
        {
            if (sensor != null && this.StatusCamera)
            {
                CloseCamera();

                pxcmStatus status = PXCMSession.CreateInstance(out sensor);
                if (status != pxcmStatus.PXCM_STATUS_NO_ERROR)
                    myPipeline = new MyPipeline(sensor);

                InitCamera();
            }
            else
            {
                MessageBox.Show("The device is not started", "Erro");
            }
        }

        public override void ConfigModules()
        {
            if (sensor != null && !this.StatusCamera)
            {
                foreach (IModule module in Modules)
                {
                    switch (module.GetNameModule())
                    {
                        case "ImageRealSense":
                            myPipeline.EnableImage((PXCMImage.ColorFormat)((ModuleImageRealSense)module).ImgFormat);
                            break;
                        case "AudioRealSense":
                            myPipeline.EnableAudio((PXCMAudio.AudioFormat)((ModuleAudioRealSense)module).AudioFormat);
                            break;
                        case "GestureRealSense":
                            PXCMSession.ImplDesc descGest = new PXCMSession.ImplDesc();
                            descGest.cuids[0] = PXCMGesture.CUID;

                            for (uint i = 0; ; i++)
                            {
                                if (stopQuery)
                                {
                                    stopQuery = false;
                                    break;
                                }
                                PXCMSession.ImplDesc desc1;
                                if (sensor.QueryImpl(ref descGest, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                for (uint j = 0; ; j++)
                                {                                    
                                    myPipeline.EnableGesture(desc1.friendlyName.get());
                                    myPipeline.GestureActive = true;
                                    stopQuery = true;
                                    break;
                                }
                            }
                            break;
                        case "VoiceRegonitionRealSense":
                            voiceModule = ((ModuleVoiceRecognitionRealSense)module).KindModule;
                            if (voiceModule != ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule.VoiceSynthesis)
                            {
                                PXCMSession.ImplDesc descRec = new PXCMSession.ImplDesc();
                                descRec.cuids[0] = PXCMVoiceRecognition.CUID;

                                for (uint i = 0; ; i++)
                                {
                                    if (stopQuery)
                                    {
                                        stopQuery = false;
                                        break;
                                    }
                                    PXCMSession.ImplDesc desc1;
                                    if (sensor.QueryImpl(ref descRec, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                    PXCMVoiceRecognition vrec;
                                    if (sensor.CreateImpl<PXCMVoiceRecognition>(ref desc1, PXCMVoiceRecognition.CUID, out vrec) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                    for (uint j = 0; ; j++)
                                    {
                                        PXCMVoiceRecognition.ProfileInfo pinfo;
                                        if (vrec.QueryProfile(j, out pinfo) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                        voiceLanguage = ((PXCMVoiceRecognition.ProfileInfo.Language)((ModuleVoiceRecognitionRealSense)module).Language);

                                        if (voiceLanguage == pinfo.language)
                                        {
                                            if (voiceModule == ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule.VoiceCommand)
                                            {
                                                voiceCommands = new string[((ModuleVoiceRecognitionRealSense)module).GetComandos().Count];
                                                for (int x = 0; x < ((ModuleVoiceRecognitionRealSense)module).GetComandos().Count; x++)
                                                {
                                                    voiceCommands[x] = ((ModuleVoiceRecognitionRealSense)module).GetComandos()[x];
                                                }
                                                myPipeline.EnableVoiceRecognition(desc1.friendlyName.get());
                                                myPipeline.VoiceProfileIndex = j;
                                                myPipeline.SetVoiceCommands(voiceCommands);
                                                stopQuery = true;
                                                break;
                                            }
                                            else if (voiceModule == ModuleVoiceRecognitionRealSense.KindVoiceRecognitionModule.VoiceDictation)
                                            {
                                                myPipeline.EnableVoiceRecognition(desc1.friendlyName.get());
                                                myPipeline.VoiceProfileIndex = j;
                                                myPipeline.SetVoiceDictation();
                                                stopQuery = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case "FaceTrackingRealSense":
                            PXCMSession.ImplDesc descFace = new PXCMSession.ImplDesc();
                            descFace.cuids[0] = PXCMFaceAnalysis.CUID;

                            for (uint i = 0; ; i++)
                            {
                                if (stopQuery)
                                {
                                    stopQuery = false;
                                    break;
                                }
                                PXCMSession.ImplDesc desc1;
                                if (sensor.QueryImpl(ref descFace, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;
                                
                                for (uint j = 0; ; j++)
                                {                                    
                                    myPipeline.EnableFaceLandmark(desc1.friendlyName.get());
                                    myPipeline.EnableFaceLocation(desc1.friendlyName.get());
                                    myPipeline.FaceActive = true;
                                    stopQuery = true;
                                    break;
                                }
                            }
                            break;
                        case "EmotionRealSense":
                            PXCMSession.ImplDesc descEmot = new PXCMSession.ImplDesc();
                            descEmot.cuids[0] = PXCMEmotion.CUID;

                            for (uint i = 0; ; i++)
                            {
                                if (stopQuery)
                                {
                                    stopQuery = false;
                                    break;
                                }
                                PXCMSession.ImplDesc desc1;
                                if (sensor.QueryImpl(ref descEmot, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                for (uint j = 0; ; j++)
                                {
                                    myPipeline.EnableEmotion(desc1.friendlyName.get());
                                    myPipeline.EmotionActive = true;
                                    stopQuery = true;
                                    break;
                                }
                            }
                            break;
                        case "BackgroundRemovalRealSense":
                            PXCMSession.ImplDesc descBack = new PXCMSession.ImplDesc();
                            descBack.cuids[0] = PXCMSegmentation.CUID;

                            for (uint i = 0; ; i++)
                            {
                                if (stopQuery)
                                {
                                    stopQuery = false;
                                    break;
                                }
                                PXCMSession.ImplDesc desc1;
                                if (sensor.QueryImpl(ref descBack, i, out desc1) < pxcmStatus.PXCM_STATUS_NO_ERROR) break;

                                for (uint j = 0; ; j++)
                                {
                                    myPipeline.EnableSegmentation(desc1.friendlyName.get());

                                    backgroundMode = (PXCMSegmentation.BlendMode)((ModuleBackgroundRemovalRealSense)module).BackgroundMode;
                                    backgroundSourceFile = ((ModuleBackgroundRemovalRealSense)module).ImgSourceFile;

                                    myPipeline.BackgroundActive = true;
                                    stopQuery = true;
                                    break;
                                }
                            }
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("The device is already started, it's needed to call RestartCamera()", "Erro");
            }
        }

        private bool ReadImageFromFile(PXCMAccelerator accel, out PXCMImage image, string bgImageFile)
        {
            // Read bitmap into the memory
            Bitmap bitmap = (Bitmap)Image.FromFile(bgImageFile);

            PXCMImage.ImageInfo iinfo = new PXCMImage.ImageInfo();
            iinfo.width = (uint)bitmap.Width;
            iinfo.height = (uint)bitmap.Height;
            iinfo.format = PXCMImage.ColorFormat.COLOR_FORMAT_RGB32;

            // Create the image
            accel.CreateImage(ref iinfo, out image);

            // Copy the data
            PXCMImage.ImageData idata;
            pxcmStatus sts = image.AcquireAccess(PXCMImage.Access.ACCESS_WRITE, PXCMImage.ColorFormat.COLOR_FORMAT_RGB32, out idata);
            if (sts != pxcmStatus.PXCM_STATUS_NO_ERROR) return false;

            BitmapData bdata = new BitmapData();
            bdata.Scan0 = idata.buffer.planes[0];
            bdata.Stride = idata.buffer.pitches[0];
            bdata.PixelFormat = System.Drawing.Imaging.PixelFormat.Format32bppRgb;
            bdata.Width = bitmap.Width;
            bdata.Height = bitmap.Height;

            BitmapData bdata2 = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                 ImageLockMode.ReadOnly | ImageLockMode.UserInputBuffer, System.Drawing.Imaging.PixelFormat.Format32bppRgb, bdata);
            image.ReleaseAccess(ref idata);
            bitmap.UnlockBits(bdata);

            return true;
        }

        public override void CloseCamera()
        {
            if (sensor != null && StatusCamera)
            {
                try
                {
                    t.Abort();
                }
                catch (Exception)
                {
                    Console.WriteLine("stoping thread");
                }
                myPipeline.DisposeElements();
                myPipeline.ReleaseFrame();
                myPipeline.Dispose();
                myPipeline = null;
                sensor.Dispose();
                StatusCamera = false;
            }
            else
            {
                MessageBox.Show("The device is not started", "Erro");
            }
        }

        public override object GetSensor()
        {
            Dictionary<int, object> lista = new Dictionary<int, object>();
            lista.Add(1,sensor);
            lista.Add(2,myPipeline);
            return lista;
        }

    }
}
