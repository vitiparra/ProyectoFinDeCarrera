
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
            StringBuilder texto = new StringBuilder("<elementos>");
            if (obj.var1 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var1");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32[]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.var1.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("System.Int32");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("1");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var1.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var1.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var1.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (System.Int32 elementoAuxobjvar1 in obj.var1)
                {
                    texto.Append("<valor>");
                    texto.Append(elementoAuxobjvar1.ToString());
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.var2 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var2");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.String[]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.var2.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("System.String");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("1");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var2.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var2.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var2.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (System.String elementoAuxobjvar2 in obj.var2)
                {
                    texto.Append("<valor>");
                    texto.Append(elementoAuxobjvar2.ToString());
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.var3 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var3");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32[,]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.var3.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("System.Int32");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("2");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var3.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var3.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var3.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");

                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var3.GetLength(1));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var3.GetLowerBound(1));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var3.GetUpperBound(1));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (System.Int32 elementoAuxobjvar3 in obj.var3)
                {
                    texto.Append("<valor>");
                    texto.Append(elementoAuxobjvar3.ToString());
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.var4 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var4");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32[,,]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.var4.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("System.Int32");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("3");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var4.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var4.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var4.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");

                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var4.GetLength(1));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var4.GetLowerBound(1));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var4.GetUpperBound(1));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");

                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var4.GetLength(2));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var4.GetLowerBound(2));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var4.GetUpperBound(2));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (System.Int32 elementoAuxobjvar4 in obj.var4)
                {
                    texto.Append("<valor>");
                    texto.Append(elementoAuxobjvar4.ToString());
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.var6 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var6");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Collections.Generic.Dictionary`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                IDictionary auxobjvar6 = obj.var6 as IDictionary;
                texto.Append("<count>");
                texto.Append(obj.var6.Count);
                texto.Append("</count>");
                texto.Append("<tipoDeIndex>");
                texto.Append("System.String");
                texto.Append("</tipoDeIndex>");
                texto.Append("<tipoDeValue>");
                texto.Append("System.Int32");
                texto.Append("</tipoDeValue>");
                texto.Append("<valores>");
                foreach (KeyValuePair<System.String, System.Int32> parobjvar6 in obj.var6)
                {
                    texto.Append("<clave>");
                    {
                        System.String elementoAuxobjvar6 = parobjvar6.Key;
                        texto.Append(elementoAuxobjvar6.ToString());
                    }
                    texto.Append("</clave>");
                    {
                        System.Int32 elementoAuxobjvar6 = parobjvar6.Value;
                        texto.Append("<valor>");
                        texto.Append(elementoAuxobjvar6.ToString());
                    }
                    texto.Append("</valor>");
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.var7 == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("var7");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("Fase02.DentroDelArray[]");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append("<count>");
                texto.Append(obj.var7.Length);
                texto.Append("</count>");
                texto.Append("<tipoDeElementos>");
                texto.Append("Fase02.DentroDelArray");
                texto.Append("</tipoDeElementos>");
                texto.Append("<rank>");
                texto.Append("1");
                texto.Append("</rank>");
                texto.Append("<datosDeLosRangos>");
                texto.Append("<datosDeRango>");
                texto.Append("<longitud>");
                texto.Append(obj.var7.GetLength(0));
                texto.Append("</longitud>");
                texto.Append("<valorMenor>");
                texto.Append(obj.var7.GetLowerBound(0));
                texto.Append("</valorMenor>");
                texto.Append("<valorMayor>");
                texto.Append(obj.var7.GetUpperBound(0));
                texto.Append("</valorMayor>");
                texto.Append("</datosDeRango>");
                texto.Append("</datosDeLosRangos>");
                texto.Append("<valores>");
                foreach (Fase02.DentroDelArray elementoAuxobjvar7 in obj.var7)
                {
                    texto.Append("<valor>");
                    texto.Append(DentroDelArrayCodec.encode(elementoAuxobjvar7));
                    texto.Append("</valor>"); ;
                }
                texto.Append("</valores>");
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            texto.Append("</elementos>");
            //str = texto;
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase03Array obj)
        {
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            xml.LoadXml(codigo);
            XmlNode nodoPrincipal = xml.SelectSingleNode("elementos");
            XmlNodeReader nr = new XmlNodeReader(nodoPrincipal);
            nr.Read(); // Elementos
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjvar1 = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar1Length0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar1GetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar1GetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjvar1GetLowerBound0; auxIndice0 <= auxobjvar1GetUpperBound0; auxIndice0++)
                {
                    if (obj.var1 == null) obj.var1 = new System.Int32[auxobjvar1Length0];
                    nr.Read(); // Valor
                    nr.Read(); // Valor del campo o propiedad
                    obj.var1[auxIndice0] = Int32.Parse(nr.Value);
                    nr.Read(); // Valor
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjvar2 = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar2Length0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar2GetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar2GetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjvar2GetLowerBound0; auxIndice0 <= auxobjvar2GetUpperBound0; auxIndice0++)
                {
                    if (obj.var2 == null) obj.var2 = new System.String[auxobjvar2Length0];
                    nr.Read(); // Valor
                    nr.Read(); // Valor del campo o propiedad
                    obj.var2[auxIndice0] = nr.Value;
                    nr.Read(); // Valor
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjvar3 = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar3Length0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar3GetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar3GetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar3Length1 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar3GetLowerBound1 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar3GetUpperBound1 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjvar3GetLowerBound0; auxIndice0 <= auxobjvar3GetUpperBound0; auxIndice0++)
                {
                    for (int auxIndice1 = auxobjvar3GetLowerBound1; auxIndice1 <= auxobjvar3GetUpperBound1; auxIndice1++)
                    {
                        if (obj.var3 == null) obj.var3 = new System.Int32[auxobjvar3Length0, auxobjvar3Length1];
                        nr.Read(); // Valor
                        nr.Read(); // Valor del campo o propiedad
                        obj.var3[auxIndice0, auxIndice1] = Int32.Parse(nr.Value);
                        nr.Read(); // Valor
                    }
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjvar4 = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar4Length0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar4GetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar4GetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar4Length1 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar4GetLowerBound1 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar4GetUpperBound1 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar4Length2 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar4GetLowerBound2 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar4GetUpperBound2 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjvar4GetLowerBound0; auxIndice0 <= auxobjvar4GetUpperBound0; auxIndice0++)
                {
                    for (int auxIndice1 = auxobjvar4GetLowerBound1; auxIndice1 <= auxobjvar4GetUpperBound1; auxIndice1++)
                    {
                        for (int auxIndice2 = auxobjvar4GetLowerBound2; auxIndice2 <= auxobjvar4GetUpperBound2; auxIndice2++)
                        {
                            if (obj.var4 == null) obj.var4 = new System.Int32[auxobjvar4Length0, auxobjvar4Length1, auxobjvar4Length2];
                            nr.Read(); // Valor
                            nr.Read(); // Valor del campo o propiedad
                            obj.var4[auxIndice0, auxIndice1, auxIndice2] = Int32.Parse(nr.Value);
                            nr.Read(); // Valor
                        }
                    }
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read();
                int lengthobjvar6 = Int32.Parse(nr.Value);
                nr.Read(); // Count;
                nr.Read();// Tipo de index
                nr.Read();
                Type tipoIndiceobjvar6 = Type.GetType(nr.Value);
                nr.Read(); // Tipo de index
                nr.Read(); // Tipo de value
                nr.Read();
                Type tipoValorobjvar6 = Type.GetType(nr.Value);
                nr.Read(); // Tipo de value
                nr.Read(); // Valores
                type = Type.GetType(typeof(Dictionary<System.String, System.Int32>).AssemblyQualifiedName);
                IDictionary<System.String, System.Int32> dictAuxobjvar6 = (IDictionary<System.String, System.Int32>)Activator.CreateInstance(type);
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                obj.var6 = new Dictionary<System.String, System.Int32>();
                for (int iauxobjvar6 = 0; iauxobjvar6 < lengthobjvar6; iauxobjvar6++)
                {
                    nr.Read(); // Clave
                    System.String indiceAuxobjvar6 = "";
                    {
                        System.String elementoAuxobjvar6 = "";
                        nr.Read(); // Valor del campo o propiedad
                        elementoAuxobjvar6 = nr.Value;
                        nr.Read(); // Clave
                        indiceAuxobjvar6 = elementoAuxobjvar6;
                    }
                    {
                        System.Int32 elementoAuxobjvar6 = new System.Int32();
                        nr.Read(); // Valor
                        nr.Read(); // Valor del campo o propiedad
                        elementoAuxobjvar6 = Int32.Parse(nr.Value);
                        obj.var6.Add(indiceAuxobjvar6, elementoAuxobjvar6);
                        nr.Read(); // Valor
                    }
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Count
                nr.Read(); // Length
                int lengthobjvar7 = Int32.Parse(nr.Value);
                nr.Read(); // Count
                nr.Read(); // Tipo de elementos
                nr.Read();
                tipo = Type.GetType(nr.Value);
                nr.Read(); // Tipo de elementos
                nr.Read(); // Rank
                nr.Read();
                rango = Int32.Parse(nr.Value);
                nr.Read(); // Rank
                nr.Read(); // Datos de los rangos
                nr.Read(); // Datos de rango
                nr.Read(); // Longitud
                nr.Read();
                int auxobjvar7Length0 = Int32.Parse(nr.Value);
                nr.Read(); // Longitud
                nr.Read(); // Valor menor
                nr.Read();
                int auxobjvar7GetLowerBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor menor
                nr.Read(); // Valor mayor
                nr.Read();
                int auxobjvar7GetUpperBound0 = Int32.Parse(nr.Value);
                nr.Read(); // Valor mayor
                nr.Read(); // Datos de rango
                nr.Read(); // Datos de los rangos
                nr.Read(); // Valores
                for (int auxIndice0 = auxobjvar7GetLowerBound0; auxIndice0 <= auxobjvar7GetUpperBound0; auxIndice0++)
                {
                    if (obj.var7 == null) obj.var7 = new Fase02.DentroDelArray[auxobjvar7Length0];
                    nr.Read(); // Valor
                    Fase02.DentroDelArray objAux = new Fase02.DentroDelArray();
                    nr.Read(); // Elementos
                    string restoXML = nr.ReadOuterXml().ToString();
                    DentroDelArrayCodec.decode(ref restoXML, ref objAux);
                    obj.var7[auxIndice0] = objAux;
                }
                nr.Read(); // Valores
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elementos
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
            StringBuilder texto = new StringBuilder("<elementos>");
            if (obj.uno == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("uno");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.Int32");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append(obj.uno.ToString());
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            if (obj.dos == null)
            {
                texto.Append("<elemento>");
                texto.Append("null");
                texto.Append("</elemento>");
            }
            else
            {
                texto.Append("<elemento>");
                texto.Append("<nombre>");
                texto.Append("dos");
                texto.Append("</nombre>");
                texto.Append("<tipo>");
                texto.Append("System.String");
                texto.Append("</tipo>");
                texto.Append("<valor>");
                texto.Append(obj.dos.ToString());
                texto.Append("</valor>");
                texto.Append("</elemento>");
            }
            texto.Append("</elementos>");
            //str = texto;
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.DentroDelArray obj)
        {
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            xml.LoadXml(codigo);
            XmlNode nodoPrincipal = xml.SelectSingleNode("elementos");
            XmlNodeReader nr = new XmlNodeReader(nodoPrincipal);
            nr.Read(); // Elementos
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Valor del campo o propiedad
                obj.uno = Int32.Parse(nr.Value);
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elemento
            nr.Read(); // Nombre o null (si el miembro es null no hay más información de él)
            if (nr.Value != "null")
            {
                nr.Read(); // Nombre
                nr.Read(); // Nombre
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Tipo
                nr.Read(); // Valor
                nr.Read(); // Valor del campo o propiedad
                obj.dos = nr.Value;
                nr.Read(); // Valor
            }
            nr.Read(); // Elemento
            nr.Read(); // Elementos
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

            c3a.var7 = new Fase02.DentroDelArray[2];
            Fase02.DentroDelArray aux1 = new Fase02.DentroDelArray();
            aux1.uno = 1;
            aux1.dos = "dos";
            c3a.var7[0] = aux1;
            Fase02.DentroDelArray aux2 = new Fase02.DentroDelArray();
            aux2.uno = 3;
            aux2.dos = "cuatro";
            c3a.var7[1] = aux2;

            #endregion

            Clase03ArrayCodec serializador1 = new Clase03ArrayCodec();
            if (serializador1 != null)
            {
                Type tipo1 = serializador1.GetType();
                Console.WriteLine(tipo1.FullName);
                string codigo = serializador1.codificar(c3a);
                Console.WriteLine(codigo);

                Fase02.Clase03Array c3b = new Fase02.Clase03Array();
                serializador1.decodificar(codigo, ref c3b);
            }
            else
            {
                Console.WriteLine("No se ha podido generar el serializador");
            }
            Console.ReadLine();
        }
    }
}
