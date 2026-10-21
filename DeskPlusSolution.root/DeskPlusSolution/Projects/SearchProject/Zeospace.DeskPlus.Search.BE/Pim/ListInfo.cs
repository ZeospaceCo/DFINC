using System;

namespace Zeospace.DeskPlus.Search.BE.Pim
{
    [Serializable]
    public class ListInfo : SearchInfo
    {
        public ListInfo()
        {
        }

        public string Repeat { get; set; }

        public string Alert { get; set; }

        public string AlertType { get; set; }

        public string AlertTime { get; set; }

        public int ReservationID { get; set; }

        public string Lunar { get; set; }
    }
}