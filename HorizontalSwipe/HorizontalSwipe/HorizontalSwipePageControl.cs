using System;
using System.Linq;
using CoreGraphics;
using UIKit;

namespace HorizontalSwipe
{
    public class HorizontalSwipePageControl : UIPageControl
    {

        public HorizontalSwipePageControl()
        {
            ActiveImagePurple = UIImage.FromBundle("images/onboarding/purple-pip-full.png");    
            ActiveImageWhite = UIImage.FromBundle("images/onboarding/white-pip-full.png");    
            InactiveImagePurple = UIImage.FromBundle("images/onboarding/purple-pip-circle.png");    
            InactiveImageWhite = UIImage.FromBundle("images/onboarding/white-pip-circle.png");
        }

        public UIImage ActiveImagePurple { get; set; }
        public UIImage InactiveImagePurple { get; set; }
        public UIImage ActiveImageWhite { get; set; }
        public UIImage InactiveImageWhite { get; set; }

        private void UpdateDots()
        {
            for (int index = 0; index < Subviews.Length; index++)
            {
                var view = Subviews[index];
                var dot = view.Subviews.OfType<UIImageView>().Select(subview => subview).FirstOrDefault();
                
                if (dot == null)
                {
                    dot = new UIImageView(new CGRect(0, 0, view.Frame.Width , view.Frame.Height));
                    view.AddSubview(dot);
                }

                dot.Image = index == CurrentPage
                    ? CurrentPage == 0 ? ActiveImagePurple : ActiveImageWhite
                    : CurrentPage == 0 ? InactiveImagePurple : InactiveImageWhite;

            }
        }

        public override nint CurrentPage
        {
            get { return base.CurrentPage; }
            set { base.CurrentPage = value; UpdateDots(); } 
        }
    }
}