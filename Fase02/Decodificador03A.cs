using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador03A
    {
//        public override Object decode(Stream aux)
        public Clase03Array decode(String s)
        {
            int v1 = 0;
            int numEle1 = 0;
            string v2 = "";
            int numEle2 = 0;

            Clase03Array cOut = new Clase03Array();
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

            cOut.var1 = new int[numEle1];
            for (int i = 0; i < numEle1; i++)
            {
                cOut.var1[i] = i;
            }

            cOut.var2 = new string[numEle2];
            for (int i = 0; i < numEle2; i++)
            {
                cOut.var2[i] = Convert.ToString(i);
            }

            return cOut;
        }
    }
}
