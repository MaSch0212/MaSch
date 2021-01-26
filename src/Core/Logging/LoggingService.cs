using System;
using System.Collections.Generic;
using System.Linq;
using MaSch.Common.Extensions;

namespace MaSch.Common.Logging
{
    /// <summary>
    /// The default implementation of <see cref="ILoggingService"/>.
    /// </summary>
    /// <seealso cref="ILoggingService" />
    public class LoggingService : ILoggingService
    {
        private readonly IList<ILoggingProvider> _loggingProviders;

        /// <inheritdoc/>
        public bool HasLoggingProvider => _loggingProviders.Count > 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        public LoggingService() : this((IEnumerable<ILoggingProvider>?)null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="providers">The providers to use in the <see cref="LoggingService"/>.</param>
        public LoggingService(params ILoggingProvider[]? providers) : this((IEnumerable<ILoggingProvider>?)providers) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService" /> class.
        /// </summary>
        /// <param name="providers">The providers to use in the <see cref="LoggingService"/>.</param>
        public LoggingService(IEnumerable<ILoggingProvider>? providers)
        {
            _loggingProviders = providers?.Where(x => x != null).Distinct().ToList() ?? new List<ILoggingProvider>();
        }

        /// <inheritdoc/>
        public virtual void Log(LogType logType, string? message)
            => _loggingProviders.ForEach(x => x.Log(logType, message));

        /// <inheritdoc/>
        public virtual void Log(LogType logType, string? message, Exception? exception)
            => _loggingProviders.ForEach(x => x.Log(logType, message, exception));

        /// <inheritdoc/>
        public virtual bool AddLoggingProvider(ILoggingProvider provider) 
            => _loggingProviders.AddIfNotExists(Guard.NotNull(provider, nameof(provider)));

        /// <inheritdoc/>
        public virtual bool RemoveLoggingProvider(ILoggingProvider provider) 
            => _loggingProviders.TryRemove(Guard.NotNull(provider, nameof(provider)));
    }
}
