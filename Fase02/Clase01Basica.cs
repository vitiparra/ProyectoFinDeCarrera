using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProtoBuf;

namespace Fase02
{
    [Serializable]
    [ProtoContract]
    public class Clase01Basica
    {
//        public Wrapper<int> val { get; set; } // se espera que isGenericçType devuelva FALSE
        [ProtoMember(1)]
        public int var1 { get; set;}
        [ProtoMember(2)]
        public string var2 { get; set; }
    }

    public class Wrapper<T>
    {
        public T Value { get; set; } // se espera que isGenericçType devuelva TRUE
    }
}
