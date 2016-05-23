using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class VoiceRecognitionSenseEventArgs : EventArgs
    {

        private PXCMVoiceRecognition.Recognition objVoiceRecognition;

        public VoiceRecognitionSenseEventArgs(PXCMVoiceRecognition.Recognition ObjVoiceRecognition) 
        {
            objVoiceRecognition = ObjVoiceRecognition;
        }

        public PXCMVoiceRecognition.Recognition ObjVoiceRecognition
        {
            get { return objVoiceRecognition; }
        }

    }
}
