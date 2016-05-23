using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;


namespace FwPercept
{
    public sealed class BackgroundRemovalFramesKinectEventArgs : EventArgs
    {

        private BackgroundRemovedColorFrameReadyEventArgs objFrame;

        public BackgroundRemovedColorFrameReadyEventArgs ObjFrame
        {
            get { return objFrame; }
        }

        public BackgroundRemovalFramesKinectEventArgs(BackgroundRemovedColorFrameReadyEventArgs ObjFrame) 
        {
            objFrame = ObjFrame;
        }

    }
}
