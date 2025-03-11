using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Models;
using System.Windows.Data;

namespace Calc.Converters
{
    public class BaseEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            if (parameter is NumberBase paramBase && value is NumberBase selectedBase)
            {
                return selectedBase == paramBase;
            }

            if (parameter is string || parameter is int)
            {
                int digit;
                if (parameter is string)
                    int.TryParse(parameter.ToString(), out digit);
                else
                    digit = (int)parameter;

                if (value is NumberBase currentBase)
                {
                    switch (currentBase)
                    {
                        case NumberBase.Binary:
                            return digit <= 1;
                        case NumberBase.Octal:
                            return digit <= 7;
                        case NumberBase.Decimal:
                            return digit <= 9;
                        case NumberBase.Hexadecimal:
                            return true;
                    }
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
