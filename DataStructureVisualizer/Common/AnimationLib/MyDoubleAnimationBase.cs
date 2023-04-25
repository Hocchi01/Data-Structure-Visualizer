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
    public class MyDoubleAnimationBase : DoubleAnimation, ILinkableAnimation
    {
        private LogViewModel? log = null;

        public LogViewModel? Log
        {
            get { return log; }
            set
            {
                log = value;
                if (log != null)
                {
                    this.AttachLog(log);
                }
            }
        }

        public DependencyObject TargetControl { get; set; } = null;
        public object TargetParam { get; set; } = null;
        public string? TargetName { get; set; } = null;

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
                this.SetActions(() => { WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(log)); }, null);
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
            this.SetActions(before, after);
        }

        public MyDoubleAnimationBase(float by, double time, double? from)
        {
            if (from != null) { From = from; }
            By = by;
            Duration = new Duration(TimeSpan.FromMilliseconds(time));
        }

        public MyDoubleAnimationBase(float by, double time, double? from, Action? before, Action? after) : this(by, time, from)
        {
            this.SetActions(before, after);
        }
    }
}
