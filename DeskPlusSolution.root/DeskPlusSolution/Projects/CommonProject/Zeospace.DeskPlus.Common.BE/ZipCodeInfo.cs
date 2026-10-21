using System;

namespace Zeospace.DeskPlus.Common.BE
{
    [Serializable]
    public class ZipCodeInfo
    {
        public ZipCodeInfo()
        {
        }

        public string ZipCode { get; set; }

        public string Si { get; set; }

        public string Gu { get; set; }

        public string Dong { get; set; }

        public string Ri { get; set; }

        public string Building { get; set; }

        public string Bunji { get; set; }

        public string Num_Bldg { get; set; }
    }
}