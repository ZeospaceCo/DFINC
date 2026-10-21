using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Text.RegularExpressions;
using System.Data;
using System.Collections.Specialized;
using System.Text;
using System.Collections;
using System.IO;


namespace Zeospace.DeskPlus.RM.Web.Ws.Home
{
    /// <summary>
    /// Main의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Main : Base.BaseWSRM
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-21
        /// 작성목적 : 전체 예약 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMyList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Home.Main).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                DAL.Reverse dal = new DAL.Reverse();
                DataTableReader dtr = dal.GetMyList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ReserveInfo info = new BE.ReserveInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.Subject = dtr["Subject"].ToString();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();                              

                            info.ReserveDate = Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.ReserveDate += " ~ " + Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            if (dtr["Usable"].ToString() == "0")
                            {                                
                                info.Usable = "<div class=\"rdgb\">";
                                info.UsableStr = resMng.GetString("Usable0");
                            }
                            else if (dtr["Usable"].ToString() == "1")
                            {
                                info.Usable = "<div class=\"okgb\">";
                                info.UsableStr = resMng.GetString("Usable1");
                            }
                            else
                            {
                                info.Usable = "<div class=\"rjgb\">";
                                info.UsableStr = resMng.GetString("Usable2");
                            }

                            info.ManagerUserID = dtr["ManagerUserID"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = this.GetLangName(
                                   dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                   dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                   dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                   dtr["NameZhtw"].ToString().ToNull(), PreLang);

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
        /// 작성일자 : 2016-03-21
        /// 작성목적 : 전체 예약 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetMainList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Home.Main).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string listGubun = GetDecryptParam("listGubun", "LISTMY");

                DAL.Reverse dal = new DAL.Reverse();
                DataTableReader dtr = dal.GetMainList(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, CurrentUserInfo.DeptID, listGubun, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ReserveInfo info = new BE.ReserveInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.Subject = dtr["Subject"].ToString();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();

                            info.ReserveDate = Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.ReserveDate += " ~ " + Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            if (dtr["Usable"].ToString() == "0")
                            {
                                info.Usable = "<div class=\"rdgb\">";
                                info.UsableStr = resMng.GetString("Usable0");
                            }
                            else if (dtr["Usable"].ToString() == "1")
                            {
                                info.Usable = "<div class=\"okgb\">";
                                info.UsableStr = resMng.GetString("Usable1");
                            }
                            else
                            {
                                info.Usable = "<div class=\"rjgb\">";
                                info.UsableStr = resMng.GetString("Usable2");
                            }

                            info.ManagerUserID = dtr["ManagerUserID"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = this.GetLangName(
                                   dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                   dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                   dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                   dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.Section = dtr["Section"].ToString().ToNull();

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

        // <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-16
        /// 작성목적 : 부서 예약 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetDeptList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Home.Main).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");

                DAL.Reverse dal = new DAL.Reverse();
                DataTableReader dtr = dal.GetDeptList(CurrentUserInfo.CompanyID, CurrentUserInfo.DeptID, pageSize, pageSizePageNo);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ReserveInfo info = new BE.ReserveInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ReservationID = dtr["ReservationID"].ToString().ToNumber<int>();
                            info.ParentReservationID = dtr["ParentReservationID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.Subject = dtr["Subject"].ToString();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();
                            info.ReplyCount = dtr["ReplyCount"].ToString().ToNumber<int>();  

                            info.ReserveDate = Convert.ToDateTime(dtr["StartDate"].ToString()).ToString("yyyy-MM-dd HH:mm");
                            info.ReserveDate += " ~ " + Convert.ToDateTime(dtr["EndDate"].ToString()).ToString("yyyy-MM-dd HH:mm");

                            if (dtr["Usable"].ToString() == "0")
                            {
                                info.Usable = "<div class=\"rdgb\">";
                                info.UsableStr = resMng.GetString("Usable0");
                            }
                            else if (dtr["Usable"].ToString() == "1")
                            {
                                info.Usable = "<div class=\"okgb\">";
                                info.UsableStr = resMng.GetString("Usable1");
                            }
                            else
                            {
                                info.Usable = "<div class=\"rjgb\">";
                                info.UsableStr = resMng.GetString("Usable2");
                            }

                            info.ManagerUserID = dtr["ManagerUserID"].ToString().ToNumber<int>();
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.WriteUserName = this.GetLangName(
                                   dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                   dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                   dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                   dtr["NameZhtw"].ToString().ToNull(), PreLang);

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
        /// 작성일자 : 2016-03-18
        /// 작성목적 : 자원을 사용할 수 있는 지 확인
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool UseResource(string p)
        {
            try
            {
                bool rtnVal = true;

                SetDecryptString(p);
                string listID = GetDecryptParam("listID", "0");
                int usableQty = GetDecryptParam("usableQty", "0").ToNumber<int>();
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string sTime = GetDecryptParam("sTime", "");
                string eTime = GetDecryptParam("eTime", "");

                DAL.Reverse dal = new DAL.Reverse();

                string startDateStr = DateHelpers.GetCultureDate(startDate.Split(',')[0] + " " + sTime, "yyyyMMddHHmm", PreLangReal);
                string endDateStr = DateHelpers.GetCultureDate(endDate.Split(',')[0] + " " + eTime, "yyyyMMddHHmm", PreLangReal);

                DataSet ds = dal.GetUseResource(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID);
                
                int totReservationQty = 0;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    totReservationQty += dr["Qty"].ToString().ToNumber<int>();

                    if (totReservationQty > 0)
                    {
                        int balanceQty = (usableQty - totReservationQty);

                        if (balanceQty < 1)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (usableQty < 1)
                        {
                            return false;
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
        /// 작성일자 : 2016-03-21
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            SetDecryptString(p);

            int reservationID = GetDecryptParam("reservationID", "0").ToNumber<int>();

            string response = "0";

            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttach(CurrentUserInfo.CompanyID, 0, reservationID, "0");

            string totalFileInfo = "";
            string encFilePath = "";

            foreach (var _attachInfo in attachInfo)
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


        #endregion 

        #region MOD()
        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2017-01-20
        /// 작성목적 : 자원관리 탭 설정 저장하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModRMTabMenu(string p)
        {
            bool rtn = true;

            try
            {
                SetDecryptString(p);
                string tabType = GetDecryptParam("tabType", "day");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModRMTabMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tabType);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRMTabMenu: ", ex);
                rtn = false;
            }

            return rtn;
        }

        /// <summary>
        /// 작성자 : maniaysw
        /// 작성일자 : 2017-01-20
        /// 작성목적 : 자원관리 Type 설정 저장하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModRMTypeMenu(string p)
        {
            bool rtn = true;
            
            try
            {
                SetDecryptString(p);
                string type = GetDecryptParam("type", "1");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModRMTypeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, type);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRMTabMenu: ", ex);
                rtn = false;
            }

            return rtn;
        }

        #endregion

        #region ADD
        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-03-18
        /// 작성목적 : 자원관리 홈메뉴 저장
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddRMHomeMenu(string p)
        {
            try
            {
                SetDecryptString(p);
                string listGubun = GetDecryptParam("listGubun", "LISTMY");                                                    

                // 기본탭
                Account.DAL.UserEnvironment envdal = new Account.DAL.UserEnvironment();
                envdal.RMHomeMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, listGubun); 

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }
        #endregion
    }
}
