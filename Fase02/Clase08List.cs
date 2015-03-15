using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase02
{
    public class Clase08List
    {
        public class clase
        {
            public string v1;
            public int v2;
        }

        public List<int> var1 { get; set; }
        public List<clase> var2 { get; set; }
        public Dictionary<int, string> var3 { get; set; }
        public Dictionary<int, clase> var4 { get; set; }
    }
}
