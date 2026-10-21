using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.PMS.BE;

namespace Zeospace.DeskPlus.PMS.BLL
{
    public class Unit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 프로젝트 unit 추가
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-21
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public int Add(UnitInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Unit dal = new DAL.Unit();
                    int unitID = dal.Add(_listInfo);

                    if (unitID < 0)
                    {
                        return -1;
                    }
                    else
                    {
                        #region 첨부파일 추가

                        if (_listInfo.HiddenSpoolPath != "" && _listInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;

                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.UnitAttach attachDal = new UnitAttach();
                                attachDal.Add(_listInfo.CompanyID, _listInfo.ProjectID, unitID, _listInfo.HiddenSpoolPath, "");
                            }
                        }

                        #endregion 첨부파일 추가

                        #region 3. 프로젝트 단위 책임자 추가

                        if (_listInfo.UnitCharger.Length > 0)
                        {
                            string[] chargerStr = _listInfo.UnitCharger.Split(',');
                            DAL.UnitCharger iCharger = new DAL.UnitCharger();
                            for (int i = 0; i < chargerStr.Length; i++)
                            {
                                BE.UnitChargerInfo projUnitInfo = new UnitChargerInfo();
                                projUnitInfo.CompanyID = _listInfo.CompanyID;
                                projUnitInfo.ProjectID = _listInfo.ProjectID;
                                projUnitInfo.UnitID = unitID;
                                projUnitInfo.UserID = chargerStr[i].ToNumber<int>();
                                iCharger.Add(projUnitInfo);
                            }
                        }

                        #endregion 3. 프로젝트 단위 책임자 추가

                        ts.Complete();
                        return unitID;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(UnitInfo _listInfo) : " + ex);

                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 프로젝트 단위 수정
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-21
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Mod(UnitInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Zeospace.DeskPlus.PMS.DAL.Unit dal = new DAL.Unit();
                    bool success = dal.Mod(_listInfo);
                    int UnitID = _listInfo.UnitID;
                    if (!success)
                    {
                        return false;
                    }
                    else
                    {
                        #region 2. 첨부파일

                        #region 1. 첨부파일 내역 : 기존 파일을 삭제할 경우 파일삭제 및 DB 삭제 (수정 모드시)

                        if (_listInfo.HiddenFileList != null && _listInfo.HiddenSaveFileList != null)
                        {
                            int attachFileID = 0;
                            string fileFullPath = String.Empty;
                            string delFileList = String.Empty;
                            string[] tmpFileList;
                            string[] fileInfoArr;

                            ArrayList delFileListArr = FileHelper.GetDelFileList(_listInfo.HiddenFileList, _listInfo.HiddenSaveFileList);
                            DAL.UnitAttach iAttachFile = new DAL.UnitAttach();

                            for (int i = 0; i < delFileListArr.Count; i++)
                            {
                                delFileList = delFileListArr[i].ToString();        //삭제된 파일 리스트 정보   AttachFileID | FileFullName
                                tmpFileList = delFileList.Split('|');              //Array로 변환
                                attachFileID = tmpFileList[0].ToNumber<int>();    //AttachFileID
                                fileFullPath = tmpFileList[1].ToString();          //FileFullName "S>23>/DeskPlusApp/FileData/1/Board/200705/108/>chapter11.html>dfef8a4f-d178-4724-bf99-e77868a7d6e7.zeo"
                                fileInfoArr = fileFullPath.Split('>');
                                fileFullPath = fileInfoArr[2] + fileInfoArr[4];     //DeskPlusApp/FileData/1/Board/200705/108/dfef8a4f-d178-4724-bf99-e77868a7d6e7.zeo

                                iAttachFile.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.UnitID, attachFileID);  // DataBase 삭제
                                FileHelper.DelFile(fileFullPath, false);			// 실제 파일 삭제
                            }
                        }

                        #endregion 1. 첨부파일 내역 : 기존 파일을 삭제할 경우 파일삭제 및 DB 삭제 (수정 모드시)

                        #region 첨부파일 추가

                        if (_listInfo.HiddenSpoolPath != "" && _listInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_listInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.UnitAttach attachDal = new UnitAttach();
                                attachDal.Add(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.UnitID, _listInfo.HiddenSpoolPath, FileHelper.GetYearMonth());
                            }
                        }

                        #endregion 첨부파일 추가

                        #region 4. 프로젝트 단위 책임자 추가

                        if (_listInfo.UnitCharger.Length > 0)
                        {
                            string[] chargerStr = _listInfo.UnitCharger.Split(',');
                            DAL.UnitCharger iCharger = new DAL.UnitCharger();
                            BE.UnitChargerInfo projUnitInfo1 = new UnitChargerInfo();
                            projUnitInfo1.CompanyID = _listInfo.CompanyID;
                            projUnitInfo1.ProjectID = _listInfo.ProjectID;
                            projUnitInfo1.UnitID = UnitID;

                            iCharger.Del(projUnitInfo1);

                            for (int i = 0; i < chargerStr.Length; i++)
                            {
                                BE.UnitChargerInfo projUnitInfo2 = new UnitChargerInfo();
                                projUnitInfo2.CompanyID = _listInfo.CompanyID;
                                projUnitInfo2.ProjectID = _listInfo.ProjectID;
                                projUnitInfo2.UnitID = UnitID;
                                projUnitInfo2.UserID = chargerStr[i].ToNumber<int>();
                                iCharger.Add(projUnitInfo2);
                            }
                        }

                        #endregion 4. 프로젝트 단위 책임자 추가

                        #endregion 2. 첨부파일

                        ts.Complete();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod : " + ex);

                throw;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 메서드명   :
        /// 작성목적   : 삭제
        /// 작성자     : 심재운
        /// 최초작성일 : 2009-01-21
        /// </summary>
        /// <param name="_listInfo">The _list info.</param>
        /// <returns></returns>
        public bool Del(UnitInfo _listInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    #region 1. 프로젝트 단위 담당자 삭제

                    BE.UnitChargerInfo chargerInfo = new UnitChargerInfo();
                    chargerInfo.CompanyID = _listInfo.CompanyID;
                    chargerInfo.ProjectID = _listInfo.ProjectID;
                    chargerInfo.UnitID = _listInfo.UnitID;

                    DAL.UnitCharger charger = new DAL.UnitCharger();
                    charger.Del(chargerInfo);

                    #endregion 1. 프로젝트 단위 담당자 삭제

                    #region 2. 프로젝트 게시판 파일 삭제

                    DAL.UnitAttach attach = new DAL.UnitAttach();
                    List<BE.UnitAttachInfo> al = attach.GetAttachFile(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.UnitID);

                    string filePath = "";
                    foreach (BE.UnitAttachInfo _be in al)
                    {
                        filePath = _be.FilePath;
                        break;
                    }

                    if (filePath.Length > 0)
                    {
                        filePath = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.FileHelper.GetServerMapPath(filePath);

                        if (Directory.Exists(filePath))
                        {
                            Directory.Delete(filePath, true);
                        }
                    }

                    #endregion 2. 프로젝트 게시판 파일 삭제

                    #region 4. 프로젝트 게시판 첨부파일 삭제

                    attach.Del(_listInfo.CompanyID, _listInfo.ProjectID, _listInfo.UnitID);

                    #endregion 4. 프로젝트 게시판 첨부파일 삭제

                    #region 5. 프로젝트 게시판 삭제

                    Zeospace.DeskPlus.PMS.DAL.Unit dal = new DAL.Unit();
                    dal.Del(_listInfo);

                    #endregion 5. 프로젝트 게시판 삭제

                    ts.Complete();
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