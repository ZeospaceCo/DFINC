using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.EIP.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;

namespace Zeospace.DeskPlus.EIP.Web.Ws.Portal
{
    /// <summary>
    /// GetPortlet의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddPortletUser : Base.BaseWSEIP
    {

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 :
        /// 작성목적 : 개인별 포틀릿 리스트 카운트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("categoryID", "0");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");
                int tabID = GetDecryptParam("tabID", "").ToNumber<int>();

                DAL.Portlet biz = new DAL.Portlet();

                return biz.GetUserPageListCount(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), false, CurrentUserInfo.UserID, "", "", tabID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.EIP.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.EIP.Web.Ws.Portal.AddPortletUser).Assembly;	        //어셈블리 선언

                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                string categoryID = GetDecryptParam("categoryID", "0");
                string searchKey = GetDecryptParam("searchKey", "");
                string searchValue = GetDecryptParam("searchValue", "");

                string order = GetDecryptParam("order", "");
                int tabID = GetDecryptParam("tabID", "").ToNumber<int>();

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                string PhotoName = string.Empty;

                DAL.Portlet biz = new DAL.Portlet();

                DataTableReader dtr = biz.GetUserPageList(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), false, CurrentUserInfo.UserID,                                         
                                         setPageingStr[0], setPageingStr[1],
                                         searchKey, searchValue, order, tabID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.PortletInfo info = new PortletInfo();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.PortletID = dtr["PortletID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();

                            #region 다국어(제목)
                            info.TitleName = this.GetLangName(dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.Description = dtr["Description"].ToString();
                            info.PortletIDCnt = dtr["PortletIDCnt"].ToString().ToNumber<int>();

                            #region 이미지

                            string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "");
                            string thumImgPath = this.ImgPath + "/Icon/rss.gif";

                            info.PhotoName = dtr["PhotoName"].ToString();

                            if (info.PhotoName.Length > 0)
                            {
                                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                                thumImgPath = baseFileDirectory + CurrentUserInfo.CompanyID + "/EIP/Portlet/" + info.PhotoName.Split('.')[0] + "_S." + info.PhotoName.Split('.')[1];
                            }

                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", thumImgPath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.PhotoName = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                            #endregion                            

                            info.FileDel = resMng.GetString("Del");

                            listobj.Add(info);
                        }
                    }

                }
                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 카테고리 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortletCategory(string p)
        {
            try
            {
                SetDecryptString(p);
                
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");

                BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.ParentCategoryID = categoryID;
                info.NameBase = name;
                info.Description = "";
                info.ActiveCategory = "1";
                info.IsAdmin = "0";
                info.WriteUserID = CurrentUserInfo.UserID;                

                DAL.PortletCategory dal = new DAL.PortletCategory();
                dal.Add(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 카테고리 수정하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModPortletCategory(string p)
        {
            try
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");

                BE.PortletCategoryInfo info = new BE.PortletCategoryInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID;
                info.NameBase = name;
                info.Description = "";
                info.ActiveCategory = "1";
                info.IsAdmin = "0";
                info.WriteUserID = CurrentUserInfo.UserID;

                DAL.PortletCategory dal = new DAL.PortletCategory();
                dal.Mod(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 카테고리 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortletCategory(string p)
        {
            try
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                if (categoryID > 0)
                {
                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    dal.Del(CurrentUserInfo.CompanyID, categoryID);
                }

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 목적 : 상위폴더명 및 폴더명 가져오기
        /// </summary>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetCategoryName(string p)
        {
            try
            {
                SetDecryptString(p);
                string categoryName = "";
                string pCategoryName = "";

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string gubun = GetDecryptParam("gubun", "add");

                if (categoryID > 0)
                {
                    DAL.PortletCategory dal = new DAL.PortletCategory();
                    DataSet ds = dal.GetPortletCategory(CurrentUserInfo.CompanyID, categoryID, CurrentUserInfo.UserID, false, true);

                    if (ds != null)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (gubun == "add")
                            {
                                pCategoryName = dr["PathNameBase"].ToString().Substring(1, dr["PathNameBase"].ToString().Length - 1); //상위폴더
                                categoryName = "";//현폴더
                            }
                            else if (gubun == "mod")
                            {
                                categoryName = dr["NameBase"].ToString(); //현폴더                   
                                
                                if (dr["ParentCategoryID"].ToString().ToNumber<int>() > 0)
                                {
                                    DataSet pds = dal.GetPortletCategory(CurrentUserInfo.CompanyID,
                                        dr["ParentCategoryID"].ToString().ToNumber<int>(), CurrentUserInfo.UserID, false, true);

                                    if (pds != null)
                                    {
                                        foreach (DataRow pdr in pds.Tables[0].Rows)
                                        {
                                            pCategoryName = pdr["PathNameBase"].ToString().Substring(1, pdr["PathNameBase"].ToString().Length - 1);
                                        }
                                    }
                                }
                            }
                            else if (gubun == "rss")
                            {
                                categoryName = dr["NameBase"].ToString(); //현폴더
                            }
                        }
                    }
                }

                return pCategoryName + "#" + categoryName.ToReverseXSS();

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 카테고리 추가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddPortlet(string p)
        {
            try
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                string name = GetDecryptParam("name", "");
                string dataUrl = GetDecryptParam("dataUrl", "");

                BE.PortletInfo info = new BE.PortletInfo();

                info.CompanyID = CurrentUserInfo.CompanyID;
                info.CategoryID = categoryID;

                info.NameBase = name.Trim();
                info.NameKo = name.Trim();
                info.NameEn = name.Trim();
                info.NameJa = name.Trim();
                info.NameFr = name.Trim();
                info.NameZhcn = name.Trim();
                info.NameZhtw = name.Trim();

                info.PortletCode = "";
                //포틀릿종류 ( T : 탭포틀릿 / P : 일반 포틀릿)
                info.PortletType = "P";
                //UI 형식 (T : 템플릿/ L: 내부페이지Layer/ F : IFrame)
                info.UIType = "L";
                info.UITemplate = "";
                info.UIURL = "/DeskPlusEIP/Portlet/Template/List.ascx";
                info.DataType = "R";
                info.DataURL = dataUrl;
                info.LinkType = "P";
                info.LinkPopupSize = "|";
                info.LinkURL = "";
                info.IsPesonalization = "1";
                info.ActivePortlet = "1";
                info.SendMethod = "0";
                info.Description = name;
                info.PhotoName = "";
                info.IsAdmin = "0"; //사용자
                info.WriteUserID = CurrentUserInfo.UserID;

                DAL.Portlet dal = new DAL.Portlet();
                dal.Add(info);

                return true;

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shimpark
        /// 작성일자 : 2012-02-08
        /// 작성목적 : 포틀릿 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPortlet(string p)
        {
            try
            {
                SetDecryptString(p);

                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int portletID = GetDecryptParam("portletID", "0").ToNumber<int>();                

                DAL.Portlet dal = new DAL.Portlet();
                dal.Del(CurrentUserInfo.CompanyID, categoryID, portletID);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}
