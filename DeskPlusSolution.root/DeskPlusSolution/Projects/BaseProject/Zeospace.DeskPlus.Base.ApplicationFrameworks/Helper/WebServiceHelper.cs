using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper
{
    /// <summary>
    /// WebServiceHelper에 대한 요약 설명입니다.
    /// </summary>
    public abstract class WebServiceHelper
    {
        #region 웹서비스 에러처리

        /// <summary>
        /// 웹서비스 에러처리
        /// </summary>
        /// <param name="_inputData"></param>
        /// <returns></returns>
        public static string GetErrorCode(string _inputData)
        {
            //------OutPut 변수 선언----------------------
            string errorCode = "";

            StringReader stream;
            XmlTextReader reader = null;

            try
            {
                stream = new StringReader(_inputData);
                reader = new XmlTextReader(stream);

                string tmpName = "";
                string tmpValue = "";
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes)
                        {
                            while (reader.MoveToNextAttribute())
                            {
                                if (reader.Name == "id")
                                {
                                    tmpName = reader.Value;
                                    tmpValue = reader.ReadString();
                                    if (tmpName == "" || tmpValue == "")
                                    {
                                        break;
                                    }
                                    switch (tmpName.ToUpper())
                                    {
                                        case "ERRORCODE":
                                            errorCode = tmpValue;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("WebServiceHelper[GetErrorCode] inputData : " + _inputData + "  " + ex.Message);
                errorCode = "1";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return errorCode;
        }

        /// <summary>
        /// 리턴 메시지
        /// </summary>
        /// <param name="_debugMsg"></param>
        /// <param name="_errorCode"></param>
        /// <param name="_errorMsg"></param>
        /// <returns></returns>
        public static string ServiceReturnMessage(string _debugMsg, string _errorCode, string _errorMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"euc-kr\" ?>\n");
            sb.Append("<root>\n");
            sb.Append("<params>\n");
            sb.Append("<param id=\"__DebugMsg__\" type=\"STRING\">" + _debugMsg + "</param>\n");
            sb.Append("<param id=\"ErrorCode\" type=\"INTEGER\">" + _errorCode + "</param>\n");
            sb.Append("<param id=\"ErrorMsg\" type=\"STRING\">" + _errorMsg + "</param>\n");
            sb.Append("</params>\n");
            sb.Append("</root>\n");
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("ServiceReturnMessage : " + sb.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 리턴 메시지
        /// </summary>
        /// <param name="_debugMsg"></param>
        /// <param name="_errorCode"></param>
        /// <param name="_errorMsg"></param>
        /// <param name="_elName"></param>
        /// <param name="_elValue"></param>
        /// <returns></returns>
        public static string ServiceReturnMessage(string _debugMsg, string _errorCode, string _errorMsg, string[] _elName, string[] _elValue)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"euc-kr\" ?>\n");
            sb.Append("<root>\n");
            sb.Append("<params>\n");
            sb.Append("<param id=\"__DebugMsg__\" type=\"STRING\">" + _debugMsg + "</param>\n");
            sb.Append("<param id=\"ErrorCode\" type=\"INTEGER\">" + _errorCode + "</param>\n");
            sb.Append("<param id=\"ErrorMsg\" type=\"STRING\">" + _errorMsg + "</param>\n");
            for (int i = 0; i < _elName.Length; i++)
            {
                sb.Append("<param id=\"" + _elName[i] + "\" type=\"STRING\">" + _elValue[i] + "</param>\n");
            }
            sb.Append("</params>\n");
            sb.Append("</root>\n");
            //Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("ServiceReturnMessage : " + sb.ToString());
            return sb.ToString();
        }

        #endregion 웹서비스 에러처리



        #region 회사

        /// <summary>
        /// 로컬 회사 아이디 가져오기 : Gets the company ID.
        /// </summary>
        /// <param name="_companyCode">_company code.</param>
        /// <returns></returns>
        public static int GetCompanyID(string _companyCode)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyCode", SqlDbType.VarChar, 20)
													 };
            parms[0].Value = _companyCode;

            object companyID = SQLHelper.ExecuteScalar(SQLHelper.CONN_MASTER, CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Company_SelectByCompanyCode", parms);

            if (companyID != null)
            {
                return Convert.ToInt32(companyID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 타업체 회사 아이디 가져오기 : Gets the company code.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <returns></returns>
        public static string GetCompanyCode(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;

            object companyCode = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Company_SelectByCompanyID", parms);

            if (companyCode != null)
            {
                return companyCode.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion 회사

        #region 사용자

        /// <summary>
        /// LOGIN 아이디로 USERID 가져오기 : Gets the user ID.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public static int GetUserID(int _companyID, string _loginID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@LoginID", SqlDbType.NVarChar, 50)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _loginID;

            object userID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_User_SelectByLoginID", parms);

            if (userID != null)
            {
                return Convert.ToInt32(userID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// UserID로 로그인 ID 정보 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static string GetLoginID(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@UserID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            object loginID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_User_SelectByUserID", parms);

            if (loginID != null)
            {
                return loginID.ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion 사용자

        #region 주소록

        /// <summary>
        /// 주소록 아이디 개수 가져오기
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <param name="_interfaceID">인터페이스아이디</param>
        /// <returns></returns>
        public static int GetPrivateAddressID(int _companyID, string _interfaceID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@InterfaceID", SqlDbType.VarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _interfaceID;

            object privateID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Address_SelectByInterfaceID", parms);

            if (privateID != null)
            {
                return Convert.ToInt32(privateID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 주소록 인터페이스 아이디 가져오기
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <param name="_privateID">주소록아이디</param>
        /// <returns></returns>
        public static string GetAddressInterfaceID(int _companyID, int _privateID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@PrivateID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _privateID;

            object interfaceID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Address_SelectByPrivateID", parms);

            if (interfaceID != null)
            {
                return interfaceID.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 공용주소록 아이디 총개수 가져오기
        /// </summary>
        /// <param name="_companyID">회사아읻</param>
        /// <param name="_interfaceID">인터페이스아이디</param>
        /// <returns></returns>
        public static int GetCommonAddressID(int _companyID, string _interfaceID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@InterfaceID", SqlDbType.VarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _interfaceID;

            object privateID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_CommonAddress_SelectByInterfaceID", parms);

            if (privateID != null)
            {
                return Convert.ToInt32(privateID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 공용주소록 인터페이스 가져오기
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <param name="_commonID">공용주소록아이디</param>
        /// <returns></returns>
        public static string GetCommonAddressInterfaceID(int _companyID, int _commonID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@CommonID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _commonID;

            object interfaceID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_CommonAddress_SelectByCommonID", parms);

            if (interfaceID != null)
            {
                return interfaceID.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 공용주소록 회사별 최상위 카테고리 아이디
        /// </summary>
        /// <param name="_companyID">회사아이디</param>
        /// <returns></returns>
        public static int GetCommonTopCategoryID(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;

            object commonID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_CommonAddress_SelectByTopCategoryID", parms);

            if (commonID != null)
            {
                return Convert.ToInt32(commonID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 공용주소록 인터페이스로 작성자유저아이디 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_interfaceID"></param>
        /// <returns></returns>
        public static int GetCommonWriteUserID(int _companyID, string _interfaceID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@InterfaceID", SqlDbType.VarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _interfaceID;

            object userID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_CommonAddress_SelectWriteUserID", parms);

            if (userID != null)
            {
                return Convert.ToInt32(userID);
            }
            else
            {
                return -1;
            }
        }

        #endregion 주소록

        /// <summary>
        /// LOGIN 아이디로 회사ID , USERID 가져오기 : Gets the user info.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_loginID"></param>
        /// <returns></returns>
        public static int[] GetUserInfo(int _companyID, string _loginID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@LoginID", SqlDbType.NVarChar, 50)
													 };

            parms[0].Value = _companyID;
            parms[1].Value = _loginID;

            int[] userArr = new int[2];

            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Users_SelectByLoginID", parms))
            {
                if (sdr.Read())
                {
                    userArr[0] = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                    userArr[1] = (sdr.GetValue(1) == DBNull.Value) ? -1 : sdr.GetInt32(1);
                }
            }

            return userArr;
        }

        /// <summary>
        /// 타업체 코드 아이디로 CODEID 가져오기 : Gets the code.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_interfaceCode"></param>
        /// <param name="_scope"></param>
        /// <returns></returns>
        public static int GetCode(int _companyID, string _interfaceCode, string _scope)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@InterfaceCode", SqlDbType.VarChar, 20),
														 new SqlParameter("@Scope", SqlDbType.Char, 1)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _interfaceCode;
            parms[2].Value = _scope;

            object codeID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Code_SelectByInterfaceCode", parms);

            if (codeID != null)
            {
                return Convert.ToInt32(codeID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// CODE 아이디로 타업체 아이디 가져오기 :Gets the interface code.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_codeID"></param>
        /// <returns></returns>
        public static string GetInterfaceCode(int _companyID, int _codeID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@CodeID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _codeID;

            object interfaceCode = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Code_SelectByCodeID", parms);

            if (interfaceCode != null)
            {
                return interfaceCode.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 타업체 부서 아이디로 DeptCode 가져오기 : Gets the dept ID.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptCode"></param>
        /// <returns></returns>
        public static int GetDeptID(int _companyID, string _deptCode)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@DeptCode", SqlDbType.VarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _deptCode;

            object deptID = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Depts_SelectByDeptCode", parms);

            if (deptID != null)
            {
                return Convert.ToInt32(deptID);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// deptCode 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_deptID"></param>
        /// <returns></returns>
        public static string GetDeptCode(int _companyID, int _deptID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@DeptID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _deptID;

            object deptCode = SQLHelper.ExecuteScalar(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Depts_SelectByDeptID", parms);

            if (deptCode != null)
            {
                return deptCode.ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 직급, 직책 코드 아이디 가져오기(회사별) : Gets the duty title top ID.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_name">_name.</param>
        /// <returns></returns>
        public static int GetDutyTitleTopID(int _companyID, string _name)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@Name", SqlDbType.NVarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _name;

            int codeID = 0;
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Code_SelectByName", parms))
            {
                if (sdr.Read())
                {
                    codeID = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                }
            }

            if (codeID > 0)
            {
                return codeID;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 회사별 공지사항, 게시판의 최상위 카테고리값 가져오기 : Gets the category ID by title.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_title">_title.</param>
        /// <returns></returns>
        public static int GetCategoryIDByTitle(int _companyID, string _title)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@Title", SqlDbType.NVarChar, 20)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _title;

            int categoryID = 0;
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_BoardCategory_SelectByTitle", parms))
            {
                if (sdr.Read())
                {
                    categoryID = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                }
            }

            if (categoryID > 0)
            {
                return categoryID;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 현재날짜를 기준으로 한달이전 자료 가져오기 : Gets the board list util one month.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <returns></returns>
        public static DataSet GetBoardListUtilOneMonth(int _companyID, string _categoryID)
        {
            string qryStr = "select CategoryID, ListID, GroupID, ListOrder, Depth, Title, WriteUserID, " +
                " WriteDateAll, Filecount, Name from vw_DP_BOARD_List where companyID = {0} and CategoryID IN ({1}) and WriteDateAll between Dateadd(Month, -1, GetDate()) and GetDate() " +
                " ORDER BY OptionTop desc,GroupID DESC,ListOrder ASC,WriteDate DESC";

            string qry = String.Format(qryStr, _companyID, _categoryID);
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, "GetBoardListUtilOneMonth", null);
        }

        /// <summary>
        /// 게시판 리스트 최종게시물 아이디(읽기권한에 할당된 카테고리들만~) : Gets the board list for max list ID.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_categoryID">_category ID.</param>
        /// <returns></returns>
        public static int GetBoardListForMaxListID(int _companyID, string _categoryID)
        {
            string qryStr = "select Max(ListID) " +
                " from vw_DP_BOARD_List where companyID = {0} and CategoryID IN ({1})";

            string qry = String.Format(qryStr, _companyID, _categoryID);

            int listID = 0;
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, null))
            {
                if (sdr.Read())
                {
                    listID = (sdr.GetValue(0) == DBNull.Value) ? -1 : sdr.GetInt32(0);
                }
            }

            if (listID > 0)
            {
                return listID;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 게시판 카테고리 권한 가져오기 : Gets the board authority by category ID.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_categoryID">_category ID.</param>
        /// <returns></returns>
        public static DataSet GetBoardAuthorityByCategoryID(int _companyID, int _categoryID)
        {
            string qryStr = "Select Type, WMode, ownerID From vw_DP_BOARD_Authority Where CompanyID = {0} AND CategoryID = {1}";
            string qry = String.Format(qryStr, _companyID, _categoryID);
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, "GetBoardAuthorityByCategoryID", null);
        }

        /// <summary>
        /// 개인 사용자 아이디 및 그룹으로 읽기권한 이상 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public static DataSet GetBoardAuthorityByReadMode(int _companyID, int _userID, string _groupID)
        {
            //읽기권한 이상
            string qryStr = "select distinct categoryID from dp_board_authorities where companyID = {0} and Authority > 0 and ((type='G' and ownerID in ({2},0)) or (type='M' and ownerID={1}))";
            string qry = String.Format(qryStr, _companyID, _userID, _groupID);
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, "GetBoardAuthorityByReadMode", null);
        }

        /// <summary>
        /// 결재상황조회 결재문서 리스트 받기(반송제외) : Gets the approval status inquery list.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_userID">_user ID.</param>
        /// <returns></returns>
        public static DataSet GetApprovalStatusInqueryList(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@UserID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            string qryStr = "SQL_DP_WEBSERVICE_Approval_Status";
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, qryStr, "GetApprovalStatusInqueryList", parms);
        }

        /// <summary>
        /// 결재상황조회에서 반송리스트
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static DataSet GetApprovalStatusInqueryListReturn(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@UserID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            string qryStr = "SQL_DP_WEBSERVICE_Approval_Status2";
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, qryStr, "GetApprovalStatusInqueryListReturn", parms);
        }

        /// <summary>
        /// 게시판 리스트 날짜 검색 : Gets the board list search date.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_categoryID">_category ID.</param>
        /// <param name="_startDate">_start date.</param>
        /// <param name="_endDate">_end date.</param>
        /// <returns></returns>
        public static DataSet GetBoardListSearchDate(int _companyID, string _categoryID, DateTime _startDate, DateTime _endDate)
        {
            string qryStr = "select * from vw_DP_BOARD_List where companyID = {0} and CategoryID in ({1}) and " +
                " WriteDate >= '" + _startDate.ToShortDateString() + "' AND WriteDate <= '" + _endDate.ToShortDateString() + "' " +
                " order by GroupID desc, ListOrder asc, WriteDate desc";

            string qry = String.Format(qryStr, _companyID, _categoryID);
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, "GetBoardListSearchDate", null);
        }

        /// <summary>
        /// 게시판 리스트 검색(날짜는 제외) : Gets the board list search.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_categoryID">_category ID.</param>
        /// <param name="_field">_field.</param>
        /// <param name="_value">_value.</param>
        /// <returns></returns>
        public static DataSet GetBoardListSearch(int _companyID, string _categoryID, string _field, string _value)
        {
            string qryStr = "select * from vw_DP_BOARD_List where companyID = {0} and CategoryID in ({1}) and " +
                " {2} like '%{3}%'";

            string qry = String.Format(qryStr, _companyID, _categoryID, _field, _value);
            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, "GetBoardListSearch", null);
        }

        /// <summary>
        /// 전자결재 approvalTag 가져오기 : Gets the approval approval tag.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_loginID">_login ID.</param>
        /// <param name="_documentID">_document ID.</param>
        /// <param name="_sequence">_sequence.</param>
        /// <returns></returns>
        public static string GetApprovalApprovalTag(int _companyID, string _loginID, int _documentID, int _sequence)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@LoginID", SqlDbType.VarChar,50),
														 new SqlParameter("@DocumentID", SqlDbType.Int),
														 new SqlParameter("@Sequence", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _loginID;
            parms[2].Value = _documentID;
            parms[3].Value = _sequence;

            string approvalTag = "";
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Approval_GetApprovalTag", parms))
            {
                if (sdr.Read())
                {
                    approvalTag = (sdr.GetValue(0) == DBNull.Value) ? "" : sdr.GetString(0);
                }
            }

            if (approvalTag != "")
            {
                return approvalTag;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 의견모으기 건수 : Gets the poll total.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_userID">_user ID.</param>
        /// <param name="_deptID">_dept ID.</param>
        /// <returns></returns>
        public static int GetPollTotal(int _companyID, int _userID, string _deptID)
        {
            //			string qryStr = "SELECT COUNT(QuestionID) AS CntQuestionID FROM DP_POLL_Questions " +
            //				" WHERE CompanyID = {0} AND Archived = 1 AND " +
            //				" QuestionID IN(SELECT distinct(QuestionID) FROM DP_POLL_Targets " +
            //				" WHERE OwnerID = {1} OR OwnerID in({2}) OR OwnerID = 0) " +
            //				" AND DATEDIFF(DAY, StartDate, getdate()) >= 0 AND DATEDIFF(DAY, EndDate, getdate()) <= 0 " +
            //				" AND UserID NOT IN(SELECT UserID FROM DP_POLL_Votes WHERE DP_POLL_Votes.QuestionID = DP_POLL_Questions.QuestionID)";

            string qryStr = "SELECT COUNT(QuestionID) AS CntQuestionID FROM DP_POLL_Questions WHERE CompanyID = {0} " +
                " AND Archived = 1 AND QuestionID IN( SELECT distinct(QuestionID) " +
                " FROM DP_POLL_Targets WHERE OwnerID = {1} " +
                " OR OwnerID IN (SELECT DeptID FROM DP_ACCOUNT_UserDepts WHERE UserID = {1}) " +
                " OR OwnerID = 0) AND DATEDIFF(DAY, StartDate, getdate()) >= 0 " +
                " AND DATEDIFF(DAY, EndDate, getdate()) <= 0 " +
                " AND UserID NOT IN(SELECT UserID FROM DP_POLL_Votes WHERE DP_POLL_Votes.QuestionID = DP_POLL_Questions.QuestionID)";

            string qry = String.Format(qryStr, _companyID, _userID, _deptID);

            int total = 0;
            using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.ConnSolution(_companyID), CommandType.Text, qry, null))
            {
                if (sdr.Read())
                {
                    total = (sdr.GetValue(0) == DBNull.Value) ? 0 : sdr.GetInt32(0);
                }
            }

            if (total > 0)
            {
                return total;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 전자결재 - 완결,반송,요청 : Gets the approval total.
        /// </summary>
        /// <param name="_companyID">_company ID.</param>
        /// <param name="_userID">_user ID.</param>
        /// <returns></returns>
        public static DataSet GetApprovalTotal(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int),
														 new SqlParameter("@UserID", SqlDbType.Int)
													 };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Approval_GetTotalCount", "GetApprovalTotal", parms);
        }

        /// <summary>
        /// 주소록 - 사원정보(동기화) : Gets the account user info.
        /// </summary>
        /// <param name="_companyID"></param>
        /// <returns></returns>
        public static DataSet GetAccountUserInfo(int _companyID)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.Int)
													 };

            parms[0].Value = _companyID;

            //			string[] userArr = new string[12];
            //
            //			using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Account_GetUserInfo", parms))
            //			{
            //				if (sdr.Read())
            //				{
            //					userArr[0] = (sdr.GetValue(0) == DBNull.Value) ? "" : sdr.GetString(0); //이름
            //					userArr[1] = (sdr.GetValue(1) == DBNull.Value) ? "" : sdr.GetString(1); //모바일
            //					userArr[2] = (sdr.GetValue(2) == DBNull.Value) ? "" : sdr.GetString(2); //집우편번호
            //					userArr[3] = (sdr.GetValue(3) == DBNull.Value) ? "" : sdr.GetString(3); //집주소
            //					userArr[4] = (sdr.GetValue(4) == DBNull.Value) ? "" : sdr.GetString(4); //집전화
            //					userArr[5] = (sdr.GetValue(5) == DBNull.Value) ? "" : sdr.GetString(5); //메일
            //					userArr[6] = (sdr.GetValue(6) == DBNull.Value) ? "" : sdr.GetString(6); //회사이름
            //					userArr[7] = (sdr.GetValue(7) == DBNull.Value) ? "" : sdr.GetString(7); //회사우편번호
            //					userArr[8] = (sdr.GetValue(8) == DBNull.Value) ? "" : sdr.GetString(8); //회사주소
            //					userArr[9] = (sdr.GetValue(9) == DBNull.Value) ? "" : sdr.GetString(9); //회사전화번호
            //					userArr[10] = (sdr.GetValue(10) == DBNull.Value) ? "" : sdr.GetString(10); //회사팩스번호
            //					userArr[11] = (sdr.GetValue(11) == DBNull.Value) ? "" : sdr.GetString(11); //메모
            //				}
            //			}

            //return userArr;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_Account_GetUserInfo", "GetAccountUserInfo", parms);
        }

        /// <summary>
        /// 게시판 리스트 날짜별 가져오기 (VM 용)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_categoryID"></param>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        /// <returns></returns>
        public static DataSet GetBoardListByDate(int _companyID, string _categoryID, string _startDate, string _endDate)
        {
            SqlParameter[] parms = new SqlParameter[]{
														 new SqlParameter("@CompanyID", SqlDbType.VarChar,50),
														new SqlParameter("@CategoryID", SqlDbType.VarChar,500),
														new SqlParameter("@StartDate", SqlDbType.VarChar,50),
														new SqlParameter("@EndDate", SqlDbType.VarChar,50)
													 };

            parms[0].Value = _companyID;
            parms[1].Value = _categoryID;
            parms[2].Value = _startDate;
            parms[3].Value = _endDate;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_WEBSERVICE_BoardList_SelectByCategoryIDDateSearch", "GetBoardListByDate", parms);
        }

        #region 대무자 - 웹서비스

        /// <summary>
        /// 전자결재 : 대무자 리스트 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public static DataSet GetApprovalProxyListSearch(int _companyID, int _userID)
        {
            SqlParameter[] parms = new SqlParameter[] {
														  new SqlParameter("@CompanyID", SqlDbType.Int ),
														  new SqlParameter("@UserID", SqlDbType.Int )
													  };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;

            return SQLHelper.ExecuteDataSet(SQLHelper.ConnSolution(Convert.ToInt32(_companyID)), CommandType.StoredProcedure, "SQL_DP_APPROVAL_Documents_SelectByCompanyID_UserID", "dsRetireDocList", parms);
        }

        /// <summary>
        /// 전자결재 : 대무자 리스트 검색 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <param name="_doc_id"></param>
        /// <param name="_gubun"></param>
        /// <returns></returns>
        public static bool SetApprovalProxyListSearch(int _companyID, int _userID, int _doc_id, string _gubun)
        {
            SqlParameter[] parms = new SqlParameter[] {
														  new SqlParameter("@CompanyID", SqlDbType.Int ),
														  new SqlParameter("@UserID", SqlDbType.Int ),
														  new SqlParameter("@DOCUMENTID", SqlDbType.Int ),
														  new SqlParameter("@Gubun", SqlDbType.Char )
													  };
            parms[0].Value = _companyID;
            parms[1].Value = _userID;
            parms[2].Value = _doc_id;
            parms[3].Value = _gubun;

            try
            {
                SQLHelper.ExecuteNonQuery(SQLHelper.ConnSolution(_companyID), CommandType.StoredProcedure, "SQL_DP_APPROVAL_Documents_DeleteByCompanyID_UserID", parms);
                return true;
            }
            catch (SqlException ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Approval.DALSQL.Documents[SetApprovalProxyListSearch]", ex);
                return false;
            }
        }

        #endregion 대무자 - 웹서비스
    }

    #region Enum

    /// <summary>
    /// FunctionID enum
    /// </summary>
    public enum FunctionID
    {
        /// <summary>
        ///
        /// </summary>
        Admin = 0,

        /// <summary>
        ///
        /// </summary>
        Address = 1,

        /// <summary>
        ///
        /// </summary>
        Approval = 2,

        /// <summary>
        ///
        /// </summary>
        Board = 3,

        /// <summary>
        ///
        /// </summary>
        Mail = 4,

        /// <summary>
        ///
        /// </summary>
        Pims = 5,

        /// <summary>
        ///
        /// </summary>
        Poll = 6,

        /// <summary>
        ///
        /// </summary>
        MyInfo = 7,

        /// <summary>
        ///
        /// </summary>
        WebDisk = 8,

        /// <summary>
        ///
        /// </summary>
        SMS = 9,

        /// <summary>
        ///
        /// </summary>
        Account = 10
    };

    #endregion Enum
}