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

namespace Fase07
{
    class Program
    {
        static Stopwatch watch;
        static int veces = 100000;
        static void Main(string[] args)
        {
            Program p = new Program();
            watch = new System.Diagnostics.Stopwatch();
            string strSerializado = ""; // Código serializado intermedio en todos los casos

            /*
             * 01. Clase básica
             */
            Console.WriteLine("============== Clase con dos campos (Clase01Basica) ==============");

            // Instanciando y rellenando campos
            Clase01Basica c1 = new Clase01Basica();
            Clase01Basica c1decoded;
            c1.var1 = 1;
            c1.var2 = "2";

            // - XMLSerializer
            #region XMLSerializer
            Console.WriteLine("Serialización con XMLSerializer");

            XmlSerializer serializer = new XmlSerializer(typeof(Clase01Basica));
            TextWriter writer = new StreamWriter("fichero.txt");

            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                serializer.Serialize(writer, c1);
            }
            watch.Stop();
            writer.Close();
            Console.WriteLine("Codificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");

            FileStream fs;
            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                fs = new FileStream("fichero2.txt", FileMode.Open);
                c1decoded = (Clase01Basica)serializer.Deserialize(fs);
                fs.Close();
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con XMLSerializer: " + watch.ElapsedMilliseconds + " milisegundos");
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
            Console.WriteLine("Codificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");


            watch.Restart(); // Comienza a contar el tiempo
            for (int i = 0; i < veces; i++)
            {
                stream2.Position = 0;
                //Deserialize the Record object back into a new record object.
                c1decoded = (Clase01Basica)DCserializer.ReadObject(stream2);
            }
            watch.Stop();
            Console.WriteLine("Decodificación Clase01Basica con DataContractSerializer: " + watch.ElapsedMilliseconds + " milisegundos");

/*

            MemoryStream stream1 = new MemoryStream();
            //Serialize the Record object to a memory stream using DataContractSerializer.
            DataContractSerializer serializer = new DataContractSerializer(typeof(Clase01Basica));
            serializer.WriteObject(stream1, instanciaClase01Basica);

            stream1.Position = 0;
            //Deserialize the Record object back into a new record object.
            Clase01Basica instanciaClase01Basica2 = (Clase01Basica)serializer.ReadObject(stream1);
*/
            #endregion DataContractSerializer

            // - Nuestro proyecto en CSV
            #region NuestroProyecto(CSV)
            Console.WriteLine("Serialización con nuestro proyecto (CSV)");

            // Generando el serializador a partir del tipo
            Generador g1 = new Generador(c1.GetType());
            dynamic serializador1 = g1.getSerializer();

            if (serializador1 != null)
            {
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    strSerializado = serializador1.codificar(c1);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");

                c1decoded = new Clase01Basica();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(strSerializado, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (CSV): " + watch.ElapsedMilliseconds + " milisegundos");
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
                    strSerializado = serializador1.codificar(c1);
                    //                    strSerializado = Clase01BasicaCodec.codificar(c1);
                }
                watch.Stop();
                Console.WriteLine("Codificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");

                c1decoded = new Clase01Basica();
                watch.Restart(); // Comienza a contar el tiempo
                for (int i = 0; i < veces; i++)
                {
                    serializador1.decodificar(strSerializado, ref c1decoded);
                }
                watch.Stop();
                Console.WriteLine("Decodificación Clase01Basica con nuestro proyecto (XML): " + watch.ElapsedMilliseconds + " milisegundos");
            }
            else
            {
                Console.WriteLine("Error generando el serializador");
            }
            #endregion NuestroProyecto(XML)

            Console.WriteLine("==================");
            Console.ReadLine();
        }
    }
}
