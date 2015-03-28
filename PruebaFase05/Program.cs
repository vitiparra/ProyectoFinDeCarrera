using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Fase02;

namespace PruebaFase05
{
    class Program
    {
        static void Main(string[] args)
        {
            Fase02.Clase01Basica c = new Clase01Basica();
            c.var1 = 1;
            c.var2 = "hola";
            string texto = Codificador.encode(c);
            Console.WriteLine(texto);
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
            texto += TAB + TAB + TAB + "<tipoDeObjeto>¿Int32?</tipoDeObjeto>" + SALTO;
            texto += TAB + TAB + TAB + "<tipoDeElemento>propiedad</tipoDeElemento>" + SALTO;
            texto += TAB + TAB + TAB + "<tipo>System.Int32</tipo>" + SALTO;
            texto += TAB + TAB + TAB + "<isArray>False</isArray>" + SALTO;
            texto += TAB + TAB + TAB + "<valor>" + SALTO;
            texto += TAB + TAB + TAB + TAB + c.GetType().GetProperty("var1").GetValue(c, null) + SALTO;
            texto += TAB + TAB + TAB + "</valor>" + SALTO;
            texto += TAB + TAB + "</elemento>" + SALTO;
            texto += TAB + "</elementos>" + SALTO;
            texto += "</serializacion>" + SALTO;
            return texto;
        }
    }
}
