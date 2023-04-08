using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
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
        public List<KeyValuePair<string, DependencyObject>> RegisterTable { get; set; }

        /// <summary>
        /// 为动画绑定控件及其属性
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="targetControl"></param>
        /// <param name="targetParam"></param>
        public void Link(AnimationTimeline animation, DependencyObject targetControl, object targetParam)
        {
            SetTarget(animation, targetControl);
            if (targetParam is string)
            {
                SetTargetProperty(animation, new PropertyPath((string)targetParam));
            }
            else
            {
                SetTargetProperty(animation, new PropertyPath(targetParam));
            }
        }

        public void AddAsyncAnimations(List<AnimationTimeline> animations, List<DependencyObject> targetControls, List<object> targetParams, Action? before = null, Action? after = null)
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
                Link(animations[i], targetControls[i], targetParams[i]);
            }

            offset += animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds;
        }

        public void AddAsyncAnimations<T>(T animation, List<DependencyObject> targetControls, object targetParams, Action? before = null, Action? after = null) where T : AnimationTimeline, ILinkableAnimation
        {
            var animations = new List<T>();
            for (int i = 0; i < targetControls.Count; i++)
            {
                T anim = Comm.DeepCopy(animation) as T;
                anim.TargetControl = targetControls[i];
                anim.TargetParam = targetParams;
                animations.Add(anim);
            }

            AddAsyncAnimations(animations, before, after);
        }

        public void AddAsyncAnimations<T>(List<T> animations, Action? before = null, Action? after = null) where T : AnimationTimeline, ILinkableAnimation
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
                Link(animations[i], animations[i].TargetControl, animations[i].TargetParam);
                if (maxTimePos != i)
                {
                    Children.Add(animations[i]);
                }
            }

            // 把其中执行时间最长的动画添加到当前 Children 最末尾
            Children.Add(animations[maxTimePos]);

            offset += animations[maxTimePos].Duration.TimeSpan.TotalMilliseconds;
        }

        public void AddSyncAnimation(AnimationTimeline animation, DependencyObject targetControl, object targetParam)
        {
            animation.BeginTime = TimeSpan.FromMilliseconds(offset);
            Children.Add(animation);
            Link(animation, targetControl, targetParam);

            offset += animation.Duration.TimeSpan.TotalMilliseconds;
        }

        public void AddSyncAnimation<T>(T animation) where T : AnimationTimeline, ILinkableAnimation
        {
            AddSyncAnimation(animation, animation.TargetControl, animation.TargetParam);
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

        // 
        public void Begin_Ex(FrameworkElement containingObject, bool isControllable = true)
        {
            foreach (var item in RegisterTable)
            {
                containingObject.RegisterName(item.Key, item.Value);
            }

            Duration = TimeSpan.FromMilliseconds(offset); // 最终暂停 1s
            Begin(containingObject, isControllable);
            WeakReferenceMessenger.Default.Send(new BeginAnyAnimationMessage());
        }

        public void Delay(double millisecond)
        {
            offset += millisecond;
        }

        public MyStoryboard()
        {
            //// 添加一个空动画，便于插入事件动作
            //Children.Add(new DoubleAnimation() { Duration = TimeSpan.Zero });
            Completed += (s, e) => 
            { 
                WeakReferenceMessenger.Default.Send(new EndAnyAnimationMessage());
            };

            RegisterTable = new List<KeyValuePair<string, DependencyObject>>();
        }


        //private void Reset()
        //{
        //    offset = 0;
        //    Children.Clear();
        //}
    }
}
