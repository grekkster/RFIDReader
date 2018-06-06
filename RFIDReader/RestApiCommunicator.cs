using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Orgsu.RFIDReader.Configuration;

namespace Orgsu.RFIDReader
{
    /// <summary>
    /// RestApiCommunicator is used for communication with Rest Api
    /// - sends tag data for validation
    /// </summary>
    class RestApiCommunicator
    {
        // url example without replacement: //example: https://arvs.orgsu.futuredev.cz/api/v1/validation/device{0}/raceday/{1}

        //TODO ctor!/properta url, device_id, raceday_id - z konfig. souboru - musi znat, lepsi ctor
        //TODO Log DI ctor param??
        //TODO urlValidation, testDeviceID, testRaceDayID - co property, co ctor? url je zadano z configu, tak asi const,
        // testDeviceID, testRaceDayID se mohou menit
        //NEBO budou urlValidation, testDeviceID, testRaceDayID parametry metody???

        private static readonly HttpClient client = new HttpClient();
        //TODO udelat univerzalne!!!
        private ValidationData validationData;
        //private readonly Logger.Logger logger;
        //static HttpClient client = new HttpClient();

        //public string UrlValidation { get; set; }
        //public string DeviceID { get; set; }
        //public string RaceDayID { get; set; }

        public RestApiCommunicator(ValidationData validationData)
        {
            this.validationData = validationData;
        }

        //public RestApiCommunicator(ValidationData validationData, Logger.Logger logger)
        //{
        //    ValidationData = validationData;
        //    this.logger = logger;
        //}

        //TODO vracet result/response/neco jineho?
        public async Task SendTagIdAsync(string tagId)
        {
            try
            {
                //TODO univerzalne:
                //najit nahradni, nahradit value, opakovat !! pozor aby se neopakovalo - po nahrazeni vzit vzdy zbytek textu

                string processedUrl = validationData.UrlValidation;

                // TODO nejak slusne tvorit json + content?
                //var postData = new Dictionary<string, string>
                //{
                //    //TODO dvojtecka?
                //    { "id:", tagId }
                //};
                //var content = new FormUrlEncodedContent(postData); // "error":"Unsupported Media Type","exception":"org.springframework.web.HttpMediaTypeNotSupportedException","message":"Content type 'application/x-www-form-urlencoded;charset=UTF-8' not supported","path":"/api/v1/validation/device/1/raceday/XA8B3A1F"}

                string json = "{\"id\":\"" + tagId + "\"}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(processedUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();

                //logger.Log(Logger.LogEntryLevel.Info, responseString);
            }
            catch (Exception exception) //HttpRequestException | ArgumentNullException
            {
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to send tag to url.\nException: {exception.Message}");
                //controlForm.RtbInfo.AppendText($"Unable to send tag to url.\nException: {exception.Message}\n");
            }
        }
    }
}
