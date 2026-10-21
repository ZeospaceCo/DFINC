using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;


namespace Zeospace.DeskPlus.Visit.Web.Ws.Content
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSVisit
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 첨부 파일가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int reserveGroupID = GetDecryptParam("reserveGroupID", "0").ToNumber<int>();

                DAL.Attach dal = new DAL.Attach();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, reserveGroupID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new BE.AttachInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
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
                            info.FileDel = resMng.GetString("Del");

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 기본 공유자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetShareList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

                DAL.Share dal = new DAL.Share();

                DataTableReader dtr = dal.GetShare(CurrentUserInfo.CompanyID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ShareInfo info = new BE.ShareInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.Title = this.GetLangName(
                                       dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                       dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                       dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                       dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            if (dtr["Type"].ToString() == "M")
                            {
                                info.Gubun = resMng.GetString("ShareType2");
                            }
                            else
                            {
                                info.Gubun = resMng.GetString("ShareType1");
                            }

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 공유자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetOpenList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Visit.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int reserveGroupID = GetDecryptParam("reserveGroupID", "0").ToNumber<int>();

                DAL.OpenList dal = new DAL.OpenList();

                DataTableReader dtr = dal.GetOpenList(CurrentUserInfo.CompanyID, reserveGroupID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.OpenListInfo info = new BE.OpenListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.Title = this.GetLangName(
                                       dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                       dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                       dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                       dtr["NameZhtw"].ToString().ToNull(), PreLang);

                            info.Type = dtr["Type"].ToString();
                            info.OwnerID = dtr["OwnerID"].ToString().ToNumber<int>();

                            if (dtr["Type"].ToString() == "M")
                            {
                                info.Gubun = resMng.GetString("ShareType2");
                            }
                            else
                            {
                                info.Gubun = resMng.GetString("ShareType1");
                            }

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
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 예약자원 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetRmList(string p)
        {
            try
            {
                SetDecryptString(p);
                int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

                DAL.RmList dal = new DAL.RmList();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, reserveID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.RmListInfo info = new BE.RmListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();
                            info.Qty = dtr["Qty"].ToString().ToNumber<int>();
                            info.UsableQty = dtr["UsableQty"].ToString().ToNumber<int>();
                            info.Authority = dtr["Authority"].ToString();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();

                            string[] arrManyName = dtr["MManyName"].ToString().Split('|');
                            if (arrManyName.Length == 7)
                            {
                                info.ManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                       arrManyName[2], arrManyName[3], arrManyName[4],
                                                                       arrManyName[5], arrManyName[6], PreLang);
                            }

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
        /// 작성일자 : 2012-03-12
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
                string qty = GetDecryptParam("qty", "");
                string usableQty = GetDecryptParam("usableQty", "");
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string sTime = GetDecryptParam("sTime", "");
                string eTime = GetDecryptParam("eTime", "");
                string repeat = GetDecryptParam("repeat", "");
                int reserveID = GetDecryptParam("reserveID", "").ToNumber<int>();
                int reserveGroupID = GetDecryptParam("reserveGroupID", "").ToNumber<int>();
                string repeatChk = GetDecryptParam("repeatChk", "");
                string repeatSel = GetDecryptParam("repeatSel", "");
                string beforeStartDate = GetDecryptParam("beforeStartDate", "");

                listID = listID.TrimEnd(',');
                qty = qty.TrimEnd('$');
                usableQty = usableQty.TrimEnd('$');
                repeat = repeat.TrimEnd('$');

                string[] arrListID = listID.Split(',');
                string[] arrQty = qty.Split('$');
                string[] arrusableQty = usableQty.Split('$');
                string[] arrRepeat = repeat.Split('$');

                Zeospace.DeskPlus.RM.DAL.Reverse dal = new Zeospace.DeskPlus.RM.DAL.Reverse();

                if (repeat != "")
                {
                    #region  반복 설정 O
                    for (int i = 0; i < arrRepeat.Length; i++)
                    {
                        string startDateStr = DateHelper.GetCultureDate(arrRepeat[i] + " " + sTime, "yyyyMMddHHmm", PreLangReal);
                        string endDateStr = DateHelper.GetCultureDate(arrRepeat[i] + " " + eTime, "yyyyMMddHHmm", PreLangReal);

                        DataSet ds = new DataSet();

                        if (repeatSel == "1")
                        {
                            ds = dal.GetUseVisit1(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                        }
                        else if (repeatSel == "2")
                        {                            
                            DateTime dt = Convert.ToDateTime(startDate);
                            int tmpStartDate = (dt.Year.ToString() + dt.Month.ToString().PadLeft(2, '0') + dt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                            int tmpBeforeDate = beforeStartDate.ToNumber<int>();

                            tmpStartDate = (tmpStartDate > tmpBeforeDate) ? tmpBeforeDate : tmpStartDate;
                                                     
                            DateTime dtt = Convert.ToDateTime(arrRepeat[i]);
                            int tmpDate = (dtt.Year.ToString() + dtt.Month.ToString().PadLeft(2, '0') + dtt.Day.ToString().PadLeft(2, '0')).ToNumber<int>();

                            if (tmpStartDate > tmpDate)
                            {
                                ds = dal.GetUseVisit1(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                            }
                            else
                            {
                                ds = dal.GetUseVisit2(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                            }
                        }
                        else if (repeatSel == "3")
                        {
                            ds = dal.GetUseVisit2(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                        }
                        else
                        {
                            ds = dal.GetUseVisit1(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                        }

                        for (int j = 0; j < arrListID.Length; j++)
                        {
                            int totReservationQty = 0;

                            DataRow[] dbRow = ds.Tables[0].Select("ListID =" + arrListID[j]);
                            foreach (DataRow dr in dbRow)
                            {
                                totReservationQty += dr["Qty"].ToString().ToNumber<int>();

                                if (totReservationQty > 0)
                                {
                                    // 자원 예약을 한개 라도 있을 경우
                                    // 자원 예약 가능 갯수에서 예약한 갯수를 빼면 예약 가능한 갯수가 나옴
                                    int balanceQty = (arrusableQty[j].ToString().ToNumber<int>() - totReservationQty);

                                    // 예약 가능 갯수가 예약 신청한 갯수보다 작다면 예약 불가
                                    if (balanceQty < arrQty[j].ToString().ToNumber<int>())
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    // 자원 예약 갯수가 0
                                    // 자원 예약 가능 갯수가 예약 신청한 갯수보다 작다면 예약 불가
                                    if (arrusableQty[j].ToString().ToNumber<int>() < arrQty[j].ToString().ToNumber<int>())
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region 반복 설정 X

                    string startDateStr = DateHelper.GetCultureDate(startDate + " " + sTime, "yyyyMMddHHmm", PreLangReal);
                    string endDateStr = DateHelper.GetCultureDate(endDate + " " + eTime, "yyyyMMddHHmm", PreLangReal);

                    DataSet ds = new DataSet();

                    if (repeatChk != "")
                    {
                        if (repeatSel == "1")
                        {
                            ds = dal.GetUseVisit1(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                        }
                        else
                        {
                            ds = dal.GetUseVisit2(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                        }
                    }
                    else
                    {
                        ds = dal.GetUseVisit1(CurrentUserInfo.CompanyID, startDateStr, endDateStr, listID, reserveID);
                    }

                    for (int j = 0; j < arrListID.Length; j++)
                    {
                        int totReservationQty = 0;

                        DataRow[] dbRow = ds.Tables[0].Select("ListID =" + arrListID[j]);
                        foreach (DataRow dr in dbRow)
                        {
                            totReservationQty += dr["Qty"].ToString().ToNumber<int>();

                            if (totReservationQty > 0)
                            {
                                int balanceQty = (arrusableQty[j].ToString().ToNumber<int>() - totReservationQty);

                                if (balanceQty < arrQty[j].ToString().ToNumber<int>())
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (arrusableQty[j].ToString().ToNumber<int>() < arrQty[j].ToString().ToNumber<int>())
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    #endregion
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
        /// 작성일자 : 2014-02-13
        /// 작성목적 : 반복 일자 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetRepeat(string p)
        {
            try
            {
                SetDecryptString(p);
                string repeatType = GetDecryptParam("repeatType", "");
                string selectType = GetDecryptParam("selectType", "");
                int intervalCnt = GetDecryptParam("intervalCnt", "0").ToNumber<int>();
                string option1 = GetDecryptParam("option1", "");
                string option2 = GetDecryptParam("option2", "");
                string option3 = GetDecryptParam("option3", "");
                string option4 = GetDecryptParam("option4", "");
                string option5 = GetDecryptParam("option5", "");
                string option6 = GetDecryptParam("option6", "");
                string option7 = GetDecryptParam("option7", "");
                int maxReptCnt = GetDecryptParam("maxReptCnt", "0").ToNumber<int>();
                string endDate = GetDecryptParam("endDate", "");
                string startDate = GetDecryptParam("startDate", "");
                string repeatChk = GetDecryptParam("repeatChk", "");
                string repeatSel = GetDecryptParam("repeatSel", "");
                string groupStartDate = GetDecryptParam("groupStartDate", "");

                string rtnVal = String.Empty;

                if ((repeatSel == "3") && (repeatChk == "Y"))
                {
                    //int tmpStartDate = startDate.Replace("-", "").ToNumber<int>();
                    //int tmpGroupDate = groupStartDate.Replace("-", "").ToNumber<int>();
                    DateTime dtStart = Convert.ToDateTime(startDate);
                    DateTime dtGroup = Convert.ToDateTime(groupStartDate);
                    int tmpStartDate = (dtStart.Year.ToString() + dtStart.Month.ToString().PadLeft(2, '0') + dtStart.Day.ToString().PadLeft(2, '0')).ToNumber<int>();
                    int tmpGroupDate = (dtGroup.Year.ToString() + dtGroup.Month.ToString().PadLeft(2, '0') + dtGroup.Day.ToString().PadLeft(2, '0')).ToNumber<int>();


                    startDate = (tmpStartDate > tmpGroupDate) ? groupStartDate : startDate;
                }

                if ((repeatType == "D") && (selectType == "1"))
                {
                    // 매일 반복 (주기 입력)   
                    //유형1 [매 X일 마다] 이면 입력한 값으로 주기값
                    DateTime dtCalcRangeTime = Convert.ToDateTime(startDate);

                    if (maxReptCnt == 0)
                    {
                        DateTime dtToTime = Convert.ToDateTime(endDate);

                        while (dtCalcRangeTime <= dtToTime)
                        {
                            rtnVal += dtCalcRangeTime.ToShortDateString() + "$";
                            dtCalcRangeTime = dtCalcRangeTime.AddDays(intervalCnt);
                        }
                    }
                    else
                    {
                        for (int i = 0; maxReptCnt > i; i++)
                        {
                            rtnVal += dtCalcRangeTime.ToShortDateString() + "$";
                            dtCalcRangeTime = dtCalcRangeTime.AddDays(intervalCnt);
                        }
                    }
                }
                else if ((repeatType == "D") && (selectType == "2"))
                {
                    //매일반복 (평일)
                    DateTime dtCalcRangeTime = Convert.ToDateTime(startDate);
                    if (maxReptCnt == 0)
                    {
                        DateTime dtToTime = Convert.ToDateTime(endDate);

                        while (dtCalcRangeTime <= dtToTime)
                        {
                            if (Convert.ToInt32(dtCalcRangeTime.DayOfWeek) != 0 && Convert.ToInt32(dtCalcRangeTime.DayOfWeek) != 6)
                            {
                                rtnVal += dtCalcRangeTime.ToShortDateString() + "$";
                            }

                            dtCalcRangeTime = dtCalcRangeTime.AddDays(1);
                        }
                    }
                    else
                    {
                        for (int i = 0; maxReptCnt > i; i++)
                        {
                            if (Convert.ToInt32(dtCalcRangeTime.DayOfWeek) == 6)
                            {
                                dtCalcRangeTime = dtCalcRangeTime.AddDays(2);
                            }

                            if (Convert.ToInt32(dtCalcRangeTime.DayOfWeek) == 0)
                            {
                                dtCalcRangeTime = dtCalcRangeTime.AddDays(1);
                            }

                            rtnVal += dtCalcRangeTime.ToShortDateString() + "$";
                            dtCalcRangeTime = dtCalcRangeTime.AddDays(1);
                        }
                    }
                }
                else if (repeatType == "W")
                {
                    //매주반복
                    DateTime dtCalcRangeTime = Convert.ToDateTime(startDate);
                    if (maxReptCnt == 0)
                    {
                        DateTime dtToTime = Convert.ToDateTime(endDate);
                        DateTime SomeDayInWeek;

                        int offsetFromSunday = Convert.ToInt32(dtCalcRangeTime.DayOfWeek);

                        DateTime intervalStartDay = dtCalcRangeTime.AddDays(-(offsetFromSunday)); // 그주의 일요일로 변경

                        while (intervalStartDay <= dtToTime)
                        {
                            SomeDayInWeek = intervalStartDay;

                            if ((option1 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option2 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option3 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option4 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option5 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option6 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option7 == "1") && (SomeDayInWeek >= dtCalcRangeTime))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                if (SomeDayInWeek >= dtToTime) break;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            intervalStartDay = intervalStartDay.AddDays(intervalCnt * 7);
                        }
                    }
                    else
                    {
                        DateTime SomeDayInWeek;

                        DateTime dtFromTime = Convert.ToDateTime(startDate);
                        int offsetFromSunday = Convert.ToInt32(dtCalcRangeTime.DayOfWeek);

                        for (int j = 0; maxReptCnt > j; )
                        {
                            SomeDayInWeek = dtCalcRangeTime.AddDays(-(offsetFromSunday)); // 그주의 일요일로 변경

                            if ((option1 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option2 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option3 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option4 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option5 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option6 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            if ((option7 == "1") && (SomeDayInWeek >= dtFromTime && maxReptCnt > j))
                            {
                                rtnVal += SomeDayInWeek.ToShortDateString() + "$";
                                j++;
                            }

                            SomeDayInWeek = SomeDayInWeek.AddDays(1);

                            dtCalcRangeTime = dtCalcRangeTime.AddDays(intervalCnt * 7);
                        }
                    }
                }
                else if ((repeatType == "M") && (selectType == "1"))
                {
                    //매월 반복  : 일자선택
                    DateTime dtCalcRangeTime = Convert.ToDateTime(startDate);
                    if (maxReptCnt == 0)
                    {
                        DateTime dtFromTime = Convert.ToDateTime(startDate);
                        DateTime dtToTime = Convert.ToDateTime(endDate);
                        DateTime theDayEveryMonth;

                        int everyThisDay = Convert.ToInt32(option1);

                        while (dtCalcRangeTime <= dtToTime)
                        {
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, everyThisDay);

                            if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                            {
                                rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                            }

                            dtCalcRangeTime = dtCalcRangeTime.AddMonths(intervalCnt);
                        }
                    }
                    else
                    {
                        DateTime dtFromTime = Convert.ToDateTime(startDate);
                        DateTime theDayEveryMonth;

                        int everyThisDay = Convert.ToInt32(option1);

                        for (int i = 0; maxReptCnt > i; )
                        {
                            theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, everyThisDay);

                            if (theDayEveryMonth >= dtFromTime)
                            {
                                rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                                i++;
                            }

                            dtCalcRangeTime = dtCalcRangeTime.AddMonths(intervalCnt);
                        }
                    }
                }
                else if ((repeatType == "M") && (selectType == "2"))
                {
                    //매월 반복  : 첫째.둘째..
                    if (maxReptCnt == 0)
                    {
                        DateTime dtFromTime = Convert.ToDateTime(startDate);
                        DateTime dtToTime = Convert.ToDateTime(endDate);
                        DateTime dtCalcRangeTime = new DateTime(dtFromTime.Year, dtFromTime.Month, 1);
                        DateTime theDayEveryMonth;
                        DayOfWeek SelectedDayOfWeek = DayOfWeek.Sunday;

                        int lorder = option1.ToNumber<int>();

                        while (dtCalcRangeTime <= dtToTime)
                        {
                            if (lorder >= 1 && lorder <= 4)
                            {
                                // 첫번째~네번째
                                switch (option2)
                                {
                                    case "1":	// "날"
                                        theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, lorder);
                                        break;
                                    case "2":	// "평일"
                                        theDayEveryMonth = getOrdinalWorkDay(lorder, dtCalcRangeTime);
                                        break;
                                    case "3":	// "주말"
                                        theDayEveryMonth = getOrdinalWeekEnd(lorder, dtCalcRangeTime);
                                        break;
                                    default:
                                        switch (option2)
                                        {
                                            case "4": SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                            case "5": SelectedDayOfWeek = DayOfWeek.Monday; break;
                                            case "6": SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                            case "7": SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                            case "8": SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                            case "9": SelectedDayOfWeek = DayOfWeek.Friday; break;
                                            case "10": SelectedDayOfWeek = DayOfWeek.Saturday; break;
                                        };
                                        theDayEveryMonth = getOrdinalDayOfWeekDay(lorder, SelectedDayOfWeek, dtCalcRangeTime);
                                        break;

                                }

                                if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                                {
                                    rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                                }
                            }
                            else
                            {
                                // 마지막째
                                switch (option2)
                                {
                                    case "1":	// "날"
                                        theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, System.DateTime.DaysInMonth(dtCalcRangeTime.Year, dtCalcRangeTime.Month));
                                        break;
                                    case "2":
                                        theDayEveryMonth = getLastWorkDay(dtCalcRangeTime);
                                        break;
                                    case "3":
                                        theDayEveryMonth = getLastWeekEnd(dtCalcRangeTime);
                                        break;
                                    default:
                                        switch (option2)
                                        {
                                            case "4": SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                            case "5": SelectedDayOfWeek = DayOfWeek.Monday; break;
                                            case "6": SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                            case "7": SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                            case "8": SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                            case "9": SelectedDayOfWeek = DayOfWeek.Friday; break;
                                            case "10": SelectedDayOfWeek = DayOfWeek.Saturday; break;
                                        };
                                        theDayEveryMonth = getLastDayOfWeekDay(SelectedDayOfWeek, dtCalcRangeTime);
                                        break;
                                }

                                if (theDayEveryMonth >= dtFromTime && theDayEveryMonth <= dtToTime)
                                {
                                    rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                                }
                            }

                            dtCalcRangeTime = dtCalcRangeTime.AddMonths(intervalCnt);
                        }
                    }
                    else
                    {
                        DateTime dtFromTime = Convert.ToDateTime(startDate);
                        DateTime dtCalcRangeTime = new DateTime(dtFromTime.Year, dtFromTime.Month, 1);
                        DateTime theDayEveryMonth;
                        DayOfWeek SelectedDayOfWeek = DayOfWeek.Sunday;

                        int lorder = option1.ToNumber<int>();

                        for (int i = 0; maxReptCnt > i; )
                        {
                            if (lorder >= 1 && lorder <= 4)
                            {
                                // 첫번째~네번째
                                switch (option2)
                                {
                                    case "1":	// "날"
                                        theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, lorder);
                                        break;
                                    case "2":	// "평일"
                                        theDayEveryMonth = getOrdinalWorkDay(lorder, dtCalcRangeTime);
                                        break;
                                    case "3":
                                        theDayEveryMonth = getOrdinalWeekEnd(lorder, dtCalcRangeTime);
                                        break;
                                    default:
                                        switch (option2)
                                        {
                                            case "4": SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                            case "5": SelectedDayOfWeek = DayOfWeek.Monday; break;
                                            case "6": SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                            case "7": SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                            case "8": SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                            case "9": SelectedDayOfWeek = DayOfWeek.Friday; break;
                                            case "10": SelectedDayOfWeek = DayOfWeek.Saturday; break;
                                        };
                                        theDayEveryMonth = getOrdinalDayOfWeekDay(lorder, SelectedDayOfWeek, dtCalcRangeTime);
                                        break;

                                }

                                if (theDayEveryMonth >= dtFromTime)
                                {
                                    rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                                    i++;
                                }
                            }
                            else
                            {
                                // 마지막째
                                switch (option2)
                                {
                                    case "1":	// "날"
                                        theDayEveryMonth = new DateTime(dtCalcRangeTime.Year, dtCalcRangeTime.Month, System.DateTime.DaysInMonth(dtCalcRangeTime.Year, dtCalcRangeTime.Month));
                                        break;
                                    case "2":
                                        theDayEveryMonth = getLastWorkDay(dtCalcRangeTime);
                                        break;
                                    case "3":
                                        theDayEveryMonth = getLastWeekEnd(dtCalcRangeTime);
                                        break;
                                    default:
                                        switch (option2)
                                        {
                                            case "4": SelectedDayOfWeek = DayOfWeek.Sunday; break;
                                            case "5": SelectedDayOfWeek = DayOfWeek.Monday; break;
                                            case "6": SelectedDayOfWeek = DayOfWeek.Tuesday; break;
                                            case "7": SelectedDayOfWeek = DayOfWeek.Wednesday; break;
                                            case "8": SelectedDayOfWeek = DayOfWeek.Thursday; break;
                                            case "9": SelectedDayOfWeek = DayOfWeek.Friday; break;
                                            case "10": SelectedDayOfWeek = DayOfWeek.Saturday; break;
                                        };
                                        theDayEveryMonth = getLastDayOfWeekDay(SelectedDayOfWeek, dtCalcRangeTime);
                                        break;
                                }

                                if (theDayEveryMonth >= dtFromTime)
                                {
                                    rtnVal += theDayEveryMonth.ToShortDateString() + "$";
                                    i++;
                                }
                            }

                            dtCalcRangeTime = dtCalcRangeTime.AddMonths(intervalCnt);
                        }
                    }
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 첫번째~네번째 평일 반환
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getOrdinalWorkDay(int _ordinal, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (Convert.ToInt32(i.DayOfWeek) != 0 && Convert.ToInt32(i.DayOfWeek) != 6)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }

        /// <summary>
        /// 첫번째~네번째 주말 반환
        /// 일요일부터 시작되는 달은 첫번재 일요일이 첫번째 주말이고 그 외의 경우는 토요일이 주말
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dt"></param>
        /// <returns></returns>
        private DateTime getOrdinalWeekEnd(int _ordinal, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (Convert.ToInt32(i.DayOfWeek) == 0 || Convert.ToInt32(i.DayOfWeek) == 6)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }

        /// <summary>
        /// 첫번째~네번째 X요일 반환
        /// </summary>
        /// <param name="_ordinal"></param>
        /// <param name="_dayOfWeek"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getOrdinalDayOfWeekDay(int _ordinal, DayOfWeek _dayOfWeek, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);
            int ordinalCount = 1;

            for (DateTime i = firstDateThisMonth; lastDateThisMonth >= i; i = i.AddDays(1))
            {
                if (i.DayOfWeek == _dayOfWeek)
                {
                    if (_ordinal == ordinalCount)
                    {
                        returnDate = i;
                        break;
                    }
                    ordinalCount++;
                }
            }

            return returnDate;
        }


        /// <summary>
        /// 마지막째 X요일 반환
        /// </summary>
        /// <param name="_dayOfWeek"></param>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getLastDayOfWeekDay(DayOfWeek _dayOfWeek, DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (i.DayOfWeek == _dayOfWeek)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }

        /// <summary>
        /// 마지막째 평일 반환
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns>DateTime</returns>
        private DateTime getLastWorkDay(DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (Convert.ToInt32(i.DayOfWeek) != 0 && Convert.ToInt32(i.DayOfWeek) != 6)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }

        /// <summary>
        /// 마지막 주말 반환
        /// 무조건 뒤에서 첫번재 토요일
        /// </summary>
        /// <param name="_dt"></param>
        /// <returns></returns>
        private DateTime getLastWeekEnd(DateTime _dt)
        {
            DateTime firstDateThisMonth = new DateTime(_dt.Year, _dt.Month, 1);
            DateTime lastDateThisMonth = new DateTime(_dt.Year, _dt.Month, System.DateTime.DaysInMonth(_dt.Year, _dt.Month));
            DateTime returnDate = new DateTime(1, 1, 1);

            for (DateTime i = lastDateThisMonth; firstDateThisMonth <= i; i = i.AddDays(-1))
            {
                if (Convert.ToInt32(i.DayOfWeek) == 6)
                {
                    returnDate = i;
                    break;
                }
            }

            return returnDate;
        }

        #endregion 

        #region MOD()
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModVisitTabMenu(string p)
        {
            bool rtn = true;

            try
            {
                SetDecryptString(p);
                string tabType = GetDecryptParam("tabType", "day");

                Account.DAL.UserEnvironment env = new Account.DAL.UserEnvironment();

                rtn = env.ModVisitTabMenu(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, tabType);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRMTabMenu: ", ex);
                rtn = false;
            }

            return rtn;
        }

        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-12
        /// 작성목적 : 첨부 파일 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);
            int reserveGroupID = GetDecryptParam("reserveGroupID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

            BLL.Attach bll = new BLL.Attach();

            bll.Del(CurrentUserInfo.CompanyID, reserveGroupID, attachFileID);

            return true;
        }

        #endregion 
    }
}
