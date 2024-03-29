﻿using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.Common.Messages;
using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public class MyStoryboard : Storyboard
    {
        private double offset = 0;
        public double Offset
        {
            get { return offset; }
            set { offset = value; }
        }


        public List<KeyValuePair<string, DependencyObject>> RegisterTable { get; set; }

        /// <summary>
        /// 为动画绑定控件及其属性
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="targetControl"></param>
        /// <param name="targetParam"></param>
        public void Link(AnimationTimeline animation, DependencyObject targetControl, object targetParam, string? targetName)
        {
            if (targetName != null)
            {
                RegisterTable.Add(new KeyValuePair<string, DependencyObject>(targetName, targetControl));
                SetTargetName(animation, targetName);
            }
            else
            {
                SetTarget(animation, targetControl);
            }
            
            if (targetParam is string)
            {
                SetTargetProperty(animation, new PropertyPath((string)targetParam));
            }
            else
            {
                SetTargetProperty(animation, new PropertyPath(targetParam));
            }
        }

        /// <summary>
        /// 添加多个异步动画：（已过时）
        /// </summary>
        /// <param name="animations"></param>
        /// <param name="targetControls"></param>
        /// <param name="targetParams"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="targetNames"></param>
        [Obsolete]
        public void AddAsyncAnimations(List<AnimationTimeline> animations, List<DependencyObject> targetControls, List<object> targetParams, Action? before = null, Action? after = null, List<string>? targetNames = null)
        {
            // 为持续时间最长的动画追加动作
            int maxTimePos = 0;
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Duration.TimeSpan.TotalMilliseconds > animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds)
                {
                    maxTimePos = i;
                }
            }
            animations[maxTimePos].SetActions(before, after);
            
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].BeginTime = TimeSpan.FromMilliseconds(offset);
                Children.Add(animations[i]);
                string? targetName = targetNames != null ? targetNames[i] : null;
                Link(animations[i], targetControls[i], targetParams[i], targetName);
            }

            offset += animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 添加多个异步动画：每个动画类型可不同
        /// </summary>
        /// <param name="animations"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="log"></param>
        public void AddAsyncAnimations(List<AnimationTimeline> animations, Action? before = null, Action? after = null, LogViewModel? log = null)
        {
            if (log != null)
            {
                animations[0].AttachLog(log);
            }

            // 为持续时间最长的动画追加动作
            int maxTimePos = 0;
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Duration.TimeSpan.TotalMilliseconds > animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds)
                {
                    maxTimePos = i;
                }
            }
            animations[maxTimePos].SetActions(before, after);

            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].BeginTime = TimeSpan.FromMilliseconds(offset);
                var linkableAnim = animations[i] as ILinkableAnimation;
                Link(animations[i], linkableAnim.TargetControl, linkableAnim.TargetParam, linkableAnim.TargetName);
                if (maxTimePos != i)
                {
                    Children.Add(animations[i]);
                }
            }

            // 把其中执行时间最长的动画添加到当前 Children 最末尾
            Children.Add(animations[maxTimePos]);

            offset += animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 添加多个异步动画：每个动画相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animation"></param>
        /// <param name="targetControls"></param>
        /// <param name="targetParams"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        /// <param name="targetNames"></param>
        public void AddAsyncAnimations<T>(T animation, List<DependencyObject> targetControls, object targetParams, Action? before = null, Action? after = null, List<string>? targetNames = null, LogViewModel? log = null) where T : AnimationTimeline, ILinkableAnimation
        {
            var animations = new List<T>();
            for (int i = 0; i < targetControls.Count; i++)
            {
                T anim = Comm.DeepCopy(animation) as T;
                anim.TargetControl = targetControls[i];
                anim.TargetParam = targetParams;
                if (targetNames != null)
                {
                    anim.TargetName = targetNames[i];
                }
                animations.Add(anim);
            }

            AddAsyncAnimations(animations, before, after, log);
        }

        /// <summary>
        /// 添加多个异步动画：同类动画，每个动画参数可不同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animations"></param>
        /// <param name="before"></param>
        /// <param name="after"></param>
        public void AddAsyncAnimations<T>(List<T> animations, Action? before = null, Action? after = null, LogViewModel? log = null) where T : AnimationTimeline, ILinkableAnimation
        {
            if (log != null)
            {
                animations[0].AttachLog(log);
            }

            // 为持续时间最长的动画追加动作
            int maxTimePos = 0;
            for (int i = 0; i < animations.Count; i++)
            {
                if (animations[i].Duration.TimeSpan.TotalMilliseconds > animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds)
                {
                    maxTimePos = i;
                }
            }
            animations[maxTimePos].SetActions(before, after);

            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].BeginTime = TimeSpan.FromMilliseconds(offset);
                Link(animations[i], animations[i].TargetControl, animations[i].TargetParam, animations[i].TargetName);
                if (maxTimePos != i)
                {
                    Children.Add(animations[i]);
                }
            }

            // 把其中执行时间最长的动画添加到当前 Children 最末尾
            Children.Add(animations[maxTimePos]);

            offset += animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 添加单个同步动画：需传入动画参数
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="targetControl"></param>
        /// <param name="targetParam"></param>
        /// <param name="targetName"></param>
        public void AddSyncAnimation(AnimationTimeline animation, DependencyObject targetControl, object targetParam, string? targetName = null)
        {
            animation.BeginTime = TimeSpan.FromMilliseconds(offset);
            Children.Add(animation);
            Link(animation, targetControl, targetParam, targetName);

            offset += animation.Duration.TimeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 添加单个同步动画
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="animation"></param>
        public void AddSyncAnimation<T>(T animation) where T : AnimationTimeline, ILinkableAnimation
        {
            AddSyncAnimation(animation, animation.TargetControl, animation.TargetParam, animation.TargetName);
        }

        public void AddSyncAnimation<T>(T animation, Action? before = null, Action? after = null) where T : AnimationTimeline, ILinkableAnimation
        {
            animation.SetActions(before, after);
            AddSyncAnimation(animation, animation.TargetControl, animation.TargetParam, animation.TargetName);
        }

        public void InsertAction(Action action)
        {
            int count = Children.Count;
            if (count == 0)
            {
                action();
            }
            else
            {
                (Children[count - 1] as AnimationTimeline).SetActions(null, action);
            }
        }

        public void InsertLog(LogViewModel log)
        {
            int count = Children.Count;
            if (count == 0)
            {
                WeakReferenceMessenger.Default.Send(new AddAnyLogMessage(log));
            }
            else
            {
                (Children[count - 1] as AnimationTimeline).AttachLog(log);
            }
        }

        // 
        public void Begin_Ex(FrameworkElement containingObject, bool isControllable = true)
        {
            foreach (var item in RegisterTable)
            {
                containingObject.RegisterName(item.Key, item.Value);
            }
            Completed += (_, _) =>
            {
                foreach (var item in RegisterTable)
                {
                    containingObject.UnregisterName(item.Key);
                }
            };

            Duration = TimeSpan.FromMilliseconds(offset + 1000); // 最终暂停 1s
            Begin(containingObject, isControllable);

            if (isControllable)
            {
                WeakReferenceMessenger.Default.Send(new BeginAnyAnimationMessage());

                //AnimationControlPanelViewModel.Instance.State = AnimationState.Paused;
                //WeakReferenceMessenger.Default.Send(new PauseAnyAnimationMessage());
            }

            
        }

        public void Delay(double millisecond)
        {
            offset += millisecond;
        }

        public MyStoryboard(bool isMain = true)
        {
            //// 添加一个空动画，便于插入事件动作
            //Children.Add(new DoubleAnimation() { Duration = TimeSpan.Zero });
            if (isMain)
            {
                Completed += (s, e) =>
                {
                    WeakReferenceMessenger.Default.Send(new EndAnyAnimationMessage());
                };
            }
            

            RegisterTable = new List<KeyValuePair<string, DependencyObject>>();
        }


        //private void Reset()
        //{
        //    offset = 0;
        //    Children.Clear();
        //}
    }
}
