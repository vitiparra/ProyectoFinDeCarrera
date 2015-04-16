using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase02
{
    class Codificador03A : CodificadorBaseA2
    {
        public override String encode(Object aux)
        {
            Clase03Array c = (Clase03Array)aux;
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
                ele2 = elemento.ToString();
                numEle2++;
            }

            foreach (int elemento in c.var4)
            {
                ele2 = elemento.ToString();
                numEle2++;
            }

            foreach (int[] elemento in c.var5)
            {
                foreach(int elemento2 in elemento)
                {
                    ele2 = elemento2.ToString();
                    numEle2++;
                }
            }
            */
            return string.Format("{0},{1},{2},{3}", ele1, numEle1, ele2, numEle2);
        }
    }
}
