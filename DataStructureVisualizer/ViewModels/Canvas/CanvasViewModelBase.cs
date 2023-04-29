using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels.Data;
using DataStructureVisualizer.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataStructureVisualizer.ViewModels.Canvas
{
    internal abstract partial class CanvasViewModelBase :
        ObservableRecipient,
        IRecipient<ValueChangedMessage<int[]>>,
        IRecipient<LoadAddAnimationMessage>,
        IRecipient<PauseAnyAnimationMessage>,
        IRecipient<ResumeAnyAnimationMessage>,
        IRecipient<LoadRemoveAnimationMessage>,
        IRecipient<StoryboradSpeedRadioChangedMessage>,
        IRecipient<GenerateDataMessage>
    {
        protected DS_SecondaryType Type;

        /// <summary>
        /// 数据结构的数据源
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<int> values;

        partial void OnValuesChanged(ObservableCollection<int> value)
        {
            // ReloadValues();
        }

        public MyStoryboard MainStoryboard { get; set; }

        protected UIElement GetCanvas()
        {
            //return WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>());
            return WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>(), "canvas");
        }

        protected CodeBlockPanelUserControl GetCodeBlockPanelView()
        {
            //return WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>());
            return (CodeBlockPanelUserControl)WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>(), "codeBlockPanel");
        }

        public CanvasViewModelBase()
        {
            IsActive = true;
            MainStoryboard = new MyStoryboard();

            Values = new ObservableCollection<int>();
            Values.CollectionChanged += (s, e) =>
            {
                ReloadValues();
            };
        }

        public abstract void UpdateDataItems(GenerateDataMessage? message);

        /// <summary>
        /// 响应【随机生成工具】的消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Receive(ValueChangedMessage<int[]> message)
        {
            //Values = new ObservableCollection<int>(message.Value);
            //Values.CollectionChanged += (s, e) =>
            //{
            //    ReloadValues();
            //};
            throw new NotImplementedException();
        }

        /// <summary>
        /// 响应【添加工具】的消息
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="NotImplementedException"></exception>
        public abstract void Receive(LoadAddAnimationMessage message);

        /// <summary>
        /// 响应【暂停动画】的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(PauseAnyAnimationMessage message)
        {
            MainStoryboard.Pause((Panel)GetCanvas());
        }

        /// <summary>
        /// 响应【恢复动画】的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(ResumeAnyAnimationMessage message)
        {
            if (MainStoryboard.GetIsPaused((Panel)GetCanvas()))
            {
                MainStoryboard.Resume((Panel)GetCanvas());
            }
        }

        private void ReloadValues()
        {
            UpdateDataItems(null);

            /* 
             * 1. 通知工具改变索引值选取的范围 
             */
            WeakReferenceMessenger.Default.Send(new DataSourceChangedMessage(Values));
        }

        /// <summary>
        /// 响应【删除工具】的消息
        /// </summary>
        /// <param name="message"></param>
        public abstract void Receive(LoadRemoveAnimationMessage message);

        /// <summary>
        /// 响应动画播放速率修改的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(StoryboradSpeedRadioChangedMessage message)
        {
            var canvas = GetCanvas() as FrameworkElement;
            MainStoryboard.SetSpeedRatio(canvas, message.SpeedRatio);
        }

        /// <summary>
        /// 响应【随机生成工具】的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(GenerateDataMessage message)
        {
            Values = new ObservableCollection<int>(message.Values);
            WeakReferenceMessenger.Default.Send(new DataSourceChangedMessage(Values));
            Values.CollectionChanged += (s, e) =>
            {
                ReloadValues();
            };
            UpdateDataItems(message);
        }
    }
}
