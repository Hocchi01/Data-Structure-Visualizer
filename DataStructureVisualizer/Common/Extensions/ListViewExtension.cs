using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.Extensions
{
    public class ListViewExtension : DependencyObject
    {
        public static bool GetAutoScrollToCurrentItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoScrollToCurrentItemProperty);
        }

        public static void SetAutoScrollToCurrentItem(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoScrollToCurrentItemProperty, value);
        }

        public static readonly DependencyProperty AutoScrollToCurrentItemProperty =
            DependencyProperty.RegisterAttached("AutoScrollToCurrentItem", typeof(bool), typeof(ListViewExtension), new UIPropertyMetadata(default(bool), OnAutoScrollToCurrentItemChanged));

        public static void OnAutoScrollToCurrentItemChanged(DependencyObject s, DependencyPropertyChangedEventArgs e)
        {
            var listView = s as ListView;
            if (listView != null)
            {
                var listViewItems = listView.Items;
                if (listViewItems != null)
                {
                    var newValue = (bool)e.NewValue;

                    var autoScrollToCurrentItemWorker = new EventHandler((s1, e2) => OnAutoScrollToCurrentItem(listView, listView.Items.CurrentPosition));

                    if (newValue)
                        listViewItems.CurrentChanged += autoScrollToCurrentItemWorker;
                    else
                        listViewItems.CurrentChanged -= autoScrollToCurrentItemWorker;
                }
            }
        }

        public static void OnAutoScrollToCurrentItem(ListView ListView, int index)
        {
            if (ListView != null && ListView.Items != null && ListView.Items.Count > index && index >= 0)
                ListView.ScrollIntoView(ListView.Items[index]);
        }
    }
}
