using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.EnforceAdmin
{
    /// <summary>
    /// ListSubmitAdmin의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListSubmit : Base.BaseWSApproval
    {
        #region 리스트

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
        /// 상태정보 가져오기
        /// </summary>
        /// <param name="_state"></param>
        /// <returns></returns>
        private string GetStateStr(string _state)
        {
            if (_state == null) return "";

            string rtnval = "";
            switch (_state.ToUpper())
            {
                case "W":
                    rtnval = GetResource("EnforceStateW");
                    break;
                case "R":
                    rtnval = GetResource("EnforceStateR");
                    break;
                case "S":
                    rtnval = GetResource("EnforceStateS");
                    break;
                default:
                    rtnval = GetResource("EnforceStateEtc");
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 시행문 직인리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Folder.EnforceAdmin.ListSubmit).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string recordPerPage = GetDecryptParam("recordPerPage", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");
                string enforceStampIDArr = GetDecryptParam("enforceStampIDArr", "");
                string stampDeptIDArr = GetDecryptParam("stampDeptIDArr", "");

                if (enforceStampIDArr.Equals("") || stampDeptIDArr.Equals(""))
                {
                    return null;
                }

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), recordPerPage.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Enforce biz = new DAL.Enforce();

                #region 날짜

                string startDate = "", endDate = "", getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DataTableReader dtr = biz.GetPageListEnforceSubmitAdmin(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, enforceStampIDArr, stampDeptIDArr, setPageingStr[0], setPageingStr[1], startDate, endDate, searchField, searchValue, order);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.EnforceInfo info = new EnforceInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.EnforceID = dtr["EnforceID"].ToString().ToNumber<int>();
                            info.ReqUserID = dtr["ReqUserID"].ToString().ToNumber<int>();
                            info.ReqDeptCodeID = dtr["ReqDeptCodeID"].ToString().ToNumber<int>();
                            info.ReqUserMail = dtr["ReqUserMail"].ToString().ToNull();
                            info.ReqNumber = dtr["ReqNumber"].ToString().ToNull();
                            info.ReqCount = dtr["ReqCount"].ToString().ToNumber<int>();
                            info.EnforceStampID = dtr["EnforceStampID"].ToString().ToNumber<int>();
                            info.StampDeptID = dtr["StampDeptID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.DocNumber = dtr["DocNumber"].ToString().ToNull();
                            info.ContentHTML = dtr["ContentHTML"].ToString().ToNull();
                            info.Subject = dtr["Subject"].ToString().ToNull();
                            info.Receive = dtr["Receive"].ToString().ToNull();
                            info.Reference = dtr["Reference"].ToString().ToNull();
                            info.EnforceDate = dtr["EnforceDate"].ToString().ToNull();
                            info.ExpFirst = dtr["ExpFirst"].ToString().ToNull();
                            info.ExpMid = dtr["ExpMid"].ToString().ToNull();
                            info.ExpEnd = dtr["ExpEnd"].ToString().ToNull();
                            info.ZipCode = dtr["ZipCode"].ToString().ToNull();
                            info.AddressFirst = dtr["AddressFirst"].ToString().ToNull();
                            info.AddressEnd = dtr["AddressEnd"].ToString().ToNull();
                            info.Phone = dtr["Phone"].ToString().ToNull();
                            info.Fax = dtr["Fax"].ToString().ToNull();
                            info.Homepage = dtr["Homepage"].ToString().ToNull();
                            info.SendType = dtr["SendType"].ToString().ToNull();
                            info.StampType = dtr["StampType"].ToString().ToNull();
                            info.DocType = dtr["DocType"].ToString().ToNull();
                            info.Quantity = dtr["Quantity"].ToString().ToNull();
                            info.Purpose = dtr["Purpose"].ToString().ToNull();
                            info.TargetMail = dtr["TargetMail"].ToString().ToNull();
                            info.TargetMailSubject = dtr["TargetMailSubject"].ToString().ToNull();
                            info.FormPath = dtr["FormPath"].ToString().ToNull();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateStr = string.Empty;

                            info.ConfirmUserID = dtr["ConfirmUserID"].ToString().ToNumber<int>();

                            info.ConfirmDeptCodeID = dtr["ConfirmDeptID"].ToString().ToNumber<int>();

                            if (DateTime.TryParse(dtr["ConfirmDate"].ToString(), out dt))
                                info.ConfirmDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.ConfirmDateStr = string.Empty;

                            info.ConfirmNumber = dtr["ConfirmNumber"].ToString().ToNull();
                            info.ConfirmCount = dtr["ConfirmCount"].ToString().ToNumber<int>();
                            info.ConfirmOpinion = dtr["ConfirmOpinion"].ToString().ToNull();
                            info.State = dtr["State"].ToString().ToNull();
                            info.StateStr = GetStateStr(info.State);
                            info.IsSend = dtr["IsSend"].ToString().ToNull();
                            info.IsSendStr = (info.IsSend.Equals("0")) ? this.GetResource("EnforceIsSend0") : this.GetResource("EnforceIsSend1");

                            info.ReqDeptID = dtr["ReqDeptID"].ToString().ToNumber<int>();
                            info.ReqTitleID = dtr["ReqTitleID"].ToString().ToNumber<int>();
                            info.ReqDutyID = dtr["ReqDutyID"].ToString().ToNumber<int>();

                            #region 신청자 다국어 설정
                            info.ReqNameBase = this.GetLangName(
                                    dtr["ReqNameBase"].ToString().ToNull(), dtr["ReqNameKo"].ToString().ToNull(),
                                    dtr["ReqNameEn"].ToString().ToNull(), dtr["ReqNameJa"].ToString().ToNull(),
                                    dtr["ReqNameFr"].ToString().ToNull(), dtr["ReqNameZhcn"].ToString().ToNull(),
                                    dtr["ReqNameZhtw"].ToString().ToNull(), PreLang);
                            info.ReqDeptBase = this.GetLangName(
                                    dtr["ReqDeptBase"].ToString().ToNull(), dtr["ReqDeptKo"].ToString().ToNull(),
                                    dtr["ReqDeptEn"].ToString().ToNull(), dtr["ReqDeptJa"].ToString().ToNull(),
                                    dtr["ReqDeptFr"].ToString().ToNull(), dtr["ReqDeptZhcn"].ToString().ToNull(),
                                    dtr["ReqDeptZhtw"].ToString().ToNull(), PreLang);
                            info.ReqTitleBase = this.GetLangName(
                                    dtr["ReqTitleBase"].ToString().ToNull(), dtr["ReqTitleKo"].ToString().ToNull(),
                                    dtr["ReqTitleEn"].ToString().ToNull(), dtr["ReqTitleJa"].ToString().ToNull(),
                                    dtr["ReqTitleFr"].ToString().ToNull(), dtr["ReqTitleZhcn"].ToString().ToNull(),
                                    dtr["ReqTitleZhtw"].ToString().ToNull(), PreLang);
                            info.ReqDutyBase = this.GetLangName(
                                    dtr["ReqDutyBase"].ToString().ToNull(), dtr["ReqDutyKo"].ToString().ToNull(),
                                    dtr["ReqDutyEn"].ToString().ToNull(), dtr["ReqDutyJa"].ToString().ToNull(),
                                    dtr["ReqDutyFr"].ToString().ToNull(), dtr["ReqDutyZhcn"].ToString().ToNull(),
                                    dtr["ReqDutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 확인자 다국어 설정
                            info.ConfirmNameBase = this.GetLangName(
                                    dtr["ConfirmNameBase"].ToString().ToNull(), dtr["ConfirmNameKo"].ToString().ToNull(),
                                    dtr["ConfirmNameEn"].ToString().ToNull(), dtr["ConfirmNameJa"].ToString().ToNull(),
                                    dtr["ConfirmNameFr"].ToString().ToNull(), dtr["ConfirmNameZhcn"].ToString().ToNull(),
                                    dtr["ConfirmNameZhtw"].ToString().ToNull(), PreLang);
                            info.ConfirmDeptBase = this.GetLangName(
                                    dtr["ConfirmDeptBase"].ToString().ToNull(), dtr["ConfirmDeptKo"].ToString().ToNull(),
                                    dtr["ConfirmDeptEn"].ToString().ToNull(), dtr["ConfirmDeptJa"].ToString().ToNull(),
                                    dtr["ConfirmDeptFr"].ToString().ToNull(), dtr["ConfirmDeptZhcn"].ToString().ToNull(),
                                    dtr["ConfirmDeptZhtw"].ToString().ToNull(), PreLang);
                            info.ConfirmTitleBase = this.GetLangName(
                                    dtr["ConfirmTitleBase"].ToString().ToNull(), dtr["ConfirmTitleKo"].ToString().ToNull(),
                                    dtr["ConfirmTitleEn"].ToString().ToNull(), dtr["ConfirmTitleJa"].ToString().ToNull(),
                                    dtr["ConfirmTitleFr"].ToString().ToNull(), dtr["ConfirmTitleZhcn"].ToString().ToNull(),
                                    dtr["ConfirmTitleZhtw"].ToString().ToNull(), PreLang);
                            info.ConfirmDutyBase = this.GetLangName(
                                    dtr["ConfirmDutyBase"].ToString().ToNull(), dtr["ConfirmDutyKo"].ToString().ToNull(),
                                    dtr["ConfirmDutyEn"].ToString().ToNull(), dtr["ConfirmDutyJa"].ToString().ToNull(),
                                    dtr["ConfirmDutyFr"].ToString().ToNull(), dtr["ConfirmDutyZhcn"].ToString().ToNull(),
                                    dtr["ConfirmDutyZhtw"].ToString().ToNull(), PreLang);

                            #endregion

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["WriteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
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
        /// 문서 카운트
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
                string searchTerm = GetDecryptParam("searchTerm", "");
                string searchTermValue = GetDecryptParam("searchTermValue", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string enforceStampIDArr = GetDecryptParam("enforceStampIDArr", "");
                string stampDeptIDArr = GetDecryptParam("stampDeptIDArr", "");

                if (enforceStampIDArr.Equals("") || stampDeptIDArr.Equals(""))
                {
                    return 0;
                }

                int icnt = 0;

                #region 날짜

                string startDate = "", endDate = "", getDate = "";

                if (searchTerm == "week" || searchTerm == "month")
                {
                    if (searchTerm == "week")
                        getDate = DateHelpers.GetCultureStartEndDate("week", PreLangReal);
                    else if (searchTerm == "month")
                        getDate = DateHelpers.GetCultureStartEndDate("month", PreLangReal);

                    startDate = DateHelpers.GetCultureDate(getDate.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(getDate.Split(',')[1], "yyyyMMdd", PreLangReal);
                }
                else if (searchTerm == "period")
                {
                    startDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchTermValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DAL.Enforce biz = new DAL.Enforce();
                icnt = biz.GetPageCountEnforceSubmitAdmin(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, enforceStampIDArr, stampDeptIDArr, startDate, endDate, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
