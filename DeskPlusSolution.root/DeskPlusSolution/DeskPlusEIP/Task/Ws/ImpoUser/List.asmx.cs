using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Task.BE;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Task.Web.Ws.ImpoUser
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSTask
    {

        #region GET()

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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 중요표시 협업리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetPageList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Task.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Task.Web.Ws.ImpoUser.List).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);              
                string totalCount = GetDecryptParam("totalCount", "0");
                string pageSize = GetDecryptParam("pageSize", "0");
                string pageSizePageNo = GetDecryptParam("pageSizePageNo", "0");
                string searchStatus = GetDecryptParam("searchStatus", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchDateValue = GetDecryptParam("searchDateValue", "");
                string itemID = GetDecryptParam("itemID", "0");
                string itemSubID = GetDecryptParam("itemSubID", "0");
                string isGubun = GetDecryptParam("isGubun", "0");

                string rtnSetPaging = SetPaging(totalCount.ToNumber<int>(), pageSize.ToNumber<int>(), pageSizePageNo.ToNumber<int>());
                string[] setPageingStr = rtnSetPaging.Split(',');

                #region 날짜

                string startDate = "";
                string endDate = "";

                if (searchField == "RequestDate" || searchField == "StartDate" || searchField == "EndDate")
                {
                    startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion

                DAL.ImpoUser biz = new DAL.ImpoUser();

                DataTableReader dtr = biz.GetPageList(CurrentUserInfo.CompanyID, setPageingStr[0], setPageingStr[1],
                    startDate, endDate, searchStatus, searchField, searchValue, CurrentUserInfo.UserID, itemID.ToNumber<int>(), itemSubID.ToNumber<int>(), isGubun.ToNumber<int>());

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                            info.IsGubun = dtr["IsGubun"].ToString().ToNull();

                            info.ItemName = this.GetLangName(
                                    dtr["ItemNameBase"].ToString().ToNull(), dtr["ItemNameKo"].ToString().ToNull(),
                                    dtr["ItemNameEn"].ToString().ToNull(), dtr["ItemNameJa"].ToString().ToNull(),
                                    dtr["ItemNameFr"].ToString().ToNull(), dtr["ItemNameZhcn"].ToString().ToNull(),
                                    dtr["ItemNameZhtw"].ToString().ToNull(), PreLang);

                            info.ItemSubName = this.GetLangName(
                                    dtr["ItemSubNameBase"].ToString().ToNull(), dtr["ItemSubNameKo"].ToString().ToNull(),
                                    dtr["ItemSubNameEn"].ToString().ToNull(), dtr["ItemSubNameJa"].ToString().ToNull(),
                                    dtr["ItemSubNameFr"].ToString().ToNull(), dtr["ItemSubNameZhcn"].ToString().ToNull(),
                                    dtr["ItemSubNameZhtw"].ToString().ToNull(), PreLang);

                            info.Status = GetStatus(dtr["StatusLast"].ToString()); //1:대기;2:접수;3:처리중;4:완료;5:반려
                            info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);                            
                            info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);
                            info.ExcessDay = dtr["ExcessDay"].ToString();
                            info.TaskNumber = dtr["TaskNumber"].ToString();

                            info.ROW = (totalCount.ToNumber<int>() - dtr["ROW"].ToString().ToNumber<int>()) + 1;
                            info.Title = dtr["Title"].ToString();
                            info.FileCount = dtr["FileCount"].ToString().ToNumber<int>();                            
                            info.ReplyCnt = dtr["ReplyCnt"].ToString().ToNumber<int>();

                            #region 요청자 이름 다국어 설정
                            info.NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString().ToNull(), dtr["NameKo"].ToString().ToNull(),
                                    dtr["NameEn"].ToString().ToNull(), dtr["NameJa"].ToString().ToNull(),
                                    dtr["NameFr"].ToString().ToNull(), dtr["NameZhcn"].ToString().ToNull(),
                                    dtr["NameZhtw"].ToString().ToNull(), PreLang);
                            #endregion
                            info.WriteUserID = dtr["WriteUserID"].ToString().ToNumber<int>();
                            info.ViewerCount = dtr["ViewerCount"].ToString().ToNumber<int>();
                              
                            #region 수신자 이름 다국어 설정
                            string[] tmpToName = dtr["TOName"].ToString().Split('|');

                            info.TONameBase = this.GetLangName(
                                    tmpToName[0], tmpToName[1], tmpToName[2], tmpToName[3],
                                    tmpToName[4], tmpToName[5], tmpToName[6], PreLang);
                            #endregion

                            info.TOUserID = dtr["TOUserID"].ToString().ToNumber<int>();
                            info.TOCnt = dtr["TOCnt"].ToString().ToNumber<int>();

                            info.ImIsType = dtr["ImIsType"].ToString();

                            if (info.ImIsType == "RE")
                            {
                                info.ImIsTypeStr = resMng.GetString("RequestCoWorkStr");
                            }
                            else if (info.ImIsType == "TO")
                            {
                                info.ImIsTypeStr = resMng.GetString("RequestedCoWorkStr");
                            }
                            else if (info.ImIsType == "CC")
                            {
                                info.ImIsTypeStr = resMng.GetString("CCCoWorkStr");
                            }
                            else if (info.ImIsType == "FO")
                            {
                                info.ImIsTypeStr = resMng.GetString("CollaboReceivedStr");
                            }
                            else if (info.ImIsType == "AL")
                            {
                                info.ImIsTypeStr = resMng.GetString("AllCoWorkListStr");
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
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 중요표시 협업리스트 카운트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetListCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string searchStatus = GetDecryptParam("searchStatus", "");
                string searchField = GetDecryptParam("searchField", "");
                string searchValue = GetDecryptParam("searchValue", "");
                string searchDateValue = GetDecryptParam("searchDateValue", "");
                string itemID = GetDecryptParam("itemID", "0");
                string itemSubID = GetDecryptParam("itemSubID", "0");
                string isGubun = GetDecryptParam("isGubun", "0");

                #region 날짜

                string startDate = "";
                string endDate = "";

                if (searchField == "RequestDate" || searchField == "StartDate" || searchField == "EndDate")
                {
                    startDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[0], "yyyyMMdd", PreLangReal);
                    endDate = DateHelpers.GetCultureDate(searchDateValue.Split(',')[1], "yyyyMMdd", PreLangReal);
                }

                #endregion
            
                int icnt = 0;

                DAL.ImpoUser biz = new DAL.ImpoUser();
                icnt = biz.GetPageListCount(CurrentUserInfo.CompanyID, startDate, endDate, searchStatus, searchField,
                    searchValue, CurrentUserInfo.UserID, itemID.ToNumber<int>(), itemSubID.ToNumber<int>(), isGubun.ToNumber<int>());

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 대분류에 맞는 소분류 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.ItemSubInfo> GetItemSub(string p)
        {
            SetDecryptString(p);
            string itemID = GetDecryptParam("itemID", "0");

            List<BE.ItemSubInfo> alList = new List<ItemSubInfo>();

            if (itemID.ToNumber<int>() > 0)
            {
                DAL.ItemSub dal = new DAL.ItemSub();
                var list = dal.GetItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), false);

                foreach (var be in list)
                {
                    BE.ItemSubInfo Info = new ItemSubInfo();
                    Info.ItemSubID = be.ItemSubID;
                    Info.ItemID = be.ItemID;

                    #region 이름 다국어 설정

                    Info.NameBase = this.GetLangName(
                            be.NameBase.ToString().ToNull(), be.NameKo.ToString().ToNull(),
                            be.NameEn.ToString().ToNull(), be.NameJa.ToString().ToNull(),
                            be.NameFr.ToString().ToNull(), be.NameZhcn.ToString().ToNull(),
                            be.NameZhtw.ToString().ToNull(), PreLang);

                    if (Info.NameBase.Length > 6) Info.NameBase = Info.NameBase.Substring(0, 5) + "..";
                    #endregion 이름 다국어 설정

                    alList.Add(Info);
                }
            }

            return alList;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 첨부파일 정보가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public string GetAttachFileListMenu(string p)
        {
            SetDecryptString(p);

            int groupID = GetDecryptParam("groupID", "0").ToNumber<Int32>();

            string response = "0";

            DAL.Attach dal = new DAL.Attach();
            List<BE.AttachInfo> attachInfo = dal.GetAttachGroupList(CurrentUserInfo.CompanyID, groupID);

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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 메뉴별 사용자 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModStatus(string p)
        {
            try
            {
                SetDecryptString(p);
                string status = GetDecryptParam("status", "");
                string isType = GetDecryptParam("isType", "");                

                BE.StatusInfo info = new BE.StatusInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.IsType = isType;
                info.UserID = CurrentUserInfo.UserID;
                info.Status = status;

                DAL.Status dal = new DAL.Status();
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
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 메뉴별 사용자 페이징 정보 설정하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModListUser(string p)
        {
            try
            {
                SetDecryptString(p);

                string isType = GetDecryptParam("isType", "");      
                string pageSize = GetDecryptParam("pageSize", "0");

                BE.ListUserInfo info = new BE.ListUserInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.IsType = isType;
                info.UserID = CurrentUserInfo.UserID;
                info.PageSize = pageSize.ToNumber<int>();

                DAL.ListUser dal = new DAL.ListUser();
                dal.AddMod(info);

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

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-23
        /// 작성목적 : 리스트 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelList(string p)
        {
            SetDecryptString(p);
            string listID = GetDecryptParam("listID", "");

            if (listID.Length > 0)
            {
                DAL.ImpoUser dal = new DAL.ImpoUser();

                foreach (string list in listID.Split(','))
                {
                    BE.ImpoUserInfo info = new BE.ImpoUserInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.ListID = list.ToNumber<int>();
                    info.UserID = CurrentUserInfo.UserID;

                    dal.Del(info);
                }
            }

            return true;
        }             


        #endregion 
    }
}

