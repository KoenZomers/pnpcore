﻿using PnP.Core.Model;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace PnP.Core.QueryModel
{
    /// <summary>
    ///     <para>
    ///         Defines method to execute queries asynchronously that are described by an IQueryable object.
    ///     </para>
    /// </summary>
    public interface IAsyncQueryProvider : IQueryProvider
    {
        /// <summary>
        /// Executes the strongly-typed query represented by a specified expression tree asynchronously.
        /// </summary>
        TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the expression to the current batch
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        Task<IEnumerableBatchResult<TResult>> AddToCurrentBatchAsync<TResult>(Expression expression);
    }
}
