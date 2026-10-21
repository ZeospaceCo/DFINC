using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Admin.Reserve
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSRM
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 첨부 파일가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFileList(string p)
        {
            try
            {
                SetDecryptString(p);
                int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

                DAL.Attach dal = new DAL.Attach();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, 0, reservationID, "0");

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new BE.AttachInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.AttachFileID = dtr["AttachFileID"].ToString().ToNumber<int>();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.FilePath = dtr["FilePath"].ToString();
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", info.FilePath + info.FileGUID);
                            paraDic.Add("fileName", info.FileName);
                            info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                            info.FileType = dtr["FileType"].ToString().Replace(".", "");

                            info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(dtr["FileLength"].ToString()) / 1024) + " KB";

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
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 반복 일정이 있는 지 체크
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool GetRepeat(string p)
        {
            try
            {
                bool rtnVal = false;

                SetDecryptString(p);
                int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

                DAL.Repeat dal = new DAL.Repeat();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, reservationID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            rtnVal = true;
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
   
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 댓글 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShortReplyList(string p)
        {
            SetDecryptString(p);
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            List<object> listobj = new List<object>();

            DAL.ShortReply sr = new DAL.ShortReply();

            DataTableReader dtr = sr.GetShortReplyList(CurrentUserInfo.CompanyID, reservationID);
            if (dtr != null)
            {
                string name = "";

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.ShortReplyInfo info = new BE.ShortReplyInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();                        
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
                            info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.InputDate = string.Empty;

                        #region 날짜 비교(New Img)

                        DateTime writeDate = Convert.ToDateTime(dtr["InputDate"].ToString());
                        DateTime todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                        TimeSpan gap = todayDate.Subtract(writeDate);

                        info.ShowNew = (gap.Days <= 0) ? "Y" : "N";

                        #endregion 날짜 비교(New Img)

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성목적 : 공유자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShareUserList(string p)
        {
            SetDecryptString(p);
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            List<object> listobj = new List<object>();
                        
            DAL.ShareUser dal = new DAL.ShareUser();

            DataTableReader dtr = dal.GetShareUserList(CurrentUserInfo.CompanyID, reservationID);
            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {                        
                        BE.ShareUserInfo info = new BE.ShareUserInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.ShareUserID = dtr["ShareUserID"].ToString().ToNumber<int>();
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();
                        info.DeptID = dtr["DeptID"].ToString().ToNumber<int>();
                        info.Kind = dtr["Kind"].ToString();

                        info.NameBase = dtr["NameBase"].ToString();
                        info.NameKo = dtr["NameKo"].ToString();
                        info.NameEn = dtr["NameEn"].ToString();
                        info.NameJa = dtr["NameJa"].ToString();
                        info.NameFr = dtr["NameFr"].ToString();
                        info.NameZhcn = dtr["NameZhcn"].ToString();
                        info.NameZhtw = dtr["NameZhtw"].ToString();
                        
                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 거부사유 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRejectList(string p)
        {
            SetDecryptString(p);
            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            List<object> listobj = new List<object>();

            DAL.Reject dal = new DAL.Reject();

            DataTableReader dtr = dal.GetRejectList(CurrentUserInfo.CompanyID, reservationID);
            if (dtr != null)
            {
                string name = "";

                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.RejectInfo info = new BE.RejectInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                        info.RejectID = dtr["RejectID"].ToString().ToNumber<int>();
                        info.Content = dtr["Content"].ToString().Replace("\n", "<br>");
                        info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                        name = this.GetLangName(
                            dtr["NameBase"].ToString(),
                            dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                            dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                            PreLang);

                        info.UserName = name;

                        DateTime dt;

                        if (DateTime.TryParse(dtr["InputDate"].ToString(), out dt))
                            info.InputDate = DateHelpers.GetCultureDate(dt, PreLangReal).Substring(0, 10);
                        else
                            info.InputDate = string.Empty;

                        listobj.Add(info);
                    }
                }
            }

            return listobj;
        }

        #endregion GET()

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2017-03-08
        /// 작성목적 : 댓글 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShortReply(string p)
        {
            SetDecryptString(p);
            string reservationID = GetDecryptParam("reservationID", "0");
            string shortReplyID = GetDecryptParam("shortReplyID", "0");

            DAL.ShortReply dal = new DAL.ShortReply();
            return dal.Del(CurrentUserInfo.CompanyID, reservationID.ToNumber<int>(), shortReplyID.ToNumber<int>());            
        }

        #endregion 
    }
}
