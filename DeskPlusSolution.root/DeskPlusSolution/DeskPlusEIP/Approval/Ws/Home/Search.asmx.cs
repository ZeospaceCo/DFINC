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
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Home
{
    /// <summary>
    /// Search의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Search : Base.BaseWSApproval
    {
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


        #region 검색


        /// <summary>
        /// 작성자 : 심재운
        /// 게시판 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageListDocAll(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchValue = GetDecryptParam("searchValue", "");


                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Document biz = new DAL.Document();
                DAL.Home dal = new DAL.Home();
                DAL.AttachFile dalAttach = new DAL.AttachFile();

                DataTableReader dtr = dal.GetPageListDocAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1], searchValue);

                string baseImageDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseImageDirectory", "/DeskPlusEIP/Design/");


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.Approval.BE.DocumentInfo info = new BE.DocumentInfo();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                            info.DocNumber = dtr["DocNumber"].ToString();

                            #region 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptBase = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleBase = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyBase = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                                info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteTimeStr = string.Empty;


                            if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                                info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.CompleteDateStr = string.Empty;

                            info.ROW = (totalItemCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.Filecount = dalAttach.GetAttachFileCntByPath(info.CompanyID, info.ChildDocID);

                            info.Subject = dtr["Subject"].ToString();//제목                            

                            #region 날짜 비교(New Img)
                            DateTime writeDate = Convert.ToDateTime(dtr["CompleteDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(writeDate);
                            int approvalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                            info.ShowNew = (gap.Days <= approvalNewShowDay) ? "Y" : "N";
                            #endregion

                            //info.ImpoCnt = dtr["ImpoCnt"].ToString().ToNumber<int>();
                            info.IsGuard = dtr["IsGuard"].ToString();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetPageListCountDocAll(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchValue = GetDecryptParam("searchValue", "");


                int icnt = 0;

                DAL.Home dal = new DAL.Home();
                icnt = dal.GetPageListCountDocAll(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 문서 권한 체크
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetIsAuthDoc(string p)
        {
            try
            {
                SetDecryptString(p);
                string documentID = GetDecryptParam("documentID", "");

                bool rtnval = false;

                DAL.Document dal = new DAL.Document();
                rtnval = dal.GetIsAuthDoc(CurrentUserInfo.CompanyID, documentID.ToNumber<int>(), CurrentUserInfo.DeptID, CurrentUserInfo.CodeTitleID, CurrentUserInfo.UserID);

                return rtnval;
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
