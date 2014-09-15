using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador04A : DecodificadorBaseA
    {
//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Clase04Struct c = new Clase04Struct();

            c.valor3.valor1 = v1;
            c.valor3.valor2 = v2;

            return c;
        }
    }
}
