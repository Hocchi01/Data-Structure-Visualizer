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
        public const string VerticallyMoveParam = "(UIElement.RenderTransform).(TranslateTransform.Y)";

        public const string VerticallyScaleParam = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
        public const string HorizontallyScaleParam = "(UIElement.RenderTransform).(ScaleTransform.ScaleX)";

        //public const string VerticallyMoveParam = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)";
        //public const string HorizontallyMoveParam = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)";
        //public const string VerticallyScaleParam = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)";

        //"(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)"



        public const double StepLen = 52.0;
        public const double ArrayStart = 8.0;
        
    }
}
