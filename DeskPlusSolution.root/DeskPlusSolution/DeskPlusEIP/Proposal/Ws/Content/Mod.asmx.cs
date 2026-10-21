using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;

namespace Zeospace.DeskPlus.Proposal.Web.Ws.Content
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSProposal
    {
        #region GET

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

                string proposalNo = GetDecryptParam("proposalNo", "0");

                List<BE.AttachInfo> listobj = new List<BE.AttachInfo>();

                DAL.Attach dal = new DAL.Attach();
                List<BE.AttachInfo> attachInfo = dal.GetAttachFileList(CurrentUserInfo.CompanyID, proposalNo.ToNumber<int>());

                foreach (BE.AttachInfo _attachInfo in attachInfo)
                {
                    BE.AttachInfo info = new BE.AttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ProposalNo = _attachInfo.ProposalNo;
                    info.AttachID = _attachInfo.AttachID;

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

        #endregion

        #endregion

        #region DEL

        #region 첨부파일

        /// <summary>
        /// 작성자 : shine
        /// 용도 : 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);

            int proposalNo = GetDecryptParam("proposalNo", "0").ToNumber<int>();
            int attachID = GetDecryptParam("attachID", "0").ToNumber<int>();

            BLL.Attach attachBiz = new BLL.Attach();

            attachBiz.Del(CurrentUserInfo.CompanyID, proposalNo, attachID);

            return true;
        }

        #endregion

        #endregion
    }
}
