using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using HorizontalSwipe.Controls;
using HorizontalSwipe.Steps;
using UIKit;

namespace HorizontalSwipe
{
	public partial class ViewController : UIViewController
	{
		enum SwipeDirection
		{
			Forward,
			Backward
		}

		private MultiStepProcessHorizontal _pageViewController;
		private HorizontalSwipePageControl _pageControl;
		private UIButton _skipButton;
		private UIButton _getStartedButton;
		private List<IMultiStepProcessStep> _steps;
		private SwipeDirection _swipeDirection;
		private CGPoint _getStartedButtonPos;


		public List<IMultiStepProcessStep> Steps => _steps ?? (_steps = GetSteps());


		public ViewController()
		{
		}

		protected ViewController(IntPtr handle) : base(handle)
		{
			_swipeDirection = SwipeDirection.Forward;
		}

		public override void LoadView()
		{
			View = new UIView();

			_pageViewController = new MultiStepProcessHorizontal(new MultiStepProcessDataSource(Steps));
			_pageViewController.WillTransition += _multiStepProcessHorizontal_WillTransition;

			_pageControl = new HorizontalSwipePageControl
			{
				CurrentPage = 0,
				Pages = Steps.Count - 1
			};
			_skipButton = new UIButton();
			_skipButton.SetTitle("Skip", UIControlState.Normal);
			_skipButton.SetTitleColor(IOSTheme.JGLightPink, UIControlState.Normal);
			_skipButton.Font = IOSTheme.ThemeFontRegular(14);

			_skipButton.TouchUpInside += SkipTapped;


			_getStartedButton = new UIButton();
			_getStartedButton.SetTitle("Get started", UIControlState.Normal);
			_getStartedButton.SetTitleColor(UIColor.White, UIControlState.Normal);
			_getStartedButton.Font = IOSTheme.ThemeFontBold(16);
			_getStartedButton.Layer.BorderWidth = 1;
			_getStartedButton.Layer.BorderColor = UIColor.White.CGColor;
			_getStartedButton.Layer.CornerRadius = 2;
			_getStartedButton.AccessibilityLabel = "GetStartedButton";


			_getStartedButton.TouchUpInside += GetStartedTapped;

			_pageControl.CurrentPage = 0;

			View.Add(_pageViewController.View);
			View.Add(_pageControl);
			View.Add(_skipButton);
			View.Add(_getStartedButton);

			View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();
			View.AddConstraints(
				_pageViewController.View.AtTopOf(View),
				_pageViewController.View.AtBottomOf(View),
				_pageViewController.View.AtLeftOf(View),
				_pageViewController.View.AtRightOf(View),

				_pageControl.WithSameCenterX(_pageViewController.View),
				_pageControl.AtBottomOf(View, 20),

				_skipButton.AtRightOf(View, 18),
				_skipButton.WithSameCenterY(_pageControl),

				_getStartedButton.Height().EqualTo(44),
				_getStartedButton.WithSameCenterX(View),
				_getStartedButton.AtLeftOf(View, 22),
				_getStartedButton.AtRightOf(View, 22),
				_getStartedButton.AtBottomOf(View, 22)
			);

			View.BringSubviewToFront(_getStartedButton);

		}

		private void _multiStepProcessHorizontal_WillTransition(object sender, UIPageViewControllerTransitionEventArgs e)
		{
			var pendingStep = e.PendingViewControllers.FirstOrDefault() as IMultiStepProcessStep;
			if (pendingStep == null)
			{
				return;
			}

			_swipeDirection = pendingStep.StepIndex > _pageControl.CurrentPage
				? SwipeDirection.Forward
				: SwipeDirection.Backward;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationController.NavigationBar.Hidden = true;
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			_getStartedButtonPos = _getStartedButton.Center;
		}

		public override bool PrefersStatusBarHidden()
		{
			return true;
		}


		private void SkipTapped(object s, EventArgs e)
		{
			ContinueToGetStarted();
		}

		private void GetStartedTapped(object s, EventArgs e)
		{
			ContinueToGetStarted();
		}

		private void ContinueToGetStarted()
		{

			NavigationController.PushViewController(new GetStartedStep(), true);
		}

		private void HandleStepActivated(object sender, MultiStepProcessStepEventArgs args)
		{
			var isLastStep = args.Index + 1 == _pageControl.Pages;
			var isGetStartedStep = args.Index == _pageControl.Pages;
			var isAbortedTransition = args.Index == _pageControl.CurrentPage;

			if (!isGetStartedStep)
			{
				_pageControl.CurrentPage = args.Index;
			}

			_skipButton.Hidden = isLastStep || isGetStartedStep;
			_pageControl.Hidden = isLastStep || isGetStartedStep;
			_skipButton.SetTitleColor(args.Index == 0 ? IOSTheme.JGLightPink : UIColor.White, UIControlState.Normal);

			_getStartedButton.Hidden = !isLastStep;

			if ((isLastStep && !isAbortedTransition) || (isLastStep && _swipeDirection == SwipeDirection.Backward))
			{
				_getStartedButton.Center = new CGPoint(_getStartedButtonPos.X, _getStartedButtonPos.Y + 66);
				UIView.Animate(
					0.3f,
					0.0f,
					UIViewAnimationOptions.CurveEaseInOut,
					() => _getStartedButton.Center = new CGPoint(_getStartedButtonPos.X, _getStartedButtonPos.Y),
					() => { }
				);
			}
			else
			{
				_getStartedButton.Center = _getStartedButtonPos;
			}

			_pageControl.Alpha = 1.0f;
			_skipButton.Alpha = 1.0f;
		}

		private void HandleStepDeactivated(object sender, MultiStepProcessStepEventArgs args)
		{
			var isCurrentActiveStep = args.Index == _pageControl.CurrentPage;
			var isTransitioningFromFirstStep = args.Index == 0;
			var isTransitionFromSecondStepBackwards = args.Index == 1 && _swipeDirection == SwipeDirection.Backward;
			var isTransitioningBetweenFirstTwoSteps = isTransitioningFromFirstStep || isTransitionFromSecondStepBackwards;
			var isTransitionigFromLastStep = args.Index + 1 == _pageControl.Pages;

			if (isTransitionigFromLastStep)
			{
				_getStartedButton.Hidden = true;
			}

			if (isCurrentActiveStep && isTransitioningBetweenFirstTwoSteps)
			{
				_pageControl.Alpha = 0.0f;
				_skipButton.Alpha = 0.0f;
			}
		}

		private List<IMultiStepProcessStep> GetSteps()
		{
			var steps = new List<IMultiStepProcessStep>
				{
					new MakeGoodthingsHappenStep(),
					new FundraiseStep(),
					new ConnectStep(),
					new DiscoverStep(),
					new GetStartedStep()
				};

			steps.ForEach(s =>
			{
				s.StepActivated += HandleStepActivated;
				s.StepDeactivated += HandleStepDeactivated;
			});

			return steps;
		}

		protected override void Dispose(bool disposing)
		{
			if (_steps == null)
			{
				return;
			}
			foreach (var s in _steps)
			{
				s.Dispose();
			}
			_steps = null;

			base.Dispose(disposing);
		}
	}
}
