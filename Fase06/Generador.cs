using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Fase06
{
    /*
     * Clase que genera el código de una clase codificador/decodificador para una determinada clase cuyo tipo se recibe como parámetro
     * La clase generada tiene dos métodos que se rellenan dependiendo de los miembros a serializar de la clase
     * 
     */
    public class Generador
    {
        private static readonly string TAB = "\t";
        private static readonly string SALTO = "\r\n";
        private int tabuladores = 0;

        private enum tiposDeCodificacion
        {
            XML,
            CSV,
            JSON,
            Binary
        }
        private tiposDeCodificacion tipoDeCodificacion;

        private string strCodigo; // El código de la clase XXXSerializador
        private string strEncode; // El código del método encode
        private string strDecode; // El código del método decode

        // Variable principal: el tipo del que se va a crear el serializador
        private Type tipoInicial;
        private Type tipo;
        private static Dictionary<Type, Object> clases;

        public Generador(Type tipo)
        {
            this.tipoInicial = tipo;
            this.tipo = tipo;
            // Tipo de codificación por defecto
            tipoDeCodificacion = tiposDeCodificacion.CSV;

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
            #region mostrarCodigo
            // Mostrar por pantalla el código generado
            Console.WriteLine(strCodigo);
            // Sacar el código generado a un archivo de texto (en <proyecto>/bin/Debug)
            using (StreamWriter writer = new StreamWriter("filename.txt"))
            {
                writer.Write(strCodigo);
            }
            #endregion
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
            // Generar la cabecera de la clase
            this.getCabecera();

            for (int i = 0; i < clases.Count; i++) // Se recorren todas clases guardadas en el Dictionary
            {
                KeyValuePair<Type, Object> par = clases.ElementAt(i);
                if (par.Value == null) // Si la clase aún no se ha generado, se genera
                { 
                    this.tipo = par.Key;

                    // Generar los métodos encode y decode (se hace a la vez)
                    this.generateEncodeAndDecodeMethods();
                }

                // Se cierra la clase en cuestión (puede haber varias)
                this.getCierre();
            }
            // Cierre del Namespace
            this.getCierre(); 
        }

        private void getCabecera()
        {
            // Incluir los paquetes necesarios
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
            // Cabecera de la clase
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

            strCodigo += @"
        public void decodificar(string codigo, ref " + tipo.FullName.Replace("+", ".") + " obj){";
            strCodigo += @"
            decode(ref codigo, ref obj);
        }";

            strEncode = @"
        public static string encode(" + tipo.FullName.Replace("+", ".") + " obj){";

            strDecode = @"
        public static void decode(ref String codigo, ref " + tipo.FullName.Replace("+", ".") + " obj){";
            strDecode += @"
            int count;
            Type tipo;
            Type type;
            int rango;
            string nombre;";

            BindingFlags flags = BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
                | BindingFlags.FlattenHierarchy
                | BindingFlags.Static;
            // Se serializan todos los miembros públicos; propios y heredados
            MemberInfo[] miembros = tipo.GetMembers(flags);

            // Se invoca el tipo de serialización correspondiente
            switch (this.tipoDeCodificacion)
            {
                case tiposDeCodificacion.CSV:
                    GenerateCodeCSV(miembros);
                    break;
                case tiposDeCodificacion.XML:
                default:
                    GenerateCodeXML(miembros);
                    break;
            }
        }

        private void GenerateCodeXML(MemberInfo[] miembros)
        {
            strEncode += @"
            StringBuilder texto = new StringBuilder(""" + abrir("elementos") + "\");";

            strDecode += @"
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            xml.LoadXml(codigo);
            XmlNode nodoPrincipal = xml.SelectSingleNode(""elementos"");
            XmlNodeReader nr = new XmlNodeReader(nodoPrincipal);
            nr.Read(); // Elementos";

            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!esSerializable(miembro)) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
                {
                    continue;
                }

                Type t;
                string nombre;
                if (miembro.MemberType == MemberTypes.Property)
                {
                    PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo
                    t = propertyInfo.PropertyType;
                    nombre = propertyInfo.Name;
                }
                else //if(miembro.MemberType == MemberTypes.Field)
                {
                    FieldInfo fieldInfo = miembro as FieldInfo; // Conversión para obtener los datos del tipo de campo
                    t = fieldInfo.FieldType;
                    nombre = fieldInfo.Name;
                }


                strEncode += @"
            texto.Append(""" + abrir("elemento");

                strDecode += @"
            nr.Read(); // Elemento";

                strEncode += @""");
            texto.Append(""" + abrir("nombre");
                strDecode += @"
            nr.Read(); // Nombre";
                strEncode += @""");
            texto.Append(""" + mostrarValor(nombre);
                strDecode += @"
            nr.Read(); // Nombre";
                strEncode += @""");
            texto.Append(""" + cerrar("nombre");
                strDecode += @"
            nr.Read(); // Nombre";

                strEncode += @""");
            texto.Append(""" + abrir("tipo");
                strDecode += @"
            nr.Read(); // Tipo";
                strEncode += @""");
            texto.Append(""" + mostrarValor(t.FullName);
                strDecode += @"
            nr.Read(); // Tipo";
                strEncode += @""");
            texto.Append(""" + cerrar("tipo");
                strDecode += @"
            nr.Read(); // Tipo";

                strEncode += @""");
            texto.Append(""" + abrir("valor") + "\");";
                strDecode += @"
            nr.Read(); // Valor";

                getValueXML(t, "obj." + nombre, nombre);

                strEncode += @"
            texto.Append(""" + cerrar("valor") + "\");";
                strDecode += @"
            nr.Read(); // Valor";
                strEncode += @"
            texto.Append(""" + cerrar("elemento") + "\");";
                strDecode += @"
            nr.Read(); // Elemento";
            } //foreach

            strEncode += @"
            texto.Append(""" + cerrar("elementos");
            strDecode += @"
            nr.Read(); // Elementos";
            strEncode += @""");
            //str = texto;
            return texto.ToString();
        }";
            strDecode += @"
            Console.WriteLine(""n"");
        }";
            strCodigo += strEncode;
            strCodigo += this.newLine();
            strCodigo += strDecode;
        }

        protected void getValueXML(Type t, string nombre, string nombreCampo, bool isArray = false, bool isList = false, bool isDictionaryIndex = false, bool isDictionaryValue = false)
        {
            if (t.IsPrimitive || t.FullName == "System.String" || isDictionaryValue) // Datos primitivos, simplemente cogemos su valor
            {
                strEncode += @"
            texto.Append(" + nombre + ".ToString());";
                strDecode += @"
            nr.Read(); // Valor del campo o propiedad";

                if (isArray)
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombreCampo + " = nr.Value;";
                    }
                    else
                    {
                        strDecode += @"
            " + nombreCampo + " = " + t.Name + ".Parse(nr.Value);";
                    }
                }
                else if (isList)
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombreCampo + ".Add(nr.Value);";
                    }
                    else
                    {
                        strDecode += @"
            " + nombreCampo + ".Add(" + t.Name + ".Parse(nr.Value));";
                    }
                }
                else
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombre + " = nr.Value;";
                    }
                    else
                    {
                        strDecode += @"
            " + nombre + " = " + t.Name + ".Parse(nr.Value);";
                    }
                }
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
                string tipoElemento = t.GetElementType().FullName;
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            texto.Append(""" + abrir("count");
                strDecode += @"
            nr.Read(); // Count";
                strEncode += @""");
            texto.Append(" + nombre + ".Length);";
                strDecode += @"
            nr.Read(); // Length
            int length" + nombreAux + " = Int32.Parse(nr.Value);";
                strEncode += @"
            texto.Append(""" + cerrar("count");
                strDecode += @"
            nr.Read(); // Count";

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeElementos");
                strDecode += @"
            nr.Read(); // Tipo de elementos";
                strEncode += @""");
            texto.Append(""" + tipoElemento;
                strDecode += @"
            nr.Read();
            tipo = Type.GetType(nr.Value);";
                strEncode += @""");
            texto.Append(""" + cerrar("tipoDeElementos");
                strDecode += @"
            nr.Read(); // Tipo de elementos";

                strEncode += @""");
            texto.Append(""" + abrir("rank");
                strDecode += @"
            nr.Read(); // Rank";
                strEncode += @""");
            texto.Append(""" + t.GetArrayRank();
                strDecode += @"
            nr.Read();
            rango = Int32.Parse(nr.Value);";
                strEncode += @""");
            texto.Append(""" + cerrar("rank");
                strDecode += @"
            nr.Read(); // Rank";

                strEncode += @""");
            texto.Append(""" + abrir("datosDeLosRangos");
                strDecode += @"
            nr.Read(); // Datos de los rangos";
                strEncode += @""");";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strEncode += @"
            texto.Append(""" + abrir("datosDeRango");
                    strDecode += @"
            nr.Read(); // Datos de rango";
                    strEncode += @""");
            texto.Append(""" + abrir("longitud");
                    strDecode += @"
            nr.Read(); // Longitud";
                    strEncode += @""");
            texto.Append(" + nombre + ".GetLength(" + i + "));";
                    strDecode += @"
            nr.Read();
            int aux" + nombreAux + "Length" + i + " = Int32.Parse(nr.Value);";
                    strEncode += @"
            texto.Append(""" + cerrar("longitud");
                    strDecode += @"
            nr.Read(); // Longitud";
 
                    strEncode += @""");
            texto.Append(""" + abrir("valorMenor");
                    strDecode += @"
            nr.Read(); // Valor menor";
                    strEncode += @""");
            texto.Append(" + nombre + ".GetLowerBound(" + i + "));";
                    strDecode += @"
            nr.Read();
            int aux" + nombreAux + "GetLowerBound" + i + " = Int32.Parse(nr.Value);";
                    strEncode += @"
            texto.Append(""" + cerrar("valorMenor");
                    strDecode += @"
            nr.Read(); // Valor menor";

                    strEncode += @""");
            texto.Append(""" + abrir("valorMayor");
                    strDecode += @"
            nr.Read(); // Valor mayor";
                    strEncode += @""");
            texto.Append(" + nombre + ".GetUpperBound(" + i + "));";
                    strDecode += @"
            nr.Read();
            int aux" + nombreAux + "GetUpperBound" + i + " = Int32.Parse(nr.Value);";
                    strEncode += @"
            texto.Append(""" + cerrar("valorMayor");
                    strDecode += @"
            nr.Read(); // Valor mayor";

                    strEncode += @""");
            texto.Append(""" + cerrar("datosDeRango");
                    strDecode += @"
            nr.Read(); // Datos de rango";
                    strEncode += @""");
            ";

                }

                strEncode += "texto.Append(\"" + cerrar("datosDeLosRangos") + "";
                strDecode += @"
            nr.Read(); // Datos de los rangos";
                strDecode += @"
            nr.Read(); // Valores";

                // Montar tantos FOR anidados como dimensiones tenga el array
                string indices = "[";
                string longitudes = "[";
                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strDecode += @"
            for(int auxIndice" + i + " = aux" + nombreAux + "GetLowerBound" + i + "; auxIndice" + i + " <= aux" + nombreAux + "GetUpperBound" + i + "; auxIndice" + i + "++){";
                    indices += "auxIndice" + i + ",";
                    longitudes += "aux" + nombreAux + "Length" + i + ",";
                }
                indices = indices.Substring(0, indices.Length - 1);
                indices += "]";
                longitudes = longitudes.Substring(0, longitudes.Length - 1);
                longitudes += "]";

                strDecode += @"
            if(" + nombre + " == null) " + nombre + " = new " + tipoElemento + longitudes + ";";
