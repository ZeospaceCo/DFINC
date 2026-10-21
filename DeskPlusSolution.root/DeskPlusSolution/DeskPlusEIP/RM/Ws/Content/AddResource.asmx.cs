using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Content
{
    /// <summary>
    /// AddResource의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class AddResource : Base.BaseWSRM
    {
        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-15
        /// 작성목적 : 리스트 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetList(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();       

                DAL.List dal = new DAL.List();
                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, categoryID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Title = dtr["Title"].ToString();

                            string[] arrManyName = dtr["ManyName"].ToString().Split('|');

                            if (arrManyName.Length == 7)
                            {
                                info.ManagerName = this.GetLangName(arrManyName[0], arrManyName[1],
                                                                   arrManyName[2], arrManyName[3], arrManyName[4],
                                                                   arrManyName[5], arrManyName[6], PreLang);
                            }
                            else
                            {
                                info.ManagerName = "";
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
        /// 작성일자 : 2012-02-15
        /// 작성목적 : 자원 상세 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetListInfo(string p)
        {
            try
            {
                SetDecryptString(p);
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                DAL.List dal = new DAL.List();
                DataTableReader dtr = dal.GetListInfo(CurrentUserInfo.CompanyID, listID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            Zeospace.DeskPlus.RM.BE.ListInfo info = new BE.ListInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.Code = dtr["Code"].ToString().ToReverseXSS();
                            info.Title = dtr["Title"].ToString().ToReverseXSS();
                            info.Content = dtr["Content"].ToString().Replace("\r\n", "<br>").ToReverseXSS();
                            info.Qty = dtr["Qty"].ToString().ToNumber<int>();
                            info.UsableQty = dtr["UsableQty"].ToString().ToNumber<int>();
                            info.Place = dtr["Place"].ToString().ToReverseXSS();
                            info.Authority = dtr["Authority"].ToString();

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
        /// 작성일자 : 2012-03-05
        /// 작성목적 : 이미지 파일가져오기
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
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                DAL.Attach dal = new DAL.Attach();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, listID, "1");

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.AttachInfo info = new BE.AttachInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.CategoryID = dtr["CategoryID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.AttachFileID = dtr["AttachFileID"].ToString().ToNumber<int>();
                            info.FileName = dtr["FileName"].ToString();
                            info.FileGUID = dtr["FileGUID"].ToString();

                            info.FilePath = dtr["FilePath"].ToString() + dtr["FileGUID"].ToString();
                            Dictionary<string, string> paraDic = new Dictionary<string, string>();
                            paraDic.Add("filePath", info.FilePath);
                            string encParams = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));
                            info.FilePath = "/DeskPlusEIP/Common/DownLoad/ImageView.aspx?p=" + encParams;

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

        #endregion 
    }
}
