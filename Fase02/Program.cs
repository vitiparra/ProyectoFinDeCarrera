﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Stopwatch watch;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.watch = new System.Diagnostics.Stopwatch();

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
            p.watch.Restart();
            Clase01Basica c1decoded = (Clase01Basica)c1decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 02. Clase con métodos
             */
            Clase02Metodos c2 = new Clase02Metodos();

            p.codificarClase02(c2);
            Object c2decodedAux = p.decodificarClase02(1, "2"); // Los parámetros sobran (arrastrados de la clase Abstract)

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase02
            p.watch.Restart();
            Clase02Metodos c2decoded = (Clase02Metodos)c2decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");

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
            p.watch.Restart();
            Clase03Array c3decoded = (Clase03Array)c3decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");

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
            p.watch.Restart();
            Clase04Struct c4decoded = (Clase04Struct)c4decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");

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
            p.watch.Restart();
            Clase05Clase c5decoded = (Clase05Clase)c5decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("==================");
            /*
             * 06. Clase con una clase derivada
             */
            Clase06ClaseDerivada c6 = new Clase06ClaseDerivada();
            c6.var1 = 1;
            c6.var2 = "2";
            c6.var3 = 3;

            p.codificarClase06(c6);
            Object c6decodedAux = p.decodificarClase06(1, "2");

            // Cálculo del tiempo de conversión del objeto genérico recibido por el decodificador al objeto real
            // Este tiempo se sumará al tiempo de ejecución del método decodificarClase04
            p.watch.Restart();
            Clase06ClaseDerivada c6decoded = (Clase06ClaseDerivada)c6decodedAux;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");
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
            watch.Restart();
            cod1A.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica B. Clase con dos atributos");
            Codificador01B cod1B = new Codificador01B();
            watch.Restart();
            cod1B.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica C. Clase con dos atributos");
            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase01(int v1, string v2)
        {
            Console.WriteLine("Decodificación básica A. Clase con dos atributos");
            Decodificador01A dec = new Decodificador01A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

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
            watch.Restart();
            cod.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con métodos B. Clase con dos métodos");
            Codificador02B cod2 = new Codificador02B();
            watch.Restart();
            cod2.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con métodos C. Clase con dos métodos");
            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase02(int v1, string v2)
        {
            Console.WriteLine("Decodificación con métodos. Clase con dos métodos");
            Decodificador02A dec = new Decodificador02A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            return cAux;
        }

        protected void codificarClase03(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con dos arrays");
            Codificador03A cod = new Codificador03A();

            watch.Restart();
            cod.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con dos arrays");
            Codificador03B cod2 = new Codificador03B();

            watch.Restart();
            cod2.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
            
            Console.WriteLine("Codificación con estructuras complejas C. Clase con dos arrays");
            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase03(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con dos arrays");
            Decodificador03A dec = new Decodificador03A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            return cAux;
        }

        protected void codificarClase04(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con un struct");
            Codificador04A cod = new Codificador04A();

            watch.Restart();
            cod.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con un struct");
            Codificador04B cod2 = new Codificador04B();

            watch.Restart();
            cod2.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con struct");
            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase04(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con un struct");
            Decodificador04A dec = new Decodificador04A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            return cAux;
        }

        protected void codificarClase05(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase con otra clase en su interior");
            Codificador05A cod = new Codificador05A();

            watch.Restart();
            cod.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase con otra clase en su interior");
            Codificador05B cod2 = new Codificador05B();

            watch.Restart();
            cod2.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con otra clase en su interior");

            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase05(int v1, string v2)
        {
            Console.WriteLine("Decodificación con estructuras complejas. Clase con una clase en su interior");
            Decodificador05A dec = new Decodificador05A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            return cAux;
        }

        protected void codificarClase06(Object c)
        {
            Console.WriteLine("Codificación con estructuras complejas A. Clase que deriva de otra clase");
            Codificador06A cod = new Codificador06A();

            watch.Restart();
            cod.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas B. Clase que deriva de otra clase");
            Codificador06B cod2 = new Codificador06B();

            watch.Restart();
            cod2.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación con estructuras complejas C. Clase con otra clase en su interior");

            watch.Restart();
            String aux = c.codificar();
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarClase06(int v1, string v2)
        {
            Console.WriteLine("Decodificación con clases derivadas. Clase derivada de otra");
            Decodificador06A dec = new Decodificador06A();

            watch.Restart();
            Object cAux = dec.decode(v1, v2);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            return cAux;
        }
    }
}
