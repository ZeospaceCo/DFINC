using System;

namespace Zeospace.DeskPlus.Approval.BE
{
    [Serializable]
    public class FavoriteFormInfo : FormInfo
    {
        public int UserID { get; set; }

        public int FavoriteFormID { get; set; }

        public int FavOrder { get; set; }

        /*Viewing용도 추가필드*/
    }
}