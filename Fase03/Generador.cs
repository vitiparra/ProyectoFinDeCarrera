using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

namespace Fase03
{
    /*
     * Clase que genera el código de una clase codificador/decodificador para una determinada clase que se recibe como parámetro
     * En un primer momento, el código de dicha clase se genera y se guarda en un atributo de esta clase
     */
    class Generador
    {
        private Object clase { get; set; }
        private Type tipoDeClase { get; set; }
        private string nombreClase { get; set; }
        private string codigoGenerado { get; set; }
        private string tipoDeSalida = "XML"; // Por defecto

        private string TAB = "\t";
        private string SALTO = "\r\n";

        public Generador(Object clase)
        {
            this.clase = clase; // Esto sobra
            this.tipoDeClase = clase.GetType(); // Este será el parámetro que se reciba en el constructor
            this.nombreClase = tipoDeClase.Name;

            // Encode
            this.codigoGenerado = "public class ";
            this.codigoGenerado += this.nombreClase + "Codec {";

            this.codigoGenerado += this.SALTO + this.TAB;
            this.codigoGenerado += "public String encode(" + this.tipoDeClase + " obj){";

            this.codigoGenerado += this.SALTO + this.TAB + this.TAB;
            this.codigoGenerado += "String texto = \"\";";

            this.codigoGenerado += this.abrir("serializacion");

            this.codigoGenerado += this.abrir("accesibilidad");

            this.codigoGenerado += this.getAccesibilidad(this.tipoDeClase);

            this.codigoGenerado += this.cerrar("accesibilidad");

            // Modificador, opcional
            bool pintar = false;
            string modificador = this.getModificador(this.tipoDeClase, ref pintar);
            if (pintar)
            {
                this.codigoGenerado += this.abrir("modificador");
                this.codigoGenerado += modificador;
                this.codigoGenerado += this.cerrar("modificador");
            }

            this.codigoGenerado += this.abrir("tipoDeObjeto");

            this.codigoGenerado += this.getTipoDeObjeto(this.tipoDeClase);

            this.codigoGenerado += this.cerrar("tipoDeObjeto");

            // Se obtiene el código de todos los miembros públicos, privados y estáticos; propios y heredados
            MemberInfo[] miembros = this.tipoDeClase.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            this.codigoGenerado += this.getCodigoByMembers(miembros, this.clase);

            this.codigoGenerado += this.cerrar("serializacion");
            // Aquí acaba el serializador ------------------------------
            // String a stream (esto no hace falta, es para unir ahora this.codigoGenerado con str)
            byte[] byteArray = Encoding.UTF8.GetBytes(this.codigoGenerado);
            MemoryStream str = new MemoryStream(byteArray);
            // Leer el stream
            StreamReader responseReader = new StreamReader(str);
            string responseString = responseReader.ReadToEnd();

            // Decode
            this.codigoGenerado += this.SALTO + this.TAB;

            this.codigoGenerado += "public void decode(stream str, ref " + this.tipoDeClase + " obj){";

            this.codigoGenerado += this.SALTO + this.TAB + this.TAB;
            this.codigoGenerado += "String texto = \"\";";

            this.codigoGenerado += this.SALTO + this.TAB + this.TAB;
            this.codigoGenerado += this.decodeString(responseString, this.clase);

            this.codigoGenerado += this.SALTO;
            this.codigoGenerado += "}";

            //            this.codigoGenerado += this.getPropiedades(this.tipoDeClase);
            //            this.codigoGenerado += this.getCampos(this.tipoDeClase);
            /*
             * Trabajar con el objeto a partir de su tipo
             * No funciona, no se reconoce el parámetro de typeof como un tipo válido
             */
            //            Type myType = typeof(this.tipoDeClase.GetType());

            /*
             * Trabajar con los campos del objeto ¿? Parece que no hay ¿qué son campos de un objeto?
             */
            /*            FieldInfo[] campos = this.tipoDeClase.GetFields();
                        foreach (FieldInfo campo in campos)
                        {
                            this.codigoGenerado += "campos";
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += "CAMPOS";
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.FieldType + " - " + campo.Name;
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.MemberType; // Indica el tipo de miembro que es
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.IsStatic; // Indica si el campo es estático o no (TRUE, FALSE)
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.IsPrivate; // Indica si el campo es privado o no (TRUE, FALSE)
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.GetType(); // Tipo de objeto del campo
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += campo.FieldType; // Tipo de objeto del campo
                            this.codigoGenerado += this.SALTO;

                        }
            */
            /*
             * Propiedades de la clase
             */
            /*
                        PropertyInfo[] atributos = tipoClase.GetProperties();
                        foreach (PropertyInfo atributo in atributos)
                        {
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.PropertyType + " - " + atributo.Name;
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.Attributes; // Atributos del elemento. Para variables normales, None
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.CanRead; // Indica si el elemento se puede leer (TRUE, FALSE)
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.CanWrite; // Indica si el elemento se puede escribir (TRUE, FALSE)
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.DeclaringType; // El tipo del elemento (su clase)
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.GetGetMethod(); // El método GET para ese elemento
                            this.codigoGenerado += this.SALTO;
                            this.codigoGenerado += atributo.IsSpecialName; // Indica si es un nombre especial ¿?
                            this.codigoGenerado += this.SALTO;
                
                        }
            */

            Console.WriteLine(this.codigoGenerado);
        }

