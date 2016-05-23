using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public interface IModuleSkeleton
    {
        ISkeleton GetDataSkeleton(EventArgs e);
        IGestus GetDataGesture(EventArgs e);
    }
}
