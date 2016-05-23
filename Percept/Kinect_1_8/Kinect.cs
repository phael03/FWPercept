using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using Microsoft.Kinect;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Synthesis;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;

namespace FwPercept
{
    public sealed class Kinect : Camera
    {

        private KinectSensor sensor = null;
        private SpeechRecognitionEngine speechEngine = null;
        private KinectSensorChooser sensorChooser = null;

        public event EventHandler<AllFramesKinectEventArgs> GetAllFrames;
        public event EventHandler<SpeechKinectEventArgs> GetSpeechFrames;
        public event EventHandler<BackgroundRemovalFramesKinectEventArgs> GetBackgroundRemovalFrames;        

        private Boolean EnableModuleSpeech = false;
        private string Language = "";
        private List<string> Comandos = null;

        private BackgroundRemovedColorStream backgroundRemovedColorStream = null;
        private Skeleton[] skeletonsBackground = null;
        private Boolean EnableModuleBackground = false;
        private ColorImageFormat imgColorFormatBackground;

        public Kinect()
            : base() 
        {
            if (KinectSensor.KinectSensors.Count > 0)
            {
                sensor = KinectSensor.KinectSensors[0];
            }
        }

        public override void StartCamera() {    
            if (sensor != null && !StatusCamera)
            {
                sensor.Start();
                StatusCamera = true;
                sensor.AllFramesReady += sensor_AllFramesReady;

                if (EnableModuleSpeech)
                    SpeechRecognitionModule(Language, Comandos);
            }
            else
            {
                MessageBox.Show("The device is already started, it's needed to call RestartCamera()", "Erro");
            }
        }

        public override void RestartCamera()
        {
            if (sensor != null && this.StatusCamera) 
            {
                CloseCamera();
                if (KinectSensor.KinectSensors.Count > 0)
                {
                    sensor = KinectSensor.KinectSensors[0];
                }
                InitCamera();
            }else
            {
                MessageBox.Show("The device is not started", "Erro");
            }
            
        }

        //Será chamado 30 vezes por segundo
        private void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            AllFramesKinectEventArgs args = new AllFramesKinectEventArgs(e);
            OnAllFrames(args);
            if (EnableModuleBackground) 
            {
                SensorAllFramesReady(e);
            }
        }

