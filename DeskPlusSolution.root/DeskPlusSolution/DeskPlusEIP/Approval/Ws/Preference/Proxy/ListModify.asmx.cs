using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Proxy
{
    /// <summary>
    /// ListModify의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListModify : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : 
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
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
        /// 작성자 : 
        /// 대결자 페이징 총개수
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageListCnt(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                DAL.Proxy list = new DAL.Proxy();
                return list.GetModifyCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetFavoriteListCnt : " + ex);
                return -1;
            }

        }


        /// <summary>
        /// 작성자 : 
        /// 대결자 페이징 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0"); //페이지에 보여지는 목록수
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0"); //페이지 넘버

                HttpContext context = HttpContext.Current;
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Proxy biz = new DAL.Proxy();

                DataTableReader dtr = biz.GetModifyList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, setPageingStr[0], setPageingStr[1]);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.ProxyInfo info = new BE.ProxyInfo();

                            //info.ApprovalUserSeq = dtr["ROW"].ToString().ToNumber<int>();
                            info.ProxyID = dtr["ProxyID"].ToString().ToNumber<int>();
                            info.ProxyUserID = dtr["ProxyUserID"].ToString().ToNumber<int>();
                            info.ProxyDeptID = dtr["ProxyDeptID"].ToString().ToNumber<int>();

                            info.ProxyBaseDept = dtr["BaseDept"].ToString().ToNumber<int>();
                            info.ProxyMail = dtr["Mail"].ToString();
                            info.ProxyMobilePhone = dtr["MobilePhone"].ToString();
                            info.ProxyOfficePhone = dtr["OfficePhone"].ToString();
                            info.ProxyState = dtr["IsProxy"].ToString();

                            #region 사진 이미지 설정
                            string ProxyPhotoName = dtr["PhotoImage"].ToString();
                            info.ProxyPhotoName = "";
                            string imgPath = "";
                            string imgTag = "";
                            string encParams = "";
                            Dictionary<string, string> paraDic = null;

                            if (!string.IsNullOrEmpty(ProxyPhotoName) && ProxyPhotoName.Length > 0)
                            {
                                imgPath = baseFileDirectory + "{0}/Users/{1}/Picture/" + ProxyPhotoName;
                                imgPath = string.Format(imgPath, CurrentUserInfo.CompanyID, info.ProxyUserID);

                                if (System.IO.File.Exists(context.Server.MapPath(imgPath)))
                                {
                                    #region 스크린샷
                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("filePath", imgPath);
                                    encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                    imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                    #endregion

                                    imgTag = "<img src='{0}' width='52' height='52' />";
                                    imgTag = string.Format(imgTag, imgPath);
                                    info.ProxyPhotoName = imgTag;
                                }
                            }

                            if (info.ProxyPhotoName.Length == 0)
                            {
                                #region 스크린샷
                                paraDic = new Dictionary<string, string>();
                                paraDic.Add("filePath", baseImageDirectory + "C/Img/Icon/ico_noimg.gif");
                                encParams = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                                imgPath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;
                                #endregion

                                info.ProxyPhotoName = "<img src='" + imgPath + "' width='52' height='52' alt='' title='' />";
                            }

                            #endregion

                            #region 직급 다국어 설정
                            info.ProxyTitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 이름 다국어 설정
                            info.ProxyUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.ProxyDeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.ProxyDutyName = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

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
        /// 작성자 : 
        /// 대결자 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelProxyUser(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                Zeospace.DeskPlus.Approval.DAL.Proxy dal = new DAL.Proxy();
                return dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelProxyUser : " + ex);
                throw;
            }
        }
    }
}
