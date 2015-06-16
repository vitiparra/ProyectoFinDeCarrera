using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using Fase02;
using Fase06;

using Polenter.Serialization;
using ProtoBuf;

namespace Fase07
{
    class Program
    {
        static Stopwatch watch;
        static int veces = 100;
        static void Main(string[] args)
        {
            string cuadro = ";XMLSerializer;BinaryFormatter;SOAPFormatter;DataContractSerializer;SharpSerializer (XML);SharpSerializer (Binario);Protobuf;Nuestro (CSV);Nuestro (XML)\r\n";

            Program p = new Program();
            watch = new System.Diagnostics.Stopwatch();

            cuadro += p.benchmarkClase01Basica();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase02ArrayNormal();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase03Arrays();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase04Struct();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase05Clase();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase06ClaseDerivada();
            cuadro += "\r\n";
            cuadro += p.benchmarkClase07ClaseConTodo();
//            cuadro += "\r\n";
//            cuadro += p.benchmarkClase08List();

            using (TextWriter comparativaCSV = File.CreateText("comparativa.csv"))
            {
                comparativaCSV.Write(cuadro);
                comparativaCSV.Write(comparativaCSV.NewLine);
            }

            Console.WriteLine(cuadro);
            Console.ReadLine();
        }

        public string benchmarkClase01Basica(){
            string linea1 = "";
            string linea2 = "";

            /*
             * 01. Clase básica
             */
            linea1 += "Clase01Basica (Encode);";
            linea2 += "Clase01Basica (Decode);";
            Console.WriteLine("============== Clase con dos campos (Clase01Basica) ==============\r\n");

            // Instanciando y rellenando campos
            Clase01Basica c1 = new Clase01Basica();
            Clase01Basica c1decoded;

            // - XMLSerializer
            #region XMLSerializer

            XmlSerializer serializer = new XmlSerializer(typeof(Clase01Basica));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c1);
            }
            watch.Stop();
            writer.Close();

            // Generamos el fichero de nuevo, con una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c1);
            writer.Close();

