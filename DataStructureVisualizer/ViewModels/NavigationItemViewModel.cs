using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    class NavigationItemViewModel
    {
        public string Name { get; set; }
        public DS_MainType Type { get; set; }
        public string IconKind { get; set; }
        public List<NavigationChildItemViewModel> Children { get; set; }
    }
}
