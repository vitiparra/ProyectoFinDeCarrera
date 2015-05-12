using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador02A : DecodificadorBaseA2
    {
//        public override Object decode(Stream aux)
        public override Object decode(String s)
        {
            Clase02ArrayNormal c = new Clase02ArrayNormal();
            return c;
        }
    }
}
