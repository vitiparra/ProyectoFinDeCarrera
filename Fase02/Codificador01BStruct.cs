using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01BStruct : CodificadorBaseB
    {
        public override void encode(ref Object aux)
        {
            Struct01Basica s = (Struct01Basica)aux;
            Console.WriteLine(s.var1);
            Console.WriteLine(s.var2);
        }
    }
}
