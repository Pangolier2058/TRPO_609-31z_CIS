using Microsoft.VisualStudio.TestTools.UnitTesting;
using Calculator.Logic;
using System.Globalization;

namespace Calculator.Tests
{
    [TestClass]
    public class CalculatorEngineTests
    {
        private CalculatorEngine _engine = null!;

        // Этот метод выполняется перед каждым тестом, чтобы создать чистый калькулятор
        [TestInitialize]
        public void Setup()
        {
            _engine = new CalculatorEngine();
        }

        [TestMethod]
        public void TestBasicAddition()
        {
            _engine.EnterDigit("2");
            _engine.SetOperation('+');
            _engine.EnterDigit("3");
            _engine.Compute();
            
            Assert.AreEqual("5", _engine.DisplayText);
        }

        [TestMethod]
        public void TestSequentialCalculation()
        {
            // 2 + 3 = 5
            _engine.EnterDigit("2");
            _engine.SetOperation('+');
            _engine.EnterDigit("3");
            _engine.Compute(); 
            
            // Теперь к 5 прибавляем 4
            _engine.SetOperation('+'); 
            _engine.EnterDigit("4");
            _engine.Compute();
            
            Assert.AreEqual("9", _engine.DisplayText);
        }

        [TestMethod]
        public void TestRepeatLastOperation()
        {
            // 5 + 3 = 8
            _engine.EnterDigit("5");
            _engine.SetOperation('+');
            _engine.EnterDigit("3");
            _engine.Compute(); 
            Assert.AreEqual("8", _engine.DisplayText);
            
            // Нажимаем "=" еще раз (8 + 3 = 11)
            _engine.Compute(); 
            Assert.AreEqual("11", _engine.DisplayText);
            
            // И еще раз (11 + 3 = 14)
            _engine.Compute(); 
            Assert.AreEqual("14", _engine.DisplayText);
        }

        [TestMethod]
        public void TestDivisionByZero()
        {
            _engine.EnterDigit("5");
            _engine.SetOperation('/');
            _engine.EnterDigit("0");
            _engine.Compute();
            
            Assert.IsTrue(_engine.IsError);
            Assert.AreEqual("Ошибка: деление на ноль", _engine.DisplayText);
        }

        [TestMethod]
        public void TestClearAfterError()
        {
            // Вызываем ошибку
            _engine.EnterDigit("1");
            _engine.SetOperation('/');
            _engine.EnterDigit("0");
            _engine.Compute();
            
            // Сбрасываем
            _engine.Clear();
            
            Assert.IsFalse(_engine.IsError);
            Assert.AreEqual("0", _engine.DisplayText);
        }

        [TestMethod]
        public void TestDoubleDecimalSeparatorBlocked()
        {
            // Получаем системный разделитель (запятая или точка)
            var sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            
            _engine.EnterDigit("5");
            _engine.EnterDigit(sep);
            _engine.EnterDigit("2");
            _engine.EnterDigit(sep); // Вторая запятая должна быть проигнорирована
            
            Assert.AreEqual($"5{sep}2", _engine.DisplayText);
        }
    }
}