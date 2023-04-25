using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using DataStructureVisualizer.Common.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    internal partial class LogPanelViewModel : ObservableRecipient, IRecipient<AddAnyLogMessage>, IRecipient<BeginAnyAnimationMessage>
    {
        [ObservableProperty]
        private int logItemsLastIndex = -1;

        [ObservableProperty]
        private ObservableCollection<LogViewModel> logItems;

        public LogPanelViewModel()
        {
            IsActive = true;
            logItems = new ObservableCollection<LogViewModel>();
        }

        public void Receive(AddAnyLogMessage message)
        {
            LogItems.Add(message.Log);
            LogItemsLastIndex++;
        }

        public void Receive(BeginAnyAnimationMessage message)
        {
            LogItems.Clear();
            LogItemsLastIndex = -1;
        }
    }
}