//            if(" + nombre + " == null) " + nombre + " = Array.CreateInstance(" + tipoElemento + ", "  + longitudes + ");";

                strEncode += @""");
            texto.Append(""" + abrir("valores");
                strEncode += @""");
            foreach (" + tipoElemento + " elementoAux" + nombreAux + " in " + nombre + ")";
                strEncode += @"
            {
                texto.Append(""" + abrir("valor") + "\");";
                strDecode += @"
            nr.Read(); // Valor";

                getValueXML(t.GetElementType(), "elementoAux" + nombreAux, nombre + indices, true);

                strEncode += @"
                texto.Append(""" + cerrar("valor") + "\");";
                strDecode += @"
            nr.Read(); // Valor";
                strEncode += @";
            }";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strDecode += @"
            }";
                }

                strEncode += @"
            texto.Append(""" + cerrar("valores") + "\");";
                strDecode += @"
            nr.Read(); // Valores";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.List")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            Console.WriteLine(""1"");
            IList aux" + nombreAux + " = " + nombre + " as IList;";
                strEncode += @"
            texto.Append(""" + abrir("count");
                strEncode += @""");
            texto.Append(aux" + nombreAux + ".Count);";
                strEncode += @"
            texto.Append(""" + cerrar("count");

                strEncode += @""");
            Console.WriteLine(""2"");
            texto.Append(""" + abrir("tipoDeValue"); // Quizás no hace falta
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeValue");

                strEncode += @""");
            Console.WriteLine(""3"");
            texto.Append(""" + abrir("valores");

                strEncode += @""");
            foreach (" + arguments[0].FullName + " item in aux" + nombreAux + ")";
                strEncode += @"
            {";
                strEncode += @"
            Console.WriteLine(""5"");
                texto.Append(""" + abrir("valor");
                strEncode += @""");";
                getValueXML(arguments[0], nombre, nombreCampo);
                strEncode += @"
            Console.WriteLine(""6"");
                texto.Append(""" + cerrar("valor");
                strEncode += @""");
            }
            Console.WriteLine(""7"");
            texto.Append(""" + cerrar("valores") + "\");";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.Dictionary")) // Dictionary (o hashtable), se codifica con sus claves y valores
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            IDictionary aux" + nombreAux + " = " + nombre + " as IDictionary;";
                strEncode += @"
            texto.Append(""" + abrir("count");
                strDecode += @"
            nr.Read(); // Count";
                strEncode += @""");
            texto.Append(" + nombre + ".Count);";
                strDecode += @"
            nr.Read();
            int length" + nombreAux + " = Int32.Parse(nr.Value);";
                strEncode += @"
            texto.Append(""" + cerrar("count");
                strDecode += @"
            nr.Read(); // Count;";

                // Tipo de índice
                strEncode += @""");
            texto.Append(""" + abrir("tipoDeIndex");
                strDecode += @"
            nr.Read();// Tipo de index";
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strDecode += @"
            nr.Read();
            Type tipoIndice" + nombreAux + " = Type.GetType(nr.Value);";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeIndex");
                strDecode += @"
            nr.Read(); // Tipo de index";

                // Tipo de valor
                strEncode += @""");
            texto.Append(""" + abrir("tipoDeValue");
                strDecode += @"
            nr.Read(); // Tipo de value";
                strEncode += @""");
            texto.Append(""" + arguments[1].FullName + "\");";
                strDecode += @"
            nr.Read();
            Type tipoValor" + nombreAux + " = Type.GetType(nr.Value);";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeValue");
                strDecode += @"
            nr.Read(); // Tipo de value";

                strEncode += @""");
            texto.Append(""" + abrir("valores");
                strDecode += @"
            nr.Read(); // Valores";

                strEncode += @""");
            foreach (KeyValuePair<" + arguments[0].FullName + ", " + arguments[1].FullName + "> par" + nombreAux + " in " + nombre + ")";
                strEncode += @"
            {
                texto.Append(""" + abrir("clave");
                strEncode += @""");
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = par" + nombreAux + ".Key;";

                strDecode += @"
            type = Type.GetType(typeof (Dictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + ">).AssemblyQualifiedName);";
                strDecode += @"
            IDictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + "> dictAux" + nombreAux + " = (IDictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + ">)Activator.CreateInstance(type);";
                strDecode += @"
            // Instanciación del miembro (si es una clase instanciable es imperativo)
            " + nombre + " = new Dictionary<" + arguments[0].FullName + "," + arguments[1].FullName + ">();";
                strDecode += @"
            for (int iaux" + nombreAux + " = 0; iaux" + nombreAux + " < length" + nombreAux + "; iaux" + nombreAux + "++){";
                strDecode += @"
            nr.Read(); // Clave";
                if (arguments[0].FullName == "System.String")
                {
                    strDecode += @"
                " + arguments[0].FullName + " indiceAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                " + arguments[0].FullName + " indiceAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                }

                if (arguments[0].FullName == "System.String")
                {
                    strDecode += @"
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                }


                getValueXML(arguments[0], " elementoAux" + nombreAux, nombre, false, false, true, false);

                strEncode += @"}
                texto.Append(""" + cerrar("clave");
                strDecode += @"
            nr.Read(); // Clave";

                strEncode += @""");
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = par" + nombreAux + ".Value;";
                strDecode += @"
                indiceAux" + nombreAux + " = elementoAux" + nombreAux + ";";
                if (arguments[1].FullName == "System.String")
                {
                    strDecode += @"
                }";
                strDecode += @"
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                }
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = new " + arguments[1].FullName + "();";
                }

                strEncode += @"
                texto.Append(""" + abrir("valor");
                strEncode += @""");";
                strDecode += @"
            nr.Read(); // Valor";

                getValueXML(arguments[1], " elementoAux" + nombreAux, nombre, false, false, false, true);

                strDecode += @"
                " + nombre + ".Add(indiceAux" + nombreAux + ", elementoAux" + nombreAux + ");";
                strDecode += @"
            nr.Read(); // Valor";
                strDecode += @"
                }
            }";
                strEncode += @"
            }";

                strEncode += @"
                texto.Append(""" + cerrar("valor");
                strEncode += @""");
            }
            texto.Append(""" + cerrar("valores") + "\");";
                strDecode += @"
            nr.Read(); // Valores";

            }
            else //if (!t.FullName.StartsWith("System.")) // OBJETOS EXTENOS (debería ser el caso por defecto
            {
                // Hay que generar o invocar el serializador para esta clase
                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
                // Llamar al método encode de la clase SerializadorZZZ
                strEncode += @"
            texto.Append(" + t.Name + "Codec.encode(" + nombre + "));";

                strDecode += @"
            " + t.FullName.Replace("+", ".") + " objAux = new " + t.FullName.Replace("+", ".") + "();";
                strDecode += @"
            string restoXML = nr.ReadInnerXml().ToString();
            " + t.Name + "Codec.decode(ref restoXML, ref objAux);";
                strDecode += @"
//            cont++;
            " + nombre + " = objAux;";

                clases.Add(t, null);
            }
        }

        private void GenerateCodeCSV(MemberInfo[] miembros)
        {
            strEncode += @"
            StringBuilder texto = new StringBuilder();";
            strDecode += @"
            Queue<string> elementos = new Queue<string>(codigo.Split(','));";

            foreach (MemberInfo miembro in miembros)
            {
                if (!esSerializable(miembro))
                {
                    continue;
                }

                Type t;
                string nombre;
                if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                {
                    PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo
                    t = propertyInfo.PropertyType;
                    nombre = propertyInfo.Name;
                }
                else //if(miembro.MemberType == MemberTypes.Field)
                {
                    FieldInfo fieldInfo = miembro as FieldInfo; // Conversión para obtener los datos del tipo de campo
                    t = fieldInfo.FieldType;
                    nombre = fieldInfo.Name;
                }

                strEncode += @"
              if(obj." + nombre + " == null)";
                strEncode += @"
              {
                texto.Append(""NULL,"");
              }
              else
              {
//            texto.Append(""" + nombre + ",\");";
                strEncode += @"
//            texto.Append(""" + t.FullName + ",\");";
                strDecode += @"
              string comprobarNull" + nombre + " = elementos.Peek();";
                strDecode += @"
              if(comprobarNull" + nombre + " == \"NULL\")";
                strDecode += @"
              {";

                strDecode += @"
                elementos.Dequeue(); // Quitamos el NULL
              }
              else
              {
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());";

                getValueCSV(t, "obj." + nombre, nombre);

                strEncode += @"
              } // if(obj.nombre == null) ";

                strDecode += @"
              }// if(comprobarNull" + nombre + " == \"NULL\")";


            } //foreach
            strEncode += @"
            return texto.ToString();
        }";
            strDecode += @"
            codigo = string.Join("","", elementos.ToArray());
        }";

            strCodigo += strEncode;
            strCodigo += this.newLine();
            strCodigo += strDecode;
        }

        protected void getValueCSV(Type t, string nombre, string nombreCampo, bool isArray = false, bool isList = false, bool isDictionaryIndex = false, bool isDictionaryValue = false)
        {
            if (t.IsPrimitive || t.FullName == "System.String" || isDictionaryIndex || isDictionaryValue) // Datos primitivos, simplemente cogemos su valor
            {
                strEncode += @"
            texto.Append(" + nombre + ".ToString() + \",\");";

                if (isArray)
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombreCampo + " = elementos.Dequeue();";
                    }
                    else
                    {
                        strDecode += @"
            " + nombreCampo + " = " + t.Name + ".Parse(elementos.Dequeue());";
                    }
                }
                else if (isList)
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombre + ".Add(elementos.Dequeue());";
                    }
                    else
                    {
                        strDecode += @"
            " + nombre + ".Add(" + t.Name + ".Parse(elementos.Dequeue()));";
                    }
                }
                else
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombre + " = elementos.Dequeue();";
                    }
                    else
                    {
                        strDecode += @"
            " + nombre + " = " + t.Name + ".Parse(elementos.Dequeue());";
                    }
                }
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
                string tipoElemento = t.GetElementType().FullName; // Tipo de los elementos del array
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            texto.Append(" + nombre + ".Length + \",\");";
                strDecode += @"
            int length" + nombreAux + " = Int32.Parse(elementos.Dequeue());";

                strEncode += @"
            texto.Append(""" + tipoElemento + "\" + \",\");";
                strDecode += @"
            tipo = Type.GetType(elementos.Dequeue());";

                strEncode += @"
            texto.Append(""" + t.GetArrayRank() + "\" + \",\");";
                strDecode += @"
            rango = Int32.Parse(elementos.Dequeue());";

                // Definir la longitud de cada rango, y sus límites inferior y superior
                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strEncode += @"
            texto.Append(" + nombre + ".GetLength(" + i + ") + \",\");";
                    strDecode += @"
            int aux" + nombreAux + "Length" + i + " = Int32.Parse(elementos.Dequeue());";

                    strEncode += @"
            texto.Append(" + nombre + ".GetLowerBound(" + i + ") + \",\");";
                    strEncode += @"
            texto.Append(" + nombre + ".GetUpperBound(" + i + ") + \",\");";
                    strDecode += @"
            int aux" + nombreAux + "GetLowerBound" + i + " = Int32.Parse(elementos.Dequeue());";
                    strDecode += @"
            int aux" + nombreAux + "GetUpperBound" + i + " = Int32.Parse(elementos.Dequeue());";
                }

                // Montar tantos FOR anidados como dimensiones tenga el array
                string indices = "[";
                string longitudes = "[";
                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strDecode += @"
            for(int auxIndice" + i + " = aux" + nombreAux + "GetLowerBound" + i + "; auxIndice" + i + " <= aux" + nombreAux + "GetUpperBound" + i + "; auxIndice" + i + "++){";
                    indices += "auxIndice" + i + ",";
                    longitudes += "aux" + nombreAux + "Length" + i + ",";
                }
                indices = indices.Substring(0, indices.Length - 1);
                indices += "]";
                longitudes = longitudes.Substring(0, longitudes.Length - 1);
                longitudes += "]";

                strDecode += @"
            if(" + nombre + " == null) " + nombre + " = new " + tipoElemento + longitudes + ";";
//            if(" + nombre + " == null) " + nombre + " = Array.CreateInstance(" + tipoElemento + ", "  + longitudes + ");";
                strEncode += @"
            foreach (" + tipoElemento + " elementoAux" + nombreAux + " in " + nombre + ")";
                strEncode += @"
            {";

                getValueCSV(t.GetElementType(), "elementoAux" + nombreAux, nombre + indices, true);

                strEncode += @"
            }";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strDecode += @"
            }";
                }

            }
            else if (t.FullName.StartsWith("System.Collections.Generic.List")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
//            IList aux" + nombreAux + " = " + nombre + " as IList;";
                strEncode += @"
            texto.Append(" + nombre + ".Count + \",\");";
                strDecode += @"
            int length" + nombreAux + " = Int32.Parse(elementos.Dequeue());";

                strEncode += @"
            texto.Append(""" + arguments[0].FullName + "\" + \",\");";
                strDecode += @"
            tipo = Type.GetType(elementos.Dequeue());";

                strEncode += @"
            foreach (" + arguments[0].FullName + " elementoAux" + nombreAux + " in " + nombre + ")";
                strEncode += @"
            {";
                strDecode += @"
                type = Type.GetType(typeof (List<" + arguments[0].FullName + ">).AssemblyQualifiedName);";
                strDecode += @"
                IList<" + arguments[0].FullName + "> listaAux" + nombreAux + " = (IList<" + arguments[0].FullName + ">)Activator.CreateInstance(type);";
                strDecode += @"
                " + arguments[0].FullName + " elementoAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                strDecode += @"
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                " + nombre + " = new List<" + arguments[0].FullName + ">();";
                strDecode += @"
            for (int iaux" + nombreAux + " = 0; iaux" + nombreAux + " < length" + nombreAux + "; iaux" + nombreAux + "++)";
                strDecode += @"
            {";
                getValueCSV(arguments[0], " elementoAux" + nombreAux, nombre, false, true);
                strEncode += @"
            }";
                strDecode += @"
                " + nombre + ".Add(elementoAux" + nombreAux + ");";
                strDecode += @"
            }";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.Dictionary")) // Dictionary (o hashtable), se codifica con sus claves y valores
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos

                strEncode += @"
//            IDictionary aux" + nombreAux + " = " + nombre + " as IDictionary;";
                strEncode += @"
            texto.Append(" + nombre + ".Count + \",\");";
                strDecode += @"
            int length" + nombreAux + " = Int32.Parse(elementos.Dequeue());";

                // Tipo del índice
                strEncode += @"
            texto.Append(""" + arguments[0].FullName + "\" + \",\");";
                strDecode += @"
            Type tipoIndice = Type.GetType(elementos.Dequeue());";

                // Tipo del valor
                strEncode += @"
            texto.Append(""" + arguments[1].FullName + "\" + \",\");";
                strDecode += @"
            Type tipoValor = Type.GetType(elementos.Dequeue());";

                strEncode += @"
            foreach (KeyValuePair<" + arguments[0].FullName + ", " + arguments[1].FullName + "> par" + nombreAux + " in " + nombre + ")";
                strEncode += @"
            {";
                strEncode += @"
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = par" + nombreAux + ".Key;";

                strDecode += @"
                type = Type.GetType(typeof (Dictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + ">).AssemblyQualifiedName);";
                strDecode += @"
                IDictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + "> dictAux" + nombreAux + " = (IDictionary<" + arguments[0].FullName + ", " + arguments[1].FullName + ">)Activator.CreateInstance(type);";
                strDecode += @"
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                " + nombre + " = new Dictionary<" + arguments[0].FullName + "," + arguments[1].FullName + ">();";
                strDecode += @"
            for (int iaux" + nombreAux + " = 0; iaux" + nombreAux + " < length" + nombreAux + "; iaux" + nombreAux + "++)";
                strDecode += @"
            {";
                if (arguments[0].FullName == "System.String")
                {
                    strDecode += @"
                " + arguments[0].FullName + " indiceAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                " + arguments[0].FullName + " indiceAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                }

                if (arguments[0].FullName == "System.String")
                {
                    strDecode += @"
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                {" + arguments[0].FullName + " elementoAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                }

                getValueCSV(arguments[0], " elementoAux" + nombreAux, nombre, false, false, true, false);

                strEncode += @"}
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = par" + nombreAux + ".Value;";
                strDecode += @"
                indiceAux" + nombreAux + " = elementoAux" + nombreAux + ";";
                if (arguments[1].FullName == "System.String")
                {
                    strDecode += @"
                }
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = \"\";";
                }
                else
                {
                    strDecode += @"
                }
                {" + arguments[1].FullName + " elementoAux" + nombreAux + " = new " + arguments[1].FullName + "();";
                }

                getValueCSV(arguments[1], " elementoAux" + nombreAux, nombre, false, false, false, true);

                strDecode += @"
                " + nombre + ".Add(indiceAux" + nombreAux + ", elementoAux" + nombreAux + ");";
                strDecode += @"
                }
            }";
                strEncode += @"}
            }";
            }
            else //if (!t.FullName.StartsWith("System.")) // OBJETOS EXTENOS (debería ser el caso por defecto
            {
                // Hay que generar o invocar el serializador para esta clase
                // Todos los serializadores ya creados están en un dictionary con el nombre del serializador y el objeto
                // Si no existe el serializador adecuado en este dictionary, hay que invocarlo, compilarlo, y meterlo en él
                // Llamar al método encode de la clase SerializadorZZZ
                strEncode += @"
            texto.Append(" + t.Name + "Codec.encode(" + nombre + "));";

                strDecode += @"
            " + t.FullName.Replace("+", ".") + " objAux" + nombreCampo.Replace(".", "").Replace("[", "").Replace("]","") + " = new " + t.FullName.Replace("+", ".") + "();";
                strDecode += @"
            string aux = string.Join("","", elementos.ToArray());
            " + t.Name + "Codec.decode(ref aux, ref objAux" + nombreCampo.Replace(".", "").Replace("[", "").Replace("]", "") + ");";
                strDecode += @"
            elementos = new Queue<string>(aux.Split(','));
            " + nombreCampo + " = objAux" + nombreCampo.Replace(".", "").Replace("[", "").Replace("]", "") + " as " + t.FullName.Replace("+", ".") + ";";

                clases.Add(t, null);
            }
        }

        private bool esSerializable(MemberInfo miembro)
        {
            bool serializar = true;
            if (miembro.Name.Contains("_BackingField")) return false;
            if (miembro.MemberType != MemberTypes.Property && miembro.MemberType != MemberTypes.Field) return false;

            // Identificar atributos estándar de serialización 
            IEnumerable<System.Attribute> attrs = miembro.GetCustomAttributes();
            foreach (System.Attribute attr in attrs)
            {
                if (attr is NonSerializedAttribute) return false;
                if (attr is ObsoleteAttribute) return false;
            }
            return serializar;
        }

        private string abrir(string texto)
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

        private string cerrar(string texto)
        {
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

        private string getAccesibilidad(Type tipo)
        {
            string accesibilidad = "";
            string codigo = "";
            // Accesibilidad: public, protected, internal, protected internal o private
            if (tipo.IsPublic)
            {
                accesibilidad += "public";
            }
            else if (tipo.IsNotPublic)
            {
                accesibilidad += "private";
            }
            else
            {
                accesibilidad += "protected";
            }
            codigo += accesibilidad;
            return codigo;
        }

        private string getModificador(Type tipo, ref bool pintar)
        {
            string modificador = "";
            string codigo = "";
            pintar = true;
            // Modificadores: abstract, async, override, parcial, readonly, sealed, static, unsafe, virtual, volatile
            if (tipo.IsAbstract)
            {
                modificador += "abstract";
            }
            else if (tipo.IsSealed)
            {
                modificador += "sealed";
            }
            else
            {
                modificador += "";
                pintar = false;
            }
            codigo += modificador;
            return codigo;
        }

        private string getTipoDeObjeto(Type tipo)
        {
            string tipoDeObjeto = "";
            string codigo = "";
            // Identificación del tipo de objeto: clase, interface, enum, struct
            if (tipo.IsPrimitive)
            {
                TypeAttributes atr = tipo.Attributes;
                tipoDeObjeto += atr.ToString();
            }
            else if (tipo.IsInterface)
            {
                tipoDeObjeto += "interface";
            }
            else if (tipo.IsEnum)
            {
                tipoDeObjeto += "enum";
            }
            else if (tipo.IsValueType)
            {
                if (tipo.IsEnum)
                {
                    tipoDeObjeto += "enum";
                }
                else
                {
                    tipoDeObjeto += "struct";
                }
            }
            else if (tipo.IsClass)
            {
                tipoDeObjeto += "class";
            }
            else
            {
                tipoDeObjeto += "unknown";
            }
            codigo += tipoDeObjeto;
            return codigo;
        }

        private string mostrarValor(object texto)
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
