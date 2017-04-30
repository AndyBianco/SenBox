using Android.App;
using Android.Widget;
using Android.OS;
using System;
using att.iot.client;
using System.Text;

namespace App2
{
    [Activity(Label = "Test_IoT_MIC", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public string GetAirState(int ppm)
        {
            if (ppm < 50)
            {
                return "Air frais";
            }
            if (ppm < 200)
            {
                return "Pas de pollution";
            }
            if (ppm < 400)
            {
                return "Basse pollution";
            }
            if (ppm < 600)
            {
                return "Haute pollution";
            }
            return "Très haute pollution, danger !";
        }
        private static void Init()
        {
            //Création d'un objet Logger (qui sert juste pour créer des logs)
            //_logger = new Logger();
            // Création de l'objet qui permet d'accéder aux données stockées chez AllThingsTalk
        }

        private string GetStateFromDevice(SensorService SenBox)
        {
            Data temp = SenBox.TemperatureReceipt();
            Data lux = SenBox.LightReceipt();
            Data sound = SenBox.SoundReceipt();
            Data airPressure = SenBox.AirPressureReceipt();
            Data humidity = SenBox.HumidityReceipt();
            Data airQuality = SenBox.AirQuantityReceipt();
            int airPPM = 25 * Convert.ToInt32(airQuality.Value) / 1023;
            string airState = GetAirState(airPPM);

            string sensorvalues = ($"Bonjour Oussem ! Il fait {Math.Round(temp.Value, 2).ToString()}° dans ton bureau.\n" +
                            $"Luminosité mesurée : {Math.Round(lux.Value, 2).ToString()} lux.\n" +
                            $"Bruit ambiant capté par le micro : {Math.Round(sound.Value, 0)}\n" +
                            $"Pression atmosphérique mesurée : {Math.Round(airPressure.Value, 2).ToString()} hPa\n" +
                            $"Humidité dans l'air : {Math.Round(humidity.Value, 2).ToString()}%\n" +
                            $"Qualité de l'air respiré : {airState} ({Math.Round(airQuality.Value, 2).ToString()})\n\n" +
                            $"Analyse de ces données : \n");

            StringBuilder conseils = new StringBuilder(sensorvalues);
            if (lux.Value < 95.0)
                conseils.Append("La clarté de cette pièce est trop basse !\n");
            else conseils.Append("La luminosité dans la pièce est bonne.");
            if (15.0 > temp.Value)
                conseils.Append("Il fait un peu froid dans la pièce ! \n");
            else if (temp.Value > 25.0)
                conseils.Append("Il fait un peu chaud dans la pièce. \n");
            else conseils.Append("La température est OK.\n");

            if (sound.Value < 130)
                conseils.Append("Il n'y a pas trop de bruit ambiant. \n");
            else conseils.Append("Le niveau sonore est trop élevé pour travailler !\n");
            if (airPPM < 50)
                conseils.Append("Enfin, tu respires de l'air frais");
            else if (airPPM < 200)
                conseils.Append("Enfin, il n'y a pas de pollution dans l'air.");
            else if (airPPM < 400)
                conseils.Append("Enfin, je détecte une faible pollution.");
            else if (airPPM < 600)
                conseils.Append("Enfin, je détecte une haute pollution");
            else conseils.Append("Enfin, je te suggère d'aérer ! l'air est très pollué !!");

            return conseils.ToString();
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Init();
            App2.SensorService.Init();
            App2.SensorService SenBox = App2.SensorService.Instance;
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            TextView message = FindViewById<TextView>(Resource.Id.message);
            Button btn_Refresh = FindViewById<Button>(Resource.Id.btn_refresh);
                btn_Refresh.Click += (object sender, EventArgs e) => message.Text = GetStateFromDevice(SenBox);
                message.Text = GetStateFromDevice(SenBox);
        }

    }
}

