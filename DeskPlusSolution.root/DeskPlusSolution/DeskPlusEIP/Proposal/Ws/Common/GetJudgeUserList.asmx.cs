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

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Common
{
    /// <summary>
    /// GetJudgeUserList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetJudgeUserList : Base.BaseWSProposal
    {

        /// <summary>
        /// 작성자 : shine
        /// 1차 심사자 리스트 가져옹기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Common.JudgeUserList> GetList(string p)
        {
            try
            {
                SetDecryptString(p);

                string degree = GetDecryptParam("degree", "1");

                List<Ws.Common.JudgeUserList> alList = new List<JudgeUserList>();
                DAL.Judge biz = new DAL.Judge();

                DataTableReader dtr = biz.GetJudgeList(CurrentUserInfo.CompanyID, degree.ToNumber<int>());

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Common.JudgeUserList info = new JudgeUserList();

                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.Name = dtr["Name"].ToString().ToNull();
                            info.DeptName = dtr["DeptName"].ToString().ToNull();
                            info.ParentDeptName = dtr["ParentDeptName"].ToString().ToNull();
                            info.TitleName = dtr["TitleName"].ToString().ToNull();

                            alList.Add(info);
                        }
                    }
                }
                return alList;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class JudgeUserList
    {
        public JudgeUserList() { }

        public int CompanyID { get; set; }
        public int Degree { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public string DeptName { get; set; }
        public string ParentDeptName { get; set; }
        public string TitleName { get; set; }
    }
}
