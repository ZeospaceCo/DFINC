using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Transactions;
using System.Web;
using System.Xml;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.RM.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;

namespace Zeospace.DeskPlus.RM.BLL
{
    public class Reserve : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseBLL
    {
        #region ADD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-17
        /// 작성목적 : 자원 예약 (반복일정 0)
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_visit"></param>
        /// <returns></returns>
        public string Add(ReserveInfo _info, List<BE.ReserveEtcInfo> _al, RepeatInfo _repInfo, bool _visit, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int tmpVal = 0;
                string tmpReservationID = String.Empty;
                string tmpRtnVal = String.Empty;
                DateTime StartDate = DateTime.Now;
                int reservationID = 0;

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 1.자원예약 처리
                        DAL.Reverse dal = new DAL.Reverse();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentReservationID = tmpVal;

                        reservationID = dal.Add(_info);
                        //int reservationID = dal.Add(_info);
                        if (reservationID <= 0) return "-1";

                        tmpReservationID += reservationID + ",";

                        if (_info.ParentReservationID == 0)
                        {
                            tmpVal = reservationID;
                            StartDate = info.StartDate;
                        }

                        // 2. 예약자원 저장하기
                        DAL.ReverseList listdal = new DAL.ReverseList();
                        int reservationListID = listdal.Add(_info.CompanyID, reservationID, _info.ListID, _info.Qty);

                        if (reservationListID < 1)
                        {
                            return "-1";
                        }
                    }

                    // 3. 반복설정 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReservationID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return "-1";
                    }

                    //공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if(_suInfo != null)
                    {
                        if (!suBll.Add(_suInfo, tmpVal))
                        {
                            return "-1";
                        }
                    }
                                            

