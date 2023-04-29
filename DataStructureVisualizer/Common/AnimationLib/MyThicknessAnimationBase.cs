using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    internal class MyThicknessAnimationBase : ThicknessAnimation, ILinkableAnimation
    {
        public DependencyObject TargetControl { get; set; } = null;
        public object TargetParam { get; set; } = null;
        public string? TargetName { get; set; } = null;

        public MyThicknessAnimationBase(Thickness to, double time, Thickness? from = null, Action? before = null, Action? after = null)
        {
            if (from != null) { From = from; }
            To = to;
            Duration = new Duration(TimeSpan.FromMilliseconds(time));
            this.SetActions(before, after);
        }
    }
}
