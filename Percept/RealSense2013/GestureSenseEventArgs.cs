using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    sealed public class GestureSenseEventArgs : EventArgs
    {

        private PXCMGesture.Gesture objGesture;

        public GestureSenseEventArgs(PXCMGesture.Gesture gesture) 
        {
            objGesture = gesture;
        }

        public PXCMGesture.Gesture ObjGesture
        {
            get { return objGesture; }            
        }

    }
}
