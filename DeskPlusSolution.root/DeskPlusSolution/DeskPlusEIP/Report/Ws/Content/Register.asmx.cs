using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// 나의 일정 및 할일 가져오기
    /// </summary>
    [Serializable]
    public class RegisterPimInfo
    {
        public RegisterPimInfo()
        {
        }

        public int CompanyID { get; set; }

        public int ScheduleID { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public int UserID { get; set; }

        public int OpenType { get; set; }
    }

    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Register : Base.BaseWSReport
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 일정관리 나의 일정 및 할일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<RegisterPimInfo> GetPimMySche(string p)
        {
            try
            {
                SetDecryptString(p);

                string startDate = GetDecryptParam("startDate", System.DateTime.Now.ToString("yyyyMMdd"));

                DateTime sDate = Convert.ToDateTime(DateHelper.GetCultureDateChar8(startDate, PreLangReal) + " 00:00:01");
                DateTime eDate = sDate.AddDays(5).AddHours(23).AddMinutes(59).AddSeconds(59);

                DAL.Pim dal = new DAL.Pim();

                DataTableReader dtr = dal.GetPimMySche(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID.ToString(),
                    CurrentUserInfo.DeptID, sDate, eDate, "1,2", CurrentUserInfo.UserID.ToString() + ",");

                List<RegisterPimInfo> listobj = new List<RegisterPimInfo>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Ws.Content.RegisterPimInfo info = new RegisterPimInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ScheduleID = dtr["ScheduleID"].ToString().ToNumber<int>();
                            info.Type = dtr["Type"].ToString();
                            info.Title = dtr["Title"].ToString();
                            info.StartDate = (dtr["StartTime"].ToString().ToNull() == "") ? "" : Convert.ToDateTime(dtr["StartTime"].ToString()).ToShortDateString();
                            info.EndDate = (dtr["EndTime"].ToString().ToNull() == "") ? "" : Convert.ToDateTime(dtr["EndTime"].ToString()).ToShortDateString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                            info.OpenType = dtr["OpenType"].ToString().ToNumber<int>();

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
        /// 보고자 작성글 가져오기
        /// 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<GetReporterEntity> GetReporter(string p)
        {
            try
            {
                SetDecryptString(p);

                int listID = GetDecryptParam("listID", "0").ToNumber<int>();
                List<GetReporterEntity> listobj = new List<GetReporterEntity>();
                if (listID == 0) return listobj;

                DAL.List dal = new DAL.List();
                var alList = dal.GetList(CurrentUserInfo.CompanyID, listID.ToString());

                foreach (var info in alList)
                {
                    GetReporterEntity be = new GetReporterEntity();
                    be.NameBase = this.GetLangName(
                            info.NameBase, info.NameKo, info.NameEn, info.NameJa,
                            info.NameFr, info.NameZhcn, info.NameZhtw, PreLang).ToReverseXSS();

                    be.DeptBase = this.GetLangName(
                            info.DeptBase, info.DeptKo, info.DeptEn, info.DeptJa,
                            info.DeptFr, info.DeptZhcn, info.DeptZhtw, PreLang).ToReverseXSS();

                    be.TitleBase = this.GetLangName(
                            info.TitleBase, info.TitleKo, info.TitleEn, info.TitleJa,
                            info.TitleFr, info.TitleZhcn, info.TitleZhtw, PreLang).ToReverseXSS();

                    be.NowJob = info.NowJob.ToReverseXSS();
                    be.NextJob = info.NextJob.ToReverseXSS();
                    be.Issue = info.Issue.ToReverseXSS();

                    listobj.Add(be);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }

    [Serializable]
    public class GetReporterEntity
    {
        /// <summary>
        /// Gets or sets the dept base.
        /// </summary>
        /// <value>
        /// The dept base.
        /// </value>
        public string DeptBase { get; set; }

        /// <summary>
        /// Gets or sets the title base.
        /// </summary>
        /// <value>
        /// The title base.
        /// </value>
        public string TitleBase { get; set; }

        /// <summary>
        /// Gets or sets the name base.
        /// </summary>
        /// <value>
        /// The name base.
        /// </value>
        public string NameBase { get; set; }

        /// <summary>
        /// Gets or sets the now job.
        /// </summary>
        /// <value>
        /// The now job.
        /// </value>
        public string NowJob { get; set; }

        /// <summary>
        /// Gets or sets the next job.
        /// </summary>
        /// <value>
        /// The next job.
        /// </value>
        public string NextJob { get; set; }

        /// <summary>
        /// Gets or sets the issue.
        /// </summary>
        /// <value>
        /// The issue.
        /// </value>
        public string Issue { get; set; }
    }
}