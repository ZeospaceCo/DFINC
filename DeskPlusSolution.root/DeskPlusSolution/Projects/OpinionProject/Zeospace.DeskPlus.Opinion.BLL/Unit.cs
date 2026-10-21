using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Opinion.BE;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class Unit : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD

        /// <summary>
        /// 질문추가
        /// </summary>
        /// <param name="_uInfo"></param>
        /// <param name="_list"></param>
        /// <returns></returns>
        public int Add(BE.UnitInfo _uInfo, List<BE.UnitItemInfo> _list)
        {
            if (_uInfo == null) return -1;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Unit u = new DAL.Unit();
                    int unitID = u.Add(_uInfo);
                    if (unitID > 0)
                    {
                        if (_list != null)
                        {
                            string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                            targetPath += "{0}/Opinion/Unit/{1}/{2}/";
                            targetPath = string.Format(targetPath, _uInfo.CompanyID, GetYearMonth(DateTime.Now), unitID);

                            DAL.UnitItem ui = new DAL.UnitItem();
                            foreach (UnitItemInfo itemInfo in _list)
                            {
                                itemInfo.UnitID = unitID;
                                if (!_uInfo.HiddenSpoolPath.Trim().Equals("") && !itemInfo.ImgUrl.Trim().Equals(""))
                                {
                                    //이미지(파일)처리
                                    string[] arr = AddFile(_uInfo.HiddenSpoolPath + itemInfo.ImgUrl, targetPath);
                                    itemInfo.ImgUrl = arr[0];
                                    itemInfo.ImgName = arr[1];
                                }
                                if (ui.Add(itemInfo) < 0)
                                {
                                    return -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                    return unitID;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Unit[Add] : " + ex);
                return -1;
            }
        }
        /// <summary>
        /// 항목파일처리
        /// </summary>
        /// <param name="_originpath"></param>
        /// <param name="_targetpath"></param>
        /// <returns></returns>
        public string [] AddFile(string _originpath, string _targetpath)
        {
            DirectoryInfo di = new DirectoryInfo(FileHelper.GetServerMapPath(_targetpath));
            if (!di.Exists)
            {
                FileHelper.CreateSubFolder(_targetpath);
            }
            FileInfo fInfo = new FileInfo(_originpath);
            string filename = System.Guid.NewGuid().ToString() + fInfo.Extension;

            FileHelper.CopyFile(_originpath, _targetpath + filename);

            string[] s = new string[2];
            s[0] = _targetpath + filename;
            s[1] = filename;
            return s;
        }

        public string GetYearMonth(DateTime _dt)
        {
            return _dt.Year.ToString().PadLeft(4, '0') + _dt.Month.ToString().PadLeft(2, '0');
        }

        #endregion

        #region MOD

        /// <summary>
        /// 질문수정
        /// </summary>
        /// <param name="_uInfo"></param>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool Mod(BE.UnitInfo _uInfo, List<BE.UnitItemInfo> _list)
        {
            try
            {
                if (_uInfo == null) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Unit u = new DAL.Unit();
                    if (u.Mod(_uInfo))
                    {
                        DAL.UnitItem ui = new DAL.UnitItem();
                        if (ui.Del(_uInfo.CompanyID, _uInfo.UnitID))
                        {
                            if (_list != null)
                            {
                                //기존첨부파일삭제
                                string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                                targetPath += "{0}/Opinion/Unit/{1}/{2}/";
                                targetPath = string.Format(targetPath, _uInfo.CompanyID, GetYearMonth(_uInfo.WriteDate), _uInfo.UnitID);
                                DirectoryInfo di = new DirectoryInfo(FileHelper.GetServerMapPath(targetPath));
                                if (di.Exists)
                                {
                                    FileHelper.DelFolder(targetPath, false);
                                }

                                foreach (UnitItemInfo itemInfo in _list)
                                {
                                    if (!_uInfo.HiddenSpoolPath.Trim().Equals("") && !itemInfo.ImgUrl.Trim().Equals(""))
                                    {
                                        //이미지(파일)처리
                                        string [] arr = AddFile(_uInfo.HiddenSpoolPath + itemInfo.ImgUrl, targetPath);
                                        itemInfo.ImgUrl = arr[0];
                                        itemInfo.ImgName = arr[1];
                                    }

                                    ui.Add(itemInfo);
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Unit[Mod] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 활성화/비활성화
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <param name="_isActive"></param>
        /// <returns></returns>
        public bool UpdActive(int _companyID, int _unitID, string _isActive)
        {
            try
            {
                if (_companyID < 0 || _unitID < 0 || _isActive.Equals("")) return false;

                DAL.Unit u = new DAL.Unit();
                return u.UpdActive(_companyID, _unitID, _isActive);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Unit[UpdActive] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL

        /// <summary>
        /// 질문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _unitID, DateTime _writedate)
        {
            try
            {
                if (_companyID < 0 || _unitID < 0) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.UnitItem ui = new DAL.UnitItem();
                    if (ui.Del(_companyID, _unitID))
                    {
                        DAL.Unit u = new DAL.Unit();
                        if (!u.Del(_companyID, _unitID))
                        {
                            return false;
                        }

                        //기존첨부파일삭제
                        string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        targetPath += "{0}/Opinion/Unit/{1}/{2}";
                        targetPath = string.Format(targetPath, _companyID, GetYearMonth(_writedate), _unitID);
                        DirectoryInfo di = new DirectoryInfo(FileHelper.GetServerMapPath(targetPath));
                        if (di.Exists)
                        {
                            FileHelper.DelFolder(targetPath, false);
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Unit[Mod] : " + ex);
                return false;
            }
        }

        #endregion
    }
}
