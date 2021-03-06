﻿using MaSch.Test.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Diagnostics.CodeAnalysis;

namespace MaSch.Test.Components.Test.Models
{
    [TestClass]
    public class MockVerifiableCollectionTests : TestClassBase
    {
        [TestMethod]
        public void Ctor()
        {
            using var collection = new MockVerifiableCollection();

            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Ctor_List()
        {
            var mock1 = new Mock<IMockVerifiable>(MockBehavior.Strict);
            var mock2 = new Mock<IMockVerifiable>(MockBehavior.Strict);

            var collection = new MockVerifiableCollection(new[] { mock1.Object, mock2.Object });

            Assert.AreEqual(2, collection.Count);
            Assert.AreSame(mock1.Object, collection[0]);
            Assert.AreSame(mock2.Object, collection[1]);
        }

        [TestMethod]
        [SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created.", Justification = "Would fail test.")]
        public void Verify()
        {
            var mock1 = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock1.Setup(x => x.Verify(Times.Once(), "My message"));
            var mock2 = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock2.Setup(x => x.Verify(Times.Once(), "My message"));

            var collection = new MockVerifiableCollection(new[] { mock1.Object, mock2.Object });
            collection.Verify(Times.Once(), "My message");

            mock1.Verify(x => x.Verify(Times.Once(), "My message"), Times.Once());
            mock2.Verify(x => x.Verify(Times.Once(), "My message"), Times.Once());
        }

        [TestMethod]
        public void Dispose_()
        {
            var mock1 = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock1.Setup(x => x.Dispose());
            var mock2 = new Mock<IMockVerifiable>(MockBehavior.Strict);
            mock2.Setup(x => x.Dispose());

            var collection = new MockVerifiableCollection(new[] { mock1.Object, mock2.Object });
            ((IDisposable)collection).Dispose();

            mock1.Verify(x => x.Dispose(), Times.Once());
            mock2.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
