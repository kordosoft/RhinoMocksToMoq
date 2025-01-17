﻿namespace RhinoMocksToMoq.Tests
{
    using Xunit;
    using Rhino.Mocks;
    using System.Collections.Generic;

    public sealed class FunctionTests
    {
        [Fact]
        public void Method()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3);

            var sum = new CalculatorService(calculator).Add(1, 2);

            Assert.Equal(3, sum);
            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void Once()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Once();

            var sum = new CalculatorService(calculator).Add(1, 2);

            Assert.Equal(3, sum);
            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void Twice()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Twice();

            var calculatorService = new CalculatorService(calculator);
            var sum = calculatorService.Add(1, 2);
            var sum2 = calculatorService.Add(1, 2);

            Assert.Equal(3, sum);
            Assert.Equal(3, sum2);
            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Any(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Any();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
            }
            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void AtLeastOnce(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.AtLeastOnce();

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
            }

            if (times == 0)
            {
                Assert.Throws<Moq.MockException>(() => calculator.VerifyAllExpectations());
            }
            else
            {
                calculator.VerifyAllExpectations();
            }
        }

        [Fact]
        public void Never()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Never();

            var calculatorService = new CalculatorService(calculator);

            calculator.VerifyAllExpectations();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public void Times(int times)
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(1, 2)).Return(3).Repeat.Times(times);

            var calculatorService = new CalculatorService(calculator);
            for (var i = 0; i < times; i++)
            {
                var sum = calculatorService.Add(1, 2);
                Assert.Equal(3, sum);
            }

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ReturnInOrder()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Random()).ReturnInOrder(new int[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(1, calculatorService.Random());
            Assert.Equal(2, calculatorService.Random());
            Assert.Equal(3, calculatorService.Random());
            Assert.Equal(4, calculatorService.Random());

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void IgnoreArguments()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(0, 0))
                .IgnoreArguments()
                .ReturnInOrder(new int?[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(1, calculatorService.Add(0, 0));
            Assert.Equal(2, calculatorService.Add(0, 1));
            Assert.Equal(3, calculatorService.Add(0, 2));
            Assert.Equal(4, calculatorService.Add(0, 3));

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ArgsMatchers()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(Arg<int>.Matches(x => x > 2), Arg<int>.Is.Equal(2)))
                .ReturnInOrder(new int?[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(1, calculatorService.Add(4, 2));
            Assert.Equal(2, calculatorService.Add(3, 2));
            Assert.Equal(0, calculatorService.Add(5, 1));
            Assert.Equal(0, calculatorService.Add(0, 2));

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ArgsIsAnything()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(Arg<int>.Matches(x => x > 2), Arg<int>.Is.Anything))
                .ReturnInOrder(new int?[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(1, calculatorService.Add(4, -1));
            Assert.Equal(2, calculatorService.Add(3, 2));
            Assert.Equal(3, calculatorService.Add(5, default));

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ArgsIsNotNull()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(Arg<int>.Is.NotNull, 2))
                .ReturnInOrder(new int?[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(1, calculatorService.Add(1, 2));
            Assert.Equal(2, calculatorService.Add(2, 2));
            Assert.Equal(0, calculatorService.Add(null, 2));
            Assert.Equal(3, calculatorService.Add(3, 2));

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void ArgsIsNull()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.Add(Arg<int?>.Is.Null, 2))
                .ReturnInOrder(new int?[] { 1, 2, 3, 4 });

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(0, calculatorService.Add(1, 2));
            Assert.Equal(1, calculatorService.Add(null, 2));
            Assert.Equal(2, calculatorService.Add(null, 2));
            Assert.Equal(0, calculatorService.Add(3, 2));
            Assert.Equal(0, calculatorService.Add(null, 3));

            calculator.VerifyAllExpectations();
        }


        [Fact]
        public void ArgListContainsAll()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })))
                .Return(5);

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3 }));
            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3, 4 }));
            Assert.Equal(0, calculatorService.SumAll(new List<int> { 1, 2, 4 }));

            calculator.VerifyAllExpectations();
        }

        [Fact]
        public void AssertWasCalled()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })))
                .Return(5);

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3 }));
            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3, 4 }));
            Assert.Equal(0, calculatorService.SumAll(new List<int> { 1, 2, 4 }));

            calculator.AssertWasCalled(x => x.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })));
        }

        [Fact]
        public void AssertWasCalledTwice()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })))
                .Return(5);

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3 }));
            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3, 4 }));
            Assert.Equal(0, calculatorService.SumAll(new List<int> { 1, 2, 4 }));

            calculator.AssertWasCalled(x => x.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })), x => x.Repeat.Twice());
        }

        [Fact]
        public void AssertWasNotCalled()
        {
            var calculator = MockRepository.GenerateMock<ICalculator>();
            calculator.Expect(cal => cal.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 1, 2, 3 })))
                .Return(5);

            var calculatorService = new CalculatorService(calculator);

            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3 }));
            Assert.Equal(5, calculatorService.SumAll(new List<int> { 1, 2, 3, 4 }));
            Assert.Equal(0, calculatorService.SumAll(new List<int> { 1, 2, 4 }));

            calculator.AssertWasNotCalled(x => x.SumAll(Arg<List<int>>.List.ContainsAll(new List<int> { 0, 1, 2 })));
        }

        [Fact]
        public void ReturnNull ()
        {
	        var calculator = MockRepository.GenerateMock<ICalculator>();
	        calculator.Expect(cal => cal.Add(Arg<int?>.Is.Anything, Arg<int>.Is.Anything))
		        .Return(null);

	        var calculatorService = new CalculatorService(calculator);

	        Assert.Null(calculatorService.Add(2, 3));
	        Assert.Null(calculatorService.Add(2, 3));

            calculator.VerifyAllExpectations();
        }
    }
}
