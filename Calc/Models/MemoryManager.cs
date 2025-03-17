using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Models
{
    public class MemoryValue
    {
        public double Value { get; set; }
    }

    public class MemoryManager
    {
        private ObservableCollection<MemoryValue> _memoryValues;
        private double _currentMemoryValue;

        public ObservableCollection<MemoryValue> MemoryValues => _memoryValues;

        public MemoryManager()
        {
            _memoryValues = new ObservableCollection<MemoryValue>();
            _currentMemoryValue = 0;
        }

        public void Clear()
        {
            _currentMemoryValue = 0;
            _memoryValues.Clear();
        }

        public double Recall()
        {
            return _currentMemoryValue;
        }

        public void Store(double value)
        {
            _currentMemoryValue = value;
            _memoryValues.Insert(0, new MemoryValue { Value = value });
        }

        public void Add(double value)
        {
            _currentMemoryValue += value;
            if (_memoryValues.Any())
            {
                _memoryValues[0].Value = _currentMemoryValue;
            }
            else
            {
                Store(_currentMemoryValue);
            }
        }

        public void Subtract(double value)
        {
            _currentMemoryValue -= value;
            if (_memoryValues.Any())
            {
                _memoryValues[0].Value = _currentMemoryValue;
            }
            else
            {
                Store(_currentMemoryValue);
            }
        }

        public bool HasMemory()
        {
            return _memoryValues.Any();
        }

        public List<double> GetMemoryStack()
        {
            return _memoryValues.Select(mv => mv.Value).ToList();
        }
    }
}
