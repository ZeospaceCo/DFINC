using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Folder.Approve
{
    /// <summary>
    /// Add의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Add : Base.BaseWSApproval
    {

        #region 결재문서함

        /// <summary>
        /// 작성자 : 심재운
        /// 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int Register(string p)
        {

            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            string active = GetDecryptParam("active", "");
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");

            BE.ApprovalFolderInfo be = new BE.ApprovalFolderInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            be.ParentFolderID = folderID;
            be.ActiveFolder = active;

            be.FolderBase = name;

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.FolderKo = multiName.Split(',')[0];
                be.FolderEn = multiName.Split(',')[1];
                be.FolderJa = multiName.Split(',')[2];
                be.FolderFr = multiName.Split(',')[3];
                be.FolderZhcn = multiName.Split(',')[4];
                be.FolderZhtw = multiName.Split(',')[5];
            }

            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
            return dal.Add(be);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Modify(string p)
        {
            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            string active = GetDecryptParam("active", "");
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");


            BE.ApprovalFolderInfo be = new BE.ApprovalFolderInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            be.FolderID = folderID;
            be.ActiveFolder = active;

            be.FolderBase = name;

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.FolderKo = multiName.Split(',')[0];
                be.FolderEn = multiName.Split(',')[1];
                be.FolderJa = multiName.Split(',')[2];
                be.FolderFr = multiName.Split(',')[3];
                be.FolderZhcn = multiName.Split(',')[4];
                be.FolderZhtw = multiName.Split(',')[5];
            }

            be.ModifyUserID = CurrentUserInfo.UserID;

            DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
            return dal.Mod(be);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {

            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();

            if (folderID > 0)
            {
                DAL.ApprovalFolder dal = new DAL.ApprovalFolder();
                dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);

                return true;
            }

            return false;
        }

        #endregion

        #region 반려문서함
        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int RegisterReject(string p)
        {

            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            string active = GetDecryptParam("active", "");
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");

            BE.ApprovalFolderRInfo be = new BE.ApprovalFolderRInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            be.ParentFolderID = folderID;
            be.ActiveFolder = active;

            be.FolderBase = name;

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.FolderKo = multiName.Split(',')[0];
                be.FolderEn = multiName.Split(',')[1];
                be.FolderJa = multiName.Split(',')[2];
                be.FolderFr = multiName.Split(',')[3];
                be.FolderZhcn = multiName.Split(',')[4];
                be.FolderZhtw = multiName.Split(',')[5];
            }

            be.WriteUserID = CurrentUserInfo.UserID;

            DAL.ApprovalFolderR dal = new DAL.ApprovalFolderR();
            return dal.Add(be);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 수정
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool ModifyReject(string p)
        {
            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();
            string active = GetDecryptParam("active", "");
            string name = GetDecryptParam("name", "");
            string multiName = GetDecryptParam("multiName", "");


            BE.ApprovalFolderRInfo be = new BE.ApprovalFolderRInfo();
            be.CompanyID = CurrentUserInfo.CompanyID;
            be.UserID = CurrentUserInfo.UserID;
            be.FolderID = folderID;
            be.ActiveFolder = active;

            be.FolderBase = name;

            if (multiName != "" && multiName.IndexOf(",") != -1)
            {
                be.FolderKo = multiName.Split(',')[0];
                be.FolderEn = multiName.Split(',')[1];
                be.FolderJa = multiName.Split(',')[2];
                be.FolderFr = multiName.Split(',')[3];
                be.FolderZhcn = multiName.Split(',')[4];
                be.FolderZhtw = multiName.Split(',')[5];
            }

            be.ModifyUserID = CurrentUserInfo.UserID;

            DAL.ApprovalFolderR dal = new DAL.ApprovalFolderR();
            return dal.Mod(be);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelReject(string p)
        {

            SetDecryptString(p);
            int folderID = GetDecryptParam("folderID", "0").ToNumber<int>();

            if (folderID > 0)
            {
                DAL.ApprovalFolderR dal = new DAL.ApprovalFolderR();
                dal.Del(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, folderID);

                return true;
            }

            return false;
        }
        #endregion

    }
}
