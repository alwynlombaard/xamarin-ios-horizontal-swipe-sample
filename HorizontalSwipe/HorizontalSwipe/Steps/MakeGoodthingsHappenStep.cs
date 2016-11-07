using System;
using Cirrious.FluentLayouts.Touch;
using UIKit;
using HorizontalSwipe.Controls;

namespace HorizontalSwipe.Steps
{
    public class MakeGoodthingsHappenStep : UIViewController, IMultiStepProcessStep
    {

        public override void LoadView()
        {
            View = new UIView();
            var image = new UIImageView(UIImage.FromBundle("images/onboarding/make-good-things-happen.png"));
            var headingLabel = new UILabel
            {
                Text = "Make good\nthings happen",
                TextColor = IOSTheme.JGLightPink,
                Font = IOSTheme.ThemeFontLight(36),
                Lines = 2,
                TextAlignment = UITextAlignment.Center
            };

            View.Add(image);
            View.Add(headingLabel);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
            View.AddConstraints(
                image.AtTopOf(View),
                image.AtBottomOf(View),
                image.AtLeftOf(View),
                image.AtRightOf(View),

                headingLabel.AtTopOf(View, UIScreen.MainScreen.Bounds.Height * 0.20f),
                headingLabel.WithSameCenterX(View)
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