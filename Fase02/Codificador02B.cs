using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador02B
    {
        public String encode(ref Clase02Metodos c)
        {
            return string.Format("{0},{1}", c.metodo1(), c.metodo2());
        }
    }
}
