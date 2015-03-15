using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01AStruct : CodificadorBaseA
    {
        public override void encode(Object aux)
        {
            Struct01Basica s = (Struct01Basica)aux;
//            return string.Format("{0}, {1}", s.var1, s.var2);
        }
    }
}
