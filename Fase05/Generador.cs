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
    class Generador
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
        private Type tipo;

        public Generador(Type tipo)
        {
            this.tipo = tipo;
            // Tipo de codificación por defecto
            Generador.tipoDeCodificacion = tiposDeCodificacion.XML;

            // TODO Identificar a partir de un atributo del tipo si hay que codificar de otra manera
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
            serializador = this.compile(this.tipo.Name);
            return serializador;
        }

        // Alias de generateSerializer
        private dynamic crearSerializador()
        {
            return this.getSerializer();
        }

        /*
         * Mete en strCodigo todo el código del serializador a partir del tipo conocido
         * Este código contiene una definición de clase y dos métodos estáticos encode y decode
         */
        private void generateSerializer()
        {
            // 1º Generar la cabecera de la clase
            this.getCabecera();

            // 2º Generar los métodos encode y decode (se hace a la vez)
            this.generateEncodeAndDecodeMethods();
            strCodigo += strEncode;
            strCodigo += this.newLine();
            strCodigo += strDecode;

            // 3º Generar los métodos auxiliares
            this.generateAuxiliarMethods();

            // 4º Generar los métodos auxiliares
//            this.generateAuxiliarMethods();
            this.getCierre();
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

            // 2º Cabecera de la clase
            strCodigo += "    public class ";
            strCodigo += this.tipo.Name + "Codec {";
            strCodigo += @"
        public static Type tipo = Type.GetType(""" + this.tipo.Name;
            strCodigo += @""");
        public static String nombreClase = """ + this.tipo.Name;
            strCodigo += @""";
        private static string TAB = ""\t"";
        private static string SALTO = ""\r\n"";";
        }

        private void getCierre()
        {
            strCodigo += "    }"; // Cierre de clase
            strCodigo += "}"; // Cierre de namespace
        }

        private void generateEncodeAndDecodeMethods()
        {
            strEncode += @"
        public void encode(object objeto, ref String str){
            " + tipo.Name + " obj = (" + tipo.Name;
            strEncode += @")objeto;
            ";
//            strEncode += "public void encode(object obj, ref string str){";
            strDecode += @"
        public object decode(String str, object obj){";

            strEncode += @"
                string texto = """";
                texto += """ + abrir("serializacion");
            strEncode += @""";
                texto += """ + abrir("accesibilidad");
            strEncode += @""";
                texto += """ + getAccesibilidad(tipo);
            strEncode += @""";
                texto += """ + cerrar("accesibilidad");

            // Modificador, opcional
            bool pintar = false;
            string modificador = getModificador(tipo, ref pintar);
            if (pintar)
            {
                strEncode += @""";
                texto += """ + abrir("modificador");
                strEncode += @""";
                texto += """ + modificador;
                strEncode += @""";
                texto += """ + cerrar("modificador");
            }
            strEncode += @""";
                texto += """ + abrir("tipoDeObjeto");
            strEncode += @""";
                texto += """ + getTipoDeObjeto(tipo);
            strEncode += @""";
                texto += """ + cerrar("tipoDeObjeto");
            strEncode += @""";";

            BindingFlags flags = BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
                | BindingFlags.NonPublic
                | BindingFlags.FlattenHierarchy
                | BindingFlags.Static;
            // Se serializan todos los miembros públicos, privados y estáticos; propios y heredados (CONFIRMAR)
            MemberInfo[] miembros = tipo.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            strEncode += @"
                Console.WriteLine(""2"");";
            strEncode += @"
                texto += """ + abrir("elementos") + "\";";
 
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!miembro.Name.Contains("_BackingField")) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
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
                            texto += """ + abrir("valor");

                            if (t.IsPrimitive || t.Name == "String") // Datos primitivos, simplemente cogemos su valor
                            {
                                strEncode += @""";
                                texto += obj." + nombre + ".ToString();";
                            }
                            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
                            {
                                strEncode += @""";
                                texto += """ + abrir("count");
                                strEncode += @""";
                                texto += obj." + nombre + ".Count;";
                                strEncode += @"
                                texto += """ + cerrar("count");

                                strEncode += @""";
                                texto += """ + abrir("tipoDeElementos");
                                strEncode += @""";
                                texto += """ + t.GetElementType().FullName;
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
                                strEncode += @""";
                    ";

                                for(int i=0; i< t.GetArrayRank(); i++)
                                {

                                strEncode += @"
                                texto += """ + abrir("datosDeRango");
                                strEncode += @""";
                                    texto += """ + abrir("longitud");
                                strEncode += @""";
                                    texto += obj." + nombre + ".GetLength(" + i + ");";
                                strEncode += @"
                                    texto += """ + cerrar("longitud");

                                strEncode += @""";
                                    texto += """ + abrir("valorMenor");
                                strEncode += @""";
                                    texto += obj." + nombre + ".GetLowerBound(" + i + ");";
                                strEncode += @"
                                    texto += """ + cerrar("longitud");

                                strEncode += @""";
                                texto += """ + cerrar("datosDeRango");
                                strEncode += @""";
                    ";
                                }
                                strEncode += "texto += \"" + cerrar("datosDeLosRangos") + "";

                                strEncode += @""";
                                texto += """ + abrir("valores");
                                strEncode += @""";
                                foreach (object elemento in obj." + nombre + ")";
                                strEncode += @"
                                {
                                    texto += """ + abrir("cadaValor") + "\";";
                                strEncode += @"
                                    texto += """ + abrir("valor") + "\";";
                                strEncode += @"
                                    texto += elemento.ToString();";
                                strEncode += @"
                                    texto += """ + cerrar("valor") + "\";";
                                strEncode += @"
                                    texto += """ + cerrar("cadaValor");
                                strEncode += @""";
                                }";
                                strEncode += @"
                                texto += """ + cerrar("valores");
                                strEncode += @""";
                            texto += """ + abrir("valor");
                            strEncode += @""";";
                            }
                            else if (t.FullName.StartsWith("System.Collections.Generic.List")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
                            {
                            Console.WriteLine("getValue 5");
                            }
                            else if (t.FullName.StartsWith("System.Collections.Generic.Dictionary")) // Dictionary (o hashtable), se codifica con sus claves y valores
                            {
                            Console.WriteLine("getValue 6");
                            }
                            else if (!t.FullName.StartsWith("System.")) // OBJETOS EXTENOS
                            {
                            Console.WriteLine("getValue 7");
                                // Hay que generar o invocar el serializador para esta clase
                                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
                            }
                        }
                    }
                }
            } //foreach
            strEncode += @"
                texto += """ + cerrar("elementos");
            strEncode += @""";
            texto += """ + cerrar("serializacion");
            strEncode += @""";
            Console.WriteLine(texto);
            str = texto;
        }";
            strDecode += @"
            return null;
        }";

// strDecode ...
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

        /*
         * Captura la información de cada miembro de un tipo, incluyendo sus valores
         * @input MemberInfo[] miembros Conjunto de miembros de un tipo susceptibles de ser serializados tipo.GetMembers(flags)
         * @input Object obj Objeto instanciado para obtener los valores
         * @output string Código serializado con todos los miembros del objeto indicado
         */
        // Método OK para obtener todos los miembros (propiedades y variables) susceptibles de ser serializados
/*
        private string getCodigoByMembers(MemberInfo[] miembros)
        {
            codigo += abrir("elementos");
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!miembro.Name.Contains("_BackingField")) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
                {
                    if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                    {
                        PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo
                        if (!propertyInfo.PropertyType.FullName.Contains("Wrapper"))
                        {
                            Type t = propertyInfo.PropertyType;
                            string nombre = propertyInfo.Name;

                            codigo += abrir("elemento");

                            codigo += abrir("nombre");
                            codigo += mostrarValor(nombre);
                            codigo += cerrar("nombre");

                            codigo += abrir("tipo");
                            codigo += mostrarValor(t.FullName);
                            codigo += cerrar("tipo");

                            codigo += abrir("tipoDeElemento");
                            codigo += mostrarValor("propiedad");
                            codigo += cerrar("tipoDeElemento");

                            codigo += @""";
                ";
                            codigo += "pi = obj.GetType().GetProperty(\"" + nombre + "\");";
                            codigo += @"
                ";
                            codigo += "tipo = pi.PropertyType;";
                            codigo += @"
                ";
                            codigo += "isArray = tipo.IsArray;";
                            codigo += @"
                ";
                            codigo += "valor = pi.GetValue(obj, null) as Object;";
                            if t.IsPrimitive || t.Name == "String") // Datos primitivos, simplemente cogemos su valor
                            {
                                codigo += "obj." + nombre + ".ToString();";
                                Console.WriteLine("getValue 3");
                            }
                            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
                            {
                            Console.WriteLine("getValue 4");
                                codigo += abrir("count");
                                codigo += @""";
                    ";
                                codigo += "texto += obj." + nombre + ".Count;";
                                codigo += @"
                                texto += """;
                                codigo += cerrar("count");

                                codigo += abrir("tipoDeElementos");
                                codigo += mostrarValor(t.GetElementType().FullName);
                                codigo += cerrar("tipoDeElementos");

                                codigo += abrir("rank");
                                codigo += @""";
                    ";
                                codigo += "int rango = obj." + nombre + ".Rank;";
                                codigo += "texto += rango;";
                                codigo += @"
                                texto += """;
                                codigo += cerrar("rank");

                                codigo += abrir("datosDeLosRangos");
                                codigo += @""";
                    ";
                                for(int i=0; i< rango; i++)
                                {
                                codigo += abrir("longitud");
                                    texto += abrir(""datosDeRango"");

                                codigo += abrir("longitud");
                                codigo += @""";
                    ";
                                codigo += "texto += obj." + nombre + ".GetLength(i);";
                                codigo += @"
                                texto += """;
                                codigo += cerrar("count");



                                    codigo += abrir(""valorMenor"");
                                    codigo += mostrarValor(miArray.GetLowerBound(i));
                                    codigo += cerrar(""valorMenor"");

                                    codigo += cerrar(""datosDeRango"");
                                }
                                codigo += cerrar(""datosDeLosRangos"");

                codigo += abrir(""tipoDeList"");
                codigo += mostrarValor(tipoList);
                codigo += cerrar(""tipoDeList"");
            }

            codigo += abrir(""valores"");
            foreach (object elemento in list)
            {
                codigo += abrir(""cadaValor"");
                IList lista = elemento as IList;
                if (lista != null)
                {
                    codigo += abrir(""tipoDeElemento"");
                    codigo += mostrarValor(""elementoDeArray"");
                    codigo += cerrar(""tipoDeElemento"");

                    codigo += abrir(""tipo"");
                    codigo += mostrarValor(lista.GetType());
                    codigo += cerrar(""tipo"");

                    codigo += abrir(""isArray"");
                    codigo += mostrarValor(""True"");
                    codigo += cerrar(""isArray"");

                    codigo += abrir(""valor"");

                    codigo += recorrerIListObject(lista);
                    codigo += cerrar(""valor"");

//                    MemberInfo[] miembros = elemento.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
//                    codigo += getCodigoByMembers(miembros, (object)elemento);
                }
                else
                {
                    codigo += abrir(""valor"");
                    // Aquí hay que ver si el objeto es ""simple"" o es una estructura (clase, list, array, etc.)
                    codigo += mostrarValor(elemento);
                    codigo += cerrar(""valor"");
                }
                codigo += cerrar(""cadaValor"");
            }
Console.WriteLine(""f"");
            codigo += cerrar(""valores"");

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

                            codigo += "valor = obj.val[i];";

                            codigo += @"
                Console.WriteLine(""Dos miembros"");
                ";
                            codigo += "texto += \"";
                            codigo += abrir("isArray");
                            codigo += mostrarValor(propertyInfo.PropertyType.IsArray);
                            codigo += cerrar("isArray");

                            codigo += abrir("valor");
                            codigo += "\";";
                            codigo += @"
                    Console.WriteLine(""Tres miembros"");
                    if(valor != null)
                    {
                        ";
                            codigo += "texto += getValue(valor);";
                            codigo += @"
                    }
                        ";

                            /*
                             * Comprobaciones adicionales
                             * Si el tipo de este miembro es un iList, mostramos más valores (length, etc.)
                             * Si el tipo de este miembro es un IDictionary, tomamos sus valores (de la forma key, values)
                             * Si el tipo de este miembro es una clase, tendremos que llamar recursivamente a getCodigoByMembers con los miembros de la clase
                             */
    /*                        IList list = propertyInfo.GetValue(obj, null) as IList;
                            if (list != null)
                            {
                                codigo += recorrerIList(propertyInfo, obj);
                            }
                            else
                            {
                                IDictionary dict = propertyInfo.GetValue(obj, null) as IDictionary;
                                //                            if (propertyInfo.GetType().GetGenericTypeDefinition() == typeof(IDictionary<,>))
                                if (dict != null)
                                {
                                    codigo += recorrerIDictionary(propertyInfo, obj);
                                }
                                else if (miembro.GetType().IsInterface)
                                {
                                    Object objeto = propertyInfo.GetValue(obj, null) as Object;
                                    MemberInfo[] miembrosClase = objeto.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                                    codigo += getCodigoByMembers(miembrosClase, objeto);
                                }
                                else
                                {
     /
    //                        codigo += mostrarValor(propertyInfo.GetValue(obj, null));

    //                        codigo += mostrarValor("\"\" + obj.GetType().GetProperty(\"\"" + propertyInfo.Name + "\"\").GetValue(obj, null) + \"");
    //                           }
     //                       }
                            codigo += @"
                    Console.WriteLine(""Cuatro miembros"");
                        ";
                            codigo += "texto += \"";
                            codigo += cerrar("valor");
                            codigo += cerrar("elemento");
                        }
                        else if (propertyInfo.PropertyType == null) 
                        {
                            Console.WriteLine("/* **************************1************************ /");
                        }
                        else
                        {
                            Console.WriteLine("/* **************************2************************ /");
                            Console.WriteLine(propertyInfo.PropertyType.ToString());
                            Console.WriteLine("/* **************************3************************ /");
                        }
                    }
                }
            } //foreach
            codigo += cerrar("elementos");
            return codigo;
        } //getCodigoByMembers()
*/
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

        private Object compile(string className)
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
                Object loObject = loAssembly.CreateInstance("Serializer." + className + "Codec");

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
