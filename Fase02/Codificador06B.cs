using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador06B : CodificadorBaseB
    {
        public override void encode(ref Object aux)
        {
            Clase06ClaseDerivada c = (Clase06ClaseDerivada)aux;

            Console.WriteLine(c.var1);
            Console.WriteLine(c.var2);
            Console.WriteLine(c.var3);
        }
    }
}
