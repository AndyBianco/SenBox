using System;
using Quartz;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using System.Configuration;
using Senbox.Share.Services;

namespace SendboxBot.Models {
    public class ConstantCheckJob : IJob {
        public async void Execute(IJobExecutionContext context) {
            string res = "";
            var sensor = SensorService.Instance;
            var temperature = sensor.TemperatureReceipt().Value;
            var airQuality = sensor.AirQuantityReceipt().Value;
            var light = sensor.LightReceipt().Value;
            var noise = sensor.SoundReceipt().Value;

            #region Temperature
            if (temperature < 18.0)
            {
                res += "La temperature de la pièce est trop froide. ";
            }
            else if (temperature > 24.5)
            {
                res += "La temperature de la pièce est trop chaude. ";
            }
            #endregion

            #region CO2
            if (airQuality > 600)
            {
                res += "La taux de CO2 de la pièce est critique, ouvrez une fenêtre. ";
            }
            else if (airQuality > 400)
            {
                res += "La taux de CO2 de la pièce est trop élevé. ";
            }
            #endregion

            #region Light
            if (light < 95)
            {
                res += "La luminosité de la pièce est trop faible, allumez les lumières. ";
            }
            else if (light > 250)
            {
                res += "La luminosité de la pièce est trop élevée, cela peut nuir à la santé de vos yeux. ";
            }
            #endregion

            #region Noise
            if (noise > 190)
            {
                res += "La bruit ambient dans la pièce est dangereux pour votre santé, diminuez les sources de bruit. ";
            }
            else if (noise > 140)
            {
                res += "La bruit ambiant dans la pièce est nuisible pour la concentration. ";
            }
            #endregion

            if (res != "")
            {
                res = "@channel ATTENTION : " + res;
                await SendProactiveMessage(res);
            }
        }

        private static async Task SendProactiveMessage(string message)
        {
            const string slackConnector = "https://slack.botframework.com";
            MicrosoftAppCredentials.TrustServiceUrl(slackConnector);
            var recipient = new ChannelAccount(""); //ID du channel / Personne
            var account = new ChannelAccount(""); //ID BOT
            var connector = new ConnectorClient(new Uri(slackConnector), ConfigurationManager.AppSettings["MicrosoftAppId"], ConfigurationManager.AppSettings["MicrosoftAppPassword"]);

            var msg = Activity.CreateMessageActivity();
            msg.Type = ActivityTypes.Message;
            msg.From = account;
            msg.Recipient = recipient;
            msg.ChannelId = "slack";
            var conversation = ""; //ID de la conversation
            msg.Conversation = new ConversationAccount(id: conversation);
            msg.Text = message;
            await connector.Conversations.SendToConversationAsync((Activity)msg);
        }
    }
}
