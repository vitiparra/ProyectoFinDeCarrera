
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

    public class Clase07ClaseConTodoCodec
    {
        public static Type tipo = Type.GetType("Clase07ClaseConTodo");
        public static String nombreClase = "Clase07ClaseConTodo";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.Clase07ClaseConTodo obj)
        {
            return encode(obj);
        }
        public void decodificar(string codigo, ref Fase02.Clase07ClaseConTodo obj)
        {
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.Clase07ClaseConTodo obj)
        {
            StringBuilder texto = new StringBuilder("<elementos>");
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("publicInt");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append(obj.publicInt.ToString());
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.publicArrayInt == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("publicArrayInt");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32[]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.publicArrayInt.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("System.Int32");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("1");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.publicArrayInt.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.publicArrayInt.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.publicArrayInt.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (System.Int32 elementoAuxobjpublicArrayInt in obj.publicArrayInt)
                {
                    texto.Append("<valor>");
                    texto.Append(elementoAuxobjpublicArrayInt.ToString());
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("publicStaticColores");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("Fase02.Clase07ClaseConTodo+colores");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append(obj.publicStaticColores.ToString());
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            texto.Append("</elementos>");
            //str = texto;
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase07ClaseConTodo obj)
        {
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            xml.LoadXml(codigo);
            XmlNode nodoPrincipal = xml.SelectSingleNode("elementos");
            XmlNodeReader nr = new XmlNodeReader(nodoPrincipal);
            nr.Read(); // Elementos
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Valor del campo o propiedad
                obj.publicInt = Int32.Parse(nr.Value);
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjpublicArrayInt = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjpublicArrayIntLength0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjpublicArrayIntGetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjpublicArrayIntGetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjpublicArrayIntGetLowerBound0; auxIndice0 <= auxobjpublicArrayIntGetUpperBound0; auxIndice0++)
                {
                    if (obj.publicArrayInt == null) obj.publicArrayInt = new System.Int32[auxobjpublicArrayIntLength0];
                    nr.Read(); // Valor
                    nr.Read(); // Valor del campo o propiedad
                    obj.publicArrayInt[auxIndice0] = Int32.Parse(nr.Value);
                    nr.Read(); // Valor
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Valor del campo o propiedad
                obj.publicStaticColores = (Fase02.Clase07ClaseConTodo.colores)Enum.Parse(typeof(Fase02.Clase07ClaseConTodo.colores), nr.Value);
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elementos
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
 */

            Clase07ClaseConTodo c = new Clase07ClaseConTodo();
            Clase07ClaseConTodo cdecoded;

            Clase07ClaseConTodoCodec serializador1 = new Clase07ClaseConTodoCodec();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                string codigo = serializador1.codificar(c);
                Console.WriteLine(codigo);

                cdecoded = new Fase02.Clase07ClaseConTodo();
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
