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

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content.Judge
{
    /// <summary>
    /// JudgeView의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class JudgeView : Base.BaseWSProposal
    {
        #region ADD

        /// <summary>
        /// 작성자 : shine
        /// 1차 심사자의 평가하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            SetDecryptString(p);

            string proposalNo = GetDecryptParam("proposalNo", "0");
            string itemNo = GetDecryptParam("itemNo", "0");
            string codeName = GetDecryptParam("codeName", "");
            string score = GetDecryptParam("score", "");
            string sum = GetDecryptParam("sum", "0");
            int count = GetDecryptParam("count", "0").ToNumber<int>();
            string opinion = GetDecryptParam("opinion", "");

            if (proposalNo == "0" || itemNo == "0" || codeName == "" ||
                score == "" || sum == "0" || count == 0)
                return false;

            // 평가한 아이템 항목
            codeName = codeName.Replace("Item", "");
            // 평가한 항목 점수
            score = score.TrimEnd('^');

            string[] arrCodeName = codeName.Split('^');
            string[] arrScore = score.Split('^');

            BE.ExamInfo info = new ExamInfo();
            DAL.Exam biz = new DAL.Exam();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ProposalNo = proposalNo.ToNumber<int>();
            info.Degree = 1;
            info.UserID = CurrentUserInfo.UserID;
            info.ItemNo = itemNo.ToNumber<int>();
            info.TotalScore = sum.ToDouble<double>();
            info.JudgeView = opinion;

            for (int i = 0; i < count; i++)
            {
                switch (arrCodeName[i])
                {
                    case "1":
                        info.Score1 = arrScore[i].ToDouble<double>();
                        break;
                    case "2":
                        info.Score2 = arrScore[i].ToDouble<double>();
                        break;
                    case "3":
                        info.Score3 = arrScore[i].ToDouble<double>();
                        break;
                    case "4":
                        info.Score4 = arrScore[i].ToDouble<double>();
                        break;
                    case "5":
                        info.Score5 = arrScore[i].ToDouble<double>();
                        break;
                    case "6":
                        info.Score6 = arrScore[i].ToDouble<double>();
                        break;
                    case "7":
                        info.Score7 = arrScore[i].ToDouble<double>();
                        break;
                    case "8":
                        info.Score8 = arrScore[i].ToDouble<double>();
                        break;
                    case "9":
                        info.Score9 = arrScore[i].ToDouble<double>();
                        break;
                    case "10":
                        info.Score10 = arrScore[i].ToDouble<double>();
                        break;
                }
            }
            return biz.Add(info);
        }

        #endregion

        #region MOD

        /// <summary>
        /// 작성자 : shine
        /// 1차 심사자 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModJudgeUserList(string p)
        {
            SetDecryptString(p);

            string proposalNo = GetDecryptParam("proposalNo", "0");
            string userList = GetDecryptParam("userList", "");
            userList = userList.TrimEnd(',');

            if (proposalNo.Length > 0 && userList.Length > 0)
            {
                BLL.JudgeUser judge = new BLL.JudgeUser();
                return judge.Mod(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>(), userList);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
