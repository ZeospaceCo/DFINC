using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Report.Web.Base;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// ConfirmUserList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class ConfirmUserList : Base.BaseWSReport
    {
        /// <summary>
        /// Gets the page list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<ConfirmUserEntity> GetPageList(string p)
        {
            SetDecryptString(p);

            string startDate = GetDecryptParam("startDate", "");
            string gubun = GetDecryptParam("gubun", "1"); //1:개인용;2:보고용

            if (startDate.Length == 0 && gubun.Length == 0) return null;

            try
            {
                string titleName = "";
                string nameBase = "";
                int listID = 0;
                string flag = "";

                DAL.List dalList = new DAL.List();
                DAL.ShareUser dal = new DAL.ShareUser();
                DataSet getUserList = dal.GetShareUserListTargetUserID(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "1");

                List<ConfirmUserEntity> al = new List<ConfirmUserEntity>();

                if (gubun == "2")
                {
                    var dtrList = dalList.GetListByStartDate(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID.ToString(),
                        startDate, AppSettingHelper.GetDescription(FlagEnum.All));

                    if (dtrList != null)
                    {
                        if (dtrList.HasRows)
                        {
                            while (dtrList.Read())
                            {
                                listID = dtrList["ListID"].ToString().ToNumber<int>();
                                flag = dtrList["Flag"].ToString();
                            }
                        }
                    }

                    al.Add(new ConfirmUserEntity()
                    {
                        UserID = CurrentUserInfo.UserID, //보고대상자 아이디
                        ListID = listID, //리스트 아이디
                        Flag = flag, //상태
                        NameBase = CurrentUserInfo.TitleName + " " + CurrentUserInfo.Name, //보고 대상자 이름
                        TargetUserID = CurrentUserInfo.UserID //나의 아이디
                    });
                }

                if (getUserList != null)
                {
                    foreach (DataRow dtr in getUserList.Tables[0].Rows)
                    {
                        listID = 0;
                        flag = "";

                        titleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);

                        nameBase = this.GetLangName(
                                dtr["NameBase"].ToString(),
                                dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                PreLang);

                        var dtrList = dalList.GetListByStartDate(CurrentUserInfo.CompanyID, dtr["UserID"].ToString(), startDate, AppSettingHelper.GetDescription(FlagEnum.All));

                        if (dtrList != null)
                        {
                            if (dtrList.HasRows)
                            {
                                while (dtrList.Read())
                                {
                                    listID = dtrList["ListID"].ToString().ToNumber<int>();
                                    flag = dtrList["Flag"].ToString();
                                }
                            }
                        }

                        al.Add(new ConfirmUserEntity()
                        {
                            UserID = dtr["UserID"].ToString().ToNumber<int>(), //보고대상자 아이디
                            ListID = listID, //리스트 아이디
                            Flag = flag, //상태
                            NameBase = titleName + " " + nameBase, //보고 대상자 이름
                            TargetUserID = dtr["TargetUserID"].ToString().ToNumber<int>() //나의 아이디
                        });
                    }
                }

                return al;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class ConfirmUserEntity
    {
        public int UserID { get; set; }

        public int ListID { get; set; }

        public string Flag { get; set; }

        public string NameBase { get; set; }

        public int TargetUserID { get; set; }
    }
}