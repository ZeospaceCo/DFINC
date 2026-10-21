using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Transactions;
using System.Data;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.DAL;

namespace Zeospace.DeskPlus.Common.BLL
{
    public class Interface
    {
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-07
        /// 작성목적 : 로그데이터 저장
        /// </summary>
        public int AddLogData(InterfaceInfo _InterfaceInfo, int _targerid)
        {            
            try
            {
                int response = 1;

                //인터페이스 로그 저장 호출
                string statuscode = "";
                string statusmessage = "";

                if (_targerid == -1)
                {
                    statuscode = "0";
                    statusmessage = "실패";
                }
                else
                {
                    statuscode = "1";
                    statusmessage = "성공";
                }

                _InterfaceInfo.TargetID = _targerid; //저장모듈아이디 : 필수
                _InterfaceInfo.StatusCode = statuscode; //상태 : 기본값 1
                _InterfaceInfo.StatusMessage = statusmessage; //상태메세지 : 기본값 성공

                using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                {
                    DAL.Interface InterfaceDal = new DAL.Interface();
                    response = InterfaceDal.Add(_InterfaceInfo);

                    if (_targerid == -1)
                    {
                        response = -1;
                    }

                    ts.Complete();
                }

                return response;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("AddLogData", ex);
                return -1;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-19
        /// 작성목적 : 타겟 아이디 가져오기
        /// </summary>
        public DataTableReader GetTargetID(int _companyID, string _sendtarget, int _selectid, int _paratype)
        {
            DataTableReader response;

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
                DAL.Interface InterfaceDal = new DAL.Interface();
                response = InterfaceDal.GetTargetID(_companyID, _sendtarget, _selectid, _paratype);

                ts.Complete();
            }

            return response;
        }
    }
}
