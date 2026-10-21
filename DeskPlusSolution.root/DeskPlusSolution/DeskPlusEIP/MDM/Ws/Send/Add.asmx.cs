using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Data;
using Zeospace.DeskPlus.MDM.BE;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;

namespace Zeospace.DeskPlus.MDM.Web.Ws.Send
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSMDM
    {

        /// <summary>
        /// 작성자 : famo23
        /// 작성목적 : 자동완성
        /// 작성일자 : 2020-03-02
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetAutoCompleteData(string p)
        {
            try
            {
                SetDecryptString(p);
                string txt = GetDecryptParam("txt", "0");
                txt = Server.UrlDecode(txt);

                string before = txt.Trim();
                string tmpPrefix = txt.Trim();
                string[] ArrPrefix = txt.Split(';');
                string tmpContent = "";

                if (ArrPrefix.Length > 1)
                {
                    tmpPrefix = ArrPrefix[ArrPrefix.Length - 1].Trim();
                    if (ArrPrefix[ArrPrefix.Length - 1].Length > 1)
                    {
                        if (ArrPrefix[ArrPrefix.Length - 1].Substring(0, 1) == "\n")
                        {
                            tmpContent = "\n";
                        }
                    }
                    int index = txt.LastIndexOf(";");
                    before = txt.Substring(0, index + 1);
                }
                else
                {
                    before = "";
                }

                List<object> listobj = new List<object>();

                if (tmpPrefix.Trim() != "")
                {
                    DAL.Send dal = new DAL.Send();
                    DataTableReader dtr = dal.GetAutoName(CurrentUserInfo.CompanyID, tmpPrefix);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                BE.SearchInfo info = new BE.SearchInfo();

                                if (dtr["mGubun"].ToString() == "M")
                                {
                                    info.Name = "<#" + dtr["mID"].ToString() + ">" + dtr["DeptBase"].ToString() + "/" + dtr["TitleBase"].ToString() + "/" + dtr["NameBase"].ToString();
                                }
                                else
                                {
                                    info.Name = "<@" + dtr["mID"].ToString() + ">" + dtr["NameBase"].ToString();
                                }

                                info.NameStr = before + tmpContent + info.Name + ";";

                                listobj.Add(info);
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


        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2020-03-03
        /// 작성목적 : 알림 발송
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string AddSend(string p)
        {
            try
            {
                SetDecryptString(p);

                string Header = GetDecryptParam("header", "");
                string Receive = GetDecryptParam("receive", "");
                string Content = GetDecryptParam("content", "");
                string CategoryID = GetDecryptParam("categoryID", "0");
                string ListID = GetDecryptParam("listID", "0");

                Receive = Server.UrlDecode(Receive);

                string strReceive = String.Empty;
                string ReceiveKey = String.Empty;

                if (ListID == "0")
                {
                    // 수신자 
                    DAL.Receive dal = new DAL.Receive();
                    ToInfo toInfo = new ToInfo();                   

                    string[] ArrRevice = Receive.TrimEnd(';').Split(';');

                    for (int i = 0; i < ArrRevice.Length; i++)
                    {
                        if (ArrRevice[i].Trim() != "")
                        {
                            if ((ArrRevice[i].IndexOf("<") < 0) && (ArrRevice[i].IndexOf(">") < 0))
                            {
                                string tmpUser = ArrRevice[i].Trim();

                                if (tmpUser != "")
                                {
                                    DataSet ds = dal.GetUserDept(CurrentUserInfo.CompanyID, tmpUser);

                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds.Tables[0].Rows.Count > 1)
                                        {
                                            return "1^" + tmpUser;
                                        }
                                        else
                                        {
                                            foreach (DataRow dr in ds.Tables[0].Rows)
                                            {
                                                if (dr["IsGroup"].ToString() == "M")
                                                {
                                                    if (!toInfo.ToRUser.Contains(dr["mID"].ToString()))
                                                    {
                                                        toInfo.ToRUser.Add(dr["mID"].ToString(), dr["mID"].ToString());
                                                        ReceiveKey += "M_" + dr["mID"].ToString() + ";";
                                                    }

                                                    if (!toInfo.ToUser.Contains(dr["mID"].ToString()))
                                                    {
                                                        toInfo.ToUser.Add(dr["mID"].ToString(), dr["mID"].ToString());
                                                        strReceive += dr["mID"].ToString() + ";";
                                                    }
                                                }
                                                else
                                                {
                                                    if (!toInfo.ToDept.Contains(dr["mID"].ToString()))
                                                    {
                                                        toInfo.ToDept.Add(dr["mID"].ToString(), dr["mID"].ToString());
                                                        ReceiveKey += "G_" + dr["mID"].ToString() + ";";

                                                        DAL.Receive uDal = new DAL.Receive();
                                                        DataSet dsU = uDal.GetUserInfoByDept(CurrentUserInfo.CompanyID, dr["mID"].ToString());

                                                        if (dsU.Tables[0].Rows.Count > 0)
                                                        {
                                                            foreach (DataRow drU in dsU.Tables[0].Rows)
                                                            {
                                                                if (!toInfo.ToUser.Contains(drU["UserID"].ToString()))
                                                                {
                                                                    toInfo.ToUser.Add(drU["UserID"].ToString(), drU["UserID"].ToString());
                                                                    strReceive += drU["UserID"].ToString() + ";";
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return "2^" + tmpUser;
                                    }
                                }
                            }
                            else
                            {
                                if ((ArrRevice[i].IndexOf("<") > -1) && (ArrRevice[i].IndexOf(">") < 0))
                                {
                                    return "3^" + ArrRevice[i].Trim();
                                }

                                if ((ArrRevice[i].IndexOf(">") > -1) && (ArrRevice[i].IndexOf("<") < 0))
                                {
                                    return "3^" + ArrRevice[i].Trim();
                                }

                                int startIdx = ArrRevice[i].IndexOf("<");
                                int endIdx = ArrRevice[i].IndexOf(">");
                                string tmpTo = ArrRevice[i].Substring(startIdx + 1, endIdx - startIdx - 1);

                                if (tmpTo.IndexOf("#") > -1)
                                {
                                    tmpTo = tmpTo.Replace("#", "");

                                    if (!toInfo.ToRUser.Contains(tmpTo))
                                    {
                                        toInfo.ToRUser.Add(tmpTo, tmpTo);
                                        ReceiveKey += "M_" + tmpTo + ";";
                                    }

                                    if (!toInfo.ToUser.Contains(tmpTo))
                                    {
                                        toInfo.ToUser.Add(tmpTo, tmpTo);
                                        strReceive += tmpTo + ";";
                                    }
                                }
                                else if (tmpTo.IndexOf("@") > -1)
                                {
                                    tmpTo = tmpTo.Replace("@", "");

                                    if (!toInfo.ToDept.Contains(tmpTo))
                                    {
                                        toInfo.ToDept.Add(tmpTo, tmpTo);
                                        ReceiveKey += "G_" + tmpTo + ";";

                                        DAL.Receive uDal = new DAL.Receive();
                                        DataSet dsU = uDal.GetUserInfoByDept(CurrentUserInfo.CompanyID, tmpTo);

                                        if (dsU.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow drU in dsU.Tables[0].Rows)
                                            {
                                                if (!toInfo.ToUser.Contains(drU["UserID"].ToString()))
                                                {
                                                    toInfo.ToUser.Add(drU["UserID"].ToString(), drU["UserID"].ToString());
                                                    strReceive += drU["UserID"].ToString() + ";";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                BE.SendInfo info = new BE.SendInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.Header = Server.UrlDecode(Header);
                info.Content = Server.UrlDecode(Content);
                info.UserID = CurrentUserInfo.UserID;
                info.UserName = CurrentUserInfo.Name;
                info.CategoryID = CategoryID.ToNumber<int>();
                info.ListID = ListID.ToNumber<int>();
                info.AlarmURL = "/DeskPlusMobile/MDM/List.aspx";

                if (info.CategoryID > 0 && info.ListID > 0)
                {
                    string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
                    string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
                    string alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "AlarmTargetURL", "");
                    alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("categoryID", CategoryID);
                    paraDic.Add("listID", ListID);

                    HttpContext context = HttpContext.Current;
                    string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                    alarmTargetURL = alarmTargetURL + "?p=" + encparam;

                    string alarmMobileURL = "/DeskPlusMobile/Board/AlarmView.aspx?p=" + encparam;

                    info.TargetURL = alarmTargetURL;
                    info.MobileTargetURL = alarmMobileURL;
                }
                else
                {
                    info.TargetURL = "";
                    info.MobileTargetURL = "";
                }
                
                BLL.Send bll = new BLL.Send();
                bll.Add(info, ReceiveKey, strReceive);                

                return "TRUE";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex.Message);
                return "FALSE";
            }
        }

        #endregion 

        [Serializable]
        public class ToInfo
        {
            #region 생성자

            /// <summary>
            /// 
            /// </summary>
            public ToInfo()
            {
                toUser = new Hashtable();
                toRUser = new Hashtable();
                toDept = new Hashtable();
            }

            #endregion

            #region 필드

            /// <summary>
            /// toUser
            /// </summary>
            private Hashtable toUser = null;

            /// <summary>
            /// toUser
            /// </summary>
            private Hashtable toRUser = null;

            /// <summary>
            /// toDept
            /// </summary>
            private Hashtable toDept = null;

            #endregion

            #region 프로퍼티

            /// <summary>
            /// toUser.
            /// </summary>
            public Hashtable ToUser
            {
                get { return toUser; }
                set { toUser = value; }
            }

            /// <summary>
            /// toRUser.
            /// </summary>
            public Hashtable ToRUser
            {
                get { return toRUser; }
                set { toRUser = value; }
            }

            /// <summary>
            /// toDept.
            /// </summary>
            public Hashtable ToDept
            {
                get { return toDept; }
                set { toDept = value; }
            }
         
            #endregion
        }
    }
}
