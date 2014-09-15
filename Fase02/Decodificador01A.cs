using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01A : DecodificadorBaseA
    {
//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Clase01Basica c = new Clase01Basica();
            c.var1 = v1;
            c.var2 = v2;
            return c;
        }
    }
}
