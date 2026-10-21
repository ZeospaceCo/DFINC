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

namespace Zeospace.DeskPlus.Pim.Web.Ws.ShortReply
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
        #region 댓글(나에게만 보내는)

        #region ADD()

        /// <summary>
        /// 댓글 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddShortReply(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.ShortReply.ShortReply).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string nowDate = GetDecryptParam("nowDate", "");
            string contents = GetDecryptParam("contents", "");
            string preTitle = string.Empty;

            BE.ShortReplyInfo info = new ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;
            info.MyDeptName = CurrentUserInfo.DeptName;
            info.RUserID = CurrentUserInfo.UserID;
            info.JobDate = nowDate;

            preTitle = resMng.GetString("ShortReply") + "," + resMng.GetString("NoteAlarm");

            //BLL.ShortReply dal = new BLL.ShortReply();
            //dal.Add(info, CurrentUserInfo.PreferredLanguage, preTitle);

            DAL.ShortReply replyDal = new DAL.ShortReply();
            replyDal.Add(info);
            return 1;
        }

        /// <summary>
        /// 댓글 추가(다른사람에게 댓글 쓰기)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AddToShortReply(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Pim.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Pim.Web.Ws.ShortReply.ShortReply).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            SetDecryptString(p);
            string jobDate = GetDecryptParam("jobDate", "");
            string contents = GetDecryptParam("contents", "");
            string rUserID = GetDecryptParam("rUserID", "");
            string security = GetDecryptParam("security", "0");
            string groupID = GetDecryptParam("groupID", "0");
            string preTitle = string.Empty;
            bool check = false;

            BE.ShortReplyInfo info = new ShortReplyInfo();

            info.CompanyID = CurrentUserInfo.CompanyID;
            info.Content = Server.UrlDecode(contents).ToReverseXSS();
            info.UserID = CurrentUserInfo.UserID;
            info.UserName = CurrentUserInfo.Name;
            info.MyDeptName = CurrentUserInfo.DeptName;
            info.RUserID = rUserID.ToNumber<int>();
            info.JobDate = DateHelpers.GetCultureDate(jobDate, "yyyyMMdd", PreLangReal);
            info.IsSecurity = security;
            info.GroupID = groupID.ToNumber<int>();

            preTitle = resMng.GetString("ShortReply") + "," + resMng.GetString("NoteAlarm");

            // 자신이 댓글을 추가했을 경우
            if(info.RUserID == CurrentUserInfo.UserID)
                check = true;

            BLL.ShortReply dal = new BLL.ShortReply();
            dal.Add(info, CurrentUserInfo.PreferredLanguage, preTitle, check);

            //DAL.ShortReply replyDal = new DAL.ShortReply();
            //replyDal.Add(info);
            return 1;
        }
        #endregion

        #endregion

        #region Del()

        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            try
            {
                SetDecryptString(p);
                string shortReplyID = GetDecryptParam("replyID", "0");

                DAL.ShortReply dal = new DAL.ShortReply();
                return dal.Del(CurrentUserInfo.CompanyID, shortReplyID.ToNumber<int>(), CurrentUserInfo.UserID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion
    }
}
