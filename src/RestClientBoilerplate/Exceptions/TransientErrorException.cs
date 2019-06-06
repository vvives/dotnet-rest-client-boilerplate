/*
 * Copyright (c) 2019 Víctor Vives - All rights reserved.
 * 
 * Licensed under the MIT License. 
 * See LICENSE file in the project root for full license information.
 */

using System;
using System.Runtime.Serialization;

namespace RestClientBoilerplate.Exceptions
{
    /// <summary>
    /// Class that represents a custom exception that indicates a transient error.
    /// </summary>
    /// <seealso cref="System.Exception" />
    [Serializable]
    public class TransientErrorException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransientErrorException"/> class.
        /// </summary>
        public TransientErrorException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientErrorException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TransientErrorException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientErrorException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public TransientErrorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientErrorException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
        protected TransientErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
