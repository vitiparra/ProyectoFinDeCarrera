using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             * Variables que usaremos para medir el tiempo de ejecución
             * y calcular la duración de una ejecución
             */ 
            DateTime inicio, final;
            TimeSpan duracion;

            /*
             * 01. Clase básica
             */
            Clase01Basica c1 = new Clase01Basica();
            c1.var1 = 1;
            c1.var2 = "2";

            Console.WriteLine("Codificación básica. Clase con dos atributos");
            Codificador01A cod1A = new Codificador01A();
            inicio = DateTime.Now;
            cod1A.encode(c1);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Decodificación básica. Clase con dos atributos");
            Decodificador01A dec1A = new Decodificador01A();
            inicio = DateTime.Now;
            Object c1decodedAux = dec1A.decode(1, "2");
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            inicio = DateTime.Now;
            Clase01Basica c1decoded = (Clase01Basica)c1decodedAux;
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + duracion.TotalMilliseconds + " milisegundos");

            /*
            * 01. Struct básica
            */
            Struct01Basica s1 = new Struct01Basica();
            s1.var1 = 1;
            s1.var2 = "2";

            Console.WriteLine("Codificación de estructura con dos campos");
            Codificador01A cod1A_ = new Codificador01A();
            inicio = DateTime.Now;
            cod1A_.encode(c1);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Decodificación de una estructura con dos atributos");
            Decodificador01A dec1A_ = new Decodificador01A();
            inicio = DateTime.Now;
            Object c1decodedAux_ = dec1A_.decode(1, "2");
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            inicio = DateTime.Now;
            Clase01Basica c1decoded_ = (Clase01Basica)c1decodedAux_;
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + duracion.TotalMilliseconds + " milisegundos");

            Console.ReadLine();
        }
    }
}
