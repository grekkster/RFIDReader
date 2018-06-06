using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RFIDReader
{
    public partial class RFIDReaderForm : Form
    {
        private RFIDReader rfidReader;

        /// <summary>
        /// Info text box
        /// </summary>
        public RichTextBox RtbInfo { get => rtbInfo; }
        /// <summary>
        /// Open button text
        /// </summary>
        public string BtnOpenText
        {
            get => btnOpen.Text;
            set => btnOpen.Text = value;
        }
        /// <summary>
        /// TextBox prefix text
        /// </summary>
        public string TbPrefix
        {
            get => tbPrefix.Text;
            set => tbPrefix.Text = value;
        }
        /// <summary>
        /// TextBox suffix text
        /// </summary>
        public string TbSuffix
        {
            get => tbSuffix.Text;
            set => tbSuffix.Text = value;
        }
        /// <summary>
        /// TextBox DeviceId text
        /// </summary>
        public string TbDeviceId
        {
            get => tbDeviceId.Text;
            set => tbDeviceId.Text = value;
        }
        /// <summary>
        /// TextBox RaceDayId text
        /// </summary>
        public string TbRaceDayId
        {
            get => tbRaceDayId.Text;
            set => tbRaceDayId.Text = value;
        }
        /// <summary>
        /// Selected serial port combo box item
        /// </summary>
        public string ComboBoxSerialPort
        {
            get => cbSerialPort.SelectedText;
            set
            {
                if (cbSerialPort.FindStringExact(value) > -1)
                    cbSerialPort.SelectedIndex = cbSerialPort.FindStringExact(value);
            }
        }

        public RFIDReaderForm()
        {
            InitializeComponent();
            cbSerialPort.DataSource = SerialPort.GetPortNames();
            rfidReader = new RFIDReader(this);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            int status = -1;

            if (rfidReader.IsConnectionOpen)
            {
                status = rfidReader.CloseConnection();
                btnOpen.Text = "Start";
            }
            else
            {
                if (!SerialPort.GetPortNames().Contains(cbSerialPort.SelectedItem))
                    return;

                // TODO nahradit provazanim - binding
                rfidReader.Configuration.ComPort = cbSerialPort.SelectedItem.ToString();
                //update configuration
                rfidReader.Configuration.TagPrefixAsHex = TbPrefix;
                rfidReader.Configuration.TagSuffixAsHex = TbSuffix;
                var urlReplacePairs = rfidReader.Configuration.ValidationData.UrlReplacePairs;
                if (urlReplacePairs.Exists(pair => pair.Name == "DeviceID"))
                {
                    urlReplacePairs.Find(pair => pair.Name == "DeviceID").Value = TbDeviceId;
                }
                if (urlReplacePairs.Exists(pair => pair.Name == "RaceDayID"))
                {
                    urlReplacePairs.Find(pair => pair.Name == "RaceDayID").Value = TbRaceDayId;
                }

                status = rfidReader.OpenConnection(cbSerialPort.SelectedItem.ToString());
                if (status == 0)
                    btnOpen.Text = "Stop";
            }
        }

        private void cbSerialPort_Click(object sender, EventArgs e)
        {
            cbSerialPort.DataSource = SerialPort.GetPortNames();
        }

        private void rtbInfo_TextChanged(object sender, EventArgs e)
        {
            // scroll automatically to the last entry
            rtbInfo.SelectionStart = rtbInfo.Text.Length;
            rtbInfo.ScrollToCaret();
        }

        private void tbPrefix_TextChanged(object sender, EventArgs e)
        {
            tbPrefix.Text = tbPrefix.Text.Replace(" ", "");
            string item = tbPrefix.Text;
            Int64 n = 0;
            if (!Int64.TryParse(item, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out n) &&
              item != String.Empty)
            {
                tbPrefix.Text = item.Remove(item.Length - 1, 1);
            }
            tbPrefix.SelectionStart = tbPrefix.Text.Length;
        }

        private void tbSuffix_TextChanged(object sender, EventArgs e)
        {
            tbSuffix.Text = tbSuffix.Text.Replace(" ", "");
            string item = tbSuffix.Text;
            Int64 n = 0;
            if (!Int64.TryParse(item, System.Globalization.NumberStyles.HexNumber, System.Globalization.NumberFormatInfo.CurrentInfo, out n) &&
              item != String.Empty)
            {
                tbSuffix.Text = item.Remove(item.Length - 1, 1);
            }
            tbSuffix.SelectionStart = tbSuffix.Text.Length;
        }
    }
}
