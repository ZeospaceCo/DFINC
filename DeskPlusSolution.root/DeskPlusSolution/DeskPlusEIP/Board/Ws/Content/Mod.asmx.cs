using System;
using System.Collections.Generic;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using ObjList = System.Collections.Generic.List<System.Object>;

namespace Zeospace.DeskPlus.Board.Web.Ws.Content
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
            this.ResourceName = "Zeospace.DeskPlus.Board.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Board.Web.Ws.Content.Mod).Assembly;	//어셈블리 선언
            this.GetResourceManager();

            try
            {
                SetDecryptString(p);
                string categoryID = GetDecryptParam("categoryID", "0");
                string listID = GetDecryptParam("listID", "0");
                string screenShot = GetDecryptParam("screenShot", "");

                ObjList listobj = new ObjList();

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

                DAL.AttachFiles dal = new DAL.AttachFiles();
                List<BE.AttachFileInfo> attachInfo = dal.GetAttachFileLists(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>());

                string fileName = (screenShot.Length > 0) ? System.IO.Path.GetFileName(Server.MapPath(screenShot)).ToLower().Replace("thum_", "") : ""; //파일명

                string[] allowedExtensions = { ".bmp", ".tif", ".jpeg", ".jpg", ".gif", ".png", ".pjpeg", ".pjpg" };

                foreach (BE.AttachFileInfo _attachInfo in attachInfo)
                {
                    Zeospace.DeskPlus.Board.BE.AttachFileInfo info = new BE.AttachFileInfo();

                    info.CompanyID = _attachInfo.CompanyID;
                    info.CategoryID = _attachInfo.CategoryID;
                    info.ListID = _attachInfo.ListID;
                    info.AttachFileID = _attachInfo.AttachFileID;

                    //info.FilePath = _attachInfo.FilePath;
                    info.FileName = _attachInfo.FileName;
                    info.FileGUID = _attachInfo.FileGUID;

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

        #endregion 첨부파일

        #region 링크 URL

        /// <summary>
        /// 링크 URL 가져오기
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public ObjList GetLinkURLList(string p)
        {
            try
            {
                SetDecryptString(p);
                int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
                int listID = GetDecryptParam("listID", "0").ToNumber<int>();

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

                ObjList listobj = new ObjList();

                DAL.LinkURL dal = new DAL.LinkURL();
                List<BE.LinkURLInfo> linkURLInfo = dal.GetLinkURLList(boaComID, categoryID, listID);

                foreach (BE.LinkURLInfo _linkURLInfo in linkURLInfo)
                {
                    Zeospace.DeskPlus.Board.BE.LinkURLInfo info = new BE.LinkURLInfo();

                    info.CompanyID = _linkURLInfo.CompanyID;
                    info.CategoryID = _linkURLInfo.CategoryID;
                    info.ListID = _linkURLInfo.ListID;
                    info.LinkURLID = _linkURLInfo.LinkURLID;
                    info.URLName = _linkURLInfo.URLName;
                    info.URL = _linkURLInfo.URL;

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

        #endregion 링크 URL

        #region GET()

        /// <summary>
        /// 메인팝업으로 등록된 게시물 카운트
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public int GetMainPopUpCount(string p)
        {
            try
            {
                SetDecryptString(p);

                string categoryID = GetDecryptParam("catID", "0");
                string listID = GetDecryptParam("listID", "0");
                string startDate = GetDecryptParam("startDate", "");
                string endDate = GetDecryptParam("endDate", "");

                //통합게시판 회사아이디
                int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID.ToNumber<int>());

                int icnt = 0;

                startDate = DateHelper.GetCultureDate(startDate.Trim(), "yyyyMMdd", PreLangReal);
                endDate = DateHelper.GetCultureDate(endDate.Trim(), "yyyyMMdd", PreLangReal);

                DAL.ListEtc biz = new DAL.ListEtc();
                icnt = biz.GetMainPopUpCount(boaComID, categoryID.ToNumber<int>(), listID.ToNumber<int>(), startDate, endDate);

                return icnt;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }

        #endregion GET()

        #region DEL()

        #region 첨부파일
        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 첨부파일 id 별 삭제하기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelAttachFile(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int attachFileID = GetDecryptParam("attachFileID", "0").ToNumber<int>();

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

            BLL.AttachFile attachBiz = new BLL.AttachFile();

            attachBiz.Del(boaComID, categoryID, listID, attachFileID);

            return true;
        }
        #endregion 첨부파일

        #region 링크 URL 삭제
        /// <summary>
        /// 작성자   : 이지원
        /// 작성일자 : 2017-03-20
        /// 작성목적 : 링크URL 정보 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool DelLinkURL(string p)
        {
            SetDecryptString(p);
            int categoryID = GetDecryptParam("categoryID", "0").ToNumber<int>();
            int listID = GetDecryptParam("listID", "0").ToNumber<int>();
            int linkURLID = GetDecryptParam("linkURLID", "0").ToNumber<int>();

            //통합게시판 회사아이디
            int boaComID = BLL.Category.GetCompanyCategoryInfo(categoryID);

            BLL.LinkURL linkURLBiz = new BLL.LinkURL();

            linkURLBiz.Del(boaComID, categoryID, listID, linkURLID);

            return true;
        }
        #endregion

        #endregion DEL()
    }
}