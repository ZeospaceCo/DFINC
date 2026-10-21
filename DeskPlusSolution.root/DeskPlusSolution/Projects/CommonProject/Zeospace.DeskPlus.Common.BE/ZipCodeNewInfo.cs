using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class ZipCodeNewInfo
    {
        public ZipCodeNewInfo()
        {
        }

        public string ZipCode { get; set; }

        public string Do { get; set; }

        public string Si { get; set; }

        public string Street { get; set; }

        public string BuildNum1 { get; set; }

        public string BuildNum2 { get; set; }

        public string BuildName { get; set; }

        public string Dong { get; set; }
    }
}