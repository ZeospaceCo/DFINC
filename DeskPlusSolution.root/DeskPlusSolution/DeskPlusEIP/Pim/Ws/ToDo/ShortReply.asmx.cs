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
using Zeospace.DeskPlus.Pim.BE;
using Zeospace.DeskPlus.Pim.DAL;

namespace Zeospace.DeskPlus.Pim.Web.Ws.ToDo
{
    /// <summary>
    /// ShortReply의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ShortReply : Base.BaseWSPim
    {

        #region 댓글

        /// <summary>
        /// 댓글 ADD
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReply(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.ToDo.ShortReply).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string todoID = GetDecryptParam("todoID", "0");
            string contents = GetDecryptParam("contents", "");
            string security = GetDecryptParam("security", "0");
            string preTitle = string.Empty;
            //contents = Regex.Replace(contents, @"<(.|\n)*?>", string.Empty);
            

            BE.ToDoReplyInfo info = new ToDoReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.ToDoID = todoID.ToNumber<int>();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;
            info.MyDeptName = CurrentUserInfo.DeptName;
            info.IsSecurity = security;

            preTitle = resMng.GetString("TODO") + "," + resMng.GetString("InsertReply");

            BLL.ToDoReply dal = new BLL.ToDoReply();
            dal.Add(info, CurrentUserInfo.PreferredLanguage, preTitle);
            return 1;
        }

        #endregion
    }
}
