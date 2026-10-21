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

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSProposal
    {
        #region GET

        /// <summary>
        /// 작성자 : shine
        /// 진행사항 리스트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.ProgressList> GetStatusList(string p)
        {
            try
            {
                SetDecryptString(p);

                string proposalNo = GetDecryptParam("proposalNo", "0");

                DAL.Exam biz = new DAL.Exam();
                List<Ws.Content.ProgressList> alList = new List<ProgressList>();
                DataTableReader dtr = biz.GetStatusList(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.ProgressList info = new ProgressList();

                            // 평가자의 부서, 직급은 현재를 기준으로 한다.
                            info.UserID = dtr["JUserID"].ToString().ToNumber<int>();
                            info.Name = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);
                            info.DeptName = this.GetLangName(
                                dtr["DeptBase"].ToString(),
                                dtr["DeptKo"].ToString(), dtr["DeptEn"].ToString(), dtr["DeptJa"].ToString(),
                                dtr["DeptFr"].ToString(), dtr["DeptZhCn"].ToString(), dtr["DeptZhTw"].ToString(),
                                PreLang);
                            info.TitleName = this.GetLangName(
                                dtr["TitleBase"].ToString(),
                                dtr["TitleKo"].ToString(), dtr["TitleEn"].ToString(), dtr["TitleJa"].ToString(),
                                dtr["TitleFr"].ToString(), dtr["TitleZhCn"].ToString(), dtr["TitleZhTw"].ToString(),
                                PreLang);

                            info.Name = info.Name + "(" + info.DeptName + "/" + info.TitleName + ")";

                            info.JudgeView = dtr["JudgeView"].ToString();
                            info.TotalScore = dtr["TotalScore"].ToString();

                            #region 등록일
                            DateTime dt;

                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateYMD = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                            else
                                info.WriteDateYMD = string.Empty;
                            #endregion

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

        #endregion

        #region DEL

        /// <summary>
        /// 작성자 : shine
        /// 제안취소하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);

            string proposalNo = GetDecryptParam("proposalNo", "0");
            string gubunCode = GetDecryptParam("gubunCode", "");

            if (proposalNo.Length > 0)
            {
                BLL.List bll = new BLL.List();
                return bll.Del(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>(), gubunCode);
            }
            else
            {
                return false;
            }
        }

        #endregion
    }

    [Serializable]
    public class ProgressList
    {
        public ProgressList() { }

        public int UserID { get; set; }
        public string Name { get; set; }
        public string JudgeView { get; set; }
        public string TotalScore { get; set; }
        public string WriteDateYMD { get; set; }
        public string DeptName { get; set; }
        public string TitleName { get; set; }
    }
}
