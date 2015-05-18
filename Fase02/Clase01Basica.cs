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
        public double v1 = 1.0;
        [ProtoMember(2)]
        public double v2 = 0.00000024321;
        [ProtoMember(3)]
        public double v3 = 3423423434.434;
        [ProtoMember(4)]
        public double v4 = .0;
   }

    public class Wrapper<T>
    {
        public T Value { get; set; } // se espera que isGenericçType devuelva TRUE
    }
}
