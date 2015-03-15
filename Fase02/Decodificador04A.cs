using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador04A : DecodificadorBaseA2
    {
//        public override Object decode(Stream aux)
        public override Object decode(String s)
        {
            int v1 = 0;
            string v2 = "";

            String aux = s.ToString();
            String[] parametros = aux.Split(',');
            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];

            Clase04Struct c = new Clase04Struct();

            c.valor3.valor1 = v1;
            c.valor3.valor2 = v2;

            return c;
        }
    }
}
