using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senbox.Share.Models;
using Senbox.Share.Services;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace SendboxBot.Dialogs
{
    
    [Serializable]
    public class SenBoxDialog : LuisDialog<object>
    {
        private static Activity _message;

        public SenBoxDialog(params ILuisService[] services): base(services)
        {

        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            string message = "Désolé je n'ai pas compris :/";
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Hello")]
        public async Task Hello(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var temp = sensor.TemperatureReceipt();

            await context.PostAsync($"Bonjour, il fait actuelement " + temp.Value + " degrés dans la pièce.");
        }

        [LuisIntent("Light")]
        public async Task Light (IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.LightReceipt();
            await context.PostAsync("La puissance de la lumière est de " + data.Value + " lux.");
        }

        [LuisIntent("Sound")]
        public async Task Sound(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.SoundReceipt();

            if (data.Value < 130)
                await context.PostAsync("Il n'y a pas trop de bruit ambiant.");
            else
                await context.PostAsync("Le niveau sonore est trop élevé pour travailler !");
        }

        [LuisIntent("CoTwo")]
        public async Task CoTwo(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.HumidityReceipt();
            await context.PostAsync("La qualité de l'air " + GetAirState(data.Value));
        }

        [LuisIntent("Temperature")]
        public async Task Temperature(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.TemperatureReceipt();

            await context.PostAsync($"Il fait actuelement " + data.Value + " degrés dans la pièce.");
        }

        [LuisIntent("Pressure")]
        public async Task Pressure(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.AirPressureReceipt();
            await context.PostAsync($"La pression de l'air est de " + data.Value + " hPa");
        }

        [LuisIntent("Humidity")]
        public async Task Humidity(IDialogContext context, LuisResult result)
        {
            var sensor = SensorService.Instance;
            var data = sensor.HumidityReceipt();
            await context.PostAsync("L'humidité est de " + data.Value + " %");
        }

        private string GetAirState(double ppm)
        {
            if (ppm < 50)
            {
                return "est excellente :D";
            }
            if (ppm < 200)
            {
                return "est correcte :)";
            }
            if (ppm < 400)
            {
                return "n'est pas pollulé :|";
            }
            if (ppm < 600)
            {
                return "est mauvaise :(";
            }
            return "est médiocre :'(";
        }
    }
}