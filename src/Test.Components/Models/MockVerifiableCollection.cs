using MaSch.Core.Extensions;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MaSch.Test.Models
{
    /// <summary>
    /// Collection that contains <see cref="IMockVerifiable"/> and can itself also be verified, so verify all its items.
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.Collection{T}" />
    /// <seealso cref="MaSch.Test.Models.IMockVerifiable" />
    public sealed class MockVerifiableCollection : Collection<IMockVerifiable>, IMockVerifiable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockVerifiableCollection"/> class.
        /// </summary>
        public MockVerifiableCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MockVerifiableCollection"/> class.
        /// </summary>
        /// <param name="list">The list that is wrapped by the new collection.</param>
        public MockVerifiableCollection(IList<IMockVerifiable> list)
            : base(list)
        {
        }

        /// <inheritdoc/>
        public void Verify(Func<Times>? times, string? failMessage)
            => Items.ForEach(x => x.Verify(times, failMessage));

        /// <inheritdoc/>
        void IDisposable.Dispose()
            => Items.ForEach(x => x.Dispose());
    }
}
