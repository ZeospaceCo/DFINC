using System;
using System.ComponentModel;

namespace Zeospace.DeskPlus.Board.BE
{
    [Serializable]
    public class AuthorityInfo
    {
        public AuthorityInfo()
        {
        }

        public int CompanyID { get; set; }

        public int CategoryID { get; set; }

        public string Type { get; set; }

        public int OwnerID { get; set; }

        public string Authority { get; set; }

        public int WriteUserID { get; set; }

        public DateTime WriteDate { get; set; }

        public int ModifyUserID { get; set; }

        public DateTime ModifyDate { get; set; }
    }

    /// <summary>
    /// 게시판 권한
    /// 가져오기 : GetDescription(_type)
    /// </summary>
    public enum BoardAuth
    {
        [Description("1,2,3,4")]
        Read = 0, //읽기
        [Description("2,4")]
        Write = 1, //쓰기
        [Description("3,4")]
        ShortWrite = 2 //댓글쓰기
    }
}