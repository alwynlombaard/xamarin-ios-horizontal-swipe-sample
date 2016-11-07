using System;
using UIKit;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using HorizontalSwipe.Controls;

namespace HorizontalSwipe.Steps
{
    public class GetStartedStep : UIViewController , IMultiStepProcessStep
    {
        private UIActivityIndicatorView _loginActivityIndicator;
        private UIActivityIndicatorView _signupActivityIndicator;
        private UILabel _termsAndConditionsLabel;
        private UILabel _headingText;
        private UILabel _subHeadingText;
        private UIButton _loginButton;
        private UIButton _signupButton;

        public UIImage BackGroundImageForLogin { get; private set; }


        public override void LoadView()
        {
            View = new UIView();

            var img = UIImage.FromBundle("images/onboarding/get-started.png");
            var image = new UIImageView(img);
            BackGroundImageForLogin = img;

            _headingText = new UILabel
                {
                    Text = "Get started",
                    TextColor = UIColor.White,
                    Font = IOSTheme.ThemeFontLight(36),
                    TextAlignment = UITextAlignment.Center
                };

            _subHeadingText = new UILabel
                {
                    Text = "Log in or sign up",
                    TextColor = UIColor.White,
                    Font = IOSTheme.ThemeFontRegular(18),
                    TextAlignment = UITextAlignment.Center
                };

            

            _loginButton = new UIButton();
            _loginButton.SetTitle("Log in", UIControlState.Normal);
            _loginButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            _loginButton.Font = IOSTheme.ThemeFontBold(18);
            _loginButton.Layer.BorderWidth = 1;
            _loginButton.Layer.BorderColor = UIColor.White.CGColor;
            _loginButton.Layer.CornerRadius = 2;
            _loginButton.AccessibilityLabel = "LoginButton";

            _loginActivityIndicator = new UIActivityIndicatorView { HidesWhenStopped = true };
            _signupActivityIndicator = new UIActivityIndicatorView { HidesWhenStopped = true };

            _signupButton = new UIButton();
            _signupButton.SetTitle("Sign up", UIControlState.Normal);
            _signupButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            _signupButton.Font = IOSTheme.ThemeFontBold(18);
            _signupButton.Layer.BorderWidth = 1;
            _signupButton.Layer.BorderColor = UIColor.White.CGColor;
            _signupButton.Layer.CornerRadius = 2;
            _signupButton.AccessibilityLabel = "signupButton";

            int termsAndConditionsFontSize = 11;
            var termsAndConditionsColour = UIColor.White;
            _termsAndConditionsLabel = new UILabel { TextColor = termsAndConditionsColour, Font = IOSTheme.ThemeFontLight(termsAndConditionsFontSize), Lines = 0, TextAlignment = UITextAlignment.Center };
            var termsAndConditions = new NSMutableAttributedString("By continuing you are agreeing to our\n");
            termsAndConditions.Append(new NSAttributedString(" Terms of Service ", IOSTheme.ThemeFontRegular(termsAndConditionsFontSize), termsAndConditionsColour));
            termsAndConditions.Append(new NSAttributedString("and"));
            termsAndConditions.Append(new NSAttributedString(" Privacy Policy.", IOSTheme.ThemeFontRegular(termsAndConditionsFontSize), termsAndConditionsColour));
            _termsAndConditionsLabel.AttributedText = termsAndConditions;
            _termsAndConditionsLabel.UserInteractionEnabled = true;

            View.Add(image);
            View.Add(_headingText);
            View.Add(_subHeadingText);
            View.Add(_loginButton);
            View.Add(_signupButton);
            View.Add(_termsAndConditionsLabel);
            View.Add(_loginActivityIndicator);
            View.Add(_signupActivityIndicator);

            View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            View.AddConstraints(
                image.AtTopOf(View),
                image.AtBottomOf(View),
                image.AtLeftOf(View),
                image.AtRightOf(View),

                _headingText.AtTopOf(View, 65),   
                _headingText.WithSameCenterX(View),

                _subHeadingText.Below(_headingText, 25),
                _subHeadingText.WithSameCenterX(View),


                _loginButton.WithSameCenterY(View).Plus(60),
                _loginButton.AtLeftOf(View, 24),
                _loginButton.AtRightOf(View, (UIScreen.MainScreen.Bounds.Width / 2.0f) + 10),
                _loginButton.Height().EqualTo(44),

                _loginActivityIndicator.WithSameCenterX(_loginButton),
                _loginActivityIndicator.WithSameCenterY(_loginButton),

                _signupButton.WithSameCenterY(_loginButton),
                _signupButton.AtLeftOf(View, (UIScreen.MainScreen.Bounds.Width / 2.0f) + 10),
                _signupButton.AtRightOf(View, 24),
                _signupButton.WithSameHeight(_loginButton),

                _signupActivityIndicator.WithSameCenterX(_signupButton),
                _signupActivityIndicator.WithSameCenterY(_signupButton),

                _termsAndConditionsLabel.WithSameCenterX(View),
                _termsAndConditionsLabel.AtBottomOf(View, 30)


            );

            View.BringSubviewToFront(_loginActivityIndicator);
            View.BringSubviewToFront(_signupActivityIndicator);

           
            if (NavigationController != null && NavigationController.NavigationBar.TopItem != null)
            {
                NavigationController.NavigationBar.TopItem.Title = @" ";
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

        }

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if(NavigationController != null && NavigationController.NavigationBar != null)
                NavigationController.NavigationBar.Hidden = true;

            _loginButton.SetTitle("Log in", UIControlState.Normal);
            _signupButton.SetTitle("Sign up", UIControlState.Normal);
            _termsAndConditionsLabel.Alpha = 1;
            _headingText.Alpha = 1;
            _subHeadingText.Alpha = 1;

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

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            _loginActivityIndicator?.StopAnimating();

            _signupActivityIndicator?.StopAnimating();

        }

        public int StepIndex { get; set; }
        public event EventHandler<MultiStepProcessStepEventArgs> StepActivated;
        public event EventHandler<MultiStepProcessStepEventArgs> StepDeactivated;

    }
}

