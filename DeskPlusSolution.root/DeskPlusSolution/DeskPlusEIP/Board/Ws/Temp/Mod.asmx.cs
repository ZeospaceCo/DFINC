using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Temp
{
    /// <summary>
    /// Mod의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다.
    [System.Web.Script.Services.ScriptService]
    public class Mod : Base.BaseWSBoard
    {
        #region 임시저장

        /// <summary>
        /// 첨부파일 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetMyAttachFileList(string p)
        {
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Ws.Temp.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string categoryID = categoryID = GetDecryptParam("categoryID", "0");
                string listID = GetDecryptParam("listID", "0");
                string screenShot = GetDecryptParam("screenShot", "");

                ObjList listobj = new ObjList();

                DAL.MyAttachFiles dal = new DAL.MyAttachFiles();
                List<BE.MyAttachFileInfo> attachInfo = dal.GetAttachFileLists(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

                string fileName = (screenShot.Length > 0) ? System.IO.Path.GetFileName(Server.MapPath(screenShot)).ToLower().Replace("thum_", "") : ""; //파일명
                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.MyAttachFileInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Board.BE.MyAttachFileInfo info = new BE.MyAttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.CategoryID = _attachInfo.CategoryID;
                    info.MyTemplateID = _attachInfo.MyTemplateID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    //info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    //guid 가 없다는 것은 마이그래이션을 해서 그러함으로, filename 을 참조할 수도 있다.
                    info.FileGUID = (_attachInfo.FileGUID.ToNull().Length > 0) ? _attachInfo.FileGUID.ToNull() : info.FileName;

                    Dictionary<string, string> paraDic = new Dictionary<string, string>();
                    paraDic.Add("filePath", info.FilePath + info.FileGUID);
                    paraDic.Add("fileName", info.FileName);
                    info.FilePath = GetEncryptParam(Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.Utility.GetParameterDic(paraDic));

                    info.FileType = _attachInfo.FileType.Replace(".", "");
                    info.FileLength = _attachInfo.FileLength;

                    info.FileLengthKB = String.Format("{0:n1}", Convert.ToDecimal(_attachInfo.FileLength) / 1024) + " KB";

                    //스크린샷 파일과 동일 여부 찾기
                    info.ScreenShot = (fileName == info.FileName.ToLower() || fileName == info.FileGUID.ToLower()) ? "1" : "0";

                    string fileOk = "0";
                    for (int i = 0; i < allowedExtensions.Length; i++)
                    {
                        if (_attachInfo.FileType.ToLower() == allowedExtensions[i])
                        {
                            fileOk = "1";
                            break;
                        }
                    }

                    info.ImageType = fileOk;
                    info.FileDel = resMng.GetString("Del");

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

        #endregion 임시저장

        #region DEL()

        #region 임시저장

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 임시저장 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelMyAttachFile(string p)
        {
            SetDecryptString(p);
            string categoryID = GetDecryptParam("categoryID", "0");
            string listID = GetDecryptParam("listID", "0");
            string attachFileID = GetDecryptParam("attachFileID", "0");

            BLL.MyAttachFile attachBiz = new BLL.MyAttachFile();

            attachBiz.Del(CurrentUserInfo.CompanyID, categoryID.ToNumber<int>(),
                listID.ToNumber<int>(), attachFileID.ToNumber<int>());

            return true;
        }

        #endregion 임시저장

        #endregion DEL()
    }
}