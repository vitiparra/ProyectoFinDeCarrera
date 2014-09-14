using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador05A : DecodificadorBase
    {
//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Clase05Clase c = new Clase05Clase();
            c.var3.var1 = v1;
            c.var3.var2 = v2;

            return c;
        }
    }
}
