using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using DataStructureVisualizer.Common.Theme;
using DataStructureVisualizer.Common.Extensions;
using System.Diagnostics;
using DataStructureVisualizer.Common.Enums;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    class LinkedListAlgorithmFactory : AlgorithmFactoryBase
    {
        protected const string BorderBrushEndPoint = "BorderBrush.(LinearGradientBrush.EndPoint)";
        protected const string StrokeBrushEndPoint = "Stroke.(LinearGradientBrush.EndPoint)";
        protected const string FillBrushEndPoint = "Fill.(LinearGradientBrush.EndPoint)";

        public LinkedListAlgorithmFactory(Grid canvas, ItemsControl container, MyStoryboard myStoryboard, ObservableCollection<LinkedListItemViewModel> dataItems) : base(canvas, container, myStoryboard, dataItems.RevertElems())
        {
        }

        protected void SetOutBrush(ref LinearGradientBrush brush)
        {
            var theme = ThemeHelper.GetTheme();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 0);
            brush.GradientStops.Clear();
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 0.0));
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 1.0));
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 1.0));
        }

        protected void SetInBrush(ref LinearGradientBrush brush)
        {
            var theme = ThemeHelper.GetTheme();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 0);
            brush.GradientStops.Clear();
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 0.0));
            brush.GradientStops.Add(new GradientStop(theme.SecondaryMid.Color, 1.0));
            brush.GradientStops.Add(new GradientStop(theme.PrimaryMid.Color, 1.0));
        }

        protected void InitPointer()
        {
            var head = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, 0);
            SetInBrush(ref head.borderBrush);
            head.borderBrush.EndPoint = new Point(1, 0);
        }

        public void PointerNext(int toIndex, Action? before = null, Action? after = null)
        {
            int preIndex = toIndex - 1;

            var preItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, preIndex + 1); // + 1 算上头结点
            var toItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, toIndex + 1);

            var startPoint = new Point(0, 0);
            var endPoint = new Point(1, 0);

            var preBorderOut = new MyPointAnimationBase(from: startPoint, to: endPoint, time: 500, before: () => { SetOutBrush(ref preItem.borderBrush); }) { AccelerationRatio = 0.5, TargetControl = preItem.border, TargetParam = BorderBrushEndPoint };

            var preArrowIn = new MyPointAnimationBase(from: startPoint, to: endPoint, time: 500, before: () => { SetInBrush(ref preItem.next.brush); }) { AccelerationRatio = 0.5, TargetControl = preItem.next.main, TargetParam = FillBrushEndPoint };

            var preArrowOut = new MyPointAnimationBase(from: startPoint, to: endPoint, time: 500, before: () => { SetOutBrush(ref preItem.next.brush); }) { DecelerationRatio = 0.5, TargetControl = preItem.next.main, TargetParam = FillBrushEndPoint };

            var toBorderIn = new MyPointAnimationBase(from: startPoint, to: endPoint, time: 500, before: () => { SetInBrush(ref toItem.borderBrush); }) { DecelerationRatio = 0.5, TargetControl = toItem.border, TargetParam = BorderBrushEndPoint };


            MainStoryboard.AddAsyncAnimations(new List<MyPointAnimationBase> { preBorderOut, preArrowIn }, before);
            MainStoryboard.AddAsyncAnimations(new List<MyPointAnimationBase> { preArrowOut, toBorderIn }, null, after);
            // MainStoryboard.Delay(300);
        }

        public void FindElem(int elemIndex)
        {
            InitPointer();
            for (int i = 0; i <= elemIndex; i++)
            {
                PointerNext(i);

            }
        }

        public void InsertElem(int toIndex, int value)
        {
            var preItem = Comm.GetItemFromItemsControlByIndex<LinkedListItemUserControl>(Container, toIndex);

            MainStoryboard.AddAsyncAnimations(preItem.next.LengthenOrShorten(35 * 2 + 50));

            var newItemViewModel = new LinkedListItemViewModel()
            {
                Value = value,
                EditType = DataItemEditType.New,
                Color = new SolidColorBrush(new Color() { ScA = 1.0F, R = 255, G = 152, B = 0 })
            };

            var newItem = new LinkedListItemUserControl(25)
            {
                Margin = new Thickness(8),
                VerticalAlignment = VerticalAlignment.Center,
                RenderTransform = new TranslateTransform() { X = toIndex * (50 + 35) + 50 + 35, Y = 80 },
                Visibility = Visibility.Hidden,
                DataContext = newItemViewModel
            };
            Canvas.Children.Add(newItem);

            var animations1 = newItem.next.NewItemNextPointerTowardNext();
            MainStoryboard.AddAsyncAnimations(animations1[0], () => { newItem.Visibility = Visibility.Visible; });
            for (int i = 1; i < animations1.Count; i++)
            {
                MainStoryboard.AddAsyncAnimations(animations1[i]);
            }
        }
    }
}
