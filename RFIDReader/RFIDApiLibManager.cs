using System;

namespace Orgsu.RFIDReader
{
    // TODO muze byt static?
    /// <summary>
    /// RFIDApiLib manager
    /// - RFIDApiLib is an SDK provided library working with RFID reader hardware
    /// </summary>
    public class RFIDApiLibManager
    {
        /* Command response codes (Communication Agreement-EN.pdf):
        0x00 ERR_NONE Successful completion thecommand
        0x01 ERR_ GENERAL_ERR General mistake
        0x02 ERR_PAR_SET_FAILED Fail to sset parameter
        0x03 ERR_PAR_GET_FAILED Fail to read parameter
        0x04 ERR_NO_TAG No tag
        0x05 ERR_READ_FAILED Fail to read tag
        0x06 ERR_WRITE_FAILED Fail to read-in tag
        0x07 ERR_LOCK_FAILED Fail to stipulate tag
        0x08 ERR_ERASE_FAILED 标签 fail to erasuretag
        0x09
        0x0A
        0xFE ERR_CMD_ERR Nonsupport by command orparameter exceed the rang
        0xFF ERR_UNDEFINED Undefinition mistake
        */

        // default status error code for command failure
        const int cCommandError = -1;
        // default status code for successfully executed command
        const int cCommandSuccessfull = 0;
        // default communication baud rate
        const int cDefaultBaudRate = 9600;

        private RfidApiLib.RfidApi rfidApi = new RfidApiLib.RfidApi();
        private bool isOpen = false;
        // tag buffer storage
        // TODO jak incializovat? count zjisti stejna metoda, ktera vysledek vraci do pole
        // TODO random 100 nahradit necim validnim, 12 je mozne nastavit:
        //  rfidApi.ByteSize; / rfidApi.EpcInitEpc(bitcount);
        private byte[,] tagBuffer = new byte[100, 12];
        // tag count
        private byte tagCount;
        // tag flags
        private byte tagFlags;

        public bool IsOpen { get => isOpen; }
        public byte[,] TagBuffer { get => tagBuffer; }
        public byte TagCount { get => tagCount; }
        public byte TagFlags { get => tagFlags; }

        //byte[,] tagBuffer = new byte[100, 12];
        //byte tagCount;
        //byte tagFlags;
        //const int cDefBaudRate = 9600;

        //TODO - ctor - stringbuilder=log?
        //TODO obalit jakoukoli fci try blokem + zapsat chyby - jedna sablona, volat metodu, obalit sablonou, pokud chyba vrati int -1?
        //TODO nastaveni propert:
        //public byte PortType;
        //public int BaudRate;
        //public byte ByteSize;
        //public byte Parity;
        //public byte StopBits;
        //public int ReadTimeout;
        //public bool Opened;
        //public Socket sock;

        /// <summary>
        /// Opens connection on serial port comPort
        /// returns:
        /// result = 0 - successfull
        /// result <> 0 - unsuccessfull, specific error code
        /// </summary>
        /// <param name="comPort">COM port i.e.: COM1</param>
        /// <returns>result of the operation 0 - success, othervise error code</returns>
        public int OpenConnection(string comPort)
        {
            // default command result
            int status = cCommandError;
            byte v1 = 0;
            byte v2 = 0;

            try
            {
                if (IsOpen)
                    CloseComConnection();
                status = rfidApi.OpenCommPort(comPort);
                if (status != 0)
                {
                    return status;
                }

                // verify connection with reader
                status = rfidApi.GetFirmwareVersion(ref v1, ref v2);
                if (status != 0)
                {
                    rfidApi.CloseCommPort();
                    return status;
                }
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
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
        public int CloseComConnection()
        {
            // default command result
            int status = cCommandError;

            try
            {
                rfidApi.CloseCommPort();
                status = cCommandSuccessfull;
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
            }

            isOpen = false;
            return status;
        }

        /// <summary>
        /// Reads multiple tags
        /// result in: TagBuffer, TagCount, TagFlags
        /// </summary>
        /// <returns>result of the operation 0 - success, othervise error code</returns>
        public int EpcMultiTagIdentify()
        {
            // default command result
            int status = cCommandError;

            try
            {
                status = rfidApi.EpcMultiTagIdentify(ref tagBuffer, ref tagCount, ref tagFlags);
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
            }

            return status;
        }

        /// <summary>
        /// Clear tag buffer
        /// </summary>
        /// <returns>result of the operation 0 - success, othervise error code</returns>
        public int ClearTagBuffer()
        {
            // default command result
            int status = cCommandError;

            try
            {
                status = rfidApi.ClearIdBuf(); // always empty buffer before identifing new set
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
            }

            return status;
        }

        public int ResetReader()
        {
            // default command result
            int status = cCommandError;

            try
            {
                status = rfidApi.ResetReader();
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
            }

            isOpen = false;
            return status;
        }

        public int GetFirmwareVersion(ref byte major, ref byte minor)
        {
            // default command result
            int status = cCommandError;

            try
            {
                status = rfidApi.GetFirmwareVersion(ref major, ref minor);
            }
            catch (Exception exception)
            {
                //tbInfo.AppendText("CloseComConnection failed. Exception: " + exc.Message + Environment.NewLine);
                //TODO neco udelat s exception text - log
            }

            return status;
        }
    }
}
