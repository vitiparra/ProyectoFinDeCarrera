﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public abstract class DecodificadorBaseB
    {
//        public abstract Object decode(Stream aux);
        public abstract void decode(ref Object c, int v1, string v2);
    }
}