        private string getAccesibilidad(Type tipo)
        {
            string accesibilidad = "";
            string codigo = "";
            codigo += this.SALTO + this.TAB;
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
            codigo += "texto += \"" + accesibilidad + "\";";
            return codigo;
        }

        private string getModificador(Type tipo, ref bool pintar)
        {
            string modificador = "";
            string codigo = "";
            codigo += this.SALTO + this.TAB;

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
            codigo += "texto += \"" + modificador + "\";";
            return codigo;
        }

        private string getTipoDeObjeto(Type tipo)
        {
            string tipoDeObjeto = "";
            string codigo = "";
            codigo += this.SALTO + this.TAB;

            // Identificación del tipo de objeto: clase, interface, enum, struct
            if (tipo.IsClass)
            {
                tipoDeObjeto += "class ";
            }
            else if (tipo.IsInterface)
            {
                tipoDeObjeto += "interface ";
            }
            else if (tipo.IsEnum)
            {
                tipoDeObjeto += "enum ";
            }
            else if (tipo.IsValueType)
            {
                if (tipo.IsPrimitive)
                {
                    TypeAttributes atr = tipo.Attributes;
                    tipoDeObjeto += atr.ToString();
                }
                else if (tipo.IsEnum)
                {
                    tipoDeObjeto += "enum ";
                }
                else
                {
                    tipoDeObjeto += "struct ";
                }
            }
            codigo += "texto += \"" + tipoDeObjeto + "\";";
            return codigo;
        }

