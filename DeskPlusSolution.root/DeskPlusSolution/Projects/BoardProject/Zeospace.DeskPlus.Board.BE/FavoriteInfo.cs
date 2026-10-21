namespace Zeospace.DeskPlus.Board.BE
{
    public class FavoriteInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteInfo"/> class.
        /// </summary>
        public FavoriteInfo()
        {
        }

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

        public int CategoryID { get; set; }

        //public 	string	ListType	{ get; set; }
        public string Types { get; set; }
    }
}