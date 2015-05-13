using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace Fase02
{
    [ProtoContract]
    [Serializable]
    public class Clase05Clase
    {
        [Serializable]
        public class ClaseInterna
        {
            [ProtoMember(1)]
            public int var1 { get; set; }
            [ProtoMember(2)]
            public string var2 { get; set; }            
        }

        [ProtoMember(1)]
        public ClaseInterna var3 { get; set; }

        public Clase05Clase()
        {
            var3 = new ClaseInterna();
        }
    }
}
