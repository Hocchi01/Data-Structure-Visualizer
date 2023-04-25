using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructureVisualizer.ViewModels
{
    public class LogViewModel
    {
        private const string newLineCode = "&#x0a;";

        public bool IsHasCondition { get; set; } = false;
        public string Condition { get; set; }
        public string Action { get; set; }
        public List<string> Codes { get; set; }
        //public string Codes { get; set; }

        public LogViewModel(string condition, string action, string codes) : this(action, codes)
        {
            Condition = condition;
            IsHasCondition = true;
        }

        public LogViewModel(string action, string codes) : this(action)
        {
            Codes = codes.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart() + ';').ToList();
            //Codes = string.Join(null, codes.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s += newLineCode));
        }

        public LogViewModel(string action)
        {
            Action = action;
        }

    }
}
