﻿using System;
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

            #region Clase01Basica
/*
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
/
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
*/ 
            #endregion

            Fase02.Clase03Array c3 = new Clase03Array();
            Generador g3 = new Generador(c3.GetType());
            dynamic serializador3 = g3.getSerializer();
            if (serializador3 != null)
            {
                Type tipo3 = serializador3.GetType();
                Console.WriteLine(tipo3.FullName);
                #region Datos Clase03Array
                c3.var1 = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var1[i] = i;
                }
                c3.var2 = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var2[i] = "Número " + Convert.ToString(i);
                }
                c3.var3 = new int[1, 2];
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

                #endregion
                serializador3.encode(c3, ref str);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }

/*
            Fase02.Clase07ClaseConTodo c7 = new Clase07ClaseConTodo();
            Generador g7 = new Generador(c7.GetType());
            dynamic serializador7 = g7.getSerializer();
            if (serializador7 != null)
            {
                Type tipo7 = serializador7.GetType();
                Console.WriteLine(tipo7.FullName);
                serializador7.encode(c7, ref str);
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