        private void OnAllFrames(AllFramesKinectEventArgs e)
        {
            EventHandler<AllFramesKinectEventArgs> handler = GetAllFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void ConfigModules() {
            if (sensor != null && !this.StatusCamera) 
            {
                foreach (IModule module in Modules)
                {
                    switch (module.GetNameModule())
                    {
                        case "ColorKinect":
                            sensor.ColorStream.Enable((ColorImageFormat)((ModuleColorKinect)module).ImgFormat);                            
                            break;
                        case "DepthKinect":
                            sensor.DepthStream.Enable((DepthImageFormat)((ModuleDepthKinect)module).ImgFormat);                                                        
                            break;
                        case "SkeletonKinect":
                            sensor.SkeletonStream.Enable();
                            break;
                        case "SpeechRecognitionKinect":
                            if (((ModuleSpeechRecognitionKinect)module).KindModule.Equals(ModuleSpeechRecognitionKinect.KindSpeechRecognitionModule.VoiceCommand)) 
                            {
                                EnableModuleSpeech = true;
                                Language = ((ModuleSpeechRecognitionKinect)module).Language;
                                Comandos = ((ModuleSpeechRecognitionKinect)module).GetComandos();
                            }                                
                            break;
                        case "BackgroundRemovalKinect":
                            imgColorFormatBackground = (ColorImageFormat)((ModuleBackgroundRemovalKinect)module).ImgColorFormat;

                            sensor.ColorStream.Enable(imgColorFormatBackground);
                            sensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                            sensor.SkeletonStream.Enable();

                            this.backgroundRemovedColorStream = new BackgroundRemovedColorStream(sensor);
                            this.backgroundRemovedColorStream.Enable(imgColorFormatBackground, DepthImageFormat.Resolution320x240Fps30);

                            // Allocate space to put the depth, color, and skeleton data we'll receive
                            if (null == this.skeletonsBackground)
                            {
                                this.skeletonsBackground = new Skeleton[sensor.SkeletonStream.FrameSkeletonArrayLength];
                            }

                            // Add an event handler to be called when the background removed color frame is ready, so that we can
                            // composite the image and output to the app
                            this.backgroundRemovedColorStream.BackgroundRemovedFrameReady += this.BackgroundRemovedFrameReadyHandler;

                            EnableModuleBackground = true;                              
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("The device is already started, it's needed to call RestartCamera()", "Erro");
            }
        }

        //############################################################### Speech Recognition
        private void SpeechRecognitionModule(string Language, List<string> Comandos)
        {
            RecognizerInfo ri = GetKinectRecognizer(Language);

            if (null != ri)
            {
                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                //Use this code to create grammar programmatically rather than from a grammar file.                 
                var directions = new Choices();

                foreach (var cmd in Comandos)
                {
                    directions.Add(new SemanticResultValue(cmd, cmd));    
                }

                var gb = new GrammarBuilder { Culture = ri.Culture };
                gb.Append(directions);

                var g = new Grammar(gb);

                speechEngine.LoadGrammar(g);

                speechEngine.SpeechRecognized += SpeechRecognized;

                // For long recognition sessions (a few hours or more), it may be beneficial to turn off adaptation of the acoustic model. 
                // This will prevent recognition accuracy from degrading over time.
                ////speechEngine.UpdateRecognizerSetting("AdaptationOn", 0);

                speechEngine.SetInputToAudioStream(sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private static RecognizerInfo GetKinectRecognizer(string Language)
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if (Language.Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }
            return null;
        }

        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            SpeechKinectEventArgs ev = new SpeechKinectEventArgs(e);
            OnSpeechFrames(ev);
        }

        private void OnSpeechFrames(SpeechKinectEventArgs e)
        {
            EventHandler<SpeechKinectEventArgs> handler = GetSpeechFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //############################################################### Fim Speech Recognition

        //############################################################### Background Removal

        private void SensorAllFramesReady(AllFramesReadyEventArgs e)
        {
            if (null == this.sensor)
            {
                return;
            }

            try
            {
                using (var depthFrame = e.OpenDepthImageFrame())
                {
                    if (null != depthFrame)
                    {
                        this.backgroundRemovedColorStream.ProcessDepth(depthFrame.GetRawPixelData(), depthFrame.Timestamp);
                    }
                }

                using (var colorFrame = e.OpenColorImageFrame())
                {
                    if (null != colorFrame)
                    {
                        this.backgroundRemovedColorStream.ProcessColor(colorFrame.GetRawPixelData(), colorFrame.Timestamp);
                    }
                }

                using (var skeletonFrame = e.OpenSkeletonFrame())
                {
                    if (null != skeletonFrame)
                    {
                        skeletonFrame.CopySkeletonDataTo(this.skeletonsBackground);
                        this.backgroundRemovedColorStream.ProcessSkeleton(this.skeletonsBackground, skeletonFrame.Timestamp);

                        if (skeletonsBackground != null && skeletonsBackground.Length > 0)
                        {
                            Skeleton myskeleton = (from s in skeletonsBackground
                                                   where s.TrackingState == SkeletonTrackingState.Tracked
                                                   select s).FirstOrDefault();
                            if (myskeleton != null)
                                this.backgroundRemovedColorStream.SetTrackedPlayer(myskeleton.TrackingId);
                        }
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // Ignore the exception. 
            }
        }

        private void BackgroundRemovedFrameReadyHandler(object sender, BackgroundRemovedColorFrameReadyEventArgs e)
        {
            BackgroundRemovalFramesKinectEventArgs ev = new BackgroundRemovalFramesKinectEventArgs(e);
            OnBackgroundRemovalFrames(ev);
        }

        private void OnBackgroundRemovalFrames(BackgroundRemovalFramesKinectEventArgs e)
        {
            EventHandler<BackgroundRemovalFramesKinectEventArgs> handler = GetBackgroundRemovalFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //############################################################### Fim Background Removal

        public override object GetSensor() 
        {
            return sensor;
        }

        public override void CloseCamera()
        {
            if (sensor != null && StatusCamera)
            {
                if (speechEngine != null)
                {
                    sensor.AudioSource.Stop();
                    speechEngine.Dispose();
                    speechEngine = null;
                    Language = "";
                    Comandos = null;
                    EnableModuleSpeech = false;
                }

                if (sensorChooser != null) 
                {
                    EnableModuleBackground = false;
                    skeletonsBackground = null;
                    backgroundRemovedColorStream.Dispose();
                    backgroundRemovedColorStream = null;
                    sensorChooser.Stop();
                    sensorChooser = null;
                }

                sensor.Stop();
                sensor.Dispose();
                sensor = null;
                StatusCamera = false;
            }
            else
            {
                MessageBox.Show("The device is not started", "Erro");
            }
        }

    }
}
