using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace FwPercept
{
    public class MyPipeline : UtilMPipeline
    {

        public event EventHandler<ImageFramesSenseEventArgs> GetImageFrames;
        public event EventHandler<AudioFramesSenseEventArgs> GetAudioFrames;
        public event EventHandler<GestureSenseEventArgs> GetGestureFrames;
        public event EventHandler<VoiceRecognitionSenseEventArgs> GetVoiceRecognitionFrames;        

        private PXCMSession mySession;

        private Boolean gestureActive = false;
        private Boolean faceActive = false;
        private Boolean emotionActive = false;
        private Boolean backgroundActive = false;
        private Boolean voiceActive = false;

        private uint voiceProfileIndex;

        public MyPipeline(PXCMSession session)
            : base(session)
        {
            mySession = session;
        }

        public override void OnImage(PXCMImage image)
        {
            base.OnImage(image);

            ImageFramesSenseEventArgs e = new ImageFramesSenseEventArgs(image);
            OnImageFrames(e);
        }

        private void OnImageFrames(ImageFramesSenseEventArgs e)
        {
            EventHandler<ImageFramesSenseEventArgs> handler = GetImageFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void OnAudio(PXCMAudio audio)
        {
            base.OnAudio(audio);

            AudioFramesSenseEventArgs e = new AudioFramesSenseEventArgs(audio);
            OnAudioFrames(e);
        }

        private void OnAudioFrames(AudioFramesSenseEventArgs e)
        {
            EventHandler<AudioFramesSenseEventArgs> handler = GetAudioFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public override void OnGesture(ref PXCMGesture.Gesture gesture)
        {
            base.OnGesture(ref gesture);
            GestureSenseEventArgs gestureEvent = new GestureSenseEventArgs(gesture);
            OnGestureFrames(gestureEvent);
        }

        private void OnGestureFrames(GestureSenseEventArgs e)
        {
            EventHandler<GestureSenseEventArgs> handler = GetGestureFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }
       
        public override void OnRecognized(ref PXCMVoiceRecognition.Recognition data)
        {
            VoiceRecognitionSenseEventArgs e = new VoiceRecognitionSenseEventArgs(data);
            OnVoiceRecognitionFrames(e);
        }

        private void OnVoiceRecognitionFrames(VoiceRecognitionSenseEventArgs e)
        {
            EventHandler<VoiceRecognitionSenseEventArgs> handler = GetVoiceRecognitionFrames;
            if (handler != null)
            {
                handler(this, e);
            }
        }
       
        public void DisposeElements()
        {
            
        }

        public uint VoiceProfileIndex
        {
            set { voiceProfileIndex = value; }
        }

        public Boolean GestureActive
        {
            set { gestureActive = value; }
            get { return gestureActive; }
        }

        public Boolean FaceActive
        {
            set { faceActive = value; }
            get { return faceActive; }
        }

        public Boolean EmotionActive
        {
            set { emotionActive = value; }
            get { return emotionActive; }
        }

        public Boolean BackgroundActive
        {
            set { backgroundActive = value; }
            get { return backgroundActive; }
        }

        public Boolean VoiceActive
        {
            get { return voiceActive; }
            set { voiceActive = value; }
        }

    }
}
