using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DictaFoule.Mobile.iOS.API;
using DictaFoule.Mobile.iOS.Business;
using Foundation;
using Newtonsoft.Json;

namespace DictaFoule.Mobile.iOS
{
    public class Sound : Base
    {
        #region attribut

        public string Name { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public int IdProject { get; set; }
        public SoundState State { get; set; }
        public NSUrl Pathfile { get; set; }
        public string Transcription { get; set; }
        private HttpClientService ClientService { get; set; }
        public string Error { get; set; }

        #endregion

        public Sound() { }

        public Sound(string heading) : base()
        { 
            Name = heading;
            State = SoundState.New;
            this.ClientService = new HttpClientService();
        }

        public async Task<bool> GetIdProject(string name, string guid)
        {
            try
            {
                var response = await ClientService.GetService<int>("Project/GetIdProject?nameFile=" + name + "&guidElements=" + guid);
                this.IdProject = response;
                return true;
            }
            catch (RequestException ex)
            {
                this.IdProject = 0;
                return false;
            }

        }

        public async Task<bool> GetStateProject(string guid)
        {
            try
            {
                var response = await ClientService.GetService<int>("Project/GetStateProject?id_project=" + IdProject.ToString() + "&guidElements=" + guid);
                this.State = (SoundState)(response);
                return true;
            }
            catch(RequestException ex)
            {
                this.State = SoundState.New;
                return false;
            }
            
        }

        public async Task<bool> SendAudioProject()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var audio = Path.Combine(path, Name);
            var toEncodeAsBytes = System.IO.File.ReadAllBytes(Pathfile.Path);
            var returnValue = Convert.ToBase64String(toEncodeAsBytes);

            var soundFileModel = new SoundFileModel
            {
                File64 = returnValue,
                Name = this.Name,
                Guid = this.Guid
            };

            var query = JsonConvert.SerializeObject(soundFileModel);

            try
            {
                var response = await ClientService.PostService<int>("Project/Create", new StringContent(query, Encoding.Unicode, "application/json"));
                this.IdProject = response;
                return true;
            }
            catch (RequestException ex)
            {
                Error = ex.Reason;
                return false;
            }
        }

        public async Task<bool> GetTranscriptProject(string guid)
        {
            var response = await ClientService.GetService<string>("Project/GetTranscrib?id_project=" + IdProject.ToString() + "&guidElements=" + guid);
            this.Transcription = response;
            return true;
        }

        public bool IsWaiting
        {
            get {
                return this.State == SoundState.Upload || this.State == SoundState.SpeechToText
               || this.State == SoundState.TextToTask;
            }
            
        }


        public bool IsError
        {
            get {
                return this.State == SoundState.ErrorSoundCut || this.State == SoundState.ErrorSpeechToText
               || this.State == SoundState.ErrorTextToTask;
            }

        }

        public void Update()
        {
            this.GetIdProject(this.Name, this.Guid);
            this.GetStateProject(this.Guid);
        }
    }
}