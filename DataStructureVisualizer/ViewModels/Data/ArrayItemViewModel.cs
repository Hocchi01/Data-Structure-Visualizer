using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal partial class ArrayItemViewModel : LinearItemViewModel
    {
        public ArrayItemViewModel(ArrayItemViewModel aivm) 
        { 
            Value = aivm.Value;
            Index = aivm.Index;
            Color = aivm.Color;
            OldColor = aivm.OldColor;
            State = aivm.State;
        }

        public ArrayItemViewModel()
        {

        }
    }
}
