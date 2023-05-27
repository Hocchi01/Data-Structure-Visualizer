using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Tools
{
    internal partial class RandomGenerateToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private int amount = 10;

        [ObservableProperty]
        private int min = 0;

        [ObservableProperty]
        private int max = 100;

        public RandomGenerateToolViewModel()
        {
            Name = "Random Generate";
            
        }

        public override void ExecuteFunc()
        {
            int[] values = new int[Amount];
            Random r = new Random();
            for (int i = 0; i < Amount; i++)
            {
                int val = r.Next(Min, Max);
                while (values.Contains(val))
                {
                    val = (val + 1) % (Max + 1);
                }
                values[i] = val;
            }

            WeakReferenceMessenger.Default.Send(new GenerateDataMessage(values));
        }
    }
}
