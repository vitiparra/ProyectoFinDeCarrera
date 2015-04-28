
using System;
using System.Xml;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

namespace Serializer
{

    public class Clase03ArrayCodec
    {
        public static Type tipo = Type.GetType("Clase03Array");
        public static String nombreClase = "Clase03Array";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.Clase03Array obj)
        {
            return encode(obj);
        }
        public void decodificar(string codigo, ref Fase02.Clase03Array obj)
        {
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.Clase03Array obj)
        {
            StringBuilder texto = new StringBuilder();
            //            texto.Append("var1,");
            //            texto.Append("System.Int32[],");
            texto.Append(obj.var1.Length + ",");
            texto.Append("System.Int32" + ",");
            texto.Append("1" + ",");
            texto.Append(obj.var1.GetLength(0) + ",");
            texto.Append(obj.var1.GetLowerBound(0) + ",");
            texto.Append(obj.var1.GetUpperBound(0) + ",");
            foreach (System.Int32 elementoAuxobjvar1 in obj.var1)
            {
                texto.Append(elementoAuxobjvar1.ToString() + ",");
            }
            //            texto.Append("var2,");
            //            texto.Append("System.String[],");
            texto.Append(obj.var2.Length + ",");
            texto.Append("System.String" + ",");
            texto.Append("1" + ",");
            texto.Append(obj.var2.GetLength(0) + ",");
            texto.Append(obj.var2.GetLowerBound(0) + ",");
            texto.Append(obj.var2.GetUpperBound(0) + ",");
            foreach (System.String elementoAuxobjvar2 in obj.var2)
            {
                texto.Append(elementoAuxobjvar2.ToString() + ",");
            }
            //            texto.Append("var3,");
            //            texto.Append("System.Int32[,],");
            texto.Append(obj.var3.Length + ",");
            texto.Append("System.Int32" + ",");
            texto.Append("2" + ",");
            texto.Append(obj.var3.GetLength(0) + ",");
            texto.Append(obj.var3.GetLowerBound(0) + ",");
            texto.Append(obj.var3.GetUpperBound(0) + ",");
            texto.Append(obj.var3.GetLength(1) + ",");
            texto.Append(obj.var3.GetLowerBound(1) + ",");
            texto.Append(obj.var3.GetUpperBound(1) + ",");
            foreach (System.Int32 elementoAuxobjvar3 in obj.var3)
            {
                texto.Append(elementoAuxobjvar3.ToString() + ",");
            }
            //            texto.Append("var4,");
            //            texto.Append("System.Int32[,,],");
            texto.Append(obj.var4.Length + ",");
            texto.Append("System.Int32" + ",");
            texto.Append("3" + ",");
            texto.Append(obj.var4.GetLength(0) + ",");
            texto.Append(obj.var4.GetLowerBound(0) + ",");
            texto.Append(obj.var4.GetUpperBound(0) + ",");
            texto.Append(obj.var4.GetLength(1) + ",");
            texto.Append(obj.var4.GetLowerBound(1) + ",");
            texto.Append(obj.var4.GetUpperBound(1) + ",");
            texto.Append(obj.var4.GetLength(2) + ",");
            texto.Append(obj.var4.GetLowerBound(2) + ",");
            texto.Append(obj.var4.GetUpperBound(2) + ",");
            foreach (System.Int32 elementoAuxobjvar4 in obj.var4)
            {
                texto.Append(elementoAuxobjvar4.ToString() + ",");
            }
            //            texto.Append("var6,");
            //            texto.Append("System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],");
            //            IDictionary auxobjvar6 = obj.var6 as IDictionary;
            texto.Append(obj.var6.Count + ",");
            texto.Append("System.String" + ",");
            texto.Append("System.Int32" + ",");
            foreach (KeyValuePair<System.String, System.Int32> parobjvar6 in obj.var6)
            {
                {
                    System.String elementoAuxobjvar6 = parobjvar6.Key;
                    texto.Append(elementoAuxobjvar6.ToString() + ",");
                }
                {
                    System.Int32 elementoAuxobjvar6 = parobjvar6.Value;
                    texto.Append(elementoAuxobjvar6.ToString() + ",");
                }
            }
            //            texto.Append("var7,");
            //            texto.Append("Fase02.DentroDelArray[],");
            texto.Append(obj.var7.Length + ",");
            texto.Append("Fase02.DentroDelArray" + ",");
            texto.Append("1" + ",");
            texto.Append(obj.var7.GetLength(0) + ",");
            texto.Append(obj.var7.GetLowerBound(0) + ",");
            texto.Append(obj.var7.GetUpperBound(0) + ",");
            foreach (Fase02.DentroDelArray elementoAuxobjvar7 in obj.var7)
            {
                texto.Append(DentroDelArrayCodec.encode(elementoAuxobjvar7));
            }
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase03Array obj)
        {
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar1 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
            rango = Int32.Parse(elementos.Dequeue());
            int auxobjvar1Length0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar1GetLowerBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar1GetUpperBound0 = Int32.Parse(elementos.Dequeue());
            for (int auxIndice0 = auxobjvar1GetLowerBound0; auxIndice0 <= auxobjvar1GetUpperBound0; auxIndice0++)
            {
                if (obj.var1 == null) obj.var1 = new System.Int32[auxobjvar1Length0];
                obj.var1[auxIndice0] = Int32.Parse(elementos.Dequeue());
            }
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar2 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
            rango = Int32.Parse(elementos.Dequeue());
            int auxobjvar2Length0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar2GetLowerBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar2GetUpperBound0 = Int32.Parse(elementos.Dequeue());
            for (int auxIndice0 = auxobjvar2GetLowerBound0; auxIndice0 <= auxobjvar2GetUpperBound0; auxIndice0++)
            {
                if (obj.var2 == null) obj.var2 = new System.String[auxobjvar2Length0];
                obj.var2[auxIndice0] = elementos.Dequeue();
            }
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar3 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
            rango = Int32.Parse(elementos.Dequeue());
            int auxobjvar3Length0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar3GetLowerBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar3GetUpperBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar3Length1 = Int32.Parse(elementos.Dequeue());
            int auxobjvar3GetLowerBound1 = Int32.Parse(elementos.Dequeue());
            int auxobjvar3GetUpperBound1 = Int32.Parse(elementos.Dequeue());
            for (int auxIndice0 = auxobjvar3GetLowerBound0; auxIndice0 <= auxobjvar3GetUpperBound0; auxIndice0++)
            {
                for (int auxIndice1 = auxobjvar3GetLowerBound1; auxIndice1 <= auxobjvar3GetUpperBound1; auxIndice1++)
                {
                    if (obj.var3 == null) obj.var3 = new System.Int32[auxobjvar3Length0, auxobjvar3Length1];
                    obj.var3[auxIndice0, auxIndice1] = Int32.Parse(elementos.Dequeue());
                }
            }
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar4 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
            rango = Int32.Parse(elementos.Dequeue());
            int auxobjvar4Length0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetLowerBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetUpperBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4Length1 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetLowerBound1 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetUpperBound1 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4Length2 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetLowerBound2 = Int32.Parse(elementos.Dequeue());
            int auxobjvar4GetUpperBound2 = Int32.Parse(elementos.Dequeue());
            for (int auxIndice0 = auxobjvar4GetLowerBound0; auxIndice0 <= auxobjvar4GetUpperBound0; auxIndice0++)
            {
                for (int auxIndice1 = auxobjvar4GetLowerBound1; auxIndice1 <= auxobjvar4GetUpperBound1; auxIndice1++)
                {
                    for (int auxIndice2 = auxobjvar4GetLowerBound2; auxIndice2 <= auxobjvar4GetUpperBound2; auxIndice2++)
                    {
                        if (obj.var4 == null) obj.var4 = new System.Int32[auxobjvar4Length0, auxobjvar4Length1, auxobjvar4Length2];
                        obj.var4[auxIndice0, auxIndice1, auxIndice2] = Int32.Parse(elementos.Dequeue());
                    }
                }
            }
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar6 = Int32.Parse(elementos.Dequeue());
            Type tipoIndice = Type.GetType(elementos.Dequeue());
            Type tipoValor = Type.GetType(elementos.Dequeue());
            type = Type.GetType(typeof(Dictionary<System.String, System.Int32>).AssemblyQualifiedName);
            IDictionary<System.String, System.Int32> dictAuxobjvar6 = (IDictionary<System.String, System.Int32>)Activator.CreateInstance(type);
            // Instanciación del miembro (si es una clase instanciable es imperativo)
            obj.var6 = new Dictionary<System.String, System.Int32>();
            for (int iauxobjvar6 = 0; iauxobjvar6 < lengthobjvar6; iauxobjvar6++)
            {
                System.String indiceAuxobjvar6 = "";
                {
                    System.String elementoAuxobjvar6 = "";
                    elementoAuxobjvar6 = elementos.Dequeue();
                    indiceAuxobjvar6 = elementoAuxobjvar6;
                }
                {
                    System.Int32 elementoAuxobjvar6 = new System.Int32();
                    elementoAuxobjvar6 = Int32.Parse(elementos.Dequeue());
                    obj.var6.Add(indiceAuxobjvar6, elementoAuxobjvar6);
                }
            }
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar7 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
            rango = Int32.Parse(elementos.Dequeue());
            int auxobjvar7Length0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar7GetLowerBound0 = Int32.Parse(elementos.Dequeue());
            int auxobjvar7GetUpperBound0 = Int32.Parse(elementos.Dequeue());
            for (int auxIndice0 = auxobjvar7GetLowerBound0; auxIndice0 <= auxobjvar7GetUpperBound0; auxIndice0++)
            {
                if (obj.var7 == null) obj.var7 = new Fase02.DentroDelArray[auxobjvar7Length0];
                Fase02.DentroDelArray objAuxobjvar7auxIndice0 = new Fase02.DentroDelArray();
                string aux = string.Join(",", elementos.ToArray());
                DentroDelArrayCodec.decode(ref aux, ref objAuxobjvar7auxIndice0);
                elementos = new Queue<string>(aux.Split(','));
                obj.var7[auxIndice0] = objAuxobjvar7auxIndice0 as Fase02.DentroDelArray;
            }
            codigo = string.Join(",", elementos.ToArray());
        }
    }
    public class DentroDelArrayCodec
    {
        public static Type tipo = Type.GetType("DentroDelArray");
        public static String nombreClase = "DentroDelArray";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.DentroDelArray obj)
        {
            return encode(obj);
        }
        public void decodificar(string codigo, ref Fase02.DentroDelArray obj)
        {
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.DentroDelArray obj)
        {
            StringBuilder texto = new StringBuilder();
            //            texto.Append("uno,");
            //            texto.Append("System.Int32,");
            texto.Append(obj.uno.ToString() + ",");
            //            texto.Append("dos,");
            //            texto.Append("System.String,");
            texto.Append(obj.dos.ToString() + ",");
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.DentroDelArray obj)
        {
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            obj.uno = Int32.Parse(elementos.Dequeue());
            //            nombre = elementos.Dequeue();
            //            tipo = Type.GetType(elementos.Dequeue());
            obj.dos = elementos.Dequeue();
            codigo = string.Join(",", elementos.ToArray());
        }

        static void Main(string[] args)
        {

            Fase02.Clase03Array c3a = new Fase02.Clase03Array();
            #region Datos Clase03Array
            c3a.var1 = new int[3];
            for (int i = 0; i < 3; i++)
            {
                c3a.var1[i] = i;
            }
                
            c3a.var2 = new string[3];
            for (int i = 0; i < 3; i++)
            {
                c3a.var2[i] = "Número " + Convert.ToString(i);
            }
                
            c3a.var3 = new int[1, 2];
            int cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    c3a.var3[i, j] = cont;
                    cont++;
                }
            }
                
            c3a.var4 = new int[1, 2, 3];
            cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        c3a.var4[i, j, k] = cont;
                        cont++;
                    }
                }
            }
                
            c3a.var5 = new int[3][];
            cont = 0;
            for (int i = 0; i < 3; i++)
            {
                int[] aux = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    aux[j] = cont;
                        cont++;
                }
                c3a.var5[i] = aux;
            }

            c3a.var6 = new Dictionary<string, int>();
            c3a.var6.Add("uno", 1);
            c3a.var6.Add("dos", 2);
            c3a.var6.Add("tres", 3);

                            #endregion

            Clase03ArrayCodec serializador1 = new Clase03ArrayCodec();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                string codigo = serializador1.codificar(c3a);
                Console.WriteLine(codigo);

                c3a = new Fase02.Clase03Array();
                serializador1.decodificar(codigo, ref c3a);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
            Console.ReadLine();
            return;

