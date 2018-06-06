using System;

namespace Orgsu.RFIDReader.Configuration
{
    /// <summary>
    /// Encapsulates string that should be replaced, its name and replacement value
    /// </summary>
    [Serializable()]
    public class UrlReplacement
    {
        public string Name { get; set; }
        public string Replacement { get; set; }
        public string Value { get; set; }
    }
}