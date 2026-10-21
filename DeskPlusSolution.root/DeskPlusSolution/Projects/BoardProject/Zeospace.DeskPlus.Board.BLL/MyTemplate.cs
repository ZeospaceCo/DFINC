using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.BLL
{
    public class MyTemplate : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 추가하기
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <returns></returns>
        public int Add(BE.MyTemplateInfo _listinfo, BE.MyTemplateEtcInfo _etcInfo)
        {
            try
            {
                if (_listinfo == null) return -1;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.MyTemplates dalList = new DAL.MyTemplates();
                    int listID = dalList.Add(_listinfo);

                    #region 첨부파일 추가

                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.MyAttachFile attachDal = new MyAttachFile();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, listID, _listinfo.ScreenShot,
                                _listinfo.HiddenSpoolPath, "");
                        }
                    }

                    #endregion 첨부파일 추가

                    DAL.MyTemplateEtc dalEtc = new DAL.MyTemplateEtc();
                    _etcInfo.ListID = listID;
                    dalEtc.Add(_etcInfo);

                    ts.Complete();

                    return listID;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시글을 게시판에 복사하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_myTemplateID">The _my template ID.</param>
        /// <param name="_deptID">The _dept ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_loginID">The _login ID.</param>
        /// <param name="_writeUserName">Name of the _write user.</param>
        /// <param name="_mobilePhone">The _mobile phone.</param>
        /// <param name="_preLang">The _pre lang.</param>
        /// <returns></returns>
        public int Move(int _companyID, int _categoryID, int _myTemplateID, int _deptID, int _userID,
            string _loginID, string _writeUserName, string _mobilePhone, string _preLang)
        {
            try
            {
                int listID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    HttpContext context = HttpContext.Current;
                    string screenShotFileName = "";
                    string screenShotFullName = "";

                    //첨부파일 이름 가져오기
                    DAL.MyTemplates dalList = new DAL.MyTemplates();
                    List<BE.MyTemplateInfo> tempList = dalList.GetMyTemplateLists(_companyID, 0, _myTemplateID, _userID);

                    foreach (BE.MyTemplateInfo be in tempList)
                    {
                        if (!string.IsNullOrEmpty(be.ScreenShot) && be.ScreenShot.ToNull().Length > 0)
                        {
                            FileInfo sourceFileInfo = new FileInfo(context.Server.MapPath(be.ScreenShot.Trim()));
                            screenShotFullName = be.ScreenShot.Trim();
                            screenShotFileName = sourceFileInfo.Name;
                        }
                    }

                    listID = dalList.Move(_companyID, _categoryID, _myTemplateID, _deptID, _userID, _writeUserName, screenShotFileName);

                    if (listID > 0)
                    {
                        string folderRoot = "";
                        string fromPath = "";
                        string toPath = "";
                        string nowYearMon = FileHelper.GetYearMonth();
                        string baseFileDirectory = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");

                        //스크린샷 파일 카피하기
                        if (screenShotFileName.ToNull().Length > 0)
                        {
                            //폴더 생성
                            folderRoot = context.Server.MapPath(baseFileDirectory + _companyID + "/Board/" + nowYearMon + "/" + listID + "/");
                            if (!Directory.Exists(folderRoot)) Directory.CreateDirectory(folderRoot);

                            toPath = baseFileDirectory + _companyID + "/Board/" + nowYearMon + "/" + listID + "/" + screenShotFileName;

                            //파일 복사
                            File.Copy(context.Server.MapPath(screenShotFullName), context.Server.MapPath(toPath), true);
                        }

                        DAL.MyAttachFiles dalMyAttach = new DAL.MyAttachFiles();
                        List<BE.MyAttachFileInfo> al = dalMyAttach.GetAttachFileLists(_companyID, 0, _myTemplateID);
                        DAL.AttachFiles dalAttach = new DAL.AttachFiles();
                        foreach (BE.MyAttachFileInfo info in al)
                        {
                            AttachFileInfo attachFileInfo = new AttachFileInfo();
                            attachFileInfo.CompanyID = _companyID;
                            attachFileInfo.CategoryID = _categoryID;
                            attachFileInfo.ListID = listID;
                            attachFileInfo.FilePath = baseFileDirectory + _companyID + "/Board/" + nowYearMon + "/" + listID + "/";
                            attachFileInfo.FileName = info.FileName;
                            attachFileInfo.FileType = info.FileType;
                            attachFileInfo.FileLength = info.FileLength;
                            attachFileInfo.FileGUID = info.FileGUID;

                            dalAttach.Add(attachFileInfo);

                            //폴더 생성
                            folderRoot = context.Server.MapPath(baseFileDirectory + _companyID + "/Board/" + nowYearMon + "/" + listID + "/");
                            if (!Directory.Exists(folderRoot)) Directory.CreateDirectory(folderRoot);

                            fromPath = info.FilePath + info.FileGUID;
                            toPath = baseFileDirectory + _companyID + "/Board/" + nowYearMon + "/" + listID + "/" + info.FileGUID;

                            //파일 복사
                            File.Copy(context.Server.MapPath(fromPath), context.Server.MapPath(toPath), true);
                        }

                        
                    }

                    ts.Complete();                                       
                }

                SendAlarm(_companyID, _categoryID, listID, _loginID, _mobilePhone, _preLang);

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Move : " + ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : 심재운
        /// 임시글을 게시판에 복사하기
        /// </summary>
        public void SendAlarm(int _companyID, int _categoryID, int listID, string _loginID, string _mobilePhone, string _preLang)
        {
            DAL.Category dalCat = new DAL.Category();
            DataSet catDs = dalCat.GetCategory(_companyID, _categoryID);
            string catName = "", optionAlarm = "";

            if (catDs != null)
            {
                foreach (DataRow dr in catDs.Tables[0].Rows)
                {
                    catName = UtilHelper.GetLangName(dr["NameBase"].ToString(), dr["NameKo"].ToString(), dr["NameEn"].ToString(),
                        dr["NameJa"].ToString(), dr["NameFr"].ToString(), dr["NameZhCn"].ToString(), dr["NameZhTw"].ToString(), _preLang);

                    optionAlarm = dr["OptionAlarm"].ToString();
                }
            }

            BE.ListInfo listInfo = new ListInfo();
            BLL.Lists listBiz = new Lists();
            DAL.Lists listDal = new DAL.Lists();
            List<ListInfo> alList = listDal.GetList(_companyID, _categoryID, listID);
            listInfo = alList[0];

            // 코드관리로 처리
            DataView dvAlarm = AppSettingHelper.GetSysCode(_companyID, "AlarmBoard");

            foreach (DataRowView dr in dvAlarm)
            {
                string method = dr["CodeID"].ToString().ToUpper();
                switch (method)
                {
                    case "MESSENGER":
                        // 메신저 알림
                        if (optionAlarm == "1") //alarm
                        {
                            foreach (ListInfo info in alList)
                            {
                                string alarmContent = listBiz.AlarmContentTemplate(catName, info.Title, info.WriteUserName,
                                System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(),
                                info.DeptName, _preLang);

                                //1-1 알람 저장
                                listInfo.AlarmContent = alarmContent;
                                listBiz.SendAlarmUser(listInfo, listID, 1);
                            }
                        }
                        break;

                    case "SMS":
                        if (optionAlarm == "2") //sms
                        {
                            foreach (ListInfo info in alList)
                            {
                                string smsContent = AppSettingHelper.GetAppSetting(AppSettingType.BOARD, "SMSContent", "");
                                smsContent = string.Format(smsContent, info.WriteUserName, info.Title);
                                listInfo.SMSContent = smsContent;
                                listInfo.SMSLoginID = _loginID;
                                listInfo.SMSMobilePhone = _mobilePhone;
                                listBiz.SendSMSUser(listInfo);
                            }
                        }
                        break;
                }
            }

            // 모바일사용의 경우, 무조건 입력
            if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
            {
                if (optionAlarm == "1") //alarm
                {
                    foreach (ListInfo info in alList)
                    {
                        string alarmContent = listBiz.AlarmContentTemplate(catName, info.Title, info.WriteUserName,
                        System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString(),
                        info.DeptName, _preLang);

                        //1-1 알람 저장
                        listInfo.AlarmContent = alarmContent;
                        listBiz.SendAlarmUser(listInfo, listID, 4);
                    }
                }
            }  
        }

        /// <summary>
        /// 수정하기
        /// </summary>
        /// <param name="_listinfo">The _listinfo.</param>
        /// <param name="_etcInfo">The _etc info.</param>
        /// <param name="_screenGubun">The _screen gubun.</param>
        /// <returns></returns>
        public bool Mod(BE.MyTemplateInfo _listinfo, BE.MyTemplateEtcInfo _etcInfo, string _screenGubun)
        {
            if (_listinfo == null) return false;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.MyTemplates dalList = new DAL.MyTemplates();
                    dalList.Mod(_listinfo, _screenGubun);

                    #region 첨부파일 추가

                    if (_listinfo.HiddenSpoolPath != "" && _listinfo.HiddenSpoolPath != null)
                    {
                        string yearMonth = Convert.ToDateTime(_listinfo.WriteDate).ToString("yyyyMM");
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listinfo.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.MyAttachFile attachDal = new MyAttachFile();
                            attachDal.Add(_listinfo.CompanyID, _listinfo.CategoryID, _listinfo.MyTemplateID, _listinfo.ScreenShot,
                                _listinfo.HiddenSpoolPath, yearMonth);
                        }
                    }

                    #endregion 첨부파일 추가

                    DAL.MyTemplateEtc dalEtc = new DAL.MyTemplateEtc();
                    dalEtc.Mod(_etcInfo);

                    ts.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);
                throw;
            }
        }

        #endregion ADD()

        #region DEL()

        /// <summary>
        /// 작성자 : 심재운
        /// 삭제
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Del(MyTemplateInfo _listInfo)
        {
            try
            {
                if (_listInfo == null)
                {
                    return false;
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        //1.내용 삭제
                        DAL.MyTemplates dalList = new DAL.MyTemplates();
                        dalList.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.MyTemplateID, _listInfo.WriteUserID);

                        //4. 기타 삭제
                        DAL.MyTemplateEtc dalEtc = new DAL.MyTemplateEtc();
                        dalEtc.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.MyTemplateID);

                        //5 첨부파일 실제 파일 삭제하기
                        DAL.MyAttachFiles dalAttach = new DAL.MyAttachFiles();
                        List<BE.MyAttachFileInfo> alAttach = dalAttach.GetAttachFileLists(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.MyTemplateID);

                        //5.1 첨부파일 삭제
                        bool response = dalAttach.Del(_listInfo.CompanyID, _listInfo.CategoryID, _listInfo.MyTemplateID);

                        if (response)
                        {
                            foreach (BE.MyAttachFileInfo info in alAttach)
                            {
                                Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(info.FilePath, false);
                            }
                        }

                        ts.Complete();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del : " + ex);
                throw;
            }
        }

        #endregion DEL()
    }
}