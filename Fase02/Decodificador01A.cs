using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01A : DecodificadorBase
    {
//        public override Object decode(Stream aux)
        public override Object decode(String aux)
        {
            Clase01Basica c = new Clase01Basica();
            String str = aux.ToString();
            c.var1 = Convert.ToInt16(str);
            c.var2 = Convert.ToInt16(str);
            return c;
        }
    }
}
