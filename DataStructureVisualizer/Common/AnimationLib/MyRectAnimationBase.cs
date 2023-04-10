using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public class MyRectAnimationBase : RectAnimation, ILinkableAnimation
    {
        public DependencyObject TargetControl { get; set; } = null;
        public object TargetParam { get; set; } = null;
        public string? TargetName { get; set; } = null;

        public MyRectAnimationBase(Rect to, double time, Rect? from = null, Action? before = null, Action? after = null)
        {
            if (from != null) { From = from; }
            To = to;
            Duration = new Duration(TimeSpan.FromMilliseconds(time));
            this.SetActions(before, after);
        }
    }
}
