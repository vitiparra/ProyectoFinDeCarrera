using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using System.IO;
using System.Dynamic;

using Fase02;
using System.Reflection;

namespace Fase04
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "";

            Fase02.Clase01Basica c1 = new Clase01Basica();
//            c1.var1 = 2;
//            c1.var2 = "Hola";

            Fase02.Clase03Array c3 = new Clase03Array();
            c3.var1 = new int[1];
            for (int i = 0; i < 1; i++)
            {
                c3.var1[i] = i;
            }
            c3.var2 = new string[3];
            for (int i = 0; i < 3; i++)
            {
                c3.var2[i] = "Número " + Convert.ToString(i);
            }
            /*
            c3.var3 = new int[1,2];
            int cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    c3.var3[i, j] = cont;
                    cont++;
                }
            }
            c3.var4 = new int[1, 2, 3];
            cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        c3.var4[i, j, k] = cont;
                        cont++;
                    }
                }
            }
            c3.var5 = new int[3][];
            cont = 0;
            for (int i = 0; i < 3; i++)
            {
                int[] aux = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    aux[j] = cont;
                    cont++;
                }
                c3.var5[i] = aux;
            }
            */
            Fase02.Clase04Struct c4 = new Clase04Struct();
            c4.valor3.valor1 = 1;
            c4.valor3.valor2 = "hola";

            Fase02.Clase06ClaseDerivada c6 = new Clase06ClaseDerivada();
            c6.var1 = 1;
            c6.var2 = "hola";
            c6.var3 = 2;
/*
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            if (serializador1 != null)
            {
                Type tipo = serializador1.GetType();
                Console.WriteLine(tipo.FullName);
                serializador1.encode(c1, ref str);
                c1 = new Clase01Basica();
                c1 = serializador1.decode(str, c1);
                Console.WriteLine("Variables normales:");
                Console.WriteLine(c1.var1);
                Console.WriteLine(c1.var2);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }

            Generador g6 = new Generador(c6.GetType());
            dynamic serializador6 = g6.getSerializer();
            if (serializador6 != null)
            {
                Type tipo6 = serializador6.GetType();
                Console.WriteLine(tipo6.FullName);
                serializador6.encode(c6, ref str);
                c6 = new Clase06ClaseDerivada();
                c6 = serializador6.decode(str, c6);
                Console.WriteLine("Variables heredadas:");
                Console.WriteLine(c6.var1);
                Console.WriteLine(c6.var2);
                Console.WriteLine(c6.var3);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }

            Generador g3 = new Generador(c3.GetType());
            dynamic serializador3 = g3.getSerializer();
            if (serializador3 != null)
            {
                Type tipo3 = serializador3.GetType();
                Console.WriteLine(tipo3.FullName);
                serializador3.encode(c3, ref str);
                c3 = new Clase03Array();
                c3 = serializador3.decode(str, c3);
                Console.WriteLine("Array var1:");
                for (int i = 0; i < c3.var1.Length; i++)
                {
                    Console.WriteLine(c3.var1[i]);
                }
                Console.WriteLine("Array var2:");
                for (int i = 0; i < c3.var2.Length; i++)
                {
                    Console.WriteLine(c3.var2[i]);
                }
                Console.WriteLine("Array var3:");
                for (int i = 0; i < 1; i++)
                {
                    for(int j = 0; j < 2; j++)
                    {
                        Console.Write(c3.var3[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Array var4:");
                for (int i = 0; i < 1; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            Console.WriteLine(c3.var4[i,j,k]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Array var5:");
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Array del elemento " + i);
                    for (int j = 0; j < 4; j++ )
                    {
                        Console.WriteLine(c3.var5[i][j]);
                    }
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
*/
            Generador g4 = new Generador(c4.GetType());
            dynamic serializador4 = g4.getSerializer();
            if (serializador4 != null)
            {
                Type tipo4 = serializador4.GetType();
                Console.WriteLine(tipo4.FullName);
                serializador4.encode(c4, ref str);
                c4 = new Clase04Struct();
                c4 = serializador4.decode(str, c4);
                Console.WriteLine("Clase con estructura+:");
                Console.WriteLine(c4.valor3.valor1);
                Console.WriteLine(c4.valor3.valor2);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }             

            Console.ReadKey();
        }
    }
}
