using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    static class AnimationTimelineExtension
    {
        public static void SetActions(this AnimationTimeline timeline, Action? before, Action? after) 
        {
            timeline.CurrentStateInvalidated += (sender, e) =>
            {
                Clock clock = (Clock)sender;
                switch (clock.CurrentState)
                {
                    case ClockState.Active:
                        if (before != null)
                        {
                            before();
                        }
                        break;
                    case ClockState.Filling:
                        // 改变 Storyboard 动画播放速度时，会导致 Storyboard 结束后触发其中所有动画的 CurrentStateInvalidated 事件（此时 ClockState.Filling）因此在这里加了动画状态的判断
                        if (after != null && AnimationControlPanelViewModel.Instance.State != AnimationState.Stopped)
                        {
                            after();
                        }
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
