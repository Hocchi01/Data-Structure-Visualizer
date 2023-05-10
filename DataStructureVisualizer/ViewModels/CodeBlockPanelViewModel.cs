using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common;
using DataStructureVisualizer.Common.AnimationLib;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.Common.Structs;
using DataStructureVisualizer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.ViewModels
{
    internal partial class CodeBlockPanelViewModel : ObservableRecipient, IRecipient<StoryboradSpeedRadioChangedMessage>,
        IRecipient<PauseAnyAnimationMessage>,
        IRecipient<ResumeAnyAnimationMessage>,
        IRecipient<SkipAnyAnimationMessage>
    {
        // private CodeBlockPanelUserControl codeBlockPanelView;

        private static CodeBlockPanelViewModel instance = null;
        private static readonly object padlock = new object();

        private CodeBlockPanelViewModel()
        {
            IsActive = true;
            // codeBlockPanelView = (CodeBlockPanelUserControl)WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>(), "codeBlockPanel");
        }

        private CodeBlockPanelUserControl GetCodeBlockPanelView()
        {
            return (CodeBlockPanelUserControl)WeakReferenceMessenger.Default.Send(new RequestMessage<UIElement>(), "codeBlockPanel");
        }

        public static CodeBlockPanelViewModel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CodeBlockPanelViewModel();
                    }
                    return instance;
                }
            }
        }

        [ObservableProperty]
        private List<string> codeBlock;

        public MyStoryboard CodeBlockStoryboard { get; set; }

        public void SetCodeBlock(string codeBlock)
        {
            CodeBlock = codeBlock.Split("\\", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public void AddAnimation(CodeInfo info, double? offset = null, double time = 150)
        {
            var codeBlockPanelView = GetCodeBlockPanelView();

            double begin = (double)info.LineNum / CodeBlock.Count;
            double end = (double)(info.LineNum + info.Height) / CodeBlock.Count;


            var begin1Anim = new SimulatedDoubleAnimation(to: begin, time: time) { TargetControl = codeBlockPanelView.begin1, TargetParam = GradientStop.OffsetProperty, TargetName = "gs_" + Comm.GetUniqueString() };
            var begin2Anim = new SimulatedDoubleAnimation(to: begin, time: time) { TargetControl = codeBlockPanelView.begin2, TargetParam = GradientStop.OffsetProperty, TargetName = "gs_" + Comm.GetUniqueString() };
            var end1Anim = new SimulatedDoubleAnimation(to: end, time: time) { TargetControl = codeBlockPanelView.end1, TargetParam = GradientStop.OffsetProperty, TargetName = "gs_" + Comm.GetUniqueString() };
            var end2Anim = new SimulatedDoubleAnimation(to: end, time: time) { TargetControl = codeBlockPanelView.end2, TargetParam = GradientStop.OffsetProperty, TargetName = "gs_" + Comm.GetUniqueString() };


            if (offset != null)
            {
                CodeBlockStoryboard.Offset = offset ?? CodeBlockStoryboard.Offset;
            }

            CodeBlockStoryboard.AddAsyncAnimations(new List<AnimationTimeline>
            {
                begin1Anim,
                begin2Anim,
                end1Anim,
                end2Anim,
            });




        }

        /// <summary>
        /// 响应动画播放速率修改的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(StoryboradSpeedRadioChangedMessage message)
        {
            var codeBlockPanelView = GetCodeBlockPanelView();
            CodeBlockStoryboard.SetSpeedRatio(codeBlockPanelView, message.SpeedRatio);
        }

        /// <summary>
        /// 响应【暂停动画】的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(PauseAnyAnimationMessage message)
        {
            CodeBlockStoryboard.Pause(GetCodeBlockPanelView());
        }

        /// <summary>
        /// 响应【恢复动画】的消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(ResumeAnyAnimationMessage message)
        {
            if (CodeBlockStoryboard.GetIsPaused(GetCodeBlockPanelView()))
            {
                CodeBlockStoryboard.Resume(GetCodeBlockPanelView());
            }
        }

        public void Receive(SkipAnyAnimationMessage message)
        {
            var codeBlockPanelView = GetCodeBlockPanelView();
            CodeBlockStoryboard.SkipToFill(codeBlockPanelView);
        }
    }
}
