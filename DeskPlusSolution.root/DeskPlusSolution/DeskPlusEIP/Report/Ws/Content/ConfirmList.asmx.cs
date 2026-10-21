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
    /// ConfirmList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ConfirmList : Base.BaseWSReport
    {
        #region 리스트

        /// <summary>
        /// 작성자 : shine
        /// 작업일지 리스트 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetPageList(string p)
        {
            try
            {
                SetDecryptString(p);

                string years = GetDecryptParam("years", "");
                string months = GetDecryptParam("months", "");
                string yearmonth = months.Length == 1 ? years + "0" + months : years + months;

                string targetUserID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
                string targetDeptID = GetDecryptParam("deptID", CurrentUserInfo.DeptID.ToString());
                string order = GetDecryptParam("order", "");
                string flag = GetDecryptParam("flag", "0");

                int week = GetDecryptParam("week", "0").ToNumber<int>();
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.List biz = new DAL.List();
                DAL.ShareUser share = new DAL.ShareUser();
                ObjList listobj = new ObjList();

                //보고대상자 가져오기 (1:보고대상자;0:공유자)
                DataSet dsShare = share.GetShareUserListTargetUserID(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "1");

                var getWeekList = DateHelpers.GetMonFriByYearMonth(years.ToNumber<int>(), months.ToNumber<int>(), week);
                string weekStartDate = "";
                string shareUserList = "";

                if (week != 0)
                {
                    foreach (KeyValuePair<DateTime, DateTime> dic in getWeekList)
                    {
                        weekStartDate = dic.Key.ToString("yyyyMMdd");
                    }
                }

                // 공유 유저아이디 가져오기
                if (dsShare != null)
                {
                    foreach (DataRow dr in dsShare.Tables[0].Rows)
                    {
                        shareUserList += dr["UserID"].ToString() + ",";
                    }
                }
                shareUserList = shareUserList.TrimEnd(',');

                if (shareUserList == "")
                    return listobj;

                DataTableReader dtr = biz.GetConfirmList(CurrentUserInfo.CompanyID, yearmonth, shareUserList, weekStartDate, order, searchField, searchValue, flag, CurrentUserInfo.UserID);
                bool holChk = true;
                string tmpUserID = "";
                int i = 0;

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            //이관된 문서는 제외한다.
                            if (dtr["IsLinked"].ToString() == "0")
                            {
                                if (i == 0)
                                {
                                    holChk = !holChk;
                                    tmpUserID = dtr["StartDate"].ToString();
                                }

                                if (tmpUserID != dtr["StartDate"].ToString())
                                {
                                    tmpUserID = dtr["StartDate"].ToString();
                                    holChk = !holChk;
                                }

                                var info = new BE.ListInfo();

                                info.StartDate = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                                info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                                info.StartDateYMD = dtr["StartDate"].ToString();
                                info.EndDateYMD = dtr["EndDate"].ToString();
                                info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                                info.FileCount = dtr["FileCount"].ToString().ToNumber<int>() > 0 ? 1 : 0;
                                info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();
                                info.Flag = dtr["Flag"].ToString();
                                info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                                info.HolChk = (holChk == true) ? "1" : "0";
                                info.IsClose = dtr["IsClose"].ToString();
                                info.ViewCount = dtr["ViewCount"].ToString().ToNumber<int>();

                                #region 이름 다국어 설정

                                info.NameBase = this.GetLangName(
                                        dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                        dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                        dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                        dtr["NameZhtw"].ToString().ToNull(), PreLang);

                                #endregion 이름 다국어 설정

                                #region 직급 다국어 설정

                                info.TitleName = this.GetLangName(
                                        dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                        dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                        dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                        dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                                #endregion 직급 다국어 설정

                                #region 날짜 비교(New Img)

                                if (dtr["ReplyDate"].ToString().ToNull() != "")
                                {
                                    DateTime writeDate = Convert.ToDateTime(dtr["ReplyDate"].ToString());
                                    DateTime todayDate = System.DateTime.Now;
                                    TimeSpan gap = todayDate.Subtract(writeDate);
                                    int boardNewShowDay = AppSettingHelper.GetAppSetting(AppSettingType.COMMUNITY, "BoardNewShowDay", "0").ToNumber<int>();

                                    info.ShowNew = (gap.Days <= boardNewShowDay) ? "Y" : "N";
                                }
                                else
                                    info.ShowNew = "N";

                                #endregion 날짜 비교(New Img)

                                listobj.Add(info);

                                i++;
                            }
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

        #endregion 리스트

        #region 첨부파일

        /// <summary>
        /// 작성자 : shine
        /// 작업일지 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            try
            {
                int listID = 0;
                SetDecryptString(p);
                listID = GetDecryptParam("listID", "0").ToNumber<Int32>();

                string response = "0";

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachLists(CurrentUserInfo.CompanyID, listID);

                string totalFileInfo = "";
                string encFilePath = "";

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    encFilePath = (_attachInfo.FileGUID != "") ? _attachInfo.FilePath + _attachInfo.FileGUID : _attachInfo.FilePath + _attachInfo.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", encFilePath);
                    paraDic.Add("fileName", _attachInfo.FileName);
                    encFilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    totalFileInfo += _attachInfo.FileName + ":" + encFilePath + "|"; // Key < 파일경로파일명
                }

                totalFileInfo = totalFileInfo.TrimEnd('|');

                if (totalFileInfo.Length > 0)
                {
                    response = totalFileInfo.Replace("&", "&amp;");		// 첨부파일 정보 가져오기
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion 첨부파일

        #region GET()

        /// <summary>
        /// 작성자 : shine
        /// 비공개 작업일지 관리자 여부 확인(확인요청함)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int AuthCheck(string p)
        {
            SetDecryptString(p);

            string userID = GetDecryptParam("userID", CurrentUserInfo.UserID.ToString());
            string useComplete = GetDecryptParam("useComplete", "");

            DAL.ShareUser share = new DAL.ShareUser();
            int icnt = 0;

            if (userID.ToNumber<int>() != CurrentUserInfo.UserID)
            {
                icnt = share.GetShareUserCntTargetUserID(CurrentUserInfo.CompanyID, userID.ToNumber<int>(), CurrentUserInfo.UserID, useComplete);
                return icnt;
            }
            else
            {
                return 1;
            }
        }

        #endregion GET()

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작업일지 확인 여부 수정(요청->확인)
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Confirm(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");

            if (listID.Length == 0) return false;

            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Ws.Content.ConfirmList).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            string msg = "";
            int writeUserID = 0;

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

            BLL.List biz = new BLL.List();
            return biz.ConfirmMod(CurrentUserInfo.CompanyID, listID.ToNumber<int>(), writeUserID, CurrentUserInfo.UserID, CurrentUserInfo.Name, "3", msg);
        }

        #endregion MOD()
    }
}