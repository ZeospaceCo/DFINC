using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Approval.Web.Ws.Preference.Signature
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class List : Base.BaseWSApproval
    {
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2012-06-20
        /// 작성목적 : Signature 이미지 저장
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Save(string p)
        {
            try
            {
                SetDecryptString(p);

                string SwfUploadGUID = GetDecryptParam("swfUploadGUID", "");
                string fileName = GetDecryptParam("fileName", "");
                string[] fileStr = fileName.Split('.');

                #region 인장 Copy
                string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "/DeskplusEIP/FileData/");
                FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Spool/" + SwfUploadGUID);
                FileHelper.CreateSubFolder(baseFileDirectory + CurrentUserInfo.CompanyID + "/Users/" + CurrentUserInfo.UserID + "/Picture/" + SwfUploadGUID);

                #region SPOOL 파일을 USERS 폴더에 복사하기
                string souceFile = baseFileDirectory + "{0}/Users/{1}/Spool/{2}/{3}";
                souceFile = string.Format(souceFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID, fileName);

                string destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, SwfUploadGUID + "." + fileStr[1]);

                System.IO.File.Copy(Server.MapPath(souceFile), Server.MapPath(destFile));
                #endregion

                #region 섬네일 이미지
                System.Drawing.Image image = System.Drawing.Image.FromFile(Server.MapPath(souceFile));
                //Size thumbnailSize = UtilHelper.GetThumbnailSize(image, 52);
                Size thumbnailSize = new Size(95, 95);

                System.Drawing.Image thumbnail = image.GetThumbnailImage(thumbnailSize.Width, thumbnailSize.Height, null, IntPtr.Zero);
                destFile = baseFileDirectory + "{0}/Users/{1}/Picture/{2}";
                destFile = string.Format(destFile, CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1]);
                thumbnail.Save(Server.MapPath(destFile));
                #endregion

                //33버전 결재를 위해 seal.jpg 를 하나 복사한다.(2012-03-14)
                destFile = string.Format(baseFileDirectory + "{0}/Users/{1}/Picture/{2}", CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, "seal.jpg");
                thumbnail.Save(Server.MapPath(destFile));

                #endregion 

                string signFileName = "Thum_Seal_" + SwfUploadGUID + "." + fileStr[1];

                Zeospace.DeskPlus.Account.DAL.User dal = new Zeospace.DeskPlus.Account.DAL.User();

                if (dal.ModSeal(CurrentUserInfo.CompanyID, CurrentUserInfo.UserID, signFileName))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Save", ex);
                return false;
            }
        }
    }
}
