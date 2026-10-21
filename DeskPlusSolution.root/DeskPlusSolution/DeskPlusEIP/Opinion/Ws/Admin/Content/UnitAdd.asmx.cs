using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Collections;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Opinion.Web.Ws.Admin.Content
{
    /// <summary>
    /// UnitAdd의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class UnitAdd : Base.BaseWSOpinion
    {
        /// <summary>
        /// 항목이미지 추가 (최종파일정보)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetSpoolFileNm(string p)
        {
            string rtnval = "!NULL";

            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

            string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
            filePath = filePath + "{0}/Users/{1}/Spool/{2}/";
            string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

            List<object> listobj = new List<object>();
            DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));
            if (di.Exists)
            {
                FileInfo[] sortList = di.GetFiles();
                Array.Sort(sortList,
                    new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.FileListSort());

                if (sortList.Length > 0)
                {
                    FileInfo fInfo = sortList[sortList.Length - 1];
                    //rtnval = Server.UrlEncode(fInfo.Name);
                    rtnval = HttpUtility.UrlDecode(fInfo.Name, System.Text.Encoding.GetEncoding("utf-8"));
                }
            }
            return rtnval;
        }
    }
}
