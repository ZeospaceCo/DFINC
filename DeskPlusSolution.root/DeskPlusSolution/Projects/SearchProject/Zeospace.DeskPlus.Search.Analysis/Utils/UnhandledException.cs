using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.Search.Analysis.Utils
{
    public class UnhandledException : Exception
    {
        /// <summary>
        /// Required for serialization support.
        /// </summary>
        /// <seealso cref= java.io.Serializable </seealso>
        private const long serialVersionUID = 1832101364842773720L;

        /// <summary>
        /// Constructs the exception using a cause.
        /// </summary>
        /// <param name="cause">  the underlying cause </param>
        public UnhandledException(Exception cause) : base("")
        {
        }

        /// <summary>
        /// Constructs the exception using a message and cause.
        /// </summary>
        /// <param name="message">  the message to use </param>
        /// <param name="cause">  the underlying cause </param>
        public UnhandledException(string message, Exception cause) : base(message, cause)
        {
        }
    }
}
