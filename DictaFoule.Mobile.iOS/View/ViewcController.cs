using Foundation;
using System;
using UIKit;
using System.Timers;
using CoreGraphics;
using System.Collections.Generic;
using CoreAnimation;
using AVFoundation;
using System.IO;
using System.Diagnostics;
using System.Net;
using DictaFoule.Mobile.iOS.API;
using System.Threading.Tasks;
using DictaFoule.Mobile.iOS.Business;

namespace DictaFoule.Mobile.iOS
{
    public partial class ViewcController : UIViewController
    {
                
        public TimeSpan time = new TimeSpan();
        Timer timer = new Timer();
        Timer playTimer = new Timer();


        AVAudioRecorder recorder;
        GameAudioManager player;
        NSError error;
        public NSUrl pathfile;
        NSDictionary settings;

        public string fileName;

        UITableView TableRecordView;

        public User User { get; private set; }


            public ViewcController(IntPtr handle) : base(handle)
        {
            AudioToolbox.AudioSession.Initialize();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NameFile.Text = "Nouvel Enregistrement";



            Playbtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);
            Playbtn.SetBackgroundImage(UIImage.FromFile("PlayDisableButton.png"), UIControlState.Disabled);
            Playbtn.Enabled = false;


            time = TimeSpan.Zero;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;

            playTimer.Interval = 1000; //one second  
            playTimer.Elapsed += UpdateSliderTime;

            CGColor[] colorsOrange = { new UIColor(red: 0.97f, green: 0.56f, blue: 0.37f, alpha: 1.0f).CGColor,
                new UIColor(red: 0.95f, green: 0.74f, blue: 0.43f, alpha: 1.0f).CGColor};
            CAGradientLayer gradientLayerOrange = new CAGradientLayer
            {
                Colors = colorsOrange,
                Transform = CATransform3D.MakeRotation((System.nfloat)(-Math.PI / 2), 0, 0, 1),
                Frame = new CGRect(0, 0, View.Bounds.Width, View.Bounds.Height * 0.5)
            };
            this.View.Layer.InsertSublayer(gradientLayerOrange, 0);


            CGColor[] colorsBlue = { new UIColor(red: 0.29f, green: 0.26f, blue: 0.96f, alpha: 1.0f).CGColor,
                new UIColor(red: 0.47f, green: 0.40f, blue: 0.96f, alpha: 1.0f).CGColor}; CAGradientLayer gradientLayerBlue = new CAGradientLayer
                {
                    Colors = colorsBlue,
                    Transform = CATransform3D.MakeRotation((System.nfloat)(-Math.PI / 2), 0, 0, 1),
                Frame = new CGRect(0, gradientLayerOrange.Frame.Height, View.Bounds.Width, gradientLayerOrange.Frame.Height * 0.4)
                };
            this.View.Layer.InsertSublayer(gradientLayerBlue, 1);



            TableRecordView = new UITableView(new CGRect(0, gradientLayerBlue.Frame.Y + gradientLayerBlue.Frame.Height, View.Bounds.Width, gradientLayerOrange.Frame.Height * 0.60));

            this.User = new User();
            TableRecordView.Source = new TableSource(User.Sounds, this);
            TableRecordView.ScrollEnabled = true;
            Add(TableRecordView);
          
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            TableRecordView.RowHeight = UITableView.AutomaticDimension;
            TableRecordView.EstimatedRowHeight = 40f;
            TableRecordView.ReloadData();

        }

        bool PrepareAudioRecording()
        {
            var audioSession = AVAudioSession.SharedInstance();
            var err = audioSession.SetCategory(AVAudioSessionCategory.Record);

            if (audioSession.RecordPermission == AVAudioSessionRecordPermission.Denied)
            {
                var erreurAlertController = UIAlertController.Create("Erreur", "Le micro est desactivé", UIAlertControllerStyle.Alert);
                erreurAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(erreurAlertController, true,null);
                return false;
            }
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return false;
            }
            err = audioSession.SetActive(true);
            if (err != null)
            {
                Console.WriteLine("audioSession: {0}", err);
                return false;
            }

            fileName = string.Format("Myfile{0}.wav", DateTime.Now.ToString("yyyyMMddHHmmss"));
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string audioFilePath = Path.Combine(path, fileName);
            pathfile = NSUrl.FromFilename(audioFilePath);

            NSObject[] values = new NSObject[]
            {
                NSNumber.FromFloat(44100.0f),
                NSNumber.FromInt32((int)AudioToolbox.AudioFormatType.LinearPCM),
                NSNumber.FromInt32(1),
                NSNumber.FromInt32((int)AVAudioQuality.High)
            };

            NSObject[] keys = new NSObject[]
            {
                AVAudioSettings.AVSampleRateKey,
                AVAudioSettings.AVFormatIDKey,
                AVAudioSettings.AVNumberOfChannelsKey,
                AVAudioSettings.AVEncoderAudioQualityKey
            };
            settings = NSDictionary.FromObjectsAndKeys(values, keys);

