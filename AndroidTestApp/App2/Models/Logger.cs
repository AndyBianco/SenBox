using System;
using att.iot.client;

namespace Senbox.Share.Models
{
    class Logger : ILogger
    {
        public String Message { get; set; }

        public void Error(string message, params object[] args) => Message = message;

        public void Info(string message, params object[] args) => Message = message;

        public void Trace(string message, params object[] args) => Message = message;

        public void Warn(string message, params object[] args) => Message = message;
    }
}
