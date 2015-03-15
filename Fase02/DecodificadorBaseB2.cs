using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public abstract class DecodificadorBaseB2
    {
//        public abstract Object decode(Stream aux);
        public abstract void decode(ref Object c, String s);
    }
}
