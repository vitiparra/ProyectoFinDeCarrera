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
             * 02. Clase con arrays
             */
            Clase02ArrayNormal c2 = new Clase02ArrayNormal();
            Clase02ArrayNormal c2decoded;

            Console.WriteLine("Clase con arrays");
            s = p.codificarClase02(c2);
            c2decoded = p.decodificarClase02(s);
            Console.WriteLine("==================");

            /*
             * 03. Clase con arrays
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
            /*
            c3.var3 = new int[3, 4];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    c3.var3[i,j] = i+j;
                }
            }

            c3.var4 = new int[4, 5, 6];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        c3.var4[i, j, k] = i + j + k;
                    }
                }
            }

            c3.var5 = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                int[] aux = new int[5];
                for (int j = 0; j < 5; j++)
                {
                    aux[j] = j;
                }
                c3.var5[i] = aux;
            }
            */
            Console.WriteLine("Clase con cinco arrays");
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
            c4decoded = p.decodificarClase04(s);
            Console.WriteLine("==================");

            /*
             * 05. Clase con una clase
             */
            Clase05Clase c5 = new Clase05Clase();
            Clase05Clase.ClaseInterna c5int = new Clase05Clase.ClaseInterna();
            c5int.var1 = 1;
            c5int.var2 = "2";
            c5.var3 = c5int;

            Clase05Clase c5decoded;

            s = p.codificarClase05(c5);
            c5decoded = p.decodificarClase05(s);
            Console.WriteLine("==================");

            /*
             * 06. Clase con una clase derivada
             */ 
            Clase06ClaseDerivada c6 = new Clase06ClaseDerivada();
            c6.var1 = 1;
            c6.var2 = "2";
            c6.var3 = 3;

            Clase06ClaseDerivada c6decoded;

            s = p.codificarClase06(c6);
            c6decoded = p.decodificarClase06(s);
            Console.WriteLine("==================");

            /*
             * 07. Clase con todo
             */
            Clase07ClaseConTodo c7 = new Clase07ClaseConTodo();
            c7.basePublicInt = 1;
            c7.lista = new List<int>();
            c7.lista.Add(1);
            c7.lista.Add(2);
            c7.publicArray2DInt = new int[1,2];
            c7.publicArray2DInt[0,0] = 1;
            c7.publicArray2DInt[0,1] = 1;
            c7.publicArrayInt = new int[3];
            c7.publicArrayInt[0] = 1;
            c7.publicArrayInt[1] = 2;
            c7.publicArrayInt[2] = 3;
            c7.publicArrayMatrizEscalonadaInt = new int[2][];
            int[] arrayAaux = new int[3];
            arrayAaux[0] = 1;
            arrayAaux[1] = 2;
            arrayAaux[2] = 3;
            c7.publicArrayMatrizEscalonadaInt[0] = arrayAaux;
            c7.publicArrayMatrizEscalonadaInt[1] = arrayAaux;
            c7.publicInt = 30;

            Clase07ClaseConTodo c7decoded;

            Console.WriteLine("Clase con todo");
            s = p.codificarClase07(c7);
            c7decoded = p.decodificarClase07(s);
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

        protected String codificarClase01(Clase01Basica c)
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
                sAux = SerializerStatic.encode(c);
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
                dec1B.decode(ref cOut, s);
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


        protected String codificarClase02(Clase02ArrayNormal c)
        {
            String sAux = "";

            watch.Restart();
            Codificador02A cod1A = new Codificador02A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1A.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador02B cod1B = new Codificador02B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod1B.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase02ArrayNormal decodificarClase02(String s)
        {
            Clase02ArrayNormal cOut = null;

            watch.Restart();
            Decodificador02A dec = new Decodificador02A();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1A = null;
                cAux1A = dec.decode(s);
                cOut = (Clase02ArrayNormal)cAux1A;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador02B dec1B = new Decodificador02B();
            for (int i = 0; i < this.veces; i++)
            {
                Object cAux1B = null;
                dec1B.decode(ref cAux1B, s);
                cOut = (Clase02ArrayNormal)cAux1B;
            }
            watch.Stop();
            Console.WriteLine("Decodificación con arrays B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase02ArrayNormal);
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

        protected String codificarClase03(Clase03Array c)
        {
            String sAux = "";

            watch.Restart();
            Codificador03A codA = new Codificador03A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codA.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador03B codB = new Codificador03B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codB.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con arrays D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase03Array decodificarClase03(String s)
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
                decB.decode(ref cOut, s);
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

        protected String codificarClase04(Clase04Struct c)
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
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con un atributo Struct D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase04Struct decodificarClase04(String s)
        {
            Clase04Struct cOut = null;

            watch.Restart();
            Decodificador04A dec1A = new Decodificador04A();
            for (int i = 0; i < this.veces; i++)
            {
                Clase04Struct cAux1A = null;
                cAux1A = dec1A.decode(s);
                cOut = (Clase04Struct)cAux1A;
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador04B dec1B = new Decodificador04B();
            for (int i = 0; i < this.veces; i++)
            {
                Clase04Struct cAux1B = null;
                dec1B.decode(ref cAux1B, s);
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

        protected String codificarClase05(Clase05Clase c)
        {
            String sAux = "";

            watch.Restart();
            Codificador05A cod5A = new Codificador05A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod5A.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con otra clase dentro A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador05B cod5B = new Codificador05B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod5B.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con otra clase dentro B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con otra clase dentro C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con otra clase dentro D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase05Clase decodificarClase05(string s)
        {
            Clase05Clase cOut = new Clase05Clase();

            watch.Restart();
            Decodificador05A dec5A = new Decodificador05A();
            for (int i = 0; i < this.veces; i++)
            {
                cOut = dec5A.decode(s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con miembro de clase A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador05B dec5B = new Decodificador05B();
            for (int i = 0; i < this.veces; i++)
            {
                dec5B.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con miembro de clase B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase05Clase);
            for (int i = 0; i < this.veces; i++)
            {
                Clase05Clase aux = (Clase05Clase)s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con miembro de clase C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con atributo Struct D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
        }

        protected String codificarClase06(Clase06ClaseDerivada c)
        {
            String sAux = "";

            watch.Restart();
            Codificador06A cod6A = new Codificador06A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod6A.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con clases derivadas A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador06B cod6B = new Codificador06B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = cod6B.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con clases derivadas B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación con clases derivadas C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación con clases derivadas D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase06ClaseDerivada decodificarClase06(string s)
        {
            Clase06ClaseDerivada cOut = new Clase06ClaseDerivada();

            watch.Restart();
            Decodificador06A dec6A = new Decodificador06A();
            for (int i = 0; i < this.veces; i++)
            {
                cOut = dec6A.decode(s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con clase derivada A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador06B dec6B = new Decodificador06B();
            for (int i = 0; i < this.veces; i++)
            {
                Clase06ClaseDerivada cAux6B = new Clase06ClaseDerivada();
                dec6B.decode(ref cAux6B, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con clase derivada B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase06ClaseDerivada);
            for (int i = 0; i < this.veces; i++)
            {
                Clase06ClaseDerivada aux = (Clase06ClaseDerivada)s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con clase derivada C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
        }

        protected String codificarClase07(Clase07ClaseConTodo c)
        {
            String sAux = "";

            watch.Restart();
            Codificador07A codA = new Codificador07A();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codA.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con todo A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Codificador07B codB = new Codificador07B();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = codB.encode(ref c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con todo B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = c.codificar();
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con todo C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                sAux = SerializerStatic.encode(c);
            }
            watch.Stop();
            Console.WriteLine("Codificación de clase con todo D: " + watch.ElapsedMilliseconds + " milisegundos");

            return sAux;
        }

        protected Clase07ClaseConTodo decodificarClase07(String s)
        {
            Clase07ClaseConTodo cOut = new Clase07ClaseConTodo();

            watch.Restart();
            Decodificador07A dec1A = new Decodificador07A();
            for (int i = 0; i < this.veces; i++)
            {
                cOut = dec1A.decode(s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con todo A: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Decodificador07B dec1B = new Decodificador07B();
            for (int i = 0; i < this.veces; i++)
            {
                Clase07ClaseConTodo cAux1B = new Clase07ClaseConTodo();
                dec1B.decode(ref cAux1B, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con todo B: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            Type t = typeof(Fase02.Clase07ClaseConTodo);
            for (int i = 0; i < this.veces; i++)
            {
                Clase07ClaseConTodo aux = (Clase07ClaseConTodo)s.decodificar(t);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con todo C: " + watch.ElapsedMilliseconds + " milisegundos");

            watch.Restart();
            for (int i = 0; i < this.veces; i++)
            {
                SerializerStatic.decode(ref cOut, s);
            }
            watch.Stop();
            Console.WriteLine("Decodificación clase con todo D: " + watch.ElapsedMilliseconds + " milisegundos");

            return cOut;
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
