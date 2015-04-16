using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    public class Clase05Clase
    {
        public class ClaseInterna
        {
            public int var1 { get; set; }
            public string var2 { get; set; }            
        }

        public ClaseInterna var3 { get; set; }

        public Clase05Clase()
        {
            var3 = new ClaseInterna();
        }
    }
}
