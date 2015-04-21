
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

    public class Clase03aUnArrayCodec {
        public static Type tipo = Type.GetType("Clase03aUnArray");
        public static String nombreClase = "Clase03aUnArray";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.Clase03aUnArray obj){
            return encode(obj);}
        public void decodificar(string codigo, ref Fase02.Clase03aUnArray obj){
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.Clase03aUnArray obj){
            StringBuilder texto = new StringBuilder();
//            texto.Append("var4,");
//            texto.Append("System.Collections.Generic.List`1[[Fase02.ClaseViti, Fase02, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]],");
            IList auxobjvar4 = obj.var4 as IList;
            texto.Append(auxobjvar4.Count + ",");
            texto.Append("Fase02.ClaseViti" + ",");
            foreach (Fase02.ClaseViti elementoAuxobjvar4 in obj.var4)
            {
            texto.Append(ClaseVitiCodec.encode( elementoAuxobjvar4));
            }
//            texto.Append("var5,");
//            texto.Append("System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]],");
            IDictionary auxobjvar5 = obj.var5 as IDictionary;
            texto.Append(auxobjvar5.Count + ",");
            texto.Append("System.Int32" + ",");
            texto.Append("System.String" + ",");
            foreach (KeyValuePair<System.Int32, System.String> elementoAuxobjvar5 in obj.var5)
            {
            texto.Append( elementoAuxobjvar5.ToString() + ",");
            texto.Append( elementoAuxobjvar5.ToString() + ",");
            }
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase03aUnArray obj){
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar4 = Int32.Parse(elementos.Dequeue());
            tipo = Type.GetType(elementos.Dequeue());
                type = Type.GetType(typeof (List<Fase02.ClaseViti>).AssemblyQualifiedName);
                IList<Fase02.ClaseViti> listaAuxobjvar4 = (IList<Fase02.ClaseViti>)Activator.CreateInstance(type);
                Fase02.ClaseViti elementoAuxobjvar4 = new Fase02.ClaseViti();
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                obj.var4 = new List<Fase02.ClaseViti>();
            for (int iauxobjvar4 = 0; iauxobjvar4 < lengthobjvar4; iauxobjvar4++)
            {
            Fase02.ClaseViti objAuxauxobjvar4 = new Fase02.ClaseViti();
            string aux = string.Join(",", elementos.ToArray());
            ClaseVitiCodec.decode(ref aux, ref objAuxauxobjvar4);
            elementos = new Queue<string>(aux.Split(','));
             elementoAuxobjvar4 = objAuxauxobjvar4 as Fase02.ClaseViti;
                obj.var4.Add(elementoAuxobjvar4);
            }
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            int lengthobjvar5 = Int32.Parse(elementos.Dequeue());
            Type tipoIndice = Type.GetType(elementos.Dequeue());
            Type tipoValor = Type.GetType(elementos.Dequeue());
                type = Type.GetType(typeof (Dictionary<System.Int32, System.String>).AssemblyQualifiedName);
                IDictionary<System.Int32, System.String> dictAuxobjvar5 = (IDictionary<System.Int32, System.String>)Activator.CreateInstance(type);
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                obj.var5 = new Dictionary<System.Int32,System.String>();
            for (int iauxobjvar5 = 0; iauxobjvar5 < lengthobjvar5; iauxobjvar5++)
            {
                System.Int32 indiceAuxobjvar5 = new System.Int32();
                {System.Int32 elementoAuxobjvar5 = new System.Int32();
             elementoAuxobjvar5 = Int32.Parse(elementos.Dequeue());
                indiceAuxobjvar5 = elementoAuxobjvar5;
                }
                {System.String elementoAuxobjvar5 = "";
             elementoAuxobjvar5 = elementos.Dequeue();
                obj.var5.Add(indiceAuxobjvar5, elementoAuxobjvar5);
                }
            }
            codigo = string.Join(",", elementos.ToArray());
        }
    }
    public class ClaseVitiCodec {
        public static Type tipo = Type.GetType("ClaseViti");
        public static String nombreClase = "ClaseViti";
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        public string codificar(Fase02.ClaseViti obj){
            return encode(obj);}
        public void decodificar(string codigo, ref Fase02.ClaseViti obj){
            decode(ref codigo, ref obj);
        }
        public static string encode(Fase02.ClaseViti obj){
            StringBuilder texto = new StringBuilder();
//            texto.Append("v1,");
//            texto.Append("System.String,");
            texto.Append(obj.v1.ToString() + ",");
//            texto.Append("v2,");
//            texto.Append("System.Int32,");
            texto.Append(obj.v2.ToString() + ",");
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.ClaseViti obj){
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            obj.v1 = elementos.Dequeue();
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            obj.v2 = Int32.Parse(elementos.Dequeue());
            codigo = string.Join(",", elementos.ToArray());
        }

        static void Main(string[] args)
        {
            Fase02.Clase03aUnArray c3 = new Fase02.Clase03aUnArray();
            Clase03aUnArrayCodec serializador3 = new Clase03aUnArrayCodec();

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

            c3.var5 = new Dictionary<int, string>();
            c3.var5.Add(4, "cuatro");
            c3.var5.Add(5, "cinco");
            c3.var5.Add(6, "seis");

            string codigo = serializador3.codificar(c3);
            Console.WriteLine(codigo);
            Fase02.Clase03aUnArray c3aux = new Fase02.Clase03aUnArray();
            serializador3.decodificar(codigo, ref c3aux);

            foreach (Fase02.ClaseViti i in c3aux.var4)
            {
                Console.WriteLine(i.v1.ToString() + "," + i.v2.ToString());
            }
            Console.ReadKey();
        }

    }
}
