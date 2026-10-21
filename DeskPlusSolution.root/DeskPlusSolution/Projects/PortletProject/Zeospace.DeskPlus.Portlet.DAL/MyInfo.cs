using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class MyInfo : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 가져오기

        /// <summary>
        /// 작성자  : shine
        /// 작성일자 : 2011-02-11
        /// 작성목적 : 나의정보 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <param name="_rootDeptID">The _root dept ID.</param>
        /// <returns></returns>
        public DataTableReader GetMyInfo(int _companyID, int _userID, int _rootDeptID, int _baseDept)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_MyInfo_Sel");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@RootDeptID", SqlDbType.Int).Value = _rootDeptID;
                    cmd.Parameters.Add("@BaseDept", SqlDbType.Int).Value = _baseDept;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : freeman
        /// 작성일자 : 2012.05.02
        /// 작성목적 : 일정관리, 전자결재 건수 정보
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_deptID">The _deptID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetCountList(int _companyID, int _deptID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    SqlCommand cmd = new SqlCommand("DP_PT_MyInfo_SelUserID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 작성자  : freeman
        /// 작성일자 :  2012-05-02
        /// 작성목적 : 최신메일 카운트 (제타 6.x)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <param name="_mailDomain">The _mail domain.</param>
        /// <returns></returns>
        public DataTableReader GetMailCount(int _companyID, string _mailID, string _mailDomain)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_MyInfo_SelZettaMail");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar).Value = _mailID + "@" + _mailDomain; //6.5 멀티컴패니는 메일도메인도 같이 넘겨야함

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null; //오류가 나더라도 메일 카운트만 -1 으로 됨
                //throw;
            }
        }

        /// <summary>
        /// 작성자  : shimpark
        /// 작성일자 :  2012-05-02
        /// 작성목적 : 최신메일 카운트 (제타 5.2)
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_mailID">The _mail ID.</param>
        /// <returns></returns>
        public DataTableReader GetMailCount52(int _companyID, string _mailID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_MyInfo_SelZettaMail52");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar).Value = _mailID;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null; //오류가 나더라도 메일 카운트만 -1 으로 됨
                //throw;
            }
        }

        /// <summary>
        /// 제우스 사용자 메일 카운트 가져오기 (메일id 같은 사람이 있으므로 도메인까지 넘겨야 함)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_mailID"></param>
        /// <param name="_mailDomain"></param>
        /// <returns></returns>
        public DataTableReader GetMailCount52_Zeus(int _companyID, string _mailID, string _mailDomain)
        {
            try
            {
                using (var cn = new SqlConnection(ConnMAIL(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.PORTLET, "QRY_PORTLET_MyInfo_SelZettaMail52_Zeus");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@MailID", SqlDbType.VarChar).Value = _mailID;
                    cmd.Parameters.Add("@MailDomain", SqlDbType.VarChar).Value = _mailDomain;

                    cn.Open();

                    return ExecuteDataTableReader(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null; //오류가 나더라도 메일 카운트만 -1 으로 됨
                //throw;
            }
        }

        #endregion 가져오기
    }
}