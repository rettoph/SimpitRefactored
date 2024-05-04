using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KerbalSimpit.Core.Constants
{
    public static class SpecialBytes
    {
        /// <summary>
        /// Indicates the end of a stream has been read
        /// </summary>
        public const int EndOfData = -1;

        /// <summary>
        /// Indicates the end of a continuous message
        /// </summary>
        public const int EndOfMessage = 0;
    }
}
