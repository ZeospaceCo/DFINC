using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Zeospace.DeskPlusMobile.Web.Helper;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.BLL
{
    public class Draft
    {
        #region ADD

        /// <summary>
        /// 임시저장
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool Add(BE.DraftInfo _info)
        {
            try
            {
                bool rtnval = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    Ws.Data.Draft dDal = new Ws.Data.Draft();
                    int draftID = dDal.Add(_info);
                    if (draftID > 0)
                    {
                        _info.DraftID = draftID;

                        #region DP_APP_DraftSequence

                        if (_info.DraftSequenceList != null)
                        {
                            if (_info.DraftSequenceList.Count > 0)
                            {
                                Ws.Data.DraftSequence dsDal = new Ws.Data.DraftSequence();

                                foreach (BE.DraftSequenceInfo info in _info.DraftSequenceList)
                                {
                                    info.DraftID = _info.DraftID;
                                    if (!dsDal.Add(info))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        #endregion DP_APP_DraftSequence

                        #region DP_APP_DraftBaseDoc

                        if (_info.DraftBaseDocList != null)
                        {
                            if (_info.DraftBaseDocList.Count > 0)
                            {
                                Ws.Data.DraftBaseDoc bDal = new Ws.Data.DraftBaseDoc();

                                foreach (BE.DraftBaseDocInfo info in _info.DraftBaseDocList)
                                {
                                    info.DraftID = _info.DraftID;
                                    if (bDal.Add(info) < 0)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        #endregion DP_APP_DraftBaseDoc

                        #region DP_APP_DraftFile

                        string yearMonth = System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString().PadLeft(2, '0');

                        #region 이전 파일생성

                        if (_info.BeforeHiddenPath != null)
                        {
                            if (!_info.BeforeHiddenPath.Equals(""))
                            {
                                BLL.DraftFile attachDal = new DraftFile();
                                attachDal.Mod(_info.CompanyID, draftID, yearMonth, _info.BeforeHiddenPath);
                            }
                        }

                        #endregion 이전 파일생성

                        #region 신규 파일생성

                        if (_info.HiddenSpoolPath != null)
                        {
                            //  FileData/@CompanyID/Approval/Draft/@년월/@DraftID/Attach
                            if (!_info.HiddenSpoolPath.Equals(""))
                            {
                                HttpContext context = HttpContext.Current;
                                DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                                if (di.Exists)
                                {
                                    BLL.DraftFile dfBll = new DraftFile();
                                    if (!dfBll.Add(_info.CompanyID, draftID, yearMonth, _info.HiddenSpoolPath))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }

                        #endregion 신규 파일생성

                        #endregion DP_APP_DraftFile
                    }
                    else
                    {
                        rtnval = false;
                    }

                    ts.Complete();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.Draft[Add] : ", ex);
                throw;
            }
        }

        #endregion ADD

        #region MOD

        /// <summary>
        /// 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_draftID"></param>
        /// <param name="_yyyymm"></param>
        /// <returns></returns>
        public bool Mod(BE.DraftInfo _info, int _draftID, string _yyyymm)
        {
            try
            {
                if (_info == null || _draftID.Equals(0) || _yyyymm.Equals("")) return false;

                bool rtnval = false;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    rtnval = Add(_info);
                    if (rtnval)
                    {
                        rtnval = Del(_info.CompanyID, _draftID, _yyyymm);
                    }

                    ts.Complete();
                }

                return rtnval;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.Draft[Mod] : ", ex);
                throw;
            }
        }

        #endregion MOD

        #region DEL

        /// <summary>
        /// 임시저장 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_formID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _draftID, string _yyyymm)
        {
            try
            {
                if (_companyID.Equals(0) || _draftID.Equals(0) || _yyyymm.Equals("")) return false;

                Ws.Data.Draft dDal = new Ws.Data.Draft();
                if (dDal.Del(_companyID, _draftID))
                {
                    // 첨부파일이 있다면, 해당 부분 삭제
                    string baseFileDirectory = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_FILEPATH");
                    string targetFolder = baseFileDirectory + "/" + _companyID.ToString() + "/Approval/Draft/" + _yyyymm + "/" + _draftID.ToString();

                    //  ex) /DeskPlusMobile/FileData/1/Approval/Draft/201206/37
                    if (Directory.Exists(FileHelper.GetServerMapPath(targetFolder))) FileHelper.DelFolder(targetFolder, false);
                }

                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Approval.BLL.Draft[Del] : ", ex);
                throw;
            }
        }

        #endregion DEL
    }
}