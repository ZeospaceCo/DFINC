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
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Admin.EnforceSeal
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {
        /// <summary>
        /// 직인삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string DelStamp(string p)
        {
            SetDecryptString(p);
            string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");
            string filename = GetDecryptParam("filename", "");

            if (swfUploadGUID.Equals("") || filename.Equals("")) return "";

            string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");  //  ex) /DeskPlusEIP/FileData/
            string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
            souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID, filename);

            File.Delete(Server.MapPath(souceFile));

            string noImage = this.BaseImgPath + "/Icon/ico_noimg2.gif";
            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("filePath", noImage);
            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
            noImage = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

            return "<img src='" + noImage + "' width='60' height='50' alt='' title='' />";
        }
    }
}
