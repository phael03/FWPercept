using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace FwPercept
{
    public interface IModuleAudio
    {
        byte[] GetDataFromAudioFrame(EventArgs e);
    }
}
