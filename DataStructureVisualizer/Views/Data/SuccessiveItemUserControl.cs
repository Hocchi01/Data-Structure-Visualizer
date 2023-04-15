using DataStructureVisualizer.Common.AnimationLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Views.Data
{
    public class SuccessiveItemUserControl : DataItemUserControlBase
    {
        public SimulatedDoubleAnimation MoveValueItem(float by, Action? before = null, Action? after = null)
        {
            return new SimulatedDoubleAnimation(by: by, time: 500, before: before, after: after)
            {
                TargetControl = ValueItem,
                TargetParam = AnimationHelper.HorizontallyMoveParam
            };
        }

        public SimulatedDoubleAnimation MoveValueItem(int by, Action? before = null, Action? after = null)
        {
            return MoveValueItem(by * (float)AnimationHelper.StepLen, before, after);
        }


    }
}
