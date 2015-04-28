using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public class DentroDelArray
    {
        public int uno;
        public string dos;
    }

    public class Clase03Array
    {
        public int[] var1 { get; set; }
        public string[] var2 { get; set; }
        public int[,] var3 { get; set; }
        public int[,,] var4 { get; set; }
        [Obsolete]
        public int[][] var5 { get; set; }
        public Dictionary<string, int> var6 { get; set; }
        public DentroDelArray[] var7;
    }
}
