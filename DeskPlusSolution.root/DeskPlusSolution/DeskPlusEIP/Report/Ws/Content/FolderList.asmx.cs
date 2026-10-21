using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Report.Web.Ws.Content
{
    /// <summary>
    /// FolderList의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class FolderList : Base.BaseWSReport
    {
        /// <summary>
        /// 분류삭제
        /// 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFolder(string p)
        {
            try
            {
                SetDecryptString(p);

                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                if (folderID == 0) return false;

                DAL.Folder dal = new DAL.Folder();
                return dal.Del(CurrentUserInfo.CompanyID, folderID);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 분류 권한 리스트 가져오기
        /// 심재운
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<BE.FolderAuthInfo> GetFolderAuthList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Report.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Report.Web.Ws.Content.FolderList).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);

                List<BE.FolderAuthInfo> obj = new List<BE.FolderAuthInfo>();
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                if (folderID == 0) return obj;

                DAL.FolderAuth dal = new DAL.FolderAuth();
                var dtr = dal.GetFolderAuth(CurrentUserInfo.CompanyID, folderID);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            obj.Add(new BE.FolderAuthInfo()
                            {
                                CompanyID = dtr["CompanyID"].ToString().ToNumber<int>(),
                                FolderID = dtr["FolderID"].ToString().ToNumber<int>(),
                                Gubun = dtr["Gubun"].ToString(),
                                OwnerID = dtr["OwnerID"].ToString().ToNumber<int>(),
                                NameBase = this.GetLangName(
                                    dtr["NameBase"].ToString(),
                                    dtr["NameKo"].ToString(), dtr["NameEn"].ToString(), dtr["NameJa"].ToString(),
                                    dtr["NameFr"].ToString(), dtr["NameZhCn"].ToString(), dtr["NameZhTw"].ToString(),
                                    PreLang).ToReverseXSS(),
                                DelStr = resMng.GetString("Del")
                            });
                        }
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 권한 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddFolderAuth(string p)
        {
            try
            {
                SetDecryptString(p);

                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string ownerList = GetDecryptParam("ownerList", "").TrimEnd(';');

                if (folderID == 0 || ownerList == "") return false;

                DAL.FolderAuth dal = new DAL.FolderAuth();

                /* 구분-아이디;구분-아이디; */
                foreach (string tmp in ownerList.Split(';'))
                {
                    if (tmp.Length == 0) continue;
                    BE.FolderAuthInfo info = new BE.FolderAuthInfo();
                    info.CompanyID = CurrentUserInfo.CompanyID;
                    info.FolderID = folderID;
                    info.Gubun = tmp.Split('-')[0];
                    info.OwnerID = tmp.Split('-')[1].ToNumber<int>();
                    info.WriteUserID = CurrentUserInfo.UserID;

                    dal.Add(info);
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 권한 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelFolderAuth(string p)
        {
            try
            {
                SetDecryptString(p);

                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string gubun = GetDecryptParam("gubun", "");
                int ownerID = GetDecryptParam("ownerID", "0").ToNumber<int>();

                if (folderID == 0 || gubun == "" || ownerID == 0) return false;

                DAL.FolderAuth dal = new DAL.FolderAuth();

                BE.FolderAuthInfo info = new BE.FolderAuthInfo();
                info.CompanyID = CurrentUserInfo.CompanyID;
                info.FolderID = folderID;
                info.Gubun = gubun;
                info.OwnerID = ownerID;

                dal.Del(info);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }
    }
}