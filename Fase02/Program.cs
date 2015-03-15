using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        Stopwatch watch;
        int veces = 1000000;
        static String s;

        static void Main(string[] args)
        {
            Program p = new Program();
            p.watch = new System.Diagnostics.Stopwatch();

            /*
             * 01. Clase básica
             */
            Clase01Basica c1 = new Clase01Basica();
            Clase01Basica c1decoded;

            c1.var1 = 1;
            c1.var2 = "2";

            Console.WriteLine("Clase con dos atributos");
            s = p.codificarClase01(c1);
            c1decoded = p.decodificarClase01(s);
            Console.WriteLine("==================");

            /*
             * 02. Clase con métodos
             */
            Clase02Metodos c2 = new Clase02Metodos();
            Clase02Metodos c2decoded;

            Console.WriteLine("Clase con dos métodos");
            s = p.codificarClase02(c2);
            c2decoded = (Clase02Metodos)p.decodificarClase02(s);
            Console.WriteLine("==================");

            /*
             * 03. Clase con dos arrays
             */
            Clase03Array c3 = new Clase03Array();
            Clase03Array c3decoded;
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

            Console.WriteLine("Clase con dos arrays");
            s = p.codificarClase03(c3);
            c3decoded = (Clase03Array)p.decodificarClase03(s);
            Console.WriteLine("==================");

            /*
             * 04. Clase con una estructura
             */ 
            Clase04Struct c4 = new Clase04Struct();
            c4.valor3 = new Clase04Struct.estructura(1, "2");

            Clase04Struct c4decoded;

            Console.WriteLine("Clase con un atributo Struct");
            s = p.codificarClase04(c4);
            c4decoded = (Clase04Struct)p.decodificarClase04(s);
            Console.WriteLine("==================");

            /*
             * 05. Clase con una clase
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

            Console.WriteLine("==================");

            /*
            * 01. Struct básica
            Struct01Basica s1 = new Struct01Basica();
            s1.var1 = 1;
            s1.var2 = "2";

            p.codificarStruct01(s1);
            Object c1decodedAux_ = p.decodificarStruct01(1, "2");

            p.watch.Restart();
            Struct01Basica c1decoded_ = (Struct01Basica)c1decodedAux_;
            p.watch.Stop();
            Console.WriteLine("Tiempo de conversión de objeto: " + p.watch.ElapsedMilliseconds + " milisegundos");
*/
            Console.ReadLine();
        }

        protected String codificarClase01(Object c)
        {
            String sAux = "";

            watch.Restart();
            Codificador01A cod1A = new Codificador01A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1A.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación básica A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador01B cod1B = new Codificador01B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1B.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación básica B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación básica C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode((Clase01Basica)c);
            }
            watch.Stop();
            Console.WriteLine("Codificación básica D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase01Basica decodificarClase01(String s)
        {
            Clase01Basica cOut = null;

            watch.Restart();
            Decodificador01A dec1A = new Decodificador01A();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1A = null;
                cAux1A = dec1A.decode(s);
                cOut = (Clase01Basica)cAux1A;
            }
            watch.Stop();
            Console.WriteLine("Decodificación básica A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador01B dec1B = new Decodificador01B();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1B = null;
                dec1B.decode(ref cAux1B, s);
                cOut = (Clase01Basica)cAux1B;
            }
            watch.Stop();
            Console.WriteLine("Decodificación básica B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase01Basica);
            for (int i = 0; i < this.veces; i++)
            {
                Object aux = s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación básica C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación básica D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
        }


        protected String codificarClase02(Object c)
        {
            String sAux = "";

            watch.Restart();
            Codificador02A cod1A = new Codificador02A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1A.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con métodos A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador02B cod1B = new Codificador02B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1B.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con métodos B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación con métodos C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode((Clase02Metodos)c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con métodos D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Object decodificarClase02(String s)
        {
            Clase02Metodos cOut = null;

            watch.Restart();
            Decodificador02A dec = new Decodificador02A();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1A = null;
                cAux1A = dec.decode(s);
                cOut = (Clase02Metodos)cAux1A;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con métodos A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador02B dec1B = new Decodificador02B();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1B = null;
                dec1B.decode(ref cAux1B, s);
                cOut = (Clase02Metodos)cAux1B;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con métodos B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase02Metodos);
            for (int i = 0; i < this.veces; i++)
            {
                Object aux = s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación con métodos C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación con métodos D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
        }

        protected String codificarClase03(Object c)
        {
            String sAux = "";

            watch.Restart();
            Codificador03A codA = new Codificador03A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codA.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con dos arrays A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador03B codB = new Codificador03B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codB.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con dos arrays B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación con dos arrays C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode((Clase03Array)c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con dos arrays D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Object decodificarClase03(String s)
        {
            Clase03Array cOut = null;

            watch.Restart();
            Decodificador03A dec = new Decodificador03A();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAuxA = null;
                cAuxA = dec.decode(s);
                cOut = (Clase03Array)cAuxA;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador03B decB = new Decodificador03B();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAuxB = null;
                decB.decode(ref cAuxB, s);
                cOut = (Clase03Array)cAuxB;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase03Array);
            for (int i = 0; i < this.veces; i++)
            {
                Object aux = s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
        }

        protected String codificarClase04(Object c)
        {
            String sAux = "";

            watch.Restart();
            Codificador04A codA = new Codificador04A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codA.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con un atributo Struct A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador04B codB = new Codificador04B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codB.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con un atributo Struct B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con un atributo Struct C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode((Clase04Struct)c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con un atributo Struct D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Object decodificarClase04(String s)
        {
            Clase04Struct cOut = null;

            watch.Restart();
            Decodificador04A dec1A = new Decodificador04A();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1A = null;
                cAux1A = dec1A.decode(s);
                cOut = (Clase04Struct)cAux1A;
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador04B dec1B = new Decodificador04B();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1B = null;
                dec1B.decode(ref cAux1B, s);
                cOut = (Clase04Struct)cAux1B;
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase04Struct);
            for (int i = 0; i < this.veces; i++)
            {
                Object aux = s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
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
        protected void codificarStruct01(Object c)
        {
            Console.WriteLine("Codificación básica A. Struct con dos atributos simples");
            Codificador01AStruct cod1A = new Codificador01AStruct();
            watch.Restart();
            cod1A.encode(c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica B. Struct con dos atributos simples");
            Codificador01BStruct cod1B = new Codificador01BStruct();
            watch.Restart();
            cod1B.encode(ref c);
            watch.Stop();
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");

            Console.WriteLine("Codificación básica C. Struct con dos atributos simples");
            watch.Restart();
            String aux = c.codificar(); // Ver como hago esto
            watch.Stop();
            Console.WriteLine("Codificado: " + aux);
            Console.WriteLine("Tiempo: " + watch.ElapsedMilliseconds + " milisegundos");
        }

        protected Object decodificarStruct01(String s)
        {
            Console.WriteLine("Decodificación básica A. Struct con dos atributos simples");
            Decodificador01AStruct dec = new Decodificador01AStruct();

            watch.Restart();
            Object cAux = dec.decode(s);
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
            */
        }
    }
}
