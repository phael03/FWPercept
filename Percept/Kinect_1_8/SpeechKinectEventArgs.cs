using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Speech.Recognition;

namespace FwPercept
{
    public sealed class SpeechKinectEventArgs : EventArgs
    {

        private SpeechRecognizedEventArgs objSpeech;

        public SpeechKinectEventArgs(SpeechRecognizedEventArgs ObjSpeech) 
        {
            objSpeech = ObjSpeech;
        }

        public SpeechRecognizedEventArgs ObjSpeech
        {
            get { return objSpeech; }
        }
    }
}
