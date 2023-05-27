using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Windows.Controls;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    partial class LinkedListCanvasViewModel : CanvasViewModelBase
    {
        [ObservableProperty]
        private ObservableCollection<LinkedListItemViewModel> dataItems;

        public LinkedListCanvasViewModel()
        {
            Type = DS_SecondaryType.LinkedList;
        }

        /// <summary>
        /// 响应【添加工具】的“执行添加动画”消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Receive(LoadAddAnimationMessage message)
        {
            int addIndex = message.Index;
            int addValue = message.Value;

            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("llItemsControl") as ItemsControl;
            MainStoryboard = new MyStoryboard();
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            var codeBlockPanelView = GetCodeBlockPanelView();
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard(false);

            var llaf = new LinkedListAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems) { Values = Values };

            llaf.InsertElemAlgorithm(addIndex, addValue);
        }

        /// <summary>
        /// 响应【删除工具】的“加载删除动画”消息
        /// </summary>
        /// <param name="message"></param>
        public override void Receive(LoadRemoveAnimationMessage message)
        {
            int rmvIndex = message.Index;

            Grid canvas = (Grid)GetCanvas();
            var container = canvas.FindName("llItemsControl") as ItemsControl;
            MainStoryboard = new MyStoryboard();
            var codeBlockPanel = CodeBlockPanelViewModel.Instance;
            var codeBlockPanelView = GetCodeBlockPanelView();
            codeBlockPanel.CodeBlockStoryboard = new MyStoryboard(false);

            var llaf = new LinkedListAlgorithmFactory(canvas, codeBlockPanelView, container, MainStoryboard, DataItems);

            llaf.RemoveElemAlgorithm(rmvIndex);
        }

        public override void UpdateDataItems(GenerateDataMessage? message)
        {
            DataItems = new ObservableCollection<LinkedListItemViewModel>();

            List<Color> colors = Comm.GetColorGradientByValues(Values);

            DataItems.Add(new LinkedListItemViewModel() { Type = LinkedListItemType.Head });
            for (int i = 0; i < Values.Count; i++)
            {
                DataItems.Add(new LinkedListItemViewModel()
                {
                    Value = Values[i],
                    Index = i,
                    Color = new SolidColorBrush(colors[i]),
                });
            }
            DataItems.Add(new LinkedListItemViewModel() { Type = LinkedListItemType.Tail });
        }
    }
}
