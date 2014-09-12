using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public abstract class DecodificadorBase
    {
//        public abstract Object decode(Stream aux);
        public abstract Object decode(int v1, string v2);
    }
}
