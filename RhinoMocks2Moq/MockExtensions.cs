﻿namespace Rhino.Mocks
{
    using System;
    using System.Linq.Expressions;
    using Moq;

    public static class MockExtensions
{
        public static IExpect<T, TR> Expect<T, TR>(this T obj, Expression<Func<T, TR>> expression) where T : class
        {
            return Stub(obj, expression);
        }

        public static IExpect<T> Expect<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            return Stub(obj, expression);
        }

        public static IExpect<T, TR> Stub<T, TR>(this T obj, Expression<Func<T, TR>> expression) where T : class
        {
            return new Expect<T, TR>(MockRepository.Get(obj), expression);
        }

        public static IExpect<T> Stub<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            return new Expect<T>(MockRepository.Get(obj), expression);
        }

        public static void AssertWasNotCalled<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            MockRepository.Get(obj).Verify(expression, Times.Never);
        }

        public static void AssertWasNotCalled<T>(this T obj, Expression<Action<T>> expression, Func<RepeatAdapter, Times> timesDelegate) where T : class
        {
            MockRepository.Get(obj).Verify(expression, timesDelegate(new RepeatAdapter()));
        }

        public static void AssertWasCalled<T>(this T obj, Expression<Action<T>> expression) where T : class
        {
            MockRepository.Get(obj).Verify(expression, Times.AtLeastOnce);
        }

        public static void AssertWasCalled<T>(this T obj, Expression<Action<T>> expression, Func<RepeatAdapter, Times> timesDelegate) where T : class
        {
             MockRepository.Get(obj).Verify(expression, timesDelegate(new RepeatAdapter()));
        }

        public static void VerifyAllExpectations<T>(this T obj) where T : class
        {
            MockRepository.Get(obj).Verify();
        }
    }
}
