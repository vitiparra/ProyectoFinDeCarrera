using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public class Clase04Struct
    {
        public struct estructura
        {
            public int valor1;
            public string valor2;

            public estructura(int v1, string v2)
            {
                valor1 = v1;
                valor2 = v2;
            }
        }

        public estructura valor3;
    }
}
