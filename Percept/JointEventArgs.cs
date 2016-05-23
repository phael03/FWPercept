using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    sealed public class JointEventArgs : EventArgs
    {
        private float currentPositionX;
        private float currentPositionY;
        private float currentPositionZ;

        private float difPositionX;
        private float difPositionY;
        private float difPositionZ;

        private Boolean orientationX;
        private Boolean orientationY;
        private Boolean orientationZ;

        public JointEventArgs() 
        {
            CurrentPositionX = 0;
            CurrentPositionY = 0;
            CurrentPositionZ = 0;
            DifPositionX = 0;
            DifPositionY = 0;
            DifPositionZ = 0;
            OrientationX = false;
            OrientationY = false;
            OrientationZ = false;
        }

        public float CurrentPositionX
        {
            get { return currentPositionX; }
            set { currentPositionX = value; }
        }
        public float CurrentPositionY
        {
            get { return currentPositionY; }
            set { currentPositionY = value; }
        }

        public float CurrentPositionZ
        {
            get { return currentPositionZ; }
            set { currentPositionZ = value; }
        }

        public float DifPositionX
        {
            get { return difPositionX; }
            set { difPositionX = value; }
        }

        public float DifPositionY
        {
            get { return difPositionY; }
            set { difPositionY = value; }
        }

        public float DifPositionZ
        {
            get { return difPositionZ; }
            set { difPositionZ = value; }
        }

        public Boolean OrientationX
        {
            get { return orientationX; }
            set { orientationX = value; }
        }

        public Boolean OrientationY
        {
            get { return orientationY; }
            set { orientationY = value; }
        }

        public Boolean OrientationZ
        {
            get { return orientationZ; }
            set { orientationZ = value; }
        }
    }    
}
