using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ProtoBuf;

namespace Fase02
{
    [Serializable]
    [ProtoContract]
    public class Clase08List
    {
        [Serializable]
        [ProtoContract]
        public class clase
        {
            [ProtoMember(1)]
            public string v1;
            [ProtoMember(2)]
            public int v2;
        }

        [ProtoMember(1)]
        public List<int> var1 { get; set; }
        [ProtoMember(2)]
        public List<clase> var2 { get; set; }
        [ProtoMember(3)]
        public Dictionary<int, string> var3 { get; set; }
        [ProtoMember(4)]
        public Dictionary<int, clase> var4 { get; set; }
    }
}
