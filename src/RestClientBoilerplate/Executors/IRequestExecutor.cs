/*
 * Copyright (c) 2019 Víctor Vives - All rights reserved.
 * 
 * Licensed under the MIT License. 
 * See LICENSE file in the project root for full license information.
 */
 
using System.Threading.Tasks;

namespace RestClientBoilerplate.Executors
{
    /// <summary>
    /// Common shared interface between request executors.
    /// </summary>
    public interface IRequestExecutor
    {
        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        Task<string> ExecuteRequestAsync(string requestBody);
    }
}
