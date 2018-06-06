using Orgsu.Xml;
using System;
using System.IO;
using System.Reflection;
using System.Timers;
using Orgsu.RFIDReader;
using Orgsu.RFIDReader.Configuration;

//TODO namespace
namespace RFIDReader
{
    /// <summary>
    /// Main RFID reader application class
    /// </summary>
    public class RFIDReader
    {
        //TODO parametr? zatim natvrdo
        private const string configFileName = "config.xml";
        /// <summary>
        /// RFIDReader is controller via this UI control form
        /// </summary>
        private readonly RFIDReaderForm controlForm;

        private RFIDApiLibManager rfidApiManager;
        private Timer timerReading;
        private bool isConnectionOpen = false;
        /// <summary>
        /// original tag id value
        /// </summary>
        private string tagId;
        /// <summary>
        /// processed tag id value without prefix, suffix
        /// </summary>
        private string processedTagId;
        /// <summary>
        /// is reading loop running
        /// </summary>
        private bool isReading;
        private RestApiCommunicator restApiCommunicator;

        /// <summary>
        /// Open connection flag
        /// true => open;
        /// false => closed
        /// </summary>
        public bool IsConnectionOpen { get => isConnectionOpen; }
        public Configuration Configuration { get; private set; }

        #region Constructors
        ///// <summary>
        ///// construtor without UI control form
        ///// </summary>
        //public RFIDReader()
        //{
        //    Console.WriteLine("RFIDReader object created without control UI form.");
        //    Initialize();
        //}

        /// <summary>
        /// construtor with UI control form
        /// </summary>
        /// <param name="controlForm">RFIDReader application control UI form</param>
        public RFIDReader(RFIDReaderForm controlForm)
        {
            this.controlForm = controlForm;
            Console.WriteLine("RFIDReader object created with control UI form.");
            Initialize();
        }
        #endregion
        // TODO destructor/finalize - zavrit spojeni
        // + ulozit log

        private void Initialize()
        {
            //TODO - zachyceni i neosetrenych exception:
            //https://logmatic.io/blog/logging-in-net-the-power-of-c-logs/
            try
            {
                //TODO create/field Log

                // TODO log jako parametr konstruktoru
                rfidApiManager = new RFIDApiLibManager();
                timerReading = new Timer();
                timerReading.AutoReset = false;
                timerReading.Elapsed += OnTimedEvent;


                Console.WriteLine("Initialization...");
                //TODO create/field Config + load config! (pracovni adresar/app exepath)
                string appExecPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string configfile = Path.Combine(appExecPath, configFileName);

                //TODO try + ukladani zpet
                //TODO public config + vyplnit UI (nabindovat)
                try
                {
                    //TODO vycist z formu/default configuration kdyz se nenajde?
                    Configuration = XmlUtils.DeSerializeObject<Configuration>(configfile);
                    UpdateReaderFormData();
                }
                catch (Exception)
                {
                    //TODO nahradit logem -> podle typu app psat do konzole/formu
                    Console.WriteLine($"Unable to load configuration file: {configfile}.\nConfiguration file must be valid and present in application folder to work correctly.");
                    controlForm.RtbInfo.AppendText($"Unable to load configuration file: {configfile}.\nConfiguration file must be valid and present in application folder to work correctly.\n");
                }

                // create RestApiCommunicator - needs configuration parameter
                restApiCommunicator = new RestApiCommunicator(Configuration.ValidationData);

                if (Configuration.RunAutomatically)
                {
                    //TODO open/closeconnection button nabindovat na isconnectionopen - mozna get, private set + notifikace?
                    int status = OpenConnection(Configuration.ComPort);
                }
                UpdateReaderFormData();
            }
            catch (Exception exception)
            {
                // TODO close jen kdyz je open?
                CloseConnection();
                //TODO open/CloseConnection button nabindovat na IsConnectionOpen - mozna get, private set + notifikace?
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to initialize. Please restart application.\nException: {exception.Message}");
                controlForm.RtbInfo.AppendText($"Unable to initialize. Please restart application.\nException: {exception.Message}\n");
            }
        }

