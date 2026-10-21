using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeospace.DeskPlus.WebHard.BE
{
    [Serializable]
    public class FavoriteInfo
    {
        public FavoriteInfo() { }

        public int CompanyID { get; set; }
        public int FavoriteID { get; set; }
        public int ParentFavoriteID { get; set; }
        public int UserID { get; set; }
        public int FavoriteOrder { get; set; }
        public string NameBase { get; set; }
        public string NameKo { get; set; }
        public string NameEn { get; set; }
        public string NameJa { get; set; }
        public string NameFr { get; set; }
        public string NameZhcn { get; set; }
        public string NameZhtw { get; set; }
        public string PathID { get; set; }
        public string PathNameBase { get; set; }
        public string PathNameKo { get; set; }
        public string PathNameEn { get; set; }
        public string PathNameJa { get; set; }
        public string PathNameFr { get; set; }
        public string PathNameZhcn { get; set; }
        public string PathNameZhtw { get; set; }
        public int FolderID { get; set; }
        public string Types { get; set; }


    }
}
