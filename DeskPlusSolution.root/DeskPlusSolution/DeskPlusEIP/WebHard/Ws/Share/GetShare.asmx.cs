using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.WebHard.Web.Ws.Share
{
    /// <summary>
    /// GetShare의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class GetShare : Base.BaseWSWebHard
    {

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-28
        /// 작성목적 : 폴더 공유 설정 하기 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddShare(string p)
        {
            try
            {
                SetDecryptString(p);
                string folderID = GetDecryptParam("folderID", "0");
                string tmpValue = GetDecryptParam("tmpValue", "");
                string types = GetDecryptParam("types", "");

                List<BE.ShareFolderInfo> alShare = new List<BE.ShareFolderInfo>();

                string shareValue = tmpValue.Trim();
                if (shareValue.Length > 0)
                {
                    foreach (string _shareValue in shareValue.Split(','))
                    {
                        //구분$부서아이디값or사용자아이디값$다운로드$업로드
                        string[] arrVal = _shareValue.Split('$');

                        BE.ShareFolderInfo info = new BE.ShareFolderInfo();
                        info.CompanyID = CurrentUserInfo.CompanyID;
                        info.UserID = CurrentUserInfo.UserID;
                        info.FolderID = folderID.ToNumber<int>();
                        info.TargetID = arrVal[1].ToNumber<int>();
                        info.TargetType = arrVal[0];

                        if (arrVal[2] == "0" && arrVal[3] == "0")
                        {
                            info.Authority = "3";
                        }
                        else if (arrVal[2] == "1" && arrVal[3] == "0")
                        {
                            info.Authority = "2";
                        }
                        else if (arrVal[2] == "0" && arrVal[3] == "1")
                        {
                            info.Authority = "1";
                        }
                        else
                        {
                            info.Authority = "0";
                        }

                        alShare.Add(info);
                    }
                }

                BLL.ShareFolder bll = new BLL.ShareFolder();
                return bll.Add(alShare, types);

            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                return false;
            }
        }

        #endregion 

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2011-10-28
        /// 작성목적 : 폴더 공유 해제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelShare(string p)
        {
            try
            {
                SetDecryptString(p);
                int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
                string types = GetDecryptParam("types", "");

                int CompanyID = CurrentUserInfo.CompanyID;
                int UserID = CurrentUserInfo.UserID;

                BLL.ShareFolder bll = new BLL.ShareFolder();
                return bll.Del(CompanyID, UserID, folderID, types);
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
