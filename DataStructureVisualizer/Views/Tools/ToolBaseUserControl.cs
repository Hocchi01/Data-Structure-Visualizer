using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace DataStructureVisualizer.Views.Tools
{
    public partial class ToolBaseUserControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(ToolBaseUserControl));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty MainTemplateProperty =
        DependencyProperty.Register(nameof(MainTemplate), typeof(DataTemplate), typeof(ToolBaseUserControl));

        public DataTemplate MainTemplate
        {
            get => (DataTemplate)GetValue(MainTemplateProperty);
            set => SetValue(MainTemplateProperty, value);
        }

        public static readonly DependencyProperty DetailTemplateProperty =
        DependencyProperty.Register(nameof(DetailTemplate), typeof(DataTemplate), typeof(ToolBaseUserControl));

        public DataTemplate DetailTemplate
        {
            get => (DataTemplate)GetValue(DetailTemplateProperty);
            set => SetValue(DetailTemplateProperty, value);
        }

        public static readonly DependencyProperty DetailHeightProperty =
        DependencyProperty.Register(nameof(DetailHeight), typeof(double), typeof(ToolBaseUserControl));

        public double DetailHeight
        {
            get => (double)GetValue(DetailHeightProperty);
            set => SetValue(DetailHeightProperty, value);
        }

        public ToolBaseUserControl()
        {
            DefaultStyleKey = typeof(ToolBaseUserControl);
            // InitializeComponent();
        }
    }
}
