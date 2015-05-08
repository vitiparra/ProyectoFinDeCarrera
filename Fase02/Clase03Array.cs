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
    public class DentroDelArray
    {
        public int uno;
        public string dos;
    }

    [Serializable]
    [ProtoContract]
    public class Clase03Array
    {
        [ProtoMember(1)]
        public int[] var1 { get; set; }
        [ProtoMember(2)]
        public string[] var2 { get; set; }
        [ProtoMember(3)]
        public int[,] var3 { get; set; }
        [ProtoMember(4)]
        public int[, ,] var4 { get; set; }
        [Obsolete]
        [ProtoMember(5)]
        public int[][] var5 { get; set; }
        [ProtoMember(6)]
        public Dictionary<string, int> var6 { get; set; }
        [ProtoMember(7)]
        public DentroDelArray[] var7;
    }
}
