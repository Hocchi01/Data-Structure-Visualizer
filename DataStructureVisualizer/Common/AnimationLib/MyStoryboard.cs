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
    internal class MyStoryboard : Storyboard
    {
        private double offset = 0;

        public void Link(AnimationTimeline animation, UIElement targetControl, object targetParam)
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

        //public void AddAsyncAnimation(AnimationTimeline animation)
        //{
        //    animation.BeginTime = TimeSpan.FromMilliseconds(offset);
        //    Children.Add(animation);
        //}

        public void AddSyncAnimation(AnimationTimeline animation, UIElement targetControl, object targetParam)
        {
            animation.BeginTime = TimeSpan.FromMilliseconds(offset);
            Children.Add(animation);
            Link(animation, targetControl, targetParam);

            offset += animation.Duration.TimeSpan.TotalMilliseconds;
        }

        // 
        public void Begin_Ex(FrameworkElement containingObject, bool isControllable)
        {
            Duration = TimeSpan.FromMilliseconds(offset + 1000); // 最终暂停 1s
            Begin(containingObject, isControllable);
            WeakReferenceMessenger.Default.Send(new BeginAnyAnimationMessage());
        }

        public MyStoryboard()
        {
            Completed += (s, e) => 
            { 
                WeakReferenceMessenger.Default.Send(new EndAnyAnimationMessage());
            };
        }

        //private void Reset()
        //{
        //    offset = 0;
        //    Children.Clear();
        //}
    }
}
