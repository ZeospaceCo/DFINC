using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Pim.Web.Ws.Admin.Category
{
    /// <summary>
    /// Register의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Register : Base.BaseWSPim
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 추가하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            SetDecryptString(p);
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");
            string colors = GetDecryptParam("colors", "");
            string useActive = GetDecryptParam("useActive", "");
            string statusCode = GetDecryptParam("statusCode", "");

            BE.StatusInfo info = new BE.StatusInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.StatusCode = statusCode;

            info.NameBase = name.Trim();            
            
            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }

            info.Colors = colors;
            info.UseActive = useActive;
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Status dal = new DAL.Status();
            dal.Add(info);

            return true;
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 추가하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Mod(string p)
        {
            SetDecryptString(p);
            int statusID = GetDecryptParam("statusID", "0").ToNumber<int>();
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");
            string colors = GetDecryptParam("colors", "");
            string useActive = GetDecryptParam("useActive", "");
            string statusCode = GetDecryptParam("statusCode", "");

            if (statusID == 0) return false;

            BE.StatusInfo info = new BE.StatusInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.StatusID = statusID;
            info.StatusCode = statusCode;

            info.NameBase = name.Trim();            

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                info.NameKo = multiName.Split(',')[0];
                info.NameEn = multiName.Split(',')[1];
                info.NameJa = multiName.Split(',')[2];
                info.NameFr = multiName.Split(',')[3];
                info.NameZhcn = multiName.Split(',')[4];
                info.NameZhtw = multiName.Split(',')[5];
            }

            info.Colors = colors;
            info.UseActive = useActive;
            info.WriteUserID = CurrentUserInfo.UserID;

            DAL.Status dal = new DAL.Status();
            dal.Mod(info);

            return true;
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);
            int statusID = GetDecryptParam("statusID", "0").ToNumber<int>();

            if (statusID == 0) return false;

            DAL.Status dal = new DAL.Status();
            dal.Del(CurrentUserInfo.CompanyID, statusID);

            return true;
        }
    }
}
