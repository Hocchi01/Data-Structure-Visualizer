using DataStructureVisualizer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace DataStructureVisualizer.Common.AnimationLib
{
    public class SimulatedDoubleAnimation : MyDoubleAnimationBase
    {
        public SimulatedDoubleAnimation() : base() 
        {
            Simulate(0.5, 0.5);
        }

        public SimulatedDoubleAnimation(double to, double time, double? from = null, double acc = 0.5, double dec = 0.5, LogViewModel? log = null) : base(to, time, from, log)
        {
            Simulate(acc, dec);
        }

        public SimulatedDoubleAnimation
        (
            double to,
            double time,
            Action? before,
            Action? after,
            double? from = null, 
            double acc = 0.5, 
            double dec = 0.5, 
            LogViewModel? log = null
        ) : base(to, time, before, after, from, log)
        {
            Simulate(acc, dec);
        }

        public SimulatedDoubleAnimation(float by, double time, double? from = null) : base(by, time, from) 
        {
            Simulate(0.5, 0.5);
        }

        public SimulatedDoubleAnimation(float by, double time, Action? before, Action? after, double? from = null) : base(by, time, from, before, after)
        {
            Simulate(0.5, 0.5);
        }

        private void Simulate(double acc, double dec)
        {
            AccelerationRatio = acc;
            DecelerationRatio = dec;
        }
    }
}
