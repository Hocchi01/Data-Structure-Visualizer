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
    partial class AddToolViewModel : ToolViewModelBase, IRecipient<DataSourceChangedMessage>
    {
        [ObservableProperty]
        private int index;

        [ObservableProperty]
        private int value;

        [ObservableProperty]
        private int[] indices = { 0 };

        public override void ExecuteFunc()
        {
            WeakReferenceMessenger.Default.Send(new LoadAddAnimationMessage(Index, Value));
        }

        /// <summary>
        /// 响应数据源改变消息
        /// </summary>
        /// <remarks>
        /// 生成的索引序列中的最大值应比数据源的最大索引大 1，其表示向数据末尾追加数据
        /// </remarks>
        /// <param name="message"></param>
        public void Receive(DataSourceChangedMessage message)
        {
            Indices = Enumerable.Range(0, message.Value.Count + 1).ToArray();
        }

        public AddToolViewModel()
        {
            IsActive = true;
        }
    }
}
