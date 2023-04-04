using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.AnimationLib
{
    internal static class AnimationHelper
    {
        public const string HorizontallyMoveParam = "(UIElement.RenderTransform).(TranslateTransform.X)";

        public const string VerticallyScaleParam = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";



        public const double StepLen = 52.0;
        public const double ArrayStart = 8.0;
        
    }
}
