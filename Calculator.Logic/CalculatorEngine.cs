using System;
using System.Globalization;

namespace Calculator.Logic
{
    public class CalculatorEngine
    {
        private double? _previousResult;
        private char? _lastOperation;
        private double? _lastOperand;
        private string _currentInput = "0";
        private bool _isNewNumber = true;
        private bool _isError = false;
        private bool _isAfterCompute = false;

        public string DisplayText => _isError ? "Ошибка: деление на ноль" : _currentInput;
        public bool IsError => _isError;

        public void Clear()
        {
            _previousResult = null;
            _lastOperation = null;
            _lastOperand = null;
            _currentInput = "0";
            _isNewNumber = true;
            _isError = false;
            _isAfterCompute = false;
        }

        public void EnterDigit(string digit)
        {
            if (_isError) return;

            var culture = CultureInfo.CurrentCulture;
            var decimalSeparator = culture.NumberFormat.NumberDecimalSeparator;

            if (_isNewNumber)
            {
                _currentInput = digit == decimalSeparator ? "0" + decimalSeparator : digit;
                _isNewNumber = false;
            }
            else
            {
                if (digit == decimalSeparator && _currentInput.Contains(decimalSeparator))
                    return;
                
                if (_currentInput == "0" && digit != decimalSeparator)
                    _currentInput = digit;
                else
                    _currentInput += digit;
            }
        }

        public void SetOperation(char operation)
        {
            if (_isError) return;

            if (_isAfterCompute && _isNewNumber)
            {
                // Нажали операцию сразу после "=", продолжаем цепочку с текущим результатом
            }
            else if (_isAfterCompute && !_isNewNumber)
            {
                // Ввели новое число после "=", а потом нажали операцию -> начинаем всё заново с этого числа
                _previousResult = double.Parse(_currentInput, CultureInfo.CurrentCulture);
                _lastOperand = null;
                _isAfterCompute = false;
            }
            else if (!_isNewNumber)
            {
                // Обычная цепочка (без предшествующего "=")
                if (_previousResult.HasValue && _lastOperation.HasValue)
                {
                    Compute();
                }
                else
                {
                    _previousResult = double.Parse(_currentInput, CultureInfo.CurrentCulture);
                }
                _lastOperand = double.Parse(_currentInput, CultureInfo.CurrentCulture);
            }
            else
            {
                // Смена операции без ввода нового числа
                _previousResult = double.Parse(_currentInput, CultureInfo.CurrentCulture);
                _lastOperand = null;
            }

            _lastOperation = operation;
            _isNewNumber = true;
        }

        public void Compute()
        {
            if (_isError) return;

            if (_isNewNumber && _lastOperation.HasValue && _lastOperand.HasValue && _previousResult.HasValue)
            {
                RepeatLastOperation();
                return;
            }

            if (!_previousResult.HasValue || !_lastOperation.HasValue)
                return;

            double current = double.Parse(_currentInput, CultureInfo.CurrentCulture);
            double result = 0;

            switch (_lastOperation)
            {
                case '+': result = _previousResult.Value + current; break;
                case '-': result = _previousResult.Value - current; break;
                case '*': result = _previousResult.Value * current; break;
                case '/':
                    if (current == 0)
                    {
                        _isError = true;
                        _previousResult = null;
                        _lastOperation = null;
                        _lastOperand = null;
                        return;
                    }
                    result = _previousResult.Value / current;
                    break;
            }

            _previousResult = result;
            _lastOperand = current;
            _currentInput = result.ToString(CultureInfo.CurrentCulture);
            _isNewNumber = true;
            _isAfterCompute = true;
        }

        public void RepeatLastOperation()
        {
            if (_isError || !_lastOperation.HasValue || !_lastOperand.HasValue || !_previousResult.HasValue)
                return;

            double result = 0;
            double prev = _previousResult.Value;
            double operand = _lastOperand.Value;

            switch (_lastOperation)
            {
                case '+': result = prev + operand; break;
                case '-': result = prev - operand; break;
                case '*': result = prev * operand; break;
                case '/':
                    if (operand == 0)
                    {
                        _isError = true;
                        return;
                    }
                    result = prev / operand;
                    break;
            }

            _previousResult = result;
            _currentInput = result.ToString(CultureInfo.CurrentCulture);
            _isNewNumber = true;
            _isAfterCompute = true;
        }
    }
}