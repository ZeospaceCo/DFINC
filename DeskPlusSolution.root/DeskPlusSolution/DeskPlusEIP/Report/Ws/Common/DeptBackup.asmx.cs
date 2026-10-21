using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Common
{
    /// <summary>
    /// DeptBackup의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class DeptBackup : Base.BaseWSReport
    {
        /// <summary>
        /// 리스트 가져오기
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<DeptBackupInfo> GetList()
        {
            List<DeptBackupInfo> obj = new List<DeptBackupInfo>();

            Zeospace.DeskPlus.Account.DAL.Dept_History dal = new Account.DAL.Dept_History();
            DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID);

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        DeptBackupInfo info = new DeptBackupInfo();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();

                        info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);

                        info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();

                        obj.Add(info);
                    }
                }
            }

            return obj;
        }
    }

    /// <summary>
    /// DeptBackupInfo
    /// </summary>
    [Serializable]
    public class DeptBackupInfo
    {
        public DeptBackupInfo()
        {
        }

        public int CompanyID { get; set; }

        public int DeptID { get; set; }

        public string NameBase { get; set; }
    }
}