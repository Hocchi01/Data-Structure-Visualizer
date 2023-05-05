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
    internal partial class SearchToolViewModel : ToolViewModelBase
    {
        [ObservableProperty]
        private int value;

        public SearchToolViewModel()
        {
            Name = "Search";
        }

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadSearchAnimationMessage(Value));
        }
    }
}
