/*
 * Copyright (c) 2019 Víctor Vives - All rights reserved.
 * 
 * Licensed under the MIT License. 
 * See LICENSE file in the project root for full license information.
 */

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Polly;

using RestClientBoilerplate.Exceptions;
using RestClientBoilerplate.Utils;

namespace RestClientBoilerplate.Executors
{
    /// <summary>
    /// Class that represents a Rest request executor.
    /// </summary>
    /// <seealso cref="RestClientBoilerplate.Executors.IRequestExecutor" />
    public class RestRequestExecutor : IRequestExecutor
    {
        /// <summary>
        /// The URL.
        /// </summary>
        private readonly string url;

        /// <summary>
        /// The timeout in milliseconds.
        /// </summary>
        private readonly int timeout;

        /// <summary>
        /// The maximum retry attempts.
        /// </summary>
        private readonly int retryCount = 3;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RestRequestExecutor" /> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        public RestRequestExecutor(string url , int timeout)
        {
            this.url = url;
            this.timeout = timeout;
        }

        /// <summary>
        /// Executes the request asynchronous.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        public async Task<string> ExecuteRequestAsync(string requestBody)
        {
            try
            {
                // Execute the connection via policy.
                PolicyResult<HttpResponseMessage> policyResult = await Policy
                    .Handle<TransientErrorException>()
                    .RetryAsync(this.retryCount, this.OnRetryAsync)
                    .ExecuteAndCaptureAsync(() => this.ExecuteRemoteRequestAsync(requestBody))
                    .ConfigureAwait(false);

                // Throw inner exception if the operation fails.
                if (policyResult.Outcome == OutcomeType.Failure)
                {
                    throw policyResult.FinalException;
                }

                HttpResponseMessage message = policyResult.Result;

                string content = await message
                    .Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);

                return content;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Called when [retry asynchronous].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="retry">The retry.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        private async Task OnRetryAsync(Exception exception, int retry)
        {
            await Task.Yield();
        }

        /// <summary>
        /// Executes the remote request asynchronous.
        /// </summary>
        /// <param name="requestBody">The request body.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="RestClientBoilerplate.Exceptions.TransientErrorException">
        /// There was a transient error during the remote connection.
        /// </exception>
        private async Task<HttpResponseMessage> ExecuteRemoteRequestAsync(string requestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.Timeout = TimeSpan.FromMilliseconds(this.timeout);

                    using (HttpResponseMessage response = await client.PostAsync(this.url, new StringContent(requestBody, Encoding.UTF8, "application/json")))
                    {
                        response.EnsureSuccessStatusCode();

                        return response;
                    }
                }
                catch (HttpRequestException ex)
                {
                    throw new TransientErrorException("There was a transient error during the remote connection.", ex);
                }
                catch (WebException ex)
                {
                    if (ExceptionUtils.IsTransientError(ex))
                    {
                        throw new TransientErrorException("There was a transient error during the remote connection.", ex);
                    }

                    throw;
                }
            }
        }
    }
}
