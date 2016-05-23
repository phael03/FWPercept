using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows;

namespace FwPercept
{
    public abstract class Camera
    {

        private List<IModule> modules = new List<IModule>();
        private Boolean statusCamera = false;
        private Type typeCamera = null;

        public void InitCamera(){
            ConfigModules();
            StartCamera();            
        }

        public abstract void StartCamera();

        public abstract void RestartCamera();

        public abstract void ConfigModules();

        public abstract void CloseCamera();

        public abstract object GetSensor();        

        public Boolean EnableModule(IModule modulo) {
            if (modulo.GetTypeTargetCamera() == typeCamera)
            {
                foreach (var item in modules)
                {
                    if (item.GetIdModule() == modulo.GetIdModule())
                    {
                        MessageBox.Show("Module already included.", "Erro");
                        return false;
                    }
                }
                modules.Add(modulo);                
                return true;
            }
            else 
            {
                MessageBox.Show("Module incompatible with target Device.", "Erro");
                return false;
            }
        }

        public Type TypeCamera
        {
            get { return typeCamera; }
            set { typeCamera = value; }
        }

        protected List<IModule> Modules
        {
            get { return modules; }
            set { modules = value; }
        }

        public Boolean StatusCamera
        {
            get { return statusCamera; }
            set { statusCamera = value; }
        }
    }
}
