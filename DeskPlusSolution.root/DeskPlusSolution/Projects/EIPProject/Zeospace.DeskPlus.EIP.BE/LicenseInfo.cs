using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Zeospace.DeskPlus.EIP.BE
{
    [Serializable]
    public class LicenseInfo
    {
        public LicenseInfo() { }

        /// <summary>
        /// LicenseNumber
        /// </summary>
        public string LicenseNumber { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Count
        /// </summary>
        public string Count { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Expire
        /// </summary>
        public string Expire { get; set; }
    }
}
