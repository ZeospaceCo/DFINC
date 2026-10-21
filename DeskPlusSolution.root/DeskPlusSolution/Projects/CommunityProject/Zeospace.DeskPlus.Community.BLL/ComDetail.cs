using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Community.BE;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class ComDetail : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-04
        /// 작성목적 : 커뮤니티 디자인 정보 저장
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_communityID"></param>
        /// <returns></returns>
        public bool Add(int _companyID, int _communityID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Detail 테이블 추가 
                    BE.ComDetailInfo info = new BE.ComDetailInfo();
                    info.CompanyID = _companyID;
                    info.CommunityID = _communityID;
                    info.TitleImage = "item1";
                    info.TitleTextColor = "#000000";
                    info.MenuColor1 = "#FFFFFF";
                    info.MenuColor2 = "#E4F2FF";
                    info.MenuColor3 = "#E9EBF3";
                    info.MenuBlock1TextClass = "#000000";
                    info.MenuBlock2TextClass = "#000000";
                    info.AdminDescription = "";

                    DAL.ComDetail dal = new DAL.ComDetail();

                    if (!dal.Add(info))
                    {
                        return false;
                    }

                    // 2. CSS 파일 생성 
                    HttpContext context = HttpContext.Current;
                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string tmpfilePath = filePath + "{0}/Community/{1}";
                    string folderVirtualPath = string.Format(tmpfilePath, _companyID, _communityID);
                    string folderPath = context.Server.MapPath(folderVirtualPath);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string fileCssPath = context.Server.MapPath(folderVirtualPath) + "\\Community.css";
                    if (File.Exists(fileCssPath))
                    {
                        File.Delete(fileCssPath);
                    }
                    FileStream aa = File.Create(fileCssPath);
                    aa.Close();

                    StreamWriter writer = File.AppendText(fileCssPath);

                    writer.WriteLine(" .cm_siteMain .header {padding:0px;display:inline-block;width:99%;margin:0px 0px 5px 0px; background:url('/DeskPlusEIP/Design/C/Img/Community/" + info.TitleImage + ".jpg');border:1px}");
                    writer.WriteLine(" .cm_siteMain .header .tit {color:" + info.TitleTextColor + ";font-size:12px;font-weight:bold;padding:0px 5px 0px 5px;margin:0px 0px 10px 0px;border:0px solid red;}");
                    writer.WriteLine(" .cm_siteMain .header .info li {color:" + info.TitleTextColor + ";float:left;padding:0px 7px;}");
                    writer.WriteLine(" .cm_userType1 {background:" + info.MenuColor1 + ";padding:5px 0px;border-left:1px solid #D2D2D2;border-right:1px solid #D2D2D2;}");
                    writer.WriteLine(" .cm_userType1 .box01{padding:9px 13px; background:" + info.MenuColor2 + ";margin:0px 5px;border:0px solid red;}");
                    writer.WriteLine(" .cm_userType1 .box02{padding:9px 13px; background:" + info.MenuColor3 + ";margin:5px 5px 0px 5px;border:0px solid red;}");
                    writer.WriteLine(" .cm_userType1 .box01 span {color:" + info.MenuBlock1TextClass + ";}");
                    writer.WriteLine(" .cm_userType1 .box02 span {color:" + info.MenuBlock2TextClass + ";}");
                    writer.WriteLine(" .cm_userType1 .box02 a {color:" + info.MenuBlock2TextClass + ";}");     
                    writer.Close();
                       

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return false;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-05-29
        /// 작성목적 : 커뮤니티 디자인 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Mod(ComDetailInfo _info)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Detail 테이블 수정
                    DAL.ComDetail dal = new DAL.ComDetail();

                    if (!dal.Mod(_info))
                    {
                        return false;
                    }

                    // 2. CSS 파일 생성 
                    HttpContext context = HttpContext.Current;
                    string filePath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                    string tmpfilePath = filePath + "{0}/Community/{1}";
                    string folderVirtualPath = string.Format(tmpfilePath, _info.CompanyID, _info.CommunityID);
                    string folderPath = context.Server.MapPath(folderVirtualPath);
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string fileCssPath = context.Server.MapPath(folderVirtualPath) + "\\Community.css";
                    if (File.Exists(fileCssPath))
                    {
                        File.Delete(fileCssPath);
                    }
                    FileStream aa = File.Create(fileCssPath);
                    aa.Close();

                    StreamWriter writer = File.AppendText(fileCssPath);

                    writer.WriteLine(" .cm_siteMain .header {padding:0px;display:inline-block;width:99%;margin:0px 0px 5px 0px; background:url('/DeskPlusEIP/Design/C/Img/Community/" + _info.TitleImage + ".jpg');border:1px}");
                    writer.WriteLine(" .cm_siteMain .header .tit {color:" + _info.TitleTextColor + ";font-size:12px;font-weight:bold;padding:0px 5px 0px 5px;margin:0px 0px 10px 0px;border:0px solid red;}");
                    writer.WriteLine(" .cm_siteMain .header .info li {color:" + _info.TitleTextColor + ";float:left;padding:0px 7px;}");
                    writer.WriteLine(" .cm_userType1 {background:" + _info.MenuColor1 + ";padding:5px 0px;border-left:1px solid #D2D2D2;border-right:1px solid #D2D2D2;}");
                    writer.WriteLine(" .cm_userType1 .box01{padding:9px 13px; background:" + _info.MenuColor2 + ";margin:0px 5px;border:0px solid red;}");
                    writer.WriteLine(" .cm_userType1 .box02{padding:9px 13px; background:" + _info.MenuColor3 + ";margin:5px 5px 0px 5px;border:0px solid red;}");
                    writer.WriteLine(" .cm_userType1 .box01 span {color:" + _info.MenuBlock1TextClass + ";}");
                    writer.WriteLine(" .cm_userType1 .box02 span {color:" + _info.MenuBlock2TextClass + ";}");
                    writer.WriteLine(" .cm_userType1 .box02 a {color:" + _info.MenuBlock2TextClass + ";}");
                    writer.Close();


                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add()", ex);
                return false;
            }
        }

        #endregion 
    }
}
