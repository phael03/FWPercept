using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace FwPercept
{
    sealed public class AllFramesKinectEventArgs : EventArgs
    {

        private AllFramesReadyEventArgs objFrame;

        public AllFramesReadyEventArgs ObjFrame
        {
            get { return objFrame; }
        }

        public AllFramesKinectEventArgs(AllFramesReadyEventArgs ObjFrame) 
        {
            objFrame = ObjFrame;
        }

    }
}
