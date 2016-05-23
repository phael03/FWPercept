using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace FwPercept
{
    public sealed class Percept
    {

        private static Percept build = null;
        private Camera camera = null;

        private Percept() { }

        public static Percept GetInstance()
        {
            if(build==null){
                build = new Percept();
            }
            return build;
        }

        public Camera GetCamera(Type typeClass) {
            camera = (Camera)Activator.CreateInstance(typeClass);
            typeClass.GetProperty("TypeCamera").SetValue(camera, typeClass, null);            
            return camera;
        }

    }
}
