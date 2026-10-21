using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Common.BE;

namespace Zeospace.DeskPlus.Common.DAL
{
    public class Interface : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-07
        /// 작성목적 : 로그데이터 저장
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(BE.InterfaceInfo _info)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_COM_Interface_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID; //업체아이디
                    cmd.Parameters.Add("@ModuleSend", SqlDbType.NVarChar, 20).Value = _info.ModuleSend; //발송모듈
                    cmd.Parameters.Add("@SendTarget", SqlDbType.NVarChar, 20).Value = _info.SendTarget; //수신모듈
                    cmd.Parameters.Add("@SendID", SqlDbType.Int).Value = _info.SendID; //전자결재아이디
                    cmd.Parameters.Add("@TargetID", SqlDbType.Int).Value = _info.TargetID; //저장모듈아이디                    
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID; //등록자아이디
                    cmd.Parameters.Add("@WriteUserName", SqlDbType.NVarChar, 20).Value = _info.WriteUserName; //등록자이름
                    cmd.Parameters.Add("@StatusCode", SqlDbType.Char, 1).Value = _info.StatusCode; //상태
                    cmd.Parameters.Add("@StatusMessage", SqlDbType.NVarChar, 20).Value = _info.StatusMessage; //상태메세지                 
                    cn.Open();

                    ExecuteNonQuery(cmd);

                    return 1;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return 0;
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2015-10-19
        /// 작성목적 : 타겟 아이디 가져오기
        /// </summary>
        public DataTableReader GetTargetID(int _companyID, string _sendtarget, int _selectid, int _paratype)
        {
;
            using (SqlConnection cn = new SqlConnection(ConnSolution(_companyID)))
            {
                string query = "";

                if (_paratype == 1)
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Interface_InterfaceID");
                }
                else
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.COMMON, "QRY_COM_Interface_SendID");
                }

                SqlCommand cmd = new SqlCommand(query, cn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                cmd.Parameters.Add("@SendTarget", SqlDbType.Char).Value = _sendtarget;
                cmd.Parameters.Add("@SeleceID", SqlDbType.Char).Value = _selectid;

                cn.Open();

                return ExecuteDataTableReader(cmd);
            }
        }
    }
}
