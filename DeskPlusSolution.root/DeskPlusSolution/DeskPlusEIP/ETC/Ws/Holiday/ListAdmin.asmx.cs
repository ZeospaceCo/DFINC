using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.ETC.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;
using Zeospace.DeskPlus.Component.Excel;
using Zeospace.DeskPlus.Account.DAL;

namespace Zeospace.DeskPlus.ETC.Web.Ws.Holiday
{
    /// <summary>
    /// HoliAdminList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ListAdmin : Base.BaseWSETC
    {
        public string SMonth = string.Empty; 
        public string EMonth = string.Empty; 

        #region 리스트

        /// <summary>
        /// 페이징 사이즈 및 카운트 구하기
        /// </summary>
        /// <param name="_totalCnt">The _total CNT.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageNo">The _page no.</param>
        /// <returns></returns>
        private string SetPaging(int _totalCnt, int _pageSize, int _pageNo)
        {
            //페이지 총 개수

            int recsInLastPage = _totalCnt % _pageSize;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
                recsInLastPage = _pageSize;

            int sizeOfPage = _pageSize;
            int pageIndex = _pageNo - 1;
            int pageCount = Convert.ToInt32(((_totalCnt - 1) / sizeOfPage) + 1);


            //넘어온 페이지값과 총페이지값이 동일할 경우(마지막 페이지일경우)
            if (pageIndex + 1 == pageCount)
                sizeOfPage = recsInLastPage;//나머지 개수만큼 출력

            //페이지번호*페이지사이즈
            int pageNoPageSize = _pageSize * pageIndex;

            return sizeOfPage.ToString() + "," + pageNoPageSize.ToString();
        }

        /// <summary>
        /// 작성자 : 김형석
        /// 리스트 가져오기
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
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string order = GetDecryptParam("order", "");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                DAL.HoliAdmin biz = new DAL.HoliAdmin();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID,
                    startDate, endDate,
                    setPageingStr[0], setPageingStr[1],
                    searchField, searchValue, order);


                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.HoliAdminInfo info = new BE.HoliAdminInfo();

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.ROW = listNum.ToString();
                            info.UserID = dtr["UserID"].ToString().ToNumber<int>();

                            #region 이름 다국어 설정
                            info.WriteUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyName = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.All1 = dtr["All1"].ToString();
                            info.Used1 = dtr["Used1"].ToString();
                            info.Jan1 = (Convert.ToDouble(info.All1) - Convert.ToDouble(info.Used1)).ToString();

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
        /// Gets the list count.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                int icnt = 0;

