using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DictaFoule.Mobile.iOS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stripe;
using UIKit;

namespace DictaFoule.Mobile.iOS.API
{
    public static class ApiCall
    {
        #region Constante
        private const string apiDev = "https://api-connect-canin.azurewebsites.net";
        #endregion


        public async static Task<string> SendAudioToCreateProject(SoundFileModel soundFileModel)
        {
            int idProject = 0;

            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/Create");
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(soundFileModel);
                stream.Write(json);
            }

            try
            {
                var response = await request.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var receiveContent = reader.ReadToEnd();
                    idProject = Convert.ToInt32(receiveContent);
                    reader.Close();
                }
                return Convert.ToString(idProject);
            }
            catch (Exception ex)
            {
                return "Erreur " + ex.Message;
            }
        }

        public async static Task<bool> CreateUserToDataBase()
        {
            var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;

            var usermodel = new UserModel 
            { 
                Guid = nsUid.AsString() 
            };

            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/CreateUser");
            request.Method = "POST";
            request.ContentType = "application/json";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(usermodel);
                stream.Write(json);
            }

            try
            {
                var response = await request.GetResponseAsync();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
        }

       public async static Task<bool> GetUserToDataBase()
        {
            var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;
            var guidElements = nsUid.AsString();
            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/GetUser?guidElements=" + guidElements);
            request.Method = "GET";
            try
            {
                var response = await request.GetResponseAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<string> StripeCreateToken(StripeTokenModel stripeTokenModel)
        {
            //StripeConfiguration.SetApiKey("pk_live_QH5KMYyGcXiCMnxVCgafCwvl");
            StripeConfiguration.SetApiKey("pk_test_AaNnNT0FF5mv410J1FbJjyzf");
            try
            {
                var tokenOption = new TokenCreateOptions()
                {
                    Card = new CreditCardOptions()
                    {
                        Number = stripeTokenModel.Number,
                        ExpYear = stripeTokenModel.ExpirationYear,
                        ExpMonth = stripeTokenModel.ExpirationMonth,
                        Cvc = stripeTokenModel.Cvc,
                        Name = stripeTokenModel.Name,
                    },
                };

                var tokenService = new TokenService();
                Token stripeToken = await tokenService.CreateAsync(tokenOption);
                return stripeToken.Id;
            }
            catch (Exception)
            {
                return String.Empty;
            }

        }

        public static int GetIdProject(string name)
        {
            int idproject = 0;
            var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;
            var guidElements = nsUid.AsString();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/GetIdProject?nameFile=" + name + "&guidElements=" + guidElements);
                request.Method = "GET";
                request.ContentType = "application/json";
                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var receiveContent = reader.ReadToEnd();
                    idproject = Convert.ToInt32(receiveContent);
                    reader.Close();
                }
            }
            catch (Exception)
            {
                return 0;
            }
            return idproject;
         }

        public async static Task<bool> SendEmail(SendEmailModel sendEmailModel)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/SendEmail");
                request.Method = "POST";
                request.ContentType = "application/json";
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(sendEmailModel);
                    stream.Write(json);
                }
                var reponse = await request.GetResponseAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<bool> Payement(PayementModel payementModel)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/Payment");
            var postData = "Token=" + payementModel.Token + "&" + "Amount=" + payementModel.Amount + "&IdProject=" + payementModel.IdProject + "&Name=" + payementModel.Name + "&Email=" + payementModel.Email;
            var data = Encoding.UTF8.GetBytes(postData);
            request.Method = "POST";
            request.ContentLength = data.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.UserAgent = "curl/7.37.0";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                bool result;
                WebResponse response = await request.GetResponseAsync();
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var receiveContent = reader.ReadToEnd();
                    result = Convert.ToBoolean(receiveContent);
                    reader.Close();
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static int GetStateProject(int idProject, string guidElements)
        {
            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/GetStateProject?id_project=" + idProject + "&guidElements=" + guidElements);
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                var state = 0;
                var response = request.GetResponse();
                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var receiveContent = reader.ReadToEnd();
                    state = Convert.ToInt32(receiveContent);
                    reader.Close();
                }
                return state;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static string GetTranscriptProject(int idProject, string guidElements)
        {
            var transcription = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create("http://ff-prod-api-dictafoule.azurewebsites.net/v1/Project/GetTranscrib?id_project=" + idProject + "&guidElements=" + guidElements);
            request.Method = "GET";
            request.ContentType = "application/json";
            try
            {
                var response = request.GetResponse();

                using (var stream = response.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var receiveContent = reader.ReadToEnd();
                    transcription = receiveContent;
                    reader.Close();
                }
                return transcription;
            }
            catch (Exception)
            {
                return transcription; 
            }
        }
    }
}