            Console.WriteLine("Codificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            FileStream fs;
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                c1decoded = (Clase01Basica)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase01Basica con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase01Basica)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase01Basica con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase01Basica con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase01Basica)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase01Basica));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c1);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase01Basica)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c1, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase01Basica)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c1, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase01Basica)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c1);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                c1decoded = ProtoBuf.Serializer.Deserialize<Clase01Basica>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase01Basica();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Generador(c1.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine(str);
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase01Basica();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase02ArrayNormal()
        {
            string linea1 = "";
            string linea2 = "";

            /*
             * 02. Clase con arrays
             */
            linea1 += "Clase02ArrayNormal (Encode);";
            linea2 += "Clase02ArrayNormal (Decode);";
            Console.WriteLine("============== Clase con arrays simples (Clase02ArrayNormal) ==============\r\n");

            // Instanciando y rellenando campos
            Clase02ArrayNormal c = new Clase02ArrayNormal();
            Clase02ArrayNormal decoded;
            #region Datos Clase02ArrayNormal
            c.var1 = new int[3];
            for (int i = 0; i < 3; i++)
            {
                c.var1[i] = i;
            }

            c.var2 = new string[3];
            for (int i = 0; i < 3; i++)
            {
                c.var2[i] = "Número " + Convert.ToString(i);
            }
/*
            c.var3 = new int[1, 2];
            int cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    c.var3[i, j] = cont;
                    cont++;
                }
            }

            c.var4 = new int[1, 2, 3];
            cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        c.var4[i, j, k] = cont;
                        cont++;
                    }
                }
            }
*/ 
            #endregion

            // - XMLSerializer
            #region XMLSerializer
            FileStream fs;
/*
            linea1 += ";";
            linea2 += ";";
*/
            XmlSerializer serializer = new XmlSerializer(typeof(Clase02ArrayNormal));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c);
            }
            watch.Stop();
            writer.Close();

            // Generamos el fichero de nuevo, con una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c);
            writer.Close();

            Console.WriteLine("Codificación Clase02ArrayNormal con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                decoded = (Clase02ArrayNormal)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase02ArrayNormal con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";"; 
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase02ArrayNormal con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    decoded = (Clase02ArrayNormal)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase02ArrayNormal con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
/*
            linea1 += ";";
            linea2 += ";";
*/
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase02ArrayNormal con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    decoded = (Clase02ArrayNormal)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase02ArrayNormal con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con DataContractSerializer");

            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase03Array));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase02ArrayNormal con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                decoded = (Clase02ArrayNormal)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase02ArrayNormal con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/ 
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase02ArrayNormal con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                decoded = (Clase02ArrayNormal)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase02ArrayNormal con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase02ArrayNormal con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                decoded = (Clase02ArrayNormal)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase02ArrayNormal con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
/*
            linea1 += ";";
            linea2 += ";";
*/
            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase02ArrayNormal con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                decoded = ProtoBuf.Serializer.Deserialize<Clase02ArrayNormal>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase02ArrayNormal con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Fase06.Generador g1 = new Fase06.Generador(c.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase02ArrayNormal con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                decoded = new Clase02ArrayNormal();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase02ArrayNormal con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Fase06.Generador(c.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase02ArrayNormal con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                decoded = new Clase02ArrayNormal();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase02ArrayNormal con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase03Arrays()
        {
            string linea1 = "";
            string linea2 = "";

            /*
             * 03. Clase con arrays
             */
            linea1 += "Clase03Arrays (Encode);";
            linea2 += "Clase03Arrays (Decode);";
            Console.WriteLine("============== Clase con distintos tipos de array (Clase03Array) ==============\r\n");

            // Instanciando y rellenando campos
            Clase03Array c = new Clase03Array();
            Clase03Array decoded;
            #region Datos Clase03Array
            c.var1 = new int[3];
            for (int i = 0; i < 3; i++)
            {
                c.var1[i] = i;
            }

            c.var2 = new string[3];
            for (int i = 0; i < 3; i++)
            {
                c.var2[i] = "Número " + Convert.ToString(i);
            }

            c.var3 = new int[1, 2];
            int cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    c.var3[i, j] = cont;
                    cont++;
                }
            }

            c.var4 = new int[1, 2, 3];
            cont = 0;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        c.var4[i, j, k] = cont;
                        cont++;
                    }
                }
            }

            c.var5 = new int[3][];
            cont = 0;
            for (int i = 0; i < 3; i++)
            {
                int[] aux = new int[4];
                for (int j = 0; j < 4; j++)
                {
                    aux[j] = cont;
                    cont++;
                }
                c.var5[i] = aux;
            }

            c.var6 = new Dictionary<string, int>();
            c.var6.Add("uno", 1);
            c.var6.Add("dos", 2);
            c.var6.Add("tres", 3);

            c.var7 = new DentroDelArray[2];
            DentroDelArray aux1 = new DentroDelArray();
            aux1.uno = 1;
            aux1.dos = "dos";
            c.var7[0] = aux1;
            DentroDelArray aux2 = new DentroDelArray();
            aux2.uno = 1;
            aux2.dos = "dos";
            c.var7[1] = aux2;

            #endregion

            // - XMLSerializer
            #region XMLSerializer
            FileStream fs;
            linea1 += ";";
            linea2 += ";";

