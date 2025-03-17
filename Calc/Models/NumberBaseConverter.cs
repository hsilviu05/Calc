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
            bool isNegative = intValue < 0;
            intValue = Math.Abs(intValue);

            string result;
            switch (targetBase)
            {
                case NumberBase.Decimal:
                    result = intValue.ToString();
                    break;

                case NumberBase.Hexadecimal:
                    result = Convert.ToString(intValue, 16).ToUpper();
                    break;

                case NumberBase.Octal:
                    result = Convert.ToString(intValue, 8);
                    break;

                case NumberBase.Binary:
                    result = Convert.ToString(intValue, 2);
                    break;

                default:
                    throw new ArgumentException("Invalid number base");
            }

            return isNegative ? "-" + result : result;
        }

        public double ConvertFromBase(string value, NumberBase sourceBase)
        {
            try
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                bool isNegative = value.StartsWith("-");
                string absValue = isNegative ? value.Substring(1) : value;

                long result;
                switch (sourceBase)
                {
                    case NumberBase.Decimal:
                        result = long.Parse(absValue);
                        break;

                    case NumberBase.Hexadecimal:
                        result = Convert.ToInt64(absValue, 16);
                        break;

                    case NumberBase.Octal:
                        result = Convert.ToInt64(absValue, 8);
                        break;

                    case NumberBase.Binary:
                        result = Convert.ToInt64(absValue, 2);
                        break;

                    default:
                        throw new ArgumentException("Invalid number base");
                }

                return isNegative ? -result : result;
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException)
            {
                throw new FormatException($"Invalid number format for {sourceBase} base");
            }
        }

        public bool IsValidDigitForBase(char digit, NumberBase baseType)
        {
            if (digit == '-')
                return true;

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
