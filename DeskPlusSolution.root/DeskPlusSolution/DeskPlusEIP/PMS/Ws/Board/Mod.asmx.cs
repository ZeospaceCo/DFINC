using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Text.RegularExpressions;
using System.IO;
using Zeospace.DeskPlus.PMS.BE;
using System.Collections;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Collections.Specialized;
using System.Text;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.PMS.Web.Ws.Board
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSPMS
    {

        #region 첨부파일

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetAttachFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.PMS.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.PMS.Web.Ws.Board.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
                int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
                int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();

                ObjList listobj = new ObjList();

                DAL.BoardAttach dal = new DAL.BoardAttach();
                List<BE.BoardAttachInfo> attachInfo = dal.GetAttachFile(CurrentUserInfo.CompanyID, projectID, menuID, boardID);
                
                foreach (BE.BoardAttachInfo _attachInfo in attachInfo)
                {
                    BE.BoardAttachInfo info = new BE.BoardAttachInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.ProjectID = _attachInfo.ProjectID;
                    info.MenuID = _attachInfo.MenuID;
                    info.BoardID = _attachInfo.BoardID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    //info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = _attachInfo.FileGUID;

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;
                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";
                    info.FileDel = resMng.GetString("FileDel");

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

        #region DEL()

        #region 첨부파일

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);
            int projectID = GetDecryptParam("projectID", "0").ToNumber<int>();
            int menuID = GetDecryptParam("menuID", "0").ToNumber<int>();
            int boardID = GetDecryptParam("boardID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

            BLL.BoardAttach attachBiz = new BLL.BoardAttach();

            attachBiz.Del(CurrentUserInfo.CompanyID, projectID, menuID, boardID, attachFileID);

            return true;
        }

        #endregion

        #endregion
    }
}
