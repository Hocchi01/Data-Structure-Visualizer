﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.ViewModels.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using Microsoft.Extensions.DependencyInjection;

namespace DataStructureVisualizer.ViewModels
{
    internal partial class MainWindowViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<NavigationChildItemViewModel>>, IRecipient<RequestMessage<UIElement>>
    {
        public NavigationViewModel Navigation { get; }
        public ToolboxViewModel Toolbox { get; }
        public AnimationControlPanelViewModel AnimationControlPanel { get; }
        public LogPanelViewModel LogPanel { get; }

        [ObservableProperty]
        private UserControl? currentCanvasView;

        public CanvasViewModelBase? CurrentCanvas { get; set; }

        public MainWindowViewModel() 
        {
            IsActive = true;
            Navigation = new NavigationViewModel();
            Toolbox = new ToolboxViewModel();
            AnimationControlPanel = new AnimationControlPanelViewModel();
            LogPanel = new LogPanelViewModel();
        }

        /// <summary>
        /// 响应 Canvas 切换消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(PropertyChangedMessage<NavigationChildItemViewModel> message)
        {
            CurrentCanvasView = Activator.CreateInstance(message.NewValue.CanvasViewType) as UserControl;

            if (CurrentCanvas != null)
            {
                // 注销旧 ViewModel 注册的消息监听，否则该 ViewModel 不会被垃圾回收
                CurrentCanvas.IsActive = false;
            }
            CurrentCanvas = Activator.CreateInstance(message.NewValue.CanvasViewModelType) as CanvasViewModelBase;

            if (CurrentCanvasView != null)
            {
                CurrentCanvasView.DataContext = CurrentCanvas;
            } 
        }

        /// <summary>
        /// 响应 “获取 canvas 控件” 的请求消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(RequestMessage<UIElement> message)
        {
            message.Reply((UIElement)CurrentCanvasView.FindName("canvas"));
        }
    }


}