/*
            Fase02.Clase03Array c3 = new Fase02.Clase03Array();
            Clase03ArrayCodec serializador3 = new Clase03ArrayCodec();
            if (serializador3 != null)
            {
                c3.var1 = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var1[i] = i;
                }

                c3.var2 = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    c3.var2[i] = i.ToString();
                }

                c3.var4 = new int[3, 2, 2];
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            c3.var4[i, j, k] = i * (j + 3) + (k * 3);
                        }
                    }
                }
                /*
                                c3.var3 = new int[3][];
                                for (int i = 0; i < 3; i++)
                                {
                                    int[] aux = new int[4];
                                    for (int j = 0; j < 4; j++)
                                    {
                                        aux[j] = i * (j + 3);
                                    }
                                    c3.var3[i] = aux;
                                }
                                Console.WriteLine("Antes");
                                for (int i = 0; i < 3; i++)
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        Console.WriteLine(c3.var3[i][j]);
                                    }
                                }

                                c3.var4 = new List<Fase02.ClaseViti>();
                                Fase02.ClaseViti clase = new Fase02.ClaseViti();
                                clase.v1 = "uno";
                                clase.v2 = 1;
                                c3.var4.Add(clase);
                                Fase02.ClaseViti clase2 = new Fase02.ClaseViti();
                                clase2.v1 = "dos";
                                clase2.v2 = 2;
                                c3.var4.Add(clase2);
                                Fase02.ClaseViti clase3 = new Fase02.ClaseViti();
                                clase3.v1 = "tres";
                                clase3.v2 = 3;
                                c3.var4.Add(clase3);
                */
/*                c3.var6 = new Dictionary<string, int>();
                c3.var6.Add("cuatro", 4);
                c3.var6.Add("cinco", 5);
                c3.var6.Add("seis", 6);

                string codigo = serializador3.codificar(c3);
                Console.WriteLine(codigo);
                Fase02.Clase03Array c3aux = new Fase02.Clase03Array();
                serializador3.decodificar(codigo, ref c3aux);

                serializador3.ToString();
*/
                Console.ReadLine();
        }
    }
}
