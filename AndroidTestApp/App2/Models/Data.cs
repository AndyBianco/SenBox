using System;
using Newtonsoft.Json.Linq;

namespace Senbox.Share.Models
{
    [Serializable]
    public class Data
    {
        public DateTime At { get; set; }

        public double Value { get; set; }

        public static explicit operator Data(JObject v)
        {
            throw new NotImplementedException();
        }
    }
}
