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
using System.Collections;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Content
{
    /// <summary>
    /// Result의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Result : Base.BaseWSOpinion
    {
        #region GET
        /// <summary>
        /// 특정설문의 파일리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetAttachList(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Attach attDal = new DAL.Attach();
            DataTableReader dtr = attDal.GetAttachList(CurrentUserInfo.CompanyID, quesID);

            List<object> attobj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.AttachInfo attInfo = new BE.AttachInfo();
                        attInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        attInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        attInfo.AttachID = dtr["AttachID"].ToString().ToNumber<int>();
                        attInfo.FilePath = dtr["FilePath"].ToString();
                        attInfo.FileName = dtr["FileName"].ToString();
                        attInfo.FileType = dtr["FileType"].ToString();
                        attInfo.FileLength = dtr["FileLength"].ToString().ToNumber<int>();
                        attInfo.FileGUID = dtr["FileGUID"].ToString();
                        attInfo.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(dtr["FileLength"].ToString().ToNumber<int>()) / 1024) + " KB";

                        attobj.Add(attInfo);
                    }
                }
            }
            return attobj;

        }
        /// <summary>
        /// 특정설문의 답글가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetReplyList(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.Reply rDal = new DAL.Reply();
            DataTableReader dtr = rDal.GetReplyList(CurrentUserInfo.CompanyID, quesID);

            List<object> rObj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ReplyInfo rInfo = new BE.ReplyInfo();
                        rInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        rInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        rInfo.ReplyID = dtr["ReplyID"].ToString().ToNumber<int>();
                        rInfo.NameBase = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                        rInfo.DeptBase = GetLangName(dtr["DeptBase"].ToString(), dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(), dtr["DeptFr"].ToString(), dtr["DeptZhcn"].ToString(), dtr["DeptZhtw"].ToString(), PreLang);
                        rInfo.TitleBase = GetLangName(dtr["TitleBase"].ToString(), dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(), dtr["TitleFr"].ToString(), dtr["TitleZhcn"].ToString(), dtr["TitleZhtw"].ToString(), PreLang);
                        rInfo.DutyBase = GetLangName(dtr["DutyBase"].ToString(), dtr["DutyKo"].ToString(), dtr["DutyEn"].ToString(), dtr["DutyJa"].ToString(), dtr["DutyFr"].ToString(), dtr["DutyZhcn"].ToString(), dtr["DutyZhtw"].ToString(), PreLang);
                        rInfo.Cont = dtr["Cont"].ToString();
                        rInfo.IsEdit = (dtr["UserID"].ToString().Equals(CurrentUserInfo.UserID.ToString())) ? "1" : "0";

                        DateTime wdt;
                        if (DateTime.TryParse(dtr["WriteDate"].ToString(), out wdt))
                            rInfo.WriteDateStr = DateHelpers.GetCultureDate(wdt, CurrentUserInfo.PreferredLanguage).Substring(0, 10);
                        else
                            rInfo.WriteDateStr = string.Empty;

                        rObj.Add(rInfo);
                    }
                }
            }
            return rObj;
        }

        /// <summary>
        /// 특정설문의 질문리스트가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetQuesUnitList(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Content.Result).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();

            DAL.QuesUnit quDAL = new DAL.QuesUnit();
            DataTableReader dtr = quDAL.GetQuesUnitList(CurrentUserInfo.CompanyID, quesID);
            List<object> quObj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                        quInfo.RowNum = dtr["RowNum"].ToString().ToNumber<int>();
                        quInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        quInfo.QuesUnitID = dtr["QuesUnitID"].ToString().ToNumber<int>();
                        quInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        quInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        quInfo.Title = dtr["Title"].ToString();
                        quInfo.IsReq = dtr["IsReq"].ToString();
                        quInfo.IsReqStr = (quInfo.IsReq.Equals("1")) ? resMng.GetString("TitReq") : resMng.GetString("TitNoReq");
                        quInfo.ItemCnt = dtr["ItemCnt"].ToString();
                        quInfo.Type = dtr["Type"].ToString();
                        quInfo.Point = dtr["Point"].ToString().ToNumber<int>();

                        quObj.Add(quInfo);
                    }
                }
            }
            return quObj;
        }
       
        /// <summary>
        /// 특정설문의 특정질문에 대한 설문현황조회
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetQUResultList(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();

            DAL.QuesUnit quDAL = new DAL.QuesUnit();
            DataTableReader dtr = quDAL.GetQUResultList(CurrentUserInfo.CompanyID, quesID, unitID);
            List<object> quObj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                        quInfo.RowNum = dtr["RowNum"].ToString().ToNumber<int>();
                        quInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        quInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        quInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        quInfo.Title = dtr["Title"].ToString();
                        quInfo.Type = dtr["Type"].ToString();
                        quInfo.IsReq = dtr["IsReq"].ToString();
                        quInfo.Point = dtr["Point"].ToString().ToNumber<int>();
                        quInfo.UnitItemID = dtr["UnitItemID"].ToString().ToNumber<int>();
                        quInfo.ItemType = dtr["ItemType"].ToString();
                        quInfo.ItemCont = dtr["ItemCont"].ToString();
                        quInfo.ItemImgUrl = dtr["ItemImgUrl"].ToString();
                        quInfo.VoteCnt = dtr["VoteCnt"].ToString().ToNumber<int>();
                        quInfo.VoteTotCnt = dtr["VoteTotCnt"].ToString().ToNumber<int>();

                        quObj.Add(quInfo);
                    }
                }
            }
            return quObj;
        }

        /// <summary>
        /// 가점형 설문결과가져오기(단일가점형)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetResultListTypeC(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Content.Result).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            int point = GetDecryptParam("point", "0").ToNumber<int>();

            DAL.Vote vDAL = new DAL.Vote();
            DataSet dsResult = vDAL.GetResultVoteList(CurrentUserInfo.CompanyID, quesID, unitID);
            
            List<object> rtnobj = new List<object>();
            int totCnt = 0;
            for (int i = 1; i < point+1; i++)
            {
                DataRow[] dr = dsResult.Tables[0].Select("Point = '" + i + "'");

                BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                quInfo.Point = i;
                quInfo.RowNum = quInfo.Point;
                quInfo.Title = quInfo.Point.ToString() + " " + resMng.GetString("Point");
                quInfo.VoteCnt = (dr.Length > 0) ? dr[0]["VoteCnt"].ToString().ToNumber<int>() : 0;
                quInfo.VoteTotCnt = (dr.Length > 0) ? dr[0]["VoteTotCnt"].ToString().ToNumber<int>() : 0;
                totCnt = (quInfo.VoteTotCnt > 0) ? quInfo.VoteTotCnt : totCnt;

                rtnobj.Add(quInfo);
            }

            foreach (BE.QuesUnitInfo uInfo in rtnobj)
            {
                uInfo.VoteTotCnt = totCnt;
                uInfo.QuesID = quesID;
                uInfo.UnitID = unitID;
            }

            return rtnobj;
        }

        /// <summary>
        /// 가점형 설문결과가져오기(다중가점형)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetResultListTypeD(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Opinion.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Opinion.Web.Ws.Content.Result).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();
            int point = GetDecryptParam("point", "0").ToNumber<int>();
            int unitItemID = GetDecryptParam("unititemID", "0").ToNumber<int>();

            DAL.Vote vDAL = new DAL.Vote();
            DataSet dsResult = vDAL.GetResultVoteList(CurrentUserInfo.CompanyID, quesID, unitID, unitItemID);

            List<object> rtnobj = new List<object>();
            int totCnt = 0;
            for (int i = 1; i < point + 1; i++)
            {
                DataRow[] dr = dsResult.Tables[0].Select("Point = '" + i + "'");

                BE.QuesUnitInfo quInfo = new BE.QuesUnitInfo();
                quInfo.Point = i;
                quInfo.RowNum = quInfo.Point;
                quInfo.Title = quInfo.Point.ToString() + " " + resMng.GetString("Point");
                quInfo.VoteCnt = (dr.Length > 0) ? dr[0]["VoteCnt"].ToString().ToNumber<int>() : 0;
                quInfo.VoteTotCnt = (dr.Length > 0) ? dr[0]["VoteTotCnt"].ToString().ToNumber<int>() : 0;
                totCnt = (quInfo.VoteTotCnt > 0) ? quInfo.VoteTotCnt : totCnt;

                rtnobj.Add(quInfo);
            }

            foreach (BE.QuesUnitInfo uInfo in rtnobj)
            {
                uInfo.VoteTotCnt = totCnt;
                uInfo.QuesID = quesID;
                uInfo.UnitID = unitID;
                uInfo.UnitItemID = unitItemID;
            }

            return rtnobj;
        }

        /// <summary>
        /// 특정설문의 특정 항목에 대한 투표리스트 정보
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<object> GetResultListTypeE(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int unitID = GetDecryptParam("unitID", "0").ToNumber<int>();

            DAL.Vote vDAL = new DAL.Vote();
            DataTableReader dtr = vDAL.GetVoteList(CurrentUserInfo.CompanyID, quesID, unitID);
            List<object> quObj = new List<object>();
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.VoteInfo vInfo = new BE.VoteInfo();
                        vInfo.RowNum = dtr["RowNum"].ToString().ToNumber<int>();
                        vInfo.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        vInfo.VoteID = dtr["VoteID"].ToString().ToNumber<int>();
                        vInfo.QuesID = dtr["QuesID"].ToString().ToNumber<int>();
                        vInfo.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        vInfo.UnitID = dtr["UnitID"].ToString().ToNumber<int>();
                        vInfo.UnitItemID = dtr["UnitItemID"].ToString().ToNumber<int>();
                        vInfo.UnitItemCont = dtr["UnitItemCont"].ToString();
                        vInfo.VoteTotCnt = dtr["VoteTotCnt"].ToString().ToNumber<int>();
                        quObj.Add(vInfo);
                    }
                }
            }
            return quObj;
        }
        #endregion

        #region SET

        /// <summary>
        /// 답글삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelReply(string p)
        {
            SetDecryptString(p);
            int quesID = GetDecryptParam("quesID", "0").ToNumber<int>();
            int replyID = GetDecryptParam("replyID", "0").ToNumber<int>();

            DAL.Reply rDAL = new DAL.Reply();
            return rDAL.Del(CurrentUserInfo.CompanyID, replyID);
        }

        #endregion
    }
}
