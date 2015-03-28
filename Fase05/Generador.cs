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

            // 4º Generar los métodos auxiliares
            this.generateAuxiliarMethods();
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
        public static Type tipo = Type.GetType(""" + this.tipo.FullName;
            strCodigo += @""");
        public static String nombreClase = """ + this.tipo.FullName;
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
        public void encode(object obj, ref String str){";
//            strEncode += "public void encode(object obj, ref string str){";
            strDecode += @"
        public object decode(String str, object obj){";

            strEncode += @"
              " + this.tipo.FullName + " aux = (" + this.tipo.FullName + ")obj;";
            strEncode += @"
                texto += """ + abrir("serializacion");
            strEncode += @"""
                texto += """ + abrir("accesibilidad");
            strEncode += @"""
                texto += """ + getAccesibilidad(tipo);
            strEncode += @"""
                texto += """ + cerrar("accesibilidad");

            // Modificador, opcional
            bool pintar = false;
            string modificador = getModificador(tipo, ref pintar);
            if (pintar)
            {
                strEncode += @"""
                texto += """ + abrir("modificador");
                strEncode += @"""
                texto += """ + modificador;
                strEncode += @"""
                texto += """ + cerrar("modificador");
            }
            strEncode += @"""
                texto += """ + abrir("tipoDeObjeto");
            strEncode += @"""
                texto += """ + getTipoDeObjeto(tipo);
            strEncode += @"""
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
            texto += """ + getCodigoByMembers(miembros);
            strEncode += @""";
            texto += """ + cerrar("serializacion");
            strEncode += @""";
            Console.WriteLine(texto);
            str = texto;";

// strDecode ...
        }

        private static string abrir(string texto)
        {
            string codigo = SALTO;
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
        private string getCodigoByMembers(MemberInfo[] miembros)
        {
            string codigo = "";
            codigo += abrir("elementos");
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!miembro.Name.Contains("_BackingField")) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
                {
                    if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                    {
                        PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo

                        codigo += abrir("elemento");

                        codigo += abrir("nombre");
                        codigo += mostrarValor(propertyInfo.Name);
                        codigo += cerrar("nombre");

                        codigo += abrir("tipoDeObjeto");
                        codigo += mostrarValor(propertyInfo.DeclaringType);
                        codigo += cerrar("tipoDeObjeto");

                        codigo += abrir("tipoDeElemento");
                        codigo += mostrarValor("propiedad");
                        codigo += cerrar("tipoDeElemento");

                        codigo += abrir("tipo");
                        codigo += mostrarValor(propertyInfo.PropertyType.FullName);
                        codigo += cerrar("tipo");

                        codigo += abrir("isArray");
                        codigo += mostrarValor(propertyInfo.PropertyType.IsArray);
                        codigo += cerrar("isArray");

                        codigo += abrir("valor");
                        /*
                         * Comprobaciones adicionales
                         * Si el tipo de este miembro es un iList, mostramos más valores (length, etc.)
                         * Si el tipo de este miembro es un IDictionary, tomamos sus valores (de la forma key, values)
                         * Si el tipo de este miembro es una clase, tendremos que llamar recursivamente a getCodigoByMembers con los miembros de la clase
                         */
                        IList list = propertyInfo.GetValue(obj, null) as IList;
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
                                codigo += mostrarValor(propertyInfo.GetValue(obj, null));
                            }
                        }
                        codigo += cerrar("valor");
                        codigo += cerrar("elemento");
                    }
                }
            } //foreach
        } //getCodigoByMembers()
    }
}
