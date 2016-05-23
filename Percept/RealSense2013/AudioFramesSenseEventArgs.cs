using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    sealed public class AudioFramesSenseEventArgs : EventArgs
    {

        private PXCMAudio objAudio;

        public AudioFramesSenseEventArgs(PXCMAudio ObjAudio) 
        {
            objAudio = ObjAudio;
        }

        public PXCMAudio ObjAudio
        {
            get { return objAudio; }
        }

    }
}
