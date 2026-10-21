using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Auth;
using System.Data;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.WebHard.Web.Content
{
    public partial class List : Base.BasePageWebHard
    {
        #region 변수설정
        public int FolderID;
        public int CurrentPage = 0;
        public string TotalCount = String.Empty;  //총카운트
        public string RecordPerPage = String.Empty; //리스트 개수
        public string SearchField = String.Empty; //검색키워드
        public string SearchValue = String.Empty; //검색어
        public string SearchTerm = String.Empty;  //기간검색키워드
        public string SearchTermValue = String.Empty;  //기간검색어       
        public string Order = String.Empty; //sort     
        public string Types;
        public string SiteMap = String.Empty;//사이트맵
        public int PrivateQuota = 0;// 개인에게 할당된 용량
        public int FavoriteID;
        public int ParentFolderID;
        public int ParentFavoriteID;
        public int ChildCnt;
        public string DownCheck = "Y";
        public int UID;        
        public string UseCount = "0";       // 개인이 사용한 용량
        public string PGubun = "N";
        public int PAdmin = 0;
        public string FileLength = "0";     // 공용폴더 파일별 제한용량
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.WebHard.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.WebHard.Web.Content.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SwfUploadGUID = System.Guid.NewGuid().ToString();
                SetLanguage();
                LoadData();
            }
        }
        #endregion

        #region DataLoad (컨트롤에 데이터를 바인딩 하는 메소드)

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-19
        /// 작성목적 : Loads the data.
        /// </summary>
        private void LoadData()
        {           
            LoadParam();

            if (Types == "U" || Types == "L")
            {
                DAL.File dal = new DAL.File();

                DataSet ds = dal.GetPublicUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FolderID, Types);

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        // 공용폴더 할당된 용량 가져오기 (MB)
                        PrivateQuota = dr["FolderLength"].ToString().ToNumber<int>();
                        // 공용폴더 사용한 용량 가져오기
                        UseCount = dr["FileLengthSum"].ToString();
                        // 공용폴더 파일별 용량 제한
                        FileLength = dr["FileLength"].ToString();
                    }
                }                
            }
            else
            {
                // 개인에게 할당된 용량 가져오기 (MB)
                PrivateQuota = CurrentUserInfo.PrivateQuota;

                // 개인이 사용한 용량 가져오기
                DAL.File dal = new DAL.File();
                UseCount = dal.GetUseCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }

            ddlSearchField.SelectedValue = SearchField;
            ctbSearch.Text = SearchValue;
            ddlSearchTerm.SelectedValue = SearchTerm;                       

            #region PageSize 설정하기

            RecordPerPage = GetEnvKeyValue(CurrentUserInfo.CompanyID, AppSettingType.WEBHARD, "ListCount", "10");//페이지리스트수

            string tmpTypes = Types;
            if (tmpTypes == "M")
            {
                tmpTypes = "P";
            }

            DAL.FolderUser dalUser = new DAL.FolderUser();
            List<BE.FolderUserInfo> al = dalUser.GetList(CurrentUserInfo.CompanyID, FolderID, CurrentUserInfo.UserID, tmpTypes);

            foreach (BE.FolderUserInfo userInfo in al)
            {
                RecordPerPage = userInfo.PageSize.ToString();
            }

            ddlListCount.SelectedValue = RecordPerPage;

            #endregion

            cbtnAdmin.Visible = false;

            if (AccessAuth == true)
            {
                if (PAdmin == 1)
                {
                    cbtnAdmin.Visible = false;
                }
                else
                {
                    cbtnAdmin.Visible = true;
                }
            }

            if (Types == "U" || Types == "L")            
            {
                // 공용폴더       
                CbtnSend.Visible = false;
                CbtnShare.Visible = false;
                CbtnMove.Visible = false;
                CbtnCopy.Visible = false;
                CbtnDel.Visible = false;
                CbtnAllDel.Visible = false;
                cbtnOrder.Visible = false;
                cbtnFavor.Visible = false;
                cbtnFavorMove.Visible = false;
                CbtnDownload.Visible = false;
                CbtnUpload.Visible = false;                

                #region admin 관리자 체크
                if (PAdmin == 1)
                {
                    if (ParentFolderID != -1)
                    {
                        CbtnDownload.Visible = true;
                        CbtnUpload.Visible = true;
                        CbtnDel.Visible = true;
                        CbtnAllDel.Visible = true;
                        CbtnSend.Visible = true;
                        CbtnMove.Visible = true;
                        CbtnCopy.Visible = true;

                        DownCheck = "Y";
                    }
                }
                else
                {
                    if (ParentFolderID != -1)
                    {
                        cbtnFavor.Visible = true;
                    }

                    PublicInfo();
                }             

                #endregion admin 관리자 체크
                     
            }
            else if ((Types == "P") || (Types == "M"))
            {
                // 개인폴더, 공유한 폴더                 
                CbtnSend.Visible= true;
                CbtnShare.Visible = true;
                CbtnMove.Visible = true;
                CbtnCopy.Visible = true;
                CbtnDel.Visible = true;
                CbtnAllDel.Visible = true;
                CbtnDownload.Visible = true;
                CbtnUpload.Visible = true;

                if (FavoriteID > 0)
                {
                    cbtnOrder.Visible = false;    
                    cbtnFavor.Visible = false;
                    if (ChildCnt > 0)
                    {
                        cbtnFavorMove.Visible = false;
                    }
                    else
                    {
                        cbtnFavorMove.Visible = true;
                    }     
                }
                else
                {
                    cbtnFavorMove.Visible = false;
                    cbtnOrder.Visible = false;       
                    cbtnFavor.Visible = true;
                }
            }
            else if (Types == "J")
            {
                //전달 받은 파일
                cbtnFavorMove.Visible = false;
                cbtnOrder.Visible = false;
                cbtnFavor.Visible = false;
                CbtnSend.Visible = true;
                CbtnShare.Visible = false;
                CbtnMove.Visible = true;
                CbtnCopy.Visible = true;
                CbtnDel.Visible = true;
                CbtnAllDel.Visible = true;
                CbtnDownload.Visible = true;
                CbtnUpload.Visible = false;
            }
            else if (Types == "S")
            {
                //공유 받은 폴더
                CbtnSend.Visible = false;
                CbtnShare.Visible = false;
                CbtnMove.Visible = false;
                CbtnCopy.Visible = false;
                CbtnDel.Visible = false;
                CbtnAllDel.Visible = false;

                if (FavoriteID > 0)
                {
                    cbtnOrder.Visible = false;
                    cbtnFavor.Visible = false;
                    if (ChildCnt > 0)
                    {
                        cbtnFavorMove.Visible = false;
                    }
                    else
                    {
                        cbtnFavorMove.Visible = true;
                    }
                }
                else
                {
                    cbtnFavorMove.Visible = false;
                    cbtnOrder.Visible = false;
                    cbtnFavor.Visible = true;
                }
 
                ShareInfo();
            }          
            else if (Types == "FC")
            {
                //즐겨찾기 폴더
                cbtnOrder.Visible = true;
                cbtnFavor.Visible = false;
                CbtnSend.Visible = false;
                CbtnShare.Visible = false;
                CbtnMove.Visible = false;
                CbtnCopy.Visible = false;
                CbtnDel.Visible = false;
                CbtnAllDel.Visible = false;
                CbtnDownload.Visible = false;
                CbtnUpload.Visible = false;

                DownCheck = "N";

                if (FavoriteID > 0)
                {
                    if (ChildCnt > 0)
                    {
                        cbtnFavorMove.Visible = false;
                    }
                    else
                    {
                        cbtnFavorMove.Visible = true;
                    }                    
                }   
                else
                {
                    cbtnFavorMove.Visible = false;
                    
                }
            }
            else
            {
                cbtnFavorMove.Visible = false;
                cbtnOrder.Visible = false;
                cbtnFavor.Visible = false;
                CbtnSend.Visible = false;
                CbtnShare.Visible = false;
                CbtnMove.Visible = false;
                CbtnCopy.Visible = false;
                CbtnDel.Visible = false;
                CbtnAllDel.Visible = false;
                CbtnDownload.Visible = false;
                CbtnUpload.Visible = false;

                DownCheck = "N";
            }

            GetListCount();

            GetSiteMap(); //사이트맵

            // 모아보기에서 접근 했을 시에 즐겨찾기이동, 폴더공유, 관리자 버튼 안 보이게 처리
            if (PGubun == "Y")
            {
                cbtnFavorMove.Visible = false;
                CbtnShare.Visible = false;
                cbtnAdmin.Visible = false;
            }
        }

        #endregion
        
        #region (함수)       

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-19
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                cbtnAdmin.Text = resMng.GetString("Manager");       //관리자;
                CbtnSend.Text = resMng.GetString("Send");           //전달;
                CbtnShare.Text = resMng.GetString("FolderShare");   //폴더공유;
                CbtnMove.Text = resMng.GetString("Move");           //이동;
                CbtnCopy.Text = resMng.GetString("Copy");           //복사;
                CbtnDel.Text = resMng.GetString("Del");             //삭제;
                CbtnAllDel.Text = resMng.GetString("AllDel");       //완전삭제;
                CbtnDownload.Text = resMng.GetString("Download");   //다운로드;
                CbtnUpload.Text = resMng.GetString("Upload");       //업로드;
                CbtnSearch.Text = resMng.GetString("Search");       //검색
                cbtnFavor.Text = resMng.GetString("FavoriteFolderAdd"); //즐겨찾기추가
                cbtnFavorMove.Text = resMng.GetString("FavoriteFolderMove"); //즐겨찾기이동
                cbtnOrder.Text = resMng.GetString("FavoriteOrder"); //하위순서변경

                ddlSearchTerm.Items.Clear();
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("ALL"), ""));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Week"), "week"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Month"), "month"));
                ddlSearchTerm.Items.Add(new ListItem(this.resMng.GetString("Period"), "period"));                
 
                ddlSearchField.Items.Clear();
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FileName"), "FileName"));
                ddlSearchField.Items.Add(new ListItem(this.resMng.GetString("FileType"), "FileType"));
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-19
        /// 작성목적 : 파라미터
        /// </summary>
        private void LoadParam()
        {
            ParentFolderID = GetDecryptParam("parentFolderID", "-1").ToNumber<int>();
            FolderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            CurrentPage = GetDecryptParam("currentPage", "1").ToNumber<int>();
            SearchField = GetDecryptParam("searchField", "");
            SearchValue = GetDecryptParam("searchValue", "");
            SearchTerm = GetDecryptParam("searchTerm", "");
            SearchTermValue = GetDecryptParam("searchTermValue", "");
            Order = GetDecryptParam("order", "");
            Order = (Order.Length == 0) ? "WriteDate desc" : Order;
            Types = GetDecryptParam("types", "");
            FavoriteID = GetDecryptParam("favoriteID", "0").ToNumber<int>();
            ParentFavoriteID = GetDecryptParam("parentFavoriteID", "0").ToNumber<int>();
            ChildCnt = GetDecryptParam("childCnt", "0").ToNumber<int>();
            PGubun = GetDecryptParam("pGubun", "N");
            PAdmin = GetDecryptParam("padmin", "0").ToNumber<int>();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-19
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            string startDate = "";
            string endDate = "";
            string getDate = "";

            if (SearchTerm == "week" || SearchTerm == "month")
            {
                if (SearchTerm == "week")
                    getDate = DateHelper.GetCultureStartEndDate("week", PreLangReal);
                else if (SearchTerm == "month")
                    getDate = DateHelper.GetCultureStartEndDate("month", PreLangReal);

                startDate = DateHelper.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
            }
            else if (SearchTerm == "period")
            {
                startDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(SearchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
            }

            DAL.File dal = new DAL.File();
            TotalCount = dal.GetPageListCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, FolderID, SearchField, SearchValue, startDate, endDate).ToString();
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-27
        /// 작성목적 : 사이트맵 보이기
        /// </summary>
        private void GetSiteMap()
        {
            string name = String.Empty;

            switch (Types)
            {
                case "U":
                case "L":
                    UID = CurrentUserInfo.UserID;
                    //SiteMap = GetResource("PublicFolder");  //"공용폴더";   
                    DAL.Folder uDal = new DAL.Folder();
                    DataSet dsu = uDal.GetFolder(CurrentUserInfo.CompanyID, FolderID);

                    if (dsu != null)
                    {
                        foreach (DataRow dr in dsu.Tables[0].Rows)
                        {
                            SiteMap = this.GetLangName(
                               dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                               dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                            SiteMap = (SiteMap == "ROOT") ? GetResource("PublicFolder") : SiteMap; 

                            /*
                            name = this.GetLangName(
                               dr["PathNameBase"].ToString(), dr["PathNameKo"].ToString(), dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                               dr["PathNameFr"].ToString(), dr["PathNameZhcn"].ToString(), dr["PathNameZhtw"].ToString(), PreLang);

                            switch (PreLang)
                            {
                                case "ko":
                                    name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "en":
                                    name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "ja":
                                    name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "fr":
                                    name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zh-cn":
                                    name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zh-tw":
                                    name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                            }

                            SiteMap += name;
                            */
                        }
                    }

                    break;

                case "P":
                    UID = CurrentUserInfo.UserID;
                    //SiteMap = GetResource("PrivateFolderName");  //"개인폴더";   
                    DAL.Folder pDal = new DAL.Folder();
                    DataSet dsP = pDal.GetFolder(CurrentUserInfo.CompanyID, FolderID);

                    if (dsP != null)
                    {
                        foreach (DataRow dr in dsP.Tables[0].Rows)
                        {
                            SiteMap = this.GetLangName(
                               dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                               dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                            SiteMap = (SiteMap == "ROOT") ? GetResource("PrivateFolderName") : SiteMap; 

                            /*
                            name = this.GetLangName(
                               dr["PathNameBase"].ToString(),dr["PathNameKo"].ToString(), dr["PathNameEn"].ToString(), dr["PathNameJa"].ToString(),
                               dr["PathNameFr"].ToString(), dr["PathNameZhcn"].ToString(), dr["PathNameZhtw"].ToString(), PreLang);

                            switch (PreLang)
                            {
                                case "ko":
                                    name = (dr["NameKo"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "en":
                                    name = (dr["NameEn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "ja":
                                    name = (dr["NameJa"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "fr":
                                    name = (dr["NameFr"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zh-cn":
                                    name = (dr["NameZhcn"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                                case "zh-tw":
                                    name = (dr["NameZhtw"].ToString() == "") ? dr["PathNameBase"].ToString() : name;
                                    break;
                            }

                            SiteMap += name;
                            */
                        }
                    }

                    break;
                case "J":
                    UID = CurrentUserInfo.UserID;
                    SiteMap = GetResource("SendFile");  //"전달받은파일"; 
                    break;
                case "S":
                    //SiteMap = GetResource("ShareFolder");  //"공유받은폴더";   
                    string FolderUserName = String.Empty;

                    DAL.Folder sDal = new DAL.Folder();
                    DataSet dsS = sDal.GetFolder(CurrentUserInfo.CompanyID, FolderID);

                    if (dsS != null)
                    {
                        foreach (DataRow dr in dsS.Tables[0].Rows)
                        {
                            /*
                            FolderUserName = this.GetLangName(
                               dr["UserNameBase"].ToString(),
                               dr["UserNameKo"].ToString(), dr["UserNameEn"].ToString(), dr["UserNameJa"].ToString(),
                               dr["UserNameFr"].ToString(), dr["UserNameZhcn"].ToString(), dr["UserNameZhtw"].ToString(), PreLang);
                            */

                            name = this.GetLangName(
                               dr["NameBase"].ToString(),
                               dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                               dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                            //name = (name == "ROOT") ? " > " + GetResource("PrivateFolderName") : " > " + name;
                            //SiteMap += " > " + FolderUserName + name;
                            SiteMap = (name == "ROOT") ? GetResource("PrivateFolderName") : name;                           

                            UID = dr["UserID"].ToString().ToNumber<int>();
                        }
                    }

                    break;
                case "M":
                    UID = CurrentUserInfo.UserID;
                    //SiteMap = GetResource("SharedFolder");  //"공유한 폴더";   
                    DAL.Folder mDal = new DAL.Folder();
                    DataSet dsM = mDal.GetFolder(CurrentUserInfo.CompanyID, FolderID);

                    if (dsM != null)
                    {
                        foreach (DataRow dr in dsM.Tables[0].Rows)
                        {
                            name = this.GetLangName(
                                  dr["NameBase"].ToString(),
                               dr["NameKo"].ToString(), dr["NameEn"].ToString(), dr["NameJa"].ToString(),
                               dr["NameFr"].ToString(), dr["NameZhcn"].ToString(), dr["NameZhtw"].ToString(), PreLang);

                            SiteMap = (name == "ROOT") ? GetResource("PrivateFolderName") : name;
                            //name = (name == "ROOT") ? " > " + GetResource("PrivateFolderName") : " > " + name;
                            //SiteMap += name;
                        }
                    }
                    break;
                case "FC":
                    UID = CurrentUserInfo.UserID;
                    //SiteMap = GetResource("FavoriteFolder");
                    DAL.Favorite fcDal = new DAL.Favorite();
                    DataTableReader dtr = fcDal.GetFavoriteList(CurrentUserInfo.CompanyID, FavoriteID, CurrentUserInfo.UserID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                               SiteMap = this.GetLangName(
                                   dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                   dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                                /*
                                name = this.GetLangName(
                                    dtr["PathNameBase"].ToString(), dtr["PathNameKo"].ToString(), dtr["PathNameEn"].ToString(), dtr["PathNameJa"].ToString(),
                                    dtr["PathNameFr"].ToString(), dtr["PathNameZhCn"].ToString(), dtr["PathNameZhTw"].ToString(), PreLang);


                                switch (PreLang)
                                {
                                    case "ko":
                                        name = (dtr["NameKo"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                    case "en":
                                        name = (dtr["NameEn"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                    case "ja":
                                        name = (dtr["NameJa"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                    case "fr":
                                        name = (dtr["NameFr"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                    case "zh-cn":
                                        name = (dtr["NameZhcn"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                    case "zh-tw":
                                        name = (dtr["NameZhtw"].ToString() == "") ? dtr["PathNameBase"].ToString() : name;
                                        break;
                                }

                                SiteMap += name;
                                */
                            }
                        }
                    }
                    break;
                default:
                    UID = CurrentUserInfo.UserID;
                    SiteMap = "";
                    break;
            }

        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-12-20
        /// 작성목적 : 해당 폴더의 공유권한 정보 가져오기
        /// </summary>
        private void ShareInfo()
        {
            BLL.ShareFolder bll = new BLL.ShareFolder();
            DataSet ds = bll.GetShareAuthority(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.UserDepts, FolderID);
            
            if (ds != null)
            {
                // 개인 권한 우선 
                DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");
                if (dr.Length > 0)
                {
                    if (dr[0]["Authority"].ToString() == "3")
                    {
                        CbtnDownload.Visible = true;
                        CbtnUpload.Visible = true;
                        DownCheck = "Y";
                    }
                    else if (dr[0]["Authority"].ToString() == "2")
                    {
                        CbtnDownload.Visible = false;
                        CbtnUpload.Visible = true;
                        DownCheck = "N";
                    }
                    else if (dr[0]["Authority"].ToString() == "1")
                    {
                        CbtnDownload.Visible = true;
                        CbtnUpload.Visible = false;
                        DownCheck = "Y";
                    }
                    else
                    {
                        CbtnDownload.Visible = false;
                        CbtnUpload.Visible = false;
                        DownCheck = "N";
                    }
                }
                else
                {
                    // 부서 권한
                    CbtnDownload.Visible = false;
                    CbtnUpload.Visible = false;
                    DownCheck = "N";

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        if (dbRow["Authority"].ToString() == "3")
                        {                            
                            CbtnDownload.Visible = true;
                            CbtnUpload.Visible = true;
                            DownCheck = "Y";
                            break;
                        }
                        else if (dbRow["Authority"].ToString() == "2")
                        {                            
                            CbtnUpload.Visible = true;
                        }
                        else if (dbRow["Authority"].ToString() == "1")
                        {
                            CbtnDownload.Visible = true;
                            DownCheck = "Y";
                        }
                    }                  
                }
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-12
        /// 작성목적 : 해당 폴더의 공유권한 정보 가져오기
        /// </summary>
        private void PublicInfo()
        {
            BLL.PublicFolder bll = new BLL.PublicFolder();
            DataSet ds = bll.GetPublicAuthority(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.UserDepts, FolderID);

            if (ds != null)
            {                                
                // 개인 권한 우선 
                DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");
                if (dr.Length > 0)
                {
                    if (dr[0]["Authority"].ToString() == "3")
                    {                        
                        CbtnSend.Visible = true;
                        CbtnMove.Visible = true;
                        CbtnCopy.Visible = true;

                        CbtnDownload.Visible = true;
                        CbtnUpload.Visible = true;

                        CbtnDel.Visible = true;
                        CbtnAllDel.Visible = true;

                        DownCheck = "Y";
                    }
                    else if (dr[0]["Authority"].ToString() == "2")
                    {
                        CbtnDownload.Visible = false;
                        CbtnUpload.Visible = true;

                        CbtnDel.Visible = true;
                        CbtnAllDel.Visible = true;

                        DownCheck = "N";
                    }
                    else if (dr[0]["Authority"].ToString() == "1")
                    {
                        CbtnSend.Visible = true;
                        CbtnMove.Visible = true;
                        CbtnCopy.Visible = true;

                        CbtnDownload.Visible = true;
                        CbtnUpload.Visible = false;

                        CbtnDel.Visible = true;
                        CbtnAllDel.Visible = true;

                        DownCheck = "Y";
                    }
                    else
                    {
                        CbtnDownload.Visible = false;
                        CbtnUpload.Visible = false;

                        CbtnDel.Visible = true;
                        CbtnAllDel.Visible = true;

                        DownCheck = "N";
                    }
                }
                else
                {
                    // 부서 권한
                    CbtnDownload.Visible = false;
                    CbtnUpload.Visible = false;
                    DownCheck = "N";

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        if (dbRow["Authority"].ToString() == "3")
                        {
                            CbtnSend.Visible = true;
                            CbtnMove.Visible = true;
                            CbtnCopy.Visible = true;

                            CbtnDownload.Visible = true;
                            CbtnUpload.Visible = true;

                            CbtnDel.Visible = true;
                            CbtnAllDel.Visible = true;

                            DownCheck = "Y";
                            break;
                        }
                        else if (dbRow["Authority"].ToString() == "2")
                        {
                            CbtnUpload.Visible = true;

                            CbtnDel.Visible = true;
                            CbtnAllDel.Visible = true;
                        }
                        else if (dbRow["Authority"].ToString() == "1")
                        {
                            CbtnSend.Visible = true;
                            CbtnMove.Visible = true;
                            CbtnCopy.Visible = true;

                            CbtnDownload.Visible = true;

                            CbtnDel.Visible = true;
                            CbtnAllDel.Visible = true;

                            DownCheck = "Y";
                        }
                    }
                }
            }
        }
        #endregion        

        #region (프로퍼티)

        /// <summary>
        /// SwfUploadGUID
        /// </summary>
        public string SwfUploadGUID
        {
            get { return GetViewStateString("SwfUploadGUID", ""); }
            set { ViewState["SwfUploadGUID"] = value; }
        } 

        #endregion

        #region (이벤트핸들러)
        #endregion
    }
}