        private string getPropiedades(Type tipo)
        {
            string codigo = "";
            PropertyInfo[] propiedades;

            // Propiedades públicas
            codigo += this.SALTO;
            codigo += "//[PROPIEDADES PÚBLICAS]";
            codigo += this.SALTO;
            propiedades = this.tipoDeClase.GetProperties();
            foreach (PropertyInfo atributo in propiedades)
            {
                MethodInfo metodo = atributo.GetGetMethod();
                //                String ambito = metodo.GetMethodBody().GetILAsByteArray().ToString();
                String ambito = "";
                ambito = this.getAmbito(metodo);
                codigo += this.SALTO + this.TAB + this.TAB;
                codigo += "texto += \"{{\"" + ambito + "\"}, {" + atributo.PropertyType + "},{\" + obj." + atributo.Name + " + \"},{\"" + atributo.GetValue(this.clase, null) + "\"}},\";";
            }

            // Propiedades privadas y protegidas
            codigo += this.SALTO;
            codigo += "//[PROPIEDADES PRIVADAS Y PROTEGIDAS]";
            codigo += this.SALTO;
            propiedades = this.tipoDeClase.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (PropertyInfo atributo in propiedades)
            {
                String ambito = "";
                if (atributo.GetGetMethod() != null)
                {
                    ambito = this.getAmbito(atributo.GetGetMethod());
                }
                codigo += this.SALTO + this.TAB + this.TAB;
                codigo += "texto += \"{{\"" + ambito + "\"}, {" + atributo.PropertyType + "},{\" + obj." + atributo.Name + " + \"},{\"" + atributo.GetValue(this.clase, null) + "\"}},\";";
            }

            // Propiedades privadas estáticas
            codigo += this.SALTO;
            codigo += "//[PROPIEDADES PRIVADAS ESTÁTICAS]";
            codigo += this.SALTO;
            //            FieldInfo[] fieldInfos = this.tipoDeClase.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo[] fieldInfos = this.tipoDeClase.GetFields(BindingFlags.Static | BindingFlags.NonPublic);
            foreach (FieldInfo field in fieldInfos)
            {
                String ambito = this.getAmbitoField(field);
                codigo += this.SALTO + this.TAB + this.TAB;
                codigo += "texto += \"{{\"" + ambito + "\"}, {" + field.FieldType + "},{\" + obj." + field.Name + " + \"},{\"" + field.GetValue(this.clase) + "\"}},\";";
            }


            // Propiedades públicas estáticas
            codigo += this.SALTO;
            codigo += "//[PROPIEDADES PÚBLICAS ESTÁTICAS]";
            codigo += this.SALTO;
            //            FieldInfo[] fieldInfos = this.tipoDeClase.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfos = this.tipoDeClase.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fieldInfos)
            {
                String ambito = this.getAmbitoField(field);
                codigo += this.SALTO + this.TAB + this.TAB;
                codigo += "texto += \"{{\"" + ambito + "\"}, {" + field.FieldType + "},{\" + obj." + field.Name + " + \"},{\"" + field.GetValue(this.clase) + "\"}},\";";
            }

            // Otras propiedades complejas (OJO, aparecen también estáticas)
            codigo += this.SALTO;
            codigo += "[MIEMBROS]";
            codigo += this.SALTO;
            MemberInfo[] miembros = this.tipoDeClase.GetMembers();
            foreach (MemberInfo miembro in miembros)
            {
                if (miembro.MemberType.ToString() != "Method"
                 && miembro.MemberType.ToString() != "Constructor"
                 && miembro.MemberType.ToString() != "Property" // Ya estaban identificadas antes con PropertyInfo
                )
                {
                    codigo += this.SALTO + this.TAB + this.TAB;
                    /*
                                        Type tipo1 = Type.GetType(miembro.GetProperty("FieldType").ToString());
                                        codigo += "Tipo de miembro " + tipo1.ToString();
                    */
                    MemberTypes tipoDeMiembro = miembro.MemberType;
                    if (tipoDeMiembro.ToString() == "NestedType")
                    {
                        if (tipoDeMiembro.GetType().IsEnum)
                        {
                            codigo += this.SALTO + this.TAB + this.TAB;
                            codigo += "// A continuación se declara el tipo " + miembro.GetType().ReflectedType + " - " + miembro.Name + "";
                            codigo += Type.GetTypeCode(miembro.GetType()).ToString();
                        }
                    }
                    else
                    {
                        codigo += this.SALTO + this.TAB + this.TAB;
                        codigo += "texto += \"{{" + miembro.MemberType.ToString() + "},{\" + obj." + miembro.Name + " + \"}},\";";
                    }
                    /*                codigo += miembro.ReflectedType; // Indica el tipo de miembro que es
                                    codigo += this.SALTO;
                                    codigo += miembro.DeclaringType; // Indica la clase de este miembro
                                    codigo += this.SALTO;
                                    codigo += miembro.GetType(); // Tipo de objeto del campo
                                    codigo += this.SALTO;
                    */
                }
            }
            return codigo;
        }

