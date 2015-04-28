using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fase02;
using Fase06;

namespace Fase06
{
    public class Program
    {
        static void Main(string[] args)
        {
            string str = "";

            #region ClasePrueba+ClaseB
/*
            Fase02.ClasePrueba c = new Fase02.ClasePrueba();
            Generador gPrueba = new Generador(c.GetType());
            var serializadorPrueba = gPrueba.getSerializer();
            if (serializadorPrueba != null)
            {
                string codigo = serializadorPrueba.codificar(c);
                Console.WriteLine(codigo);
                Fase02.ClasePrueba aux = new Fase02.ClasePrueba();
                aux.var1 = 0;
                serializadorPrueba.decodificar(codigo, ref aux);
                Console.WriteLine(aux.var1 + "," + aux.var2.varB1 + "," + aux.var3);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
*/
            #endregion

            #region Clase03Array
/*            Fase02.Clase03Array c3 = new Clase03Array();
            Generador g3 = new Generador(c3.GetType());
            dynamic serializador3 = g3.getSerializer();
            if (serializador3 != null)
            {
                c3.var1 = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var1[i] = i;
                }

                c3.var2 = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var2[i] = i.ToString();
                }

                c3.var4 = new int[3, 2, 2];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            c3.var4[i, j, k] = i * (j + 3) + (k * 3);
                        }
                    }
                }

                c3.var6 = new Dictionary<string, int>();
                c3.var6.Add("cuatro", 4);
                c3.var6.Add("cinco", 5);
                c3.var6.Add("seis", 6);

                string codigo = serializador3.codificar(c3);
                Console.WriteLine(codigo);
                Fase02.Clase03Array c3aux = new Clase03Array();
                serializador3.decodificar(codigo, ref c3aux);

                for (int i = 0; i < 3; i++)
                {
                    Console.Write(c3aux.var1[i]);
                }
                Console.WriteLine("");

                if(c3aux.var2 == null)
                {
                    Console.Write("¡var2 no se ha serializado!");
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Write(c3aux.var2[i]);
                    }
                }
                Console.WriteLine("");

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Console.Write(c3aux.var4[i, j, k]);
                        }
                    }
                }

                foreach(KeyValuePair<string, int> aux in c3aux.var6)
                {
                    Console.WriteLine(aux.Key + " = " + aux.Value);
                }
*/
            #endregion
/*
                foreach (Fase02.ClaseViti i in c3aux.var4)
                {
                    Console.WriteLine(i.v1.ToString() + "," + i.v2.ToString());
                }

                foreach (KeyValuePair<int, string> i in c3aux.var5)
                {
                    Console.WriteLine(i.Key.ToString() + "," + i.Value.ToString());
                }

            }
*/
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
                            string codigo = serializador1.codificar(c1);
                            Console.WriteLine(codigo);

                            c1 = new Fase02.Clase01Basica();
                            c1 = serializador1.decodificar(codigo, ref c1);
                            Console.WriteLine(c1.var1);
                            Console.WriteLine(c1.var2);
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido generar el serializador");
                        }
*/ 
                        #endregion

                        Fase02.Clase03Array c3a = new Clase03Array();
                        Generador g3a = new Generador(c3a.GetType());
                        dynamic serializador3a = g3a.getSerializer();
                        if (serializador3a != null)
                        {
                            #region Datos Clase03Array
                            c3a.var1 = new int[3];
                            for (int i = 0; i < 3; i++)
                            {
                                c3a.var1[i] = i;
                            }
                
                            c3a.var2 = new string[3];
                            for (int i = 0; i < 3; i++)
                            {
                                c3a.var2[i] = "Número " + Convert.ToString(i);
                            }
                
                            c3a.var3 = new int[1, 2];
                            int cont = 0;
                            for (int i = 0; i < 1; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    c3a.var3[i, j] = cont;
                                    cont++;
                                }
                            }
                
                            c3a.var4 = new int[1, 2, 3];
                            cont = 0;
                            for (int i = 0; i < 1; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {
                                        c3a.var4[i, j, k] = cont;
                                        cont++;
                                    }
                                }
                            }
                
                            c3a.var5 = new int[3][];
                            cont = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                int[] aux = new int[4];
                                for (int j = 0; j < 4; j++)
                                {
                                    aux[j] = cont;
                                        cont++;
                                }
                                c3a.var5[i] = aux;
                            }

                            c3a.var6 = new Dictionary<string, int>();
                            c3a.var6.Add("uno", 1);
                            c3a.var6.Add("dos", 2);
                            c3a.var6.Add("tres", 3);

                            Fase02.DentroDelArray auxDentro = new DentroDelArray();
                            auxDentro.uno = 1;
                            auxDentro.dos = "dos";
                            Fase02.DentroDelArray auxDentro2 = new DentroDelArray();
                            auxDentro2.uno = 3;
                            auxDentro2.dos = "cuatro";

                            c3a.var7 = new DentroDelArray[2];
                            c3a.var7[0] = auxDentro;
                            c3a.var7[1] = auxDentro2;
                            #endregion
                            string codigo = serializador3a.codificar(c3a);
                            Console.WriteLine(codigo);
                            Clase03Array aux3a = new Clase03Array();
                            serializador3a.decodificar(codigo, ref aux3a);

                            for (int i = 0; i < 3; i++)
                            {
                                Console.Write(c3a.var1[i] + ",");
                            }
                            Console.WriteLine();
/*
                            for (int i = 0; i < 3; i++)
                            {
                                Console.Write(c3a.var2[i] + ",");
                            }
                            Console.WriteLine();
*/
                            for (int i = 0; i < 1; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    Console.Write(c3a.var3[i, j] + ",");
                                }
                            }
                            Console.WriteLine();

                            for (int i = 0; i < 1; i++)
                            {
                                for (int j = 0; j < 2; j++)
                                {
                                    for (int k = 0; k < 3; k++)
                                    {
                                        Console.Write(c3a.var4[i, j, k]);
                                    }
                                }
                            }
                            Console.WriteLine();
/*
                            for (int i = 0; i < 3; i++)
                            {
                                int[] aux = new int[4];
                                for (int j = 0; j < 4; j++)
                                {
                                    Console.Write(aux[j] + ",");
                                }
                                c3a.var5[i] = aux;
                                Console.WriteLine();
                            }
                            Console.WriteLine();
*/
                            foreach(KeyValuePair<string, int> par in c3a.var6)
                            {
                                Console.WriteLine(par.Key + "=>" + par.Value);
                            }
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
                            Console.Write(serializador7.codificar(c7));
            //                serializador7.encode(c7, ref str);
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
