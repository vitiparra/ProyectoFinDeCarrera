using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador06B
    {
        public String encode(ref Clase06ClaseDerivada c)
        {
            return string.Format("{0},{1},{2}", c.var1, c.var2, c.var3);
        }
    }
}
