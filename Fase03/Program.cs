using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fase02;

namespace Fase03
{
    class Program
    {
        static void Main(string[] args)
        {
            Generador g;
            /*
                        Fase02.Clase01Basica c1 = new Clase01Basica();
                        c1.var1 = 2;
                        c1.var2 = "Hola";
                        g = new Generador(c1);

                        Fase02.Clase02Metodos c2 = new Clase02Metodos();
                        c2.metodo1();
                        c2.metodo2();
                        g = new Generador(c2);
            
                        Fase02.Clase03Array c3 = new Clase03Array();
                        g = new Generador(c3);
            
                        Fase02.Clase04Struct c4 = new Clase04Struct();
                        g = new Generador(c4);
            */
            Fase02.Struct01Basica c5 = new Fase02.Struct01Basica();
            g = new Generador(c5);
            /*
                        Fase02.Clase07ClaseConTodo c7 = new Fase02.Clase07ClaseConTodo();
                        g = new Generador(c7);
            */
            Console.ReadKey();
        }
    }
}
