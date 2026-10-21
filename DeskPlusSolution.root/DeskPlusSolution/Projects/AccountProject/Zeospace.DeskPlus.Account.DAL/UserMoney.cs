using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Account.BE;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Account.DAL
{
    public class UserMoney : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region GET()

        /// <summary>
        /// 작성자   : shine
        /// 작성일자 : 2012-06-19
        /// 작성목적 : 연동정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_userID">The _user ID.</param>
        /// <returns></returns>
        public DataTableReader GetUserMoneyInfo(int _companyID, int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = AppSettingHelper.GetDynamicQuery(AppSettingType.ACCOUNT, "QRY_ACC_UserMoney_Sel");

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
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

        #endregion GET()

        #region ADD()

        /// <summary>
        /// 작성자 : shine
        /// 작성일자 : 2012-06-19
        /// 작성목적 : 연동정보 추가, 수정(관리자용)
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Add(UserMoneyInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_ACC_UserMoney_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _info.UserID;
                    cmd.Parameters.Add("@DeptName", SqlDbType.VarChar, 50).Value = _info.DeptName;
                    cmd.Parameters.Add("@BankNum", SqlDbType.VarChar, 50).Value = _info.BankNum;
                    cmd.Parameters.Add("@BankName", SqlDbType.VarChar, 50).Value = _info.BankName;
                    cmd.Parameters.Add("@ComPoint", SqlDbType.VarChar, 50).Value = _info.ComPoint;
                    cmd.Parameters.Add("@UserCode", SqlDbType.Int).Value = _info.UserCode;
                    cmd.Parameters.Add("@UserGrade", SqlDbType.VarChar, 25).Value = _info.UserGrade;
                    cmd.Parameters.Add("@Gcode", SqlDbType.VarChar, 10).Value = _info.Gcode;
                    cmd.Parameters.Add("@Gname", SqlDbType.VarChar, 10).Value = _info.Gname;
                    cmd.Parameters.Add("@Cname", SqlDbType.VarChar, 30).Value = _info.Cname;
                    cmd.Parameters.Add("@Cnum", SqlDbType.VarChar, 30).Value = _info.Cnum;
                    cmd.Parameters.Add("@Cmoney", SqlDbType.VarChar, 50).Value = _info.Cmoney;
                    cmd.Parameters.Add("@Hdaesang", SqlDbType.Char, 1).Value = _info.Hdaesang;
                    cmd.Parameters.Add("@MPDS", SqlDbType.Int).Value = _info.MPDS;
                    cmd.Parameters.Add("@Reason", SqlDbType.VarChar, 50).Value = _info.Reason;
                    cmd.Parameters.Add("@mbo1stname", SqlDbType.VarChar, 10).Value = _info.mbo1stname;
                    cmd.Parameters.Add("@mbo2ndname", SqlDbType.VarChar, 50).Value = _info.mbo2stname;
                    cmd.Parameters.Add("@1stPyMan", SqlDbType.Char, 1).Value = _info.F1stPyMan;
                    cmd.Parameters.Add("@2ndPyMan", SqlDbType.Char, 1).Value = _info.S2ndPyMan;
                    cmd.Parameters.Add("@IDNumber", SqlDbType.VarChar, 20).Value = _info.IDNumber;
                    cmd.Parameters.Add("@PrivateMail", SqlDbType.VarChar, 50).Value = _info.PrivateMail;
                    cmd.Parameters.Add("@Height", SqlDbType.VarChar, 10).Value = _info.Height;
                    cmd.Parameters.Add("@Weight", SqlDbType.VarChar, 10).Value = _info.Weight;
                    cmd.Parameters.Add("@SightR", SqlDbType.VarChar, 10).Value = _info.SightR;
                    cmd.Parameters.Add("@SightL", SqlDbType.VarChar, 10).Value = _info.SightL;
                    cmd.Parameters.Add("@BloodType", SqlDbType.VarChar, 10).Value = _info.BloodType;
                    cmd.Parameters.Add("@Skill", SqlDbType.VarChar, 50).Value = _info.Skill;
                    cmd.Parameters.Add("@Hobby", SqlDbType.VarChar, 50).Value = _info.Religion;
                    cmd.Parameters.Add("@Religion", SqlDbType.VarChar, 50).Value = _info.Religion;
                    cmd.Parameters.Add("@LivingType", SqlDbType.VarChar, 50).Value = _info.LivingType;
                    cmd.Parameters.Add("@OwnType", SqlDbType.VarChar, 50).Value = _info.OwnType;
                    cmd.Parameters.Add("@ArmyType", SqlDbType.Char, 1).Value = _info.ArmyType;
                    cmd.Parameters.Add("@ArmyGubun", SqlDbType.VarChar, 50).Value = _info.ArmyGubun;
                    cmd.Parameters.Add("@ArmyNumber", SqlDbType.VarChar, 50).Value = _info.ArmyNumber;
                    cmd.Parameters.Add("@ArmyGroup", SqlDbType.VarChar, 50).Value = _info.ArmyGroup;
                    cmd.Parameters.Add("@ArmyBranch", SqlDbType.VarChar, 50).Value = _info.ArmyBranch;
                    cmd.Parameters.Add("@ArmyJoinDate", SqlDbType.VarChar, 10).Value = _info.ArmyJoinDate;
                    cmd.Parameters.Add("@ArmyRetireDate", SqlDbType.VarChar, 10).Value = _info.ArmyRetireDate;
                    cmd.Parameters.Add("@Class", SqlDbType.VarChar, 20).Value = _info.Class;
                    cmd.Parameters.Add("@VeteranType", SqlDbType.Char, 1).Value = _info.VeteranType;
                    cmd.Parameters.Add("@VeteranNumber", SqlDbType.VarChar, 20).Value = _info.VeteranNumber;
                    cmd.Parameters.Add("@ObstacleType", SqlDbType.Char, 1).Value = _info.ObstacleType;
                    cmd.Parameters.Add("@ObstacleNumber", SqlDbType.VarChar, 20).Value = _info.ObstacleNumber;
                    cmd.Parameters.Add("@Etc", SqlDbType.Text).Value = _info.Etc;

                    cmd.Parameters.Add("@etcplus1", SqlDbType.VarChar, 10).Value = _info.etcplus1;
                    cmd.Parameters.Add("@etcplus2", SqlDbType.VarChar, 10).Value = _info.etcplus2;
                    cmd.Parameters.Add("@etcplus3", SqlDbType.VarChar, 10).Value = _info.etcplus3;
                    cmd.Parameters.Add("@etcplus4", SqlDbType.VarChar, 10).Value = _info.etcplus4;
                    cmd.Parameters.Add("@etcplus5", SqlDbType.VarChar, 10).Value = _info.etcplus5;
                    cmd.Parameters.Add("@etcplus6", SqlDbType.VarChar, 10).Value = _info.etcplus6;
                    cmd.Parameters.Add("@etcplus7", SqlDbType.VarChar, 10).Value = _info.etcplus7;
                    cmd.Parameters.Add("@etcplus8", SqlDbType.VarChar, 10).Value = _info.etcplus8;
                    cmd.Parameters.Add("@etcplus9", SqlDbType.Int).Value = _info.etcplus9;
                    cmd.Parameters.Add("@etcplus10", SqlDbType.Int).Value = _info.etcplus10;

                    cn.Open();
                    ExecuteNonQuery(cmd);
                }

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion ADD()
    }
}