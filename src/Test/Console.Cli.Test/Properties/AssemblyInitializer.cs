﻿using MaSch.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaSch.Console.Cli.Test.Properties
{
    [TestClass]
    public class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
        {
            TestClassBase.DefaultMockBehavior = Moq.MockBehavior.Strict;
        }
    }
}
