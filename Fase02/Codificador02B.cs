using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador02B : CodificadorBaseB2
    {
        public override String encode(ref Object aux)
        {
            Clase02Metodos c = (Clase02Metodos)aux;
            return string.Format("{0},{1}", c.metodo1(), c.metodo2());
        }
    }
}
