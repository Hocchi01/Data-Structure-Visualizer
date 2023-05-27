using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Tools
{
    partial class InputGenerateToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private string data = "";

        public override void ExecuteFunc()
        {
            int[] values = Data.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();

            WeakReferenceMessenger.Default.Send(new GenerateDataMessage(values));
        }
    }
}