        /*
         * Captura la información de cada miembro de un tipo, incluyendo sus valores
         * @input MemberInfo[] miembros Conjunto de miembros de un tipo susceptibles de ser serializados tipo.GetMembers(flags)
         * @input Object obj Objeto instanciado para obtener los valores
         * @output string Código serializado con todos los miembros del objeto indicado
         */
        // Método OK para obtener todos los miembros (propiedades y variables) susceptibles de ser serializados
        private string getCodigoByMembers(MemberInfo[] miembros, Object obj)
        {
            string codigo = "";
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!miembro.Name.Contains("_BackingField")) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar "miembros")
                {
                    if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                    {
                        PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo

                        codigo += this.abrir("propiedad");

                        codigo += this.abrir("nombre");
                        codigo += this.mostrarValor(propertyInfo.Name);
                        codigo += this.cerrar("nombre");

                        codigo += this.abrir("tipo");
                        codigo += this.mostrarValor(propertyInfo.PropertyType.Name);
                        codigo += this.cerrar("tipo");

                        /*
                         * Comprobaciones adicionales
                         * Si el tipo de este miembro es un iList, mostramos más valores (length, etc.)
                         * Si el tipo de este miembro es una clase,tendremos que llamar recursivamente a getCodigoByMembers con los miembros de la clase
                         */
                        IList list = propertyInfo.GetValue(obj, null) as IList;
                        if (list != null)
                        {
                            codigo += this.recorrerIList(propertyInfo, obj);
                        }
                        else
                        {
                            if (miembro.GetType().IsInterface)
                            {
                                Object objeto = propertyInfo.GetValue(obj, null) as Object;
                                MemberInfo[] miembrosClase = objeto.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                                codigo += this.getCodigoByMembers(miembrosClase, objeto);
                            }
                            else
                            {
                                codigo += this.abrir("valor");
                                codigo += this.mostrarValor(propertyInfo.GetValue(obj, null));
                                codigo += this.cerrar("valor");
                            }
                        }
                        codigo += this.cerrar("propiedad");
                    }
                    else if (miembro.MemberType == MemberTypes.Field) // Campos (variables)
                    {
                        FieldInfo fi = miembro as FieldInfo; // Conversión para obtener los datos del tipo de campo

                        codigo += this.abrir("campo");

                        codigo += this.abrir("nombre");
                        codigo += this.mostrarValor(fi.Name);
                        codigo += this.cerrar("nombre");

                        codigo += this.abrir("tipo");
                        codigo += this.mostrarValor(fi.FieldType.Name);
                        codigo += this.cerrar("tipo");

                        codigo += this.abrir("valor");
                        codigo += this.mostrarValor(fi.GetValue(obj));
                        codigo += this.cerrar("valor");

                        codigo += this.cerrar("campo");
                    }
                    else if (miembro.MemberType == MemberTypes.NestedType) // Objetos anidados
                    {
                        codigo += this.abrir("nested type");

                        codigo += this.abrir("nombre");
                        codigo += this.mostrarValor(miembro.Name);
                        codigo += this.cerrar("nombre");

                        codigo += this.abrir("tipo");
                        codigo += this.mostrarValor(miembro.MemberType.ToString());
                        codigo += this.cerrar("tipo");

                        codigo += this.abrir("valor");
                        codigo += this.mostrarValor(miembro.ToString());
                        codigo += this.cerrar("valor");

                        codigo += this.cerrar("nested type");
                    }
                }
            }
            return codigo;

            // Para saber si una variable es de la case o de una clase padre:
            // miembro.ReflectedType == miembro.DeclaringType
        }

        private string getCampos(Type tipo)
        {
            string codigo = "";
            return codigo;
        }

        private string getAmbitoField(FieldInfo field)
        {
            String ambito = "";

            if (field.IsPublic)
            {
                ambito = "public";
            }
            else if (field.IsPrivate)
            {
                ambito = "private";
            }

            if (field.IsStatic)
            {
                ambito += " static";
            }
            return ambito;
        }

        private string getAmbito(MethodInfo metodo)
        {
            if (metodo.IsStatic) { return "static "; }
            if (metodo.IsPublic) { return "public "; }
            if (metodo.IsFamily) { return "protected "; }
            if (metodo.IsAssembly) { return "internal "; }
            if (metodo.IsPrivate) { return "private "; }
            return "unknown ";

        }

        private string recorrerIList(PropertyInfo propertyInfo, object obj)
        {
            string codigo = "";
            IList list = propertyInfo.GetValue(obj, null) as IList;
            string tipoList = "iList";

            codigo += this.abrir("count");
            codigo += this.mostrarValor(list.Count);
            codigo += this.cerrar("count");

            codigo += this.abrir("type");
            codigo += this.mostrarValor(list.GetType().Name);
            codigo += this.cerrar("type");

            if (propertyInfo.PropertyType.IsArray)
            {
                tipoList = "array";

                Array miArray = propertyInfo.GetValue(obj, null) as Array;
                codigo += this.abrir("rank");
                codigo += this.mostrarValor(miArray.Rank);
                codigo += this.cerrar("rank");
            }

            codigo += this.abrir(tipoList);
            codigo += this.mostrarValor("true");
            codigo += this.cerrar(tipoList);

            codigo += this.abrir("values");
            foreach (object elemento in list)
            {
                codigo += this.SALTO + this.TAB + this.TAB;
                IList lista = elemento as IList;
                if (lista != null)
                {
                    MemberInfo[] miembros = elemento.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    codigo += this.getCodigoByMembers(miembros, (object)elemento);
                }
                else
                {
                    codigo += "[TIPO] " + elemento.GetType().FullName;
                    codigo += this.abrir("value");
                    codigo += this.mostrarValor(elemento);
                    codigo += this.cerrar("value");
                }
            }
            codigo += this.cerrar("values");

            return codigo;
        }

        private string getAmbitoPropiedad(PropertyInfo atributo)
        {
            String ambito = "";
            PropertyAttributes atributos = atributo.Attributes;
            ambito += "[ATRIBUTOS]";
            ambito += atributos.ToString();

            /*
                        if (atributo.Attributes)
                        {
                            ambito = "public";
                        }
                        else if (atributo.IsPrivate)
                        {
                            ambito = "private";
                        }

                        if (atributo.IsStatic)
                        {
                            ambito += " static";
                        }
            */
            return ambito;

        }

        private string getValoresEnum(Type tipo)
        {
            // Hay que buscar dónde se captura la propiedad DeclaredMembers de un tipo
            String contenido = "";
            contenido += "//[ATRIBUTOS]";
            MemberInfo[] mis = tipo.GetDefaultMembers();
            foreach (MemberInfo mi in mis)
            {
                contenido += mi.ToString();
            }
            return contenido;

        }

        private string decodeString(string texto, object obj)
        {
            string codigo = "";
            return codigo;
        }

        private string abrir(string texto)
        {
            string codigo = "";
            codigo += this.SALTO + this.TAB;
            // Aquí se controla quje tipo de salida queremos, si XML, JSON, etc.
            //            switch (this.tipoDeSalida)
            //            {
            //                case "XML":
            //                default:
            codigo += "texto += \"<" + texto + ">\";";
            //            }
            return codigo;
        }

        private string cerrar(string texto)
        {
            string codigo = "";
            codigo += this.SALTO + this.TAB;
            // Aquí se controla quje tipo de salida queremos, si XML, JSON, etc.
            //            switch (this.tipoDeSalida)
            //            {
            //                case "XML":
            //                default:
            codigo += "texto += \"</" + texto + ">\";";
            //            }
            return codigo;
        }

        private string mostrarValor(object texto)
        {
            string codigo = "";
            codigo = codigo += this.SALTO + this.TAB;
            if (texto != null)
            {
                switch (this.tipoDeSalida)
                {
                    case "XML":
                    default:
                        return codigo + "texto += \"" + texto.ToString() + "\";";
                }
            }
            else
            {
                return "";
            }
        }
    }
}
