using CommunityToolkit.Mvvm.ComponentModel;
using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal partial class DataItemViewModelBase : ObservableObject
    {
        public int? Value { get; set; } = null;
        public int? Index { get; set; } = null;
        [ObservableProperty]
        private DataItemState state = DataItemState.Normal;
    }
}
