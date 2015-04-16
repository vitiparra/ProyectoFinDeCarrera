using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase02
{
    public class Decodificador05B
    {
        public void decode(ref Clase05Clase c, String aux)
        {
            int v1 = 0;
            string v2 = "";

            String[] parametros = aux.Split(',');
            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];

            c.var3.var1 = v1;
            c.var3.var2 = v2;
        }
    }
}
