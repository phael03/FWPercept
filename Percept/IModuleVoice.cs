using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public interface IModuleVoice
    {
        IRecognition GetCommandFromFrame(EventArgs e);
        List<string> GetComandos();
        string GetDescriptionLanguage();
    }
}
