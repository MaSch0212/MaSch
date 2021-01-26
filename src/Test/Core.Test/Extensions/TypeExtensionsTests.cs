using System;
using System.Collections.Generic;
using MaSch.Core.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Core.Test.Extensions
{
    [TestClass]
    public class TypeExtensionsTests
    {
        [TestMethod]
        public void IsCastableTo_ExplicitValueTypes_False()
        {
            var typeCasts = new List<Tuple<Type, Type>>()
            {
                new Tuple<Type, Type>(typeof (string), typeof (byte)),
                new Tuple<Type, Type>(typeof (string), typeof (short)),
                new Tuple<Type, Type>(typeof (string), typeof (int)),
                new Tuple<Type, Type>(typeof (string), typeof (long)),
                new Tuple<Type, Type>(typeof (string), typeof (float)),
                new Tuple<Type, Type>(typeof (string), typeof (double)),
                new Tuple<Type, Type>(typeof (string), typeof (decimal)),
            };

            foreach (var typeCast in typeCasts)
            {
                Assert.IsFalse(typeCast.Item1.IsCastableTo(typeCast.Item2));
            }
        }

        [TestMethod]
        public void IsCastableTo_ExplicitValueTypes_True()
        {
            Assert.IsTrue(typeof(double).IsCastableTo(typeof(int)));
        }

        [TestMethod]
        public void IsCastableTo_ImplicitValueTypes_False()
        {
            Assert.IsFalse(typeof(double).IsCastableTo(typeof(int), true));
        }

        [TestMethod]
        public void IsCastableTo_ImplicitValueTypes_True()
        {
            Assert.IsTrue(typeof(int).IsCastableTo(typeof(double), true));
        }
    }
}
