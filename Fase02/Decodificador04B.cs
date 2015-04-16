using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador04B
    {
//        public override Object decode(Stream aux)
        public void decode(ref Clase04Struct c, String s)
        {
            int v1 = 0;
            string v2 = "";

            Clase04Struct cOut = new Clase04Struct();
            String aux = s.ToString();
            String[] parametros = aux.Split(',');

            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];

            cOut.valor3.valor1 = v1;
            cOut.valor3.valor2 = v2;

            c = cOut;
        }
    }
}
