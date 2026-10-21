using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using Zeospace.DeskPlus.Account.BE;

namespace Zeospace.DeskPlus.Account.Web.Ws.MyInfo
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSAccount
    {

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 사진 및 도장 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelPhotoSeal(string p)
        {
            try
            {
                SetDecryptString(p);

                string gubun = GetDecryptParam("gubun", "");
                if (gubun == "") return false;

                DAL.User dal = new DAL.User();
                List<UserInfo> userList = dal.GetUserInfo(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, false, false);

                string photoImage = "";
                string photoSeal = "";
                if (userList.Count > 0)
                {
                    photoImage = userList[0].PhotoImage;
                    photoSeal = userList[0].SealImage;
                }

                //사진이나 도장 삭제는 결재에서 이전에 사용할 경우, 문제 발생 할 수 있으므로 db 만 삭제함.
                if (gubun == "photo")
                {
                    photoImage = "";
                }

                if (gubun == "seal")
                {
                    photoSeal = "";
                }

                return dal.ModPhotoSeal(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, photoSeal, photoImage);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelPhotoSeal : " + ex.Message);
                throw;
            }
        }
    }
}
