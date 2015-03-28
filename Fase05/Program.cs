using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fase02;

namespace Fase05
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "";

            Fase02.Clase01Basica c1 = new Clase01Basica();
            c1.var1 = 2;
            c1.var2 = "Hola";

            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                serializador1.encode(c1, ref str);
/*
                c1 = new Clase01Basica();
                c1 = serializador1.decode(str, c1);
                Console.WriteLine(c1.var1);
                Console.WriteLine(c1.var2);
*/
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
            Console.ReadKey();
        }
    }
}
