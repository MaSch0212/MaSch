using MaSch.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace MaSch.Core.Test
{
    [TestClass]
    public class ActionOnDisposeTests
    {
        [TestMethod]
        public void Constructor_NullChecks()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ActionOnDispose((Action)null!));
            Assert.IsNotNull(new ActionOnDispose(() => { }));

            Assert.ThrowsException<ArgumentNullException>(() => new ActionOnDispose((Action<TimeSpan>)null!));
            Assert.IsNotNull(new ActionOnDispose((TimeSpan t) => { }));
        }

        [TestMethod]
        public void Dispose_WithoutTime()
        {
            int callCount = 0;

            var obj = new ActionOnDispose(() => Interlocked.Increment(ref callCount));

            Assert.AreEqual(0, callCount);
            obj.Dispose();
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public void Dispose_WithTime()
        {
            int callCount = 0;
            TimeSpan? lastTimeSpan = null;

            var obj = new ActionOnDispose(t =>
            {
                Interlocked.Increment(ref callCount);
                lastTimeSpan = t;
            });

            Assert.AreEqual(0, callCount);
            Thread.Sleep(100);
            obj.Dispose();
            Assert.AreEqual(1, callCount);
            Assert.That.IsBetween(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150), lastTimeSpan!.Value);
        }
    }
}
