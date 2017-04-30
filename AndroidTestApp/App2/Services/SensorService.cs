using Newtonsoft.Json;
using Senbox.Share.Models;

namespace Senbox.Share.Services
{
    public class SensorService
    {
        private static Device _device;

        private static SensorService _singletonInstance;

        public static SensorService Instance => _singletonInstance;
        private SensorService() { }

        public static readonly int Temperature = 5;
        public static readonly int Light = 6;
        public static readonly int Pressure = 10;
        public static readonly int Humidity = 11;
        public static readonly int Sound = 12;
        public static readonly int AirQuality = 13;

        public static void Init()
        {
            _device = new Device(Settings.Account_User, Settings.Account_Password, null);
            _device.DeviceId = Settings.DeviceId;
            _singletonInstance = new SensorService();
        }

        public Data TemperatureReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(Temperature));
        }

        public Data LightReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(Light));
        }

        public Data AirPressureReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(Pressure));
        }

        public Data HumidityReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(Humidity));
        }

        public Data SoundReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(Sound));
        }

        public Data AirQuantityReceipt()
        {
            return JsonConvert.DeserializeObject<Data>(_device.GetValue(AirQuality));
        }
    }
}
