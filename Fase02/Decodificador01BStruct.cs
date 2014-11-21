using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01BStruct : DecodificadorBaseB
    {
//        public override Object decode(Stream aux)
        public override void decode(ref Object c, int v1, string v2)
        {
            c = (Struct01Basica)c;
//            c.var1 = v1;
//            c.var2 = v2;
        }
    }
}
