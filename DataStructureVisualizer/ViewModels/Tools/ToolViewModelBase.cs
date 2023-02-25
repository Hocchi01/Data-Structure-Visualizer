using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Tools
{
    internal abstract partial class ToolViewModelBase : ObservableRecipient
    {
        [ObservableProperty]
        private bool isShowMoreSetting = false;

        [RelayCommand]
        private void SwitchMoreSettingVisibility()
        {
            IsShowMoreSetting = !IsShowMoreSetting;
        }

        [RelayCommand]
        public abstract void ExecuteFunc();

        public string Name { get; set; } = "";

        public ToolViewModelBase() 
        { 
            
        }
    }
}
