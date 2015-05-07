using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    [Serializable]
    public class Clase01Basica
    {
//        public Wrapper<int> val { get; set; } // se espera que isGenericçType devuelva FALSE
        public int var1 { get; set;}
        public string var2 { get; set; }
    }

    public class Wrapper<T>
    {
        public T Value { get; set; } // se espera que isGenericçType devuelva TRUE
    }
}
