using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;

namespace HorizontalSwipe.Controls
{
    public sealed class MultiStepProcessHorizontal : UIPageViewController
    {
        public MultiStepProcessHorizontal(MultiStepProcessDataSource dataSource) :base(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal)
        {
            DataSource = dataSource;
            SetViewControllers(new[] {dataSource.Steps.FirstOrDefault() as UIViewController}, UIPageViewControllerNavigationDirection.Forward, false, null);
        }
    }
    

    public class MultiStepProcessDataSource : UIPageViewControllerDataSource
    {
        private readonly List<IMultiStepProcessStep> _steps;
       
        public MultiStepProcessDataSource(List<IMultiStepProcessStep> steps )
        {
            if (steps == null)
            {
              throw new ArgumentNullException(nameof(steps));
            }
            if (!steps.Any())
            {
                throw new ArgumentException("steps cannot be empty.", nameof(steps));
            }

            if (steps.Any(s => !(s is UIViewController)))
            {
                throw new ArgumentException("all steps must be a UIViewController", nameof(steps));
            }

            _steps = steps;
            
            for (int i = 0; i < _steps.Count; i++)
            {
                var step = _steps[i];
                step.StepIndex = i;
            }
        }

        public List<IMultiStepProcessStep> Steps => _steps;

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController,
            UIViewController referenceViewController)
        {
            var step = referenceViewController as IMultiStepProcessStep;
            if (step == null)
            {
                return null;
            }

            var index = _steps.IndexOf(step);
            if (index <= 0)
            {
                return null;
            }

            return   _steps[index - 1] as UIViewController;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            var step = referenceViewController as IMultiStepProcessStep;
            if (step == null)
            {
                return null;
            }
            var index = _steps.IndexOf(step);
            if (index + 1 == _steps.Count)
            {
                return null;
            }

            return _steps[(step.StepIndex + 1)] as UIViewController;
        }
    }


    public interface IMultiStepProcessStep : IDisposable
    {
        int StepIndex { get; set; }
        event EventHandler<MultiStepProcessStepEventArgs> StepActivated;
        event EventHandler<MultiStepProcessStepEventArgs> StepDeactivated;
    }

    public class MultiStepProcessStepEventArgs
    {
        public int Index { get; set; }
    }
}