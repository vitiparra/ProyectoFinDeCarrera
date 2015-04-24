using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fase02;

namespace Fase05
{
    public class Program
    {
        public class ClaseB
        {
            public int varB1 { get; set; }
            public ClaseB()
            {
                varB1 = 100;
            }
        }

        public class ClasePrueba
        {
            public int var1 { get; set; }
            public ClaseB var2 { get; set; }

            public ClasePrueba()
            {
                var1 = 33;
                ClaseB b = new ClaseB();
                var2 = b;
            }
        }

        static void Main(string[] args)
        {
            string str = "";

            ClasePrueba c = new ClasePrueba();

            Generador gPrueba = new Generador(c.GetType());
            var serializadorPrueba = gPrueba.getSerializer();
            if (serializadorPrueba != null)
            {
                //                Console.Write(serializadorPrueba.codificar(c));
                string codigo = serializadorPrueba.codificar(c);
                Console.WriteLine(codigo);
                ClasePrueba aux = new ClasePrueba();
                serializadorPrueba.decodificar(codigo, ref aux);
                Console.WriteLine(aux.var1 + "," + aux.var2.varB1);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }

            Fase02.Clase03Array c3 = new Clase03Array();
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
/*
                c3.var3 = new int[3][];
                for (int i = 0; i < 3; i++)
                {
                    int[] aux = new int[4];
                    for (int j = 0; j < 4; j++)
                    {
                        aux[j] = i * (j + 3);
                    }
                    c3.var3[i] = aux;
                }
                Console.WriteLine("Antes");
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        Console.WriteLine(c3.var3[i][j]);
                    }
                }

                c3.var4 = new List<Fase02.ClaseViti>();
                Fase02.ClaseViti clase = new Fase02.ClaseViti();
                clase.v1 = "uno";
                clase.v2 = 1;
                c3.var4.Add(clase);
                Fase02.ClaseViti clase2 = new Fase02.ClaseViti();
                clase2.v1 = "dos";
                clase2.v2 = 2;
                c3.var4.Add(clase2);
                Fase02.ClaseViti clase3 = new Fase02.ClaseViti();
                clase3.v1 = "tres";
                clase3.v2 = 3;
                c3.var4.Add(clase3);
*/
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

                for (int i = 0; i < 3; i++)
                {
                    Console.Write(c3aux.var2[i]);
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
/*
                foreach (Fase02.ClaseViti i in c3aux.var4)
                {
                    Console.WriteLine(i.v1.ToString() + "," + i.v2.ToString());
                }

                foreach (KeyValuePair<int, string> i in c3aux.var5)
                {
                    Console.WriteLine(i.Key.ToString() + "," + i.Value.ToString());
                }
*/

            }
            /* 
                        #region Clase01Basica

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
                        #endregion

            /*
                        Fase02.Clase03Array c3 = new Clase03Array();
                        Generador g3 = new Generador(c3.GetType());
                        dynamic serializador3 = g3.getSerializer();
                        if (serializador3 != null)
                        {
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

                            c3.var6 = new Dictionary<string, int>();
                            c3.var6.Add("uno", 1);
                            c3.var6.Add("dos", 2);
                            c3.var6.Add("tres", 3);

                            #endregion
                            Console.Write(serializador3.codificar(c3));
            //                serializador3.encode(c3, ref str);
                        }
                        else
                        {
                            Console.WriteLine("No se ha podido generar el serializador");
                        }

            /
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
