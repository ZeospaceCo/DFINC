using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Board.Ws.BE;
using System.Text;

namespace Zeospace.DeskPlusMobile.Web.Approval.Ws.Data
{
    public class DocumentProcess
    {
        #region GET()

        /// <summary>
        /// 문서번호 생성
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public string GetDocNumber(int _companyID, int _documentID)
        {
            try
            {
                SqlParameter[] parm = new SqlParameter[]{
                                             new SqlParameter("@CompanyID", SqlDbType.Int)
                                            ,new SqlParameter("@DocumentID", SqlDbType.Int)
                };
                parm[0].Value = _companyID;
                parm[1].Value = _documentID;


                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_SelByDocNumber", parm);

                return obj.ToString();

            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocNumber: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 문서정보 가져오기 (특정문서의 전체정보)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.DocumentProcessInfo> GetDocument(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_DocProcessInfo");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

                List<BE.DocumentProcessInfo> list = new List<BE.DocumentProcessInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DocumentProcessInfo info = new BE.DocumentProcessInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.Number2 = dr["Number2"].ToString();
                    info.Number3 = dr["Number3"].ToString();
                    info.LifeTime = dr["LifeTime"].ToString();
                    info.AccessTitleID = dr["AccessTitleID"].ToString().ToNumber<int>();
                    info.Subject = dr["Subject"].ToString();
                    info.ContentHTML = dr["ContentHTML"].ToString();
                    info.ContentTEXT = dr["ContentTEXT"].ToString();
                    info.IsLegacy = dr["IsLegacy"].ToString();
                    info.CustomFileName = dr["CustomFileName"].ToString();
                    info.IsAfterSubmitCall = dr["IsAfterSubmitCall"].ToString();
                    info.IsGuard = dr["IsGuard"].ToString();
                    info.IsLinkedEtc = dr["IsLinkedEtc"].ToString();
                    info.IsUseFlow = dr["IsUseFlow"].ToString();
                    info.CarbonView = dr["CarbonView"].ToString();
                    info.FormID = dr["FormID"].ToString().ToNumber<int>();
                    info.FormBase = dr["FormBase"].ToString().ToNull();
                    info.Etc1 = dr["Etc1"].ToString();
                    info.Etc2 = dr["Etc2"].ToString();
                    info.Etc3 = dr["Etc3"].ToString();
                    info.Etc4 = dr["Etc4"].ToString();
                    info.Etc5 = dr["Etc5"].ToString();
                    info.Etc6 = dr["Etc6"].ToString();
                    info.Etc7 = dr["Etc7"].ToString();
                    info.Etc8 = dr["Etc8"].ToString();
                    info.Etc9 = dr["Etc9"].ToString();
                    info.Etc10 = dr["Etc10"].ToString();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocument: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 문서정보 가져오기 (특정문서의 전체정보)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public List<BE.DocumentProcessInfo> GetVWDocument(int _companyID, int _documentID)
        {
            try
            {
                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLApproval, "SEL_QRY_VWDocProcess");
                sql = string.Format(sql, _companyID, _documentID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sql, "dsDoc", null);

                List<BE.DocumentProcessInfo> list = new List<BE.DocumentProcessInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DocumentProcessInfo info = new BE.DocumentProcessInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.Number2 = dr["Number2"].ToString();
                    info.Number3 = dr["Number3"].ToString();
                    info.LifeTime = dr["LifeTime"].ToString();
                    info.AccessTitleID = dr["AccessTitleID"].ToString().ToNumber<int>();
                    info.Subject = dr["Subject"].ToString();
                    info.ContentHTML = dr["ContentHTML"].ToString();
                    info.ContentTEXT = dr["ContentTEXT"].ToString();
                    info.IsLegacy = dr["IsLegacy"].ToString();
                    info.CustomFileName = dr["CustomFileName"].ToString();
                    info.IsAfterSubmitCall = dr["IsAfterSubmitCall"].ToString();
                    info.IsGuard = dr["IsGuard"].ToString();
                    info.IsLinkedEtc = dr["IsLinkedEtc"].ToString();
                    info.IsUseFlow = dr["IsUseFlow"].ToString();
                    info.CarbonView = dr["CarbonView"].ToString();
                    info.FormID = dr["FormID"].ToString().ToNumber<int>();
                    info.FormBase = dr["FormBase"].ToString().ToNull();
                    info.DeptID = dr["DeptID"].ToString().ToNumber<int>();
                    info.Etc1 = dr["Etc1"].ToString();
                    info.Etc2 = dr["Etc2"].ToString();
                    info.Etc3 = dr["Etc3"].ToString();
                    info.Etc4 = dr["Etc4"].ToString();
                    info.Etc5 = dr["Etc5"].ToString();
                    info.Etc6 = dr["Etc6"].ToString();
                    info.Etc7 = dr["Etc7"].ToString();
                    info.Etc8 = dr["Etc8"].ToString();
                    info.Etc9 = dr["Etc9"].ToString();
                    info.Etc10 = dr["Etc10"].ToString();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocument: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 문서정보 가져오기 (특정문서의 전체정보)
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_groupID"></param>
        /// <returns></returns>
        public List<BE.DocumentProcessInfo> GetDocuments(int _companyID, int _groupID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
											   new SqlParameter("@CompanyID", SqlDbType.Int),
											   new SqlParameter("@GroupID", SqlDbType.Int)
										   };
                parms[0].Value = _companyID;
                parms[1].Value = _groupID;

                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_SelByGroupID", "dsDoc", parms);

                List<BE.DocumentProcessInfo> list = new List<BE.DocumentProcessInfo>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BE.DocumentProcessInfo info = new BE.DocumentProcessInfo();
                    info.CompanyID = dr["CompanyID"].ToString().ToNumber<int>();
                    info.DocumentID = dr["DocumentID"].ToString().ToNumber<int>();
                    info.Number2 = dr["Number2"].ToString();

                    list.Add(info);
                }

                return list;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("GetDocuments: " + errMsg);
                throw;
            }
        }


        #endregion

        #region ADD()

        /// <summary>
        /// 추가
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public int Add(BE.DocumentProcessInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@ParentDocumentID", SqlDbType.Int),
                                    new SqlParameter("@Number1", SqlDbType.NVarChar,30),
                                    new SqlParameter("@Number2", SqlDbType.Char, 6),
                                    new SqlParameter("@Number3", SqlDbType.NVarChar,50),
                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                    new SqlParameter("@EmployeeNumber", SqlDbType.NVarChar,50),
                                    new SqlParameter("@UserID", SqlDbType.Int),
                                    new SqlParameter("@LifeTime", SqlDbType.NVarChar,100),
                                    new SqlParameter("@AccessTitleID", SqlDbType.Int),
                                    new SqlParameter("@ApprovalState", SqlDbType.Char,1),
                                    new SqlParameter("@AccountTag", SqlDbType.VarChar,20),
                                    new SqlParameter("@Subject", SqlDbType.NVarChar,500),
                                    new SqlParameter("@ContentHTML", SqlDbType.NText),
                                    new SqlParameter("@ContentTEXT", SqlDbType.NVarChar,4000),
                                    new SqlParameter("@IsLegacy", SqlDbType.Char,1),
                                    new SqlParameter("@CustomFileName", SqlDbType.NVarChar,100),
                                    new SqlParameter("@IsAfterSubmitCall", SqlDbType.Char,1),
                                    new SqlParameter("@IsDRM", SqlDbType.Char,1),
                                    new SqlParameter("@IsGuard", SqlDbType.Char,1),
                                    new SqlParameter("@IsLinkedEtc", SqlDbType.NVarChar,500),
                                    new SqlParameter("@IsUseFlow", SqlDbType.NVarChar,50),
                                    new SqlParameter("@CarbonView", SqlDbType.Char,50),
                                    new SqlParameter("@FormID", SqlDbType.Int),
                                    new SqlParameter("@GroupID", SqlDbType.Int),
                                    new SqlParameter("@Etc1", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc2", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc3", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc4", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc5", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc6", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc7", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc8", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc9", SqlDbType.NVarChar,500),
                                    new SqlParameter("@Etc10", SqlDbType.NVarChar,500),
                                    new SqlParameter("@IsMobile", SqlDbType.Char,1)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.ParentDocumentID;
                parms[2].Value = _info.Number1.ToNull();
                parms[3].Value = _info.Number2.ToNull();
                parms[4].Value = _info.Number3.ToNull();
                parms[5].Value = _info.DeptID;
                parms[6].Value = _info.EmployeeNumber.ToNull();
                parms[7].Value = _info.UserID;
                parms[8].Value = _info.LifeTime.ToNull();
                parms[9].Value = _info.AccessTitleID;
                parms[10].Value = _info.ApprovalState.ToNull();
                parms[11].Value = _info.AccountTag.ToNull();
                parms[12].Value = _info.Subject.ToNull().ToXSS();
                parms[13].Value = _info.ContentHTML.ToNull().ToEditorXSS();
                parms[14].Value = _info.ContentTEXT.ToNull();
                parms[15].Value = _info.IsLegacy;
                parms[16].Value = _info.CustomFileName.ToNull();
                parms[17].Value = _info.IsAfterSubmitCall;
                parms[18].Value = _info.IsDRM;
                parms[19].Value = _info.IsGuard;
                parms[20].Value = _info.IsLinkedEtc.ToNull();
                parms[21].Value = _info.IsUseFlow.ToNull();
                parms[22].Value = _info.CarbonView.ToNull();
                parms[23].Value = _info.FormID;
                parms[24].Value = _info.GroupID;
                parms[25].Value = _info.Etc1.ToNull();
                parms[26].Value = _info.Etc2.ToNull();
                parms[27].Value = _info.Etc3.ToNull();
                parms[28].Value = _info.Etc4.ToNull();
                parms[29].Value = _info.Etc5.ToNull();
                parms[30].Value = _info.Etc6.ToNull();
                parms[31].Value = _info.Etc7.ToNull();
                parms[32].Value = _info.Etc8.ToNull();
                parms[33].Value = _info.Etc9.ToNull();
                parms[34].Value = _info.Etc10.ToNull();
                parms[35].Value = _info.IsMobile;


                object obj = SQLHelper.ExecuteScalar(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_Ins", parms);

                return Convert.ToInt32(obj);
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Add: " + errMsg);
                throw;
            }
        }

        #endregion ADD()

        #region MOD()

        /// <summary>
        /// 승인처리-문서상태/문서내용 반영
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModApprovalState(BE.DocumentProcessInfo _info)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@GroupID", SqlDbType.Int),
                                    new SqlParameter("@AccountTag", SqlDbType.VarChar, 20),
                                    new SqlParameter("@ApprovalState", SqlDbType.Char,1),
                                    new SqlParameter("@ContentHTML", SqlDbType.NText),
                                    new SqlParameter("@ContentTEXT", SqlDbType.NVarChar,4000)
                    
			    };

