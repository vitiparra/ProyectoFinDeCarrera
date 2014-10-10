using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodecExtensions;

namespace Fase02
{
    class Program
    {
        /*
         * Variables que usaremos para medir el tiempo de ejecución
         * y calcular la duración de una ejecución
         */
        DateTime inicio, final;
        TimeSpan duracion;

        static void Main(string[] args)
        {
            Program p = new Program();

            /*
             * 01. Clase básica
             */
            Clase01Basica c1 = new Clase01Basica();
            c1.var1 = 1;
            c1.var2 = "2";

            p.codificarClase01(c1);
            Object c1decodedAux = p.decodificarClase01(1, "2");

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase01
            p.inicio = DateTime.Now;
            Clase01Basica c1decoded = (Clase01Basica)c1decodedAux;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 02. Clase con métodos
             */
            Clase02Metodos c2 = new Clase02Metodos();

            p.codificarClase02(c2);
            Object c2decodedAux = p.decodificarClase02(1, "2"); // Los parámetros sobran (arrastrados de la clase Abstract)

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase02
            p.inicio = DateTime.Now;
            Clase02Metodos c2decoded = (Clase02Metodos)c2decodedAux;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 03. Clase con dos arrays
             */
            Clase03Array c3 = new Clase03Array();
            c3.var1 = new int[1];
            for (int i = 0; i < 1; i++)
            {
                c3.var1[i] = i;
            }
            c3.var2 = new string[2];
            for (int i = 0; i < 2; i++)
            {
                c3.var2[i] = Convert.ToString(i);
            }

            p.codificarClase03(c3);
            Object c3decodedAux = p.decodificarClase03(1, "2");

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase03
            p.inicio = DateTime.Now;
            Clase03Array c3decoded = (Clase03Array)c3decodedAux;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 04. Clase con una estructura
             */
            Clase04Struct c4 = new Clase04Struct();
            c4.valor3 = new Clase04Struct.estructura(1, "2");

            p.codificarClase04(c4);
            Object c4decodedAux = p.decodificarClase04(1, "2");

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase04
            p.inicio = DateTime.Now;
            Clase04Struct c4decoded = (Clase04Struct)c4decodedAux;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 05. Clase con una clase
             */
            Clase05Clase c5 = new Clase05Clase();
            Clase05Clase.ClaseInterna c5int = new Clase05Clase.ClaseInterna();
            c5int.var1 = 1;
            c5int.var2 = "2";
            c5.var3 = c5int;

            p.codificarClase05(c5);
            Object c5decodedAux = p.decodificarClase05(1, "2");

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase04
            p.inicio = DateTime.Now;
            Clase05Clase c5decoded = (Clase05Clase)c5decodedAux;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");

            /*
            * 01. Struct básica
            */
/*            Struct01Basica s1 = new Struct01Basica();
            s1.var1 = 1;
            s1.var2 = "2";

            Console.WriteLine("Codificación de estructura con dos campos");
            Codificador01A cod1A_ = new Codificador01A();
            p.inicio = DateTime.Now;
            cod1A_.encode(c1);
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo: " + p.duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Decodificación de una estructura con dos atributos");
            Decodificador01A dec1A_ = new Decodificador01A();
            p.inicio = DateTime.Now;
            Object c1decodedAux_ = dec1A_.decode(1, "2");
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo: " + p.duracion.TotalMilliseconds + " milisegundos");

            p.inicio = DateTime.Now;
            Clase01Basica c1decoded_ = (Clase01Basica)c1decodedAux_;
            p.final = DateTime.Now;
            p.duracion = p.final - p.inicio;
            Console.WriteLine("Tiempo de conversión de objeto: " + p.duracion.TotalMilliseconds + " milisegundos");
*/
            Console.ReadLine();
        }

        protected void codificarClase01(Object c)
        {
            Console.WriteLine("Codificación básica A. Clase con dos atributos");
            Codificador01A cod1A = new Codificador01A();
            inicio = DateTime.Now;
            cod1A.encode(c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica B. Clase con dos atributos");
            Codificador01B cod1B = new Codificador01B();
            inicio = DateTime.Now;
            cod1B.encode(ref c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica C. Clase con dos atributos");
            inicio = DateTime.Now;
            String aux = c.codificar();
            final = DateTime.Now;
            Console.WriteLine("Codificado: " + aux);
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
        }

        protected Object decodificarClase01(int v1, string v2)
        {
            Console.WriteLine("Decodificación básica A. Clase con dos atributos");
            Decodificador01A dec = new Decodificador01A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
/*
            Console.WriteLine("Decodificación básica B. Clase con dos atributos");
            Decodificador01A dec = new Decodificador01A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
*/        }
        protected void codificarClase02(Object c)
        {
            Console.WriteLine("Codificación con métodos A. Clase con dos métodos");
            Codificador02A cod = new Codificador02A();
            inicio = DateTime.Now;
            cod.encode(c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con métodos B. Clase con dos métodos");
            Codificador02B cod2 = new Codificador02B();
            inicio = DateTime.Now;
            cod2.encode(ref c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con métodos C. Clase con dos métodos");
            inicio = DateTime.Now;
            String aux = c.codificar();
            final = DateTime.Now;
            Console.WriteLine("Codificado: " + aux);
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
        }

        protected Object decodificarClase02(int v1, string v2)
        {
            Console.WriteLine("Decodificación con métodos. Clase con dos métodos");
            Decodificador02A dec = new Decodificador02A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
        }

        protected void codificarClase03(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con dos arrays");
            Codificador03A cod = new Codificador03A();
            inicio = DateTime.Now;
            cod.encode(c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con dos arrays");
            Codificador03B cod2 = new Codificador03B();
            inicio = DateTime.Now;
            cod2.encode(ref c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con dos arrays");
            inicio = DateTime.Now;
            String aux = c.codificar();
            final = DateTime.Now;
            Console.WriteLine("Codificado: " + aux);
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
        }

        protected Object decodificarClase03(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con dos arrays");
            Decodificador03A dec = new Decodificador03A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
        }

        protected void codificarClase04(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con un struct");
            Codificador04A cod = new Codificador04A();
            inicio = DateTime.Now;
            cod.encode(c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con un struct");
            Codificador04B cod2 = new Codificador04B();
            inicio = DateTime.Now;
            cod2.encode(ref c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con struct");
            inicio = DateTime.Now;
            String aux = c.codificar();
            final = DateTime.Now;
            Console.WriteLine("Codificado: " + aux);
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
        }

        protected Object decodificarClase04(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con un struct");
            Decodificador04A dec = new Decodificador04A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
        }

        protected void codificarClase05(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con otra clase en su interior");
            Codificador05A cod = new Codificador05A();
            inicio = DateTime.Now;
            cod.encode(c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con otra clase en su interior");
            Codificador05B cod2 = new Codificador05B();
            inicio = DateTime.Now;
            cod2.encode(ref c);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con otra clase en su interior");
            inicio = DateTime.Now;
            String aux = c.codificar();
            final = DateTime.Now;
            Console.WriteLine("Codificado: " + aux);
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
        }

        protected Object decodificarClase05(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con una clase en su interior");
            Decodificador05A dec = new Decodificador05A();
            inicio = DateTime.Now;
            Object cAux = dec.decode(v1, v2);
            final = DateTime.Now;
            duracion = final - inicio;
            Console.WriteLine("Tiempo: " + duracion.TotalMilliseconds + " milisegundos");
            return cAux;
        }

    }
}
