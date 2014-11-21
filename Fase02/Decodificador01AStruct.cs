using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01AStruct : DecodificadorBaseA
    {
//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Struct01Basica s = new Struct01Basica();
            s.var1 = v1;
            s.var2 = v2;
            return s;
        }
    }
}
