using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWinBackGrnd.IO.GraphicFile.Exceptions
{

    /// <summary>
    /// Use when reporting an error pertaining to an unsupported feature/script version when parsing.
    /// </summary>
    public class UnsupportedVersionException : Exception
    {
        public string MinimumVersion;
        public string FoundVersion;

        /// <summary>
        /// Default Exception constructor with user-friendly message.
        /// </summary>
        /// <param name="message">The message to be passed to the user.</param>
        public UnsupportedVersionException(string message) : base("Minimum supported version: 0.1\nFound version: 0\nMessage attached: " + message)
        {
            MinimumVersion = "0.1";
            FoundVersion = "0.0";
        }

        public UnsupportedVersionException(string message, string min, string found) : base("Minimum supported version: " + min +
                                                                                       "\nFound version: " + found + "\nMessage attached: " + message)
        {
            MinimumVersion = min;
            FoundVersion = found;
        }

    }
}
