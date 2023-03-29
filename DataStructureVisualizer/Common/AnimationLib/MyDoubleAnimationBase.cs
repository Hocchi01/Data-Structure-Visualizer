using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    internal class MyDoubleAnimationBase : DoubleAnimation
    {
        // public LogViewModel? Log { get; set; } = null;
        //public UIElement TargetControl { get; set; }
        //public object TargetParam { get; set; }
        public MyDoubleAnimationBase() { }

        public MyDoubleAnimationBase(double to, double time, double? from)
        {
            if (from != null) { From = from; }
            To = to;
            Duration = new Duration(TimeSpan.FromMilliseconds(time));
        }

        public MyDoubleAnimationBase(double to, double time, double? from, LogViewModel? log) : this(to, time, from)
        {
            if (log != null)
            {
                SetActions(() => { WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(log)); }, null);
            }
        }

        public MyDoubleAnimationBase(double to, double time, Action? before, Action? after, double? from, LogViewModel? log) : this(to, time, from)
        {
            if (log != null)
            {
                if (before == null)
                {
                    before = () => { WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(log)); };
                }
                else
                {
                    before += () => { WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(log)); };
                }
            }
            SetActions(before, after);
        }

        public void SetActions(Action? before, Action? after)
        {
            CurrentStateInvalidated += (sender, e) =>
            {
                Clock clock = (Clock)sender;
                switch (clock.CurrentState)
                {
                    case ClockState.Active:
                        if (before != null)
                        {
                            before();
                        }
                        break;
                    case ClockState.Filling:
                        if (after != null)
                        {
                            after();
                        }
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
