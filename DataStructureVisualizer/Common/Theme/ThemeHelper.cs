using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataStructureVisualizer.Common.Theme
{
    static class ThemeHelper
    {
        public static Color NewColor = new Color() { ScA = 1.0F, R = 255, G = 152, B = 0 };
        public static Color CorrectColor = new Color() { ScA = 1.0F, R = 76, G = 175, B = 80 };

        public static ITheme GetTheme()
        {
            return new PaletteHelper().GetTheme();
        }
    }
}
