using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Portlet.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Globalization;

namespace Zeospace.DeskPlus.Portlet.Web.Ws.TodoPopup
{
    /// <summary>
    /// TodoList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class TodoList : Base.BaseWSPortlet
    {
        //설정저장
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetSetting(string p)
        {
            try
            {
                SetDecryptString(p);
                string todoMenu = GetDecryptParam("todomenu", "APP-S,APP-W,TAS-S,PIM-S").TrimEnd(',');

                Zeospace.DeskPlus.Account.DAL.UserEnvironment dal = new Account.DAL.UserEnvironment();
                return dal.ModTodoMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, todoMenu);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        //리스트,건수가져오기
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ItemInfo GetList(string p)
        {
            try
            {
                SetDecryptString(p);
                /*
                전자결재-승인		(APP-S)
                전자결재-대기		(APP-W)
                업무협업-진행		(TAS-S)
                업무협업-대기		(TAS-W)
                일정관리-할일		(PIM-T)
                일정관리-일정		(PIM-S)
                자원관리-예약현황	(RM-C) 
                자원관리자-승인대기	(RM-W)	
                */

                #region 다국어 리소스 로드/Title 정보
                this.ResourceName = "Zeospace.DeskPlus.Portlet.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.Portlet.Web.Ws.TodoPopup.TodoList).Assembly;	//어셈블리 선언
                this.GetResourceManager();
                #endregion

                string itemid = GetDecryptParam("itemid", "item_0");
                string menu = GetDecryptParam("menu", "");
                DateTime tmpday = DateTime.Now;

                int companyID = CurrentUserInfo.CompanyID;
                int userID = CurrentUserInfo.UserID;
                int deptID = CurrentUserInfo.DeptID;
                int pagesize = 4;

                ItemInfo iteminfo = new ItemInfo();

                iteminfo.CLIENTID = itemid;
                iteminfo.MENU = menu;

                DataTableReader dtr;
                Zeospace.DeskPlus.Portlet.DAL.TodoPopup dal = new DAL.TodoPopup();

                List<ListInfo> list = null;

                // 항목추가 시, 수정필요.(환경설정ID값매칭)
                switch (menu.ToString())
                {
                    case "APP-S":
                        #region 전자결재-승인

                        iteminfo.TITLE = this.GetResource("TodoItem01");        //전자결재-승인
                        iteminfo.COUNT = dal.GetApproveCnt(companyID, userID).ToString();
                        iteminfo.URL = "";

                        Dictionary<string, string> paraDic = null;
                        list = new List<ListInfo>();
                        dtr = dal.GetApproveList(companyID, userID, pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["DocumentID"].ToString();
                                    info.SUBJ = dtr["Subject"].ToString();
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteTime"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("documentID", dtr["DocumentID"].ToString());
                                    paraDic.Add("sequence", dtr["Sequence"].ToString());
                                    paraDic.Add("approvalTag", dtr["ApprovalTag"].ToString());
                                    paraDic.Add("targetUserID", dtr["TargetUserID"].ToString());
                                    paraDic.Add("targetDeptID", dtr["TargetDeptID"].ToString());
                                    paraDic.Add("proxyUserID", "0");

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskplusEIP/Approval/Process/Basic/ApprovePopup.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "APP-W":
                        #region 전자결재-대기

                        iteminfo.TITLE = this.GetResource("TodoItem02");        //"전자결재-대기";
                        iteminfo.COUNT = dal.GetSubmitCnt(companyID, userID).ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetSubmitList(companyID, userID, pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["DocumentID"].ToString();
                                    info.SUBJ = dtr["Subject"].ToString();
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteTime"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("documentID", dtr["DocumentID"].ToString());
                                    paraDic.Add("srh", "srh");

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskplusEIP/Approval/Common/Popup/ViewTot.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "TAS-S":
                        #region 업무협업-진행

                        iteminfo.TITLE = this.GetResource("TodoItem03");        //"업무협업-진행";
                        iteminfo.COUNT = dal.GetTASCnt(companyID, userID, "S").ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetTASList(companyID, userID, "S", pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ListID"].ToString();
                                    info.SUBJ = dtr["Title"].ToString();
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteDate"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("listID", dtr["ListID"].ToString());
                                    paraDic.Add("groupID", dtr["GroupID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/Task/Common/ViewPopupTotal.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "TAS-W":
                        #region 업무협업-대기

                        iteminfo.TITLE = this.GetResource("TodoItem04");        //"업무협업-대기";
                        iteminfo.COUNT = dal.GetTASCnt(companyID, userID, "W").ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetTASList(companyID, userID, "W", pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ListID"].ToString();
                                    info.SUBJ = dtr["Title"].ToString();
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteDate"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("listID", dtr["ListID"].ToString());
                                    paraDic.Add("groupID", dtr["GroupID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/Task/Common/ViewPopupTotal.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "PIM-T":
                        #region 일정관리-할일

                        iteminfo.TITLE = this.GetResource("TodoItem05");        //"일정관리-할일";
                        iteminfo.COUNT = dal.GetPimTodoListCnt(companyID, userID, tmpday).ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetPimTodoList(companyID, userID, tmpday, pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ToDoID"].ToString();
                                    info.SUBJ = dtr["Title"].ToString();        //상중하?완료여부?
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd 00:00");   //??

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("TodoID", dtr["ToDoID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/Pim/ToDO/ViewPopup.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "PIM-S":
                        #region 일정관리-일정

                        iteminfo.TITLE = this.GetResource("TodoItem06");        //"일정관리-일정";
                        iteminfo.COUNT = dal.GetPimScheduleListCnt(companyID, userID, deptID, tmpday).ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetPimScheduleList(companyID, userID, deptID, tmpday, pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ScheduleID"].ToString();
                                    info.SUBJ = dtr["Title"].ToString();        //상중하?완료여부?
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd 00:00");   //??

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("DisplayID", dtr["DisplayID"].ToString());
                                    paraDic.Add("ScheduleID", dtr["ScheduleID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/Pim/Schedule/ViewPopup.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "RM-C":
                        #region 자원관리-예약현황

                        iteminfo.TITLE = this.GetResource("TodoItem07");        //"자원관리-예약현황";
                        iteminfo.COUNT = dal.GetRMUsableListCnt(companyID, userID, "").ToString();      // 0 or 1
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetRMUsableList(companyID, userID, "", pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ListID"].ToString();     //자원명,제목??
                                    info.SUBJ = dtr["Subject"].ToString();
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteDate"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("tagGubun", "main");
                                    paraDic.Add("reservationID", dtr["ReservationID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    case "RM-W":
                        #region 자원관리자-승인대기

                        iteminfo.TITLE = this.GetResource("TodoItem08");        //"자원관리자-승인대기";
                        iteminfo.COUNT = dal.GetRMMgrUsable0ListCnt(companyID, userID).ToString();
                        iteminfo.URL = "";

                        list = new List<ListInfo>();
                        dtr = dal.GetRMMgrUsable0List(companyID, userID, pagesize);
                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    ListInfo info = new ListInfo();
                                    info.SECTION = menu;
                                    info.CID = dtr["CompanyID"].ToString();
                                    info.ID = dtr["ListID"].ToString();     //자원명,제목??
                                    info.SUBJ = dtr["Subject"].ToString();
                                    info.URL = "";
                                    info.NAME = GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(), dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);
                                    info.DATE = DateTime.Parse(dtr["WriteDate"].ToString()).ToString("yyyy-MM-dd 00:00");

                                    paraDic = new Dictionary<string, string>();
                                    paraDic.Add("tagGubun", "main");
                                    paraDic.Add("reservationID", dtr["ReservationID"].ToString());

                                    string encParams = GetEncryptParam(Utility.GetParameterDic(paraDic));
                                    info.URL = "/DeskPlusEIP/RM/Content/ViewPopup.aspx?p=" + encParams;

                                    list.Add(info);
                                }
                            }
                        }

                        #endregion
                        break;

                    default:
                        iteminfo.TITLE = "";
                        iteminfo.COUNT = "0";
                        iteminfo.URL = "";
                        list = new List<ListInfo>();
                        break;
                }

                iteminfo.ItemList = list;

                return iteminfo;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return null;
            }
        }
    }

    [Serializable]
    public class ItemInfo
    {
        public string CLIENTID;     //clientid
        public string MENU;         //호출메뉴
        public string TITLE;        //타이틀
        public string URL;          //연결URL
        public string COUNT;        //건수
        public List<ListInfo> ItemList { get; set; }        //리스트
    }

    [Serializable]
    public class ListInfo
    {
        public string SECTION;
        public string CID;
        public string ID;
        public string SUBJ;
        public string URL;
        public string NAME;
        public string DATE;
        public string ETC1;
        public string ETC2;
        public string ETC3;
        public string ETC4;
        public string ETC5;
    }
}
