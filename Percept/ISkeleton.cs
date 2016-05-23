﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwPercept
{
    public interface ISkeleton
    {
        List<IJoint> GetJoints();
        int GetId();
    }
}
