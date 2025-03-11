using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public enum NumberBase
    {
        Decimal = 10,
        Hexadecimal = 16,
        Octal = 8,
        Binary = 2
    }

    public class NumberBaseConverter
    {
        public NumberBase CurrentBase { get; set; }

        public NumberBaseConverter()
        {
            CurrentBase = NumberBase.Decimal;
        }

        public string ConvertToBase(double value, NumberBase targetBase)
        {
            long intValue = (long)value;

            switch (targetBase)
            {
                case NumberBase.Decimal:
                    return intValue.ToString();

                case NumberBase.Hexadecimal:
                    return Convert.ToString(intValue, 16).ToUpper();

                case NumberBase.Octal:
                    return Convert.ToString(intValue, 8);

                case NumberBase.Binary:
                    return Convert.ToString(intValue, 2);

                default:
                    throw new ArgumentException("Invalid number base");
            }
        }

        public double ConvertFromBase(string value, NumberBase sourceBase)
        {
            try
            {
                switch (sourceBase)
                {
                    case NumberBase.Decimal:
                        return double.Parse(value);

                    case NumberBase.Hexadecimal:
                        return Convert.ToInt64(value, 16);

                    case NumberBase.Octal:
                        return Convert.ToInt64(value, 8);

                    case NumberBase.Binary:
                        return Convert.ToInt64(value, 2);

                    default:
                        throw new ArgumentException("Invalid number base");
                }
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                throw new FormatException("Invalid number format for the selected base");
            }
        }

        public bool IsValidDigitForBase(char digit, NumberBase baseType)
        {
            switch (baseType)
            {
                case NumberBase.Decimal:
                    return char.IsDigit(digit);

                case NumberBase.Hexadecimal:
                    return char.IsDigit(digit) || (char.ToUpper(digit) >= 'A' && char.ToUpper(digit) <= 'F');

                case NumberBase.Octal:
                    return digit >= '0' && digit <= '7';

                case NumberBase.Binary:
                    return digit == '0' || digit == '1';

                default:
                    return false;
            }
        }
    }
}
