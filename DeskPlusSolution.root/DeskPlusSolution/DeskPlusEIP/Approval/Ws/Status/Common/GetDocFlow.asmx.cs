using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Status.Common
{
    /// <summary>
    /// GetDocFlow의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetDocFlow : Base.BaseWSApproval
    {

        #region GET()

        /// <summary>
        /// 결재정보
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public Object GetDocFlowList(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();
                string isComplete = GetDecryptParam("isComplete", "N");

                if (documentID.Equals(0) || isComplete.Equals("")) return null;

                DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                List<BE.DocumentProcessInfo> listuni = dpDal.GetDocumentUnion(CurrentUserInfo.CompanyID, documentID);
                foreach (BE.DocumentProcessInfo info in listuni)
                {
                    if (info.ApprovalState == "C" || info.ApprovalState == "R")
                    {
                        isComplete = "Y";
                    }
                    else
                    {
                        isComplete = "N";
                    }
                }

                List<BE.DocumentProcessInfo> listpro = dpDal.GetDocument(CurrentUserInfo.CompanyID, documentID);
                if (listpro.Count > 0)
                {
                    //중간결재자에게 반려된 경우
                    isComplete = "N";
                }

                Object o = null;
                if (isComplete.Equals("N"))
                {
                    #region 진행중인문서
                    List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, documentID);

                    DAL.Sequence seqDal = new DAL.Sequence();
                    DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                    DAL.CarbonCopyUser ccDal = new DAL.CarbonCopyUser();
                    foreach (BE.DocumentProcessInfo info in list)
                    {
                        //양식
                        info.Number3 = (info.Number3.Equals("")) ? "&nbsp" : info.Number3;
                        //제목
                        info.Subject = (info.Subject.Trim().Equals("")) ? "&nbsp" : info.Subject.Trim();
                        //상신일
                        info.WriteTimeStr = info.WriteTime.Year.ToString().PadLeft(4, '0') + "-" + info.WriteTime.Month.ToString().PadLeft(2, '0') + "-" + info.WriteTime.Day.ToString().PadLeft(2, '0') + " " + info.WriteTime.Hour.ToString().PadLeft(2, '0') + ":" + info.WriteTime.Minute.ToString().PadLeft(2, '0');

                        //결재의견 수
                        info.ReplyCnt = info.ReplyCnt;

                        //결재라인 - 발신
                        info.SequenceList = seqDal.GetSeqLine(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.SequenceInfo sinfo in info.SequenceList)
                        {
                            sinfo.ApprovalType = sinfo.ApprovalType.Trim();
                            sinfo.ApprovalDateStr = (sinfo.ApprovalDate.Year.Equals(1)) ? "&nbsp;" : sinfo.ApprovalDate.Year.ToString().PadLeft(4, '0') + "-" + sinfo.ApprovalDate.Month.ToString().PadLeft(2, '0') + "-" + sinfo.ApprovalDate.Day.ToString().PadLeft(2, '0') + " " + sinfo.ApprovalDate.Hour.ToString().PadLeft(2, '0') + ":" + sinfo.ApprovalDate.Minute.ToString().PadLeft(2, '0');
                        }

                        //결재라인 - 수신
                        info.CounterPartUserList = cpDal.GetCounterPartUsers(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.CounterPartUserInfo cinfo in info.CounterPartUserList)
                        {
                            cinfo.ApprovalType = cinfo.ApprovalType.Trim();
                            cinfo.ApprovalTag = cinfo.ApprovalTag.Trim();
                            cinfo.ApprovalDateStr = (cinfo.ApprovalDate.Year.Equals(1)) ? "&nbsp;" : cinfo.ApprovalDate.Year.ToString().PadLeft(4, '0') + "-" + cinfo.ApprovalDate.Month.ToString().PadLeft(2, '0') + "-" + cinfo.ApprovalDate.Day.ToString().PadLeft(2, '0') + " " + cinfo.ApprovalDate.Hour.ToString().PadLeft(2, '0') + ":" + cinfo.ApprovalDate.Minute.ToString().PadLeft(2, '0');
                            cinfo.ViewDateStr = (cinfo.ViewDate.Year.Equals(1)) ? "&nbsp;" : cinfo.ViewDate.Year.ToString().PadLeft(4, '0') + "-" + cinfo.ViewDate.Month.ToString().PadLeft(2, '0') + "-" + cinfo.ViewDate.Day.ToString().PadLeft(2, '0') + " " + cinfo.ViewDate.Hour.ToString().PadLeft(2, '0') + ":" + cinfo.ViewDate.Minute.ToString().PadLeft(2, '0');
                        }

                        //결재라인 - 참조
                        info.CarbonCopyUserList = ccDal.GetCarbonCopyUser(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.CarbonCopyUserInfo ccinfo in info.CarbonCopyUserList)
                        {
                            ccinfo.ApprovalTag = ccinfo.ApprovalTag.Trim();
                            ccinfo.ViewingDateStr = (ccinfo.ViewingDate.Year.Equals(1)) ? "&nbsp;" : ccinfo.ViewingDate.Year.ToString().PadLeft(4, '0') + "-" + ccinfo.ViewingDate.Month.ToString().PadLeft(2, '0') + "-" + ccinfo.ViewingDate.Day.ToString().PadLeft(2, '0') + " " + ccinfo.ViewingDate.Hour.ToString().PadLeft(2, '0') + ":" + ccinfo.ViewingDate.Minute.ToString().PadLeft(2, '0');
                        }

                        o = list;
                    }
                    #endregion
                }
                else
                {
                    #region 완료문서

                    DAL.Document dDal = new DAL.Document();
                    List<BE.DocumentInfo> list = dDal.GetDocList(CurrentUserInfo.CompanyID, documentID);

                    DAL.SequenceBackUp seqDal = new DAL.SequenceBackUp();
                    DAL.CounterPartUser cpDal = new DAL.CounterPartUser();
                    DAL.CarbonCopyUser ccDal = new DAL.CarbonCopyUser();
                    foreach (BE.DocumentInfo info in list)
                    {
                        info.Subject = (info.Subject.Trim().Equals("")) ? "&nbsp" : info.Subject.Trim();
                        info.Number3 = (info.Number3.Equals("")) ? "&nbsp" : info.Number3;
                        info.WriteTimeStr = info.WriteTime.Year.ToString().PadLeft(4, '0') + "-" + info.WriteTime.Month.ToString().PadLeft(2, '0') + "-" + info.WriteTime.Day.ToString().PadLeft(2, '0') + " " + info.WriteTime.Hour.ToString().PadLeft(2, '0') + ":" + info.WriteTime.Minute.ToString().PadLeft(2, '0');

                        info.SequenceList = seqDal.GetSeqLine(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.SequenceBackUpInfo sinfo in info.SequenceList)
                        {
                            sinfo.ApprovalType = sinfo.ApprovalType.Trim();
                            sinfo.ApprovalDateStr = (sinfo.ApprovalDate.Year.Equals(1)) ? "&nbsp;" : sinfo.ApprovalDate.Year.ToString().PadLeft(4, '0') + "-" + sinfo.ApprovalDate.Month.ToString().PadLeft(2, '0') + "-" + sinfo.ApprovalDate.Day.ToString().PadLeft(2, '0') + " " + sinfo.ApprovalDate.Hour.ToString().PadLeft(2, '0') + ":" + sinfo.ApprovalDate.Minute.ToString().PadLeft(2, '0');
                        }
                        info.CounterPartUserList = cpDal.GetCounterPartUsers(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.CounterPartUserInfo cinfo in info.CounterPartUserList)
                        {
                            cinfo.ApprovalType = cinfo.ApprovalType.Trim();
                            cinfo.ApprovalTag = cinfo.ApprovalTag.Trim();
                            cinfo.ApprovalDateStr = (cinfo.ApprovalDate.Year.Equals(1)) ? "&nbsp;" : cinfo.ApprovalDate.Year.ToString().PadLeft(4, '0') + "-" + cinfo.ApprovalDate.Month.ToString().PadLeft(2, '0') + "-" + cinfo.ApprovalDate.Day.ToString().PadLeft(2, '0') + " " + cinfo.ApprovalDate.Hour.ToString().PadLeft(2, '0') + ":" + cinfo.ApprovalDate.Minute.ToString().PadLeft(2, '0');
                            cinfo.ViewDateStr = (cinfo.ViewDate.Year.Equals(1)) ? "&nbsp;" : cinfo.ViewDate.Year.ToString().PadLeft(4, '0') + "-" + cinfo.ViewDate.Month.ToString().PadLeft(2, '0') + "-" + cinfo.ViewDate.Day.ToString().PadLeft(2, '0') + " " + cinfo.ViewDate.Hour.ToString().PadLeft(2, '0') + ":" + cinfo.ViewDate.Minute.ToString().PadLeft(2, '0');
                        }
                        info.CarbonCopyUserList = ccDal.GetCarbonCopyUser(CurrentUserInfo.CompanyID, info.DocumentID);
                        foreach (BE.CarbonCopyUserInfo ccinfo in info.CarbonCopyUserList)
                        {
                            ccinfo.ApprovalTag = ccinfo.ApprovalTag.Trim();
                            ccinfo.ViewingDateStr = (ccinfo.ViewingDate.Year.Equals(1)) ? "&nbsp;" : ccinfo.ViewingDate.Year.ToString().PadLeft(4, '0') + "-" + ccinfo.ViewingDate.Month.ToString().PadLeft(2, '0') + "-" + ccinfo.ViewingDate.Day.ToString().PadLeft(2, '0') + " " + ccinfo.ViewingDate.Hour.ToString().PadLeft(2, '0') + ":" + ccinfo.ViewingDate.Minute.ToString().PadLeft(2, '0');
                        }

                        o = list;
                    }

                    #endregion
                }

                return o;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 관련 문서정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetUpPath(string p)
        {
            try
            {
                SetDecryptString(p);

                string uppathID = GetDecryptParam("uppathID", "");
                int lastDocID = GetDecryptParam("lastDocID", "0").ToNumber<int>();
                string isComplete = GetDecryptParam("isComplete", "N");

                if (lastDocID.Equals(0) || isComplete.Equals("")) return null;

                string uppath = "";
                if (isComplete.Equals("N"))
                {
                    DAL.DocumentProcess dpDal = new DAL.DocumentProcess();
                    List<BE.DocumentProcessInfo> list = dpDal.GetDocument(CurrentUserInfo.CompanyID, lastDocID);
                    foreach (BE.DocumentProcessInfo info in list)
                    {
                        uppath = info.UpPathID; //  ,278,284
                    }
                }
                else
                {
                    DAL.Document dDal = new DAL.Document();
                    List<BE.DocumentInfo> list = dDal.GetDocList(CurrentUserInfo.CompanyID, lastDocID);
                    foreach (BE.DocumentInfo info in list)
                    {
                        uppath = info.UpPathID; //,278,284
                    }
                }
                return uppath.Replace(uppathID, "").Trim(',');
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 수신(ZP) 문서 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetDocZPList(string p)
        {
            try
            {
                SetDecryptString(p);

                int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();

                if (documentID.Equals(0)) return null;

                string ZPList = "";

                DAL.Document dDal = new DAL.Document();
                DataSet ds = dDal.GetDocZPList(CurrentUserInfo.CompanyID, documentID);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ZPList += dr["DocumentID"].ToString() + ",";
                }

                return ZPList.Trim(',');
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
