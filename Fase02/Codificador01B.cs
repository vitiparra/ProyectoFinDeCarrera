using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01B : CodificadorBaseB2
    {
        public override String encode(ref Object aux)
        {
            Clase01Basica c = (Clase01Basica)aux;
            return string.Format("{0},{1}", c.var1, c.var2);
        }
    }
}
