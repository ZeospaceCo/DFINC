using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.EIP.BE;

namespace Zeospace.DeskPlus.EIP.DAL
{
    public class Menu : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private MenuInfo GetMenu(IDataReader _reader)
        {
            return new MenuInfo
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                MenuID = (_reader["MenuID"] == DBNull.Value) ? -1 : _reader["MenuID"].ToString().ToNumber<int>(),
                ParentMenuID = (_reader["ParentMenuID"] == DBNull.Value) ? -1 : _reader["ParentMenuID"].ToString().ToNumber<int>(),
                MenuOrder = (_reader["MenuOrder"] == DBNull.Value) ? -1 : _reader["MenuOrder"].ToString().ToNumber<int>(),
                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),
                PathID = _reader["PathID"].ToString().ToNull(),
                PathNameBase = _reader["PathNameBase"].ToString().ToNull(),
                PathNameKo = _reader["PathNameKo"].ToString().ToNull(),
                PathNameEn = _reader["PathNameEn"].ToString().ToNull(),
                PathNameJa = _reader["PathNameJa"].ToString().ToNull(),
                PathNameFr = _reader["PathNameFr"].ToString().ToNull(),
                PathNameZhcn = _reader["PathNameZhcn"].ToString().ToNull(),
                PathNameZhtw = _reader["PathNameZhtw"].ToString().ToNull(),
                Levels = (_reader["Levels"] == DBNull.Value) ? -1 : _reader["Levels"].ToString().ToNumber<int>(),
                MenuContent = _reader["MenuContent"].ToString().ToNull(),
                TargetURL = _reader["TargetURL"].ToString().ToNull(),
                OpenURL = _reader["OpenURL"].ToString().ToNull(),
                DirectCall = _reader["DirectCall"].ToString().ToNull(),
                SendMethod = _reader["SendMethod"].ToString().ToNull()
            };
        }

        protected List<MenuInfo> GetMenuList(IDataReader reader)
        {
            List<MenuInfo> list = new List<MenuInfo>();

            while (reader.Read())
                list.Add(GetMenu(reader));
            return list;
        }

        #endregion DataReader

        #region 추가/수정/삭제

        /// <summary>
        /// 심재운
        /// 게시판 카테고리 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public int Add(MenuInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Menu_Ins", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@ParentMenuID", SqlDbType.Int).Value = _info.ParentMenuID;
                    cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = _info.MenuOrder;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@MenuContent", SqlDbType.NVarChar, 150).Value = _info.MenuContent.ToNull().ToXSS();
                    cmd.Parameters.Add("@TargetURL", SqlDbType.NVarChar, 400).Value = _info.TargetURL.ToNull();
                    cmd.Parameters.Add("@OpenURL", SqlDbType.Char, 1).Value = _info.OpenURL.ToNull();
                    cmd.Parameters.Add("@DirectCall", SqlDbType.NVarChar, 50).Value = _info.DirectCall.ToNull();
                    cmd.Parameters.Add("@SendMethod", SqlDbType.Char, 1).Value = _info.SendMethod.ToNull();
                    cmd.Parameters.Add("@ClassName", SqlDbType.VarChar, 50).Value = _info.ClassName.ToNull();

                    cn.Open();

                    return ExecuteScalar(cmd).ToString().ToNumber<int>();
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
        /// 심재운
        /// 메뉴 수정
        /// </summary>
        /// <param name="_info">The _info.</param>
        /// <returns></returns>
        public bool Mod(MenuInfo _info)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_info.CompanyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Menu_Upd", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _info.CompanyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _info.MenuID;
                    cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = _info.MenuOrder;
                    cmd.Parameters.Add("@NameBase", SqlDbType.NVarChar, 100).Value = _info.NameBase.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameKo", SqlDbType.NVarChar, 100).Value = _info.NameKo.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameEn", SqlDbType.NVarChar, 100).Value = _info.NameEn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameJa", SqlDbType.NVarChar, 100).Value = _info.NameJa.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameFr", SqlDbType.NVarChar, 100).Value = _info.NameFr.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhcn", SqlDbType.NVarChar, 100).Value = _info.NameZhcn.ToNull().ToXSS();
                    cmd.Parameters.Add("@NameZhtw", SqlDbType.NVarChar, 100).Value = _info.NameZhtw.ToNull().ToXSS();
                    cmd.Parameters.Add("@MenuContent", SqlDbType.NVarChar, 150).Value = _info.MenuContent.ToNull().ToXSS();
                    cmd.Parameters.Add("@TargetURL", SqlDbType.NVarChar, 400).Value = _info.TargetURL.ToNull();
                    cmd.Parameters.Add("@OpenURL", SqlDbType.Char, 1).Value = _info.OpenURL.ToNull();
                    cmd.Parameters.Add("@DirectCall", SqlDbType.NVarChar, 50).Value = _info.DirectCall.ToNull();
                    cmd.Parameters.Add("@SendMethod", SqlDbType.Char, 1).Value = _info.SendMethod.ToNull().ToXSS();
                    cmd.Parameters.Add("@ClassName", SqlDbType.VarChar, 50).Value = _info.ClassName.ToNull();

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

        /// <summary>
        /// 작성자 : 심재운
        /// menuid 삭제
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <returns></returns>
        public bool Del(int _companyID, int _menuID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Menu_Del", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 순서변경하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryList">The _category list.</param>
        /// <returns></returns>
        public bool ModOrder(int _companyID, string _menuList)
        {
            try
            {
                if (_menuList.Length > 0)
                {
                    string[] _menuListStr = _menuList.Split(',');

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        cn.Open();

                        for (int i = 0; i < _menuListStr.Length; i++)
                        {
                            var cmd = new SqlCommand("DP_EIP_Menu_UpdOrder", cn);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                            cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuListStr[i];
                            cmd.Parameters.Add("@MenuOrder", SqlDbType.Int).Value = (i + 1);

                            ExecuteNonQuery(cmd);
                        }
                    }
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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메뉴 이동하기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_toMenuID">The _to menu ID.</param>
        /// <returns></returns>
        public bool ModMove(int _companyID, int _menuID, int _toMenuID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_Menu_UpdMove", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                    cmd.Parameters.Add("@ToMenuID", SqlDbType.Int).Value = _toMenuID;

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

        /// <summary>
        /// 작성자 : 심재운
        /// 용도 : 메뉴 상위 변경
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _menu ID.</param>
        /// <param name="_parentMenuID">The _parent menu ID.</param>
        /// <returns></returns>
        public bool ModParentMenuID(int _companyID, int _menuID, int _parentMenuID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_Menu_UpdPID", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                    cmd.Parameters.Add("@ParentMenuID", SqlDbType.Int).Value = _parentMenuID;

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

        /// <summary>
        /// 메뉴 아이콘 설정 변경
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_iconYN"></param>
        /// <returns></returns>
        public bool ModMenuIcon(int _companyID, string _iconYN)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    cn.Open();

                    var cmd = new SqlCommand("DP_EIP_Menu_UpdIcon", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@IconYN", SqlDbType.Char, 1).Value = _iconYN;                    

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

        #endregion 추가/수정/삭제

        #region 가져오기

        /// <summary>
        /// 작성자 : 심재운
        /// 회사별 카테고리 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetMenu(int _companyID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_Sel");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMenu");
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
        /// 메뉴 아이디로  정보 가져오기 (0 이면 최상위 메뉴들 가져옴)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _category ID.</param>
        /// <returns></returns>
        public DataSet GetMenu(int _companyID, int _menuID)
        {
            try
            {
                string query = "";

                if (_menuID == 0)
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_SelByPID");
                    query = string.Format(query, "is null");

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetMenu");
                    }
                }
                else
                {
                    query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_SelByID");

                    using (var cn = new SqlConnection(ConnSolution(_companyID)))
                    {
                        var cmd = new SqlCommand(query, cn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                        cmd.Parameters.Add("@MenuID", SqlDbType.Int).Value = _menuID;
                        cn.Open();

                        return ExecuteDataSet(cmd, "GetMenu");
                    }
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
        /// DirectCall로 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_menuID">The _category ID.</param>
        /// <returns></returns>
        public List<MenuInfo> GetMenuByDirectCall(int _companyID, string _dcparam)
        {
            try
            {
                string query = "";

                query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_SelByDirectCall");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DirectCall", SqlDbType.NVarChar, 50).Value = _dcparam;
                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetMenuList(reader);
                    }
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
        /// 작성자 : 심재운
        /// 하위메뉴 정보 가져오기 (트리용)
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_parentMenuID">The _parent category ID.</param>
        /// <returns></returns>
        public DataSet GetMenuByPID(int _companyID, string _parentMenuID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_SelByPID");

                if (_parentMenuID == "0")
                {
                    query = string.Format(query, "is null");
                }
                else
                {
                    query = string.Format(query, "=" + _parentMenuID.ToInject());
                }

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMenu");
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
        /// 작성자 : 심재운
        /// 사용자별 메뉴 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public List<int> GetMenuByUser(int _companyID, int _deptID, int _userID)
        {
            try
            {
                List<int> al = new List<int>();
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Menu_SelGetUser", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@DeptID", SqlDbType.Int).Value = _deptID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(reader["MenuID"].ToString().ToNumber<int>());
                        }
                    }
                }

                return al;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자별 즐겨찾기 메뉴
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public DataSet GetFaMenuUser(int _companyID, int _userID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_MenuUser_Sel");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetMenu");
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
        /// 작성자 : seo
        /// 사용자별 메뉴 한방에 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public Dictionary<int, string> GetMenuByUserAllOne(int _companyID)
        {
            try
            {
                Dictionary<int, string> al = new Dictionary<int, string>();
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_EIP_Menu_SelGetUserAllOne", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(reader["UserID"].ToString().ToNumber<int>(), reader["Rols"].ToString());
                        }
                    }
                }

                return al;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 회사별 카테고리 정보 가져오기
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <returns></returns>
        public DataSet GetTopMenu(int _companyID)
        {
            try
            {
                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.EIP, "QRY_EIP_Menu_SelTop");

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();

                    return ExecuteDataSet(cmd, "GetTopMenu");
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

    
        #endregion 가져오기
    }
}