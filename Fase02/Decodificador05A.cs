﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador05A
    {
        public Clase05Clase decode(string aux)
        {
            int v1 = 0;
            string v2 = "";

            String[] parametros = aux.Split(',');
            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];

            Clase05Clase c = new Clase05Clase();

            c.var3.var1 = v1;
            c.var3.var2 = v2;

            return c;
        }
    }
}
