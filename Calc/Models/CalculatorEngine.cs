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
        private bool _isNewCalculation;
        private bool _hasEqualsBeenPressed;

        public CalculatorEngine()
        {
            Clear();
        }

        public void Clear()
        {
            _currentValue = 0;
            _previousValue = 0;
            _currentOperation = string.Empty;
            _isNewCalculation = true;
            _hasEqualsBeenPressed = false;
        }

        public void ClearEntry()
        {
            _currentValue = 0;
            _isNewCalculation = true;
        }

        public string GetDisplayText(bool useDigitGrouping)
        {
            if (useDigitGrouping)
            {
                // Use the current culture's number formatting
                return _currentValue.ToString("N", CultureInfo.CurrentCulture);
            }
            else
            {
                // Display without digit grouping
                return _currentValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        public void SetInput(string input)
        {
            if (_isNewCalculation || _hasEqualsBeenPressed)
            {
                _currentValue = double.Parse(input);
                _isNewCalculation = false;
                _hasEqualsBeenPressed = false;
            }
            else
            {
                _currentValue = double.Parse(_currentValue.ToString() + input);
            }
        }

        public void ProcessOperation(string operation)
        {
            if (!string.IsNullOrEmpty(_currentOperation) && !_isNewCalculation)
            {
                CalculateResult();
            }

            _previousValue = _currentValue;
            _currentOperation = operation;
            _isNewCalculation = true;
            _hasEqualsBeenPressed = false;
        }

        public void CalculateResult()
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
                    {
                        throw new DivideByZeroException("Cannot divide by zero");
                    }
                    _currentValue = _previousValue / _currentValue;
                    break;
                case "%":
                    _currentValue = _previousValue % _currentValue;
                    break;
            }

            _isNewCalculation = true;
            _hasEqualsBeenPressed = true;
        }

        public void SquareRoot()
        {
            if (_currentValue < 0)
            {
                throw new InvalidOperationException("Cannot calculate square root of a negative number");
            }

            _currentValue = Math.Sqrt(_currentValue);
            _isNewCalculation = true;
        }

        public void Square()
        {
            _currentValue = Math.Pow(_currentValue, 2);
            _isNewCalculation = true;
        }

        public void Reciprocal()
        {
            if (_currentValue == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }

            _currentValue = 1 / _currentValue;
            _isNewCalculation = true;
        }

        public void Negate()
        {
            _currentValue = -_currentValue;
        }

        public double GetCurrentValue()
        {
            return _currentValue;
        }

        public void SetCurrentValue(double value)
        {
            _currentValue = value;
            _isNewCalculation = false;
        }
    }
}
