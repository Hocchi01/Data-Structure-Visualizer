using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataStructureVisualizer.Common.Rules
{
    class IntRangeValidationRule : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int i = 0;
            if (int.TryParse(value.ToString(), out i))
            {
                if (i >= this.Min && i <= this.Max)
                {
                    return new ValidationResult(true, null);
                }
            }
            return new ValidationResult(false, string.Format("Range: [{0}, {1}]", this.Min, this.Max));
        }
    }
}
