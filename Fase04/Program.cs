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
            c1.var1 = 2;
            c1.var2 = "Hola";

            Fase02.Clase03Array c3 = new Clase03Array();
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
            c3.var3 = new int[4, 3];
            int cont = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    c3.var3[i, j] = cont;
                    cont++;
                }
            }
            c3.var4 = new int[3, 2, 1];
            cont = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 1; k++)
                    {
                        c3.var4[i, j, k] = cont;
                        cont++;
                    }
                }
            }

            Fase02.Clase04Struct c4 = new Clase04Struct();
            c4.valor3.valor1 = 1;
            c4.valor3.valor2 = "hola";

            Fase02.Clase06ClaseDerivada c6 = new Clase06ClaseDerivada();
            c6.var1 = 1;
            c6.var2 = "hola";
            c6.var3 = 2;

            /*
                        Generador g = new Generador(c6.GetType());
                        dynamic serializador = g.getSerializer();
                        if (serializador != null)
                        {
                            Type tipo = serializador.GetType();
                            Console.WriteLine(tipo.FullName);
                            serializador.encode(c6, ref str);
                            c6 = new Clase06ClaseDerivada();
                            c6 = serializador.decode(str, c6);
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido generar el serializador");
                        }

                        Generador g = new Generador(c4.GetType());
                        dynamic serializador = g.getSerializer();
                        if (serializador != null)
                        {
                            Type tipo = serializador.GetType();
                            Console.WriteLine(tipo.FullName);
                            serializador.encode(c4, ref str);
                            c4 = new Clase04Struct();
                            c4 = serializador.decode(str, c4);
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido generar el serializador");
                        }
             */

            Generador g = new Generador(c3.GetType());
            dynamic serializador = g.getSerializer();
            if (serializador != null)
            {
                Type tipo = serializador.GetType();
                Console.WriteLine(tipo.FullName);
                serializador.encode(c3, ref str);
                c3 = new Clase03Array();
                c3 = serializador.decode(str, c3);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }

            /*
                        Generador g2 = new Generador(c1.GetType());
                        dynamic serializador2 = g2.getSerializer();
                        if (serializador2 != null)
                        {
                            Type tipo = serializador2.GetType();
                            Console.WriteLine(tipo.FullName);
                            serializador2.encode(c1, ref str);
                            c1 = new Clase01Basica();
                            c1 = serializador2.decode(str, c1);
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido generar el serializador");
                        }
            */
            Console.ReadKey();
        }
    }
}
