using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace DictaFoule.Mobile.iOS
{
    class CellsCustoms : UITableViewCell
    {
        UILabel headingLabel, subheadingDateLabel, subheadingTimeLabel;
        UIImageView imageView;
        public CellsCustoms (NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {
            
            imageView = new UIImageView();
            headingLabel = new UILabel()
            {
                Font = UIFont.FromName("Helvetica-bold", 15f),
                TextColor = UIColor.FromRGB(0, 0, 0),
                BackgroundColor = UIColor.Clear
            };
            subheadingDateLabel = new UILabel()
            {
                Font = UIFont.FromName("Helvetica", 11f),
                TextColor = UIColor.FromRGB(120, 101, 246),
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };
            subheadingTimeLabel = new UILabel()
            {
                Font = UIFont.FromName("Helvetica", 11f),
                TextColor = UIColor.FromRGB(120, 101, 246),
                TextAlignment = UITextAlignment.Center,
                BackgroundColor = UIColor.Clear
            };
            ContentView.AddSubviews(new UIView[] { headingLabel, subheadingDateLabel, subheadingTimeLabel, imageView });
        }

        public void UpdateCell(string caption, string date, string time, UIImage image)
        {
            imageView.Image = image;
            headingLabel.Text = caption;
            subheadingDateLabel.Text = date;
            subheadingTimeLabel.Text = time;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            imageView.Frame = new CGRect(ContentView.Bounds.Width - 55, 5, 25, 35);
            headingLabel.Frame = new CGRect(15, 4, ContentView.Bounds.Width - 63, 25);
            subheadingDateLabel.Frame = new CGRect(0, 25, 100, 20);
            subheadingTimeLabel.Frame = new CGRect(40, 25, 100, 20);
        }
    }
}