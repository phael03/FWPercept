using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FwPercept
{
    public interface IModuleBackground
    {
        byte[] GetDataFromImageFrame(EventArgs e);
        WriteableBitmap BuildWriteableBitmap(EventArgs e);
    }
}
