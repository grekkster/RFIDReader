using System;
using Orgsu.Xml;

namespace Orgsu.RFIDReader.Configuration
{
    /// <summary>
    /// RFIDReader application configuration
    /// </summary>
    [Serializable()]
    public class Configuration
    {
        /*
        Com port, tag prefix, tag suffix, raceday_id, device_id
        Timer read delay, rest send delay
        Log file + rewrite?
        adresa cílové rest služby:
        https://arvs.orgsu.futuredev.cz/api/v1/validation/device{0}/raceday/{1}
        AutomaticConnection
        Délka vyčteného tagu?
        Co se stane, pokud konfig není nalezen? Default pokud nenajde soubor?
        */

        //TODO pri get/set upravit i ty na formu?
        //TODO !!! to stejny pridat pro posilani JSON!!! string json = "{\"id\":\"" + tagId + "\"}";

        //public int ComPort { get; set; } //TODO validation
        public string ComPort { get; set; } //TODO validation
        public string TagPrefixAsHex { get; set; } // TEST pouze na hex  //TODO validation
        public string TagSuffixAsHex { get; set; } // TEST pouze na hex  //TODO validation
        public ValidationData ValidationData { get; set; } // TODO objekt - url + {x} + parametry k nahrazení! - list s parametry + prop count s poctem polozek?
        public int ReadTagDelay { get; set; } //TODO validation
        public int SendTagDelay { get; set; } //TODO validation
        public string LogFile { get; set; }  //TODO co se ma stat kdyz neni logfile?
        public bool RunAutomatically { get; set; }
        public byte TagByteLengt { get; set; } //TODO validation

        public static void SaveToXml(Configuration configuration, string fileName)
        {
            XmlUtils.SerializeObject(configuration, fileName);
        }

        public static Configuration LoadFromXml(string fileName)
        {
            return XmlUtils.DeSerializeObject<Configuration>(fileName);
        }

        //TODO https://stackoverflow.com/questions/28055274/deserialize-object-to-itself
        //public void SaveToXml(string fileName)
        //{
        //    //https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-serialize-an-object
        //    //TODO ne stringBuilder ale ulozit do souboru!!
        //    var aSerializer = new XmlSerializer(typeof(Configuration));
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter sw = new StringWriter(sb);
        //    aSerializer.Serialize(sw, this);
        //    string xmlResult = sw.GetStringBuilder().ToString();
        //}

        //TODO https://stackoverflow.com/questions/28055274/deserialize-object-to-itself
        //public void LoadFromXml(string fileName)
        //{
        //    //https://docs.microsoft.com/en-us/dotnet/standard/serialization/how-to-deserialize-an-object
        //    XmlSerializer serializer = new XmlSerializer(typeof(Configuration));
        //    //TODO osetrit neexistenci souboru - tohle presunout, resit v configuration? pokud nenajde, default config
        //    using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
        //    {
        //        this = (Configuration)serializer.Deserialize(fileStream);
        //    };
        //}
    }
}
