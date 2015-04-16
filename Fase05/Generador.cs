using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Fase05
{
    /*
     * Clase que genera el código de una clase codificador/decodificador para una determinada clase cuyo tipo se recibe como parámetro
     * La clase generada tiene dos métodos que se rellenan dependiendo de los miembros a serializar de la clase
     * 
     */
    public class Generador
    {
        private static string TAB = "\t";
        private static string SALTO = "\r\n";
        private static int tabuladores = 0;

        private enum tiposDeCodificacion
        {
            XML,
            JSON,
            Binary
        }
        private static tiposDeCodificacion tipoDeCodificacion;

        private static string strCodigo; // El código de la clase XXXSerializador
        private static string strEncode; // El código del método encode
        private static string strDecode; // El código del método decode

        // Variable principal: el tipo del que se va a crear el serializador
        private Type tipoInicial;
        private Type tipo;
        private static Dictionary<Type, Object> clases;

        public Generador(Type tipo)
        {
            this.tipoInicial = tipo;
            this.tipo = tipo;
            // Tipo de codificación por defecto
            Generador.tipoDeCodificacion = tiposDeCodificacion.XML;
            
            // Inicializar el conjunto de clases que se van a procesar
            clases = new Dictionary<Type, object>();
            clases.Add(tipo, null);

            // TODO Identificar a partir de un atributo del tipo si hay que codificar de otra manera
        }

        // Alias de generateSerializer
        private dynamic crearSerializador()
        {
            return this.getSerializer();
        }

        /*
         * Devuelve el Objecto serializador, generado a partir del tipo
         */
        public dynamic getSerializer()
        {
            dynamic serializador = null;

            // 1º Generar el código de la clase. Se guarda en strCodigo
            this.generateSerializer();
            Console.WriteLine(strCodigo);
            // 2º Compilar e instanciar la clase serializador
            serializador = this.compile(this.tipoInicial);
            return serializador;
        }

        /*
         * Mete en strCodigo todo el código del serializador a partir del tipo conocido
         * Este código contiene una definición de clase y dos métodos estáticos encode y decode
         */
        private void generateSerializer()
        {
            // 1º Generar la cabecera de la clase
            this.getCabecera();

            bool terminar = false; // quizás no haga falta
//            while (clases.Count > 0) // el dictionary se puede ir agrandando sobre la marcha
            for (int i = 0; i < clases.Count; i++) // Se recorren todas clases guardadas en el Dictionary
            {
                terminar = true; // quizás no haga falta
                KeyValuePair<Type, Object> par = clases.ElementAt(i);
                if(par.Value == null){ // Si la clase aún no se ha generado, se genera
                    terminar = false; // quizás no haga falta
                    this.tipo = par.Key;

                    // 2º Generar los métodos encode y decode (se hace a la vez)
                    this.generateEncodeAndDecodeMethods();

                    // 3º Generar los métodos auxiliares
                    this.generateAuxiliarMethods();
                }

                // 4º Se cierra la clase en cuestión (puede haber varias)
                this.getCierre();
            }
            this.getCierre(); // Cierre del Namespace
        }

        private void getCabecera()
        {
            // 1º Incluir los paquetes necesarios
            strCodigo = @"
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
";
        }

        private void getCierre()
        {
            strCodigo += @"
    }"; // Cierre de namespace
        }

        private void generateEncodeAndDecodeMethods()
        {
            // 2º Cabecera de la clase
            strCodigo += @"
    public class ";
            strCodigo += this.tipo.Name + "Codec {";
            strCodigo += @"
        public static Type tipo = Type.GetType(""" + this.tipo.Name;
            strCodigo += @""");
        public static String nombreClase = """ + this.tipo.Name;
            strCodigo += @""";
        private static string TAB = ""\t"";
        private static string SALTO = ""\r\n"";";
            strCodigo += @"
        public string codificar(" + tipo.FullName.Replace("+", ".") + " obj){";
            strCodigo += @"
            return encode(obj);}";
            strEncode = @"
        public static string encode(" + tipo.FullName.Replace("+", ".") + " obj){";
            strDecode = @"
        public " + tipo.FullName.Replace("+", ".") + " decode(String str, object obj){";

            BindingFlags flags = BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
                | BindingFlags.FlattenHierarchy
                | BindingFlags.Static;
            // Se serializan todos los miembros públicos, privados y estáticos; propios y heredados (CONFIRMAR)
            MemberInfo[] miembros = tipo.GetMembers(flags);
            strEncode += @"
            string texto = """ + abrir("elementos") + "\";";
 
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (esSerializable(miembro)) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
                {
                    if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                    {
                        PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo
                        if (!propertyInfo.PropertyType.FullName.Contains("Wrapper"))
                        {
                            Type t = propertyInfo.PropertyType;
                            string nombre = propertyInfo.Name;

                            strEncode += @"
            texto += """ + abrir("elemento");

                            strEncode += @""";
            texto += """ + abrir("nombre");
                            strEncode += @""";
            texto += """ + mostrarValor(nombre);
                            strEncode += @""";
            texto += """ + cerrar("nombre");

                            strEncode += @""";
            texto += """ + abrir("tipo");
                            strEncode += @""";
            texto += """ + mostrarValor(t.FullName);
                            strEncode += @""";
            texto += """ + cerrar("tipo");

                            strEncode += @""";
            texto += """ + abrir("valor") + "\";";

                            getValueEncode(t, "obj." + nombre);

                            strEncode += @"
            texto += """ + cerrar("valor") + "\";";
                            strEncode += @"
            texto += """ + cerrar("elemento") + "\";";
                        }
                    }
                }
            } //foreach
            strEncode += @"
            texto += """ + cerrar("elementos");
            strEncode += @""";
            //str = texto;
            return texto;
        }";
            strDecode += @"
            return null;
        }";
            strCodigo += strEncode;
            strCodigo += this.newLine();
            strCodigo += strDecode;
