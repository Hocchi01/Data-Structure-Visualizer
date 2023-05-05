using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AlgorithmFactories;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    internal class BinarySearchTreeCanvasViewModel : BinaryTreeCanvasViewModel, IRecipient<LoadSearchAnimationMessage>
    {
        /// <summary>
        /// 响应【查找工具】的消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Receive(LoadSearchAnimationMessage message)
        {
            MainStoryboard = new MyStoryboard();
            Grid canvas = (Grid)GetCanvas();
            var codeBlockPanelView = GetCodeBlockPanelView();
            var container = canvas.FindName("binarySearchTreeItemsControl") as ItemsControl;
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            var bstaf = new BinarySearchTreeAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems);

            bstaf.Search(message.Value);
        }

        /// <summary>
        /// 响应【添加工具】的消息
        /// </summary>
        /// <param name="message"></param>
        public override void Receive(LoadAddAnimationMessage message)
        {
            MainStoryboard = new MyStoryboard();
            Grid canvas = (Grid)GetCanvas();
            var codeBlockPanelView = GetCodeBlockPanelView();
            var container = canvas.FindName("binarySearchTreeItemsControl") as ItemsControl;
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard();

            var bstaf = new BinarySearchTreeAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems);

            bstaf.Insert(message.Value);
        }

        public override void UpdateDataItems(GenerateDataMessage? message)
        {
            if (Values.Count == 0) return;
            if (message != null && message.DataItems != null)
            {
                UpdateDataItemsByNewDataItems(message.DataItems);
            }
            else
            {
                List<Color> colors = Comm.GetColorGradientByValues(Values);

                DataItems = new ObservableCollection<BinaryTreeItemViewModel> { new BinaryTreeItemViewModel() { Value = Values[0], Color = new SolidColorBrush(colors[0]), OriginalColor = new SolidColorBrush(colors[0]), ParentIndex = -1 } };

                for (int i = 1; i < Values.Count; i++)
                {
                    int? j = 0;
                    int parentIndex = 0, direct = 0;
                    while (j != null)
                    {
                        parentIndex = j ?? -1;
                        direct = Values[i] < DataItems[parentIndex].Value ? 0 : 1;
                        j = DataItems[parentIndex].Children[direct];
                    }

                    DataItems[parentIndex].Children[direct] = i;
                    DataItems.Add(new BinaryTreeItemViewModel() { Value = Values[i], Color = new SolidColorBrush(colors[i]), OriginalColor = new SolidColorBrush(colors[i]), ParentIndex = parentIndex });
                }
            }

            CalcHeight(0, 0);
            CalcMargin(0);
            CalcPostion(0);
        }
    }
}
