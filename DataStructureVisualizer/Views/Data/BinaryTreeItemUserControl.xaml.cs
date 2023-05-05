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
    public partial class BinaryTreeItemUserControl : UserControl
    {
        private const double layerHeight = 125;
        private const double radius = 25;
        private static double baseDz = Math.Sqrt(Math.Pow(radius * 2, 2) + Math.Pow(layerHeight, 2));
        public double BaseOffsetX = radius * radius * 2 / baseDz;
        public double BaseOffsetY = radius * layerHeight / baseDz;

        public BinaryTreeItemUserControl()
        {
            InitializeComponent();
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
    }
}
