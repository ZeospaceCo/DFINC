using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Visit.Web.Ws.Content
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSVisit
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.View).Assembly;	//어셈블리 선언
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
                            info.Title =  this.GetLangName(
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
            this.AssemblyName = typeof(Zeospace.DeskPlus.Visit.Web.Ws.Content.View).Assembly;	//어셈블리 선언
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
        /// 작성일자 : 2014-02-18
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
                int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

                DAL.Repeat dal = new DAL.Repeat();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, reserveID);

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
   
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-06
        /// 작성목적 : 승인 / 취소 처리
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModVisitType(string p)
        {
            SetDecryptString(p);
            string visitType = GetDecryptParam("visitType", "0");
            string preLang = GetDecryptParam("preLang", "ko");
            int reserveID = GetDecryptParam("reserveID", "0").ToNumber<int>();

            DAL.Reserve dal = new DAL.Reserve();
            if (dal.Mod(CurrentUserInfo.CompanyID, reserveID, visitType))
            {
                // 알람처리
                BLL.Reserve bll = new BLL.Reserve();
                bll.AlarmVisitType(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, reserveID, visitType, preLang);

                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 

    }
}
