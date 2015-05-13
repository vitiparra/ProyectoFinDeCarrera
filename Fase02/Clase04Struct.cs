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
    public class Clase04Struct
    {
        [Serializable]
        public struct estructura
        {
            [ProtoMember(1)]
            public int valor1;

            [ProtoMember(2)]
            public string valor2;

            public estructura(int v1, string v2)
            {
                valor1 = v1;
                valor2 = v2;
            }
        }

        [ProtoMember(1)]
        public estructura valor3;
    }
}
