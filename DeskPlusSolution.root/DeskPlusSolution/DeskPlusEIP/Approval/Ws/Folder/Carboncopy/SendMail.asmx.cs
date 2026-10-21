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

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Carboncopy
{
    /// <summary>
    /// SendMail의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class SendMail : Base.BaseWSApproval
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 메일 전송
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddMail(string p)
        {
            try
            {
                SetDecryptString(p);

                string cirDocID = GetDecryptParam("cirDocID", "");
                string userList = GetDecryptParam("userList", "");//구분$키값 (M$1,M$2)
                string subject = GetDecryptParam("subject", "");

                if (cirDocID.Length > 0 && userList.Length > 0)
                {
                    string to = "";
                    Zeospace.DeskPlus.Account.DAL.User dal = new Account.DAL.User();

                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); //메일 정규식

                    foreach (string userID in userList.Split(','))
                    {
                        if (userID.Length > 0)
                        {
                            var uList = dal.GetUserInfo(CurrentUserInfo.CompanyID, userID.Split('$')[1].ToNumber<int>(), true, true);
                            foreach (var info in uList)
                            {
                                Match match = regex.Match(info.MailID + "@" + info.MailDomain);
                                if (match.Success) to += info.MailID + "@" + info.MailDomain + ";";
                            }
                        }
                    }

                    to = to.TrimEnd(';');

                    Zeospace.DeskPlus.Approval.DAL.CirculationDocument docBiz = new DAL.CirculationDocument();
                    string contents = docBiz.GetContentHtml(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());

                    Zeospace.DeskPlus.Approval.DAL.CirculationAttachFile att = new DAL.CirculationAttachFile();
                    var attList = att.GetAttachFileLists(CurrentUserInfo.CompanyID, cirDocID.ToNumber<int>());

                    Hashtable attach = new Hashtable();

                    foreach (var attInfo in attList)
                    {
                        attach.Add(
                            Server.MapPath(attInfo.FilePath + attInfo.FileName),
                            Server.MapPath(attInfo.FilePath + attInfo.FileGUID)
                        );
                    }

                    Zeospace.DeskPlus.Common.BLL.MailZettaFactory biz = new DeskPlus.Common.BLL.MailZettaFactory();
                    biz.CreateMHT(CurrentUserInfo.MailID + "@" + CurrentUserInfo.MailDomain, CurrentUserInfo.Name, to,
                        "", "", subject, contents, attach, "", false);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }
    }
}
