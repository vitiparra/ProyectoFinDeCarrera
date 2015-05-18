using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public static partial class SerializerStatic
    {
        public static String encode (Clase01Basica obj)
            /*
             * Generar al vuelo una clase con estos dos métodos, encode y decode
             * Esta clase recibirá como parámetro la clase que se quiere serializar y deserializar
             * Reconocerá (con reflexion) los atributos públicos que tenga la clase
             * para cada atributo se generará el código que lo serializa/deserializa
             * 
             * Luego, se prueba con clases al azar, primero mostrando lo que hace en pantalla
             * después generando un fichero con el código que se va generando
             * por último, usar Mina para enviar a un buffer el código generado
             */

        {   // Mina.net que tiene buffers
            // IoBuffer buffer = ??.Allocate(size);
            // buffer.Put(obj.var1);
            // buffer.Put(obj.var2);
            // serializo obj.var1 como entero
            // serializo obj.var2 como string
    //        throw new System.NotImplementedException();
            return "";
//            return string.Format("{0},{1}", obj.var1, obj.var2);
        }
        public static void decode(ref Clase01Basica c, String s)
        {
            int v1 = 0;
            string v2 = "";

            String aux = s.ToString();
            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 2)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = parametros[1];
            }
            /*
            c.var1 = v1;
            c.var2 = v2;
             */ 
        }
    }
    
    public static partial class SerializerStatic
    {
        public static String encode(Clase02ArrayNormal obj)
        {
            return string.Format("{0},{1}", obj.var1.Length, obj.var2.Length);
        }

        public static String encode (Clase03Array c)
        {
            int ele1 = 0;
            int numEle1 = 0;

            String ele2 = "";
            int numEle2 = 0;

            foreach (int elemento in c.var1)
            {
                ele1 = elemento;
                numEle1++;
            }

            foreach (String elemento in c.var2)
            {
                ele2 = elemento;
                numEle2++;
            }
            /*
            foreach (int elemento in c.var3)
            {
                ele2 = elemento.ToString();
                numEle2++;
            }

            foreach (int elemento in c.var4)
            {
                ele2 = elemento.ToString();
                numEle2++;
            }

            foreach (int[] elemento in c.var5)
            {
                foreach (int elemento2 in elemento)
                {
                    ele2 = elemento2.ToString();
                    numEle2++;
                }
            }
            */
            return string.Format("{0},{1},{2},{3}", ele1, numEle1, ele2, numEle2);
        }

        public static String encode(Clase04Struct c)
        {
            return string.Format("{0},{1}", c.valor3.valor1, c.valor3.valor2);
        }

        public static String encode(Clase05Clase c)
        {
            return string.Format("{0},{1}", c.var3.var1, c.var3.var2);
        }

        public static String encode(Clase06ClaseDerivada c)
        {
            return string.Format("{0},{1},{2}", c.var1, c.var2, c.var3);
        }

        public static String encode(Clase07ClaseConTodo c)
        {
            int ele1 = 0;
            int numEle1 = 0;

            String ele2 = "";
            int numEle2 = 0;

            ele2 += string.Format("{0},{1}", c.publicInt, c.basePublicInt);

            foreach (int elemento in c.lista)
            {
                ele1 += elemento;
                numEle1++;
            }

            foreach (int elemento in c.publicArrayInt)
            {
                ele2 += elemento;
                numEle2++;
            }

            foreach (int elemento in c.publicArray2DInt)
            {
                ele2 += elemento;
                numEle2++;
            }

            foreach (int[] elemento in c.publicArrayMatrizEscalonadaInt)
            {
                foreach (int elemento2 in elemento)
                {
                    ele2 += elemento2;
                    numEle2++;
                }
            }

            return string.Format("{0},{1},{2},{3}", ele1, numEle1, ele2, numEle2);
        }


        public static void decode(ref Clase02ArrayNormal c, String aux)
        {
            int v1 = 0;
            string v2 = "";

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 2)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = parametros[1];
            }
        }

        public static void decode(ref Clase03Array c, String aux)
        {
            int v1 = 0;
            int numEle1 = 0;
            string v2 = "";
            int numEle2 = 0;

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 4)
            {
                v1 = Convert.ToInt16(parametros[0]);
                numEle1 = Convert.ToInt16(parametros[1]);
                v2 = parametros[2];
                numEle2 = Convert.ToInt16(parametros[3]);
            }

            c.var1 = new int[numEle1];
            for (int i = 0; i < numEle1; i++)
            {
                c.var1[i] = i;
            }

            c.var2 = new string[numEle2];
            for (int i = 0; i < numEle2; i++)
            {
                c.var2[i] = Convert.ToString(i);
            }
        }

        public static void decode(ref Clase04Struct c, String aux)
        {
            int v1 = 0;
            string v2 = "";

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 2)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = parametros[1];
            }

            c.valor3.valor1 = v1;
            c.valor3.valor2 = v2;
        }

        public static void decode(ref Clase05Clase c, String aux)
        {
            int v1 = 0;
            string v2 = "";

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 2)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = parametros[1];
            }

            c.var3.var1 = v1;
            c.var3.var2 = v2;
        }

        public static void decode(ref Clase06ClaseDerivada c, String aux)
        {
            int v1 = 0;
            string v2 = "";
            int v3 = 0;

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 3)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = parametros[1];
                v3 = Convert.ToInt16(parametros[2]);
            }

            c.var1 = v1;
            c.var2 = v2;
            c.var3 = v1;
        }

        public static void decode(ref Clase07ClaseConTodo c, String aux)
        {
            int v1 = 0;
            int v2 = 0;
            int numEle1 = 0;
            int numEle2 = 0;

            String[] parametros = aux.Split(',');

            // Se esperan dos parámetros: un int y un string
            if (parametros.Length == 4)
            {
                v1 = Convert.ToInt16(parametros[0]);
                v2 = Convert.ToInt16(parametros[1]);
                numEle1 = Convert.ToInt16(parametros[2]);
                numEle2 = Convert.ToInt16(parametros[3]);
            }

            c.basePublicInt = v1;
            c.publicInt = v2;
            c.publicArrayInt = new int[numEle1];
            for(int i = 0; i < numEle1; i++)
            {
                c.publicArrayInt[i] = numEle1;
            }

            c.publicArray2DInt = new int[numEle1, numEle2];
            for(int i = 0; i < numEle1; i++)
            {
                for(int j = 0; j < numEle2; j++)
                {
                    c.publicArray2DInt[i, j] = numEle2;
                }
            }

            c.publicArrayMatrizEscalonadaInt = new int[numEle1][];
            for(int i = 0; i < numEle1; i++)
            {
                int[] arrayAux = new int[numEle2];
                for(int j = 0; j < numEle2; j++)
                {
                    arrayAux[j] = numEle2;
                }
                c.publicArrayMatrizEscalonadaInt[i] = arrayAux;
            }

            c.lista = new List<int>();
            c.lista.Add(numEle1);
            c.lista.Add(numEle2);
        }
    }
}
