using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.Web.Ws.Admin.Content
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSRM
    {

        #region GET()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
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
        /// 작성일자 : 2012-02-28
        /// 작성목적 : 자원 담당자 정보 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetUserList(string p)
        {            
            try
            {
                this.ResourceName = "Zeospace.DeskPlus.RM.Web.Resource.GlobalString.string";			//리소스 명 선언
                this.AssemblyName = typeof(Zeospace.DeskPlus.RM.Web.Ws.Admin.Content.View).Assembly;	//어셈블리 선언
                this.GetResourceManager();


                SetDecryptString(p);
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                DAL.ListUser dal = new DAL.ListUser();

                DataTableReader dtr = dal.GetList(CurrentUserInfo.CompanyID, listID);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.ListUserInfo info = new BE.ListUserInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.ListID = dtr["ListID"].ToString().ToNumber<int>();
                            info.ListUserID = dtr["ListUserID"].ToString().ToNumber<int>();
                            info.ManagerUserID = dtr["ManagerUserID"].ToString().ToNumber<int>();

                            info.ManagerName = this.GetLangName(dtr["NameBase"].ToString(), dtr["NameKo"].ToString(),
                                                                dtr["NameEn"].ToString(), dtr["NameJa"].ToString(), dtr["NameFr"].ToString(),
                                                                dtr["NameZhcn"].ToString(), dtr["NameZhtw"].ToString(), PreLang);

                            if (dtr["Gubun"].ToString() == "1")
                            {
                                info.Gubun = resMng.GetString("ManagerGubun1");
                            }
                            else
                            {
                                info.Gubun = resMng.GetString("ManagerGubun2");
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

        #endregion 


    }
}
