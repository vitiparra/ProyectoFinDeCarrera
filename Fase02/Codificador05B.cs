using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador05B
    {
        public String encode(ref Clase05Clase c)
        {
            return string.Format("{0},{1}", c.var3.var1, c.var3.var2);
        }
    }
}
