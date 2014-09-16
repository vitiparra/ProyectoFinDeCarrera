using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador03B : CodificadorBaseB
    {
        public override void encode(ref Object aux)
        {
            Clase03Array c = (Clase03Array)aux;

            Console.WriteLine("Array de enteros:");
            foreach (int elemento in c.var1)
            {
                Console.WriteLine(elemento);

            }

            Console.WriteLine("Array de cadenas:");
            foreach (string elemento in c.var2)
            {
                Console.WriteLine(elemento);

            }
        }
    }
}
