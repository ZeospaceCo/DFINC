using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.Collections.Specialized;

namespace Zeospace.DeskPlusMobile.Web.Board.Ws.Data
{
    public class Authority
    {

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <returns></returns>
        public DataSet GetAuthority(int _companyID, int _categoryID)
        {
            SqlParameter[] parms = new SqlParameter[] {
											            new SqlParameter("@CompanyID", SqlDbType.Int),
											            new SqlParameter("@CategoryID", SqlDbType.Int)};

            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Auth_SelByCID", "GetAuthority", parms);
        }

        /// <summary>
        /// Gets the authority.
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_ownerIDs">The _owner I ds.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public DataSet GetAuthority(int _companyID, string _categoryID, string _ownerIDs, string _gubun)
        {
            SqlParameter[] parms = new SqlParameter[] {
				                                        new SqlParameter("@CompanyID", SqlDbType.VarChar,10),
                                                        new SqlParameter("@CategoryID", SqlDbType.VarChar,5000),
                                                        new SqlParameter("@OwnerID", SqlDbType.VarChar,5000),
                                                        new SqlParameter("@Gubun", SqlDbType.Char,1)
													  };

            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _ownerIDs;
            parms[3].Value = _gubun;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "DPM_BOA_Auth_Sel", "GetAuthorityCateogry", parms);
        }


        /// <summary>
        /// 작성자 : 심재운
        /// 나의 권한 카테고리 아이디 값들
        /// _childOwnerList : 부서들 또는 사용자
        /// gubun : 1(사용자) 2(부서) 3(전체)
        /// 반환값 : CategoryID, Authority
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_childCatList">The _category ID.</param>
        /// <param name="_childOwnerList">The _owner I ds.</param>
        /// <param name="_gubun">The _gubun.</param>
        /// <returns></returns>
        public NameValueCollection GetMyAuthCategory(int _companyID, string _childCatList, string _childOwnerList, string _gubun)
        {
            //key 는 중복가능함
            NameValueCollection authCatList = new NameValueCollection();

            using (var cn = new SqlConnection(SQLHelper.ConnSolution(_companyID)))
            {
                var cmd = new SqlCommand("DP_BOA_Auth_SelGetMyAuth", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CompanyID", SqlDbType.VarChar, 10).Value = _companyID;
                cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 5000).Value = _childCatList;
                cmd.Parameters.Add("@OwnerID", SqlDbType.VarChar, 5000).Value = _childOwnerList;
                cmd.Parameters.Add("@Gubun", SqlDbType.Char, 1).Value = _gubun;

                cn.Open();

                DataTableReader dtr = ExecuteDataTableReader(cmd);

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            authCatList.Add(dtr["CategoryID"].ToString(), dtr["Authority"].ToString());
                        }
                    }
                }
            }

            return authCatList;
        }


        /// <summary>
        /// Executes the data table reader.
        /// </summary>
        /// <param name="cmd">The command.</param>
        /// <returns></returns>
        protected DataTableReader ExecuteDataTableReader(SqlCommand cmd)
        {
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt.CreateDataReader();
            }
        }
    }
}