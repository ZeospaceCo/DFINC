using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Zeospace.DeskPlus.RM.BE;
using System.Transactions;
using System.Web;
using System.IO;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.RM.BLL
{
    public class List : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region GET()
        #endregion

        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-09
        /// 작성목적 : 자원 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <returns></returns>
        public int Add(ListInfo _info, List<BE.ListUserInfo> _al)
        {
            try
            {
                int listID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {  
                    // 1. List 테이블 처리
                    DAL.List dal = new DAL.List();
                    listID = dal.Add(_info);

                    if (listID > 0)
                    {
                        // 2. ListUser 테이블 처리 
                        foreach (BE.ListUserInfo info in _al)
                        {
                            DAL.ListUser dalUser = new DAL.ListUser();
                            info.ListID = listID;
                            if (!dalUser.Add(info))
                            {
                                return -1;
                            }
                        }                        
                        
                        // 3. Attach 테이블 처리 & 파일 저장
                        if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.Attach bll = new BLL.Attach();
                                
                                if (!bll.Add(_info, listID, "1"))
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
                }

                return listID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion 

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-22
        /// 작성목적 : 자원 수정
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <returns></returns>
        public bool Mod(ListInfo _info, List<BE.ListUserInfo> _al)
        {
            try
            {
                bool rtnVal = true;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. List 테이블 처리
                    DAL.List dal = new DAL.List();

                    if (dal.Mod(_info))
                    {
                        // 2. ListUser 테이블 처리 
                        DAL.ListUser dalUser = new DAL.ListUser();

                        // 2-1 . 테이블 값 삭제 
                        dalUser.Del(_info.CompanyID, _info.ListID);

                        // 2-2 . 테이블 값 넣기 
                        foreach (BE.ListUserInfo info in _al)
                        {                            
                            if (!dalUser.Add(info))
                            {
                                return false;
                            }
                        }   

                        // 2. Attach 테이블 처리 & 파일 저장
                        if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                        {
                            HttpContext context = HttpContext.Current;
                            DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                            if (di.Exists)
                            {
                                BLL.Attach bll = new BLL.Attach();

                                if (!bll.Add(_info, _info.ListID, "1"))
                                {
                                    return false;
                                }
                            }
                        }

                        // 3. QRCode 삭제 처리
                        if ((_info.QRCodeUse == "0") && (_info.QRCodePath != ""))
                        {
                            HttpContext context = HttpContext.Current;

                            if (File.Exists(context.Server.MapPath(_info.QRCodePath)))
                            {
                                File.Delete(context.Server.MapPath(_info.QRCodePath));
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }


        #endregion 

        #region DEL()
        #endregion
    }
}
