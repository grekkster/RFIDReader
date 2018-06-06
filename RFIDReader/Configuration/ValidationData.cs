using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orgsu.RFIDReader.Configuration
{
    /// <summary>
    /// Encapsulates URL address with tempotaty substrings which should be replaced
    /// UrlValidation - URL validation address with tempotaty substrings to be replaced
    /// UrlReplacePairs - list of items to be replaced (name; to be replaced string; replacement string)
    /// </summary>
    [Serializable()]
    public class ValidationData
    {
        /// <summary>
        /// URL validation address with tempotaty substrings to be replaced
        /// </summary>
        public string UrlValidationTemplate { get; set; }

        // TODO na form pak vypsat list key value, ne natvrdo
        /// <summary>
        /// list of items to be replaced (name; to be replaced string; replacement string)
        /// </summary>
        public List<UrlReplacement> UrlReplacePairs { get; set; }

        /// <summary>
        /// Final URL validation address with tempotaty substrings already replaced by values
        /// - if unable to replace tokens, returns empty string
        /// </summary>
        public string UrlValidation
        {
            get
            {
                string processedUrl = UrlValidationTemplate;
                try
                {
                    Dictionary<string, string> replacements = new Dictionary<string, string>();
                    foreach (var urlReplaceItem in UrlReplacePairs)
                    {
                        replacements.Add(urlReplaceItem.Replacement, urlReplaceItem.Value);
                    }

                    foreach (var k in replacements.Keys)
                    {
                        processedUrl = processedUrl.Replace(k, replacements[k]);
                    }
                }
                catch (Exception)
                {

                    processedUrl = String.Empty;
                }
                return processedUrl;
            }
        }

    }
}
