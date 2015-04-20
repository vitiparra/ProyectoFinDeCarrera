﻿using System;
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
            Console.WriteLine(strCodigo);
            // 2º Compilar e instanciar la clase serializador
            using (StreamWriter writer = new StreamWriter("filename.txt"))
            {
                 writer.Write(strCodigo);
            }
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
//                    this.generateAuxiliarMethods();
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
            int rango;
            string nombre;";

            BindingFlags flags = BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.DeclaredOnly
                | BindingFlags.FlattenHierarchy
                | BindingFlags.Static;
            // Se serializan todos los miembros públicos; propios y heredados (CONFIRMAR)
            MemberInfo[] miembros = tipo.GetMembers(flags);
            switch(this.tipoDeCodificacion)
            {
                case tiposDeCodificacion.XML:
                    GenerateCodeXML(miembros);
                    break;
                case tiposDeCodificacion.CSV:
                default:
                    GenerateCodeCSV(miembros);
                    break;

            }
        }

        private void GenerateCodeXML(MemberInfo[] miembros)
        {
            strEncode += @"
            StringBuilder texto = new StringBuilder(""" + abrir("elementos") + "\");";

            switch (this.tipoDeCodificacion) 
            {
                // Aquí vendrán todas las modalidades de codificación (JSON, CSV, Binary, etc.)
                case tiposDeCodificacion.XML:
                default:
                    strDecode += @"
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            Console.WriteLine(""2"");
            xml.LoadXml(str);
            Console.WriteLine(""3"");
            XmlNode nodoPrincipal = xml.SelectSingleNode(""elementos"");
            Console.WriteLine(""4"");
            XmlNodeReader nr = new XmlNodeReader(nodoPrincipal);
            Console.WriteLine(""5"");";
                    break;
            }

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
            texto.Append(""" + abrir("elemento");

                            strDecode += @"

            Console.WriteLine(""6"");
            nr.Read();";

                            strEncode += @""");
            texto.Append(""" + abrir("nombre");
                            strEncode += @""");
            texto.Append(""" + mostrarValor(nombre);
                            strEncode += @""");
            texto.Append(""" + cerrar("nombre");

                            strEncode += @""");
            texto.Append(""" + abrir("tipo");
                            strEncode += @""");
            texto.Append(""" + mostrarValor(t.FullName);
                            strEncode += @""");
            texto.Append(""" + cerrar("tipo");

                            strEncode += @""");
            texto.Append(""" + abrir("valor") + "\");";

                            getValueXML(t, "obj." + nombre, nombre);

                            strEncode += @"
            texto.Append(""" + cerrar("valor") + "\");";
                            strEncode += @"
            texto.Append(""" + cerrar("elemento") + "\");";
                        }
                    }
                }
            } //foreach
            strEncode += @"
            texto.Append(""" + cerrar("elementos");
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
            // strDecode ...

        }

        protected void getValueXML(Type t, string nombre, string nombreCampo, bool isArrayOrList = false)
        {
            if (t.IsPrimitive || t.FullName == "System.String") // Datos primitivos, simplemente cogemos su valor
            {
                strEncode += @"
            texto.Append(" + nombre + ".ToString());";
                if (isArrayOrList)
                {
                    strDecode += @"
            Console.WriteLine(""7"");
            nr.Read();";
                    strDecode += @"
            " + nombre + ".SetValue(" + t.Name + ".Parse(nr.Value), i" + nombreCampo + ");";
                    strDecode += @"";
                }
                else
                {
                    strDecode += @"
            Console.WriteLine(""8"");
            Console.WriteLine(nr.Value);
            " + nombre + " = " + t.Name + ".Parse(nr.Value);";
                    strDecode += @"
            Console.WriteLine(""8b"");";
                }
            }
            else if (t.IsArray) // Array, se codifica con sus parámetros (longitud, tipo de datos, rango, etc.) y sus datos
            {
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            Array aux" + nombreAux + " = " + nombre + " as Array;";
                strDecode += @"
            Console.WriteLine(""9"");
            Array aux" + nombreAux + " = " + nombre + " as Array;";

                strEncode += @"
            texto.Append(""" + abrir("count");

                strEncode += @""");
            texto.Append(aux" + nombreAux + ".Length);";
                strDecode += @"
            Console.WriteLine(""10"");
            nr.Read();
            count = Int32.Parse(nr.Value);";
                strEncode += @"
            texto.Append(""" + cerrar("count");

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeElementos");
                strEncode += @""");
            texto.Append(""" + t.GetElementType().FullName;
                strDecode += @"
            Console.WriteLine(""11"");
            nr.Read();
            tipo = Type.GetType(nr.Value);";
                strEncode += @""");
            texto.Append(""" + cerrar("tipoDeElementos");

                strEncode += @""");
            texto.Append(""" + abrir("rank");
                strEncode += @""");
            texto.Append(""" + t.GetArrayRank();
                strDecode += @"
            Console.WriteLine(""12"");
            nr.Read();
            rango = Int32.Parse(nr.Value);";
                strEncode += @""");
            texto.Append(""" + cerrar("rank");

                strEncode += @""");
            texto.Append(""" + abrir("datosDeLosRangos");
                strEncode += @""");";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strEncode += @"
            texto.Append(""" + abrir("datosDeRango");
                    strEncode += @""");
            texto.Append(""" + abrir("longitud");
                    strEncode += @""");
            texto.Append(aux" + nombreAux + ".GetLength(" + i + "));";
                    strEncode += @"
            texto.Append(""" + cerrar("longitud");

                    strEncode += @""");
            texto.Append(""" + abrir("valorMenor");
                    strEncode += @""");
            texto.Append(aux" + nombreAux + ".GetLowerBound(" + i + "));";
                    strEncode += @"
            texto.Append(""" + cerrar("valorMenor");

                    strEncode += @""");
            texto.Append(""" + cerrar("datosDeRango");
                    strEncode += @""");
            ";
                }
                strEncode += "texto.Append(\"" + cerrar("datosDeLosRangos") + "";

                strEncode += @""");
            texto.Append(""" + abrir("valores");
                strEncode += @""");
            foreach (object elementoAux" + nombreAux + " in aux" + nombreAux + ")";
                strEncode += @"
            {
                texto.Append(""" + abrir("cadaValor") + "\");";
                strDecode += @"
            Console.WriteLine(""13"");
            for (int i" + nombreAux + " = 0; i" + nombreAux + " < count; i" + nombreAux + "++)";
                strDecode += @"
            {";

                getValueXML(t.GetElementType(), "aux" + nombreAux, nombreAux, true);

                strEncode += @"
                texto.Append(""" + cerrar("valor") + "\");";
                strEncode += @"
                texto.Append(""" + cerrar("cadaValor");
                strEncode += @""");
            }";


                strDecode += @"
            Console.WriteLine(""14"");
            }
            // Volcar el array auxiliar a su correspondiente en el objeto
            " + nombre + " = aux" + nombreAux + " as " + t.FullName + ";";
                strEncode += @"
            texto.Append(""" + cerrar("valores") + "\");";
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
                strEncode += @""");
            texto.Append(aux" + nombreAux + ".Count);";
                strEncode += @"
            texto.Append(""" + cerrar("count");

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeIndex"); // Quizás no hace falta
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeIndex");

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeValue"); // Quizás no hace falta
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeValue");

                strEncode += @""");
            texto.Append(""" + abrir("valores");

                strEncode += @""");
            foreach (DictionaryEntry item in aux" + nombreAux + ")";
                strEncode += @"
            {
                texto.Append(""" + abrir("clave");
                strEncode += @""");";
                getValueXML(arguments[0], nombre, nombreCampo);
                strEncode += @"
                texto.Append(""" + cerrar("clave");

                strEncode += @""");
                texto.Append(""" + abrir("valor");
                strEncode += @""");";
                getValueXML(arguments[0], nombre, nombreCampo);
                strEncode += @"
                texto.Append(""" + cerrar("valor");
                strEncode += @""");
            }
            texto.Append(""" + cerrar("valores") + "\");";

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
            " + t.Name + "Codec.decode(nr, ref objAux);";
                strDecode += @"
            cont++;
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
//            texto.Append(""" + nombre + ",\");";
                            strEncode += @"
//            texto.Append(""" + t.FullName + ",\");";
                            strDecode += @"
//            nombre = elementos.Dequeue();
//            tipo = Type.GetType(elementos.Dequeue());";
                            getValueCSV(t, "obj." + nombre, nombre);
                        }
                    }
                }
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

        protected void getValueCSV(Type t, string nombre, string nombreCampo, bool isArray = false, bool isList = false)
        {
            if (t.IsPrimitive || t.FullName == "System.String") // Datos primitivos, simplemente cogemos su valor
            {
                strEncode += @"
            texto.Append(" + nombre + ".ToString() + \",\");";

                if (isArray)
                {
                    if (t.FullName == "System.String")
                    {
                        strDecode += @"
            " + nombreCampo + ".SetValue(elementos.Dequeue(), i" + nombreCampo + ");";
                    }
                    else
                    {
                        strDecode += @"
            " + nombreCampo + ".SetValue(" + t.Name + ".Parse(elementos.Dequeue()), i" + nombreCampo + ");";
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
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            Array aux" + nombreAux + " = " + nombre + " as Array;";
                strDecode += @"
            Array aux" + nombreAux + " = " + nombre + " as Array;";

                strEncode += @"
            texto.Append(aux" + nombreAux + ".Length + \",\");";
                strDecode += @"
            int length" + nombreAux + " = Int32.Parse(elementos.Dequeue());";

                strEncode += @"
            texto.Append(""" + t.GetElementType().FullName + "\" + \",\");";
                strDecode += @"
            tipo = Type.GetType(elementos.Dequeue());";
 
                strEncode += @"
//            texto.Append(""" + t.GetArrayRank() + "\" + \",\");" ;
                strDecode += @"
//            rango = Int32.Parse(elementos.Dequeue());";

                for (int i = 0; i < t.GetArrayRank(); i++)
                {
                    strEncode += @"
//            texto.Append(aux" + nombreAux + ".GetLength(" + i + ") + \",\");";
                    strDecode += @"
//            int aux" + nombreAux + "Length" + i + " = Int32.Parse(elementos.Dequeue());";

                    strEncode += @"
//            texto.Append(aux" + nombreAux + ".GetLowerBound(" + i + ") + \",\");";
                    strDecode += @"
//            int aux" + nombreAux + "GetLowerBound" + i + " = Int32.Parse(elementos.Dequeue());";
                }

                strEncode += @"
            foreach (" + t.GetElementType().FullName + " elementoAux" + nombreAux + " in aux" + nombreAux + ")";
                strEncode += @"
            {";
                strDecode += @"
            aux" + nombreAux + " = Array.CreateInstance(tipo, length" + nombreAux + ");";
                strDecode += @"
            for (int iaux" + nombreAux + " = 0; iaux" + nombreAux + " < length" + nombreAux + "; iaux" + nombreAux + "++)";
                strDecode += @"
            {";

                getValueCSV(t.GetElementType(), "elementoAux" + nombreAux, "aux" + nombreAux, true);

                strEncode += @"
            }";
                strDecode += @"
            }
            // Volcar el array auxiliar a su correspondiente en el objeto
            " + nombre + " = aux" + nombreAux + " as " + t.FullName + ";";
            }
            else if (t.FullName.StartsWith("System.Collections.Generic.List")) // Lista, se codifica con sus parámetros (longitud, tipo de datos, etc.) y sus datos
            {
                Type[] arguments = t.GetGenericArguments();
                string nombreAux = nombre.Replace(".", ""); // Necesario para evitar que se procesen nombres con puntos
                strEncode += @"
            IList aux" + nombreAux + " = " + nombre + " as IList;";
                strEncode += @"
            texto.Append(aux" + nombreAux + ".Count + \",\");";
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
                var type = Type.GetType(typeof (List<" + arguments[0].FullName + ">).AssemblyQualifiedName);";
                strDecode += @"
                IList<" + arguments[0].FullName + "> listaAux" + nombreAux + " = (IList<" + arguments[0].FullName + ">)Activator.CreateInstance(type);";
                strDecode += @"
                " + arguments[0].FullName + " elementoAux" + nombreAux + " = new " + arguments[0].FullName + "();";
                strDecode += @"
                // Instanciación del miembro (si es una clase instanciable es imperativo)
                " + nombre + " = (List<" + arguments[0].FullName + ">)Activator.CreateInstance(type);";
                strDecode += @"
            for (int iaux" + nombreAux + " = 0; iaux" + nombreAux + " < length" + nombreAux + "; iaux" + nombreAux + "++)";
                strDecode += @"
            {";
                getValueCSV(arguments[0], " elementoAux" + nombreAux, "aux" + nombreAux, false, true);
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
            IDictionary aux" + nombreAux + " = " + nombre + " as IDictionary;";
                strEncode += @"
            texto.Append(""" + abrir("count");
                strEncode += @""");
            texto.Append(aux" + nombreAux + ".Count);";
                strEncode += @"
            texto.Append(""" + cerrar("count");

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeIndex"); // Quizás no hace falta
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeIndex");

                strEncode += @""");
            texto.Append(""" + abrir("tipoDeValue"); // Quizás no hace falta
                strEncode += @""");
            texto.Append(""" + arguments[0].FullName + "\");";
                strEncode += @"
            texto.Append(""" + cerrar("tipoDeValue");

                strEncode += @""");
            texto.Append(""" + abrir("valores");

                strEncode += @""");
            foreach (DictionaryEntry item in aux" + nombreAux + ")";
                strEncode += @"
            {
                texto.Append(""" + abrir("clave");
                strEncode += @""");";
                getValueCSV(arguments[0], nombre, nombreCampo);
                strEncode += @"
                texto.Append(""" + cerrar("clave");

                strEncode += @""");
                texto.Append(""" + abrir("valor");
                strEncode += @""");";
                getValueCSV(arguments[0], nombre, nombreCampo);
                strEncode += @"
                texto.Append(""" + cerrar("valor");
                strEncode += @""");
            }
            texto.Append(""" + cerrar("valores") + "\");";

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
            " + t.FullName.Replace("+", ".") + " objAux" + nombreCampo + " = new " + t.FullName.Replace("+", ".") + "();";
                strDecode += @"
            string aux = string.Join("","", elementos.ToArray());
            " + t.Name + "Codec.decode(ref aux, ref objAux" + nombreCampo + ");";
                strDecode += @"
            elementos = new Queue<string>(aux.Split(','));
            " + nombre + " = objAux" + nombreCampo + " as " + t.FullName.Replace("+", ".") + ";";

                clases.Add(t, null);
            }
        }

        private bool esSerializable(MemberInfo miembro)
        {
            return !miembro.Name.Contains("_BackingField") && (miembro.MemberType == MemberTypes.Property || miembro.MemberType == MemberTypes.Field);
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

        private string cerrar(string texto){
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

        private string getAccesibilidad(Type tipo){
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

        private string getModificador(Type tipo, ref bool pintar){
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

        private string getTipoDeObjeto(Type tipo){
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
