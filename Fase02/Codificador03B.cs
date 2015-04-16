using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador03B
    {
        public String encode(ref Clase03Array c)
        {
            int ele1 = 0;
            int numEle1 = 0;

            String ele2 = "";
            int numEle2 = 0;

            foreach (int elemento in c.var1)
            {
                ele1 = elemento;
                numEle1++;
            }
            /*
            foreach (String elemento in c.var2)
            {
                ele2 = elemento;
                numEle2++;
            }

            foreach (int elemento in c.var3)
            {
                numEle2 += elemento;
            }

            foreach (int elemento in c.var4)
            {
                numEle2 += elemento;
            }

            foreach (int[] elemento in c.var5)
            {
                foreach (int elemento2 in elemento)
                {
                    numEle2 += elemento2;
                }
            }
            */
            return string.Format("{0},{1},{2},{3}", ele1, numEle1, ele2, numEle2);

        }
    }
}
