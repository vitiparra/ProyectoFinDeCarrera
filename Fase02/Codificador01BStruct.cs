﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01BStruct : CodificadorBaseB2
    {
        public override String encode(ref Object aux)
        {
            Struct01Basica s = (Struct01Basica)aux;
            return string.Format("{0}, {1}", s.var1, s.var2);
        }
    }
}
