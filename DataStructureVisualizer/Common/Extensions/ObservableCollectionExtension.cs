using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DataStructureVisualizer.Common.Extensions
{
    static class ObservableCollectionExtension
    {
        public static void RaiseCollectionChanged<T>(this ObservableCollection<T> collection) 
        {
            
        }

        public static ObservableCollection<DataItemViewModelBase> RevertElems<T>(this ObservableCollection<T> oldCollection)
            where T : DataItemViewModelBase
        {
            var newCollection = new ObservableCollection<DataItemViewModelBase>();
            foreach (var item in oldCollection)
            {
                newCollection.Add(item);
            }
            return newCollection;
        }
    }
}
