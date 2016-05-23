using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public sealed class SkeletonFramesSenseEventArgs : EventArgs
    {
        private PXCMGesture objSkeleton;

        public SkeletonFramesSenseEventArgs(PXCMGesture skeleton) 
        {
            objSkeleton = skeleton;
        }

        public PXCMGesture ObjSkeleton
        {
            get { return objSkeleton; }            
        }
    }
}
