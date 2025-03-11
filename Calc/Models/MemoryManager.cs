using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class MemoryManager
    {
        private double _memoryValue;
        private List<double> _memoryStack;

        public MemoryManager()
        {
            _memoryValue = 0;
            _memoryStack = new List<double>();
        }

        public void MemoryClear()
        {
            _memoryValue = 0;
            _memoryStack.Clear();
        }

        public double MemoryRecall()
        {
            return _memoryValue;
        }

        public void MemoryStore(double value)
        {
            _memoryValue = value;
            if (!_memoryStack.Contains(value))
            {
                _memoryStack.Add(value);
            }
        }

        public void MemoryAdd(double value)
        {
            _memoryValue += value;
            if (!_memoryStack.Contains(_memoryValue))
            {
                _memoryStack.Add(_memoryValue);
            }
        }

        public void MemorySubtract(double value)
        {
            _memoryValue -= value;
            if (!_memoryStack.Contains(_memoryValue))
            {
                _memoryStack.Add(_memoryValue);
            }
        }

        public bool HasMemory()
        {
            return _memoryValue != 0 || _memoryStack.Count > 0;
        }

        public List<double> GetMemoryStack()
        {
            return new List<double>(_memoryStack);
        }
    }
}
