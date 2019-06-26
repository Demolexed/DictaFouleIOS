// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace DictaFoule.Mobile.iOS
{
    [Register ("PayementController")]
    partial class PayementController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField cardExpMonth { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField cardExpYear { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField CVCTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIVisualEffectView EffectViewBlur { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EmailTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MgsError { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NameTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NbCbTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton PayBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel PriceLbl { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeTranscriptionTxt { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView WaitPayement { get; set; }

        [Action ("CancelBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancelBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("PayBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void PayBtn_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (CancelBtn != null) {
                CancelBtn.Dispose ();
                CancelBtn = null;
            }

            if (cardExpMonth != null) {
                cardExpMonth.Dispose ();
                cardExpMonth = null;
            }

            if (cardExpYear != null) {
                cardExpYear.Dispose ();
                cardExpYear = null;
            }

            if (CVCTxt != null) {
                CVCTxt.Dispose ();
                CVCTxt = null;
            }

            if (EffectViewBlur != null) {
                EffectViewBlur.Dispose ();
                EffectViewBlur = null;
            }

            if (EmailTxt != null) {
                EmailTxt.Dispose ();
                EmailTxt = null;
            }

            if (MgsError != null) {
                MgsError.Dispose ();
                MgsError = null;
            }

            if (NameTxt != null) {
                NameTxt.Dispose ();
                NameTxt = null;
            }

            if (NbCbTxt != null) {
                NbCbTxt.Dispose ();
                NbCbTxt = null;
            }

            if (PayBtn != null) {
                PayBtn.Dispose ();
                PayBtn = null;
            }

            if (PriceLbl != null) {
                PriceLbl.Dispose ();
                PriceLbl = null;
            }

            if (TimeTranscriptionTxt != null) {
                TimeTranscriptionTxt.Dispose ();
                TimeTranscriptionTxt = null;
            }

            if (WaitPayement != null) {
                WaitPayement.Dispose ();
                WaitPayement = null;
            }
        }
    }
}