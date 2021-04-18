using System;
using System.Net;
using System.Security;
using Microsoft.SharePoint.Client;
using Newtonsoft.Json.Linq;
using PnP.Framework;


namespace console_push_data
{
    class Program
    {


        private const string SHAREPOINT_DOMAIN = "XXXXXXX.sharepoint.com";
        private const string SHAREPOINT_SITE_URL = "https://" + SHAREPOINT_DOMAIN;

        private const string APPLICATION_CLIENT_ID = "XXXXXXXXXXXXXXXX";
        private const string DIRECTORY_TENANT_ID = "XXXXXXXXXXXXXXXX";
        
        private const string CERTIFICATE_PATH = "XXXXXXXXXXXXXXXX.pfx";
        private const string CERTIFICATE_PASSWORD = "XXXXXXXXXXXXXXXX";

        private const string LIST_NAME = "XXXXXXXXXXXXXXXX";


        static void Main(string[] args)
        {
            double currentTemperature = GetCurrentTemperature();
            LogTemperature(currentTemperature);
        }


        private static void LogTemperature(double currentTemperature)
        {

            var authManager = new PnP.Framework.AuthenticationManager(APPLICATION_CLIENT_ID, CERTIFICATE_PATH, CERTIFICATE_PASSWORD, DIRECTORY_TENANT_ID);

            using (var context = authManager.GetContext(SHAREPOINT_SITE_URL))
            {
                Web web = context.Web;
                context.Load(web, w => w.Lists);
                context.ExecuteQuery();

                List list = web.Lists.GetByTitle(LIST_NAME);
                context.ExecuteQuery(); 

                ListItemCreationInformation newItem = new ListItemCreationInformation();
                ListItem oListItem = list.AddItem(newItem);
                oListItem["Title"] = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                oListItem["Temperature"] = currentTemperature.ToString();

                oListItem.Update();
                context.ExecuteQuery(); 
            }
        }


        private static double GetCurrentTemperature()
        {
            JObject currentObservations = GetCurrentObservations();

            string temperature = currentObservations.SelectToken("$.properties.temperature.value").Value<string>();

            double celcius = double.Parse(temperature);
            return celcius.CelciusToFahrenheit();
        }


        private static JObject GetCurrentObservations()
        {
            //https://api.weather.gov/points/41.2381,-85.853
            //https://api.weather.gov/gridpoints/IWX/43,42/stations
            //https://api.weather.gov/stations/KASW
            //https://api.weather.gov/stations/KASW/observations/latest

            const string WARSAW_CURRENT_CONDITIONS_URL = "https://api.weather.gov/stations/KASW/observations/latest";

            var wc = new System.Net.WebClient();
            wc.Headers.Add("User-Agent", "sampleapp.com");

            string json;
            while (true)
            {
                try
                {
                    json  = wc.DownloadString(WARSAW_CURRENT_CONDITIONS_URL);
                    break;
                }
                catch (WebException ex)
                {
                    //they must be using some cdn that fails periodically
                    HttpWebResponse response = (HttpWebResponse)ex.Response;
                    if (response.StatusCode == HttpStatusCode.BadGateway)
                    {
                        throw ex;
                    }
                }
            }
            return JObject.Parse(json);            
        }


    }
}
