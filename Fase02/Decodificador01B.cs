using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01B
    {
        public void decode(ref Clase01Basica cOut, String s)
        {
            int v1 = 0;
            string v2 = "";

            String aux = s.ToString();
            String[] parametros = aux.Split(',');

            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];

/*            cOut.var1 = v1;
            cOut.var2 = v2;
*/        }
    }
}
