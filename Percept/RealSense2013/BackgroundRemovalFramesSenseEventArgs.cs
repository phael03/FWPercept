using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class BackgroundRemovalFramesSenseEventArgs : EventArgs
    {

        private PXCMImage objImage;
        private PXCMSegmentation objSegmentation;

        public BackgroundRemovalFramesSenseEventArgs(PXCMImage ObjImage, PXCMSegmentation ObjSegmentation) 
        {
            objImage = ObjImage;
            objSegmentation = ObjSegmentation;
        }
        
        public PXCMImage ObjImage
        {
            get { return objImage; }
        }

        public PXCMSegmentation ObjSegmentation
        {
            get { return objSegmentation; }
        }
    }
}
