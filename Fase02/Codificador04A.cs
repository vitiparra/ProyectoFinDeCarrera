using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador04A : CodificadorBaseA
    {
        public override void encode(Object aux)
        {
            Clase04Struct c = (Clase04Struct)aux;

            Console.WriteLine(c.valor3.valor1);
            Console.WriteLine(c.valor3.valor2);
        }
    }
}
