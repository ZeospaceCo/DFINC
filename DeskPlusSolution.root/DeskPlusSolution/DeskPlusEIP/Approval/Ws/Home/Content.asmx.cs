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
    /// Content의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Content : Base.BaseWSApproval
    {
        #region 공통
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

        private string GetSectionStr(string _section)
        {
            string rtnval = "";
            switch (_section.ToUpper())
            {
                case "APPROVE":
                    rtnval = this.GetResource("HomeTabTit02");      //진행
                    break;
                case "COMPLETE":
                    rtnval = this.GetResource("HomeTabTit03");      //완료
                    break;
                case "CARBON":
                    rtnval = this.GetResource("HomeTabTit03");      //참조
                    break;
                default:
                    rtnval = "";
                    break;
            }

            //switch (_state.ToUpper())
            //{ 
            //    case"W":
            //        rtnval = this.GetResource("StatusW");
            //        break;
            //    case "I":
            //        rtnval = this.GetResource("StatusI");
            //        break;
            //    case "R":
            //        rtnval = this.GetResource("StatusR");
            //        break;
            //    case "D":
            //        rtnval = this.GetResource("StatusD");
            //        break;
            //    case "C":
            //        rtnval = this.GetResource("StatusC");
            //        break;
            //    case "H":
            //        rtnval = this.GetResource("StatusH");
            //        break;
            //    case "P":
            //        rtnval = this.GetResource("StatusW");
            //        break;
            //    default :
            //        rtnval = "";
            //        break;
            //}
            return rtnval;
        }

        private string GetClassInfo(string _section)
        {
            string rtnval = "";
            switch (_section.ToUpper())
            {
                case "APPROVE":
                    rtnval = "doc_ing";      //진행
                    break;
                case "COMPLETE":
                    rtnval = "doc_end";      //완료
                    break;
                case "CARBON":
                    //rtnval = "doc_ref";      //참조
                    rtnval = "doc_end";
                    break;
                default:
                    rtnval = "";
                    break;
            }
            return rtnval;
        }

        private string GetApprovalState(string _state)
        {
            string rtnval = "";
            switch (_state.ToUpper())
            {
                case "W":
                    rtnval = this.GetResource("StatusW");
                    break;
                case "I":
                    rtnval = this.GetResource("StatusI");
                    break;
                case "R":
                    rtnval = this.GetResource("StatusR");
                    break;
                case "D":
                    rtnval = this.GetResource("StatusD");
                    break;
                case "C":
                    rtnval = this.GetResource("StatusC");
                    break;
                case "H":
                    rtnval = this.GetResource("StatusH");
                    break;
                case "P":
                    rtnval = this.GetResource("StatusW");
                    break;
                default:
                    rtnval = "";
                    break;
            }
            return rtnval;
        }

        private string GetShowNew(DateTime _dt)
        {
            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            TimeSpan gap = todayDate.Subtract(_dt);
            int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "BoardNewShowDay", "0").ToNumber<int>();

            return (gap.Days <= boardNewShowDay) ? "Y" : "N";
        }
        #endregion

        #region 자주사용하는양식

        /// <summary>
        /// 자주사용하는양식
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavForm(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            DAL.FavoriteForm fDal = new DAL.FavoriteForm();
            DataTableReader dtr = fDal.GetFavoriteFormList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);
            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.FavoriteFormInfo info = new BE.FavoriteFormInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.FavoriteFormID = dtr["FavoriteFormID"].ToString().ToNumber<int>();
                        info.FormID = dtr["FormID"].ToString().ToNumber<int>();
                        info.FormBase = this.GetLangName(
                                        dtr["FormBase"].ToString().ToNull(), dtr["FormKo"].ToString().ToNull(), dtr["FormEn"].ToString().ToNull()
                                        , dtr["FormJa"].ToString().ToNull(), dtr["FormFr"].ToString().ToNull(), dtr["FormZhcn"].ToString().ToNull()
                                        , dtr["FormZhtw"].ToString().ToNull(), PreLang);
                        info.NameBase = this.GetLangName(
                                        dtr["FolderBase"].ToString().ToNull(), dtr["FolderKo"].ToString().ToNull(), dtr["FolderEn"].ToString().ToNull()
                                        , dtr["FolderJa"].ToString().ToNull(), dtr["FolderFr"].ToString().ToNull(), dtr["FolderZhcn"].ToString().ToNull()
                                        , dtr["FolderZhtw"].ToString().ToNull(), PreLang);
                        info.PathNameBase = this.GetLangName(
                                        dtr["PathBase"].ToString().ToNull(), dtr["PathKo"].ToString().ToNull(), dtr["PathEn"].ToString().ToNull()
                                        , dtr["PathJa"].ToString().ToNull(), dtr["PathFr"].ToString().ToNull(), dtr["PathZhcn"].ToString().ToNull()
                                        , dtr["PathZhtw"].ToString().ToNull(), PreLang);

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        #endregion

        #region 전체/결재대상/완료문서/참조문서

        /// <summary>
        /// 값 셋팅
        /// </summary>
        /// <param name="_dtr"></param>
        /// <returns></returns>
        public List<object> SetObjVals(DataTableReader _dtr)
        {
            try
            {
                List<object> uobj = new List<object>();

                if (_dtr != null)
                {
                    if (_dtr.HasRows)
                    {
                        DateTime dt;
                        while (_dtr.Read())
                        {
                            BE.DocumentProcessInfo info = new DocumentProcessInfo();

                            info.CompanyID = _dtr["CompanyID"].ToString().ToNumber<int>();
                            info.DocumentID = _dtr["DocumentID"].ToString().ToNumber<int>();
                            info.ParentDocumentID = _dtr["ParentDocumentID"].ToString().ToNumber<int>();
                            info.ChildDocID = _dtr["LastDocID"].ToString().ToNumber<int>();
                            info.GroupID = _dtr["GroupID"].ToString().ToNumber<int>();
                            info.Number3 = _dtr["Number3"].ToString();
                            info.AccountTag = _dtr["AccountTag"].ToString();
                            info.ApprovalState = _dtr["ApprovalState"].ToString();
                            info.ApprovalStateStr = GetApprovalState(info.ApprovalState);       // 결재정보
                            info.Subject = _dtr["Subject"].ToString();                  //제목                            
                            info.Section = _dtr["Section"].ToString();                  //Approve, Complete, Carbon
                            info.SectionStr = GetSectionStr(info.Section);              // 진행,완료,참조
                            info.SectionStr = (info.ApprovalState.Equals("R")) ? info.ApprovalStateStr : info.SectionStr;   //반려인 경우는 반려로 표기.
                            info.ClassInfo = GetClassInfo(info.Section);                // class
                            info.IsGuard = _dtr["IsGuard"].ToString();
                            info.Prob = 0;

                            if (info.Section.ToUpper().Equals("APPROVE"))
                            {
                                info.IsComplete = "N";

                                #region 결재대상

                                // 결재페이지 이동 파라미터
                                info.Sequence = _dtr["Sequence"].ToString().ToNumber<int>();
                                info.TargetUserID = _dtr["TargetUserID"].ToString().ToNumber<int>();
                                info.DeptID = _dtr["DeptID"].ToString().ToNumber<int>();
                                info.ApprovalTag = _dtr["ApprovalTag"].ToString();
                                info.MyTurn = _dtr["MyTurn"].ToString();

                                //상신일
                                if (DateTime.TryParse(_dtr["WriteTime"].ToString(), out dt))
                                    info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                                else
                                    info.WriteTimeStr = string.Empty;

                                // 결재진행률
                                if (_dtr["SeqCnt"].ToString().ToNumber<int>() > 0)
                                {
                                    info.Prob = (float.Parse(_dtr["AppCnt"].ToString()) / float.Parse(_dtr["SeqCnt"].ToString())) * 100;
                                    info.Prob = Math.Round(info.Prob); // Math.Ceiling(doubleValue)-올림, Math.Round(doubleValue)-반올림, Math.Truncate(doubleValue)-버림
                                }
                                else
                                {
                                    info.Prob = 0;
                                }

                                #endregion
                            }
                            else
                            {
                                info.IsComplete = "Y";

                                #region 완료문서/참조문서

                                // 결재페이지 이동 파라미터
                                info.Sequence = 0;
                                info.TargetUserID = 0;
                                info.DeptID = 0;
                                info.ApprovalTag = "";
                                info.MyTurn = "N";

                                //완료일
                                if (DateTime.TryParse(_dtr["CompleteDate"].ToString(), out dt))
                                    info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                                else
                                    info.CompleteDateStr = string.Empty;

                                // 결재진행률
                                if (_dtr["SeqCnt"].ToString().ToNumber<int>() > 0 && info.ApprovalState.Equals("R"))
                                {
                                    info.Prob = (float.Parse(_dtr["AppCnt"].ToString()) / float.Parse(_dtr["SeqCnt"].ToString())) * 100;
                                    info.Prob = Math.Round(info.Prob); // Math.Ceiling(doubleValue)-올림, Math.Round(doubleValue)-반올림, Math.Truncate(doubleValue)-버림
                                }
                                else
                                {
                                    info.Prob = 100;
                                }

                                #endregion
                            }

                            #region 날짜 비교(New Img) - 기준값 : 결재대상(상신일) / 그외 (완료일)
                            //DateTime showDate = Convert.ToDateTime(_dtr["ShowDate"].ToString());
                            DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                            TimeSpan gap = todayDate.Subtract(dt);
                            int ApprovalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();
                            info.ShowNew = (gap.Days <= ApprovalNewShowDay) ? "Y" : "N";
                            #endregion

                            info.UserID = _dtr["UserID"].ToString().ToNumber<int>();
                            info.NameBase = this.GetLangName(
                                _dtr["NameBase"].ToString().ToNull(), _dtr["NameKo"].ToString().ToNull(),
                                _dtr["NameEn"].ToString().ToNull(), _dtr["NameJa"].ToString().ToNull(),
                                _dtr["NameFr"].ToString().ToNull(), _dtr["NameZhcn"].ToString().ToNull(),
                                _dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            #region 결재라인처리

                            DAL.Home dal = new DAL.Home();
                            DataTableReader dtrseq = dal.GetSeqLine(info.CompanyID, info.DocumentID, CurrentUserInfo.UserID, info.IsComplete);

                            List<BE.SequenceInfo> seqlist = new List<BE.SequenceInfo>();
                            if (dtrseq != null)
                            {
                                if (dtrseq.HasRows)
                                {
                                    while (dtrseq.Read())
                                    {
                                        BE.SequenceInfo ssinfo = new SequenceInfo();

                                        ssinfo.CompanyID = dtrseq["CompanyID"].ToString().ToNumber<int>();
                                        ssinfo.DocumentID = dtrseq["DocumentID"].ToString().ToNumber<int>();
                                        ssinfo.Sequence = dtrseq["Sequence"].ToString().ToNumber<int>();
                                        ssinfo.ApprovalTag = dtrseq["ApprovalTag"].ToString().ToNull();
                                        ssinfo.UserID = dtrseq["UserID"].ToString().ToNumber<int>();
                                        ssinfo.NameBase = dtrseq["NameBase"].ToString().ToNull();
                                        ssinfo.NameKo = dtrseq["NameKo"].ToString().ToNull();
                                        ssinfo.NameEn = dtrseq["NameEn"].ToString().ToNull();
                                        ssinfo.NameJa = dtrseq["NameJa"].ToString().ToNull();
                                        ssinfo.NameFr = dtrseq["NameFr"].ToString().ToNull();
                                        ssinfo.NameZhcn = dtrseq["NameZhcn"].ToString().ToNull();
                                        ssinfo.NameZhtw = dtrseq["NameZhtw"].ToString().ToNull();
                                        ssinfo.ManyName = dtrseq["ManyName"].ToString().ToNull();
                                        ssinfo.DeptCodeID = dtrseq["DeptCodeID"].ToString().ToNumber<int>();
                                        ssinfo.MyTurn = dtrseq["MyTurn"].ToString().ToNull();

                                        ssinfo.ApprovalDate = (dtrseq["ApprovalDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dtrseq["ApprovalDate"];
                                        ssinfo.ApprovalDateYMD = dtrseq["ApprovalDateYMD"].ToString().ToNull();
                                        ssinfo.ApprovalDateStr = (ssinfo.ApprovalDate.Year.Equals(1)) ? "&nbsp;" : ssinfo.ApprovalDate.Month.ToString().PadLeft(2, '0') + "." + ssinfo.ApprovalDate.Day.ToString().PadLeft(2, '0') + " " + ssinfo.ApprovalDate.Hour.ToString().PadLeft(2, '0') + ":" + ssinfo.ApprovalDate.Minute.ToString().PadLeft(2, '0');

                                        ssinfo.ApprovalType = dtrseq["ApprovalType"].ToString().ToNull().Trim();

                                        ssinfo.Opinion = dtrseq["Opinion"].ToString().ToNull();
                                        ssinfo.Proxy = dtrseq["Proxy"].ToString().ToNull();
                                        ssinfo.ProxyUserID = dtrseq["ProxyUserID"].ToString().ToNumber<int>();
                                        ssinfo.ProxyNameBase = dtrseq["ProxyNameBase"].ToString().ToNull();
                                        ssinfo.ProxyNameKo = dtrseq["ProxyNameKo"].ToString().ToNull();
                                        ssinfo.ProxyNameEn = dtrseq["ProxyNameEn"].ToString().ToNull();
                                        ssinfo.ProxyNameJa = dtrseq["ProxyNameJa"].ToString().ToNull();
                                        ssinfo.ProxyNameFr = dtrseq["ProxyNameFr"].ToString().ToNull();
                                        ssinfo.ProxyNameZhcn = dtrseq["ProxyNameZhcn"].ToString().ToNull();
                                        ssinfo.ProxyNameZhtw = dtrseq["ProxyNameZhtw"].ToString().ToNull();
                                        ssinfo.ProxyManyName = dtrseq["ProxyManyName"].ToString().ToNull();

                                        ssinfo.P_DeptBase = dtrseq["P_DeptBase"].ToString().ToNull();
                                        ssinfo.P_DeptKo = dtrseq["P_DeptKo"].ToString().ToNull();
                                        ssinfo.P_DeptEn = dtrseq["P_DeptEn"].ToString().ToNull();
                                        ssinfo.P_DeptJa = dtrseq["P_DeptJa"].ToString().ToNull();
                                        ssinfo.P_DeptFr = dtrseq["P_DeptFr"].ToString().ToNull();
                                        ssinfo.P_DeptZhcn = dtrseq["P_DeptZhcn"].ToString().ToNull();
                                        ssinfo.P_DeptZhtw = dtrseq["P_DeptZhtw"].ToString().ToNull();

                                        ssinfo.P_TitleBase = dtrseq["P_TitleBase"].ToString().ToNull();
                                        ssinfo.P_TitleKo = dtrseq["P_TitleKo"].ToString().ToNull();
                                        ssinfo.P_TitleEn = dtrseq["P_TitleEn"].ToString().ToNull();
                                        ssinfo.P_TitleJa = dtrseq["P_TitleJa"].ToString().ToNull();
                                        ssinfo.P_TitleFr = dtrseq["P_TitleFr"].ToString().ToNull();
                                        ssinfo.P_TitleZhcn = dtrseq["P_TitleZhcn"].ToString().ToNull();
                                        ssinfo.P_TitleZhtw = dtrseq["P_TitleZhtw"].ToString().ToNull();

                                        #region ####  부서,직급,직책 다국어 시작 #######

                                        ssinfo.DeptID = dtrseq["DeptID"].ToString().ToNumber<int>();
                                        ssinfo.DeptBase = dtrseq["DeptBase"].ToString().ToNull();
                                        ssinfo.DeptKo = dtrseq["DeptKo"].ToString().ToNull();
                                        ssinfo.DeptEn = dtrseq["DeptEn"].ToString().ToNull();
                                        ssinfo.DeptJa = dtrseq["DeptJa"].ToString().ToNull();
                                        ssinfo.DeptFr = dtrseq["DeptFr"].ToString().ToNull();
                                        ssinfo.DeptZhcn = dtrseq["DeptZhcn"].ToString().ToNull();
                                        ssinfo.DeptZhtw = dtrseq["DeptZhtw"].ToString().ToNull();
                                        ssinfo.TitleID = dtrseq["TitleID"].ToString().ToNumber<int>();
                                        ssinfo.TitleBase = dtrseq["TitleBase"].ToString().ToNull();
                                        ssinfo.TitleKo = dtrseq["TitleKo"].ToString().ToNull();
                                        ssinfo.TitleEn = dtrseq["TitleEn"].ToString().ToNull();
                                        ssinfo.TitleJa = dtrseq["TitleJa"].ToString().ToNull();
                                        ssinfo.TitleFr = dtrseq["TitleFr"].ToString().ToNull();
                                        ssinfo.TitleZhcn = dtrseq["TitleZhcn"].ToString().ToNull();
                                        ssinfo.TitleZhtw = dtrseq["TitleZhtw"].ToString().ToNull();
                                        ssinfo.DutyID = dtrseq["DutyID"].ToString().ToNumber<int>();
                                        ssinfo.DutyBase = dtrseq["DutyBase"].ToString().ToNull();
                                        ssinfo.DutyKo = dtrseq["DutyKo"].ToString().ToNull();
                                        ssinfo.DutyEn = dtrseq["DutyEn"].ToString().ToNull();
                                        ssinfo.DutyJa = dtrseq["DutyJa"].ToString().ToNull();
                                        ssinfo.DutyFr = dtrseq["DutyFr"].ToString().ToNull();
                                        ssinfo.DutyZhcn = dtrseq["DutyZhcn"].ToString().ToNull();
                                        ssinfo.DutyZhtw = dtrseq["DutyZhtw"].ToString().ToNull();

                                        #endregion ####  부서,직급,직책 다국어 시작 #######

                                        ssinfo.ClassInfo = "m_apr";
                                        ssinfo.ClassInfo = (ssinfo.UserID.Equals(CurrentUserInfo.UserID)) ? "m_me" : ssinfo.ClassInfo;
                                        ssinfo.ClassInfo = (ssinfo.MyTurn.Equals("Y")) ? "m_now" : ssinfo.ClassInfo;

                                        seqlist.Add(ssinfo);
                                    }
                                }
                            }
                            info.SequenceList = seqlist;

                            #endregion

                            uobj.Add(info);
                        }
                    }
                }

                return uobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Home-Default.asmx", ex);
                throw;
            }
        }

        /// <summary>
        /// 전체
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListTotal(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            // 기본탭
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            envdal.ApprovalHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "ListTotal");

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetListTotal(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

            return SetObjVals(dtr);
        }

        /// <summary>
        /// 결재대상
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListApprove(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            // 기본탭
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            envdal.ApprovalHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "ListApprove");

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetListApprove(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            return SetObjVals(dtr);
        }

        /// <summary>
        /// 완료문서
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListComplete(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            // 기본탭
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            envdal.ApprovalHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "ListComplete");

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetListComplete(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID);

            return SetObjVals(dtr);
        }

        /// <summary>
        /// 참조문서
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListCarbon(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            // 기본탭
            Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
            envdal.ApprovalHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "ListCarbon");

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetListCarbon(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID);

            return SetObjVals(dtr);
        }

        #endregion

        #region 최근 첨부파일 사용내역

        /// <summary>
        /// 최근 첨부파일 사용내역
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAttach(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            string totalItemCount = GetDecryptParam("totCnt", "0");
            string pageSize = GetDecryptParam("pageSize", "0");                 //페이지에 보여지는 목록수
            string pageSizePageNo = GetDecryptParam("pageNo", "0");             //페이지 넘버

            string[] arrpage = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>()).Split(',');

            List<object> uobj = new List<object>();

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetAttach(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, CurrentUserInfo.UserID, arrpage[0].ToString().ToNumber<int>(), arrpage[1].ToString().ToNumber<int>());
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AttachFileInfo info = new AttachFileInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                        info.AttachFileID = dtr["AttachFileID"].ToString().ToNumber<int>();

                        info.FilePath = dtr["FilePath"].ToString();
                        info.FileName = dtr["FileName"].ToString();
                        info.FileGUID = dtr["FileGUID"].ToString();

                        Dictionary<string, string> paraDic = new Dictionary<string, string>();
                        paraDic.Add("filePath", info.FilePath + info.FileGUID);
                        paraDic.Add("fileName", info.FileName);
                        info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                        info.FileLength = dtr["FileLength"].ToString().ToNumber<int>();
                        info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(dtr["FileLength"].ToString()) / 1024) + " KB";

                        info.FileType = dtr["FileType"].ToString().Replace(".", "");

                        // class정보로 대체 (For,아이콘)
                        string tmpFileType = info.FileType;
                        if (info.FileType.ToLower() == "png")
                        {
                            tmpFileType = "jpg";
                        }
                        if (info.FileType.ToLower() == "txt")
                        {
                            tmpFileType = "doc";
                        }
                        info.ImageType = tmpFileType.ToLower() + "_type";

                        DateTime dt;
                        if (DateTime.TryParse(dtr["CompleteDate"].ToString(), out dt))
                            info.CompleteDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.CompleteDateStr = string.Empty;

                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(dt);
                        int ApprovalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();
                        info.ShowNew = (gap.Days <= ApprovalNewShowDay) ? "Y" : "N";

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }
        #endregion

        #region 결재의견 리스트

        private string GetSection(string _section)
        {
            string rtnval = "";
            switch (_section.ToUpper())
            {
                //구분(1:상신.결재/2:첨언추가,3:전달, 4:업무톡)

                case "1":
                    rtnval = this.GetResource("ShortReplySec1");    //결재 -> 첨언
                    break;
                case "2":
                    rtnval = this.GetResource("ShortReplySec2");    //의견추가 -> 추가첨언
                    break;
                case "3":
                    rtnval = this.GetResource("ShortReplySec3");    //전달
                    break;
                case "4":
                    rtnval = this.GetResource("ShortReplySec4");    //업무톡 -> 댓글
                    break;
                default:
                    rtnval = this.GetResource("ShortReplySec");     //기타
                    break;
            }
            return rtnval;
        }

        /// <summary>
        /// 결재의견 리스트
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortReply(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            string pageSize = GetDecryptParam("pageSize", "0");                 //페이지에 보여지는 목록수
            string totalItemCount = GetDecryptParam("totCnt", "0");
            string pageSizePageNo = GetDecryptParam("pageNo", "0");             //페이지 넘버

            string[] arrpage = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>()).Split(',');

            DAL.Home dal = new DAL.Home();
            DataTableReader dtr = dal.GetShortReply(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, arrpage[0].ToString().ToNumber<int>(), arrpage[1].ToString().ToNumber<int>());

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ShortReplyInfo info = new ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();

                        //구분(1:상신.결재/ 2:첨언추가 / 3:전달 / 4:업무톡)
                        info.Section = dtr["Section"].ToString();
                        info.SectionStr = GetSection(dtr["Section"].ToString());

                        //info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.Content = dtr["Content"].ToString().ToReverseXSS();
                        info.Content = info.Content.Replace("\n", "<br>");

                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.IsGuard = dtr["IsGuard"].ToString();

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

                        #region 사진경로처리
                        if (dtr["PhotoImage"].ToString().ToNull().Equals(""))
                        {
                            info.PhotoImage = "/DeskPlusEIP/Design/C/Img/Home/noimg.gif";
                        }
                        else
                        {
                            string path = "/DeskPlusEIP/FileData/" + dtr["CompanyID"].ToString() + "/Users/" + dtr["UserID"].ToString() + "/Picture/" + dtr["PhotoImage"].ToString();
                            if (File.Exists(Server.MapPath(path)))
                            {
                                info.PhotoImage = path;
                            }
                            else
                            {
                                info.PhotoImage = "/DeskPlusEIP/Design/C/Img/Home/noimg.gif";
                            }
                        }
                        #endregion

                        DateTime dt;
                        if (DateTime.TryParse(dtr["WriteTime"].ToString(), out dt))
                            info.WriteTimeStr = DateHelpers.GetCultureDate(dt, PreLangReal);
                        else
                            info.WriteTimeStr = string.Empty;

                        #region 날짜 비교(New Img)
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(dt);
                        int ApprovalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= ApprovalNewShowDay) ? "Y" : "N";
                        #endregion

                        info.Subject = dtr["Subject"].ToString();//제목   

                        #region 상신자정보
                        info.AccountUserID = dtr["AccountUserID"].ToString().ToNumber<int>();
                        info.AccountDeptID = dtr["AccountDeptID"].ToString().ToNumber<int>();
                        info.AccountTitleID = dtr["AccountTitleID"].ToString().ToNumber<int>();
                        info.AccountDutyID = dtr["AccountDutyID"].ToString().ToNumber<int>();

                        info.AccountNameBase = this.GetLangName(
                                dtr["AccountNameBase"].ToString().ToNull(), dtr["AccountNameKo"].ToString().ToNull(),
                                dtr["AccountNameEn"].ToString().ToNull(), dtr["AccountNameJa"].ToString().ToNull(),
                                dtr["AccountNameFr"].ToString().ToNull(), dtr["AccountNameZhcn"].ToString().ToNull(),
                                dtr["AccountNameZhtw"].ToString().ToNull(), PreLang);

                        info.AccountDeptBase = this.GetLangName(
                                dtr["AccountDeptBase"].ToString().ToNull(), dtr["AccountDeptKo"].ToString().ToNull(),
                                dtr["AccountDeptEn"].ToString().ToNull(), dtr["AccountDeptJa"].ToString().ToNull(),
                                dtr["AccountDeptFr"].ToString().ToNull(), dtr["AccountDeptZhcn"].ToString().ToNull(),
                                dtr["AccountDeptZhtw"].ToString().ToNull(), PreLang);

                        info.AccountTitleBase = this.GetLangName(
                                dtr["AccountTitleBase"].ToString().ToNull(), dtr["AccountTitleKo"].ToString().ToNull(),
                                dtr["AccountTitleEn"].ToString().ToNull(), dtr["AccountTitleJa"].ToString().ToNull(),
                                dtr["AccountTitleFr"].ToString().ToNull(), dtr["AccountTitleZhcn"].ToString().ToNull(),
                                dtr["AccountTitleZhtw"].ToString().ToNull(), PreLang);

                        info.AccountDutyBase = this.GetLangName(
                                dtr["AccountDutyBase"].ToString().ToNull(), dtr["AccountDutyKo"].ToString().ToNull(),
                                dtr["AccountDutyEn"].ToString().ToNull(), dtr["AccountDutyJa"].ToString().ToNull(),
                                dtr["AccountDutyFr"].ToString().ToNull(), dtr["AccountDutyZhcn"].ToString().ToNull(),
                                dtr["AccountDutyZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        #endregion

        #region 자주 지정하는 결재자

        /// <summary>
        /// 자주 지정하는 결재자
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFavApp(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            string pageSize = GetDecryptParam("pageSize", "0");                 //페이지에 보여지는 목록수
            string totalItemCount = GetDecryptParam("totCnt", "0");
            string pageSizePageNo = GetDecryptParam("pageNo", "0");             //페이지 넘버

            string[] arrpage = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>()).Split(',');

            DAL.FavoriteApproval dal = new DAL.FavoriteApproval();
            DataTableReader dtr = dal.GetFavoriteList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, arrpage[0].ToString(), arrpage[1].ToString(), "");

            List<object> uobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Approval.BE.FavoriteApprovalInfo info = new BE.FavoriteApprovalInfo();

                        info.UserID = dtr["ApprovalUserID"].ToString().ToNumber<int>();
                        info.ApprovalUserSeq = dtr["ROW"].ToString().ToNumber<int>();
                        info.FavoriteApprovalID = dtr["FavoriteApprovalID"].ToString().ToNumber<int>();
                        info.BaseDeptStr = (dtr["BaseDept"].ToString().ToNull().Equals("1")) ? "" : "(" + GetResource("OrganAddtionJob") + ")";

                        #region 이름 다국어 설정
                        info.ApprovalUserName = this.GetLangName(
                                dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                dtr["NameZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 부서 다국어 설정
                        info.ApprovalUserDeptName = this.GetLangName(
                                dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직급 다국어 설정
                        info.ApprovalUserTitleName = this.GetLangName(
                                dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        #region 직책 다국어 설정
                        info.ApprovalUserDutyName = this.GetLangName(
                                dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                        #endregion

                        DateTime dt;
                        if (DateTime.TryParse(dtr["Adddate"].ToString(), out dt))
                            info.AddDateStr = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.AddDateStr = string.Empty;

                        // 사진정보
                        if (dtr["IsDept"].ToString().ToNull() == "M")
                        {
                            if (dtr["PhotoImage"].ToString().ToNull().Equals(""))
                            {
                                info.PhotoImage = "/DeskPlusEIP/Design/C/Img/Home/noimg.gif";
                            }
                            else
                            {
                                string path = "/DeskPlusEIP/FileData/" + dtr["CompanyID"].ToString() + "/Users/" + dtr["ApprovalUserID"].ToString() + "/Picture/" + dtr["PhotoImage"].ToString();
                                if (File.Exists(Server.MapPath(path)))
                                {
                                    info.PhotoImage = path;
                                }
                                else
                                {
                                    info.PhotoImage = "/DeskPlusEIP/Design/C/Img/Home/noimg.gif";
                                }
                            }
                        }
                        else
                        {
                            info.PhotoImage = "/DeskPlusEIP/Design/C/Img/Home/dept.gif";
                        }

                        info.IsDept = dtr["IsDept"].ToString().ToNull();        // G, M

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        /// <summary>
        /// 삭제 - 자주 지정하는 결재자 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFavApp(string p)
        {
            SetDecryptString(p);

            int favAppID = GetDecryptParam("appFavID", "0").ToNumber<int>();

            DAL.FavoriteApproval dal = new DAL.FavoriteApproval();

            BE.FavoriteApprovalInfo info = new FavoriteApprovalInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.FavoriteApprovalID = favAppID;

            return dal.Del(info);
        }
        #endregion

        #region 중요문서

        /// <summary>
        /// 중요문서
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetImpo(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Approval.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Approval.Web.Ws.Home.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);

            string totalItemCount = GetDecryptParam("totCnt", "0");
            string pageSize = GetDecryptParam("pageSize", "0");                 //페이지에 보여지는 목록수
            string pageSizePageNo = GetDecryptParam("pageNo", "0");             //페이지 넘버

            string[] arrpage = SetPaging(totalItemCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>()).Split(',');

            DAL.ImpoUser dal = new DAL.ImpoUser();
            DataTableReader dtr = dal.GetPageList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, arrpage[0].ToString(), arrpage[1].ToString(), "", "", "", "", "");

            List<object> uobj = new List<object>();
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
                        info.ChildDocID = dtr["LastDocID"].ToString().ToNumber<int>();
                        info.FolderID = 0;
                        info.ParentDocumentID = dtr["ParentDocumentID"].ToString().ToNumber<int>();
                        info.DocNumber = dtr["DocNumber"].ToString();
                        info.Number1 = dtr["Number1"].ToString();
                        info.Number2 = dtr["Number2"].ToString();
                        info.Number3 = dtr["Number3"].ToString();
                        info.Number4 = dtr["Number4"].ToString().ToNumber<int>();

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

                        info.Subject = dtr["Subject"].ToString();//제목   

                        #region 날짜 비교(New Img)
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(dt);
                        int ApprovalNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.APPROVAL, "ApprovalNewShowDay", "0").ToNumber<int>();

                        info.ShowNew = (gap.Days <= ApprovalNewShowDay) ? "Y" : "N";
                        #endregion

                        //info.ImpoCnt = dtr["Impocnt"].ToString().ToNumber<int>();
                        info.ImpoCnt = 1;

                        uobj.Add(info);
                    }
                }
            }
            return uobj;
        }

        #endregion
    }
}
