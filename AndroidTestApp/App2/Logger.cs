using att.iot.client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
namespace App2
{
    class Logger : ILogger
    {
        public void Error(string message, params object[] args)
        {
            Console.WriteLine("Error : " + message);
        }

        public void Info(string message, params object[] args)
        {
            Console.WriteLine("Info : "+message);
        }

        public void Trace(string message, params object[] args)
        {
            Console.WriteLine(message);
        }

        public void Warn(string message, params object[] args)
        {
            Console.WriteLine("Warning : " + message);
        }
    }
}
