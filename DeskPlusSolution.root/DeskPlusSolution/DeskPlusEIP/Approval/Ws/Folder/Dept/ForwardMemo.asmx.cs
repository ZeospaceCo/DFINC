using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.Approval.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Dept
{
    /// <summary>
    /// ForwardMemo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class ForwardMemo : Base.BaseWSApproval
    {
        /// <summary>
        /// 작성자 : 심재운
        /// 전달하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool AddForward(string p)
        {

            SetDecryptString(p);

            string docID = GetDecryptParam("docID", "");
            string userList = GetDecryptParam("userList", "");//구분$키값 (M$1,M$2)
            string contents = GetDecryptParam("contents", "");

            if (docID.Length > 0 && userList.Length > 0)
            {
                List<BE.ForwardDocumentInfo> fDocList = new  List<ForwardDocumentInfo>();

                foreach (string tmpDocID in docID.Split(','))
                {
                    BE.ForwardDocumentInfo fdocInfo = new ForwardDocumentInfo();
                    fdocInfo.CompanyID = CurrentUserInfo.CompanyID;
                    fdocInfo.DocumentID = tmpDocID.ToNumber<int>();
                    fdocInfo.SendUserID = CurrentUserInfo.UserID;
                    fdocInfo.Memo = contents;

                    fDocList.Add(fdocInfo);                    
                }

                List<BE.ForwardUserInfo> fUserList = new List<ForwardUserInfo>();

                foreach (string userID in userList.Split(','))
                {
                    BE.ForwardUserInfo fUserInfo = new ForwardUserInfo();
                    fUserInfo.CompanyID = CurrentUserInfo.CompanyID;                        
                    fUserInfo.ReceiveUserID = userID.Split('$')[1].ToNumber<int>();

                    fUserList.Add(fUserInfo);
                }

                BLL.ForwardDocument biz = new BLL.ForwardDocument();
                biz.Add(fDocList, fUserList);

                return true;
            }

            return false;
            
        }
    }
}
