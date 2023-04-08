using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    class MyPointAnimationBase : PointAnimation, ILinkableAnimation
    {
        public DependencyObject TargetControl { get; set; } = null;
        public object TargetParam { get; set; } = null;

        public MyPointAnimationBase(Point to, double time, Point? from = null, Action? before = null, Action? after = null)
        {
            if (from != null) { From = from; }
            To = to;
            Duration = new Duration(TimeSpan.FromMilliseconds(time));
            this.SetActions(before, after);
        }

        
    }
}