                parms[0].Value = _info.CompanyID;
                parms[1].Value = _info.DocumentID;
                parms[2].Value = _info.GroupID;
                parms[3].Value = _info.AccountTag.ToNull();
                parms[4].Value = _info.ApprovalState.ToNull();
                parms[5].Value = _info.ContentHTML.ToNull().ToEditorXSS();
                parms[6].Value = _info.ContentTEXT.ToNull();

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_UpdContentHTML", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModApprovalState: " + errMsg);
                throw;
            }
        }

        /// <summary>
        /// 완료처리
        /// </summary>
        /// <param name="_info"></param>
        /// <returns></returns>
        public bool ModConclude(int _companyID, int _documentID, int _groupID, string _accountTag, string _approvalState, string _contentHTML, string _contentTEXT, int _Number4, string _docNumber)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int),
                                    new SqlParameter("@GroupID", SqlDbType.Int),
                                    new SqlParameter("@AccountTag", SqlDbType.VarChar, 20),
                                    new SqlParameter("@ApprovalState", SqlDbType.Char,1),
                                    new SqlParameter("@ContentHTML", SqlDbType.NText),
                                    new SqlParameter("@ContentTEXT", SqlDbType.NVarChar,4000),
                                    new SqlParameter("@Number4", SqlDbType.Int),
                                    new SqlParameter("@DocNumber", SqlDbType.NVarChar,100)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;
                parms[2].Value = _groupID;
                parms[3].Value = _accountTag.ToNull();
                parms[4].Value = _approvalState.ToNull();
                parms[5].Value = _contentHTML.ToNull().ToEditorXSS();
                parms[6].Value = _contentTEXT.ToNull();
                parms[7].Value = _Number4;
                parms[8].Value = _docNumber.ToNull();

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_UpdConclude", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("ModConclude: " + errMsg);
                throw;
            }
        }


        #endregion

        #region DEL()

        /// <summary>
        /// 문서삭제
        /// </summary>
        /// <param name="_companyID"></param>
        /// <param name="_documentID"></param>
        /// <returns></returns>
        public bool Del(int _companyID, int _documentID)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                    new SqlParameter("@CompanyID", SqlDbType.Int),
                                    new SqlParameter("@DocumentID", SqlDbType.Int)
                    
			    };

                parms[0].Value = _companyID;
                parms[1].Value = _documentID;

                SQLHelper.ExecuteNonQuery(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, "DP_APP_DocProcess_Del", parms);

                return true;
            }
            catch (SqlException ex)
            {
                string errMsg = Helper.AppException.SqlExceptionLog(ex);
                Helper.AppException.WriteException("Del: " + errMsg);
                throw;
            }
        }

        #endregion

    }
}