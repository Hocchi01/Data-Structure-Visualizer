﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
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
                values[i] = r.Next(Min, Max);
            }

            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int[]>(values));
        }
    }
}