using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Proposal.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content.Admin
{
    /// <summary>
    /// ResultView의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ResultView : Base.BaseWSProposal
    {

        /// <summary>
        /// 작성자 : shine
        /// 진행사항 평가리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.Admin.ExamList> GetExamList(string p)
        {
            try
            {
                SetDecryptString(p);

                string proposalNo = GetDecryptParam("proposalNo", "0");

                DAL.Exam biz = new DAL.Exam();
                List<Ws.Content.Admin.ExamList> alList = new List<ExamList>();
                DataTableReader dtr = biz.GetStatusList(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.Admin.ExamList info = new ExamList();

                            info.ProposalNo = dtr["ProposalNo"].ToString().ToNumber<int>();
                            info.Degree = dtr["Degree"].ToString().ToNumber<int>();
                            info.UserID = dtr["JUserID"].ToString().ToNumber<int>();
                            info.Name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);
                            info.ItemNo = dtr["ItemNo"].ToString().ToNumber<int>();

                            info.Score1 = dtr["Score1"].ToString().ToNull();
                            info.Score2 = dtr["Score2"].ToString().ToNull();
                            info.Score3 = dtr["Score3"].ToString().ToNull();
                            info.Score4 = dtr["Score4"].ToString().ToNull();
                            info.Score5 = dtr["Score5"].ToString().ToNull();
                            info.Score6 = dtr["Score6"].ToString().ToNull();
                            info.Score7 = dtr["Score7"].ToString().ToNull();
                            info.Score8 = dtr["Score8"].ToString().ToNull();
                            info.Score9 = dtr["Score9"].ToString().ToNull();
                            info.Score10 = dtr["Score10"].ToString().ToNull();

                            info.TotalScore = dtr["TotalScore"].ToString().ToNull();
                            info.PersonCnt = dtr["PersonCnt"].ToString().ToNumber<int>();
                            info.TotalScoreSum = dtr["TotalScoreSum"].ToString().ToDouble<double>();
                            info.LastGrade = dtr["LastGrade"].ToString().ToNull();
                            info.TotalScoreSum = info.TotalScoreSum / info.PersonCnt;

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : shine
        /// 등급 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModGrade(string p)
        {
            try
            {
                SetDecryptString(p);

                string proposalNo = GetDecryptParam("proposalNo", "0");
                string grade = GetDecryptParam("grade", "");

                DAL.List biz = new DAL.List();

                return biz.ModGade(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>(), grade);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
    }

    /// <summary>
    /// 평가표
    /// </summary>
    [Serializable]
    public class ExamList
    {
        public ExamList() {}

        public int ProposalNo { get; set; }
        public int Degree { get; set; }
        public int UserID { get; set; }
        public int ItemNo { get;set;}
        public string Name { get; set; }
        public string Score1 { get; set; }
        public string Score2 { get; set; }
        public string Score3 { get; set; }
        public string Score4 { get; set; }
        public string Score5 { get; set; }
        public string Score6 { get; set; }
        public string Score7 { get; set; }
        public string Score8 { get; set; }
        public string Score9 { get; set; }
        public string Score10 { get; set; }
        public string TotalScore { get; set; }
        public string JudgeView { get; set; }
        public double TotalScoreSum { get; set; }
        public int PersonCnt { get; set; }
        public string LastGrade { get; set; }
    }
}
