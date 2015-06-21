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
        [ProtoMember(1)]
        public string v1 = "1";
        [ProtoMember(2)]
        public int v2 = 2;
   }
}
