using MaSch.Core;
using System;
using System.Threading;

namespace MaSch.Console.Controls
{
    public static class ConsoleSynchronizer
    {
        private static readonly object Lock = new object();

        public static IDisposable Scope()
        {
            Monitor.Enter(Lock);
            return new ActionOnDispose(() => Monitor.Exit(Lock));
        }
    }
}
