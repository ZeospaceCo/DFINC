using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Portlet.DAL
{
    public class Board : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        /// <summary>
        /// shimpark
        /// 게시물, 최다 등록자
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_categoryID">The _category identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetBoardTopRegister(int _companyID, string _categoryID, int _pageSize)
        {
            try
            {
                if (_categoryID != "") _categoryID = _categoryID.TrimEnd(',');

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_Board_SelTopRegister", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;

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
        /// 작성자   : shimpark
        /// 작성일자 : 2015.01.20
        /// 작성목적 : 최다 조회 게시물 가져오기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID">The _company ID.</param>
        /// <param name="_categoryID">The _category ID.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public DataTableReader GetBoardTopReader(int _companyID, string _categoryID, int _pageSize)
        {
            try
            {
                if (_categoryID != "") _categoryID = _categoryID.TrimEnd(',');

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_Board_SelTopReader", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@CategoryID", SqlDbType.VarChar, 1000).Value = _categoryID;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;

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
        /// 전체 게시판 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_topCount">The _top count.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <param name="_pageSizePageNo">The _page size page no.</param>
        /// <returns></returns>
        public DataTableReader GetBoardTotList(int _companyID, int _userID, int _topCount, int _pageSize, int _pageSizePageNo)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_PT_Boa_SelTotPageList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@TopCount", SqlDbType.Int).Value = _topCount;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;

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
        /// 전체게시물 가져오기
        /// 심재운
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <returns></returns>
        public int GetBoardTotCnt(int _companyID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    string query = @"SELECT COUNT(*)
                        FROM VW_BOA_List WITH (NOLOCK)
                        WHERE CompanyID = @CompanyID AND CategoryID IN (
	                        SELECT DISTINCT CategoryID from DP_BOA_Auth WITH(NOLOCK) where CompanyID = @CompanyID  and OwnerID = 0 and Type = 'G'
                        )";

                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cn.Open();

                    object obj = ExecuteScalar(cmd);
                    return obj.ToString().ToNumber<int>();
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }
    }
}