using System;

namespace Zeospace.DeskPlus.Account.BE
{
    [Serializable]
    public class UserSNSInfo
    {
        public UserSNSInfo()
        {
        }

        public int CompanyID { get; set; }

        public int UserID { get; set; }

        public int UserSNSID { get; set; }

        public string FaceBook { get; set; }

        public string FaceBookPwd { get; set; }

        public string Twitter { get; set; }

        public string TwitterPwd { get; set; }

        public string Me2day { get; set; }

        public string Me2dayPwd { get; set; }

        public string Google { get; set; }

        public string GooglePwd { get; set; }

        public string NaverBlog { get; set; }

        public string NaverBlogPwd { get; set; }
    }
}