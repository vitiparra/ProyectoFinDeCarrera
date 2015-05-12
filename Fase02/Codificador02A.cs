using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador02A : CodificadorBaseA2
    {
        public override String encode(Object aux)
        {
            Clase02ArrayNormal c = (Clase02ArrayNormal)aux;
            return string.Format("{0},{1}", c.var1.Length, c.var2.Length);
        }
    }
}