        /// <summary>
        /// Updates data from Configuration to Reader form controls
        /// </summary>
        private void UpdateReaderFormData()
        {
            controlForm.ComboBoxSerialPort = Configuration.ComPort;
            controlForm.BtnOpenText = IsConnectionOpen ? "Stop" : "Start";
            controlForm.TbPrefix = Configuration.TagPrefixAsHex;
            controlForm.TbSuffix = Configuration.TagSuffixAsHex;
            var urlReplacePairs = Configuration.ValidationData.UrlReplacePairs;
            controlForm.TbDeviceId = urlReplacePairs.Exists(pair => pair.Name == "DeviceID") ? urlReplacePairs.Find(pair => pair.Name == "DeviceID").Value : "";
            controlForm.TbRaceDayId = urlReplacePairs.Exists(pair => pair.Name == "RaceDayID") ? urlReplacePairs.Find(pair => pair.Name == "RaceDayID").Value : "";
        }

        private void StartReadingLoop()
        {
            try
            {
                if (isReading)
                {
                    //TODO nahradit logem -> podle typu app psat do konzole/formu
                    Console.WriteLine("Unable to star reading loop. Connection is open, reading loop is already runnning.");
                    controlForm.RtbInfo.AppendText("Unable to star reading loop. Connection is open, reading loop is already runnning.");
                    return;
                }

                timerReading.Interval = Configuration.ReadTagDelay;
                // must be set at the start, to remain thread safe
                if (controlForm != null)
                {
                    timerReading.SynchronizingObject = controlForm;
                }

                isReading = true;
                
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine("Tag reading started.");
                controlForm.RtbInfo.AppendText("Tag reading started.\n");

                timerReading.Start();
            }
            catch (Exception exception)
            {
                timerReading.Stop();
                //TODO open/CloseConnection button nabindovat na IsConnectionOpen - mozna get, private set + notifikace?
                CloseConnection();

                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to star reading loop.\nException: {exception.Message}");
                controlForm.RtbInfo.AppendText($"Unable to star reading loop.\nException: {exception.Message}\n");
            }
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            int delay = Configuration.ReadTagDelay;
            // reset, stop timer
            timerReading.Stop();

            // closed connecdtion - stop reading
            if (!isConnectionOpen)
            {
                isReading = false;
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine("Tag reading stopped.");
                controlForm.RtbInfo.AppendText("Tag reading stopped.\n");
                return;
            }

            string processedTagID = EpcMultiTagRead();
            if (!String.IsNullOrEmpty(processedTagID))
            //send tag
            {
                // wait before reading tags again
                delay = Configuration.SendTagDelay;
                // send tag
                restApiCommunicator.SendTagIdAsync(processedTagID);

            }
            else
            // repeat reading tags
            {
                delay = Configuration.ReadTagDelay;
            }

            // TODO jen pro test, odstranit
            Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}: OnTimer event occured. Delay: {delay}.");
            //controlForm.RtbInfo.AppendText($"{DateTime.Now.ToString("hh:mm:ss")}: OnTimer event occured. Delay: {delay}.\n");
            timerReading.Interval = delay;
            timerReading.Start();
        }

        /// <summary>
        /// RFIDReader hardware attempts to read tag
        /// returns processed tag (without prefix, suffix) if read successfully
        /// returns empty string if reading was not successfull
        /// </summary>
        /// <returns>returns processed tag without prefix, suffix if successfull; otherwise empty string</returns>
        private string EpcMultiTagRead()
        {
            tagId = "";
            processedTagId = "";
            try
            {
                byte[] tagRow = new byte[Configuration.TagByteLengt];

                // always empty buffer before identifing new set
                rfidApiManager.ClearTagBuffer();
                int status = rfidApiManager.EpcMultiTagIdentify();

                if ((status == 0) && (rfidApiManager.TagCount > 0))
                {
                    //take first tag
                    for (int i = 0; i < Configuration.TagByteLengt; i++)
                    {
                        tagRow[i] = rfidApiManager.TagBuffer[0, i];
                    }
                    tagId = BitConverter.ToString(tagRow).Replace("-", "");

                    //TODO nahradit logem -> podle typu app psat do konzole/formu
                    Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}: Tag read: {tagId}.");
                    controlForm.RtbInfo.AppendText($"{DateTime.Now.ToString("hh:mm:ss")}: Tag read: {tagId}.\n");
                    processedTagId = ProcessTagId(tagId);
                }
            }
            catch (Exception exception)
            {
                tagId = "";
                processedTagId = "";
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to read tag.\nException: {exception.Message}");
                controlForm.RtbInfo.AppendText($"Unable to read tag.\nException: {exception.Message}\n");
            }

