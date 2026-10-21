using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Zeospace.DeskPlus.Opinion.BE;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
//using Zeospace.DeskPlus.Common.BE;
using System.Xml;
using System.Collections;
using System.Web;
using System.IO;

namespace Zeospace.DeskPlus.Opinion.BLL
{
    public class Ques : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET
        #endregion

        #region ADD

        /// <summary>
        /// 설문추가
        /// </summary>
        /// <param name="_uInfo"></param>
        /// <param name="_list"></param>
        /// <returns></returns>
        public int Add(BE.QuesInfo _quesInfo)
        {
            if (_quesInfo == null) return -1;

            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //설문정보
                    DAL.Ques q = new DAL.Ques();
                    int quesID = q.Add(_quesInfo);
                    if (quesID > 0)
                    {
                        //첨부파일 추가
                        if (!_quesInfo.HiddenSpoolPath.Equals("") && _quesInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_quesInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.Attach attachDal = new Attach();
                                attachDal.Add(_quesInfo.CompanyID, quesID, _quesInfo.HiddenSpoolPath);
                            }
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();

                    return quesID;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Ques[Add] : " + ex);
                return -1;
            }
        }

        #endregion

        #region MOD

        /// <summary>
        /// 설문수정
        /// </summary>
        /// <param name="_uInfo"></param>
        /// <param name="_list"></param>
        /// <returns></returns>
        public bool Mod(BE.QuesInfo _quesInfo)
        {
            try
            {
                if (_quesInfo == null) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    //설문정보
                    DAL.Ques q = new DAL.Ques();
                    if (q.Mod(_quesInfo))
                    {
                        //첨부파일 추가
                        if (!_quesInfo.HiddenSpoolPath.Equals("") && _quesInfo.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_quesInfo.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.Attach attachDal = new Attach();
                                attachDal.Add(_quesInfo.CompanyID, _quesInfo.QuesID, _quesInfo.HiddenSpoolPath);
                            }
                        }
                    }
                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Ques[Mod] : " + ex);
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
        public bool UpdActive(int _companyID, int _quesID, string _isActive)
        {
            try
            {
                if (_companyID < 0 || _quesID < 0 || _isActive.Equals("")) return false;

                DAL.Ques q = new DAL.Ques();
                return q.UpdActive(_companyID, _quesID, _isActive);
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Ques[UpdActive] : " + ex);
                return false;
            }
        }
        #endregion

        #region DEL

        /// <summary>
        /// 설문삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_unitID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _quesID)
        {
            try
            {
                if (_companyID < 0 || _quesID < 0) return false;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Ques qDAL = new DAL.Ques();
                    if (qDAL.Del(_companyID, _quesID))
                    {
                        #region 파일삭제
                        string targetPath = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseFileDirectory", "");
                        targetPath += "{0}/Opinion/Ques/{1}";
                        targetPath = string.Format(targetPath, _companyID, _quesID);

                        if (Directory.Exists(FileHelper.GetServerMapPath(targetPath)))
                        {
                            if (!FileHelper.DelFolder(targetPath, false))
                            {
                                return false;
                            }
                        }
                        #endregion
                    }

                    ts.Complete();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Opinion.BLL.Ques[Del] : " + ex);
                return false;
            }
        }

        #endregion
    }
}
