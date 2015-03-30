using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fase02;
using System.Reflection;

namespace PruebaFase05
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Runtime.CompilerServices.DynamicAttribute a = new System.Runtime.CompilerServices.DynamicAttribute();
/*
            #region Clase01Basica
            Fase02.Clase01Basica c = new Clase01Basica();
            c.var1 = 1;
            c.var2 = "hola";
            string texto = Codificador.encode(c);
            Console.WriteLine(texto);
            #endregion

*/

            #region Clase03Array
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
            string texto3 = Codificador.encode(c3);
            Console.WriteLine(texto3);
            #endregion
            /*
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
                    for (int j = 0; j < 2; j++)
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
                            Console.WriteLine(c3.var4[i, j, k]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("Array var5:");
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine("Array del elemento " + i);
                    for (int j = 0; j < 4; j++)
                    {
                        Console.WriteLine(c3.var5[i][j]);
                    }
                    Console.WriteLine();
                }
             / 
            #endregion
            Fase02.Clase07ClaseConTodo c7 = new Clase07ClaseConTodo();
            string texto = Codificador.encode(c7);
            Console.WriteLine(texto);
 
            Fase02.Clase08List c8 = new Clase08List();
            string texto = Codificador.encode(c8);
            Console.WriteLine(texto);

            Fase02.Clase09MiembroClase c9 = new Clase09MiembroClase();
            string texto = Codificador.encode(c9);
            Console.WriteLine(texto);
*/

            Console.ReadLine();
        }
    }

    public class Codificador
    {
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        private static int tabuladores = 0;
        public static String encode(object c)
        {
            string texto = "";
            texto = "<serializacion>" + SALTO;
            texto += TAB + "<accesibilidad>public</accesibilidad>" + SALTO;
            texto += TAB + "<tipoDeObjeto>clase</tipoDeObjeto>" + SALTO;
            texto += TAB + "<tipoDeObjeto>clase</tipoDeObjeto>" + SALTO;
            texto += TAB + "<elementos>" + SALTO;
            texto += TAB + TAB + "<elemento>" + SALTO;
            texto += TAB + TAB + TAB + "<nombre>var1</nombre>" + SALTO;
            texto += TAB + TAB + TAB + "<tipo>System.Int32</tipo>" + SALTO;
            texto += TAB + TAB + TAB + "<tipoDeElemento>propiedad</tipoDeElemento>" + SALTO;

            // Identificar el tipo de elemento
            FieldInfo p = c.GetType().GetField("var1");
            Type t = p.FieldType;
            bool isArray = t.IsArray;
            Object valor = p.GetValue(c) as Object;

            texto += TAB + TAB + TAB + "<isArray>" + isArray + "</isArray>" + SALTO;
            texto += TAB + TAB + TAB + "<valor>" + SALTO;

            texto += getValue(valor);
 
            texto += TAB + TAB + TAB + "</valor>" + SALTO;
            texto += TAB + TAB + "</elemento>" + SALTO;
            texto += TAB + "</elementos>" + SALTO;
            texto += "</serializacion>" + SALTO;
            return texto;
        }

        private static string codificarArray(Array c)
        {
            string codigo = "";

            codigo += TAB + TAB + TAB + TAB + "<count>";
            codigo += c.Length;
            codigo += "</count>" + SALTO;
/*
            codigo += TAB + TAB + TAB + TAB + "<tipoDeElementos>";
            codigo += c.GetType().GetElementType().Name;
            codigo += "</tipoDeElementos>" + SALTO;
*/
            codigo += TAB + TAB + TAB + TAB + "<rank>";
            codigo += c.Rank;
            codigo += "</rank>" + SALTO;

            codigo += TAB + TAB + TAB + TAB + "<datosDeLosRangos>" + SALTO;

            for(int i=0; i< c.Rank; i++)
            {
                codigo += TAB + TAB + TAB + TAB + TAB + "<datosDeRango>" + SALTO;

                codigo += TAB + TAB + TAB + TAB + TAB + TAB + "<longitud>";
                codigo += c.GetLength(i);
                codigo += "</longitud>" + SALTO;

                codigo += TAB + TAB + TAB + TAB + TAB + TAB + "<valorMenor>";
                codigo += c.GetLowerBound(i);
                codigo += "</valorMenor>" + SALTO;

                codigo += TAB + TAB + TAB + TAB + TAB + "</datosDeRango>" + SALTO;
            }
            codigo += TAB + TAB + TAB + TAB + "</datosDeLosRangos>" + SALTO;

            codigo += TAB + TAB + TAB + TAB + "<valores>" + SALTO;
            foreach (object elemento in c)
            {
                codigo += TAB + TAB + TAB + TAB + TAB + "<cadaValor>" + SALTO;
                codigo += TAB + TAB + TAB + TAB + TAB + TAB + "<valor>";
                    // Aquí hay que ver si el objeto es ""simple"" o es una estructura (clase, list, array, etc.)
                codigo += elemento.ToString();
                codigo += "</valor>" + SALTO;
                codigo += TAB + TAB + TAB + TAB + TAB + "</cadaValor>" + SALTO;
            }
            codigo += TAB + TAB + TAB + TAB + "</valores>" + SALTO;
            return codigo;
        }

        private static string getValue(Object c)
        {
            string texto = "";
            Type t = c.GetType();

            if (t.IsPrimitive || t.Name == "String") // Datos primitivos, simplemente cogemos su valor
            {
                texto += TAB + TAB + TAB + TAB + c.ToString() + SALTO;
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
                Array aux = c as Array;

                // Generar codificador para el tipo y codificarlo
                texto += codificarArray(aux);
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.List`")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.Dictionary`")) // Dictionary (o hashtable), se codifica con sus claves y valores
            {
            }
            else if (!t.FullName.StartsWith("System.")) // OBJETOS EXTENOS
            {
                // Hay que generar o invocar el serializador para esta clase
                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
            }

            return texto;

        }

    }
}
