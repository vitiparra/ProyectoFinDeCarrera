﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Decodificador06A
    {
        //falta por probar la herencia y la agregación (clase que contiene otra clase)
        //también falta listas genéricas, ver como se pueden trabajar con ellas de manera genérica

        public Clase06ClaseDerivada decode(string aux)
        {
            int v1 = 0;
            string v2 = "";
            int v3 = 0;

            String[] parametros = aux.Split(',');
            v1 = Convert.ToInt16(parametros[0]);
            v2 = parametros[1];
            v3 = Convert.ToInt16(parametros[2]);

            Clase06ClaseDerivada c = new Clase06ClaseDerivada();

            c.var1 = v1;
            c.var2 = v2;
            c.var3 = v3;

            return c;
        }
    }
}
