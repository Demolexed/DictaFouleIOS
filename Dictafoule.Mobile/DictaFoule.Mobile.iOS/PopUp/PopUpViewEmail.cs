using System;
using System.IO;
using System.Net;
using CoreGraphics;
using DictaFoule.Mobile.iOS.API;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace DictaFoule.Mobile.iOS
{
    public class PopUpViewEmail : UIView
    {
        public delegate void PopWillCloseHandler();
        public event PopWillCloseHandler PopWillClose;
        public DetailViewController detailViewController;

        private UIVisualEffectView effectView = new UIVisualEffectView(UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark));
        private UIButton btnClose = new UIButton(UIButtonType.System);
        private UIButton btnSave = new UIButton(UIButtonType.System);
        private UILabel TxtSave = new UILabel()
        {
            Text = "Default",
            Font = UIFont.FromName("Helvetica", 14f),
            TextColor = UIColor.FromRGB(0, 0, 45),
        };
        private UILabel TxTMsgError = new UILabel()
        {
            Text = "Le champs est vide.",
            Font = UIFont.FromName("Helvetica", 10f),
            TextColor = UIColor.Red,
        };
        public UITextField SaveFile = new UITextField()
        {
            KeyboardType = UIKeyboardType.EmailAddress,
            AutocorrectionType = UITextAutocorrectionType.No
        };

        public PopUpViewEmail(CGSize size, string heading, string imageActionFile, string imageCancelFile, DetailViewController viewc)
        {
            detailViewController = viewc;
            nfloat lx = (UIScreen.MainScreen.Bounds.Width - size.Width) / 2;
            nfloat ly = (UIScreen.MainScreen.Bounds.Height - size.Height) / 3;
            this.Frame = new CGRect(new CGPoint(lx, ly), size);
            this.Layer.CornerRadius = 10;
            effectView.Alpha = 0;

            this.BackgroundColor = UIColor.FromRGB(242, 241, 234);

            SaveFile.BackgroundColor = UIColor.White;
            SaveFile.Layer.CornerRadius = 10;
            SaveFile.Frame = new CGRect(20, 50, this.Frame.Width - 50, 40);

            TxtSave.Text = heading;

            nfloat btnHeight = 68;
            btnClose.SetBackgroundImage(UIImage.FromFile(imageCancelFile), UIControlState.Normal);
            btnClose.Frame = new CGRect(SaveFile.Frame.X, this.Frame.Height - btnHeight, SaveFile.Frame.Width / 2, SaveFile.Frame.Width / 2 / 3);
            btnClose.TouchUpInside += delegate {
                Close();
            };
            this.AddSubview(btnClose);

            btnSave.SetBackgroundImage(UIImage.FromFile(imageActionFile), UIControlState.Normal);
            btnSave.Frame = new CGRect(SaveFile.Frame.Width / 2 + 25, this.Frame.Height - btnHeight, SaveFile.Frame.Width / 2 - 5, SaveFile.Frame.Width / 2 / 3);
            btnSave.TouchUpInside += async delegate {

                if (SaveFile.Text.Length <= 0)
                {
                    TxTMsgError.Hidden = false;
                    return;
                }

               
                try
                {
                    if (!SaveFile.Text.Contains("@"))
                        TxTMsgError.Text = "Email invalide.";
                    else
                    {
                        var nsUid = UIDevice.CurrentDevice.IdentifierForVendor;
                        var guidElements = nsUid.AsString();
                        var sendEmailModel = new SendEmailModel { GuidElements = guidElements, Email = SaveFile.Text, IdProject = viewc.Item.IdProject };
                       await ApiCall.SendEmail(sendEmailModel);
                        Close();
                    }

                }
                catch (Exception)
                {
                    TxTMsgError.Text = "Une erreur est survenue";
                }

            };
            this.AddSubview(btnSave);

            TxtSave.Frame = new CGRect(20, 4, this.Frame.Width, 60);
            this.AddSubview(TxtSave);

            TxTMsgError.Frame = new CGRect(20, 15, this.Frame.Width, 60);
            TxTMsgError.Hidden = true;
            this.AddSubview(TxTMsgError);


            this.AddSubview(SaveFile);
            PopUp();
        }

        public void PopUp(bool animated = true, Action popAnimationFinish = null)
        {

            UIWindow window = UIApplication.SharedApplication.KeyWindow;
            effectView.Frame = window.Bounds;
            window.EndEditing(true);
            window.AddSubview(effectView);
            window.AddSubview(this);

            if (animated)
            {
                Transform = CGAffineTransform.MakeScale(0.1f, 0.1f);
                UIView.Animate(0.15, delegate {
                    Transform = CGAffineTransform.MakeScale(1, 1);
                    effectView.Alpha = 0.8f;
                }, delegate {
                    if (null != popAnimationFinish)
                        popAnimationFinish();
                });
            }
            else
            {
                effectView.Alpha = 0.8f;
            }
        }

        public void Close(bool animated = true)
        {
            if (animated)
            {
                UIView.Animate(0.15, delegate {
                    Transform = CGAffineTransform.MakeScale(0.1f, 0.1f);
                    effectView.Alpha = 0;
                }, delegate {
                    this.RemoveFromSuperview();
                    effectView.RemoveFromSuperview();
                    PopWillClose?.Invoke();
                });
            }
            else
            {
                PopWillClose?.Invoke();
            }
        }
    }
}
