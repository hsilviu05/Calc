using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class CalculatorEngine
    {
        private double _currentValue;
        private double _previousValue;
        private string _currentOperation;
        private bool _newNumber;
        private string _currentInput;
        private bool _useOperationPrecedence;
        private List<double> _values;
        private List<string> _operations;

        public bool UseOperationPrecedence
        {
            get => _useOperationPrecedence;
            set => _useOperationPrecedence = value;
        }

        public CalculatorEngine()
        {
            Clear();
        }

        public void Clear()
        {
            _currentValue = 0;
            _previousValue = 0;
            _currentOperation = "";
            _newNumber = true;
            _currentInput = "0";
            _values = new List<double>();
            _operations = new List<string>();
        }

        public void ClearEntry()
        {
            _currentValue = 0;
            _currentInput = "0";
            _newNumber = true;
        }

        public void SetInput(string digit)
        {
            try
            {
                if (digit == "." && _currentInput.Contains("."))
                    return;

                if (_newNumber)
                {
                    _currentInput = digit == "." ? "0." : digit;
                    _newNumber = false;
                }
                else
                {
                    _currentInput += digit;
                }

                if (double.TryParse(_currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    _currentValue = result;
                }
                else
                {
                    throw new FormatException("Invalid number format");
                }
            }
            catch (Exception)
            {
                if (_newNumber)
                {
                    _currentInput = "0";
                    _currentValue = 0;
                }
                throw;
            }
        }

        public void ProcessOperation(string operation)
        {
            if (!_useOperationPrecedence)
            {
                if (!string.IsNullOrEmpty(_currentOperation))
                {
                    CalculateResult();
                }
                _previousValue = _currentValue;
                _currentOperation = operation;
                _newNumber = true;
            }
            else
            {
                _values.Add(_currentValue);
                _operations.Add(operation);
                _newNumber = true;
            }
        }

        public void CalculateResult()
        {
            if (_useOperationPrecedence)
            {
                CalculateWithPrecedence();
            }
            else
            {
                CalculateSimple();
            }
            _currentOperation = "";
            _newNumber = true;
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
        }

        private void CalculateSimple()
        {
            switch (_currentOperation)
            {
                case "+":
                    _currentValue = _previousValue + _currentValue;
                    break;
                case "-":
                    _currentValue = _previousValue - _currentValue;
                    break;
                case "*":
                    _currentValue = _previousValue * _currentValue;
                    break;
                case "/":
                    if (_currentValue == 0)
                        throw new DivideByZeroException("Cannot divide by zero");
                    _currentValue = _previousValue / _currentValue;
                    break;
                case "%":
                    _currentValue = _previousValue * (_currentValue / 100);
                    break;
            }
        }

        private void CalculateWithPrecedence()
        {
            if (_values.Count == 0)
                return;

            _values.Add(_currentValue);

            for (int i = 0; i < _operations.Count; i++)
            {
                if (_operations[i] == "*" || _operations[i] == "/" || _operations[i] == "%")
                {
                    double result;
                    switch (_operations[i])
                    {
                        case "*":
                            result = _values[i] * _values[i + 1];
                            break;
                        case "/":
                            if (_values[i + 1] == 0)
                                throw new DivideByZeroException("Cannot divide by zero");
                            result = _values[i] / _values[i + 1];
                            break;
                        case "%":
                            result = _values[i] * (_values[i + 1] / 100);
                            break;
                        default:
                            continue;
                    }
                    _values[i] = result;
                    _values.RemoveAt(i + 1);
                    _operations.RemoveAt(i);
                    i--;
                }
            }

            _currentValue = _values[0];
            for (int i = 0; i < _operations.Count; i++)
            {
                switch (_operations[i])
                {
                    case "+":
                        _currentValue += _values[i + 1];
                        break;
                    case "-":
                        _currentValue -= _values[i + 1];
                        break;
                }
            }

            _values.Clear();
            _operations.Clear();
        }

        public string GetDisplayText(bool useDigitGrouping)
        {
            if (useDigitGrouping)
            {
                return double.Parse(_currentInput).ToString("N", CultureInfo.CurrentCulture);
            }
            return _currentInput;
        }

        public double GetCurrentValue()
        {
            return _currentValue;
        }

        public void SetCurrentValue(double value)
        {
            _currentValue = value;
            _currentInput = value.ToString(CultureInfo.InvariantCulture);
            _newNumber = true;
        }

        public void Negate()
        {
            _currentValue = -_currentValue;
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
        }

        public void SquareRoot()
        {
            if (_currentValue < 0)
                throw new ArgumentException("Cannot calculate square root of a negative number");
            _currentValue = Math.Sqrt(_currentValue);
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
            _newNumber = true;
        }

        public void Square()
        {
            _currentValue = Math.Pow(_currentValue, 2);
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
            _newNumber = true;
        }

        public void Reciprocal()
        {
            if (_currentValue == 0)
                throw new DivideByZeroException("Cannot divide by zero");
            _currentValue = 1 / _currentValue;
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
            _newNumber = true;
        }

        public void Backspace()
        {
            if (_newNumber)
                return;

            if (_currentInput.Length > 1)
            {
                _currentInput = _currentInput.Substring(0, _currentInput.Length - 1);
                if (double.TryParse(_currentInput, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
                {
                    _currentValue = result;
                }
                else
                {
                    _currentInput = "0";
                    _currentValue = 0;
                    _newNumber = true;
                }
            }
            else
            {
                _currentInput = "0";
                _currentValue = 0;
                _newNumber = true;
            }
        }

        public void Percentage()
        {
            _currentValue = _currentValue / 100;
            _currentInput = _currentValue.ToString(CultureInfo.InvariantCulture);
            _newNumber = true;
        }
    }
}
