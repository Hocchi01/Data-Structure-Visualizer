using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    internal interface ILinkableAnimation
    {
        public UIElement TargetControl { get; set; }
        public object TargetParam { get; set; }
    }
}
