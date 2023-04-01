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
    internal partial class RemoveToolViewModel : ToolViewModelBase, IRecipient<DataSourceChangedMessage>
    {
        [ObservableProperty]
        private int index;

        [ObservableProperty]
        private int[]? indices = null;

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadRemoveAnimationMessage(Index));
        }

        /// <summary>
        /// 响应数据源改变消息
        /// </summary>
        /// <param name="message"></param>
        public void Receive(DataSourceChangedMessage message)
        {
            Indices = Enumerable.Range(0, message.Value.Count).ToArray();
        }

        public RemoveToolViewModel()
        {
            IsActive = true;
        }
    }
}
