using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador01A : CodificadorBase
    {
        public override void put(Object aux)
        {
            Clase01Basica c = (Clase01Basica)aux;
            Console.WriteLine(c.var1);
            Console.WriteLine(c.var2);
        }
    }
}
