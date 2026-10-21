using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
{
    /// <summary>
    /// SendMail의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class SendMail : Base.BaseWSBoard
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

                string categoryID = GetDecryptParam("categoryID", "");
                string listID = GetDecryptParam("listID", "");
                string userList = GetDecryptParam("userList", "");//구분$키값 (M$1,M$2)
                string subject = GetDecryptParam("subject", "");

                if (categoryID.Length > 0 && listID.Length > 0 && userList.Length > 0)
                {
                    string to = "";

                    #region 전체계열사 인사정보 가져오기

                    string users = "";
                    foreach (string userID in userList.Split(',')) users += userID.Split('$')[1] + ",";

                    if (users.Length > 0)
                    {
                        users = users.TrimEnd(',');
                        //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"); //메일 정규식
                        Regex regex = new Regex(@"^[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*@[0-9a-zA-Z]([-_.]?[0-9a-zA-Z])*.[a-zA-Z]{2,3}$");
                        Zeospace.DeskPlus.Account.DAL.User dalUser = new Account.DAL.User();

                        //전사 조직도 가져오기
                        var uList = dalUser.GetUserTotInfo(CurrentUserInfo.CompanyID, users, true, true);

                        foreach (var usr in uList)
                        {
                            Match match = regex.Match(usr.MailID + "@" + usr.MailDomain);
                            if (match.Success) to += usr.MailID + "@" + usr.MailDomain + ";";
                        }
                    }

                    to = to.TrimEnd(';');

                    #endregion 전체계열사 인사정보 가져오기

                    //통합게시판 회사아이디
                    int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());
                    Zeospace.DeskPlus.Board.DAL.Lists dalList = new DAL.Lists();
                    var list = dalList.GetList(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());
                    foreach (var info in list)
                    {
                        string contents = info.Content;

                        Zeospace.DeskPlus.Board.DAL.AttachFiles att = new DAL.AttachFiles();
                        var attList = att.GetAttachFileLists(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

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
                    }

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