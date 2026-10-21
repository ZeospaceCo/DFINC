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

namespace Zeospace.DeskPlus.Task.Web.Ws.Content.Request
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSTask
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
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
                var list = dal.GetItemSub(CurrentUserInfo.CompanyID, itemID.ToNumber<int>(), true);

                foreach (var be in list)
                {
                    BE.ItemSubInfo Info = new ItemSubInfo();
                    Info.ItemSubID = be.ItemSubID;
                    Info.ItemID = be.ItemID;
                    Info.TempCnt = be.TempCnt;

                    #region 이름 다국어 설정

                    Info.NameBase = this.GetLangName(
                            be.NameBase.ToString().ToNull(), be.NameKo.ToString().ToNull(),
                            be.NameEn.ToString().ToNull(), be.NameJa.ToString().ToNull(),
                            be.NameFr.ToString().ToNull(), be.NameZhcn.ToString().ToNull(),
                            be.NameZhtw.ToString().ToNull(), PreLang);

                    #endregion 이름 다국어 설정

                    alList.Add(Info);
                }
            }

            return alList;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 첨부파일 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.AttachInfo> GetAttachList(string p)
        {
            try
            {
                SetDecryptString(p);

                string listID = GetDecryptParam("listID", "0");

                List<BE.AttachInfo> listobj = new List<BE.AttachInfo>();

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachList(CurrentUserInfo.CompanyID, listID.ToNumber<int>());

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    BE.AttachInfo info = new BE.AttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachID = _attachInfo.AttachID;
                    info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = _attachInfo.FileGUID;
                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    listobj.Add(info);
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
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 전자결재 근거문서 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.BaseAppInfo> GetBaseAppList(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");

            DAL.BaseApp dal = new DAL.BaseApp();
            DataTableReader dtr = dal.GetBaseApp(CurrentUserInfo.CompanyID, listID);

            List<BE.BaseAppInfo> listobj = new List<BE.BaseAppInfo>();

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.BaseAppInfo info = new BE.BaseAppInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.BaseAppID = dtr["BaseAppID"].ToString().ToNumber<int>();                        
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();
                        info.Name = dtr["Name"].ToString().ToNull();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 업무협업 근거문서 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.BaseTaskInfo> GetBaseTaskList(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");

            DAL.BaseTask dal = new DAL.BaseTask();
            DataTableReader dtr = dal.GetBaseTask(CurrentUserInfo.CompanyID, listID);

            List<BE.BaseTaskInfo> listobj = new List<BE.BaseTaskInfo>();

            if (listID == "0") return listobj;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.BaseTaskInfo info = new BE.BaseTaskInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.BaseTaskID = dtr["BaseTaskID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();                        
                        info.Name = dtr["Name"].ToString();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 첨부파일 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttach(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int attachID = GetDecryptParam("attachID", "0").ToNumber<int>();

            BLL.Attach biz = new BLL.Attach();
            biz.Del(CurrentUserInfo.CompanyID, listID, attachID);

            return true;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 전자결재 근거문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseApp(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();

            DAL.BaseApp dal = new DAL.BaseApp();
            dal.Del(CurrentUserInfo.CompanyID, listID, documentID);

            return true;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-04-17
        /// 작성목적 : 업무협업 근거문서 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseTask(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int baseTaskID = GetDecryptParam("baseTaskID", "0").ToNumber<int>();

            DAL.BaseTask dal = new DAL.BaseTask();
            dal.Del(CurrentUserInfo.CompanyID, listID, baseTaskID);

            return true;
        }
        
        #endregion 
    }
}
