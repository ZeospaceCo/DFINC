using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Approval.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary
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
        #region GET
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
            switch (_state.Substring(0, 2).ToUpper())
            {
                case "ZD":
                    rtnval = GetResource("Send");
                    break;
                case "ZH":
                    rtnval = GetResource("ZH");
                    break;
                case "ZP":
                    rtnval = GetResource("Receive");
                    break;
                case "ZQ":
                    rtnval = GetResource("Receive");
                    break;
                case "ZA":
                    rtnval = GetResource("FlowZA2");
                    break;
                case "ZR":
                    rtnval = GetResource("ZR");
                    break;
                case "ZC":
                    rtnval = GetResource("Carboncopy");
                    break;
                default:
                    rtnval = "";
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 작성자 : 
        /// 자주지정하는 결재자 리스트 가져오기 (부서변경된 결재자리스트)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetDraftModifyList(string p)
        {
            try
            {
                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.FormFlow.Temporary.ListModify).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                SetDecryptString(p);

                string pageSize = GetDecryptParam("pageSize", "0"); //페이지에 보여지는 목록수
                string totalItemCount = GetDecryptParam("totalItemCount", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0"); //페이지 넘버

                string rtnSetPaging = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.Draft dal = new DAL.Draft();

                DataTableReader dtr = dal.GetPageModifyList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, setPageingStr[0], setPageingStr[1]);
                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.DraftSequenceInfo info = new DraftSequenceInfo();

                            info.Subject = dtr["Subject"].ToString().ToNull();

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

                            info.ApprovalTag = dtr["ApprovalTag"].ToString().ToNull();
                            info.ApprovalTagStr = GetStateStr(info.ApprovalTag);

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
        /// 자주지정하는 결재자 총개수
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetDraftModifyCnt(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                DAL.Draft dal = new DAL.Draft();
                return dal.GetPageModifyCount(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("GetDraftModifyCnt : " + ex);
                return -1;
            }

        }
        #endregion 

        #region DEL

        /// <summary>
        /// 작성자 : 
        /// 임시저장 결재자 (부서변경된 사용자 건) 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelDraftSeq(string p)
        {
            //파라미터 가져오기
            SetDecryptString(p);

            try
            {
                DAL.DraftSequence dal = new DAL.DraftSequence();
                return dal.DelModifyList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelModifyList : " + ex);
                throw;
            }
        }

        #endregion
    }
}
