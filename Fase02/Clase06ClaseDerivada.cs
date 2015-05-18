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
    public class ClaseBase
    {
        [ProtoMember(1)]
        public int var1 { get; set; }

        [ProtoMember(2)]
        public string var2 { get; set; }
    }

    [ProtoContract]
    [Serializable]
    public class Clase06ClaseDerivada : ClaseBase
    {
        [ProtoMember(1)]
        public int var3 { get; set; }


        public Clase06ClaseDerivada()
        {
            this.var1 = 0;
            this.var2 = "";
            this.var3 = 0;
        }

        public Clase06ClaseDerivada(int v1, string v2, int v3)
        {
            this.var1 = v1;
            this.var2 = v2;
            this.var3 = v3;
        }
    }
}
