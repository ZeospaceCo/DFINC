using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.BLL;
using Zeospace.DeskPlus.Board.DAL;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Portlet.Web.Package.Board
{
    public partial class GMWords : Zeospace.DeskPlus.Portlet.Web.Base.BaseControlPortlet
    {
        /// <summary>
        /// ConfigurationManager.AppSettings["SQLConnSolution"];
        /// </summary>
        private static readonly string CONN_SOLUTION = ConfigurationManager.ConnectionStrings["SQLConnSolution"].ConnectionString;

        /// <summary>
        /// 회사별 컨넥션(Solution 데이터베이스)
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        protected static string ConnSolution()
        {
            return CONN_SOLUTION;
        }

        #region 변수설정
        public int BoardTotalCount;
        public int BoardCategoryID;
        public string PhotoImage;

        private string OptionUseDept;//부서별게시판여부
        private string OptionUseView;//보기용 여부
        private string Auth; //관리자 : 1; 일반인 : 0
        #endregion

        #region Page Process (페이지 처리)
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Package.Board.GMWords).Assembly;	//어셈블리 선언
            #endregion


            if (!IsPostBack)
            {
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)
        public void LoadData()
        {

            //환경설정에 추가한 내용이 있으면 해당 사항으로 정보 변경
            //타이틀, 리스트 갯수


            /*  기본적인것은 넘어오는데 추가적으러 더 필요한 사항이 있으면 사용.
            Zeospace.DeskPlus.EIP.DAL.Portlet dal = new Zeospace.DeskPlus.EIP.DAL.Portlet();
            List<Zeospace.DeskPlus.EIP.BE.PortletInfo> al = dal.GetPortlet(CurrentUserInfo.CompanyID, categoryID, portletID);


            foreach (Zeospace.DeskPlus.EIP.BE.PortletInfo info in al)
            {

            }
             * */
            // 게시판의 CategoryID 디코딩


            GetCategory();
            GetListCount();
            GetImage();
        }
        #endregion

        #region (함수)
        private void SetLanguage()
        {
            this.GetResourceManager();

            if (!this.resMng.Equals(null))
            {

            }
        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : categoryID 디코딩
        /// 작성자     : shine
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetCategory()
        {
            //게시판 카테고리 값
            if (ParamValue.Length > 0)
            {
                foreach (string keyValue in ParamValue.Split('^'))
                {
                    string pKey = keyValue.Split('$')[0];
                    string pValue = keyValue.Split('$')[1];

                    if (pKey == "catID") BoardCategoryID = pValue.ToNumber<int>();
                }
            }

            bool isRoleAdmin = CurrentUserInfo.IsInRole("admin");//administrator
            bool isRoleBoard = CurrentUserInfo.IsInRole("board");//board admin
            Auth = (isRoleAdmin || isRoleBoard) ? "1" : "0"; //관리자 : 1; 일반인 : 0

            Zeospace.DeskPlus.Board.DAL.Category dalCat = new Zeospace.DeskPlus.Board.DAL.Category();
            DataSet ds = dalCat.GetCategory(CurrentUserInfo.CompanyID, BoardCategoryID);

            if (ds != null)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    OptionUseDept = dr["OptionUseDept"].ToString();
                    OptionUseView = dr["OptionUseView"].ToString();
                }
            }

        }

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 게시판 총 카운트 가져오기
        /// 작성자     : shine
        /// 최초작성일 : 2011-05-09
        /// </summary>
        private void GetListCount()
        {
            try
            {
                bool isChild = (OptionUseView == "1") ? true : false; //보기용이면 하위검색가능
                string myAuthCatList = "";

                if (BoardCategoryID > 0)
                {
                    //관리자 일경우
                    if (Auth == "1")
                    {
                        myAuthCatList = BoardCategoryID.ToString();

                        if (isChild)
                        {
                            Zeospace.DeskPlus.Board.BLL.Category catBiz = new Zeospace.DeskPlus.Board.BLL.Category();
                            myAuthCatList = catBiz.GetChildCategory(CurrentUserInfo.CompanyID, BoardCategoryID);
                        }
                    }
                    else
                    {
                        myAuthCatList = GetMyAuthCatList(BoardCategoryID, isChild);
                    }
                }
                else
                {
                    myAuthCatList = "-1"; //카테고리 값이 0 넘어오면 -1 로 해서 리스트 안나오게 함
                }

                Zeospace.DeskPlus.Board.DAL.Lists biz = new Zeospace.DeskPlus.Board.DAL.Lists();
                BoardTotalCount = biz.GetPageListCount(CurrentUserInfo.CompanyID,
                    myAuthCatList,
                    "", "",
                    "", "", OptionUseDept, CurrentUserInfo.DeptID);


            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                BoardTotalCount = 0;
                //throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 권한별 카테고리 리스트 가져오기
        /// </summary>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_useChild">if set to <c>true</c> [_use child].</param>
        /// <returns></returns>
        private string GetMyAuthCatList(int _categoryID, bool _useChild)
        {
            try
            {
                #region 내가 볼 수 있는 하위 카테고리
                NameValueCollection nvc = new NameValueCollection();
                string myAuthCatList = "";// _categoryID.ToString() + ",";
                Zeospace.DeskPlus.Board.BLL.Category biz = new Zeospace.DeskPlus.Board.BLL.Category();

                //보기용은 하위카테고리까지 가져옴
                if (OptionUseView == "1")
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, true, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }
                else
                {
                    nvc = biz.GetMyAuthCategory(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID,
                        CurrentUserInfo.DeptID, _categoryID, OptionUseDept, _useChild, BoardAuth.Read, true, false);

                    foreach (string s in nvc.AllKeys) myAuthCatList += s + ",";
                }

                myAuthCatList = myAuthCatList.TrimEnd(',');
                #endregion

                return myAuthCatList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "";
                //throw;
            }
        }

        /// <summary>
        /// 용도 : 사용자 정보 가져오기
        /// 작성자 : 심재운
        /// </summary>
        private void GetImage()
        {
            DAL.GMWords info = new DAL.GMWords();
            int photoUserID = info.GetPhotoUserID(CurrentUserInfo.CompanyID);

            Zeospace.DeskPlus.Account.DAL.User biz = new Zeospace.DeskPlus.Account.DAL.User();
            //관리 인사에는 비활성화 한 사용자도 보여야 한다.
            List<UserInfo> al = biz.GetUserInfo(CurrentUserInfo.CompanyID, photoUserID, false, false);

            foreach (UserInfo be in al)
            {
                LoadPhotoSeal(be.PhotoImage);
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 사진 이미지 가져오기
        /// </summary>
        /// <param name="_photo">The _photo.</param>
        /// <param name="_seal">The _seal.</param>
        private void LoadPhotoSeal(string _photo)
        {
            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


            DAL.GMWords info = new DAL.GMWords();
            int photoUserID = info.GetPhotoUserID(CurrentUserInfo.CompanyID);

            #region 이미지 경로 암호화
            string images = this.BaseImgPath + "/Icon/ico_noimg.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", images);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
            #endregion

            PhotoImage = "<img src='" + images + "' alt='photo' title='photo' />";

            images = baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + photoUserID + "/Picture/" + _photo;

            if (System.IO.File.Exists(Server.MapPath(images)))
            {
                #region 이미지 경로 암호화

                paraDic = new Dictionary<string, string>();
                paraDic.Add("filePath", images);
                encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                images = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                #endregion

                PhotoImage = "<img src='" + images + "' width='94' height='124' alt='' title='' />";
            }
        }

        #endregion

        #region (프로퍼티)

        #endregion

    }
}