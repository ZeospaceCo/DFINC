using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Board.BE;
using Zeospace.DeskPlus.Board.DAL;

namespace Zeospace.DeskPlus.Board.Web.Ws.Common
{
    /// <summary>
    /// InterestedUser의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class InterestedUser : Base.BaseWSBoard
    {
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool SetInterestedUser(string p)
        {
            SetDecryptString(p);
            string interestedUser = GetDecryptParam("interestedUser", "");

            try
            {
                DAL.InterestedUser dal = new DAL.InterestedUser();

                //삭제
                BE.InterestedUserInfo be = new BE.InterestedUserInfo();
                be.CompanyID = CurrentUserInfo.CompanyID;
                be.UserID = CurrentUserInfo.UserID;
                dal.DelInterestedUser(be);

                //추가
                string[] strTemp = interestedUser.Split('|');

                for (int i = 0; i < strTemp.Length; i++)
                {                    
                    string interestedUserIDS = strTemp[i];

                    if (interestedUserIDS.Length > 0)
                    {
                        int interestedUserID = int.Parse(strTemp[i]);

                        be.InterestedUserID = interestedUserID;
                        dal.AddInterestedUser(be);
                    }
                }

                return true;
            }
            catch (Exception exc)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SetInterestedUser", exc);
                return false;
            }
        }
    }
}