            recorder = AVAudioRecorder.Create(this.pathfile, new AudioSettings(settings), out error);
            if ((recorder == null) || (error != null))
            {
                Console.WriteLine(error);
                return false;
            }

            if (!recorder.PrepareToRecord())
            {
                recorder.Dispose();
                recorder = null;
                return false;
            }
            return true;

        }

        partial void RecordButton_TouchUpInside(UIButton sender)
        {

            if (recorder == null)
            {
                AudioToolbox.AudioSession.Category = AudioToolbox.AudioSessionCategory.PlayAndRecord;

                AudioToolbox.AudioSession.SetActive(true);
                if (!PrepareAudioRecording())
                {
                    return;
                }
            }

            if (recorder.Recording == false)
            {
                RecordButton.SetBackgroundImage(UIImage.FromBundle("EndRecord"), UIControlState.Normal);
                OkBtn.Enabled = false;
                Playbtn.Enabled = false;
                timer.Enabled = true;
                timer.Start();
                recorder.Record();
                UIApplication.SharedApplication.IdleTimerDisabled = true;
            }
            else
            {
                timer.Enabled = false;
                timer.Stop();
                OkBtn.Enabled = true;
                Playbtn.Enabled = true;
                RecordButton.SetBackgroundImage(UIImage.FromFile("Reccord_Off.png"), UIControlState.Normal);
                recorder.Stop();
                UIApplication.SharedApplication.IdleTimerDisabled = false;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time = time.Add(TimeSpan.FromSeconds(1));
            InvokeOnMainThread(() =>
            {
                TimerLbl.Text = time.ToString("t");

            });
        }

        partial void OkBtn_TouchUpInside(UIButton sender)
        {
            if (recorder == null)
                return;
            this.View.BackgroundColor = UIColor.Gray;
            CustomPopUpView cpuv = new CustomPopUpView(new CoreGraphics.CGSize(300, 175), "NOM DU FICHIER*", "SaveButton.png", "EraseFile.png", this);
            recorder.Dispose();
            recorder = null;

        }

        public void Transcribe()
        {
            var payementController = this.Storyboard.InstantiateViewController("PayementController") as PayementController;
            this.NavigationController.PushViewController(payementController, true);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string audioFilePath = Path.Combine(path, fileName);
            pathfile = NSUrl.FromFilename(audioFilePath);

            var sound = new Sound(fileName) 
            { 
                Date = DateTime.Now.ToString("dd/MM") + " - ", 
                Time = time.ToString(@"hh\:mm\:ss"), 
                Pathfile = pathfile
            };
            payementController.SetItem(sound, User);
            User.Sounds.Add(sound);
            Add(TableRecordView);
            InvokeOnMainThread(delegate { TableRecordView.ReloadData(); time = TimeSpan.Zero; TimerLbl.Text = time.ToString("t"); });
        }

        private void SaveFile(string nameFile)
        {
            if (nameFile.Length <= 0)
                nameFile = DateTime.Now.ToString();
            User.Sounds.Add(new Sound(nameFile) { Date = DateTime.Now.ToString("dd/MM")  + " - ", Time = time.ToString(@"hh\:mm\:ss") });
            TableRecordView.Source = new TableSource(User.Sounds, this);
            Add(TableRecordView);
            InvokeOnMainThread(delegate { TableRecordView.ReloadData(); });
        }

        partial void Playbtn_TouchUpInside(UIButton sender)
        {
            if (player?.IsPlaying() == false && player.IsFinish == false)
            {
                playTimer.Enabled = true;
                playTimer.Start();
                player.Play();
                Playbtn.SetBackgroundImage(UIImage.FromBundle("PauseIcon"), UIControlState.Normal);
                return;
            }
            if (player?.IsPlaying() == true)
            {
                playTimer.Enabled = false;
                playTimer.Stop();
                player.Pause();
                Playbtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);
                return;
            }
            try
            {
                player = new GameAudioManager();

                playTimer.Enabled = true;
                playTimer.Start();

                player.PlaySound(pathfile);

                SliderTime.MinValue = 0;
                SliderTime.MaxValue = (float)(int)player.Duration();
                Playbtn.SetBackgroundImage(UIImage.FromBundle("PauseIcon"), UIControlState.Normal);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private void UpdateSliderTime(object sender, ElapsedEventArgs e)
        {
            if (player.IsPlaying())
                InvokeOnMainThread(delegate {
                    SliderTime.Value += 1;
            });
            else
            {
                playTimer.Enabled = false;
                playTimer.Stop();
                InvokeOnMainThread(delegate
                {
                    SliderTime.Value = SliderTime.MinValue;
                    Playbtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);
                });
            }
        }

        public void Reset()
        {
            InvokeOnMainThread(delegate { time = TimeSpan.Zero; TimerLbl.Text = time.ToString("t"); });
        }

    }
}