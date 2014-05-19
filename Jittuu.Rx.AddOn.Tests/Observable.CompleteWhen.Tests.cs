using System;
using System.Reactive.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jittuu.Rx.AddOn;

namespace Jittuu.Rx.AddOn.Tests
{
    [TestClass]
    public class ObservableCompleteWhenTest
    {
        private IObservable<string> _obs;

        [TestInitialize]
        public void Setup()
        {
            _obs = Observable.Return("pass");
        }

        [TestMethod]
        public void Complete_when_should_return_correct_sequences()
        {
            string valueWithoutException = "";
            string valueWithException = "";


            _obs.CompleteWhen(Ex => Ex is Exception).Subscribe(i => valueWithoutException = i);

            _obs.Do(i =>
            {
                throw new Exception();
            }).CompleteWhen(Ex => Ex is Exception).Subscribe(a =>
            {
                valueWithException = a;
            });

            Assert.AreEqual("pass", valueWithoutException);
            Assert.AreEqual("", valueWithException);
        }
    }
}
