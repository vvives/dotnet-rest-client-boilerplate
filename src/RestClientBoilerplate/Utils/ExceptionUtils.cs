/*
 * Copyright (c) 2019 Víctor Vives - All rights reserved.
 * 
 * Licensed under the MIT License. 
 * See LICENSE file in the project root for full license information.
 */
 
using System.Linq;
using System.Net;

namespace RestClientBoilerplate.Utils
{
    /// <summary>
    /// Class that represents common exception utilities.
    /// </summary>
    public class ExceptionUtils
    {
        /// <summary>
        /// Determines whether [is transient error] [the specified exception].
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>
        ///   <c>true</c> if [is transient error] [the specified exception]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTransientError(WebException exception)
        {
            if (exception != null)
            {
                // The web exception it might be transient if contains one of the following status values.
                return new[]
                {
                    WebExceptionStatus.ConnectionClosed,
                    WebExceptionStatus.Timeout,
                    WebExceptionStatus.RequestCanceled
                }.Contains(exception.Status);
            }

            return false;
        }
    }
}