            return processedTagId;
        }

        private string ProcessTagId(string tagId)
        {
            string result = "";
            try
            {
                //TODO provazat formu/konfigurace
                string pref = Configuration.TagPrefixAsHex.ToUpper();
                string suf = Configuration.TagSuffixAsHex.ToUpper();

                //TODO nahradit regex/linq/necim jednodussim
                int prefixIndex = -1;
                int sufIndex = -1;
                string remainingString = tagId;

                for (int i = 0; i < tagId.Length; i++)
                {
                    remainingString = tagId.Substring(i);

                    // prefix
                    if (String.IsNullOrEmpty(pref))
                    {
                        prefixIndex = 0;
                    }
                    else
                    {
                        if (remainingString.StartsWith(pref))
                        {
                            prefixIndex = i;
                        }
                    }

                    // suffix
                    if (String.IsNullOrEmpty(suf))
                    {
                        sufIndex = tagId.Length;
                    }
                    else
                    {
                        if (remainingString.StartsWith(suf))
                        {
                            sufIndex = i;
                        }
                    }

                    // pref + suf
                    if ((prefixIndex > -1) && (sufIndex > -1) && (prefixIndex <= sufIndex))
                    {
                        result = tagId.Substring(prefixIndex + pref.Length, sufIndex - (prefixIndex + pref.Length));
                        break;
                    }
                }

            }
            catch (Exception exception)
            {
                result = "";
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to process tag.\nException: {exception.Message}");
                controlForm.RtbInfo.AppendText($"Unable to process tag.\nException: {exception.Message}\n");
            }

            //TODO nahradit logem -> podle typu app psat do konzole/formu
            Console.WriteLine($"{DateTime.Now.ToString("hh:mm:ss")}: Processed tag: {result}.");
            controlForm.RtbInfo.AppendText($"{DateTime.Now.ToString("hh:mm:ss")}: Processed tag: {result}.\n");
            return result;
        }

        /// <summary>
        /// Opens connection on serial port comPort and starts tag reading loop
        /// returns:
        /// result = 0 - successfull
        /// result <> 0 - unsuccessfull, specific error code
        /// </summary>
        /// <param name="comPort">COM port i.e.: COM1</param>
        /// <returns>result of the operation 0 - success, othervise error code</returns>
        public int OpenConnection(string comPort)
        {
            //TODO provazani s formeme + configem!!
            int status = rfidApiManager.OpenConnection(comPort);

            if (status == 0)
            {
                isConnectionOpen = true;
                //controlForm.BtnOpenText = "Closed";
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Connection is open on port: {comPort}.");
                controlForm.RtbInfo.AppendText($"Connection is open on port: {comPort}.\n");

                StartReadingLoop();
            }
            else
            {
                isConnectionOpen = false;
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine($"Unable to open connection on port: {comPort}.");
                controlForm.RtbInfo.AppendText($"Unable to open connection on port: {comPort}.\n");
            }

            return status;
        }

        /// <summary>
        /// Closes connection on serial port comPort
        /// returns:
        /// result = 0 - successfull
        /// result <> 0 - unsuccessfull, specific error code
        /// </summary>
        /// <returns>result of the operation 0 - success, othervise error code</returns>
        public int CloseConnection()
        {
            isConnectionOpen = false;
            int status = rfidApiManager.CloseComConnection();
            //controlForm.BtnOpenText := "Closed";
            if (status == 0)
            {
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine("Connection closed.");
                controlForm.RtbInfo.AppendText($"Connection closed.\n");
            }
            else
            {
                //TODO nahradit logem -> podle typu app psat do konzole/formu
                Console.WriteLine("Unable to close connection properly.");
                controlForm.RtbInfo.AppendText("Unable to close connection properly.\n");
            }
            return status;
        }
    }
}
