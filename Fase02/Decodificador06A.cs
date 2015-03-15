using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador06A : DecodificadorBaseA
    {
        //falta por probar la herencia y la agregación (clase que contiene otra clase)
        //también falta listas genéricas, ver como se pueden trabajar con ellas de manera genérica

//        public override Object decode(Stream aux)
        public override Object decode(int v1, string v2)
        {
            Clase06ClaseDerivada c = new Clase06ClaseDerivada();
            c.var1 = v1;
            c.var2 = v2;
            c.var3 = v1;

            return c;
        }
    }
}
