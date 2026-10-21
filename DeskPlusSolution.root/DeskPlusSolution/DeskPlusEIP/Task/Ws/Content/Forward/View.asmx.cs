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

namespace Zeospace.DeskPlus.Task.Web.Ws.Content.Forward
{
    /// <summary>
    /// View의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class View : Base.BaseWSTask
    {

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2016-11-07
        /// 작성목적 : 답변글 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            SetDecryptString(p);

            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int groupID = GetDecryptParam("groupID", "0").ToNumber<int>();

            if (listID == 0 || groupID == 0) return false;

            //답변글 삭제
            BLL.List dal = new BLL.List();
            dal.Del(CurrentUserInfo.CompanyID, listID);

            return true;
        }

        #endregion 
    }
}
