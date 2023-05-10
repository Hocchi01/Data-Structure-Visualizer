using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataStructureVisualizer.Views.Data
{
    /// <summary>
    /// Interaction logic for BinaryTreeItemUserControl.xaml
    /// </summary>
    public partial class BinaryTreeItemUserControl : DataItemUserControlBase
    {
        private const double layerHeight = 125;
        private const double radius = 25;
        private static double baseDz = Math.Sqrt(Math.Pow(radius * 2, 2) + Math.Pow(layerHeight, 2));
        public double BaseOffsetX = radius * radius * 2 / baseDz;
        public double BaseOffsetY = radius * layerHeight / baseDz;

        public BinaryTreeItemUserControl()
        {
            InitializeComponent();
            ValueItem = valueItem;
        }

        public List<List<AnimationTimeline>> GetInsertToLeftAnimations(int value)
        {
            leftChildValue.Text = value.ToString();

            var leftXAnim = new SimulatedDoubleAnimation(by: -(float)(radius * 2 - BaseOffsetX), time: 500)
            {
                TargetControl = left,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var leftYAnim = new SimulatedDoubleAnimation(by: (float)(layerHeight - BaseOffsetY), time: 500)
            {
                TargetControl = left,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var leftXAnim2 = new SimulatedDoubleAnimation(by: (float)(BaseOffsetX), time: 500)
            {
                TargetControl = left,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var leftYAnim2 = new SimulatedDoubleAnimation(by: (float)(-BaseOffsetY), time: 500)
            {
                TargetControl = left,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var leftChildScaleXAnim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = leftChild,
                TargetParam = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)",
            };
            var leftChildScaleYAnim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = leftChild,
                TargetParam = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)",
            };

            return new List<List<AnimationTimeline>>
            {
                new List<AnimationTimeline> { leftXAnim, leftYAnim },
                new List<AnimationTimeline> { leftChildScaleXAnim, leftChildScaleYAnim, leftXAnim2, leftYAnim2 }
            };
        }

        public List<List<AnimationTimeline>> GetInsertToRightAnimations(int value)
        {
            rightChildValue.Text = value.ToString();

            var rightXAnim = new SimulatedDoubleAnimation(by: (float)(radius * 2 - BaseOffsetX), time: 500)
            {
                TargetControl = right,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var rightYAnim = new SimulatedDoubleAnimation(by: (float)(layerHeight - BaseOffsetY), time: 500)
            {
                TargetControl = right,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var rightXAnim2 = new SimulatedDoubleAnimation(by: (float)(-BaseOffsetX), time: 500)
            {
                TargetControl = right,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var rightYAnim2 = new SimulatedDoubleAnimation(by: (float)(-BaseOffsetY), time: 500)
            {
                TargetControl = right,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var rightChildScaleXAnim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = rightChild,
                TargetParam = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleX)",
            };
            var rightChildScaleYAnim = new SimulatedDoubleAnimation(to: 1, time: 500)
            {
                TargetControl = rightChild,
                TargetParam = "(UIElement.RenderTransform).(TransformGroup.Children)[1].(ScaleTransform.ScaleY)",
            };

            return new List<List<AnimationTimeline>>
            {
                new List<AnimationTimeline> { rightXAnim, rightYAnim },
                new List<AnimationTimeline> { rightChildScaleXAnim, rightChildScaleYAnim, rightXAnim2, rightYAnim2 }
            };
        }

        public List<AnimationTimeline> GetRemoveLeftLineAnimations(double toX, double toY)
        {
            var xAnim = new SimulatedDoubleAnimation(to: toX, time: 500)
            {
                TargetControl = left,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var yAnim = new SimulatedDoubleAnimation(to: toY, time: 500)
            {
                TargetControl = left,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };

            return new List<AnimationTimeline> { xAnim, yAnim };
        }

        public List<AnimationTimeline> GetRemoveRightLineAnimations(double toX, double toY)
        {
            var xAnim = new SimulatedDoubleAnimation(to: toX, time: 500)
            {
                TargetControl = right,
                TargetParam = Line.X2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };
            var yAnim = new SimulatedDoubleAnimation(to: toY, time: 500)
            {
                TargetControl = right,
                TargetParam = Line.Y2Property,
                TargetName = "line_" + Comm.GetUniqueString()
            };

            return new List<AnimationTimeline> { xAnim, yAnim };
        }

        public List<AnimationTimeline> GetRemoveNodeAnimations()
        {
            var nodeScaleXAnim = new SimulatedDoubleAnimation(to: 0, time: 500)
            {
                TargetControl = node,
                TargetParam = "(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
            };
            var nodeScaleYAnim = new SimulatedDoubleAnimation(to: 0, time: 500)
            {
                TargetControl = node,
                TargetParam = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
            };

            return new List<AnimationTimeline> { nodeScaleXAnim, nodeScaleYAnim };
        }

        public List<AnimationTimeline> GetMoveElemAnimations(double byX, double byY)
        {
            var xAnim = new SimulatedDoubleAnimation(by: (float)byX, time: 500)
            {
                TargetControl = elem,
                TargetParam = "(UIElement.RenderTransform).(TranslateTransform.X)"
            };
            var yAnim = new SimulatedDoubleAnimation(by: (float)byY, time: 500)
            {
                TargetControl = elem,
                TargetParam = "(UIElement.RenderTransform).(TranslateTransform.Y)"
            };

            return new List<AnimationTimeline> { xAnim, yAnim };
        }

        public List<AnimationTimeline> GetMoveCopyValueItemAnimations(double byX, double byY)
        {
            var xAnim = new SimulatedDoubleAnimation(by: (float)byX, time: 500, before: () => { copyValueItem.Visibility = Visibility.Visible; }, after: null)
            {
                TargetControl = copyValueItem,
                TargetParam = "(UIElement.RenderTransform).(TranslateTransform.X)"
            };
            var yAnim = new SimulatedDoubleAnimation(by: (float)byY, time: 500)
            {
                TargetControl = copyValueItem,
                TargetParam = "(UIElement.RenderTransform).(TranslateTransform.Y)"
            };

            return new List<AnimationTimeline> { xAnim, yAnim };
        }
    }
}
