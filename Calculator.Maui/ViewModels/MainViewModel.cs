using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Calculator.Logic;

namespace Calculator.Maui.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly CalculatorEngine _engine;

        public MainViewModel()
        {
            _engine = new CalculatorEngine();
        }

        // Свойство, которое отображается на экране калькулятора
        private string _displayText = "0";
        public string DisplayText
        {
            get => _displayText;
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged();
                }
            }
        }

        // Команды для кнопок
        public ICommand EnterDigitCommand => new Command<string>(ExecuteEnterDigit);
        public ICommand SetOperationCommand => new Command<string>(ExecuteSetOperation);
        public ICommand ComputeCommand => new Command(ExecuteCompute);
        public ICommand ClearCommand => new Command(ExecuteClear);

        // Обработчики команд
        private void ExecuteEnterDigit(string digit)
        {
            _engine.EnterDigit(digit);
            DisplayText = _engine.DisplayText;
        }

        private void ExecuteSetOperation(string operation)
        {
            _engine.SetOperation(operation[0]);
            DisplayText = _engine.DisplayText;
        }

        private void ExecuteCompute()
        {
            _engine.Compute();
            DisplayText = _engine.DisplayText;
        }

        private void ExecuteClear()
        {
            _engine.Clear();
            DisplayText = _engine.DisplayText;
        }

        // Реализация INotifyPropertyChanged для обновления UI
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}