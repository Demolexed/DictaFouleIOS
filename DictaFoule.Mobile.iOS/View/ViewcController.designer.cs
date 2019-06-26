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
    [Register ("ViewcController")]
    partial class ViewcController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameFile { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkBtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton Playbtn { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RecordButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISlider SliderTime { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimerLbl { get; set; }

        [Action ("OkBtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void OkBtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("Playbtn_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void Playbtn_TouchUpInside (UIKit.UIButton sender);

        [Action ("RecordButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void RecordButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (NameFile != null) {
                NameFile.Dispose ();
                NameFile = null;
            }

            if (OkBtn != null) {
                OkBtn.Dispose ();
                OkBtn = null;
            }

            if (Playbtn != null) {
                Playbtn.Dispose ();
                Playbtn = null;
            }

            if (RecordButton != null) {
                RecordButton.Dispose ();
                RecordButton = null;
            }

            if (SliderTime != null) {
                SliderTime.Dispose ();
                SliderTime = null;
            }

            if (TimerLbl != null) {
                TimerLbl.Dispose ();
                TimerLbl = null;
            }
        }
    }
}