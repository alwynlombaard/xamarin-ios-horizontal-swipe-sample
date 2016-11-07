using System;
using Cirrious.FluentLayouts.Touch;
using UIKit;
using HorizontalSwipe.Controls;

namespace HorizontalSwipe.Steps
{
    public class DiscoverStep : UIViewController, IMultiStepProcessStep
    {

        public override void LoadView()
        {
            View = new UIView();
            var image = new UIImageView(UIImage.FromBundle("images/onboarding/discover.png"));
            var headingText = new UILabel
            {
                Text = "Discover",
                TextColor = UIColor.White,
                Font = IOSTheme.ThemeFontLight(36),
                TextAlignment = UITextAlignment.Center
            };

            var descriptionText = new UILabel
            {
                Text = "See stories from the people and\ncharities you care about",
                Lines = 2,
                TextColor = UIColor.White,
                Font = IOSTheme.ThemeFontRegular(16),
                TextAlignment = UITextAlignment.Center
            };

            View.Add(image);
            View.Add(headingText);
            View.Add(descriptionText);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.AddConstraints(
                image.AtTopOf(View),
                image.AtBottomOf(View),
                image.AtLeftOf(View),
                image.AtRightOf(View),

                headingText.AtTopOf(View, 25),
                headingText.WithSameCenterX(View),

                descriptionText.WithSameCenterX(View),
                descriptionText.AtTopOf(View, UIScreen.MainScreen.Bounds.Height * 0.78f)
            );
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            StepActivated?.Invoke(this, new MultiStepProcessStepEventArgs { Index = StepIndex });
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            StepDeactivated?.Invoke(this, new MultiStepProcessStepEventArgs { Index = StepIndex });
        }

        public int StepIndex { get; set; }
        public event EventHandler<MultiStepProcessStepEventArgs> StepActivated;
        public event EventHandler<MultiStepProcessStepEventArgs> StepDeactivated;
    }

}