/*
 *** Este serializador no admite arrays complejos
            XmlSerializer serializer = new XmlSerializer(typeof(Clase03Array));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c);
            }
            watch.Stop();
            writer.Close();

            // Generamos el fichero de nuevo, con una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c);
            writer.Close();

            Console.WriteLine("Codificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                decoded = (Clase03Array)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    decoded = (Clase03Array)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase01Basica con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase01Basica con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    decoded = (Clase03Array)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
 */ 
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con DataContractSerializer");

            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase03Array));
            for (int i = 0; i < veces; i++)
            {
                watch.Restart(); // Comienza a contar el tiempo
                DCserializer.WriteObject(stream1, c);
                watch.Stop();
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            Console.WriteLine("Codificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                decoded = (Clase03Array)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/ 
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                decoded = (Clase03Array)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                decoded = (Clase03Array)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase01Basica con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                decoded = ProtoBuf.Serializer.Deserialize<Clase03Array>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/ 
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Fase06.Generador g1 = new Fase06.Generador(c.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                decoded = new Clase03Array();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Fase06.Generador(c.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                decoded = new Clase03Array();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase04Struct()
        {
            string linea1 = "";
            string linea2 = "";

            /*
             * 01. Clase Clase04Struct
             */
            linea1 += "Clase04Struct (Encode);";
            linea2 += "Clase04Struct (Decode);";
            Console.WriteLine("============== Clase con estructura (Clase04Struct) ==============\r\n");

            // Instanciando y rellenando campos

            Clase04Struct c = new Clase04Struct();
            Clase04Struct cdecoded;
            c.valor3.valor1 = 1;
            c.valor3.valor2 = "2";

            // - XMLSerializer
            #region XMLSerializer

            XmlSerializer serializer = new XmlSerializer(typeof(Clase04Struct));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c);
            }
            watch.Stop();
            writer.Close();

            // Generamos el fichero de nuevo, con una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c);
            writer.Close();

            Console.WriteLine("Codificación Clase04Struct con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            FileStream fs;
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                cdecoded = (Clase04Struct)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase04Struct con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase04Struct con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    cdecoded = (Clase04Struct)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase04Struct con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase04Struct con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    cdecoded = (Clase04Struct)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase04Struct con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase04Struct));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase04Struct con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                cdecoded = (Clase04Struct)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase04Struct con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase04Struct con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                cdecoded = (Clase04Struct)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase04Struct con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase04Struct con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                cdecoded = (Clase04Struct)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase04Struct con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c); // Falla (No suitable Default estructura encoding found)
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase04Struct con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                cdecoded = ProtoBuf.Serializer.Deserialize<Clase04Struct>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase04Struct con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";"; 
*/ 
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase04Struct con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                cdecoded = new Clase04Struct();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref cdecoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase04Struct con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Generador(c.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase04Struct con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                cdecoded = new Clase04Struct();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref cdecoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase04Struct con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase05Clase()
        {
            string linea1 = "";
            string linea2 = "";

            /*
             * 05. Clase con clase interna
             */
            linea1 += "Clase05Clase (Encode);";
            linea2 += "Clase05Clase (Decode);";
            Console.WriteLine("============== Clase con otra clase dentro (Clase05Clase) ==============\r\n");

            // Instanciando y rellenando campos
            Clase05Clase c1 = new Clase05Clase();
            Clase05Clase c1decoded;
            Clase05Clase.ClaseInterna cInterna = new Clase05Clase.ClaseInterna();
            cInterna.var1 = 1;
            cInterna.var2 = "2";
            c1.var3 = cInterna;

            // - XMLSerializer
            #region XMLSerializer

            XmlSerializer serializer = new XmlSerializer(typeof(Clase05Clase));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c1);
            }
            watch.Stop();
            writer.Close();

            // Repetimos una sola vez para generar el fichero co una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c1);
            writer.Close();

            Console.WriteLine("Codificación Clase05Clase con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            FileStream fs;
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                c1decoded = (Clase05Clase)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase05Clase con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase05Clase con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase05Clase)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase05Clase con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase05Clase con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase05Clase)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase05Clase con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase05Clase));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c1);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase05Clase con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase05Clase)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase05Clase con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c1, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase05Clase con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase05Clase)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase05Clase con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c1, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase05Clase con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase05Clase)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase05Clase con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            linea1 += ";";
            linea2 += ";";
