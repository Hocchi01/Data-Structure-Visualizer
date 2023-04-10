using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public interface ILinkableAnimation
    {
        public DependencyObject TargetControl { get; set; }
        public object TargetParam { get; set; }
        public string? TargetName { get; set; }
    }
}