                DAL.HoliAdmin biz = new DAL.HoliAdmin();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID, searchField, searchValue);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 김형석
        /// 사용자, 년도별 전체 리스트(관리자) 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAllHoliAdminUserList(string p)
        {
            try
            {
                SetDecryptString(p);
                string totalCount = GetDecryptParam("totalCount", "0");
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");

                DAL.HoliAdmin biz = new DAL.HoliAdmin();

                DataTableReader dtr = biz.GetAllHoliAdminUserList(CurrentUserInfo.CompanyID, startDate, endDate, searchField, searchValue);

                ObjList listobj = new ObjList();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.ETC.BE.HoliAdminInfo info = new BE.HoliAdminInfo();

                            int listNum = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;

                            info.ROW = listNum.ToString();

                            #region 이름 다국어 설정
                            info.WriteUserName = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 부서 다국어 설정
                            info.DeptName = this.GetLangName(
                                    dtr["DeptBase"].ToString().ToNull(), dtr["DeptKo"].ToString().ToNull(),
                                    dtr["DeptEn"].ToString().ToNull(), dtr["DeptJa"].ToString().ToNull(),
                                    dtr["DeptFr"].ToString().ToNull(), dtr["DeptZhcn"].ToString().ToNull(),
                                    dtr["DeptZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직급 다국어 설정
                            info.TitleName = this.GetLangName(
                                    dtr["TitleBase"].ToString().ToNull(), dtr["TitleKo"].ToString().ToNull(),
                                    dtr["TitleEn"].ToString().ToNull(), dtr["TitleJa"].ToString().ToNull(),
                                    dtr["TitleFr"].ToString().ToNull(), dtr["TitleZhcn"].ToString().ToNull(),
                                    dtr["TitleZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            #region 직책 다국어 설정
                            info.DutyName = this.GetLangName(
                                    dtr["DutyBase"].ToString().ToNull(), dtr["DutyKo"].ToString().ToNull(),
                                    dtr["DutyEn"].ToString().ToNull(), dtr["DutyJa"].ToString().ToNull(),
                                    dtr["DutyFr"].ToString().ToNull(), dtr["DutyZhcn"].ToString().ToNull(),
                                    dtr["DutyZhtw"].ToString().ToNull(), PreLang);
                            #endregion

                            info.All1 = dtr["All1"].ToString();
                            info.Used1 = dtr["Used1"].ToString();
                            info.Jan1 = (Convert.ToDouble(info.All1) - Convert.ToDouble(info.Used1)).ToString();

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

        #endregion
        #region ADD()

        /// <summary>
        /// 작성자 : ellepooky
        /// 작성일자 : 2018-11-26
        /// 파일 업로드 : 엑셀파일 업로드
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>

        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string SetExcelUpload(string p)
        {
            try
            {
                SetDecryptString(p);
                string swfUploadGUID = GetDecryptParam("swfUploadGUID", "");

                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                string filePath = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/";
                string spoolPath = string.Format(filePath, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, swfUploadGUID);

                string yeardate = DateTime.Now.Year.ToString();
                string tmpPath = baseFileDirectory + "{0}/Etc/Holiday/{1}/";
                string holiPath = string.Format(tmpPath, CurrentUserInfo.CompanyID, yeardate);
                string fileGUID = "";

                List<BE.HoliInfo> listobj = new List<BE.HoliInfo>();
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(spoolPath));

                if (di.Exists)
                {
                    FileInfo[] sortList = di.GetFiles();
                    Array.Sort(sortList,
                        new Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.FileListSort());

                    foreach (FileInfo f in sortList)
                    {
                        BE.HoliInfo info = new BE.HoliInfo();

                        fileGUID = System.Guid.NewGuid().ToString() + f.Extension;
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.FileGUID = fileGUID;
                        info.FilePath = holiPath;
                        info.FileName = f.Name;
                        info.FileType = f.Extension;
                        info.FileLength = f.Length;

                        listobj.Add(info);
                    }
                }
                BLL.HoliAdminFile bll = new BLL.HoliAdminFile();

                if(bll.Add(listobj, spoolPath, holiPath))
                {
                    DataSet returnDataSet = new DataSet();
                    string[] sheetName = { "Sheet1" };

                    using (OleExcelReader reader = new OleExcelReader())
                    {
                        reader.Filename = Server.MapPath(holiPath + "/" + fileGUID); ;
                        reader.HasHeaders = true;
                        reader.KeepConnectionOpen = false;

                        string[] sheetNames = reader.GetExcelSheetNames();
                        if (sheetNames.Length < 1)
                        {
                            return "-1";
                        }
                        else
                        {            

                            reader.SheetName = sheetName;
                            returnDataSet = reader.GetDataSet();
                            DataTable dtExcelData = returnDataSet.Tables[0];

                            string date = AppSettingHelper.GetAppSetting(AppSettingType.ETC, "HolidayDateType", "");

                            if (date.Split(',').Length == 4)
                            {
                                SMonth = (date.Split(',')[1].ToNumber<int>() < 10) ? "0" + date.Split(',')[1] : date.Split(',')[1];
                                EMonth = (date.Split(',')[3].ToNumber<int>() < 10) ? "0" + date.Split(',')[3] : date.Split(',')[3];
                            }
                            else
                            {
                                SMonth = "01";
                                EMonth = "12";
                            }

                            for (int i = 0; i < dtExcelData.Rows.Count; i++)
                            {
                                BE.HoliAdminInfo be = new HoliAdminInfo();

                                be.CompanyID = CurrentUserInfo.CompanyID;
                                be.LoginID = dtExcelData.Rows[i][1].ToString().Trim();
                                be.StartDate = dtExcelData.Rows[i][0].ToString().Trim() + SMonth;
                                be.EndDate = dtExcelData.Rows[i][0].ToString().Trim() + EMonth;
                                be.Type = "1";
                                be.HoliCount = Convert.ToDouble(dtExcelData.Rows[i][3].ToString().Trim());
                                //be.HoliCount = dtExcelData.Rows[i][3].ToString().Trim().ToNumber<int>();

                                int userID = bll.GetUserID(be.LoginID);
                                DAL.HoliAdmin dal = new DAL.HoliAdmin();
                                dal.SetHoliReceive(be.CompanyID, be.StartDate, be.EndDate, userID, be.HoliCount, be.Type, CurrentUserInfo.UserID, CurrentUserInfo.Name);
                            }
                        }

                    }

                }


                return "1";
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return "-1";
            }
        }

        #endregion

        #region MOD()

        /// <summary>
        /// 용도 : 사용자별 페이지사이즈 설정하기
        /// 작성자 : 김형석
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SavePageSize(string p)
        {
            try
            {
                SetDecryptString(p);

                string folderID = GetDecryptParam("folderID", "");
                string pageSize = GetDecryptParam("pageSize", "0");
                string types = GetDecryptParam("types", "Admin");

                BE.FolderUserInfo info = new FolderUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();
                info.Types = types;

                DAL.FolderUser dal = new DAL.FolderUser();
                dal.AddMod(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 작성자 : 김형석
        /// 휴가일 수 저장하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetHoliReceive(string p)
        {
            try
            {
                SetDecryptString(p);

                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");
                string userID = GetDecryptParam("userID", "");
                string holiReceive1 = GetDecryptParam("holiReceive1", "0");

                string[] arrUserID = userID.Split(',');
                string[] arrHoli1 = holiReceive1.Split(',');

                DAL.HoliAdmin biz = new DAL.HoliAdmin();

                for (int i = 0; i < arrUserID.Length; i++)
                {
                    //연차                    
                    biz.SetHoliReceive(CurrentUserInfo.CompanyID, startDate, endDate, arrUserID[i].ToNumber<int>(), Convert.ToDouble(arrHoli1[i]), "1", CurrentUserInfo.UserID, CurrentUserInfo.Name);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion

        #region DEL()

        #endregion
    }
}
