using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.RM.Ws.BE;
using Zeospace.DeskPlusMobile.Web.RM.Ws.Data;
using System.Data;
using System.Xml;
using System.Transactions;

namespace Zeospace.DeskPlusMobile.Web.RM.Ws.BLL
{
    public class Reserve
    {
        /// <summary>
        /// 자원예약
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ReserveInfo _info)
        {
            try
            {
                int reservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve 테이블 처리
                    Data.Reserve dal = new Data.Reserve();
                    reservationID = dal.Add(_info);

                    if (reservationID > 0)
                    {
                        // 2. ReserveList 테이블 처리
                        Data.ReserveList listdal = new ReserveList();
                        int reservationListID = listdal.Add(_info.CompanyID, reservationID, _info.ListID, _info.Qty);

                        if (reservationListID < 1)
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }

                    ts.Complete();
                }
                return reservationID;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        /// <summary>
        /// 예약 자원 삭제 (반복일정이 있었을 경우 - 이번 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_writeYM"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID, int _parentReservationID, string _writeYM)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve / ReserveList 테이블 처리
                    Data.Reserve dal = new Data.Reserve();
                    int ParentReservationID = dal.DelUpd(_companyID, _reservationID, _parentReservationID);

                    if (ParentReservationID < 0)
                    {
                        return false;
                    }

                    // 2. Repeat 테이블 처리
                    Data.Repeat repdal = new Data.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우
                        // 2-1.반복 테이블 삭제
                        if (!repdal.Del(_companyID, _reservationID))
                        {
                            return false;
                        }

                        // 3. 공유자 처리
                        Data.ShareUser suDal = new Data.ShareUser();
                        if (!suDal.Del(_companyID, _reservationID))
                        {
                            return false;
                        }

                        // 4. 첨부파일 처리
                        BLL.Attach bll = new Attach();
                        if (!bll.Del(_companyID, _reservationID))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // 이미 지정되어 있는 다른 반복일정이 있을 경우
                        if (_parentReservationID != ParentReservationID)
                        {
                            // 2-2.반복 테이블 업데이트
                            if (!repdal.Upd(_companyID, _parentReservationID, ParentReservationID))
                            {
                                return false;
                            }

                            // 3. 공유자 처리
                            Data.ShareUser suDal = new Data.ShareUser();
                            if (!suDal.Upd(_companyID, _parentReservationID, ParentReservationID))
                            {
                                return false;
                            }

                            // 4. Attach 테이블 처리
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.ModRepeat(_companyID, _parentReservationID, ParentReservationID, _writeYM))
                            {
                                return false;
                            }

                        }
                    }

                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Del", ex);
                return false;
            }
        }

        /// <summary>
        /// 예약 자원 삭제 (반복일정이 있었을 경우 - 향후 일정만 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_writeYM"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID, int _parentReservationID, string _writeYM, string _startDate)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / ReserveList / Repeat / Reject 테이블 처리
                    int ParentReservationID = ModRepeat(_companyID, _parentReservationID, _startDate);
                    if (ParentReservationID < 0)
                    {
                        return false;
                    }

                    // 2. 공유자 처리                    
                    Data.ShareUser suDal = new Data.ShareUser();
                    if (!suDal.Del(_companyID, _reservationID))
                    {
                        return false;
                    }

                    // 3. 첨부파일 처리
                    if (ParentReservationID == 0)
                    {
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Del(_companyID, _reservationID))
                        {
                            return false;
                        }
                    }

                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Del", ex);
                return false;
            }
        }

        /// <summary>
        /// 예약 자원 삭제 (반복일정이 있었을 경우 - 모든 일정 삭제)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool DelAll(int _companyID, int _parentReservationID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve , ReserveList, Repeat , ShareUser, Reject 테이블 삭제
                    Data.Reserve dal = new Data.Reserve();
                    if (!dal.DelAll(_companyID, _parentReservationID))
                    {
                        return false;
                    }


                    // 2. 첨부파일 처리
                    BLL.Attach bll = new Attach();
                    if (!bll.Del(_companyID, _parentReservationID))
                    {
                        return false;
                    }

                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("DelAll", ex);
                return false;
            }
        }

        /// <summary>
        /// 예약 자원 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _reservationID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. DB 처리
                    Data.Reserve dal = new Data.Reserve();
                    if (!dal.DelTotal(_companyID, _reservationID))
                    {
                        return false;
                    }

                    // 2. 첨부파일 처리
                    BLL.Attach bll = new Attach();
                    if (!bll.Del(_companyID, _reservationID))
                    {
                        return false;
                    }

                    ts.Complete();
                }
                return true;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Del", ex);
                return false;
            }
        }


        /// <summary>
        /// 예약자원 변경 (반복일정에서 삭제) 향후 일정.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _parentReservationID, string _startDate)
        {
            try
            {
                int ParentReservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / ReserveList / Reject 테이블 처리
                    Data.Reserve dal = new Data.Reserve();
                    ParentReservationID = dal.Del(_companyID, _parentReservationID, _startDate);

                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리
                    Data.Repeat repdal = new Data.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제
                        if (!repdal.Del(_companyID, _parentReservationID))
                        {
                            return -1;
                        }
                    }

                    ts.Complete();
                }
                return ParentReservationID;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("ModRepeat", ex);
                return -1;
            }
        }
    }
}