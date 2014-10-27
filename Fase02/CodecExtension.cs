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
        public static string codificar(this Object obj)
        {
            if(obj.GetType() == typeof(Fase02.Clase01Basica))
            {
                Fase02.Clase01Basica c = (Fase02.Clase01Basica)obj;
                return "var1=" + c.var1 + "&var2=" + c.var2;
            }
            else if(obj.GetType() == typeof(Fase02.Clase02Metodos))
            {
                Fase02.Clase02Metodos c = (Fase02.Clase02Metodos)obj;
                return "met1=" + c.metodo1() + "&met2=" + c.metodo2();
            }
            else if(obj.GetType() == typeof(Fase02.Clase03Array))
            {
                Fase02.Clase03Array c = (Fase02.Clase03Array)obj;
                string texto = "var1=[";
                foreach (int elemento in c.var1)
                {
                    texto += elemento + ",";
                }
                texto = texto.Substring(0, texto.Length - 1);
                texto += "]&var2=[";
                foreach (string elemento in c.var2)
                {
                    texto += elemento + ",";
                }
                texto = texto.Substring(0, texto.Length - 1);
                texto += "]";

                return texto;
            }
            else if(obj.GetType() == typeof(Fase02.Clase04Struct))
            {
                Fase02.Clase04Struct c = (Fase02.Clase04Struct)obj;
                return "var1=" + c.valor3.valor1 + "&var2=" + c.valor3.valor2;
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
            else
            {
                return "";
            }
        }

        public static Object decodificar1(this System.IO.Stream str, Object obj)
        {
            if (obj.GetType() == typeof(Fase02.Clase01Basica))
            {
                Fase02.Clase01Basica c = (Fase02.Clase01Basica)obj;
                return "var1=" + c.var1 + "&var2=" + c.var2;
            }
            else if (obj.GetType() == typeof(Fase02.Clase02Metodos))
            {
                Fase02.Clase02Metodos c = (Fase02.Clase02Metodos)obj;
                return "met1=" + c.metodo1() + "&met2=" + c.metodo2();
            }
            else if (obj.GetType() == typeof(Fase02.Clase03Array))
            {
                Fase02.Clase03Array c = (Fase02.Clase03Array)obj;
                string texto = "var1=[";
                foreach (int elemento in c.var1)
                {
                    texto += elemento + ",";
                }
                texto = texto.Substring(0, texto.Length - 1);
                texto += "]&var2=[";
                foreach (string elemento in c.var2)
                {
                    texto += elemento + ",";
                }
                texto = texto.Substring(0, texto.Length - 1);
                texto += "]";

                return texto;
            }
            else if (obj.GetType() == typeof(Fase02.Clase04Struct))
            {
                Fase02.Clase04Struct c = (Fase02.Clase04Struct)obj;
                return "var1=" + c.valor3.valor1 + "&var2=" + c.valor3.valor2;
            }
            else if (obj.GetType() == typeof(Fase02.Clase05Clase))
            {
                Fase02.Clase05Clase c = (Fase02.Clase05Clase)obj;
                return "var1=" + c.var3.var1 + "&var2=" + c.var3.var2;
            }
            else
            {
                return "";
            }
        }

    }
}