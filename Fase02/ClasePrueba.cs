using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase02
{
    public class ClaseB
    {
        public int varB1 { get; set; }
        public ClaseB()
        {
            varB1 = 1;
        }
    }

    public class ClasePrueba
    {
        //            [NonSerialized]
        public int var1;
        public ClaseB var2 { get; set; }
        public string var3;

        public ClasePrueba()
        {
            var1 = 33;
            ClaseB b = new ClaseB();
            b.varB1 = 200;
            var2 = b;
            var3 = "adios";
        }
    }
}