                    // 4. 첨부파일
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal, "0"))
                            {
                                return "-1";
                            }
                        }
                    }

                    //방문예약 제품일 경우,
                    if (_visit)
                    {
                        tmpRtnVal = tmpReservationID;
                    }
                    else
                    {
                        tmpRtnVal = tmpVal.ToString();
                    }

                    ts.Complete();
                }

                #region 알람발송                
                string strSection = (_info.Usable == "1") ? "ADD" : "REQUEST";
                SendAlarm(_info, reservationID, StartDate, strSection);
                #endregion

                return tmpRtnVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add(Repeat)", ex);
                return "-1";
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-20
        /// 작성목적 : 자원 예약  (반복일정 X)
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(ReserveInfo _info, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int reservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    #region 자원 저장하기

                    // 1.Reserve 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    reservationID = dal.Add(_info);

                    if (reservationID > 0)
                    {
                        // 2. ReserveList
                        DAL.ReverseList listdal = new DAL.ReverseList();
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

                    // 공유자 저장하기                    
                    BLL.ShareUser suBll = new ShareUser();
                    if(_suInfo != null)
                    {
                        if (!suBll.Add(_suInfo, reservationID))
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

                            if (!bll.Add(_info, reservationID, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    #endregion 자원 저장하기                    

                    ts.Complete();
                }
                                
                #region 알람발송
                string strSection = (_info.Usable == "1") ? "ADD" : "REQUEST";
                SendAlarm(_info, reservationID, _info.StartDate, strSection);
                #endregion

                return reservationID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Add", ex);
                return -1;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 자원 수정  (반복일정 X) - 이번일정만 / 반복일정 미 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_repeatCheck"></param>
        /// <returns></returns>
        public bool Mod(ReserveInfo _info, int _parentReservationID, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    int parentReservationID = dal.Mod(_info);

                    if (parentReservationID < 0)
                    {
                        return false;
                    }

                    // 2. ReserveList 테이블 처리
                    DAL.ReverseList listdal = new DAL.ReverseList();
                    if (!listdal.Mod(_info.CompanyID, _info.ReservationID, _info.ListID, _info.Qty))
                    {
                        return false;
                    }

                    // 3. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    if (parentReservationID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우
                        // 3-1.반복 테이블 삭제
                        if (!repdal.Del(_info.CompanyID, _info.ReservationID))
                        {
                            return false;
                        }

                        //공유자 삭제
                        if (!suDal.Del(_info.CompanyID, _parentReservationID))
                        {
                            return false;
                        }                       
                        
                    }
                    else
                    {
                        // 이미 지정되어 있는 다른 반복일정이 있을 경우
                        if (_parentReservationID != parentReservationID)
                        {
                            // 3-2.반복 테이블 업데이트
                            if (!repdal.Upd(_info.CompanyID, _parentReservationID, parentReservationID))
                            {
                                return false;
                            }

                            // 4. Attach 테이블 처리
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.Mod(_info.CompanyID, _parentReservationID, parentReservationID, _info.WriteDateYM))
                            {
                                return false;
                            }

                            // 5. 공유자 테이블 처리
                            if(!suDal.Upd(_info.CompanyID, _parentReservationID, parentReservationID))
                            {
                                return false;
                            }
                        }
                    }

                    //이번 일정 공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, _info.ReservationID))
                    {
                        return false;
                    }
                    

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, _info.ReservationID, "0"))
                            {
                                return false;
                            }
                        }
                    }

                    ts.Complete();
                }

                //알람발송
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, _info.ReservationID, _info.StartDate, strSection);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 자원 수정  (반복일정 0) - 이번일정만 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(ReserveInfo _info, List<BE.ReserveEtcInfo> _al, RepeatInfo _repInfo, int _parentReservationID, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int tmpVal = 0;
                DateTime StartDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / ReserveList / Repeat / Attach 테이블 변경 , 첨부파일 이동
                    int ParentReservationID = ModRepeat(_info.CompanyID, _info.ReservationID, _info.ParentReservationID, _info.WriteDateYM);
                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        DAL.Reverse dal = new DAL.Reverse();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentReservationID = tmpVal;

                        int reservationID = dal.Add(_info);
                        if (_info.ParentReservationID == 0)
                        {
                            tmpVal = reservationID;
                            StartDate = info.StartDate;
                        }

                        if (reservationID > 0)
                        {
                            // 3. ReserveList 테이블 처리
                            DAL.ReverseList listdal = new DAL.ReverseList();
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
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReservationID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }
                                        
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    if (ParentReservationID == 0)
                    {
                        //공유자 삭제                        
                        if (!suDal.Del(_info.CompanyID, _parentReservationID))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if(ParentReservationID != _parentReservationID)
                        {
                            if (!suDal.Upd(_info.CompanyID, _parentReservationID, ParentReservationID))
                            {
                                return -1;
                            }
                        }
                    }

                    //새로운 일정 공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, tmpVal))
                    {
                        return -1;
                    }
                    
                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리
                    if (ParentReservationID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _parentReservationID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ParentReservationID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }

                    ts.Complete();
                }

                //알람발송
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, tmpVal, StartDate, strSection);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 자원 수정  (반복일정 0) - 향후 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModFuture(ReserveInfo _info, List<BE.ReserveEtcInfo> _al, RepeatInfo _repInfo, int _parentReservationID, string _startDate, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int tmpVal = 0;
                DateTime StartDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve / ReserveList / Repeat
                    int ParentReservationID = ModRepeat(_info.CompanyID, _info.ParentReservationID, _startDate);
                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        DAL.Reverse dal = new DAL.Reverse();
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentReservationID = tmpVal;

                        int reservationID = dal.Add(_info);
                        if (_info.ParentReservationID == 0)
                        {
                            tmpVal = reservationID;
                            StartDate = info.StartDate;
                        }

                        if (reservationID > 0)
                        {
                            // 3. ReserveList 테이블 처리
                            DAL.ReverseList listdal = new DAL.ReverseList();
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
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReservationID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }                    
                    
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    if (ParentReservationID == 0)
                    {
                        //공유자 삭제                        
                        if (!suDal.Del(_info.CompanyID, _parentReservationID))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (ParentReservationID != _parentReservationID)
                        {
                            if (!suDal.Upd(_info.CompanyID, _parentReservationID, ParentReservationID))
                            {
                                return -1;
                            }
                        }
                    }

                    //현재or향후 일정 공유자 저장                    
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, tmpVal))
                    {
                        return -1;
                    }                    

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리
                    if (ParentReservationID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _parentReservationID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ParentReservationID, tmpVal, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }

                    ts.Complete();
                }

                //알람발송
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, tmpVal, StartDate, strSection);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFuture", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 자원 수정  (반복일정 0) - 모든 일정 / 반복일정 수정시
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public int ModAll(ReserveInfo _info, List<BE.ReserveEtcInfo> _al, RepeatInfo _repInfo, int _parentReservationID, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int tmpVal = 0;
                DateTime StartDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve / ReserveList / Repeat 삭제
                    DAL.Reverse dal = new DAL.Reverse();
                    if (!dal.DelAll(_info.CompanyID, _info.ParentReservationID))
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentReservationID = tmpVal;

                        int reservationID = dal.Add(_info);
                        if (_info.ParentReservationID == 0)
                        {
                            tmpVal = reservationID;
                            StartDate = info.StartDate;
                        }

                        if (reservationID > 0)
                        {
                            // 3. ReserveList 테이블 처리
                            DAL.ReverseList listdal = new DAL.ReverseList();
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
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReservationID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    //공유자 삭제
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    suDal.Del(_info.CompanyID, _parentReservationID);

                    //새로운 일정 공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, tmpVal))
                    {
                        return -1;
                    }                    

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리
                    BLL.Attach attbll = new BLL.Attach();
                    if (!attbll.ModRepeat(_info.CompanyID, _parentReservationID, tmpVal, _info.WriteDateYM))
                    {
                        return -1;
                    }

                    ts.Complete();
                }

                //알람발송
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, tmpVal, StartDate, strSection);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModAll", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 자원 수정  (반복일정 X) - 선택해서 들어간 날짜 이후로의 반복 일정은 삭제, 새로운 일정 등록
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public int ModFuture(ReserveInfo _info, int _parentReservationID, string _startDate, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int reservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve / ReserveList / Repeat
                    int ParentReservationID = ModRepeat(_info.CompanyID, _parentReservationID, _startDate);
                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    // 2. Reserve 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    reservationID = dal.Add(_info);

                    if (reservationID > 0)
                    {
                        // 3. ReserveList 테이블 처리
                        DAL.ReverseList listdal = new DAL.ReverseList();
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

                    //공유자 삭제
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    suDal.Del(_info.CompanyID, _parentReservationID);

                    //새로운 일정 공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, reservationID))
                    {
                        return -1;
                    }
                    


                    // 4. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, reservationID, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    // 5. 이전 첨부파일 처리
                    if (ParentReservationID == 0)
                    {
                        // 이전 다른 반복 일정이 없을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.ModRepeat(_info.CompanyID, _parentReservationID, reservationID, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        // 이전 다른 반복 일정이 있을 경우
                        BLL.Attach bll = new BLL.Attach();
                        if (!bll.Mod(_info.CompanyID, ParentReservationID, reservationID, _info.WriteDateYM))
                        {
                            return -1;
                        }
                    }

                    ts.Complete();
                }

                //알람발송
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, reservationID, _info.StartDate, strSection);

                return reservationID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModFuture2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-12
        /// 작성목적 : 자원 수정  (반복일정 X) - 반복설정이 첨부터 없었을 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_parentReservationID"></param>
        /// <returns></returns>
        public bool Mod(ReserveInfo _info, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    int parentReservationID = dal.Mod(_info);

                    if (parentReservationID < 0)
                    {
                        return false;
                    }

                    // 2. ReserveList 테이블 처리
                    DAL.ReverseList listdal = new DAL.ReverseList();
                    if (!listdal.Mod(_info.CompanyID, _info.ReservationID, _info.ListID, _info.Qty))
                    {
                        return false;
                    }

                    //공유자 삭제
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    suDal.Del(_info.CompanyID, _info.ReservationID);

                    //공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, _info.ReservationID))
                    {
                        return false;
                    }
                    

                    // 3. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, _info.ReservationID, "0"))
                            {
                                return false;
                            }
                        }
                    }

                    ts.Complete();
                }
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, _info.ReservationID, _info.StartDate, strSection);

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod3", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-14
        /// 작성목적 : 자원 수정  (반복일정 0) - 반복이 없었다가 생긴 경우
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_al"></param>
        /// <param name="_repInfo"></param>
        /// <returns></returns>
        public int Mod(ReserveInfo _info, List<BE.ReserveEtcInfo> _al, RepeatInfo _repInfo, List<BE.ShareUserInfo> _suInfo)
        {
            try
            {
                int tmpVal = 0;
                DateTime StartDate = DateTime.Now;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 이전 일정 처리
                    // 1. Reserve  삭제
                    DAL.Reverse dal = new DAL.Reverse();
                    if (!dal.Del(_info.CompanyID, _info.ReservationID))
                    {
                        return -1;
                    }

                    // 2. ReserveList 삭제
                    DAL.ReverseList redal = new DAL.ReverseList();
                    if (!redal.Del(_info.CompanyID, _info.ReservationID))
                    {
                        return -1;
                    }

                    // 새로운 일정 처리
                    foreach (BE.ReserveEtcInfo info in _al)
                    {
                        // 2. Reserve 테이블 처리
                        _info.StartDate = info.StartDate;
                        _info.EndDate = info.EndDate;
                        _info.DifferMinite = info.DifferMinite;
                        _info.ParentReservationID = tmpVal;

                        int reservationID = dal.Add(_info);
                        if (_info.ParentReservationID == 0)
                        {
                            tmpVal = reservationID;
                            StartDate = info.StartDate;
                        }

                        if (reservationID > 0)
                        {
                            // 3. ReserveList 테이블 처리
                            DAL.ReverseList listdal = new DAL.ReverseList();
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
                    }

                    // 4. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    _repInfo.ReservationID = tmpVal;
                    if (!repdal.Add(_repInfo))
                    {
                        return -1;
                    }

                    //공유자 삭제
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    suDal.Del(_info.CompanyID, _info.ReservationID);

                    //새로운 일정 공유자 저장
                    BLL.ShareUser suBll = new ShareUser();
                    if (!suBll.Add(_suInfo, tmpVal))
                    {
                        return -1;
                    }
                    

                    // 5. Attach 테이블 처리 & 파일 저장
                    if (_info.HiddenSpoolPath != "" && _info.HiddenSpoolPath != null)
                    {
                        HttpContext context = HttpContext.Current;
                        DirectoryInfo di = new DirectoryInfo(context.Server.MapPath(_info.HiddenSpoolPath));
                        if (di.Exists)
                        {
                            BLL.Attach bll = new BLL.Attach();

                            if (!bll.Add(_info, tmpVal, "0"))
                            {
                                return -1;
                            }
                        }
                    }

                    // 6. 이전 첨부파일 처리
                    BLL.Attach attbll = new BLL.Attach();
                    if (!attbll.ModRepeat(_info.CompanyID, _info.ReservationID, tmpVal, _info.WriteDateYM))
                    {
                        return -1;
                    }

                    ts.Complete();
                }
                string strSection = (_info.Usable == "1") ? "MOD" : "REQUEST";
                SendAlarm(_info, tmpVal, StartDate, strSection);

                return tmpVal;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Mod4", ex);
                return -1;
            }
        }

        #endregion MOD()

        #region DEL()

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 변경 (반복일정에서 빼기)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_parentReservationID"></param>
        /// <param name="_writeDateYM"></param>
        /// <returns></returns>
        public int ModRepeat(int _companyID, int _reservationID, int _parentReservationID, string _writeDateYM)
        {
            try
            {
                int ParentReservationID = 0;
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1. Reserve / ReserveList 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    ParentReservationID = dal.DelUpd(_companyID, _reservationID, _parentReservationID);

                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제
                        if (!repdal.Del(_companyID, _reservationID))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (_parentReservationID != ParentReservationID)
                        {
                            // 2-2. Repeat 테이블 업데이트
                            if (!repdal.Upd(_companyID, _parentReservationID, ParentReservationID))
                            {
                                return -1;
                            }

                            // 3. 첨부파일 처리
                            BLL.Attach bll = new BLL.Attach();
                            if (!bll.ModRepeat(_companyID, _parentReservationID, ParentReservationID, _writeDateYM))
                            {
                                return -1;
                            }                            
                        }
                    }

                    ts.Complete();
                }

                return ParentReservationID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-13
        /// 작성목적 : 예약 자원 변경 (반복일정에서 삭제) 향후 일정
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
                    DAL.Reverse dal = new DAL.Reverse();
                    ParentReservationID = dal.Del(_companyID, _parentReservationID, _startDate);

                    if (ParentReservationID < 0)
                    {
                        return -1;
                    }

                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 2-1. Repeat 테이블 삭제
                        if (!repdal.Del(_companyID, _parentReservationID))
                        {
                            return -1;
                        }
                    }

                    // ParentReservationID 값이 0보다 클때 굳이 값을 변경할 필요가 없음

                    ts.Complete();
                }

                return ParentReservationID;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ModRepeat2", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-02-23
        /// 작성목적 : 예약 자원 삭제
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
                    DAL.Reverse dal = new DAL.Reverse();
                    if (!dal.DelTotal(_companyID, _reservationID))
                    {
                        return false;
                    }

                    // 2. 첨부파일 처리
                    BLL.Attach bll = new BLL.Attach();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 예약 자원 삭제 (반복일정이 있었을 경우 - 이번 일정만 삭제)
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
                    DAL.Reverse dal = new DAL.Reverse();
                    int ParentReservationID = dal.DelUpd(_companyID, _reservationID, _parentReservationID);

                    if (ParentReservationID < 0)
                    {
                        return false;
                    }

                    // 2. Repeat 테이블 처리
                    DAL.Repeat repdal = new DAL.Repeat();
                    if (ParentReservationID == 0)
                    {
                        // 이미 지정되어 있는 다른 반복일정이 없을 경우
                        // 2-1.반복 테이블 삭제
                        if (!repdal.Del(_companyID, _reservationID))
                        {
                            return false;
                        }

                        // 3. 공유자 처리
                        DAL.ShareUser suDal = new DAL.ShareUser();
                        if (!suDal.Del(_companyID, _reservationID))
                        {
                            return false;
                        }

                        // 4. 첨부파일 처리
                        BLL.Attach bll = new BLL.Attach();
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
                            DAL.ShareUser suDal = new DAL.ShareUser();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del2", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 예약 자원 삭제 (반복일정이 있었을 경우 - 향후 일정만 삭제)
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
                    DAL.ShareUser suDal = new DAL.ShareUser();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del3", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-03-15
        /// 작성목적 : 예약 자원 삭제 (반복일정이 있었을 경우 - 모든 일정 삭제)
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
                    DAL.Reverse dal = new DAL.Reverse();
                    if (!dal.DelAll(_companyID, _parentReservationID))
                    {
                        return false;
                    }

                    // 2. 첨부파일 처리
                    BLL.Attach bll = new BLL.Attach();
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("DelAll", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2014-02-17
        /// 작성목적 : 예약 자원 삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_reservationID"></param>
        /// <returns></returns>
        public bool DelVisit(int _companyID, int _reservationID)
        {
            try
            {
                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    // 1.Reserve 테이블 처리
                    DAL.Reverse dal = new DAL.Reverse();
                    if (!dal.Del(_companyID, _reservationID))
                    {
                        return false;
                    }

                    // 2.ReserveList 테이블 처리
                    DAL.ReverseList listdal = new DAL.ReverseList();
                    if (!listdal.Del(_companyID, _reservationID))
                    {
                        return false;
                    }

                    ts.Complete();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Del", ex);
                return false;
            }
        }

        #endregion DEL()

        #region 알람
                
        public void SendAlarm(ReserveInfo _info, int _reservationID, DateTime _startDate, string _section)
        {
            try
            {
                // 코드관리로 처리
                DataView dvAlarm = AppSettingHelper.GetSysCode(_info.CompanyID, "AlarmResource");                
                if (dvAlarm.Count.Equals(0)) return;

                string userIDMESSENGER = "";    //메신져
                string userIDMOBILE = "";       //모바일
                string userIDMAIL = "";         //메일
                string userIDSMS = "";          //SMS
                ArrayList Receiver = new ArrayList();

                // 알람 설정이 있을 경우
                List<int> userList = new List<int>();

                #region 자원담당자
                if (_section == "REQUEST")
                {
                    //2. 자원예약 or 수정시에 알람발송 (해당 자원이 승인이 필요시에만 알람 발송)
                    // 자원담당자                        
                    Zeospace.DeskPlus.RM.DAL.ListUser userdal = new DAL.ListUser();
                    DataTableReader dtrUser = userdal.GetList(_info.CompanyID, _info.ListID);

                    if (dtrUser != null)
                    {
                        if (dtrUser.HasRows)
                        {
                            while (dtrUser.Read())
                            {
                                string AlarmRM = dtrUser["AlarmRM"].ToString();
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                if (AlarmRM.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                if (!Receiver.Contains(dtrUser["ManagerUserID"].ToString()))
                                {
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        userIDMESSENGER += dtrUser["ManagerUserID"].ToString() + ",";
                                    }

                                    userIDMOBILE += dtrUser["ManagerUserID"].ToString() + ",";

                                    userList.Add(dtrUser["ManagerUserID"].ToString().ToNumber<int>());

                                    if (AlarmMail.Equals("1"))
                                    {
                                        if (dtrUser["MailID"].ToString().Length > 0 && dtrUser["MailDomain"].ToString().Length > 0)
                                        {
                                            userIDMAIL += dtrUser["MailID"].ToString() + "@" + dtrUser["MailDomain"].ToString() + ",";
                                        }
                                    }

                                    if (AlarmSMS.Equals("1"))
                                    {
                                        if (dtrUser["MobilePhone"].ToString().Length > 0)
                                        {
                                            userIDSMS += dtrUser["MobilePhone"].ToString() + ",";
                                        }
                                    }
                                }
                            }
                        }
                    }                        
                }

                #endregion 자원담당자

                #region 자원예약자
                if (_section == "APPROVE" || _section == "REJECT")
                {
                    //1. 자원예약 승인 or 거부시 예약자 알람발송                        
                    Account.DAL.User uDAL = new Account.DAL.User();
                    List<Account.BE.UserInfo> writeUserList = uDAL.GetUserInfo(_info.CompanyID, _info.WriteUserID, true, true);

                    foreach (Account.BE.UserInfo info in writeUserList)
                    {
                        string AlarmRM = info.AlarmRM;
                        string AlarmMail = string.Empty;
                        string AlarmMessenger = string.Empty;
                        string AlarmMobile = string.Empty;
                        string AlarmSMS = string.Empty;

                        if (AlarmRM.IndexOf("1-").Equals(0))
                        {
                            AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                            AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                            AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                            AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                        }
                        else
                        {
                            AlarmMessenger = "0";
                            AlarmMail = "0";
                            AlarmMobile = "0";
                            AlarmSMS = "0";
                        }

                        //자원담당자와 동일하면 보내지 않는다
                        if (!userList.Contains(info.UserID.ToString().ToNumber<int>()))
                        {
                            if (AlarmMessenger.Equals("1"))
                            {
                                userIDMESSENGER += info.UserID.ToString() + ",";
                            }
                            
                            userIDMOBILE += info.UserID.ToString() + ",";

                            userList.Add(info.UserID);

                            if (AlarmMail.Equals("1"))
                            {
                                if (info.MailID.ToString().Length > 0 && info.MailDomain.ToString().Length > 0)
                                {
                                    userIDMAIL += info.MailID.ToString() + "@" + info.MailDomain.ToString() + ",";
                                }
                            }

                            if (AlarmSMS.Equals("1"))
                            {
                                if (info.MobilePhone.ToString().Length > 0)
                                {
                                    userIDSMS += info.MobilePhone.ToString() + ",";
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 공유자
                if ((_section == "MOD" && _info.Usable == "1") || _section == "APPROVE" || _section == "REJECT" || _section == "ADD")
                {
                    //1. 자원관리 등록or수정 이면서 해당 자원이 담당자 승인이 필요하지 않은경우(예약과동시에 승인)에 공유자 알람발송
                    //2. 자원 승인시 공유자 알람발송
                    //3. 자원예약 거부시 공유자 알람발송
                    //공유자                        
                    //reservationID
                    DAL.ShareUser suDal = new DAL.ShareUser();
                    DataTableReader dtrShareUser = suDal.GetShareUserListByID(_info.CompanyID, _reservationID);

                    if (dtrShareUser != null)
                    {
                        if (dtrShareUser.HasRows)
                        {
                            while (dtrShareUser.Read())
                            {
                                string AlarmRM = dtrShareUser["AlarmRM"].ToString();
                                string AlarmMail = string.Empty;
                                string AlarmMessenger = string.Empty;
                                string AlarmMobile = string.Empty;
                                string AlarmSMS = string.Empty;

                                if (AlarmRM.IndexOf("1-").Equals(0))
                                {
                                    AlarmMail = (AlarmRM.ToUpper().IndexOf("MAIL") < 0) ? "0" : "1";
                                    AlarmMessenger = (AlarmRM.ToUpper().IndexOf("MESSENGER") < 0) ? "0" : "1";
                                    AlarmMobile = (AlarmRM.ToUpper().IndexOf("MOBILE") < 0) ? "0" : "1";
                                    AlarmSMS = (AlarmRM.ToUpper().IndexOf("SMS") < 0) ? "0" : "1";
                                }
                                else
                                {
                                    AlarmMessenger = "0";
                                    AlarmMail = "0";
                                    AlarmMobile = "0";
                                    AlarmSMS = "0";
                                }

                                //자원담당자, 자원예약자와 동일하면 보내지 않는다
                                if (!userList.Contains(dtrShareUser["UserID"].ToString().ToNumber<int>()) && _info.WriteUserID != dtrShareUser["UserID"].ToString().ToNumber<int>())
                                {
                                    if (AlarmMessenger.Equals("1"))
                                    {
                                        userIDMESSENGER += dtrShareUser["UserID"].ToString() + ",";
                                    }

                                    userIDMOBILE += dtrShareUser["UserID"].ToString() + ",";

                                    userList.Add(dtrShareUser["UserID"].ToString().ToNumber<int>());

                                    if (AlarmMail.Equals("1"))
                                    {
                                        if (dtrShareUser["MailID"].ToString().Length > 0 && dtrShareUser["MailDomain"].ToString().Length > 0)
                                        {
                                            userIDMAIL += dtrShareUser["MailID"].ToString() + "@" + dtrShareUser["MailDomain"].ToString() + ",";
                                        }
                                    }

                                    if (AlarmSMS.Equals("1"))
                                    {
                                        if (dtrShareUser["MobilePhone"].ToString().Length > 0)
                                        {
                                            userIDSMS += dtrShareUser["MobilePhone"].ToString() + ",";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                #endregion         

                foreach (DataRowView dr in dvAlarm)
                {
                    string method = dr["CodeID"].ToString().ToUpper();
                    switch (method)
                    {
                        case "MAIL":
                            if (_info.Mail.Length > 0)
                            {
                                if (userIDMAIL.Length > 0)
                                {
                                    SendMail(_info, _reservationID, userIDMAIL, _startDate, _section);
                                }
                            }
                            break;

                        case "MESSENGER":
                            if (userIDMESSENGER.Length > 0)
                            {
                                foreach (string userID in userIDMESSENGER.Split(','))
                                {
                                    if (userID != "")
                                    {
                                        SendMessenger(_info, _reservationID, userID.ToNumber<int>(), _startDate, _section);
                                    }
                                }
                            }
                            break;
                    }
                }

                // 모바일사용의 경우, 무조건 입력
                if (AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "IsMobile", "Y").Equals("Y"))
                {
                    if (userIDMOBILE.Length > 0)
                    {
                        foreach (string userID in userIDMOBILE.Split(','))
                        {
                            if (userID != "")
                            {
                                SendMobile(_info, _reservationID, userID.ToNumber<int>(), _startDate, _section);
                            }
                        }
                    }
                }              
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("SendAlarm()", ex);
            }
        }

        /// <summary>
        /// 작성목적 : 메신저 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_section"></param>
        /// <returns></returns>
        public bool SendMessenger(BE.ReserveInfo _info, int _reservationID, int _receiverID, DateTime _startDate, string _section)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.ModifyUserID;
            alarmInfo.SenderName = _info.ModifyUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_info, _startDate, _info.PreLang, "MESSENGER", _section, "");
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = "";

            if (_section == "REQUEST")
            {
                alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmTargetURL", "");
                alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);
                alarmTargetURL = alarmTargetURL + "$t=rmBasePage|AdminReserve";
            }
            else
            {
                alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmViewTargetURL", "");
                alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("reservationID", _reservationID.ToString());

                HttpContext context = HttpContext.Current;
                string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                alarmTargetURL = alarmTargetURL + "?p=" + encparam;
            }

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 1;
            alarmInfo.KindSubject = "ResourceManager"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }

        /// <summary>
        /// 작성목적 : 모바일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_reservationID"></param>
        /// <param name="_receiverID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_section"></param>
        /// <returns></returns>
        public bool SendMobile(BE.ReserveInfo _info, int _reservationID, int _receiverID, DateTime _startDate, string _section)
        {
            Zeospace.DeskPlus.Common.BLL.Alarm biz = new Common.BLL.Alarm();
            AlarmInfo alarmInfo = new Common.BE.AlarmInfo();
            alarmInfo.CompanyID = _info.CompanyID;
            alarmInfo.SenderID = _info.ModifyUserID;
            alarmInfo.SenderName = _info.ModifyUserName;
            alarmInfo.SenderMail = "";
            alarmInfo.SenderPhone = "";
            alarmInfo.ReceiverID = _receiverID;
            alarmInfo.ReceiverName = "";
            alarmInfo.ReceiverMail = "";
            alarmInfo.ReceiverPhone = "";
            alarmInfo.Status = "";
            alarmInfo.SendDate = System.DateTime.Now.AddMinutes(1);

            string[] arrAlarm = AlarmTemplate(_info, _startDate, _info.PreLang, "MESSENGER", _section, "");
            if (arrAlarm == null) return false;

            alarmInfo.Subject = arrAlarm[0];
            alarmInfo.Content = arrAlarm[1];

            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            string alarmTargetURL = "";
            string alarmTargetMobileURL = "";

            if (_section == "REQUEST")
            {
                alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmTargetURL", "");
                alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);
                alarmTargetURL = alarmTargetURL + "$t=rmBasePage|AdminReserve";

                alarmTargetMobileURL = "/DeskPlusMobile/RM/UsableList.aspx?";
            }
            else
            {
                alarmTargetURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmViewTargetURL", "");
                alarmTargetURL = string.Format(alarmTargetURL, baseHttp, fullDomain);

                Dictionary<string, string> paraDic = new Dictionary<string, string>();
                paraDic.Add("reservationID", _reservationID.ToString());

                HttpContext context = HttpContext.Current;
                string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
                alarmTargetURL = alarmTargetURL + "?p=" + encparam;

                alarmTargetMobileURL = "/DeskPlusMobile/RM/AlarmView.aspx?p=" + encparam;
            }

            alarmInfo.TargetUrl = alarmTargetURL;
            alarmInfo.TargetMobileUrl = alarmTargetMobileURL;
            alarmInfo.StartDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.EndDate = System.DateTime.Now.AddMinutes(1);
            alarmInfo.WriteDate = System.DateTime.Now;
            alarmInfo.AlarmType = 4;
            alarmInfo.KindSubject = "ResourceManager"; //제목에 안들어가며, 구분자값임
            alarmInfo.ViewOption = "01";
            alarmInfo.Store = 1;
            alarmInfo.UserState = 0;

            return biz.Add(alarmInfo, AlarmEnum.SuSoft.ToString());
        }
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-04-01
        /// 작성목적 : 알람 내용 템플릿
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_startDate"></param>
        /// <param name="_preLang"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        private string[] AlarmTemplate(BE.ReserveInfo _info, DateTime _startDate, string _preLang, string _gubun, string _section, string _linkURL)
        {
            try
            {
                string ListName = _info.Title;

                if (string.IsNullOrEmpty(ListName))
                {
                    DAL.List dal = new DAL.List();

                    DataTableReader dtr = dal.GetListInfo(_info.CompanyID, _info.ListID);

                    if (dtr != null)
                    {
                        if (dtr.HasRows)
                        {
                            while (dtr.Read())
                            {
                                ListName = dtr["Title"].ToString().ToReverseXSS();
                            }
                        }
                    }
                }

                XmlDocument confirmXml = null;
                string xmlPath = FileHelper.GetServerMapPath("/DeskPlusEIP/RM/App_Data/" + _preLang + ".xml");
                confirmXml = new XmlDocument();
                confirmXml.Load(xmlPath);
                XmlNode tmpXmlNode = confirmXml.DocumentElement;

                string[] arrRtnval = new string[2];
                arrRtnval[0] = tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/SUBJ/" + _section).InnerText + _info.Subject.ToReverseXSS();
                arrRtnval[1] = (tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML") != null) ? System.Web.HttpUtility.HtmlDecode(tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/ContentHTML").InnerText) : "";
                arrRtnval[1] = arrRtnval[1].Replace("$SECTION$", tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/SECTION/" + _section).InnerText);          //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$ListName$", ListName);          //자원명
                arrRtnval[1] = arrRtnval[1].Replace("$SUBJECT$", _info.Subject.ToReverseXSS());          //문서제목
                arrRtnval[1] = arrRtnval[1].Replace("$Name$", _info.WriteUserName);          //예약자
                arrRtnval[1] = arrRtnval[1].Replace("$StartDate$", _startDate.ToString("yyyy-MM-dd")); //시작년월일

                if (_linkURL != "")
                {
                    string linkURL = tmpXmlNode.SelectSingleNode("/ALARM/" + _gubun + "/PAGELINK").InnerText.Replace("$LINKURL$", _linkURL);
                    arrRtnval[1] = arrRtnval[1] + linkURL;
                }

                return arrRtnval;   // [0] : Subj / [1] : Content
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("RM.AlarmTemplate : " + ex);
                throw;
            }
        }

 
        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-06-22
        /// 작성목적 : 메일 알람
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_mailReceiver"></param>
        /// <param name="_startDate"></param>
        /// <returns></returns>
        public bool SendMail(BE.ReserveInfo _info, int _reservationID, string _mailReceiver, DateTime _startDate, string _section)
        {
            // 받는 사람
            Hashtable htMailTo = new Hashtable();

            _mailReceiver = _mailReceiver.TrimEnd(',');

            foreach (string mail in _mailReceiver.Split(','))
            {
                if (!htMailTo.ContainsKey(mail))
                {
                    htMailTo.Add(mail, mail);
                }                
            }


            string linkURL = string.Empty;

            /*
            string baseHttp = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "BaseHttp", "");
            string fullDomain = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "FullDomain", "");
            linkURL = AppSettingHelper.GetAppSetting(AppSettingType.RESOURCEMANAGER, "AlarmViewTotalTargetURL", "");
            linkURL = string.Format(linkURL, baseHttp, fullDomain);            

            Dictionary<string, string> paraDic = new Dictionary<string, string>();
            paraDic.Add("companyID", _info.CompanyID.ToString());
            paraDic.Add("reservationID", _reservationID.ToString());

            HttpContext context = HttpContext.Current;            
            string encparam = Utility.EncryptString(context.Server.UrlEncode(Utility.GetParameterDic(paraDic)));
            linkURL = linkURL + "?p=" + encparam;
            */

            MailDataInfo mailInfo = new MailDataInfo();
            mailInfo.To = htMailTo;            
            mailInfo.From = _info.Mail;

            string[] arrAlarm = AlarmTemplate(_info, _startDate, _info.PreLang, "MAIL", _section, linkURL);
            if (arrAlarm == null) return false;

            mailInfo.Subject = arrAlarm[0];
            mailInfo.Contents = arrAlarm[1];

            SendMail sendMail = new SendMail();
            return sendMail.Send(mailInfo);
        }
        


        #endregion 알람
    }
}