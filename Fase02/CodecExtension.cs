using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodecExtensions
{
    //Extension methods must be defined in a static class
    public static class CodecExtension
    {
        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static String codificar(this Object obj)
        {
            if(obj.GetType() == typeof(Fase02.Clase01Basica))
            {
                Fase02.Clase01Basica c = (Fase02.Clase01Basica)obj;
                return string.Format("{0},{1}", c.var1, c.var2);
            }
            else if (obj.GetType() == typeof(Fase02.Clase02ArrayNormal))
            {
                Fase02.Clase02ArrayNormal c = (Fase02.Clase02ArrayNormal)obj;
                return string.Format("{0},{1}", c.var1.Length, c.var2.Length);
            }
            else if(obj.GetType() == typeof(Fase02.Clase03Array))
            {
                Fase02.Clase03Array c = (Fase02.Clase03Array)obj;

                int ele1 = 0;
                int numEle1 = 0;

                String ele2 = "";
                int numEle2 = 0;

                foreach (int elemento in c.var1)
                {
                    ele1 = elemento;
                    numEle1++;
                }
                /*
                foreach (string elemento in c.var2)
                {
                    ele2 = elemento;
                    numEle2++;
                }
                */
                return string.Format("{0},{1},{2},{3}", ele1, c.var1.Length, ele2, c.var2.Length);
            }
            else if(obj.GetType() == typeof(Fase02.Clase04Struct))
            {
                Fase02.Clase04Struct c = (Fase02.Clase04Struct)obj;
                return string.Format("{0},{1}", c.valor3.valor1, c.valor3.valor2);
            }
            else if (obj.GetType() == typeof(Fase02.Clase05Clase))
            {
                Fase02.Clase05Clase c = (Fase02.Clase05Clase)obj;
                return "var1=" + c.var3.var1 + "&var2=" + c.var3.var2;
            }
            else if (obj.GetType() == typeof(Fase02.Clase06ClaseDerivada))
            {
                Fase02.Clase06ClaseDerivada c = (Fase02.Clase06ClaseDerivada)obj;
                return "var1=" + c.var1 + "&var2=" + c.var2 + "&var3=" + c.var3;
            }
            else if (obj.GetType() == typeof(Fase02.Struct01Basica))
            {
                Fase02.Struct01Basica c = (Fase02.Struct01Basica)obj;
                return "var1=" + c.var1 + "&var2=" + c.var2;
            }
            else
            {
                return "";
            }
        }

//        public static Object decodificar(this System.IO.Stream str, Type tipo)
        public static Object decodificar(this String str, Type tipo)
        {
            Object obj = null;

            if (tipo == typeof(Fase02.Clase01Basica))
            {
                Fase02.Clase01Basica c = new Fase02.Clase01Basica();
                int v1 = 0;
                string v2 = "";

                String aux = str.ToString();
                String[] parametros = aux.Split(',');

                // Se esperan dos parámetros: un int y un string
                if (parametros.Length == 2)
                {
                    v1 = Convert.ToInt16(parametros[0]);
                    v2 = parametros[1];
                }
                else
                {
                    v1 = 0;
                    v2 = "";
                }

                c.var1 = v1;
                c.var2 = v2;

                obj = c;
            }
            else if (tipo == typeof(Fase02.Clase02ArrayNormal))
            {
                Fase02.Clase02ArrayNormal c = new Fase02.Clase02ArrayNormal();
                int v1 = 0;
                string v2 = "";

                String aux = str.ToString();
                String[] parametros = aux.Split('&');

                // Se esperan dos parámetros: un int y un string
                if (parametros.Length == 2)
                {
                    String par;
                    String[] param;
                    par = parametros[0];
                    param = par.Split('=');
                    if (param.Length == 2 && param[0] == "v1")
                    {
                        v1 = Convert.ToInt16(param[1]);

                        par = parametros[1];
                        param = par.Split('=');
                        if (param.Length == 2 && param[0] == "v2")
                        {
                            v2 = param[1];
                        }
                    }
                }

                obj = c;
            }
            else if (tipo == typeof(Fase02.Clase03Array))
            {
                Fase02.Clase03Array c = new Fase02.Clase03Array();
                int v1 = 0;
                int numEle1 = 0;
                string v2 = "";
                int numEle2 = 0;

                String aux = str.ToString();
                String[] parametros = aux.Split(',');

                // Se esperan cuatro parámetros: un int, un número de elementos int, un string y un número de elementos string
                if (parametros.Length == 4)
                {
                    v1 = Convert.ToInt16(parametros[0]);
                    numEle1 = Convert.ToInt16(parametros[1]);
                    v2 = parametros[2];
                    numEle2 = Convert.ToInt16(parametros[3]);

                    c.var1 = new int[numEle1];
                    for (int i = 0; i < numEle1; i++)
                    {
                        c.var1[i] = i;
                    }

                    c.var2 = new string[numEle2];
                    for (int i = 0; i < 2; i++)
                    {
                        c.var2[i] = Convert.ToString(i);
                    }
                }
                else
                {
                    c.var1 = new int[numEle1];
                    c.var2 = new string[numEle2];
                }

                obj = c;

            }
            else if (tipo == typeof(Fase02.Clase04Struct))
            {
                Fase02.Clase04Struct c = new Fase02.Clase04Struct();
                int v1 = 0;
                string v2 = "";

                String aux = str.ToString();
                String[] parametros = aux.Split(',');

                // Se esperan dos parámetros: un int y un string
                if (parametros.Length == 2)
                {
                    v1 = Convert.ToInt16(parametros[0]);
                    v2 = parametros[1];
                }
                else
                {
                    v1 = 0;
                    v2 = "";
                }

                c.valor3.valor1 = v1;
                c.valor3.valor2 = v2;

                obj = c;
            }
/*
            else if (obj.GetType() == typeof(Fase02.Clase05Clase))
            {
                Fase02.Clase05Clase c = (Fase02.Clase05Clase)obj;
                return "var1=" + c.var3.var1 + "&var2=" + c.var3.var2;
            }
            else
            {
                return "";
            }
 */
            return obj;
        }
    }
}