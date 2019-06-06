/*
 * Copyright (c) 2019 Víctor Vives - All rights reserved.
 * 
 * Licensed under the MIT License. 
 * See LICENSE file in the project root for full license information.
 */

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
        /// Initializes a new instance of the <see cref="RestRequestExecutor"/> class.
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
        /// The asynchronous task that performs the request.
        /// </returns>
        public async Task<string> ExecuteRequestAsync(string requestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMilliseconds(this.timeout);

                using (HttpResponseMessage response = await client.PostAsync(this.url, new StringContent(requestBody, Encoding.UTF8, "application/json")))
                using (Stream body = await response.Content.ReadAsStreamAsync())
                using (StreamReader reader = new StreamReader(body, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
