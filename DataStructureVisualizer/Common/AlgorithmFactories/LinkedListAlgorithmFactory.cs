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
using System.Windows.Media.Animation;
using static System.Net.Mime.MediaTypeNames;
using DataStructureVisualizer.Views.Components;
using System.Net;

namespace DataStructureVisualizer.Common.AlgorithmFactories
{
    class LinkedListAlgorithmFactory : AlgorithmFactoryBase
    {
        public ObservableCollection<int> Values { get; set; }

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
            brush.GradientStops.Add(new GradientStop(theme.SecondaryDark.Color, 1.0));
        }

        protected void SetInBrush(ref LinearGradientBrush brush)
        {
            var theme = ThemeHelper.GetTheme();

            brush.StartPoint = new Point(0, 0);
            brush.EndPoint = new Point(0, 0);
            brush.GradientStops.Clear();
            brush.GradientStops.Add(new GradientStop(theme.SecondaryDark.Color, 0.0));
            brush.GradientStops.Add(new GradientStop(theme.SecondaryDark.Color, 1.0));
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
            var preArrowIn = new MyPointAnimationBase(from: new Point(0,0), to: new Point(1,0), time: 500, before: () => { SetInBrush(ref preItem.next.brush); }) { AccelerationRatio = 0.5, TargetControl = preItem.next.main, TargetParam = FillBrushEndPoint };

            var lengthenAnims = new List<AnimationTimeline> { preArrowIn };
            lengthenAnims.AddRange(preItem.next.LengthenOrShorten(35 * 2 + 50));

            MainStoryboard.AddAsyncAnimations(lengthenAnims);

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

            var newToNextAnims = newItem.next.NewItemNextPointerTowardNextItem();
            MainStoryboard.AddAsyncAnimations(newToNextAnims[0], () => { newItem.Visibility = Visibility.Visible; });
            for (int i = 1; i < newToNextAnims.Count; i++)
            {
                MainStoryboard.AddAsyncAnimations(newToNextAnims[i]);
            }


            //var virtualPointerViewModel = new LinkedListItemViewModel()
            //{
            //    EditType = DataItemEditType.None,
            //};
            //var virtualPointer = new LinkedListItemUserControl(35 * 2 + 50) // 
            //{
            //    Margin = new Thickness(8),
            //    VerticalAlignment = VerticalAlignment.Center,
            //    RenderTransform = new TranslateTransform() { X = toIndex * (50 + 35) },
            //    Visibility = Visibility.Hidden,
            //    DataContext = virtualPointerViewModel,
            //};

            //Canvas.Children.Add(virtualPointer);


            Point curEndPoint = preItem.next.endPoint;
            var preToNewAnims = preItem.next.PreItemNextPointerTowardNewItem();
            
            MainStoryboard.AddAsyncAnimations(preToNewAnims[0], () =>
            {
                preItem.next.FixCurrentWidth(curEndPoint);

                //preItem.next.Visibility = Visibility.Hidden;

                //virtualPointer.Visibility = Visibility.Visible;
                //virtualPointer.border.Visibility = Visibility.Hidden;
                //virtualPointer.bottom.Visibility = Visibility.Hidden;
            });
            for (int i = 1; i < preToNewAnims.Count; i++)
            {
                MainStoryboard.AddAsyncAnimations(preToNewAnims[i]);
            }


            var preArrowMoveUpAnim = new SimulatedDoubleAnimation(by: -80f, time: 500) { TargetControl = preItem.next.arrow, TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.Y)", TargetName = "arrow_" + Comm.GetUniqueString() };
            var preLineMoveUpAnim = new SimulatedDoubleAnimation(by: -80f, time: 500) { TargetControl = preItem.next.Lines[2], TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.Y)", TargetName = "line_" + Comm.GetUniqueString() };
            var newItemMoveUpAnim = new SimulatedDoubleAnimation(by: -80f, time: 500) { TargetControl = newItem, TargetParam = AnimationHelper.VerticallyMoveParam };
            var newLineMoveDownAnim = new SimulatedDoubleAnimation(by: 80f, time: 500) { TargetControl = newItem.next.Lines[2], TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.Y)", TargetName = "line_" + Comm.GetUniqueString() };
            var newArrowMoveDownAnim = new SimulatedDoubleAnimation(by: 80f, time: 500) { TargetControl = newItem.next.arrow, TargetParam = "(Geometry.Transform).(TransformGroup.Children)[1].(TranslateTransform.Y)", TargetName = "arrow_" + Comm.GetUniqueString() };

            var newLineShortenAnim = newItem.next.LengthenOrShortenLine(newItem.next.Lines[1], 0);
            var preLineShortenAnim = preItem.next.LengthenOrShortenLine(preItem.next.Lines[1], 0);
            var returnNewAnims = new List<AnimationTimeline>
            {
                preArrowMoveUpAnim,
                preLineMoveUpAnim,
                newItemMoveUpAnim,
                newLineShortenAnim,
                preLineShortenAnim,
                newLineMoveDownAnim,
                newArrowMoveDownAnim
            };
            MainStoryboard.AddAsyncAnimations(returnNewAnims);
            dataOperations.Add(() => 
            {
                Values.Insert(toIndex, value);
                Canvas.Children.Remove(newItem);
            });
        }
    }
}
