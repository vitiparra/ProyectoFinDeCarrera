using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase02
{
    public class Decodificador06B
    {
        public void decode(ref Clase06ClaseDerivada c, String aux)
        {
            int v1 = 0;
            string v2 = "";
            int v3 = 0;

            String[] parametros = aux.Split(',');
            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];
            v3 = Convert.ToInt16(parametros[2]);

            c.var1 = v1;
            c.var2 = v2;
            c.var3 = v3;
        }
    }
}