/*

            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c1);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase05Clase con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                c1decoded = ProtoBuf.Serializer.Deserialize<Clase05Clase>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase05Clase con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/ 
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                    //                    strSerializado = Clase05ClaseCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase05Clase con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase05Clase();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase05Clase con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Generador(c1.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase05Clase con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase05Clase();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase05Clase con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase06ClaseDerivada()
        {
            string linea1 = "";
            string linea2 = "";

            /*
             * 06. Clase con clase derivada
             */
            linea1 += "Clase06ClaseDerivada (Encode);";
            linea2 += "Clase06ClaseDerivada (Decode);";
            Console.WriteLine("============== Clase con otra clase derivada dentro (Clase06ClaseDerivada) ==============\r\n");

            // Instanciando y rellenando campos
            Clase06ClaseDerivada c1 = new Clase06ClaseDerivada();
            Clase06ClaseDerivada c1decoded;
            c1.var1 = 1;
            c1.var2 = "2";
            c1.var3 = 3;

            // - XMLSerializer
            #region XMLSerializer

            XmlSerializer serializer = new XmlSerializer(typeof(Clase06ClaseDerivada));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c1);
            }
            watch.Stop();
            writer.Close();

            // Repetimos una sola vez para generar el fichero co una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c1);
            writer.Close();

            Console.WriteLine("Codificación Clase06ClaseDerivada con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            FileStream fs;
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                c1decoded = (Clase06ClaseDerivada)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase06ClaseDerivada con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion XMLSerializer

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase06ClaseDerivada con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase06ClaseDerivada)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase06ClaseDerivada con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion BinaryFormatter

            // - SOAPFormatter
            #region SOAPFormatter
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase06ClaseDerivada con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase06ClaseDerivada)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase06ClaseDerivada con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase06ClaseDerivada));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c1);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase06ClaseDerivada con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase06ClaseDerivada)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase06ClaseDerivada con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c1, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase06ClaseDerivada con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase06ClaseDerivada)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase06ClaseDerivada con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c1, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase06ClaseDerivada con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase06ClaseDerivada)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase06ClaseDerivada con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            /*
            linea1 += ";";
            linea2 += ";";
            */

            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c1);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase06ClaseDerivada con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                c1decoded = ProtoBuf.Serializer.Deserialize<Clase06ClaseDerivada>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase06ClaseDerivada con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                    //                    strSerializado = Clase06ClaseDerivadaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase06ClaseDerivada con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase06ClaseDerivada();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase06ClaseDerivada con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Generador(c1.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase06ClaseDerivada con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase06ClaseDerivada();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase06ClaseDerivada con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase07ClaseConTodo()
        {
            string linea1 = "";
            string linea2 = "";
            FileStream fs;

            /*
             * 07. Clase con todo
             */
            linea1 += "Clase07ClaseConTodo (Encode);";
            linea2 += "Clase07ClaseConTodo (Decode);";
            Console.WriteLine("============== Clase con todo (Clase07ClaseConTodo) ==============\r\n");

            // Instanciando y rellenando campos
            Clase07ClaseConTodo c1 = new Clase07ClaseConTodo();
            Clase07ClaseConTodo c1decoded;

            // - XMLSerializer
            #region XMLSerializer
            linea1 += ";";
            linea2 += ";";

            /*
            XmlSerializer serializer = new XmlSerializer(typeof(Clase07ClaseConTodo));
                        TextWriter writer = new StreamWriter("fichero.txt");

                        watch.Restart(); // Comienza a contar el tiempo
                        for (int i = 0; i < veces; i++)
                        {
                            serializer.Serialize(writer, c1);
                        }
                        watch.Stop();
                        writer.Close();

                        // Repetimos una sola vez para generar el fichero co una única serialización
                        writer = new StreamWriter("fichero.txt");
                        serializer.Serialize(writer, c1);
                        writer.Close();

                        Console.WriteLine("Codificación Clase07ClaseConTodo con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
                        linea1 += watch.ElapsedMilliseconds + ";";

                        watch.Restart(); // Comienza a contar el tiempo
                        for (int i = 0; i < veces; i++)
                        {
                            fs = new FileStream("fichero.txt", FileMode.Open);
                            c1decoded = (Clase07ClaseConTodo)serializer.Deserialize(fs);
                            fs.Close();
                        }
                        watch.Stop();
                        Console.WriteLine("Decodificación Clase07ClaseConTodo con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
                        linea2 += watch.ElapsedMilliseconds + ";";
            */
            #endregion

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase07ClaseConTodo con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase07ClaseConTodo)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase07ClaseConTodo con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion

            // - SOAPFormatter
            #region SOAPFormatter
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase07ClaseConTodo con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase07ClaseConTodo)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase07ClaseConTodo con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
            #endregion

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase07ClaseConTodo));
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                DCserializer.WriteObject(stream1, c1);
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase07ClaseConTodo con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase07ClaseConTodo)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase07ClaseConTodo con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            // SharpSerializer (XML)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer.Serialize(c1, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase07ClaseConTodo con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase07ClaseConTodo)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase07ClaseConTodo con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c1, "SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase07ClaseConTodo con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase07ClaseConTodo)SharpSerializer2.Deserialize("SharpSerializer.dat");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase07ClaseConTodo con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
            /*
            linea1 += ";";
            linea2 += ";";
            */

            Console.WriteLine("Serialización con Protobuf");

                        Stream protoStream = new MemoryStream();
                        Stream protoStream2 = new MemoryStream();
                        watch.Restart(); // Comienza a contar el tiempo
                        for (int i = 0; i < veces; i++)
                        {
                            ProtoBuf.Serializer.Serialize(protoStream, c1);
                            if (i == 0)
                            {
                                protoStream2.Position = 0;
                                stream1.CopyTo(protoStream2, (int)protoStream.Length);
                            }

                        }
                        watch.Stop();
                        Console.WriteLine("Codificación Clase07ClaseConTodo con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
                        linea1 += watch.ElapsedMilliseconds + ";";

                        watch.Restart(); // Comienza a contar el tiempo
                        for (int i = 0; i < veces; i++)
                        {
                            protoStream.Position = 0;
                            c1decoded = ProtoBuf.Serializer.Deserialize<Clase07ClaseConTodo>(protoStream2);
                        }
                        watch.Stop();
                        Console.WriteLine("Decodificación Clase07ClaseConTodo con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
                        linea2 += watch.ElapsedMilliseconds + ";";
            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                    //                    strSerializado = Clase07ClaseConTodoCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase07ClaseConTodo con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                Console.WriteLine("Texto codificado (CSV): " + str);
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase07ClaseConTodo();
                c1decoded.basePublicInt = 0;
                c1decoded.lista = new List<int>();
                c1decoded.publicArray2DInt = null;
                c1decoded.publicArrayInt = null;
                c1decoded.publicArrayMatrizEscalonadaInt = null;
                c1decoded.publicInt = 0;
                c1decoded.publicStaticColores = Clase07ClaseConTodo.colores.ROJO;

                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase07ClaseConTodo con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            g1 = new Generador(c1.GetType(), "XML");
            serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase07ClaseConTodo con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                Console.WriteLine("Texto codificado (XML): " + str);
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase07ClaseConTodo();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase07ClaseConTodo con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }

        public string benchmarkClase08List()
        {
            string linea1 = "";
            string linea2 = "";
            FileStream fs;

            /*
             * 08. Clase con lista
             */
            linea1 += "Clase08List(Encode);";
            linea2 += "Clase08List (Decode);";
            Console.WriteLine("============== Clase con lista (Clase08Lit) ==============\r\n");

            // Instanciando y rellenando campos
            Clase08List c1 = new Clase08List();
            Clase08List c1decoded;

            c1.var1 = new List<int>();
            c1.var1.Add(1);
            c1.var1.Add(2);
            c1.var1.Add(3);

            c1.var2 = new List<Clase08List.clase>();

            Clase08List.clase cAux1 = new Clase08List.clase();
            cAux1.v1 = "Uno";
            cAux1.v2 = 2;
            c1.var2.Add(cAux1);

            Clase08List.clase cAux2 = new Clase08List.clase();
            cAux1.v1 = "Tres";
            cAux1.v2 = 4;
            c1.var2.Add(cAux2);

            Clase08List.clase cAux3 = new Clase08List.clase();
            cAux1.v1 = "Cinco";
            cAux1.v2 = 6;
            c1.var2.Add(cAux3);

            c1.var3 = new Dictionary<int, string>();
            c1.var3.Add(1, "uno");
            c1.var3.Add(2, "dos");
            c1.var3.Add(3, "tres");

            c1.var4 = new Dictionary<int, Clase08List.clase>();
            c1.var4.Add(1, cAux1);
            c1.var4.Add(2, cAux2);
            c1.var4.Add(3, cAux3);

            // - XMLSerializer
            #region XMLSerializer
            linea1 += ";";
            linea2 += ";";
/*
            XmlSerializer serializer = new XmlSerializer(typeof(Clase08List));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c1);
            }
            watch.Stop();
            writer.Close();

            // Repetimos una sola vez para generar el fichero co una única serialización
            writer = new StreamWriter("fichero.txt");
            serializer.Serialize(writer, c1);
            writer.Close();

            Console.WriteLine("Codificación Clase08List con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero.txt", FileMode.Open);
                c1decoded = (Clase08List)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase08List con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";"; 
*/
            #endregion XMLSerializer

            // - BinaryFormatter
            #region BinaryFormatter
            Console.WriteLine("Serialización con BinaryFormatter");

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    formatter.Serialize(fs, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs.Close();
                }
                Console.WriteLine("Codificación Clase08List con BinaryFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase08List)formatter.Deserialize(fs);
                    fs.Close();
                    fs = new FileStream("BinaryFormatter.dat", FileMode.Open);
                }
                watch.Stop();
                fs.Close();
                Console.WriteLine("Decodificación Clase08List con BinaryFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            #endregion BinaryFormatter

            /*
             * El serializador Soap no admite la serialización de tipos genéricos: System.Collections.Generic.List`1[System.Int32
             */ 
            // - SOAPFormatter
            #region SOAPFormatter
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con SOAPFormatter");

            FileStream fs3 = new FileStream("DataFile.soap", FileMode.Create);
            fs3.Close();
            SoapFormatter soapFormatter = new SoapFormatter();
            try
            {
                long tiempoTotal = 0;
                for (int i = 0; i < veces; i++)
                {
                    fs3 = new FileStream("DataFile.soap", FileMode.Create);
                    watch.Restart(); // Comienza a contar el tiempo
                    soapFormatter.Serialize(fs3, c1);
                    watch.Stop();
                    tiempoTotal += watch.ElapsedMilliseconds;
                    fs3.Close();
                }
                Console.WriteLine("Codificación Clase08List con SOAPFormatter: " + tiempoTotal + " milisegundos");
                linea1 += tiempoTotal + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }

            fs3 = new FileStream("DataFile.soap", FileMode.Open);
            try
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    c1decoded = (Clase08List)soapFormatter.Deserialize(fs3);
                    fs3.Close();
                    fs3 = new FileStream("DataFile.soap", FileMode.Open);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase08List con SOAPFormatter: " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs3.Close();
            }
*/ 
            #endregion SOAPFormatter

            // DataContractSerializer
            #region DataContractSerializer
            Console.WriteLine("Serialización con DataContractSerializer");

            MemoryStream stream1 = new MemoryStream();
            MemoryStream stream2 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer DCserializer = new DataContractSerializer(typeof(Clase08List));
            for (int i = 0; i < veces; i++)
            {
                watch.Restart(); // Comienza a contar el tiempo
                DCserializer.WriteObject(stream1, c1);
                watch.Stop();
                if (i == 0)
                {
                    stream1.Position = 0;
                    stream1.CopyTo(stream2, (int)stream1.Length);
                }
            }
            Console.WriteLine("Codificación Clase08List con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase08List)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase08List con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
            #endregion DataContractSerializer

            /*
             * Falla con un List de Object (List<Clase08List.clase>)
             */ 
            // SharpSerializer (XML)
            #region SharpSerializer
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con SharpSerializer (XML)");

            var SharpSerializer = new SharpSerializer();
            for (int i = 0; i < veces; i++)
            {
                watch.Restart(); // Comienza a contar el tiempo
                SharpSerializer.Serialize(c1, "SharpSerializer.xml");
                watch.Stop();
            }
            Console.WriteLine("Codificación Clase08List con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase08List)SharpSerializer.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase08List con SharpSerializer (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
*/ 
            #endregion SharpSerializer

            // SharpSerializer (Binario)
            #region SharpSerializer
            linea1 += ";";
            linea2 += ";";
/*
            Console.WriteLine("Serialización con SharpSerializer (Binario)");

            var SharpSerializer2 = new SharpSerializer(true);
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                SharpSerializer2.Serialize(c1, "SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Codificación Clase08List con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                c1decoded = (Clase08List)SharpSerializer2.Deserialize("SharpSerializer.xml");
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase08List con SharpSerializer (Binario): " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";
 */ 
            #endregion SharpSerializer

            // Protobuf
            #region Protobuf
//            linea1 += ";";
//            linea2 += ";";


            Console.WriteLine("Serialización con Protobuf");

            Stream protoStream = new MemoryStream();
            Stream protoStream2 = new MemoryStream();
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                ProtoBuf.Serializer.Serialize(protoStream, c1);
                if (i == 0)
                {
                    protoStream2.Position = 0;
                    stream1.CopyTo(protoStream2, (int)protoStream.Length);
                }

            }
            watch.Stop();
            Console.WriteLine("Codificación Clase08List con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea1 += watch.ElapsedMilliseconds + ";";

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                protoStream.Position = 0;
                c1decoded = ProtoBuf.Serializer.Deserialize<Clase08List>(protoStream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase08List con Protobuf: " + watch.ElapsedMilliseconds + " milisegundos");
            linea2 += watch.ElapsedMilliseconds + ";";

            #endregion Protobuf

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
/*
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();
            string str = "";
            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str = serializador1.codificar(c1);
                    //                    strSerializado = Clase07ClaseConTodoCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase08List con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase08List();

                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(str, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase08List con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
 */ 
            #endregion NuestroProyecto(CSV)

            // - Nuestro proyecto en XML
            #region NuestroProyecto(XML)
            Console.WriteLine("Serialización con nuestro proyecto en XML");

            // Generando el serializador a partir del tipo
            Generador g2 = new Generador(c1.GetType(), "XML");
            dynamic serializador2 = g2.getSerializer();
            string str2 = "";

            if (serializador2 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    str2 = serializador2.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase08List con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea1 += watch.ElapsedMilliseconds + ";";

                c1decoded = new Clase08List();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador2.decodificar(str2, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase08List con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
                linea2 += watch.ElapsedMilliseconds + ";";
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }

            return linea1 + "\r\n" + linea2;

            #endregion NuestroProyecto(XML)
        }
    }
}