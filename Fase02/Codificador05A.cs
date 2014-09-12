using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador05A : CodificadorBase
    {
        public override void encode(Object aux)
        {
            Clase05Clase c = (Clase05Clase)aux;

            Console.WriteLine(c.var3.var1);
            Console.WriteLine(c.var3.var2);
        }
    }
}
