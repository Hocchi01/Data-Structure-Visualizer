using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AlgorithmFactories;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    partial class BinaryTreeCanvasViewModel : CanvasViewModelBase, IRecipient<LoadBinaryTreeTraverseAnimationMessage>
    {
        private const double halfWidth = 0;
        private const double layerHeight = 125;
        private const double margin = 50;
        [ObservableProperty]
        private ObservableCollection<BinaryTreeItemViewModel> dataItems;

        private int height = 0;
        public BinaryTreeCanvasViewModel()
        {
            DataItems = new ObservableCollection<BinaryTreeItemViewModel> { new BinaryTreeItemViewModel() };
        }

        public override void Receive(LoadAddAnimationMessage message)
        {
            throw new NotImplementedException();
        }

        public override void Receive(LoadRemoveAnimationMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 响应【遍历工具】消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Receive(LoadBinaryTreeTraverseAnimationMessage message)
        {
            MainStoryboard = new MyStoryboard();
            Grid canvas = (Grid)GetCanvas();
            var codeBlockPanelView = GetCodeBlockPanelView();
            var container = canvas.FindName("binaryTreeItemsControl") as ItemsControl;
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            var btaf = new BinaryTreeAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems);

            btaf.Traverse(message.Mode);
        }

        public override void UpdateDataItems(GenerateDataMessage? message)
        {
            if (Values.Count == 0) return;

            var type = message == null ? TreeType.NormalBinaryTree : message.TreeType; 

            List<Color> colors = Comm.GetColorGradientByValues(Values);

            DataItems = new ObservableCollection<BinaryTreeItemViewModel> { new BinaryTreeItemViewModel() { Value = Values[0], Color = new SolidColorBrush(colors[0]), OriginalColor = new SolidColorBrush(colors[0]), ParentIndex = -1 } };

            // var leaves = new List<int> { 0 };
            var accessList = new List<Access>
            {
                new Access(0, 0),
                new Access(0, 1)
            };

            Random r = new Random();
            for (int i = 1; i < Values.Count; i++)
            {
                int accessIndex = type == TreeType.NormalBinaryTree ? r.Next(0, accessList.Count - 1) : 0;
                Access access = accessList[accessIndex];

                DataItems[access.index].Children[access.child] = i;
                accessList.RemoveAt(accessIndex);

                DataItems.Add(new BinaryTreeItemViewModel() { Value = Values[i], Color = new SolidColorBrush(colors[i]), OriginalColor = new SolidColorBrush(colors[i]), ParentIndex = access.index });

                accessList.Add(new Access(i, 0));
                accessList.Add(new Access(i, 1));
            }


            CalcHeight(0, 0);
            CalcMargin(0);
            CalcPostion(0);

        }

        private void CalcHeight(int? index, int layer)
        {
            if (index == null) return;
            int i = index ?? -1;

            DataItems[i].AxisY = layerHeight * layer;
            if (DataItems[i].IsLeaf)
            {
                height = Math.Max(height, layer);
                return;
            }

            int? leftIndex = DataItems[i].Children[0];
            int? rightIndex = DataItems[i].Children[1];

            CalcHeight(leftIndex, layer + 1);
            CalcHeight(rightIndex, layer + 1);
        }

        private double CalcMargin(int? index)
        {
            if (index == null) return margin;
            int i = index ?? -1;

            int? leftIndex = DataItems[i].Children[0];
            int? rightIndex = DataItems[i].Children[1];

            DataItems[i].LeftMargin = CalcMargin(leftIndex) + halfWidth;
            DataItems[i].RightMargin = CalcMargin(rightIndex) + halfWidth;

            return DataItems[i].LeftMargin + DataItems[i].RightMargin;
        }

        private void CalcPostion(int? index)
        {
            if (index == null) return;
            int i = index ?? -1;

            int? leftIndex = DataItems[i].Children[0];
            if (leftIndex != null)
            {
                int li = leftIndex ?? -1;
                DataItems[li].AxisX = DataItems[i].AxisX - DataItems[li].RightMargin;
                DataItems[i].LeftLineX2 += (DataItems[li].AxisX - DataItems[i].AxisX);
                DataItems[i].LeftLineY2 += layerHeight;
                DataItems[i].OffsetLeftLine();
            }

            int? rightIndex = DataItems[i].Children[1];
            if (rightIndex != null)
            {
                int ri = rightIndex ?? -1;
                DataItems[ri].AxisX = DataItems[i].AxisX + DataItems[ri].LeftMargin;
                DataItems[i].RightLineX2 += (DataItems[ri].AxisX - DataItems[i].AxisX);
                DataItems[i].RightLineY2 += layerHeight;
                DataItems[i].OffsetRightLine();
            }

            CalcPostion(leftIndex);
            CalcPostion(rightIndex);
        }

        struct Access
        {
            public int child;
            public int index;
            public Access(int i, int c)
            {
                index = i;
                child = c;
            }
        }
    }
}
