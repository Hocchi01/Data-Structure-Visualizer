using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System.Reflection;
using CommunityToolkit.Mvvm.Messaging;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    internal class ArrayCanvasViewModel : CanvasViewModelBase
    {
        public override void UpdateDataItems()
        {
            DataItems = new ObservableCollection<DataItemViewModelBase>();

            List<Color> colors = Comm.GetColorGradientByValues(Values);
            for (int i = 0; i < Values.Count; i++)
            {
                DataItems.Add(new ArrayItemViewModel()
                {
                    Value = Values[i],
                    Index = i,
                    Color = new SolidColorBrush(colors[i]),
                });
            }
        }

        /// <summary>
        /// 响应【添加工具】的 “执行添加动画” 消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Receive(LoadAddAnimationMessage message)
        {
            int count = DataItems.Count;
            Grid canvas = (Grid)GetCanvas();
            var container = canvas.Children[0] as ItemsControl;
            var paletteHelper = new PaletteHelper();

            // TODO 02: 在数组末尾添加一个“空项”
            DataItems.Add(new ArrayItemViewModel() { Index = count });

            // TODO 03: 编写动画主体
            MainStoryboard = new MyStoryboard();
            for (int i = count - 1; i >= message.Index; i--)
            {
                var itemView = Comm.GetItemFromItemsControlByIndex<ArrayItemUserControl>(container, i);
                var oldColor = (DataItems[i] as ArrayItemViewModel).Color;

                Action after = () => { itemView.rect.Fill = oldColor; };
                if (i == message.Index)
                {
                    after += () =>
                    {
                        Values.Insert(message.Index, message.Value);
                        //Values.Add(message.Value);
                        //Values = Values; // 调用 Set 访问器
                        WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(new LogViewModel() { Content = $"a[{message.Index}] = {message.Value};" }));
                    };
                }

                MainStoryboard.AddSyncAnimation
                (
                    new SimulatedDoubleAnimation
                    (
                        to: 52, 
                        time: 500,
                        before: () => { itemView.rect.Fill = new SolidColorBrush(paletteHelper.GetTheme().SecondaryMid.Color); },
                        after: after,
                        log: new LogViewModel() { Content = $"a[{i+1}] = a[{i}];" }
                    ),
                    itemView.dataItem,
                    "(UIElement.RenderTransform).(TranslateTransform.X)"
                );
            }

            // TODO 05: 执行动画
            MainStoryboard.Begin_Ex(canvas, true);
        }

        public ArrayCanvasViewModel()
        {
            Type = DS_SecondaryType.Array;
        }
    }
}
