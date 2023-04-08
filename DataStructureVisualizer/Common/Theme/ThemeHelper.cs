using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Theme
{
    static class ThemeHelper
    {
        public static ITheme GetTheme()
        {
            return new PaletteHelper().GetTheme();
        }
    }
}
