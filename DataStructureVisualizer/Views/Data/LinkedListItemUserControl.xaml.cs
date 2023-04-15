using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Theme;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataStructureVisualizer.Views.Data
{
    /// <summary>
    /// Interaction logic for LinkedListItemUserControl.xaml
    /// </summary>
    public partial class LinkedListItemUserControl : DataItemUserControlBase
    {
        protected const string BorderBrushEndPoint = "BorderBrush.(LinearGradientBrush.EndPoint)";
        protected const string StrokeBrushEndPoint = "Stroke.(LinearGradientBrush.EndPoint)";
        protected const string FillBrushEndPoint = "Fill.(LinearGradientBrush.EndPoint)";

        // public double NextPointerLength { get; set; } = 35;
        public LinkedListItemUserControl()
        {
            InitializeComponent();
            ValueItem = valueItem;
        }
        public LinkedListItemUserControl(double nextPointerLength) : this()
        {
            next.Length = nextPointerLength;
        }

        public void SetOutBrush(ref LinearGradientBrush brush)
        {
            var theme = ThemeHelper.GetTheme();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 0);
            brush.GradientStops.Clear();
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 0.0));
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 1.0));
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 1.0));
        }

        public void SetInBrush(ref LinearGradientBrush brush)
        {
            var theme = ThemeHelper.GetTheme();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 0);
            brush.GradientStops.Clear();
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 0.0));
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 1.0));
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 1.0));
        }

        private MyPointAnimationBase GetBorderAnimation(Action before, bool isEnd)
        {
            var anim =  new MyPointAnimationBase(from: new Point(0, 0), to: new Point(1, 0), time: 500, before: before) { TargetControl = border, TargetParam = BorderBrushEndPoint };

            if (isEnd)
            {
                anim.DecelerationRatio = 0.5;
            }
            else
            {
                anim.AccelerationRatio = 0.5;
            }

            return anim;
        }

        private MyPointAnimationBase GetPointerAnimation(Action before, bool isEnd)
        {
            var anim = new MyPointAnimationBase(from: new Point(0, 0), to: new Point(1, 0), time: 500, before: before) { TargetControl = next.main, TargetParam = FillBrushEndPoint };

            if (isEnd)
            {
                anim.DecelerationRatio = 0.5;
            }
            else
            {
                anim.AccelerationRatio = 0.5;
            }

            return anim;
        }

        public MyPointAnimationBase GetBorderOutAnimation(bool isEnd = false)
        {
            return GetBorderAnimation(() => { SetOutBrush(ref borderBrush); }, isEnd);
        }

        public MyPointAnimationBase GetBorderInAnimation(bool isEnd = false)
        {
            return GetBorderAnimation(() => { SetInBrush(ref borderBrush); }, isEnd);
        }

        public MyPointAnimationBase GetPointerOutAnimation(bool isEnd = false)
        {
            return GetPointerAnimation(() => { SetOutBrush(ref next.brush); }, isEnd);
        }

        public MyPointAnimationBase GetPointerInAnimation(bool isEnd = false)
        {
            return GetPointerAnimation(() => { SetInBrush(ref next.brush); }, isEnd);
        }

        public void OnlyShowPointer()
        {
            Visibility = Visibility.Visible;
            border.Visibility = Visibility.Hidden;
            bottom.Visibility = Visibility.Hidden;
        }
    }
}
