using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.ViewModels
{
    class NavigationChildItemViewModel
    {
        public string Name { get; set; }
        public DS_SecondaryType Type { get; set; }
        public Type CanvasViewModelType { get; set; }
        public Type CanvasViewType { get; set; }

        public NavigationChildItemViewModel()
        {
        }
    }
}
