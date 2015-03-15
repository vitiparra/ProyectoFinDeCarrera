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
            return string.Format("{0},{1}", obj.var1, obj.var2);
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

            c.var1 = v1;
            c.var2 = v2;
        }
    }
    
    public static partial class SerializerStatic
    {
        public static String encode(Clase02Metodos obj)
        {
            return string.Format("{0},{1}", obj.metodo1(), obj.metodo2());
        }

        public static String encode (Clase03Array obj)
        {
            return string.Format("{0},{1},{2},{3}", obj.var1[obj.var1.Length - 1], obj.var1.Length, obj.var2[obj.var2.Length - 1], obj.var2.Length);
        }

        public static String encode(Clase04Struct obj)
        {
            return string.Format("{0},{1}", obj.valor3.valor1, obj.valor3.valor2);
        }


        

        public static void decode(ref Clase02Metodos c, String s)
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

            c.metodo1();
            c.metodo2();
        }

        public static void decode(ref Clase03Array c, String s)
        {
            int v1 = 0;
            int numEle1 = 0;
            string v2 = "";
            int numEle2 = 0;

            String aux = s.ToString();
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

        public static void decode(ref Clase04Struct c, String s)
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

            c.valor3.valor1 = v1;
            c.valor3.valor2 = v2;
        }

        /*
                        static tipo decode(obj)
                        {
                            //
                        }
                 */ 
    }
}
