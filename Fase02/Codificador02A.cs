using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador02A : CodificadorBaseA
    {
        public override void encode(Object aux)
        {
            Clase02Metodos c = (Clase02Metodos)aux;
            Console.WriteLine(c.metodo1());
            Console.WriteLine(c.metodo2());
        }
    }
}
