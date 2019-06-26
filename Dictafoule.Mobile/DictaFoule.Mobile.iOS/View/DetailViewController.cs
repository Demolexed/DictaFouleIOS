using Foundation;
using System;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using AVFoundation;
using System.Timers;
using System.Net;
using System.IO;
using DictaFoule.Mobile.iOS.API;
using System.Threading.Tasks;
using DictaFoule.Mobile.iOS.Business;

namespace DictaFoule.Mobile.iOS
{
    public partial class DetailViewController : UIViewController
    {

        public int index;
        public Sound Item { get; set; }
        public User User { get; set; }

        public TimeSpan time = new TimeSpan();
        Timer playTimer = new Timer();

        public TimeSpan spentTime = new TimeSpan();
        Timer timer = new Timer();


        GameAudioManager player;

        public DetailViewController()
        {
        }

        public DetailViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CGColor[] colorsOrange = { new UIColor(red: 0.97f, green: 0.56f, blue: 0.37f, alpha: 1.0f).CGColor,
                new UIColor(red: 0.95f, green: 0.74f, blue: 0.43f, alpha: 1.0f).CGColor};
            CAGradientLayer gradientLayerOrange = new CAGradientLayer
            {
                Colors = colorsOrange,
                Transform = CATransform3D.MakeRotation((System.nfloat)(-Math.PI / 2), 0, 0, 1),
                Frame = new CGRect(0, 0, View.Bounds.Height, TranscriptionTxt.Frame.Y)
            };
            this.View.Layer.InsertSublayer(gradientLayerOrange, 0);

            PlayerBtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);

            time = TimeSpan.Zero;
            playTimer.Interval = 1000;
            playTimer.Elapsed += UpdateSliderTime;

            spentTime = TimeSpan.Zero;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;

            ErrorTxt.Hidden = true;
            ActivityIndicartorTranscrib.Hidden = true;
            WaitTranscribTxT.Hidden = true;
            TranscriptionTxt.Hidden = true;
            AskTranscrib.Hidden = true;
            OptionBtn.Hidden = true;
            ExportBtn.Hidden = true;

            Item.Update();

        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            NameRecordTxt.Text = Item.Name;
            if (Item.State == Business.SoundState.New)
                AskTranscrib.Hidden = false;
            if (Item.IsWaiting)
            {
                AskTranscrib.Hidden = true;
                ActivityIndicartorTranscrib.Hidden = false;
                ActivityIndicartorTranscrib.StartAnimating();
                WaitTranscribTxT.Hidden = false;
            }
            if (Item.State == Business.SoundState.ProjectCompleted)
            {
                Item.GetTranscriptProject(UIDevice.CurrentDevice.IdentifierForVendor.AsString());
                TranscriptionTxt.Hidden = false;
                TranscriptionTxt.Text = Item.Transcription;
                OptionBtn.Hidden = false;
                ExportBtn.Hidden = false;
                ActivityIndicartorTranscrib.Hidden = true;
                ActivityIndicartorTranscrib.StopAnimating();
                WaitTranscribTxT.Hidden = true;
            }
            if (Item.IsError)
                ErrorTxt.Hidden = false;
        }

        public void SetItem(Sound item, User user)
        {
            Item = item;
            User = user;
        }

        partial void ExportBtn_TouchUpInside(UIButton sender)
        {
            PopUpViewEmail cpuv = new PopUpViewEmail(new CoreGraphics.CGSize(300, 175), "EMAIL*", "SendButton.png", "CancelButton.png", this);
        }

        partial void PlayerBtn_TouchUpInside(UIButton sender)
        {
            if (player?.IsPlaying() == false && player.IsFinish == false)
            {
                playTimer.Enabled = true;
                timer.Enabled = true;
                playTimer.Start();
                player.Play();
                PlayerBtn.SetBackgroundImage(UIImage.FromBundle("PauseIcon"), UIControlState.Normal);
                return;
            }
            if (player?.IsPlaying() == true)
            {
                playTimer.Enabled = true;
                timer.Enabled = true;
                timer.Stop();
                playTimer.Stop();
                player.Pause();
                PlayerBtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);
                return;
            }

            else
            {
                try
                {
                    player = new GameAudioManager();
                    playTimer.Enabled = true;
                    timer.Enabled = true;
                    playTimer.Start();
                    timer.Start();
                    player.PlaySound(Item.Pathfile);

                    SliderTime.MinValue = 0;
                    SliderTime.MaxValue = (float)(int)player.Duration();
                    PlayerBtn.SetBackgroundImage(UIImage.FromBundle("PauseIcon"), UIControlState.Normal);
                }
                catch (Exception)
                {
                    UIAlertController alert = UIAlertController.Create("Erreur", "Impossible de lire votre fichier", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (action) => { }));
                    PresentViewController(alert, true, null);

                }
            }
        }

        partial void AskTranscrib_TouchUpInside(UIButton sender)
        {
            if(player != null)
                player.Dispose();

            var payementController = this.Storyboard.InstantiateViewController("PayementController") as PayementController;
            this.NavigationController.PushViewController(payementController, true);
            payementController.SetItem(Item, User);
        }

        void UpdateSliderTime(object sender, ElapsedEventArgs e)
        {
            if (player.IsPlaying())
                InvokeOnMainThread(delegate
                {
                    SliderTime.Value += 1;
                });
            else
            {
                playTimer.Enabled = false;
                timer.Enabled = false;
                playTimer.Stop();
                timer.Stop();
                InvokeOnMainThread(delegate
                {
                    SliderTime.Value = SliderTime.MinValue;
                    time = TimeSpan.Zero;
                    TimerTxt.Text = time.ToString("t");
                    PlayerBtn.SetBackgroundImage(UIImage.FromBundle("PlayIcon"), UIControlState.Normal);
                });
            }
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time = time.Add(TimeSpan.FromSeconds(1));
            InvokeOnMainThread(() =>
            {
                TimerTxt.Text = time.ToString("t");

            });
        }

    }
}