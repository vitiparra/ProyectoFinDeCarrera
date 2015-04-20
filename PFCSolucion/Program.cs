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
            Console.WriteLine("PRUEBA 6");
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
            return texto.ToString();
        }

        public static void decode(ref String codigo, ref Fase02.Clase03aUnArray obj){
            int count;
            Type tipo;
            int rango;
            string nombre;
            Console.WriteLine("PRUEBA 1");
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
            Console.WriteLine("PRUEBA 2");
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            Console.WriteLine("PRUEBA 3");
            Console.WriteLine("ele:1");
                Console.WriteLine("PRUEBA d1");
            int lengthobjvar4 = Int32.Parse(elementos.Dequeue());
                Console.WriteLine("PRUEBA d2");
                Console.WriteLine("PRUEBA d3");
            tipo = Type.GetType(elementos.Dequeue());
                Console.WriteLine("PRUEBA d4");
                Console.WriteLine("PRUEBA d4");
                var type = Type.GetType(typeof (List<Fase02.ClaseViti>).AssemblyQualifiedName);
                Console.WriteLine("PRUEBA d5");
                IList<Fase02.ClaseViti> listaAuxobjvar4 = (IList<Fase02.ClaseViti>)Activator.CreateInstance(type);
                Console.WriteLine("PRUEBA d6");
                Fase02.ClaseViti elementoAuxobjvar4 = new Fase02.ClaseViti();
                Console.WriteLine("PRUEBA d7");
            for (int iauxobjvar4 = 0; iauxobjvar4 < lengthobjvar4; iauxobjvar4++)
            {
                Console.WriteLine("PRUEBA e1");
            Fase02.ClaseViti objAuxauxobjvar4 = new Fase02.ClaseViti();
                Console.WriteLine("PRUEBA e2");
            string aux = string.Join(",", elementos.ToArray());
            Console.WriteLine(aux);
            ClaseVitiCodec.decode(ref aux, ref objAuxauxobjvar4);
                Console.WriteLine("PRUEBA e3");
            elementos = new Queue<string>(aux.Split(','));
                Console.WriteLine("PRUEBA e4");
             elementoAuxobjvar4 = objAuxauxobjvar4 as Fase02.ClaseViti;
                Console.WriteLine("PRUEBA e5");
                Console.WriteLine("PRUEBA d8");
                obj.var4.Add(elementoAuxobjvar4);
                Console.WriteLine("PRUEBA d9");
            }
            Console.WriteLine("PRUEBA 4");
            codigo = string.Join(",", elementos.ToArray());
            Console.WriteLine("PRUEBA 5");
            Console.WriteLine("FIN");
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
            Console.WriteLine("PRUEBA 6");
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
            int rango;
            string nombre;
            Console.WriteLine("PRUEBA 1");
            Queue<string> elementos = new Queue<string>(codigo.Split(','));
            Console.WriteLine("PRUEBA 2");
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            Console.WriteLine("PRUEBA 3");
            Console.WriteLine("ele:1");
            Console.WriteLine("PRUEBA b1");
            obj.v1 = elementos.Dequeue();
            Console.WriteLine("PRUEBA b2");
            Console.WriteLine("PRUEBA 2");
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());
            Console.WriteLine("PRUEBA 3");
            Console.WriteLine("ele:1");
            Console.WriteLine("PRUEBA b1");
            obj.v2 = Int32.Parse(elementos.Dequeue());
            Console.WriteLine("PRUEBA b2");
            Console.WriteLine("PRUEBA 4");
            codigo = string.Join(",", elementos.ToArray());
            Console.WriteLine("PRUEBA 5");
            Console.WriteLine("FIN");
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
