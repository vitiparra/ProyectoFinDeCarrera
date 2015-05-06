using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fase02
{
    class Codificador07A
    {
        public String encode(ref Clase07ClaseConTodo c)
        {
            int ele1 = 0;
            int numEle1 = 0;

            String ele2 = "";
            int numEle2 = 0;

            ele2 = c.publicInt + "," + c.basePublicInt;

            foreach (int elemento in c.publicArrayInt)
            {
                ele1 += elemento;
                numEle1++;
            }

            foreach (int elemento in c.publicArray2DInt)
            {
                ele2 += elemento;
                numEle2++;
            }

            foreach (int[] elemento in c.publicArrayMatrizEscalonadaInt)
            {
                foreach (int elemento2 in elemento)
                {
                    ele2 += elemento2;
                    numEle2++;
                }
            }

            foreach (int elemento in c.lista)
            {
                ele2 += elemento;
                numEle2++;
            }

            return string.Format("{0},{1},{2},{3}", ele1, numEle1, ele2, numEle2);
        }
    }
}
