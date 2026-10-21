using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using Zeospace.DeskPlus.Board.BE;

namespace Zeospace.DeskPlus.Board.DAL
{
    public class HomeList : Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.Base.BaseDAL
    {
        #region DataReader

        private ListViewModel GetHomeList(IDataReader _reader)
        {
            return new ListViewModel
            {
                CompanyID = (_reader["CompanyID"] == DBNull.Value) ? -1 : _reader["CompanyID"].ToString().ToNumber<int>(),
                CategoryID = (_reader["CategoryID"] == DBNull.Value) ? -1 : _reader["CategoryID"].ToString().ToNumber<int>(),
                ListID = (_reader["ListID"] == DBNull.Value) ? -1 : _reader["ListID"].ToString().ToNumber<int>(),
                GroupID = (_reader["GroupID"] == DBNull.Value) ? -1 : _reader["GroupID"].ToString().ToNumber<int>(),
                ListOrder = (_reader["ListOrder"] == DBNull.Value) ? -1 : _reader["ListOrder"].ToString().ToNumber<int>(),
                Depth = (_reader["Depth"] == DBNull.Value) ? -1 : _reader["Depth"].ToString().ToNumber<int>(),
                //Title = _reader["Title"].ToString().ToNull().ToXSS(),
                Title = _reader["Title"].ToString().ToNull(),
                Content = _reader["Content"].ToString().ToNull().ToEditorXSS(),
                MobileContent = LimitString(_reader["MobileContent"].ToString().ToNull(), 200),//글자 200자 제한
                OwnerDeptID = (_reader["OwnerDeptID"] == DBNull.Value) ? -1 : _reader["OwnerDeptID"].ToString().ToNumber<int>(),
                OptionModify = _reader["OptionModify"].ToString().ToNull(),
                OptionTop = _reader["OptionTop"].ToString().ToNull(),
                OptionTopDay = (_reader["OptionTopDay"] == DBNull.Value) ? -1 : _reader["OptionTopDay"].ToString().ToNumber<int>(),
                OptionAnonymity = _reader["OptionAnonymity"].ToString().ToNull(),
                ReadCount = (_reader["ReadCount"] == DBNull.Value) ? -1 : _reader["ReadCount"].ToString().ToNumber<int>(),
                ScreenShot = _reader["ScreenShot"].ToString().ToNull(),
                HeaderID = (_reader["HeaderID"] == DBNull.Value) ? -1 : _reader["HeaderID"].ToString().ToNumber<int>(),
                DeptCodeID = (_reader["DeptCodeID"] == DBNull.Value) ? -1 : _reader["DeptCodeID"].ToString().ToNumber<int>(),
                WriteUserID = (_reader["WriteUserID"] == DBNull.Value) ? -1 : _reader["WriteUserID"].ToString().ToNumber<int>(),
                WriteUserName = _reader["WriteUserName"].ToString().ToNull(),
                WriteDateDT = (_reader["WriteDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["WriteDate"],
                ModifyUserID = (_reader["ModifyUserID"] == DBNull.Value) ? -1 : _reader["ModifyUserID"].ToString().ToNumber<int>(),
                ModifyUserName = _reader["ModifyUserName"].ToString().ToNull(),
                ModifyDateDT = (_reader["ModifyDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)_reader["ModifyDate"],
                PassWord = _reader["PassWord"].ToString().ToNull(),

                Etc1 = _reader["Etc1"].ToString().ToNull(),
                Etc2 = _reader["Etc2"].ToString().ToNull(),
                Etc3 = _reader["Etc3"].ToString().ToNull(),
                Etc4 = _reader["Etc4"].ToString().ToNull(),
                Etc5 = _reader["Etc5"].ToString().ToNull(),
                Etc6 = _reader["Etc6"].ToString().ToNull(),
                Etc7 = _reader["Etc7"].ToString().ToNull(),
                Etc8 = _reader["Etc8"].ToString().ToNull().ToNumber<int>(),
                Etc9 = _reader["Etc9"].ToString().ToNull().ToNumber<int>(),

                #region ####  부서,직급,직책 다국어 시작 #######

                NameBase = _reader["NameBase"].ToString().ToNull(),
                NameKo = _reader["NameKo"].ToString().ToNull(),
                NameEn = _reader["NameEn"].ToString().ToNull(),
                NameJa = _reader["NameJa"].ToString().ToNull(),
                NameFr = _reader["NameFr"].ToString().ToNull(),
                NameZhcn = _reader["NameZhcn"].ToString().ToNull(),
                NameZhtw = _reader["NameZhtw"].ToString().ToNull(),

                DeptBase = _reader["DeptBase"].ToString().ToNull(),
                DeptKo = _reader["DeptKo"].ToString().ToNull(),
                DeptEn = _reader["DeptEn"].ToString().ToNull(),
                DeptJa = _reader["DeptJa"].ToString().ToNull(),
                DeptFr = _reader["DeptFr"].ToString().ToNull(),
                DeptZhcn = _reader["DeptZhcn"].ToString().ToNull(),
                DeptZhtw = _reader["DeptZhtw"].ToString().ToNull(),
                TitleBase = _reader["TitleBase"].ToString().ToNull(),
                TitleKo = _reader["TitleKo"].ToString().ToNull(),
                TitleEn = _reader["TitleEn"].ToString().ToNull(),
                TitleJa = _reader["TitleJa"].ToString().ToNull(),
                TitleFr = _reader["TitleFr"].ToString().ToNull(),
                TitleZhcn = _reader["TitleZhcn"].ToString().ToNull(),
                TitleZhtw = _reader["TitleZhtw"].ToString().ToNull(),
                DutyBase = _reader["DutyBase"].ToString().ToNull(),
                DutyKo = _reader["DutyKo"].ToString().ToNull(),
                DutyEn = _reader["DutyEn"].ToString().ToNull(),
                DutyJa = _reader["DutyJa"].ToString().ToNull(),
                DutyFr = _reader["DutyFr"].ToString().ToNull(),
                DutyZhcn = _reader["DutyZhcn"].ToString().ToNull(),
                DutyZhtw = _reader["DutyZhtw"].ToString().ToNull(),

                #endregion ####  부서,직급,직책 다국어 시작 #######

                #region ####  말머리 다국어 시작 #######

                HeaderBase = _reader["HeaderBase"].ToString().ToNull(),
                HeaderKo = _reader["HeaderKo"].ToString().ToNull(),
                HeaderEn = _reader["HeaderEn"].ToString().ToNull(),
                HeaderJa = _reader["HeaderJa"].ToString().ToNull(),
                HeaderFr = _reader["HeaderFr"].ToString().ToNull(),
                HeaderZhcn = _reader["HeaderZhcn"].ToString().ToNull(),
                HeaderZhtw = _reader["HeaderZhtw"].ToString().ToNull()

                #endregion ####  말머리 다국어 시작 #######
            };
        }

        protected List<ListViewModel> GetHomeLists(IDataReader reader)
        {
            List<ListViewModel> list = new List<ListViewModel>();
            while (reader.Read())
                list.Add(GetHomeList(reader));
            return list;
        }

        #endregion DataReader

        #region 공용 템플릿

        /// <summary>
        /// 최신글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetRecentWrite(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeRecentWrite");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 최다 댓글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxShort(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMaxShort");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 최다 조회
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxRead(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMaxRead");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 평가우수
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMaxRate(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMaxRate");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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

        #endregion 공용 템플릿

        #region 나의 템플릿

        /// <summary>
        /// 등록한 게시글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyWrite(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMyWrite");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 댓글 단 게시글 : 내가 작성한 글 기준으로 (타인+나) 가 기재한 댓글
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyListByShort(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMyListByShort");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 등록한 댓글 (남의 글이든, 타인 글이든, 내가 기재한 댓글 기준으로 출력)
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyShort(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMyShort");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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
        /// 중요표시
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.ListViewModel> GetMyImpo(int _companyID, int _userID, int _pageSize)
        {
            try
            {
                List<BE.ListViewModel> al = new List<BE.ListViewModel>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMyImpo");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        return GetHomeLists(reader);
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

        #endregion 나의 템플릿

        #region 최다 작성자, 최다 댓글자

        /// <summary>
        /// 금년도 최다 작성자 가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.MaxWriterInfo> GetMaxWriter(int _companyID, int _pageSize)
        {
            try
            {
                List<BE.MaxWriterInfo> al = new List<BE.MaxWriterInfo>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMaxWriter");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(new MaxWriterInfo
                            {
                                CompanyID = reader["CompanyID"].ToString().ToNumber<int>(),
                                PhotoImage = reader["PhotoImage"].ToString(),
                                UserID = reader["WriteUserID"].ToString().ToNumber<int>(),
                                Cnt = reader["Cnt"].ToString().ToNumber<int>(),
                                NameBase = reader["NameBase"].ToString().ToNull(),
                                NameKo = reader["NameKo"].ToString().ToNull(),
                                NameEn = reader["NameEn"].ToString().ToNull(),
                                NameJa = reader["NameJa"].ToString().ToNull(),
                                NameFr = reader["NameFr"].ToString().ToNull(),
                                NameZhcn = reader["NameZhcn"].ToString().ToNull(),
                                NameZhtw = reader["NameZhtw"].ToString().ToNull(),
                                DeptBase = reader["DeptBase"].ToString().ToNull(),
                                DeptKo = reader["DeptKo"].ToString().ToNull(),
                                DeptEn = reader["DeptEn"].ToString().ToNull(),
                                DeptJa = reader["DeptJa"].ToString().ToNull(),
                                DeptFr = reader["DeptFr"].ToString().ToNull(),
                                DeptZhcn = reader["DeptZhcn"].ToString().ToNull(),
                                DeptZhtw = reader["DeptZhtw"].ToString().ToNull(),
                                TitleBase = reader["TitleBase"].ToString().ToNull(),
                                TitleKo = reader["TitleKo"].ToString().ToNull(),
                                TitleEn = reader["TitleEn"].ToString().ToNull(),
                                TitleJa = reader["TitleJa"].ToString().ToNull(),
                                TitleFr = reader["TitleFr"].ToString().ToNull(),
                                TitleZhcn = reader["TitleZhcn"].ToString().ToNull(),
                                TitleZhtw = reader["TitleZhtw"].ToString().ToNull(),
                                DutyBase = reader["DutyBase"].ToString().ToNull(),
                                DutyKo = reader["DutyKo"].ToString().ToNull(),
                                DutyEn = reader["DutyEn"].ToString().ToNull(),
                                DutyJa = reader["DutyJa"].ToString().ToNull(),
                                DutyFr = reader["DutyFr"].ToString().ToNull(),
                                DutyZhcn = reader["DutyZhcn"].ToString().ToNull(),
                                DutyZhtw = reader["DutyZhtw"].ToString().ToNull()
                            });
                        }
                    }

                    return al;
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
        /// 금년도 최다 댓글자 가져오기
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_pageSize">Size of the _page.</param>
        /// <returns></returns>
        public List<BE.MaxShortReplyerInfo> GetMaxShorter(int _companyID, int _pageSize)
        {
            try
            {
                List<BE.MaxShortReplyerInfo> al = new List<BE.MaxShortReplyerInfo>();

                string query = AppSettingHelper.GetDynamicQuery(AppSettingType.BOARD, "QRY_BOA_List_SelHomeMaxShorter");
                query = string.Format(query, _pageSize);

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(new MaxShortReplyerInfo
                            {
                                CompanyID = reader["CompanyID"].ToString().ToNumber<int>(),
                                PhotoImage = reader["PhotoImage"].ToString(),
                                UserID = reader["UserID"].ToString().ToNumber<int>(),
                                Cnt = reader["Cnt"].ToString().ToNumber<int>(),
                                NameBase = reader["NameBase"].ToString().ToNull(),
                                NameKo = reader["NameKo"].ToString().ToNull(),
                                NameEn = reader["NameEn"].ToString().ToNull(),
                                NameJa = reader["NameJa"].ToString().ToNull(),
                                NameFr = reader["NameFr"].ToString().ToNull(),
                                NameZhcn = reader["NameZhcn"].ToString().ToNull(),
                                NameZhtw = reader["NameZhtw"].ToString().ToNull(),
                                DeptBase = reader["DeptBase"].ToString().ToNull(),
                                DeptKo = reader["DeptKo"].ToString().ToNull(),
                                DeptEn = reader["DeptEn"].ToString().ToNull(),
                                DeptJa = reader["DeptJa"].ToString().ToNull(),
                                DeptFr = reader["DeptFr"].ToString().ToNull(),
                                DeptZhcn = reader["DeptZhcn"].ToString().ToNull(),
                                DeptZhtw = reader["DeptZhtw"].ToString().ToNull(),
                                TitleBase = reader["TitleBase"].ToString().ToNull(),
                                TitleKo = reader["TitleKo"].ToString().ToNull(),
                                TitleEn = reader["TitleEn"].ToString().ToNull(),
                                TitleJa = reader["TitleJa"].ToString().ToNull(),
                                TitleFr = reader["TitleFr"].ToString().ToNull(),
                                TitleZhcn = reader["TitleZhcn"].ToString().ToNull(),
                                TitleZhtw = reader["TitleZhtw"].ToString().ToNull(),
                                DutyBase = reader["DutyBase"].ToString().ToNull(),
                                DutyKo = reader["DutyKo"].ToString().ToNull(),
                                DutyEn = reader["DutyEn"].ToString().ToNull(),
                                DutyJa = reader["DutyJa"].ToString().ToNull(),
                                DutyFr = reader["DutyFr"].ToString().ToNull(),
                                DutyZhcn = reader["DutyZhcn"].ToString().ToNull(),
                                DutyZhtw = reader["DutyZhtw"].ToString().ToNull()
                            });
                        }
                    }

                    return al;
                }
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        #endregion 최다 작성자, 최다 댓글자

        /// <summary>
        /// 게시판 탭별 최신글 카운트 가져오기 (오늘 날짜 기준으로 new 보이기 위함...)
        /// 조건문은 오늘날짜 비교해서 new 개수 몇개인지 count 함.
        /// </summary>
        /// <param name="_companyID">The _company identifier.</param>
        /// <param name="_userID">The _user identifier.</param>
        /// <returns></returns>
        public List<int> GetNewTab(int _companyID, int _userID)
        {
            try
            {
                List<int> al = new List<int>();

                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_statistics", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cn.Open();
                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            al.Add(reader["CNT"].ToString().ToNumber<int>());
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
        /// 급하다..미안.. 함수를 별도로 빼야 하는데..
        /// 여기다가 합세다.~~ 나중에 빼주세용.~
        /// </summary>
        /// <param name="_mobileContent">Content of the _mobile.</param>
        /// <param name="_maxLen">Length of the _max.</param>
        /// <returns></returns>
        private string LimitString(string _mobileContent, int _maxLen)
        {
            if (_mobileContent.Length > _maxLen) _mobileContent = _mobileContent.Substring(0, _maxLen - 5) + "..";

            return _mobileContent;
        }

        #region MoreList

        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 최신글 페이징 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader RecentWriteList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelRecentWriteList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 최신글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int RecentWriteCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelRecentWriteCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 최다 댓글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MaxShortCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxShortCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 최다댓글 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MaxShortList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxShortList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 최다조회 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MaxReadList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxReadList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 최다 조회 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MaxReadCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxReadCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 평가 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MaxRateList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxRateList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 평가 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MaxRateCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMaxRateCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 등록한 게시글
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MyWriteList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyWriteList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 등록한 게시글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MyWriteCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyWriteCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }



        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 댓글이 달린 내 게시글
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MyListByShortList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyListByShortList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 댓글이 달린 내 게시글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MyListByShortCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyListByShortCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }



        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-23
        /// 작성목적 : 내가 댓글을 등록 한 글
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MyShortList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyShortList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-22
        /// 작성목적 : 내가 댓글을 등록 한 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MyShortCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyShortCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }



        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-28
        /// 작성목적 : 중요표시 한 글
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader MyImpoList(int _companyID,
            string _pageSize, string _pageSizePageNo,
            string _searchStartDate, string _searchEndDate, string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyImpoList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.VarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-28
        /// 작성목적 : 중요표시 한 글 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public int MyImpoCount(int _companyID,
            string _searchStartDate, string _searchEndDate,
            string _searchField, string _searchValue,
            int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelMyImpoCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchStartDate", SqlDbType.Char, 8).Value = _searchStartDate;
                    cmd.Parameters.Add("@SearchEndDate", SqlDbType.Char, 8).Value = _searchEndDate;

                    cmd.Parameters.Add("@SearchKey", SqlDbType.NVarChar, 50).Value = _searchField;
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }

        /// <summary>
        /// 사용자 설정 탭 가져오기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int GetUserTab(int _companyID, int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelUserTab", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@UserID", SqlDbType.Char, 8).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }
                return rtCnt;
            }
            catch (SqlException ex)
            {
                string errMsg = Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.SqlExceptionLog(ex);
                Base.ApplicationFrameworks.Exceptions.AppException.WriteException(errMsg);
                throw;
            }
        }


        /// <summary>
        /// 사용자 탭 수정하기
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public bool ModUserTab(int _companyID, int _userID, int _startTabID)
        {
            try
            {
               
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_ModUserTab", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;
                    cmd.Parameters.Add("@StartTabID", SqlDbType.Int).Value = _startTabID;

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
        #endregion MoreList

        /// <summary>
        /// 작성자   : 윤석원
        /// 작성일자 : 2015-12-17
        /// 작성목적 : 검색 리스트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <returns></returns>
        public DataTableReader SearchList(int _companyID, string _searchValue, string _pageSize, string _pageSizePageNo,int _userID)
        {
            try
            {
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelSearchList", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;
                    
                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value = _pageSize;
                    cmd.Parameters.Add("@PageSizePageNo", SqlDbType.Int).Value = _pageSizePageNo;
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
        /// 작성자   : 윤석원
        /// 작성일자 : 2016-03-16
        /// 작성목적 : 검색 카운트
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_searchValue"></param>
        /// <param name="_userID"></param>
        /// <returns></returns>
        public int SearchCount(int _companyID, string _searchValue, int _userID)
        {
            try
            {
                int rtCnt = 0;
                using (var cn = new SqlConnection(ConnSolution(_companyID)))
                {
                    var cmd = new SqlCommand("DP_BOA_HOME_SelSearchCount", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyID", SqlDbType.Int).Value = _companyID;

                    cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, 500).Value = _searchValue;
                    cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = _userID;

                    cn.Open();

                    rtCnt = int.Parse(ExecuteScalar(cmd).ToString());
                }

                return rtCnt;
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