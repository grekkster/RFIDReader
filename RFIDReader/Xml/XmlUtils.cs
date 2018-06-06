using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Orgsu.Xml
{
    //TODO presunout do common namespace/knihoven!!
    //TODO optimalizovat podle odkazu
    public static class XmlUtils
    {
        //TODO precist, vylepsit, optimalizovat
        //https://www.codeproject.com/Articles/4491/Load-and-save-objects-to-XML-using-serialization
        //https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-serialize-an-object
        //https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-deserialize-an-object
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-read-object-data-from-an-xml-file
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/serialization/how-to-write-object-data-to-an-xml-file
        //https://codehandbook.org/c-object-xml/
        //https://stackoverflow.com/questions/28055274/deserialize-object-to-itself
        //https://stackoverflow.com/questions/1081325/c-sharp-how-to-xml-deserialize-object-itself

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public static void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception exception)
            {
                //Log exception here
                throw new Exception($@"Unable to serialize object.\nException: {exception.Message}");
            }
        }


        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception exception)
            {
                //Log exception here
                throw new Exception($@"Unable to deserialize file: {fileName}.\nException: {exception.Message}");
            }

            return objectOut;
        }
    }
}
