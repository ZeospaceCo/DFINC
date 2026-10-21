using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zeospace.DeskPlus.Community.BE;
using Zeospace.DeskPlus.Common.BE;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.IO;
using System.Web;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.Community.BLL
{
    public class Boa : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-23
        /// 작성목적 : 게시물 추가
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <param name="_yearMonth">The _year month.</param>
        /// <returns></returns>
        public int Add(BE.BoaInfo _info)
        {
            if (_info == null) return -1;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.Boa dal = new DAL.Boa();
                int boardID = dal.Add(_info);

                #region 첨부파일 추가
                if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                {
                    HttpContext context = HttpContext.Current;
                    DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));

                    if (di.Exists)
                    {
                        BLL.BoaAttach attach = new BoaAttach();
                        string yearMonth = FileHelper.GetYearMonth();
                        attach.Add(_info.CompanyID, _info.CommunityID, _info.MenuID, boardID, _info.ScreenShot, _info.HiddenSpoolPath, yearMonth);
                    }
                }
                #endregion

                ts.Complete();
                return boardID;
            }
        }
        #endregion

        #region DEL()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-05-29
        /// 작성목적 : 게시물 삭제
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool DelAll(BoaInfo _info)
        {
            try
            {
                if (_info == null)
                {
                    return false;
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        // 1. 첨부파일 삭제 할 실제 파일 정보 가져오기
                        DAL.BoaAttach attachDal = new DAL.BoaAttach();
                        DataTableReader dtr = attachDal.GetAttachFilePathAll(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);

                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(dtr["FilePath"].ToString(), false);
                                }
                            }
                        }

                        // 2. 내용삭제
                        DAL.Boa boaDal = new DAL.Boa();
                        boaDal.DelAll(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);                      

                        ts.Complete();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("[Del] : " + ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-07-28
        /// 작성목적 : 답변글 삭제
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Del(BoaInfo _info)
        {
            try
            {
                if (_info == null)
                {
                    return false;
                }
                else
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        // 1. 첨부파일 삭제 할 실제 파일 정보 가져오기
                        DAL.BoaAttach attachDal = new DAL.BoaAttach();
                        DataTableReader dtr = attachDal.GetAttachFilePath(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);

                        if (dtr != null)
                        {
                            if (dtr.HasRows)
                            {
                                while (dtr.Read())
                                {
                                    Base.ApplicationFrameworks.Helper.FileHelper.DelFolder(dtr["FilePath"].ToString(), false);
                                }
                            }
                        }

                        // 2. 내용삭제
                        DAL.Boa boaDal = new DAL.Boa();
                        boaDal.Del(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID);

                        ts.Complete();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("[Del] : " + ex);
                return false;
            }
        }
        #endregion

        #region MOD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 201-06-04
        /// 작성목적 : 게시물 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_screenGubun"></param>
        /// <returns></returns>
        public bool Mod(BE.BoaInfo _info, string _screenGubun)
        {
            try
            {
                if (_info != null)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        DAL.Boa dal = new DAL.Boa();
                        dal.Mod(_info, _screenGubun);

                        #region 첨부파일 추가
                        
                        if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.BoaAttach attachDal = new BoaAttach();
                                attachDal.Add(_info.CompanyID, _info.CommunityID, _info.MenuID, _info.BoardID, _info.ScreenShot, _info.HiddenSpoolPath, _info.WriteDate);
                            }
                        }

                        #endregion

                        ts.Complete();

                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("[Del] : " + ex);
                return false;
            }
        }
        #endregion
    }
}
