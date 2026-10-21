using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Coop.Web.Ws.Content.Request
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSCoop
    {
        #region 첨부파일

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.AttachInfo> GetAttachFileList(string p)
        {
            try
            {
                SetDecryptString(p);

                string listID = GetDecryptParam("listID", "0");

                List<BE.AttachInfo> listobj = new List<BE.AttachInfo>();

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, listID.ToNumber<int>());

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    BE.AttachInfo info = new BE.AttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachFileID = _attachInfo.AttachFileID;

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

        #endregion 첨부파일

        #region 전자결재 근거문서

        /// <summary>
        /// Gets the base list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<Ws.Content.Request.ListBasePage> GetBaseList(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");

            //DAL.ListBase dal = new DAL.ListBase();
            //DataTableReader dtr = dal.GetListBase(CurrentUserInfo.CompanyID, listID);

            DAL.APPBaseDoc appDal = new DAL.APPBaseDoc();
            DataTableReader dtr = appDal.GetAppBaseDoc(CurrentUserInfo.CompanyID, listID);

            List<Ws.Content.Request.ListBasePage> listobj = new List<Ws.Content.Request.ListBasePage>();

            if (listID == "0") return listobj;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        Ws.Content.Request.ListBasePage info = new ListBasePage();

                        info.APPBaseDocID = dtr["APPBaseDocID"].ToString().ToNumber<int>();
                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.DocumentID = dtr["DocumentID"].ToString().ToNumber<int>();

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

                        //1:대기;2:접수;3:처리중;4:완료;5:반려
                        info.Status = GetStatus(dtr["StatusLast"].ToString());

                        info.Name = dtr["Name"].ToString().ToNull();

                        //info.RequestDate = DateHelpers.GetCultureDateChar8(dtr["RequestDate"].ToString(), PreLangReal);
                        //info.StartDate = DateHelpers.GetCultureDateChar8(dtr["StartDate"].ToString(), PreLangReal);
                        //info.EndDate = DateHelpers.GetCultureDateChar8(dtr["EndDate"].ToString(), PreLangReal);

                        //info.Title = dtr["Title"].ToString();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        #endregion 전자결재 근거문서

        #region 업무협업 근거문서

        /// <summary>
        /// Gets the base list.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.BaseCoopInfo> GetBaseCoopList(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");
            string groupID = GetDecryptParam("groupID", "0");

            DAL.BaseCoop appDal = new DAL.BaseCoop();
            DataTableReader dtr = appDal.GetBaseCoop(CurrentUserInfo.CompanyID, listID.ToNumber<int>(), groupID.ToNumber<int>());

            List<BE.BaseCoopInfo> listobj = new List<BE.BaseCoopInfo>();

            if (listID == "0") return listobj;

            if (dtr != null)
            {
                if (dtr.HasRows)
                {
                    while (dtr.Read())
                    {
                        BE.BaseCoopInfo info = new BE.BaseCoopInfo();

                        info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                        info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                        info.GroupID = dtr["GroupID"].ToString().ToNumber<int>();
                        info.BaseCoopID = dtr["BaseCoopID"].ToString().ToNumber<int>();
                        info.BaseOrder = dtr["BaseOrder"].ToString().ToNumber<int>();
                        info.Subject = dtr["Subject"].ToString();

                        listobj.Add(info);
                    }
                }
            }
            return listobj;
        }

        #endregion 업무협업 근거문서

        #region DEL()

        #region 첨부파일

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int attachID = GetDecryptParam("attachID", "0").ToNumber<int>();

            BLL.AttachFile attachBiz = new BLL.AttachFile();

            attachBiz.Del(CurrentUserInfo.CompanyID, listID, attachID);

            return true;
        }

        #endregion 첨부파일

        #region 전자결재 근거문서

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 근거문서 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseList(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int documentID = GetDecryptParam("documentID", "0").ToNumber<int>();

            //DAL.ListBase biz = new DAL.ListBase();

            //biz.Del(CurrentUserInfo.CompanyID, listID, targetID);

            DAL.APPBaseDoc biz = new DAL.APPBaseDoc();
            biz.Del(CurrentUserInfo.CompanyID, listID, documentID);

            return true;
        }

        #endregion 전자결재 근거문서

        #region 근거문서

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 업무협업 근거문서 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelBaseCoopList(string p)
        {
            SetDecryptString(p);

            string listID = GetDecryptParam("listID", "0");
            string groupID = GetDecryptParam("groupID", "0");
            string baseCoopID = GetDecryptParam("baseCoopID", "0");

            DAL.BaseCoop biz = new DAL.BaseCoop();
            BE.BaseCoopInfo info = new BE.BaseCoopInfo();
            info.CompanyID = CurrentUserInfo.CompanyID;
            info.ListID = listID.ToNumber<int>();
            info.GroupID = groupID.ToNumber<int>();
            info.BaseCoopID = baseCoopID.ToNumber<int>();

            biz.Del(info);

            return true;
        }

        #endregion 근거문서

        #endregion DEL()
    }

    /// <summary>
    /// 근거문서 클래스
    /// </summary>
    [Serializable]
    public class ListBasePage
    {
        public ListBasePage()
        {
        }

        public int ROW { get; set; }

        public int CompanyID { get; set; }

        public int APPBaseDocID { get; set; }

        public int DocumentID { get; set; }

        public int ListID { get; set; }

        public int ParentDocumentID { get; set; }

        public string Name { get; set; }

        public string IsLegacy { get; set; }

        public string Path { get; set; }

        public int BaseOrder { get; set; }

        public string Status { get; set; }

        public string ItemName { get; set; }

        public string ItemSubName { get; set; }
    }
}