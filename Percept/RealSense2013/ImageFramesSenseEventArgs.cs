using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FwPercept
{
    sealed public class ImageFramesSenseEventArgs : EventArgs
    {
        private PXCMImage objImage;

        public ImageFramesSenseEventArgs(PXCMImage ObjImage) 
        {
            objImage = ObjImage;
        }
        
        public PXCMImage ObjImage
        {
            get { return objImage; }
        }
    }
}
