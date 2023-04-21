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
        public bool IsHasCondition { get; set; } = false;
        public string Condition { get; set; }
        public string Action { get; set; }
        public List<string> Codes { get; set; }

        public LogViewModel(string condition, string action, string codes)
        {
            Condition = condition;
            IsHasCondition = true;
            Action = action;
            Codes = codes.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart() + ';').ToList();
        }

        public LogViewModel(string action, string codes)
        {
            Action = action;
            Codes = codes.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => s.TrimStart() + ';').ToList();
        }

        public LogViewModel(string action)
        {
            Action = action;
        }

    }
}
