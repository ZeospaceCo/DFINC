using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.BE;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSReport
    {
        #region 첨부파일

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAttachFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");

                ObjList listobj = new ObjList();

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachLists(CurrentUserInfo.CompanyID, listID.ToNumber<int>());

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Report.BE.AttachInfo info = new AttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    //guid 가 없다는 것은 마이그래이션을 해서 그러함으로, filename 을 참조할 수도 있다.
                    info.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : info.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;

                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    listobj.Add(info);
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 첨부파일

        #region 댓글

        /// <summary>
        /// 댓글 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetShortReplyList(string p)
        {
            string listID = string.Empty;
            SetDecryptString(p);
            listID = GetDecryptParam("listID", "0");

            ObjList listobj = new ObjList();

            DAL.ShortReply sr = new DAL.ShortReply();

            DataTableReader dtr = sr.GetShortReplyList(CurrentUserInfo.CompanyID, listID.ToNumber<int>());
            if (dtr != null)
            {
                string name = "";

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Zeospace.DeskPlus.Report.BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.ShortReplyID = dtr["ShortReplyID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.UserName = name;
                        info.IsAuth = (info.UserID == CurrentUserInfo.UserID) ? "Y" : "N";

                        DateTime dt;

                        if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                            info.StrInputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.StrInputDate = string.Empty;

                        #region 날짜 비교(New Img)

                        DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                        DateTime todayDate = System.DateTime.Now;
                        TimeSpan gap = todayDate.Subtract(writeDate);

                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";

                        #endregion 날짜 비교(New Img)

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        #endregion 댓글

        #region DEL()

        #region 댓글

        /// <summary>
        /// 댓글 DEL
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "0");
            string shortReplyID = GetDecryptParam("ShortReplyID", "0");

            DAL.ShortReply biz = new DAL.ShortReply();
            BE.ShortReplyInfo info = biz.GetShortReplyListID(CurrentUserInfo.CompanyID,
                listID.ToNumber<int>(), shortReplyID.ToNumber<int>());

            if (info.UserID == CurrentUserInfo.UserID)
            {
                return biz.Del(CurrentUserInfo.CompanyID,
                    listID.ToNumber<int>(),
                    shortReplyID.ToNumber<int>());
            }
            else
            {
                return false;
            }
        }

        #endregion 댓글

        #endregion DEL()

        #region MOD()

        /// <summary>
        /// 일지확인 수정(2:요청,3:확인)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool FlagComplate(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");
            string flag = GetDecryptParam("flag", "0");
            if (listID == "0" || flag == "0") return false;

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Ws.Content.View).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            string msg = "";
            int writeUserID = 0;

            if (flag == "3")
            {
                DAL.List dl = new DAL.List();
                List<ListInfo> list = dl.GetList(CurrentUserInfo.CompanyID, listID);
                foreach (var info in list)
                {
                    //작성자
                    string name = this.GetLangName(
                                            info.CompNameBase.ToString().ToNull(), info.CompNameKo.ToString().ToNull(),
                                            info.CompNameEn.ToString().ToNull(), info.CompNameJa.ToString().ToNull(),
                                            info.CompNameFr.ToString().ToNull(), info.CompNameZhcn.ToString().ToNull(),
                                            info.CompNameZhtw.ToString().ToNull(), PreLang);

                    string dept = this.GetLangName(
                                            info.CompDeptBase.ToString().ToNull(), info.CompDeptKo.ToString().ToNull(),
                                            info.CompDeptEn.ToString().ToNull(), info.CompDeptJa.ToString().ToNull(),
                                            info.CompDeptFr.ToString().ToNull(), info.CompDeptZhcn.ToString().ToNull(),
                                            info.CompDeptZhtw.ToString().ToNull(), PreLang);

                    //알람 발송 글자
                    msg = dept + " " + name + " " + GetResource("WorkConfirmOK");

                    writeUserID = info.WriteUserID;
                }
            }

            BLL.List biz = new BLL.List();
            return biz.ConfirmMod(CurrentUserInfo.CompanyID, listID.ToNumber<int>(), writeUserID, CurrentUserInfo.UserID, CurrentUserInfo.Name, flag, msg);
        }

        #endregion MOD()
    }
}