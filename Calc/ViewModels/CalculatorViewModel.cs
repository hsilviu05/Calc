using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Calc.Helpers;
using Calc.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Calc.Views;

namespace Calc.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorEngine _calculatorEngine;
        private MemoryManager _memoryManager;
        private NumberBaseConverter _baseConverter;
        private string _display;
        private bool _useDigitGrouping;
        private bool _isStandardMode;
        private bool _isProgrammerMode;
        private NumberBase _selectedBase;
        private bool _useOperationPrecedence;
        private MemoryValue _selectedMemoryValue;

        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged();
            }
        }

        public bool UseDigitGrouping
        {
            get => _useDigitGrouping;
            set
            {
                _useDigitGrouping = value;
                UpdateDisplay();
                OnPropertyChanged();
            }
        }

        public bool IsStandardMode
        {
            get => _isStandardMode;
            set
            {
                _isStandardMode = value;
                OnPropertyChanged();
            }
        }

        public bool IsProgrammerMode
        {
            get => _isProgrammerMode;
            set
            {
                _isProgrammerMode = value;
                OnPropertyChanged();
            }
        }

        public NumberBase SelectedBase
        {
            get => _selectedBase;
            set
            {
                _selectedBase = value;
                _baseConverter.CurrentBase = value;
                UpdateDisplay();
                OnPropertyChanged();
            }
        }

        public bool UseOperationPrecedence
        {
            get => _useOperationPrecedence;
            set
            {
                _useOperationPrecedence = value;
                _calculatorEngine.UseOperationPrecedence = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MemoryValue> MemoryValues => _memoryManager.MemoryValues;

        public MemoryValue SelectedMemoryValue
        {
            get => _selectedMemoryValue;
            set
            {
                _selectedMemoryValue = value;
                OnPropertyChanged();
            }
        }

        public ICommand DigitCommand { get; private set; }
        public ICommand OperationCommand { get; private set; }
        public ICommand EqualsCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand ClearEntryCommand { get; private set; }
        public ICommand BackspaceCommand { get; private set; }
        public ICommand MemoryClearCommand { get; private set; }
        public ICommand MemoryRecallCommand { get; private set; }
        public ICommand MemoryStoreCommand { get; private set; }
        public ICommand MemoryAddCommand { get; private set; }
        public ICommand MemorySubtractCommand { get; private set; }
        public ICommand SquareRootCommand { get; private set; }
        public ICommand SquareCommand { get; private set; }
        public ICommand ReciprocalCommand { get; private set; }
        public ICommand NegateCommand { get; private set; }
        public ICommand PercentageCommand { get; private set; }
        public ICommand SwitchToStandardModeCommand { get; private set; }
        public ICommand SwitchToProgrammerModeCommand { get; private set; }
        public ICommand ChangeBaseCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand ShowMemoryListCommand { get; private set; }
        public ICommand UseMemoryValueCommand { get; private set; }

        public CalculatorViewModel()
        {
            _calculatorEngine = new CalculatorEngine();
            _memoryManager = new MemoryManager();
            _baseConverter = new NumberBaseConverter();

            LoadSettings();

            UpdateDisplay();

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            DigitCommand = new RelayCommand<string>(ExecuteDigitCommand);
            OperationCommand = new RelayCommand<string>(ExecuteOperationCommand);
            EqualsCommand = new RelayCommand(ExecuteEqualsCommand);
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            ClearEntryCommand = new RelayCommand(ExecuteClearEntryCommand);
            BackspaceCommand = new RelayCommand(ExecuteBackspaceCommand);
            MemoryClearCommand = new RelayCommand(ExecuteMemoryClearCommand);
            MemoryRecallCommand = new RelayCommand(ExecuteMemoryRecallCommand);
            MemoryStoreCommand = new RelayCommand(ExecuteMemoryStoreCommand);
            MemoryAddCommand = new RelayCommand(ExecuteMemoryAddCommand);
            MemorySubtractCommand = new RelayCommand(ExecuteMemorySubtractCommand);
            SquareRootCommand = new RelayCommand(ExecuteSquareRootCommand);
            SquareCommand = new RelayCommand(ExecuteSquareCommand);
            ReciprocalCommand = new RelayCommand(ExecuteReciprocalCommand);
            NegateCommand = new RelayCommand(ExecuteNegateCommand);
            PercentageCommand = new RelayCommand(ExecutePercentageCommand);
            SwitchToStandardModeCommand = new RelayCommand(ExecuteSwitchToStandardModeCommand);
            SwitchToProgrammerModeCommand = new RelayCommand(ExecuteSwitchToProgrammerModeCommand);
            ChangeBaseCommand = new RelayCommand<NumberBase>(ExecuteChangeBaseCommand);
            CutCommand = new RelayCommand(ExecuteCutCommand);
            CopyCommand = new RelayCommand(ExecuteCopyCommand);
            PasteCommand = new RelayCommand(ExecutePasteCommand);
            ShowMemoryListCommand = new RelayCommand(ExecuteShowMemoryListCommand);
            UseMemoryValueCommand = new RelayCommand(ExecuteUseMemoryValueCommand);
        }

        private void LoadSettings()
        {
            Settings settings = SettingsManager.GetSettings();

            UseDigitGrouping = settings.UseDigitGrouping;
            IsStandardMode = settings.IsStandardMode;
            IsProgrammerMode = !settings.IsStandardMode;
            SelectedBase = settings.LastNumberBase;
            UseOperationPrecedence = settings.UseOperationPrecedence;
            _baseConverter.CurrentBase = settings.LastNumberBase;
        }

        private void SaveSettings()
        {
            Settings settings = new Settings
            {
                UseDigitGrouping = UseDigitGrouping,
                IsStandardMode = IsStandardMode,
                LastNumberBase = SelectedBase,
                UseOperationPrecedence = UseOperationPrecedence
            };

            SettingsManager.SetSettings(settings);
        }

        private void UpdateDisplay()
        {
            try
            {
                if (IsProgrammerMode && _selectedBase != NumberBase.Decimal)
                {
                    string baseValue = _baseConverter.ConvertToBase(_calculatorEngine.GetCurrentValue(), _selectedBase);
                    Display = baseValue;
                }
                else
                {
                    Display = _calculatorEngine.GetDisplayText(UseDigitGrouping);
                }
            }
            catch (Exception ex)
            {
                Display = "Error";
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Command Execution Methods

        private void ExecuteDigitCommand(string digit)
        {
            try
            {
                if (IsProgrammerMode && _selectedBase != NumberBase.Decimal)
                {
                    if (_baseConverter.IsValidDigitForBase(digit[0], _selectedBase))
                    {
                        if (_calculatorEngine.IsNewNumber)
                        {
                            double newDigitValue = _baseConverter.ConvertFromBase(digit, _selectedBase);
                            _calculatorEngine.SetCurrentValue(newDigitValue);
                            Display = digit;
                        }
                        else
                        {
                            double currentValue = _calculatorEngine.GetCurrentValue();
                            double newDigitValue = _baseConverter.ConvertFromBase(digit, _selectedBase);
                            double newValue = currentValue * (int)_selectedBase + newDigitValue;
                            _calculatorEngine.SetCurrentValue(newValue);
                            Display = _baseConverter.ConvertToBase(newValue, _selectedBase);
                        }
                    }
                }
                else
                {
                    _calculatorEngine.SetInput(digit);
                    UpdateDisplay();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteOperationCommand(string operation)
        {
            try
            {
                _calculatorEngine.ProcessOperation(operation);
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteEqualsCommand()
        {
            try
            {
                _calculatorEngine.CalculateResult();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteClearCommand()
        {
            _calculatorEngine.Clear();
            UpdateDisplay();
        }

        private void ExecuteClearEntryCommand()
        {
            _calculatorEngine.ClearEntry();
            UpdateDisplay();
        }

        private void ExecuteBackspaceCommand()
        {
            string currentDisplay = Display;

            if (currentDisplay.Length > 1)
            {
                currentDisplay = currentDisplay.Substring(0, currentDisplay.Length - 1);

                try
                {
                    if (IsProgrammerMode && _selectedBase != NumberBase.Decimal)
                    {
                        Display = currentDisplay;
                        double decimalValue = _baseConverter.ConvertFromBase(Display, _selectedBase);
                        _calculatorEngine.SetCurrentValue(decimalValue);
                    }
                    else
                    {
                        double newValue = double.Parse(currentDisplay);
                        _calculatorEngine.SetCurrentValue(newValue);
                        UpdateDisplay();
                    }
                }
                catch
                {
                    ExecuteClearEntryCommand();
                }
            }
            else
            {
                ExecuteClearEntryCommand();
            }
        }

        private void ExecuteMemoryClearCommand()
        {
            _memoryManager.Clear();
            OnPropertyChanged(nameof(MemoryValues));
        }

        private void ExecuteMemoryRecallCommand()
        {
            if (_memoryManager.HasMemory())
            {
                _calculatorEngine.SetCurrentValue(_memoryManager.Recall());
                UpdateDisplay();
            }
        }

        private void ExecuteMemoryStoreCommand()
        {
            _memoryManager.Store(_calculatorEngine.GetCurrentValue());
            OnPropertyChanged(nameof(MemoryValues));
        }

        private void ExecuteMemoryAddCommand()
        {
            _memoryManager.Add(_calculatorEngine.GetCurrentValue());
            OnPropertyChanged(nameof(MemoryValues));
        }

        private void ExecuteMemorySubtractCommand()
        {
            _memoryManager.Subtract(_calculatorEngine.GetCurrentValue());
            OnPropertyChanged(nameof(MemoryValues));
        }

        private void ExecuteSquareRootCommand()
        {
            try
            {
                _calculatorEngine.SquareRoot();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteSquareCommand()
        {
            try
            {
                _calculatorEngine.Square();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                Display = "Error";
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteReciprocalCommand()
        {
            try
            {
                _calculatorEngine.Reciprocal();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteNegateCommand()
        {
            try
            {
                _calculatorEngine.Negate();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                Display = "Error";
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecutePercentageCommand()
        {
            try
            {
                _calculatorEngine.Percentage();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void ExecuteSwitchToStandardModeCommand()
        {
            IsStandardMode = true;
            IsProgrammerMode = false;
            SaveSettings();
        }

        private void ExecuteSwitchToProgrammerModeCommand()
        {
            IsStandardMode = false;
            IsProgrammerMode = true;
            SaveSettings();
        }

        private void ExecuteChangeBaseCommand(NumberBase baseType)
        {
            SelectedBase = baseType;
            SaveSettings();
        }

        private void ExecuteCutCommand()
        {
            try
            {
                Clipboard.SetText(Display);
                _calculatorEngine.Clear();
                UpdateDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to cut: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCopyCommand()
        {
            try
            {
                Clipboard.SetText(Display);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to copy: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecutePasteCommand()
        {
            try
            {
                string clipboardText = Clipboard.GetText();

                if (string.IsNullOrEmpty(clipboardText))
                    return;

                if (IsProgrammerMode && _selectedBase != NumberBase.Decimal)
                {
                    bool isValid = true;
                    foreach (char c in clipboardText)
                    {
                        if (!_baseConverter.IsValidDigitForBase(c, _selectedBase))
                        {
                            isValid = false;
                            break;
                        }
                    }

                    if (isValid)
                    {
                        Display = clipboardText;
                        double decimalValue = _baseConverter.ConvertFromBase(clipboardText, _selectedBase);
                        _calculatorEngine.SetCurrentValue(decimalValue);
                    }
                    else
                    {
                        MessageBox.Show("Invalid number format for the selected base", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    if (double.TryParse(clipboardText, out double value))
                    {
                        _calculatorEngine.SetCurrentValue(value);
                        UpdateDisplay();
                    }
                    else
                    {
                        MessageBox.Show("Invalid number format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to paste: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteShowMemoryListCommand()
        {
            var memoryListView = new MemoryListView
            {
                DataContext = this,
                Owner = Application.Current.MainWindow
            };
            memoryListView.ShowDialog();
        }

        private void ExecuteUseMemoryValueCommand()
        {
            if (SelectedMemoryValue != null)
            {
                _calculatorEngine.SetCurrentValue(SelectedMemoryValue.Value);
                UpdateDisplay();
            }
        }

        #endregion

        public void HandleKeyInput(Key key)
        {
            try
            {
                if ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9))
                {
                    string digit = key.ToString().Last().ToString();
                    ExecuteDigitCommand(digit);
                }
                else if (key == Key.Decimal || key == Key.OemPeriod)
                {
                    ExecuteDigitCommand(".");
                }
                else if (key == Key.Add || (key == Key.OemPlus && Keyboard.Modifiers == ModifierKeys.Shift))
                {
                    ExecuteOperationCommand("+");
                }
                else if (key == Key.Subtract || key == Key.OemMinus)
                {
                    ExecuteOperationCommand("-");
                }
                else if (key == Key.Multiply)
                {
                    ExecuteOperationCommand("*");
                }
                else if (key == Key.Divide || key == Key.OemQuestion)
                {
                    ExecuteOperationCommand("/");
                }
                else if (key == Key.Back)
                {
                    ExecuteBackspaceCommand();
                }
                else if (key == Key.Delete)
                {
                    ExecuteClearCommand();
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }

        private void HandleError(Exception ex)
        {
            string message;
            if (ex is DivideByZeroException)
            {
                message = "Cannot divide by zero";
            }
            else if (ex is FormatException)
            {
                message = "Invalid number format";
            }
            else if (ex is ArgumentException)
            {
                message = ex.Message;
            }
            else
            {
                message = "An error occurred";
            }

            Display = "Error";
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