// strDecode ...
        }

        private static bool esSerializable(MemberInfo miembro)
        {
            return !miembro.Name.Contains("_BackingField") && (miembro.MemberType == MemberTypes.Property || miembro.MemberType == MemberTypes.Field);
        }

        private static string abrir(string texto)
        {
            string codigo = "";
            // Aquí se controla qué tipo de salida queremos, si XML, JSON, etc.
//            switch (Generador.tipoDeCodificacion)
//            {
//                case tiposDeCodificacion.XML:
//                default:
                        codigo += "<" + texto + ">";
//            }
            return codigo;
        }

        private static string cerrar(string texto){
            string codigo = "";
            // Aquí se controla quje tipo de salida queremos, si XML, JSON, etc.
//            switch (this.tipoDeSalida)
//            {
//                case ""XML"":
//                default:
                codigo += "</" + texto + ">";
//            }
            return codigo;
        }

        private string newLine()
        {
            string s = Generador.SALTO;
            for (int i = 0; i < tabuladores; i++)
            {
                s += Generador.TAB;
            }
            return s;
        }

        private static string getAccesibilidad(Type tipo){
            string accesibilidad = "";
            string codigo = "";
            // Accesibilidad: public, protected, internal, protected internal o private
            if (tipo.IsPublic){
                accesibilidad += "public";
            }
            else if (tipo.IsNotPublic){
                accesibilidad += "private";
            }
            else{
                accesibilidad += "protected";
            }
            codigo += accesibilidad;
            return codigo;
        }

        private static string getModificador(Type tipo, ref bool pintar){
            string modificador = "";
            string codigo = "";
            pintar = true;
            // Modificadores: abstract, async, override, parcial, readonly, sealed, static, unsafe, virtual, volatile
            if(tipo.IsAbstract){
                modificador += "abstract";
            }
            else if (tipo.IsSealed){
                modificador += "sealed";
            }
            else{
                modificador += "";
                pintar = false;
            }
            codigo += modificador;
            return codigo;
        }

        private static string getTipoDeObjeto(Type tipo){
            string tipoDeObjeto = "";
            string codigo = "";
            // Identificación del tipo de objeto: clase, interface, enum, struct
            if (tipo.IsPrimitive)
            {
                TypeAttributes atr = tipo.Attributes;
                tipoDeObjeto += atr.ToString();
            }
            else if (tipo.IsInterface){
                tipoDeObjeto += "interface";
            }
            else if (tipo.IsEnum){
                tipoDeObjeto += "enum";
            }
            else if (tipo.IsValueType){
                if (tipo.IsEnum){
                    tipoDeObjeto += "enum";
                }
                else{
                    tipoDeObjeto += "struct";
                    }
            }
            else if(tipo.IsClass){
                tipoDeObjeto += "class";
            }
            else
            {
                tipoDeObjeto += "unknown";
            }
            codigo += tipoDeObjeto;
            return codigo;
        }

        private static string mostrarValor(object texto)
        {
            string codigo = "";
            if (texto != null)
            {
//                switch (this.tipoDeSalida)
//                {
//                    case ""XML"":
//                    default:
                        return codigo + texto.ToString();
//                }
            }
            else
            {
                return "";
            }
        }

        private static void getValueEncode(Type t, string nombre)
        {
            if (t.IsPrimitive || t.FullName == "System.String") // Datos primitivos, simplemente cogemos su valor
            {
                strEncode += @"
            texto += " + nombre + ".ToString();";
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            Array aux" + nombreAux + " = " + nombre + " as Array;";
                strEncode += @"
            texto += """ + abrir("count");
                strEncode += @""";
            texto += aux" + nombreAux + ".Length;";
                strEncode += @"
            texto += """ + cerrar("count");

                strEncode += @""";
            texto += """ + abrir("tipoDeElementos");
                strEncode += @""";
            texto += """ + t.GetElementType().Name;
                strEncode += @""";
            texto += """ + cerrar("tipoDeElementos");

                strEncode += @""";
            texto += """ + abrir("rank");
                strEncode += @""";
            texto += """ + t.GetArrayRank();
                strEncode += @""";
            texto += """ + cerrar("rank");

                strEncode += @""";
            texto += """ + abrir("datosDeLosRangos");
                strEncode += @""";";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strEncode += @"
            texto += """ + abrir("datosDeRango");
                    strEncode += @""";
            texto += """ + abrir("longitud");
                    strEncode += @""";
            texto += aux" + nombreAux + ".GetLength(" + i + ");";
                    strEncode += @"
            texto += """ + cerrar("longitud");

                    strEncode += @""";
            texto += """ + abrir("valorMenor");
                    strEncode += @""";
            texto += aux" + nombreAux + ".GetLowerBound(" + i + ");";
                    strEncode += @"
            texto += """ + cerrar("valorMenor");

                    strEncode += @""";
            texto += """ + cerrar("datosDeRango");
                    strEncode += @""";
            ";
                }
                strEncode += "texto += \"" + cerrar("datosDeLosRangos") + "";

                strEncode += @""";
            texto += """ + abrir("valores");
                strEncode += @""";
            foreach (object elemento" + nombreAux + " in aux" + nombreAux + ")";
                strEncode += @"
            {
                texto += """ + abrir("cadaValor") + "\";";
                strEncode += @"
                texto += """ + abrir("valor") + "\";";

                getValueEncode(t.GetElementType(), "elemento" + nombreAux + "");

                strEncode += @"
                texto += """ + cerrar("valor") + "\";";
                strEncode += @"
                texto += """ + cerrar("cadaValor");
                strEncode += @""";
            }";
                strEncode += @"
            texto += """ + cerrar("valores") + "\";";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.List")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            Console.WriteLine(""1"");
            IList aux" + nombreAux + " = " + nombre + " as IList;";
                strEncode += @"
            texto += """ + abrir("count");
                strEncode += @""";
            texto += aux" + nombreAux + ".Count;";
                strEncode += @"
            texto += """ + cerrar("count");

                strEncode += @""";
            Console.WriteLine(""2"");
            texto += """ + abrir("tipoDeValue"); // Quizás no hace falta
                strEncode += @""";
            texto += """ + arguments[0].FullName + "\";";
                strEncode += @"
            texto += """ + cerrar("tipoDeValue");

                strEncode += @""";
            Console.WriteLine(""3"");
            texto += """ + abrir("valores");

                strEncode += @""";
            foreach (" + arguments[0].FullName + " item in aux" + nombreAux + ")";
                strEncode += @"
            {";
                strEncode += @"
            Console.WriteLine(""5"");
                texto += """ + abrir("valor");
                strEncode += @""";";
                getValueEncode(arguments[0], "item");
                strEncode += @"
            Console.WriteLine(""6"");
                texto += """ + cerrar("valor");
                strEncode += @""";
            }
            Console.WriteLine(""7"");
            texto += """ + cerrar("valores") + "\";";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.Dictionary")) // Dictionary (o hashtable), se codifica con sus claves y valores
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            IDictionary aux" + nombreAux + " = " + nombre + " as IDictionary;";
                strEncode += @"
            texto += """ + abrir("count");
                strEncode += @""";
            texto += aux" + nombreAux + ".Count;";
                strEncode += @"
            texto += """ + cerrar("count");

                strEncode += @""";
            texto += """ + abrir("tipoDeIndex"); // Quizás no hace falta
                strEncode += @""";
            texto += """ + arguments[0].FullName + "\";";
                strEncode += @"
            texto += """ + cerrar("tipoDeIndex");

                strEncode += @""";
            texto += """ + abrir("tipoDeValue"); // Quizás no hace falta
                strEncode += @""";
            texto += """ + arguments[0].FullName + "\";";
                strEncode += @"
            texto += """ + cerrar("tipoDeValue");

                strEncode += @""";
            texto += """ + abrir("valores");

                strEncode += @""";
            foreach (DictionaryEntry item in aux" + nombreAux + ")";
                strEncode += @"
            {
                texto += """ + abrir("clave");
                strEncode += @""";";
                getValueEncode(arguments[0], "item.Key");
                strEncode += @"
                texto += """ + cerrar("clave");
                
                strEncode += @""";
                texto += """ + abrir("valor");
                strEncode += @""";";
                getValueEncode(arguments[0], "item.Value");
                strEncode += @"
                texto += """ + cerrar("valor");
                strEncode += @""";
            }
            texto += """ + cerrar("valores") + "\";";

            }
            else //if (!t.FullName.StartsWith("System.")) // OBJETOS EXTENOS (debería ser el caso por defecto
            {
                // Hay que generar o invocar el serializador para esta clase
                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
                // Llamar al método encode de la clase SerializadorZZZ
                strEncode += @"
            texto += " + t.Name + "Codec.encode(" + nombre + ");";
                clases.Add(t, null);
            }
        }

        private string generateAuxiliarMethods()
        {
            strCodigo += this.newLine();
            strCodigo += @"
        private static string getValue(object c)
        {
            Console.WriteLine(""getValue 1 ("" + c.ToString());
            string texto = """";
            Type t = c.GetType();
            Console.WriteLine(""getValue 2 ("" + t.Name + "")"");

            if (t.IsPrimitive || t.Name == ""String"") // Datos primitivos, simplemente cogemos su valor
            {
            Console.WriteLine(""getValue 3"");
                texto += c.ToString();
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
            Console.WriteLine(""getValue 4"");
                Array aux = c as Array;

                // Generar codificador para el tipo y codificarlo
//                texto += codificarArray(aux);
            }
            else if (t.FullName.StartsWith(""System.Collections.Generic.List"")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
            Console.WriteLine(""getValue 5"");
            }
            else if (t.FullName.StartsWith(""System.Collections.Generic.Dictionary"")) // Dictionary (o hashtable), se codifica con sus claves y valores
            {
            Console.WriteLine(""getValue 6"");
            }
            else if (!t.FullName.StartsWith(""System."")) // OBJETOS EXTENOS
            {
            Console.WriteLine(""getValue 7"");
                // Hay que generar o invocar el serializador para esta clase
                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
            }

            Console.WriteLine(""getValue 8"");
            return texto;

        }";
            return strCodigo;
        }

        private Object compile(Type tipo) // Pasar el tipo completo
        {
            // TODO Ejecutar la compilación on tye fly
            // Instanciar un objeto de la clase compilada y devolverlo
            try
            {
                Console.WriteLine("Se ejecuta CreateCompiler");
                ICodeCompiler loCompiler = new CSharpCodeProvider().CreateCompiler();

                Console.WriteLine("Se ponen parámetros a CreateCompiler");
                CompilerParameters loParameters = new CompilerParameters();
                loParameters.ReferencedAssemblies.Add("System.dll");
                loParameters.ReferencedAssemblies.Add("System.Xml.dll");
                loParameters.ReferencedAssemblies.Add(tipo.Assembly.Location);
                loParameters.GenerateInMemory = true;

                Console.WriteLine("Se ejecuta CompileAssemblyFromSource (source es el código en variable string");
                // *** Medir aquí el tiempo de compilación
                CompilerResults loCompiled =
                        loCompiler.CompileAssemblyFromSource(loParameters, strCodigo);
                // Se comprueba si hubo errores
                if (loCompiled.Errors.HasErrors)
                {
                    string lcErrorMsg = "";

                    lcErrorMsg = loCompiled.Errors.Count.ToString() + " Errors:";
                    for (int x = 0; x < loCompiled.Errors.Count; x++)
                        lcErrorMsg = lcErrorMsg + "\r\nLine: " +
                            loCompiled.Errors[x].Line.ToString() + " - " +
                            loCompiled.Errors[x].ErrorText;

                    Console.WriteLine(lcErrorMsg + "\r\n\r\n");
                    return null;
                }

                Assembly loAssembly = loCompiled.CompiledAssembly;
                // *** Retrieve an obj ref – generic type only
                Console.WriteLine("Se crea una instancia del objeto compilado al vuelo");
                Object loObject = loAssembly.CreateInstance("Serializer." + tipo.Name + "Codec");

                if (loObject == null)
                {
                    Console.WriteLine("Couldn't load class.");
                    return null;
                }

                return loObject;
            }
            catch (Exception e)
            {
                Console.WriteLine("No se ha podido crear el objeto.");
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
