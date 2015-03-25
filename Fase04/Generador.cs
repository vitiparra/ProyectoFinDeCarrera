using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Collections;
using System.IO;

using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Fase04
{
    /*
     * Clase que genera el código de una clase codificador/decodificador para una determinada clase cuyo tipo se recibe como parámetro
     * La clase generada tiene dos métodos que se rellenan dependiendo de los miembros a serializar de la clase
     * 
     */
    class Generador
    {
        private string TAB = "\t";
        private string SALTO = "\r\n";
        private static int tabuladores = 0;

        private enum tiposDeCodificacion
        {
            XML,
            JSON,
            Binary
        }
        private static tiposDeCodificacion tipoDeCodificacion;

        // No tienen por qué ser estáticos (creo)
        private static string strCodigo;
        private static string strEncode;
        private static string strDecode;

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

            // Prueba: se cambia el código de la clase por una clase de prueba
            /*
                        strCodigo = @"               using System;
                           namespace Serializer
                           {
                               public class " + this.tipo.Name + @"Codec
                               {
                                   public void encode()
                                   {
                                        Console.WriteLine(""Hola Pepe"");
                                   }
                               }
                           }";
            */
            // 2º Compilar e instanciar la clase serializador
            serializador = this.compile(this.tipo.Name);
            return serializador;
        }

        /*
         * Mete en strCodigo todo el código del serializador a partir del tipo conocido
         * Este código contiene una definición de clase, un constructor y dos métodos estáticos encode y decode
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

        // Alias de generateSerializer
        private dynamic crearSerializador()
        {
            return this.getSerializer();
        }

        private void getCabecera()
        {
            // 1º Incluir los paquetes necesarios
            strCodigo = @"using System;
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
            strCodigo += "  public class ";
            strCodigo += this.tipo.Name + "Codec {";
            strCodigo += @"
        public static Type tipo;
        public static String nombreClase;
        private static string TAB = ""\t"";
        private static string SALTO = ""\r\n"";";
        }

        private void getCierre()
        {
            strCodigo += @" }"; // Cierre de clase
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
                tipo = obj.GetType();
                nombreClase = tipo.Name;
                String texto = """";";
            // strDecode ...

            strEncode += @"
                texto += abrir(""serializacion"");
                texto += abrir(""accesibilidad"");
                texto += getAccesibilidad(tipo);
                texto += cerrar(""accesibilidad"");
                // Modificador, opcional
                bool pintar = false;
                string modificador = getModificador(tipo, ref pintar);
                if (pintar){
                    texto += abrir(""modificador"");
                    texto += modificador;
                    texto += cerrar(""modificador"");
                }
                texto += abrir(""tipoDeObjeto"");
                texto += getTipoDeObjeto(tipo);
                texto += cerrar(""tipoDeObjeto"");

                BindingFlags flags = BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.DeclaredOnly
                    | BindingFlags.NonPublic
                    | BindingFlags.FlattenHierarchy
                    | BindingFlags.Static;
                // Se serializan todos los miembros públicos, privados y estáticos; propios y heredados
                MemberInfo[] miembros = tipo.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                texto += getCodigoByMembers(miembros, obj);
                texto += cerrar(""serializacion"");
                Console.WriteLine(texto);
                str = texto;
            }";

            strDecode += @"
                Dictionary<string,object> miembros = getMembersByCodigo(str, ref obj);
                foreach(var elemento in miembros)
                {
                    Console.WriteLine(elemento.Key + "": "" + elemento.Value.ToString());
                    PropertyInfo propertyInfo = obj.GetType().GetProperty(elemento.Key);
                    propertyInfo.SetValue(obj, elemento.Value, null);
                }
                return obj;
            }";
        }

        /*
         * Añade los métodos auxiliares necesarios para ejecutar encode y decode
         */
        private void generateAuxiliarMethods()
        {
            /*
             * Método abrir
             */
            strCodigo += this.newLine();
            strCodigo += @"private static string abrir(string texto){
    string codigo = SALTO;
    // Aquí se controla qué tipo de salida queremos, si XML, JSON, etc.
    //            switch (this.tipoDeSalida)
    //            {
    //                case ""XML"":
    //                default:
    codigo += ""<"" + texto + "">"";
    //            }
    return codigo;
}";

            /*
             * Método cerrar
             */
            strCodigo += @"
        private static string cerrar(string texto){
            string codigo = """";
            // Aquí se controla quje tipo de salida queremos, si XML, JSON, etc.
//            switch (this.tipoDeSalida)
//            {
//                case ""XML"":
//                default:
                codigo += ""</"" + texto + "">"";
//            }
            return codigo;
        }";

            /*
             * Método getAccesibilidad
             */
            strCodigo += @"
        private static string getAccesibilidad(Type tipo){
            string accesibilidad = """";
            string codigo = """";
            // Accesibilidad: public, protected, internal, protected internal o private
            if (tipo.IsPublic){
                accesibilidad += ""public"";
            }
            else if (tipo.IsNotPublic){
                accesibilidad += ""private"";
            }
            else{
                accesibilidad += ""protected"";
            }
            codigo += accesibilidad;
            return codigo;
        }";

            /*
             * Método getModificador
             */
            strCodigo += @"
        private static string getModificador(Type tipo, ref bool pintar){
            string modificador = """";
            string codigo = """";
            pintar = true;
            // Modificadores: abstract, async, override, parcial, readonly, sealed, static, unsafe, virtual, volatile
            if(tipo.IsAbstract){
                modificador += ""abstract"";
            }
            else if (tipo.IsSealed){
                modificador += ""sealed"";
            }
            else{
                modificador += """";
                pintar = false;
            }
            codigo += modificador;
            return codigo;
        }";

            /*
             * Método getTipoDeObjeto
             */
            strCodigo += @"
        private static string getTipoDeObjeto(Type tipo){
            string tipoDeObjeto = """";
            string codigo = """";
            // Identificación del tipo de objeto: clase, interface, enum, struct
            if (tipo.IsPrimitive)
            {
                TypeAttributes atr = tipo.Attributes;
                tipoDeObjeto += atr.ToString();
            }
            else if (tipo.IsInterface){
                tipoDeObjeto += ""interface "";
            }
            else if (tipo.IsEnum){
                tipoDeObjeto += ""enum "";
            }
            else if (tipo.IsValueType){
                if (tipo.IsEnum){
                    tipoDeObjeto += ""enum "";
                }
                else{
                    tipoDeObjeto += ""struct "";
                    }
            }
            else if(tipo.IsClass){
                tipoDeObjeto += ""class "";
            }
            else
            {
                tipoDeObjeto += ""unknown"";
            }
            codigo += tipoDeObjeto;
            return codigo;
        }";

            // Método getCodigoByMember
            strCodigo += @"

        /*
         * Captura la información de cada miembro de un tipo, incluyendo sus valores
         * @input MemberInfo[] miembros Conjunto de miembros de un tipo susceptibles de ser serializados tipo.GetMembers(flags)
         * @input Object obj Objeto instanciado para obtener los valores
         * @output string Código serializado con todos los miembros del objeto indicado
         */
        // Método OK para obtener todos los miembros (propiedades y variables) susceptibles de ser serializados
        private static string getCodigoByMembers(MemberInfo[] miembros, Object obj)
        {
            string codigo = """";
            codigo += abrir(""elementos"");
            foreach (MemberInfo miembro in miembros)
            {
                // FASE 6: miembro.GetCustomAttributes() para capturar los atributos (comprobar atributos para saber si hay que serializar)
                if (!miembro.Name.Contains(""_BackingField"")) // Quitamos los BackingFields (TODO: ver como evitamos estos miembros al instanciar ""miembros"")
                {
                    if (miembro.MemberType == MemberTypes.Property) // Propiedades (variables con GETTER y SETTER)
                    {
                        PropertyInfo propertyInfo = miembro as PropertyInfo; // Conversión para obtener los datos del tipo de campo

                        codigo += abrir(""elemento"");

                        codigo += abrir(""tipoDeObjeto"");
                        codigo += mostrarValor(getTipoDeObjeto(obj.GetType()));
                        codigo += cerrar(""tipoDeObjeto"");

                        codigo += abrir(""tipoDeElemento"");
                        codigo += mostrarValor(""propiedad"");
                        codigo += cerrar(""tipoDeElemento"");

                        codigo += abrir(""nombre"");
                        codigo += mostrarValor(propertyInfo.Name);
                        codigo += cerrar(""nombre"");

                        codigo += abrir(""tipo"");
                        codigo += mostrarValor(propertyInfo.PropertyType.FullName);
                        codigo += cerrar(""tipo"");

                        codigo += abrir(""isArray"");
                        codigo += mostrarValor(propertyInfo.PropertyType.IsArray);
                        codigo += cerrar(""isArray"");

                        codigo += abrir(""valor"");
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
                        codigo += cerrar(""valor"");
                        codigo += cerrar(""elemento"");
                    }
                    else if (miembro.MemberType == MemberTypes.Field) // Campos (variables)
                    {
                        FieldInfo fi = miembro as FieldInfo; // Conversión para obtener los datos del tipo de campo

                        codigo += abrir(""elemento"");

                        codigo += abrir(""nombre"");
                        codigo += mostrarValor(fi.Name);
                        codigo += cerrar(""nombre"");

                        codigo += abrir(""tipoDeElemento"");
                        codigo += mostrarValor(""campo"");
                        codigo += cerrar(""tipoDeElemento"");

                        codigo += abrir(""tipo"");
                        codigo += mostrarValor(fi.FieldType.FullName);
                        codigo += cerrar(""tipo"");

                        codigo += abrir(""valor"");
                        codigo += mostrarValor(fi.GetValue(obj));
                        codigo += cerrar(""valor"");

                        codigo += cerrar(""elemento"");
                    }
                    else if (miembro.MemberType == MemberTypes.NestedType) // Objetos anidados
                    {
Console.WriteLine(""Tipo anidado"" + miembro.MemberType);
                        codigo += abrir(""elemento"");

                        codigo += abrir(""tipoDeElemento"");
                        codigo += mostrarValor(""nested type"");
                        codigo += cerrar(""tipoDeElemento"");

                        codigo += abrir(""nombre"");
                        codigo += mostrarValor(miembro.Name);
                        codigo += cerrar(""nombre"");

                        codigo += abrir(""tipo"");
                        codigo += mostrarValor(miembro.MemberType.ToString());
                        codigo += cerrar(""tipo"");

                        codigo += abrir(""valor"");
                        codigo += mostrarValor(miembro.ToString());
                        codigo += cerrar(""valor"");

                        codigo += cerrar(""elemento"");
                    }
                }
            }
            codigo += cerrar(""elementos"");
            return codigo;

            // Para saber si una variable es de la case o de una clase padre:
            // miembro.ReflectedType == miembro.DeclaringType
        }

        private static string recorrerIList(PropertyInfo propertyInfo, object obj)
        {
Console.WriteLine(""a"");
Console.WriteLine(""lista: "" + obj.ToString());
Console.WriteLine(""valor: "" + propertyInfo.ToString());
            IList list = propertyInfo.GetValue(obj, null) as IList;
            return recorrerIListObject(list);
        }

        private static string recorrerIListObject(IList list)
        {
            string codigo = """";
            string tipoList = ""iList"";
            Type t = list.GetType();

            codigo += abrir(""count"");
            codigo += mostrarValor(list.Count);
            codigo += cerrar(""count"");

            codigo += abrir(""tipoDeElementos"");
            codigo += mostrarValor(t.GetElementType().FullName);
            codigo += cerrar(""tipoDeElementos"");

            codigo += abrir(""tipoDeList"");
            codigo += mostrarValor(tipoList);
            codigo += cerrar(""tipoDeList"");
/*
            codigo += abrir(""type"");
            codigo += mostrarValor(list.GetType().GetElementType().FullName);
            codigo += cerrar(""type"");
*/

            Array miArray;
            miArray = list as Array;
            if (miArray == null)
            {
                tipoList = ""iList"";
            }
            else
            {
                tipoList = ""array"";

                codigo += abrir(""rank"");
                codigo += mostrarValor(miArray.Rank);
                codigo += cerrar(""rank"");

                codigo += abrir(""datosDeLosRangos"");
                for(int i=0; i< miArray.Rank; i++)
                {
                    codigo += abrir(""datosDeRango"");

                    codigo += abrir(""longitud"");
                    codigo += mostrarValor(miArray.GetLength(i));
                    codigo += cerrar(""longitud"");

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

            return codigo;
        }

        private static string recorrerIDictionary(PropertyInfo propertyInfo, object obj)
        {
            IDictionary dict = propertyInfo.GetValue(obj, null) as IDictionary;
            return recorrerIDictionaryObject(dict);
        }

        private static string recorrerIDictionaryObject(IDictionary dict)
        {
            string codigo = """";
            string tipoList = ""iDictionary"";

            codigo += abrir(""count"");
            codigo += mostrarValor(dict.Count);
            codigo += cerrar(""count"");

            Type t = dict.GetType();
            codigo += abrir(""type"");
            codigo += mostrarValor(t.FullName);
            codigo += cerrar(""type"");

            codigo += abrir(""tipoDeList"");
            codigo += mostrarValor(tipoList);
            codigo += cerrar(""tipoDeList"");

            Type[] arguments = dict.GetType().GetGenericArguments();
            codigo += abrir(""tipoDeIndex"");
            codigo += mostrarValor(arguments[0]);
            codigo += cerrar(""tipoDeIndex"");

            codigo += abrir(""tipoDeValue"");
            codigo += mostrarValor(arguments[1]);
            codigo += cerrar(""tipoDeValue"");

            codigo += abrir(""valores"");

            PropertyInfo[] pi = t.GetProperties();
            foreach (var propiedad in pi)
            {
                if (propiedad.Name == ""Keys"")
                {
                    codigo += abrir(""valor"");
                    IEnumerable claves = (IEnumerable)propiedad.GetValue(dict, null);
                    foreach (var clave in claves)
                    {
                        codigo += abrir(""laClave"");
                        codigo += mostrarValor(clave);
                        codigo += cerrar(""laClave"");

                        codigo += abrir(""elValor"");
                        /*
                         * Identificar si el valor es un dato único o un conjunto de datos
                         */
                        /*
                        Object objAux = dict[clave] as Object;
                        if (objAux.GetType().GetTypeInfo().IsPrimitive)
                        {
                            MemberInfo[] miembros = objAux.GetType().GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                            codigo += getCodigoByMembers(miembros, objAux);
                        }
                        else
                        {
                            codigo += mostrarValor(objAux);
                        }
                         */ 
                        codigo += mostrarValor(dict[clave]);
                        codigo += cerrar(""elValor"");
                    }
                    codigo += cerrar(""valor"");
                }
            }

            codigo += cerrar(""valores"");
            return codigo;
        }

        private static string mostrarValor(object texto)
        {
            string codigo = """";
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
                return """";
            }
        }

        private Dictionary<string,object> getMembersByCodigo(string str, ref Object obj)
        {
            Dictionary<string,object> elementos = new Dictionary<string,object>();
            XmlDocument xml = new XmlDocument();
            /*
             * Aquí va el control de errores del documento XML
            */
            xml.LoadXml(str);
            XmlNodeList nodos = xml.GetElementsByTagName(""elemento"");
            foreach (XmlNode nodo in nodos)
            {
Console.WriteLine(""vuelta al bucle con "" + nodo.InnerText);
                string nombre = nodo[""nombre""].InnerText;
                object valor = getElementValue(nodo);
                if(valor != null)
                {
Console.WriteLine(""añadiendo elemento "" + nombre + "": "" + valor.ToString());
                    elementos.Add(nombre, valor);
                }
            }
Console.WriteLine(""Finalizado recorrido por los nodos"");
/*
            XmlElement root = xml.DocumentElement;

            nodo = root.SelectSingleNode(""accesibilidad"");
*/
            return elementos;
        }

        private Boolean isStopWord(string word)
        {
            // Eliminamos propiedades indeseadas (se debe hacer en el encode, pero no está de más hacerlo aquí también)
            return (word.IndexOf(""Wrapper`"") != -1); 
        }

        private object getElementValue(XmlNode nodo)
        {
            if(nodo[""tipo""] == null)
            {
Console.WriteLine(""valor del nodo: "" + nodo.InnerText);
                return nodo.InnerText;
            }
            else
            {
                string elTipo = nodo[""tipo""].InnerText;
Console.WriteLine(""Tipo: "" + elTipo);
                if(!isStopWord(elTipo) && elTipo != """")
                {
Console.WriteLine(""No es stopWord"");
                    XmlNode nValor  = nodo[""valor""];
                    Type t = Type.GetType(elTipo);
                    if(t == null)
                    {
                        Console.WriteLine(""Error en el tipo "" + elTipo);
                        return null;
                    }
                    else
                    {
                        if(nodo[""isArray""] != null && nodo[""isArray""].InnerText == ""True"")
                        {
Console.WriteLine(""Datos del array"");
                            string tipoArray = nValor[""tipoDeElementos""].InnerText;
                            Type tArray = Type.GetType(tipoArray);
                            string cantidadArray = ""0"";
Console.WriteLine(""Tipo de array: "" + tipoArray);
                            if(nValor[""count""] != null)
                            {
                                cantidadArray = nValor[""count""].InnerText;
Console.WriteLine(""Cantidad de elementos en el array: "" + cantidadArray);
                            }
                            else
                            {
Console.WriteLine(""No hay cantidad de elementos en el array (hará falta?)"");
                            }
                            Int32 rango = 0;
                            if(nValor[""rank""] != null)
                            {
                                string rangoArray = nValor[""rank""].InnerText;
                                rango = Convert.ToInt32(rangoArray);
Console.WriteLine(""Rango del array: "" + rangoArray);
                            }
                            else
                            {
Console.WriteLine(""No hay rango del array"");
                            }

                            Dictionary<string, int> longitudes = new Dictionary<string, int>();
                            Dictionary<string, int> valoresMenores = new Dictionary<string, int>();
                            int contador = 0;

                            XmlNodeList xnldatosDeRango = nValor[""datosDeLosRangos""].SelectNodes(""datosDeRango"");
                            int[] dimensionesArray = new int[rango];
Console.WriteLine(""Dimensiones del array "" + dimensionesArray.Length);
                            if(xnldatosDeRango != null)
                            {
Console.WriteLine(""Lista de nodos datosDeRango"");
                                foreach (XmlNode datosDeUnRango in xnldatosDeRango)
                                {
                                    Int32 longitudDelRango = Convert.ToInt32(datosDeUnRango[""longitud""].InnerText);
                                    longitudes.Add(""longitud"" + contador, longitudDelRango);
                                    dimensionesArray[contador] = longitudDelRango;
Console.WriteLine("" - "" + longitudDelRango);

                                    valoresMenores.Add(""valorMenor"" + contador, Convert.ToInt32(datosDeUnRango[""valorMenor""].InnerText));
Console.WriteLine(""Datos del Rango "" + contador + "": "" + datosDeUnRango.InnerText);

                                    contador++;
                                }
                            }
                            else
                            {
Console.WriteLine(""No hay datos de rango del array"");
                            }

//                          XmlNodeList nValoresArray = oEsArray.GetElementsByTagName(""valores"");
                            if(nValor[""valores""] != null)
                            {
                                XmlNode xnValores = nValor[""valores""];
                                XmlNodeList xnlValor = xnValores.SelectNodes(""valor"");
                                contador = 0;
                                Array aux = Array.CreateInstance(tArray, dimensionesArray);
/* ********************************** RECORRIENDO ARRAY AUXILIAR PARA GUARDAR EN CADA ÍNDICE UN ""valor"" SERIALIZADO **************
Console.WriteLine(""Creado array"");
                                foreach (object auxElemento in aux)
                                {
Console.WriteLine(""Añadido valor "" + xnlValor[contador].InnerText + "" de tipo "" + tipoArray);
                                    auxElemento = Convert.ChangeType(xnlValor[contador].InnerText, tArray);
// esto también hay que controlarlo                                    auxElemento = Convert.ChangeType(getElementValue(xnlValor[contador].InnerXml), tArray);
                                    contador++;
                                }
*************************************************************************************************************************************** */
                                // Inicialización de los índices del array donde se guardará el valor
                                int[] indicesDelElemento = new int[rango];
                                for(int i=0; i<rango; i++)
                                {
                                    indicesDelElemento[i] = valoresMenores[""valorMenor"" + i];
                                }

                                foreach (XmlNode valor in xnValores)
                                {
Console.WriteLine(""Añadido valor "" + valor.InnerText + "" de tipo "" + tipoArray);
// Convertir los índices en una cadena separada por comas para mostrar los índices
string inx = """";
for(int i=0; i<rango; i++)
{
    inx += indicesDelElemento[i] + "","";
}
Console.WriteLine(""índices: "" + inx);
                                    // Guardar el valor en el índice correspondiente
                                    aux.SetValue(Convert.ChangeType(getElementValue(valor), tArray), indicesDelElemento);
                                    contador++;
                                    for(int j=rango-1; j>=0; j--)
                                    {
Console.WriteLine(""Se comprueba la dimensión "" + j);
                                        if(indicesDelElemento[j] < valoresMenores[""valorMenor"" + j] + longitudes[""longitud"" + j] - 1)
                                        {
Console.WriteLine(""Se incrementa el índice de la dimensión "" + j);
                                            indicesDelElemento[j]++;
                                            break;
                                        }
                                        else
                                        {
                                            if(indicesDelElemento[j] == valoresMenores[""valorMenor"" + j] + longitudes[""longitud"" + j] - 1)
                                            {
Console.WriteLine(""Hemos llegado al final de la dimensión "" + j);
                                                for(int k=j; k<rango; k++)
                                                {
Console.WriteLine(""Se pone a 0 la dimensión "" + k);
                                                    indicesDelElemento[k] = 0; 
                                                }
                                            }
                                            else
                                            {
Console.WriteLine(""No pasa nada, se sigue procesando el siguiente elemento de la dimensión "" + j + "": "" + indicesDelElemento[j]);
                                            }
                                        }
                                    }
                                }

Console.WriteLine(""Finalizado el recorrido por el array"");
                                return aux;
                            }
                            else{
Console.WriteLine(""No hay valores"");
                                return null;
                            }
                        }
                        else
                        {
                            if (elTipo.Contains(""Dictionary"") || elTipo.Contains(""List`""))
                            {
                                // Hay varios elementos, se capturan todos y se meten en el tipo indicado como valor válido
Console.WriteLine(""Datos del dictionary"");
                                Type tArray = Type.GetType(elTipo);
                                int cantidadDictionary = 0;
                                if (nValor[""count""] != null)
                                {
                                    cantidadDictionary = Convert.ToInt16(nValor[""count""].InnerText);
Console.WriteLine(""Cantidad de elementos en el dictionary: "" + cantidadDictionary);
                                }
                                else
                                {
                                    Console.WriteLine(""No hay cantidad de elementos en el dictionary (hará falta?)"");
                                }

                                if (cantidadDictionary > 0 && nValor[""valores""] != null)
                                {
                                    string tipoDeIndex = """";
                                    if (nValor[""tipoDeIndex""] != null)
                                    {
                                        tipoDeIndex = nValor[""tipoDeIndex""].InnerText;
                                    }
                                    string tipoDeValue = """";
                                    if (nValor[""tipoDeValue""] != null)
                                    {
                                        tipoDeValue = nValor[""tipoDeValue""].InnerText;
                                    }
                                    if (tipoDeIndex != """" && tipoDeValue != """")
                                    {
                                        Type tIndex = Type.GetType(tipoDeIndex);
                                        Type tValue = Type.GetType(tipoDeValue);
                                        var auxDict = (IDictionary)Activator.CreateInstance(tArray);

//                                        Dictionary<tIndex, tValue> auxDict = new Dictionary<Type.GetType(tipoDeIndex),Type.GetType(tipoDeValue)>();

                                        XmlNode xnValores = nValor[""valores""];
                                        XmlNodeList xnlValor = xnValores.SelectNodes(""valor"");
                                        int contador = 0;
                                        foreach (XmlNode valor in xnValores)
                                        {
                                            object elIndex = valor[""laClave""].InnerText;
                                            object elValue = valor[""elValor""].InnerText;

                                            // Guardar el valor en el índice correspondiente
                                            auxDict.Add(Convert.ChangeType(elIndex, tIndex), Convert.ChangeType(getElementValue(valor[""elValor""]), tValue));
                                            Console.WriteLine(""Añadido valor "" + valor.InnerText + "" de tipo "" + tArray);
                                        }
                                        Console.WriteLine(""Finalizado el recorrido por el dictionary"");
                                        return auxDict;
                                    }
                                    else
                                    {
                                        Console.WriteLine(""No hay índices o valores"");
                                        return null;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine(""No hay valores"");
                                    return null;
                                }
                            }
                            else if (nValor.InnerText != """")
                            {
Console.WriteLine(""Se añade un nuevo elemento al dictionary"");
                                return Convert.ChangeType(nValor.InnerText, Type.GetType(elTipo));
                            }
                            else
                            {
Console.WriteLine(""No hay valores"");
                                return null;
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
        }
";

        }

        /*
         * Aquí se invoca la compilación en tiempo de ejecución del código contenido en strCodigo
         * Se devuelve como un objeto una instancia de la clase compilada.
         * Compilar automáticamente la clase cuyo código se contiene en strCodigo
         */
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

        private string newLine()
        {
            string s = this.SALTO;
            for (int i = 0; i < tabuladores; i++)
            {
                s += this.TAB;
            }
            return s;
        }
    }
}
