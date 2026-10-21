using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Security;
using System.Transactions;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Account.BLL;
using System.Drawing;
using System.IO;


namespace Zeospace.DeskPlus.DeskPlusWS.Web
{
    public class SetPersonManage : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region 직급/직책관련
        /// <summary>
        /// 직급/직책 코드가 존재하는지를 반환한다.
        /// </summary>
        /// <param name="_interfaceCode"></param>
        /// <returns></returns>
        public int GetCodeID(string _interfaceCode, int _companyID)
        {
            try
            {
                string qry = @"
SELECT codeID FROM DP_COM_Code WHERE interfacecode = '{0}' and companyID = {1} ";
                qry = string.Format(qry, _interfaceCode, _companyID);
                WsException.WriteAssert("IsCode : " + qry, AppSettingType.APPROVAL);

                int codeID = 0;
                DataSet ds = new DataSet();
                using (var cn = new SqlConnection(ConnSolution(1)))
                {                    
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ds = ExecuteDataSet(cmd, "GetInerfaceCode");                    
                }

                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        codeID = dr["CodeID"].ToString().ToNumber<int>();
                    }
                }

                return codeID;

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsCode(): " + errMsg);
                return -1;
            }

        }

        /// <summary>
        /// 직급/직책 관련 임시테이블을 삭제한다.
        /// </summary>
        public void DelTempCode()
        {
            try
            {
                string qry = @"delete COM_Codes_Temp WHERE CompanyID = 1 ";

                WsException.WriteAssert("DelTempCode : " + qry, AppSettingType.APPROVAL);


                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelTempCode(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 직급/직책 관련 임시테이블을 입력한다.
        /// </summary>
        public void SetTempCode(string _interfaceCode)
        {
            try
            {
                string qry = @"insert into COM_Codes_Temp (interfacecode, CompanyID) values('{0}', 1)";

                qry = string.Format(qry, _interfaceCode);

                WsException.WriteAssert("SetTempCode : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetTempCode(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 직급/직책 코드중 임시테이블에 없는 데이터 삭제
        /// </summary>
        public void DelModifyValueCode()
        {
            try
            {
                //                string qry = @"update cmn_codes set activecode = '0'
                //where interfacecode is not null
                //   and interfacecode not in (select interfacecode from COM_Codes_Temp)";

                string qry = @"delete DP_COM_Code
where interfacecode is not null and interfacecode <> '' and companyID = 1  
   and interfacecode not in (select interfacecode from COM_Codes_Temp where companyID = 1)";

                WsException.WriteAssert("DelModifyValueCode : " + qry, AppSettingType.APPROVAL);


                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelModifyValueCode(): " + errMsg);
                throw;
            }
        }

        #endregion

        #region 부서관련
        /// <summary>
        /// 부서 코드가 존재하는지를 반환한다.
        /// 존재하면 부서 ID 를 반환한다.
        /// </summary>
        /// <param name="_interfaceCode"></param>
        /// <returns></returns>
        public int IsDept(string _interfaceCode, int _companyID)
        {
            try
            {
                string qry = @"
SELECT deptid FROM DP_ACC_Dept WHERE deptcode = '{0}' and companyID = {1} ";
                qry = string.Format(qry, _interfaceCode, _companyID);
                WsException.WriteAssert("IsDept : " + qry, AppSettingType.ACCOUNT);

                int rtCnt;

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    rtCnt = (obj != null) ? ExecuteScalar(cmd).ToString().ToNumber<int>() : 0;
                }

                return rtCnt;

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsDept(): " + errMsg);
                return 0;
            }

        }

        /// <summary>
        /// 부서 관련 임시테이블을 삭제한다.
        /// </summary>
        public void DelTempDept()
        {
            try
            {
                string qry = @"delete ACC_Depts_Temp WHERE CompanyID = 1 ";

                WsException.WriteAssert("DelTempDept : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelTempDept(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 부서 관련 임시테이블을 입력한다.
        /// </summary>
        public void SetTempDept(string _interfaceCode)
        {
            try
            {
                string qry = @"insert into ACC_Depts_Temp (interfacecode, CompanyID) values('{0}', 1)";

                qry = string.Format(qry, _interfaceCode);

                WsException.WriteAssert("SetTempDept : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetTempDept(): " + errMsg);
                throw;
            }

        }
        

        /// <summary>
        /// interfacecode 받아서 deptid 를 리턴한다..
        /// </summary>
        /// <param name="_companyid"></param>
        /// <param name="_parentInterfaceCode"></param>
        /// <returns></returns>
        public int GetDeptID(string _companyid, string _InterfaceCode)
        {
            try
            {
                string qry = @"SELECT deptid FROM DP_ACC_Dept WHERE deptcode = '{0}' and companyid = {1} and deptcode is not null and deptcode <> '' ";
                qry = string.Format(qry, _InterfaceCode, _companyid);
                WsException.WriteAssert("IsDept : " + qry, AppSettingType.APPROVAL);

                int rtCnt;

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    rtCnt = (obj != null) ? ExecuteScalar(cmd).ToString().ToNumber<int>() : 0;
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsDept(): " + errMsg);
                return 0;
            }
        }

        /// <summary>
        /// DeptID를 필터링 하기 위해 담아 놓는다.
        /// </summary>
        /// <param name="_companyid"></param>
        /// <returns></returns>
        public DataSet GetDeptAll(string _companyid)
        {
            try
            {
                string qry = @"SELECT deptid, deptcode FROM DP_ACC_Dept WHERE companyid = {0} ";
                qry = string.Format(qry, _companyid);
                //WsException.WriteAssert("GetDeptAll : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "acc_depts");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetDeptAll(): " + errMsg);
                return null;
            }
        }


        /// <summary>
        /// interfacecode 받아서 CodeID 를 리턴한다..
        /// </summary>
        /// <param name="_companyid"></param>
        /// <param name="_parentInterfaceCode"></param>
        /// <returns></returns>
        public int GetCodeID(string _companyid, string _InterfaceCode)
        {
            try
            {
                string qry = @"SELECT codeid FROM DP_COM_Code WHERE interfacecode = '{0}' and companyid = {1} ";
                qry = string.Format(qry, _InterfaceCode, _companyid);
                WsException.WriteAssert("GetCodeID : " + qry, AppSettingType.APPROVAL);


                int rtCnt;

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    rtCnt = (obj != null) ? ExecuteScalar(cmd).ToString().ToNumber<int>() : 0;
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetCodeID(): " + errMsg);
                return 0;
            }
        }

        public DataSet GetCodeAll(string _companyid)
        {
            try
            {
                string qry = @"SELECT * FROM DP_COM_Code where companyid = {0} ";
                qry = string.Format(qry, _companyid);
                //WsException.WriteAssert("GetCodeAll : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "cmn_codes");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetCodeAll(): " + errMsg);
                return null;
            }
        }

        /// <summary>
        /// DeptID를 필터링 하기 위해 담아 놓는다.
        /// </summary>
        /// <param name="_companyid"></param>
        /// <returns></returns>
        public DataSet GetUserDeptAll(string _companyid)
        {
            try
            {
                string qry = @"SELECT * FROM DP_ACC_UserDept WITH(NOLOCK) WHERE companyid = {0} ";
                qry = string.Format(qry, _companyid);
                //WsException.WriteAssert("GetDeptAll : " + qry, AppSettingType.APPROVAL);
                
                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "acc_userdept");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetUserDeptAll(): " + errMsg);
                return null;
            }
        }

        public void ModUserDeptOrder(int userID, int deptID, int UserDeptOrder)
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_UserDept SET UserDeptOrder = {3}
WHERE companyid = {0} AND UserID = {1} AND DeptID = {2}
";

                qry = string.Format(qry, 1, userID.ToString(), deptID.ToString(), UserDeptOrder.ToString());
                WsException.WriteAssert("ModUserDeptOrder : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModUserDeptOrder(): " + errMsg);
                throw;
            }
        }
        

        /// <summary>
        /// 부서 코드중 임시테이블에 없는 데이터 삭제 우선 ActiveCode 유무만을 0 으로 세팅한다.
        /// </summary>
        public void DelModifyValueDept()
        {
            try
            {
                string qry = @"update DP_ACC_Dept set activedept = '0'
where deptcode is not null and deptcode <> '' and companyID = 1
   and deptcode not in (select interfacecode from ACC_Depts_Temp where companyID = 1)";

                WsException.WriteAssert("DelModifyValueDept : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelModifyValueDept(): " + errMsg);
                throw;
            }
        }
        
        /// <summary>
        /// ERP에서 넘어온 DeptOrder로 업데이트
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptOrder"></param>
        public void ModDeptOrder(int deptID, int deptOrder)
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_Dept SET DeptOrder = {1}
WHERE DeptID = {0}
";

                qry = string.Format(qry, deptID.ToString(), deptOrder.ToString());
                WsException.WriteAssert("ModDeptOrder : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModDeptOrder(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// ERP에서 넘어온 CodeOrder로 업데이트
        /// </summary>
        /// <param name="codeID"></param>
        /// <param name="codeOrder"></param>
        public void ModCodeOrder(int codeID, int codeOrder)
        {
            try
            {
                string qry = @"
UPDATE DP_COM_Code SET CodeOrder = {1}
WHERE CodeID = {0}
";

                qry = string.Format(qry, codeID.ToString(), codeOrder.ToString());
                WsException.WriteAssert("ModCodeOrder : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModCodeOrder(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// deptid = parentdeptid 일 경우; ParentDeptID 를 null 처리
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptOrder"></param>
        public void ModParentDeptID()
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_Dept SET ParentDeptID = null
WHERE DeptID = ParentDeptID OR ParentDeptID = 0 
";

                WsException.WriteAssert("ModParentDeptID : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModParentDeptID(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// ParentDeptID 가 최상위 부서일 경우, RootDept = 1 처리
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptOrder"></param>
        public void ModRootDept()
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_Dept SET RootDept = '1'
WHERE ParentDeptID IS NULL
";

                WsException.WriteAssert("ModRootDept : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModRootDept(): " + errMsg);
                throw;
            }
        }
        #endregion

        #region 사용자관련
        /// <summary>
        /// 사용자 코드가 존재하는지를 반환한다.
        /// 존재하면 사용자 ID 를 반환한다.
        /// </summary>
        /// <param name="_interfaceCode"></param>
        /// <returns></returns>
        public int IsUser(string _interfaceCode)
        {
            try
            {
                string qry = @"SELECT userid FROM DP_ACC_User WHERE usercode = '{0}'";
                qry = string.Format(qry, _interfaceCode);
                WsException.WriteAssert("IsUser : " + qry, AppSettingType.APPROVAL);


                int rtCnt;

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    rtCnt = (obj != null) ? ExecuteScalar(cmd).ToString().ToNumber<int>() : 0;
                }

                return rtCnt;

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsUser(): " + errMsg);
                return 0;
            }

        }

        public DataSet IsUserAll(string _companyid)
        {
            try
            {
                string qry = @"SELECT userid, usercode, activeuser, loginid FROM DP_ACC_User where companyid = {0} ";
                qry = string.Format(qry, _companyid);
                //WsException.WriteAssert("IsUserAll : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "DP_ACC_User");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsUserAll(): " + errMsg);
                return null;
            }

        }

        public DataSet GetUserInfo(int _companyID, int _userID)
        {
            try
            {
                string qry = @"SELECT * FROM DP_ACC_User WHERE CompanyID = {0} AND UserID = {1}";
                qry = string.Format(qry, _companyID, _userID);
                //WsException.WriteAssert("GetUserInfo : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "DP_ACC_User");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetUserInfo(): " + errMsg);
                return null;
            }

        }


        /// <summary>
        /// 사용자 코드가 존재하는지를 반환한다.
        /// 존재하면 사용자 ID 를 반환한다. EP 용
        /// </summary>
        /// <param name="_interfaceCode"></param>
        /// <returns></returns>
        public int IsUserEP(string _interfaceCode)
        {
            try
            {
                string qry = @"SELECT userid FROM DP_ACC_User WHERE chinesename = '{0}' and  chinesename is not null";
                qry = string.Format(qry, _interfaceCode);
                WsException.WriteAssert("IsUser : " + qry, AppSettingType.APPROVAL);


                int rtCnt;

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    rtCnt = (obj != null) ? ExecuteScalar(cmd).ToString().ToNumber<int>() : 0;
                }

                return rtCnt;

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("IsUser(): " + errMsg);
                return 0;
            }

        }

        /// <summary>
        /// 사용자 관련 임시테이블을 삭제한다.
        /// </summary>
        public void DelTempUser()
        {
            try
            {
                string qry = @"delete ACC_User_Temp WHERE CompanyID = 1";

                WsException.WriteAssert("DelTempUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelTempUser(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 사용자 관련 임시테이블을 입력한다.
        /// </summary>
        public void SetTempUser(string _interfaceCode)
        {
            try
            {
                string qry = @"insert into ACC_User_Temp (interfacecode, CompanyID) values('{0}', 1)";

                qry = string.Format(qry, _interfaceCode);

                WsException.WriteAssert("SetTempUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetTempUser(): " + errMsg);
                throw;
            }

        }


        /// <summary>
        /// 사용자 코드중 임시테이블에 없는 데이터 삭제 우선 ActiveCode 유무만을 0 으로 세팅한다.
        /// </summary>
        public void DelModifyValueUser()
        {
            try
            {
                string qry = @"update DP_ACC_User set activeuser = '0'
where usercode is not null and usercode <> '' and companyID = 1
   and usercode not in (select interfacecode from ACC_User_Temp where companyID = 1)";

                WsException.WriteAssert("DelModifyValueUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelModifyValueUser(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 관련 임시테이블을 입력한다.
        /// </summary>
        public void SetTempUserMSG(string _interfaceCode)
        {
            try
            {
                string qry = @"insert into ACC_User_Temp (interfacecode, CompanyID) values('{0}', 2)";

                qry = string.Format(qry, _interfaceCode);

                WsException.WriteAssert("SetTempUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetTempUserMSG(): " + errMsg);
                throw;
            }

        }
        

        /// <summary>
        /// 사용자 코드중 임시테이블에 없는 데이터 삭제 우선 ActiveCode 유무만을 0 으로 세팅한다.
        /// EP 용
        /// </summary>
        public void DelModifyValueUserEP()
        {
            try
            {
                string qry = @"update DP_ACC_User set activeuser = '0'
where chinesename is not null
   and chinesename not in (select interfacecode from ACC_User_Temp)";


                WsException.WriteAssert("DelModifyValueUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelModifyValueUser(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자의 직급 순서를 가져온다.
        /// </summary>
        /// <param name="_userid"></param>
        /// <returns></returns>
        public int GetUserDeptOrder(int _userid)
        {
            try
            {
                string qry = @"
SELECT ISNULL(UserDeptOrder, 0)
FROM DP_ACC_UserDept
WHERE CompanyID = 2 and UserID = '{0}'
";
                qry = string.Format(qry, _userid.ToString());

                DataSet ds = null;                

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ds = ExecuteDataSet(cmd, "GetUserDeptOrder");
                }

                if (ds != null)
                {
                    return Int32.Parse(ds.Tables[0].Rows[0][0].ToString());
                }

                return 0;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetUserDeptOrder(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        ///  사용자 부서 맵핑을 날려준다.
        /// </summary>
        /// <param name="_userid"></param>
        public void DelUserDept(int _userid, string _basedept)
        {
            try
            {
                string qry = @"delete DP_ACC_UserDept where CompanyID = 1 and userid = '{0}'";

                qry = string.Format(qry, _userid.ToString());

                WsException.WriteAssert("DelModifyValueUser : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelModifyValueUser(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        ///  사용자 부서 맵핑을 날려준다.
        /// </summary>
        public void DelUserDept()
        {
            try
            {
                string qry = @"DELETE DP_ACC_UserDept WHERE CompanyID = 1 AND UserID NOT IN (select UserID from DP_ACC_User where UserCode is null or UserCode = '')";

                WsException.WriteAssert("DelUserDept : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelUserDept(): " + errMsg);
                throw;
            }
        }

        public DataSet GetUserInfo()
        {
            try
            {
                DataSet myDataSet = new DataSet();

                string query = "SELECT * FROM DP_ACC_User WHERE CompanyID=1 AND UserID>1 ";

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(query, cn);

                    SqlCommand mySqlCommand = cn.CreateCommand();
                    mySqlCommand.CommandText = query;
                    mySqlCommand.CommandType = CommandType.Text;

                    cn.Open();

                    SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
                    mySqlDataAdapter.SelectCommand = mySqlCommand;
                    mySqlDataAdapter.Fill(myDataSet, "DP_APP_Doc");
                }

                return myDataSet;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                return null;
            }
        }

        public void setSealUpdate(int UserID, string UserEmp)
        {
            string query = "UPDATE DP_ACC_User SET ";
            query += "PhotoImage='" + UserEmp + ".jpg' ";
            query += " WHERE UserID=" + UserID;
            SQLHelper.ExecuteNonQuery(ConnSolution(1), CommandType.Text, query, null);
        }

        /// <summary>
        /// 인장등록
        /// </summary>
        /// <param name="_userID"></param>
        /// <param name="_userName"></param>
        /// <param name="_companyid"></param>
        public void SetSignPerson(string _userID, string _userName, string _companyid)
        {
            try
            {
                Bitmap bit = new Bitmap(50, 50);

                //bit.MakeTransparent(); 
                Graphics g = Graphics.FromImage(bit);
                //Rectangle drawRect = new Rectangle(1, 1, 67, 67);
                Rectangle drawString = new Rectangle(1, 18, 50, 50);

                Font font = new Font("굴림", 10.6f, FontStyle.Bold);
                g.Clear(Color.White);
                g.DrawString(_userName, font, Brushes.Black, drawString);

                //string imagepath = @"E:\DeskPlusFileServer\FileData\1\users\{0}\Picture\";
                //string imagepath = @"D:\DeskPlusFileServer\FileData\1\users\{0}\Picture\";
                string imageURL = "/DeskPlusEIP/FileData/1/users/{0}/Picture/";
                string imagepath = HttpContext.Current.Server.MapPath(imageURL);

                imagepath = string.Format(imagepath, _userID);
                string realSignPathAndFileName = imagepath + "seal.jpg";

                if (!Directory.Exists(imagepath))
                {
                    Directory.CreateDirectory(imagepath);
                    bit.Save(realSignPathAndFileName);
                }
                else
                {
                    bit.Save(realSignPathAndFileName);
                }
            }
            catch (Exception ex)
            {
                WsException.WriteException("SetSignPerson(): " + ex);
                throw;
            }
        }

        /// <summary>
        /// 사용자 RoleID 추가
        /// </summary>
        /// <param name="_userid"></param>
        /// <param name="_roleid"></param>
        public void SetUserRoles(string _companyid, string _userid, string _roleid)
        {
            try
            {
                string qry = @"insert into DP_EIP_UserRoles (CompanyID, UserID, RoleID) values({0}, {1}, {2})";

                qry = string.Format(qry, _companyid, _userid, _roleid);

                WsException.WriteAssert("SetUserRoles : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetUserRoles(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// 사용자 RoleID를 날려준다.
        /// </summary>
        /// <param name="_userid"></param>
        public void DelUserRoles(string _userid)
        {
            try
            {
                string qry = @"delete DP_EIP_UserRoles where companyid = 2 and userid = {0}";

                qry = string.Format(qry, _userid);

                WsException.WriteAssert("DelUserRoles : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("DelUserRoles(): " + errMsg);
                throw;
            }

        }

        /// <summary>
        /// ERP에서 넘어온 PasswordUpdateDate로 업데이트
        /// </summary>
        /// <param name="deptID"></param>
        /// <param name="deptOrder"></param>
        public void ModPasswordUpdateDate(int userID, DateTime limit_date)
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_User SET PasswordUpdateDate = @PasswordUpdateDate
WHERE UserID = @UserID
";

                WsException.WriteAssert("ModPasswordUpdateDate : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@UserID", userID.ToString());
                    cmd.Parameters.AddWithValue("@PasswordUpdateDate", limit_date);

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("ModPasswordUpdateDate(): " + errMsg);
                throw;
            }
        }

        #endregion

        #region 패스워드 관련

        /// <summary>
        /// 사번을 입력 받아, 사용자 정보를 리턴한다.
        /// </summary>
        /// <param name="_empcd"></param>
        /// <returns></returns>
        public static DataSet GetUserInfoByEmpCD(string _empcd)
        {
            try
            {
                string qry = @"
SELECT ownerid, DptCD 
FROM viewuserinfo
WHERE ActiveUser = '1'
 AND BaseDept = '1'
 AND EmployeeNumber  = '{0}'
";
                qry = string.Format(qry, _empcd);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetUserInfoByEmpCD"); 
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("GetUserInfoByEmpCD(): " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사번을 입력 받아, 비밀번호 변경 후 리턴한다.
        /// </summary>
        /// <param name="_empcd"></param>
        /// <returns></returns>
        public static bool SetPwdByEmpCD(string _empcd, string _npwd)
        {
            try
            {
                string qry = @"
UPDATE DP_ACC_User SET Password = '{0}' 
WHERE CompanyID = 2 and EmployeeNumber = '{1}'
";
                qry = string.Format(qry, _npwd, _empcd);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;
                    cn.Open();

                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetPwdByEmpCD(): " + errMsg);
                throw;
            }
        }

        #endregion

        #region 임시
        
        public void SetCodeTemp(string _codeGubun, string _interFaceCode, string _codeName, string _codeOrder)
        {
            try
            {
                string qry = @"insert into acc_codeTemp values('{0}', '{1}', '{2}', '{3}')";
                qry = string.Format(qry, _codeGubun, _interFaceCode, _codeName, _codeOrder);
                WsException.WriteAssert("SetTempCode : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetCodeTemp(): " + errMsg);
                throw;
            }
        }

        public void SetDeptTemp(string _deptCode, string _deptOrder, string _parentDept, string _deptName)
        {
            try
            {
                string qry = @"insert into acc_deptTemp values('{0}', '{1}', '{2}', '{3}')";
                qry = string.Format(qry, _deptCode, _deptOrder, _parentDept, _deptName);
                WsException.WriteAssert("SetDeptTemp : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetDeptTemp(): " + errMsg);
                throw;
            }
        }

        public void SetUserTemp(string _userCode, string _userID, string _userName, string _deptCode, string _deptName, string _dutyCode, string _dutyName, string _titleCode, string _titleName, string _loginPwd)
        {
            try
            {
                string qry = @"insert into acc_userTemp values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')";
                qry = string.Format(qry, _userCode, _userID, _userName, _deptCode, _deptName, _dutyCode, _dutyName, _titleCode, _titleName, _loginPwd);
                WsException.WriteAssert("SetUserTemp : " + qry, AppSettingType.APPROVAL);

                using (var cn = new SqlConnection(ConnSolution(1)))
                {
                    var cmd = new SqlCommand(qry, cn);
                    cmd.CommandType = CommandType.Text;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                WsException.WriteException("SetUserTemp(): " + errMsg);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 목적 : 인사 수정
        /// </summary>
        /// <param name="_listinfo"></param>
        /// <returns></returns>
        public void AddUserDept(Account.BE.UserDeptInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserDept_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _info.DeptID;
                    cmd.Parameters.Add("@CodeTitleID", SqlDbType.Int).Value = _info.CodeTitleID;
                    cmd.Parameters.Add("@CodeDutyID", SqlDbType.Int).Value = _info.CodeDutyID;
                    cmd.Parameters.Add("@BaseDept", SqlDbType.Char, 1).Value = _info.BaseDept;
                    cmd.Parameters.Add("@WriteUserID", SqlDbType.Int).Value = _info.WriteUserID;

                    cn.Open();

                    ExecuteNonQuery(cmd);
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        public string ToReverseXSS(string _inputString)
        {
            //특수문자 추가 replace byfreeman 20120514
            //_inputString = _inputString.Replace("&#38;", "&");
            _inputString = _inputString.Replace("&amp;", "&");
            //_inputString = _inputString.Replace("&#34;", @"""");
            _inputString = _inputString.Replace("&quot;", @"""");
            _inputString = _inputString.Replace("&#35;", "#");

            _inputString = _inputString.Replace("&#39;", "'");
            _inputString = _inputString.Replace("&#40;", "(");
            _inputString = _inputString.Replace("&#41;", ")");

            _inputString = _inputString.Replace("&lt;", "<");
            _inputString = _inputString.Replace("&gt;", ">");

            return _inputString;
        }
    }
}