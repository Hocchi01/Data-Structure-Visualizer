using DataStructureVisualizer.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels.Data
{
    internal class LinkedListItemViewModel : LinearItemViewModel
    {
        public LinkedListItemType Type { get; set; } = LinkedListItemType.Normal;

        /// <summary>
        /// 链表结点的文本显示
        /// </summary>
        public string Text
        {
            get
            {
                switch (Type)
                {
                    case LinkedListItemType.Normal: return Value.ToString() ?? "";
                    case LinkedListItemType.Head: return "head";
                    case LinkedListItemType.Tail: return "null";
                    default: return "";
                }
            }
        }
    }
}
