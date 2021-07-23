using System;
using System.IO.Ports;

namespace AudioVisualizer.ExtDrawer
{
    class ComExtDrawer : ExtBaseDrawer
    {
        readonly string SetModeCommand = "startSpectrum" + StopChar;

        readonly SerialPort serialPort;
        string serialData;


        public ComExtDrawer(string url) : base(url)
        {
            serialPort = new SerialPort();
            serialData = "";

            try
            {
                serialPort.PortName = this.url;
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.Parity = System.IO.Ports.Parity.None;
                serialPort.StopBits = System.IO.Ports.StopBits.One;
                serialPort.ReadTimeout = 1000;
                serialPort.DtrEnable = true;
                serialPort.Open();

                serialPort.Write(SetModeCommand);

                serialPort.DataReceived += SerialPort_DataReceived;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("невозможно открыть порт " + serialPort.PortName);
                return;
            }

        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort.BytesToRead > 0)
            {
                var ch = (char)serialPort.ReadChar();

                if (ch == StopChar)
                {
                    AnalyseData(serialData);
                    serialData = "";
                }
                else
                {
                    serialData += ch;
                }
            }
        }


        public override void SendData(string data)
        {
            if (serialPort.IsOpen == false) { return; }

            try
            {
                serialPort.Write(data);
            }
            catch (TimeoutException)
            {
                haveError = true;
                serialPort.Close();
            }

        }

        public override void Dispose()
        {
            base.Dispose();
            serialPort.Close();
        }

        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

    }
}
