using DataStructureVisualizer.Common.Enums;
using DataStructureVisualizer.ViewModels.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.Common.Messages
{
    class GenerateDataMessage
    {
        public int[] Values { get; set; }
        public TreeType TreeType { get; set; }

        private ObservableCollection<DataItemViewModelBase>? dataItems = null;

        public ObservableCollection<DataItemViewModelBase>? DataItems
        {
            get { return dataItems; }
            set 
            { 
                dataItems = value;
                if (value != null)
                {
                    Values = new int[value.Count];
                    for (int i = 0; i < value.Count; i++)
                    {
                        Values[i] = value[i].Value ?? 0;
                    }
                }
            }
        }


        public GenerateDataMessage(int[] values)
        {
            Values = values;
        }

        public GenerateDataMessage()
        {

        }
    }
}
