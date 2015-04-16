using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador04A
    {
        public String encode(Clase04Struct c)
        {
            return string.Format("{0},{1}", c.valor3.valor1, c.valor3.valor2);
        }
    }
}
