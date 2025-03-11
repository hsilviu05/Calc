using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class Settings
    {
        public bool UseDigitGrouping { get; set; }
        public bool IsStandardMode { get; set; }
        public NumberBase LastNumberBase { get; set; }

        public Settings()
        {
            UseDigitGrouping = true;
            IsStandardMode = true;
            LastNumberBase = NumberBase.Decimal;
        }
    }
}
