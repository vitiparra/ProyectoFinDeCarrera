using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01A : CodificadorBaseA2
    {
        public override String encode(Object aux)
        {
            Clase01Basica c = (Clase01Basica)aux;
            return string.Format("{0},{1}", c.var1, c.var2);
        }
    }
}
