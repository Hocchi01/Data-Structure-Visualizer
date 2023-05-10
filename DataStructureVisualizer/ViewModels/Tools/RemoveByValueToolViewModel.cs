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
    partial class RemoveByValueToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private int value;

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadRemoveAnimationMessage() { Value = Value });
        }
    }
}
