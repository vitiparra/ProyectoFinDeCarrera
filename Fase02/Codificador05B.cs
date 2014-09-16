using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador05B : CodificadorBaseB
    {
        public override void encode(ref Object aux)
        {
            Clase05Clase c = (Clase05Clase)aux;

            Console.WriteLine(c.var3.var1);
            Console.WriteLine(c.var3.var2);
        }
    }
}
