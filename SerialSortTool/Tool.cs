using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialSortTool
{
    public class Tool
    {
        public Tool() { }

        public Tool(IHubContext<SerialHub> hubContext)
        {
            HubContext = hubContext;
        }

        //private static SerialPort mySerialPort;
        private static Dictionary<string, SerialPort> SerialList = new Dictionary<string, SerialPort>();

        public IHubContext<SerialHub> HubContext { get; }

        public void Start(string portName, int baud, int stopBits, int parity, int dataBits)
        {
            SerialPort mySerialPort = SerialList.GetValueOrDefault(portName, null);
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Close();
                mySerialPort = null;
            }
            if (mySerialPort == null || !mySerialPort.IsOpen)
            {
                mySerialPort = new SerialPort(portName)
                {
                    BaudRate = baud,
                    Parity = (Parity)parity,
                    StopBits = (StopBits)stopBits,
                    DataBits = dataBits,
                    Handshake = Handshake.None
                };

                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                mySerialPort.Open();

                SerialList[portName] = mySerialPort;
            }
        }

        public void Stop(string portName)
        {
            SerialPort mySerialPort = SerialList.GetValueOrDefault(portName, null);
            if (mySerialPort != null && mySerialPort.IsOpen)
            {
                mySerialPort.Close();
                mySerialPort = null;
                SerialList.Remove(portName);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting().Trim();
            //HubContext.Clients.Group(sp.PortName).SendAsync("received", JsonConvert.SerializeObject(new { PortName = sp.PortName, data = indata }));
            Program.hubContext.Clients.Group(sp.PortName).SendAsync("received", JsonConvert.SerializeObject(new { PortName = sp.PortName, data = indata })).Wait();
            Console.WriteLine("新消息");
        }
    }
}
