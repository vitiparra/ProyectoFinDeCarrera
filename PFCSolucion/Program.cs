
using System;
using System.Xml;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

using Fase02;

namespace Serializer
{

    public class Clase04StructCodec {
        public static Type tipo = Type.GetType("Clase04Struct");
        public static String nombreClase = "Clase04Struct";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.Clase04Struct obj){
            return encode(obj);}
        public void decodificar(string codigo, ref Fase02.Clase04Struct obj){
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.Clase04Struct obj){
            StringBuilder texto = new StringBuilder();
              {
//            texto.Append("valor3,");
//            texto.Append("Fase02.Clase04Struct+estructura,");
            texto.Append(estructuraCodec.encode(obj.valor3));
              } // if(obj.nombre == null) 
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase04Struct obj){
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
              string comprobarNullvalor3 = elementos.Peek();
              if(comprobarNullvalor3 == "NULL")
              {
                elementos.Dequeue(); // Quitamos el NULL
              }
              else
              {
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            Fase02.Clase04Struct.estructura objAuxvalor3 = new Fase02.Clase04Struct.estructura();
            string aux = string.Join(",", elementos.ToArray());
            estructuraCodec.decode(ref aux, ref objAuxvalor3);
            elementos = new Queue<string>(aux.Split(','));
            obj.valor3 = objAuxvalor3;
              }// if(comprobarNullvalor3 == "NULL")
            codigo = string.Join(",", elementos.ToArray());
        }
    }
    public class estructuraCodec {
        public static Type tipo = Type.GetType("estructura");
        public static String nombreClase = "estructura";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.Clase04Struct.estructura obj){
            return encode(obj);}
        public void decodificar(string codigo, ref Fase02.Clase04Struct.estructura obj){
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.Clase04Struct.estructura obj){
            StringBuilder texto = new StringBuilder();
              {
//            texto.Append("valor1,");
//            texto.Append("System.Int32,");
            texto.Append(obj.valor1.ToString() + ",");
              } // if(obj.nombre == null) 
              if(obj.valor2 == null)
              {
                texto.Append("NULL,");
              }
              else
              {
//            texto.Append("valor2,");
//            texto.Append("System.String,");
            texto.Append(obj.valor2.ToString() + ",");
              } // if(obj.nombre == null) 
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase04Struct.estructura obj){
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
              string comprobarNullvalor1 = elementos.Peek();
              if(comprobarNullvalor1 == "NULL")
              {
                elementos.Dequeue(); // Quitamos el NULL
              }
              else
              {
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            obj.valor1 = Int32.Parse(elementos.Dequeue());
              }// if(comprobarNullvalor1 == "NULL")
              string comprobarNullvalor2 = elementos.Peek();
              if(comprobarNullvalor2 == "NULL")
              {
                elementos.Dequeue(); // Quitamos el NULL
              }
              else
              {
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            obj.valor2 = elementos.Dequeue();
              }// if(comprobarNullvalor2 == "NULL")
            codigo = string.Join(",", elementos.ToArray());
        }

        static void Main(string[] args)
        {
/*
            Fase02.Clase03Array c3a = new Fase02.Clase03Array();
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

            c3a.var7 = new Fase02.DentroDelArray[2];
            Fase02.DentroDelArray aux1 = new Fase02.DentroDelArray();
            aux1.uno = 1;
            aux1.dos = "dos";
            c3a.var7[0] = aux1;
            Fase02.DentroDelArray aux2 = new Fase02.DentroDelArray();
            aux2.uno = 3;
            aux2.dos = "cuatro";
            c3a.var7[1] = aux2;

            Clase03ArrayCodec serializador1 = new Clase03ArrayCodec();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                string codigo = serializador1.codificar(c3a);
                Console.WriteLine(codigo);

                Fase02.Clase03Array c3b = new Fase02.Clase03Array();
                serializador1.decodificar(codigo, ref c3b);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
 
            #endregion
*/

            Clase04Struct c = new Clase04Struct();
            Clase04Struct cdecoded;
            c.valor3.valor1 = 1;
            c.valor3.valor2 = "2";

            Clase04StructCodec serializador1 = new Clase04StructCodec();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                string codigo = serializador1.codificar(c);
                Console.WriteLine(codigo);

                cdecoded = new Fase02.Clase04Struct();
                serializador1.decodificar(codigo, ref cdecoded);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
            Console.ReadLine();
        }
    }
}
