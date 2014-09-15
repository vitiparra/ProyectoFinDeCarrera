using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador03A : DecodificadorBaseA
    {
//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Clase03Array c = new Clase03Array();
            c.var1 = new int[v1];
            for (int i = 0; i < v1; i++)
            {
                c.var1[i] = v1;
            }
            c.var2 = new string[Convert.ToInt16(v2)];
            for (int i = 0; i < Convert.ToInt16(v2); i++)
            {
                c.var2[i] = v2;
            }

            return c;
        }
    }
}
