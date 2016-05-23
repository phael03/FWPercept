using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class EmotionRecognitionSenseEventArgs : EventArgs
    {

        private PXCMEmotion objEmotion;

        public EmotionRecognitionSenseEventArgs(PXCMEmotion ObjEmotion) 
        {
            objEmotion = ObjEmotion;
        }

        public PXCMEmotion ObjEmotion
        {
            get { return objEmotion; }
        }
    }
}
