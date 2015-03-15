using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador04A : CodificadorBaseA2
    {
        public override String encode(Object aux)
        {
            Clase04Struct c = (Clase04Struct)aux;

            return string.Format("{0},{1}", c.valor3.valor1, c.valor3.valor2);
        }
    }
}
