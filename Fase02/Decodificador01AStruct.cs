using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador01AStruct : DecodificadorBaseA2
    {
//        public override Object decode(Stream aux)
        public override Object decode(String s)
        {
            Struct01Basica str = new Struct01Basica();
            int v1 = 0;
            string v2 = "";

            String aux = s.ToString();
            String[] parametros = aux.Split(',');

            v1 = Convert.ToInt16(parametros[1]);
            v2 = parametros[1];

            str.var1 = v1;
            str.var2 = v2;

            return str;
        }
    }
}
