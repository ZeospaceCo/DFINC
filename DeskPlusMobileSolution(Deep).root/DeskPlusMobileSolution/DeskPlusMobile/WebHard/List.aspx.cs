using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Zeospace.DeskPlusMobile.Web.Helper;
using Zeospace.DeskPlusMobile.Web.Helper.WebComponent;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.WebHard.Ws.DAL;

namespace Zeospace.DeskPlusMobile.Web.WebHard
{
    public partial class List : Zeospace.DeskPlusMobile.Web.Helper.MobileBasePage
    {
        #region 변수설정
        public int parentFolderID = 0;
        public int folderID = 0;
        public string folderName = string.Empty;
        public int pageNo = 1;
        public string order = "";
        public string publicGubun = "";
        public string shareGubun = "";
        public string downCheck = "Y";
        public string uploadCheck = "Y";
        public int PrivateQuota = 0;
        public string UseCount = "0";       // 개인이 사용한 용량
        public string FileLength = "0";     // 공용폴더 파일별 제한용량
        public string fileID = "";
        public string blockExt;
        public string upFileSize;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlusMobile.Web.Resource.GlobalString.string";			//리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlusMobile.Web.WebHard.List).Assembly;	//어셈블리 선언
            #endregion

            if (!IsPostBack)
            {
                SetLanguage();
                LoadParam();

                GetFolderID();
                LoadData();
                GetListCount();
            }
        }

        /// <summary>
        /// Loads the param.
        /// </summary>
        private void LoadParam()
        {
            folderID = GetDecryptParam("FolderID", "0").ToNumber<int>();
            pageNo = GetDecryptParam("PageNo", "1").ToNumber<int>();
            order = GetDecryptParam("Order", "WriteDate desc");
            shareGubun = GetDecryptParam("ShareGubun", "N");
            publicGubun = GetDecryptParam("PublicGubun", "");
            fileID = getDSFileID(GetDecryptParam("FileID", ""));

            if (publicGubun == "")
            {
                if (folderID == -6)
                {
                    publicGubun = "Y";
                }
                else
                {
                    publicGubun = "N";
                }
            }

            ddlOrder.SelectedValue = order;
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-16
        /// 작성목적 : 다국어
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
                ddlOrder.Items.Clear();
                ddlOrder.Items.Add(new ListItem(this.resMng.GetString("Order1"), "WriteDate desc"));
                ddlOrder.Items.Add(new ListItem(this.resMng.GetString("Order2"), "FileName"));
                ddlOrder.Items.Add(new ListItem(this.resMng.GetString("Order3"), "FileType"));
                ddlOrder.Items.Add(new ListItem(this.resMng.GetString("Order4"), "FileLength"));
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2014-08-20
        /// 작성목적 : 본인의 개인폴더 FolderID 값 가져오기 & 개인폴더 생성
        /// </summary>
        public void GetFolderID()
        {
            try
            {
                if (folderID == 0)
                {
                    string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_QRY_Folder_FolderID");
                    sqlQuery = String.Format(sqlQuery, CompanyID, UserID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetFolderID", null);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        folderID = ds.Tables[0].Rows[0]["FolderID"].ToString().ToNumber<int>();
                    }
                    else
                    {
                        Ws.BE.FolderInfo info = new Ws.BE.FolderInfo();
                        info.CompanyID = CompanyID.ToNumber<int>();
                        info.UserID = UserID.ToNumber<int>();
                        info.ParentFolderID = 0;
                        info.NameBase = "ROOT";
                        info.NameKo = "ROOT";
                        info.NameEn = "ROOT";
                        info.NameJa = "ROOT";
                        info.NameFr = "ROOT";
                        info.NameZhcn = "ROOT";
                        info.NameZhtw = "ROOT";

                        Ws.DAL.Folder dal = new Ws.DAL.Folder();
                        int tmpFoilderID = dal.Add(info);

                        if (tmpFoilderID > 0)
                        {
                            folderID = tmpFoilderID;
                        }
                    }
                }
                else if (folderID == -6)
                {
                    string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_QRY_Folder_PublicFolderID");
                    sqlQuery = String.Format(sqlQuery, CompanyID, UserID);
                    DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetFolderID", null);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        folderID = ds.Tables[0].Rows[0]["FolderID"].ToString().ToNumber<int>();
                    }
                    else
                    {
                        Ws.BE.FolderInfo info = new Ws.BE.FolderInfo();
                        info.CompanyID = CompanyID.ToNumber<int>();
                        info.UserID = UserID.ToNumber<int>();
                        info.ParentFolderID = -6;
                        info.NameBase = "ROOT";
                        info.NameKo = "ROOT";
                        info.NameEn = "ROOT";
                        info.NameJa = "ROOT";
                        info.NameFr = "ROOT";
                        info.NameZhcn = "ROOT";
                        info.NameZhtw = "ROOT";                        

                        Ws.DAL.Folder dal = new Ws.DAL.Folder();
                        int tmpFoilderID = dal.Add(info);

                        if (tmpFoilderID > 0)
                        {
                            folderID = tmpFoilderID;
                        }
                    }                    
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("GetFolderID()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-04-18
        /// 작성목적 : Load
        /// </summary>
        public void LoadData()
        {
            blockExt = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "BlockExt");
            upFileSize = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "UpFileSize");

            btAddFile.Visible = true;

            if (folderID == -3)
            {
                FolderTitle = resMng.GetString("SendFile");
                btAddFile.Visible = false;
                uploadCheck = "N";
            }
            else
            {
                LoadTitle();
            }

            if (shareGubun == "Y")
            {
                ShareInfo();
                btDel.Visible = false;
            }

            if (publicGubun == "Y")
            {
                btDel.Visible = false;
                PublicInfo();
            }

            if (shareGubun != "Y" && publicGubun != "Y")
            {
                btDel.Visible = true;
            }

            
            string useMail = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "CONFIG_USEMAIL");
            if (useMail.ToUpper() != "TRUE")
            {
                btSend.Visible = false;
            }
            

            if (publicGubun != "Y")
            {
                // 개인에게 할당된 용량 가져오기 (MB)
                PrivateQuota = UserInfo.PrivateQuota;

                // 개인이 사용한 용량 가져오기
                string sqlQuery = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_QRY_File_Length");
                sqlQuery = String.Format(sqlQuery, CompanyID, UserID);
                DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.Text, sqlQuery, "GetLength", null);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    UseCount = ds.Tables[0].Rows[0]["FileSum"].ToString();
                }

                FileLength = "1073741824";
            }
            else
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)                                             
                                              ,new SqlParameter("@UserID", SqlDbType.Int)   
                                              ,new SqlParameter("@FolderID", SqlDbType.Int)  
                                              ,new SqlParameter("@Types", SqlDbType.NVarChar, 10)  
                };
                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = UserInfo.UserID;
                parms[2].Value = folderID;
                parms[3].Value = "U";

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_PublicFolderFile_FolderID");
                using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms))
                {
                    if (sdr.Read())
                    {
                        // 공용폴더 할당된 용량 가져오기 (MB)
                        PrivateQuota = sdr["FolderLength"].ToString().ToNumber<int>();
                        // 공용폴더 사용한 용량 가져오기
                        UseCount = sdr["FileLengthSum"].ToString();
                        // 공용폴더 파일별 용량 제한
                        int FileLengthInt = sdr["FileLength"].ToString().ToNumber<int>() * 1048576;
                        FileLength = FileLengthInt.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2012-04-17
        /// 작성목적 : 폴더 정보  
        /// </summary>
        public void LoadTitle()
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                              ,new SqlParameter("@UserID", SqlDbType.Int)                                                
                                              ,new SqlParameter("@FolderID", SqlDbType.Int)  
                };
                parms[0].Value = UserInfo.CompanyID;
                parms[1].Value = UserInfo.UserID;
                parms[2].Value = folderID;

                string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_Folder_FolderID");
                using (SqlDataReader sdr = SQLHelper.ExecuteReader(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, parms))
                {
                    if (sdr.Read())
                    {
                        if ((sdr["ParentFolderID"].ToString() == "") && (sdr["NameBase"].ToString() == "ROOT"))
                        {
                            if (publicGubun == "Y")
                            {
                                FolderTitle = resMng.GetString("PublicFolders");
                                folderName = "ROOT";
                            }
                            else
                            {
                                FolderTitle = resMng.GetString("PrivateFolderName");
                                parentFolderID = 0;
                            }
                        }
                        else
                        {
                            FolderTitle = this.GetLangName(sdr["NameBase"].ToString(), sdr["NameKo"].ToString(), sdr["NameEn"].ToString(),
                                   sdr["NameJa"].ToString(), sdr["NameFr"].ToString(), sdr["NameZhcn"].ToString(), sdr["NameZhtw"].ToString(), PreLang);

                            parentFolderID = sdr["ParentFolderID"].ToString().ToNumber<int>();
                        }
                    }
                }

                if ((FolderTitle == "") || (FolderTitle == null))
                {
                    if (publicGubun == "Y")
                    {
                        FolderTitle = resMng.GetString("PublicFolders");
                        folderName = "ROOT";
                    }
                    else
                    {
                        FolderTitle = resMng.GetString("PrivateFolderName");
                        parentFolderID = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("LoadTitle()", ex);
                throw;
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2012-04-18
        /// 작성목적 : 해당 폴더의 공유권한 정보 가져오기
        /// </summary>
        private void ShareInfo()
        {

            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                              ,new SqlParameter("@UserID", SqlDbType.Int)                                                
                                              ,new SqlParameter("@FolderID", SqlDbType.Int)  
            };
            parms[0].Value = UserInfo.CompanyID;
            parms[1].Value = UserInfo.UserID;
            parms[2].Value = folderID;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_ShareFolder_FolderID");
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsShare", parms);

            if (ds != null)
            {
                // 개인 권한 우선 
                DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");
                if (dr.Length > 0)
                {
                    if (dr[0]["Authority"].ToString() == "3")
                    {
                        btSend.Visible = true;
                        downCheck = "Y";
                    }
                    else if (dr[0]["Authority"].ToString() == "2")
                    {
                        btSend.Visible = false;
                        downCheck = "N";
                    }
                    else if (dr[0]["Authority"].ToString() == "1")
                    {
                        btSend.Visible = true;
                        btAddFile.Visible = false;
                        downCheck = "Y";
                        uploadCheck = "N";
                    }
                    else
                    {
                        btSend.Visible = false;
                        btAddFile.Visible = false;
                        downCheck = "N";
                        uploadCheck = "N";
                    }
                }
                else
                {
                    // 부서 권한
                    btSend.Visible = false;
                    btAddFile.Visible = false;
                    downCheck = "N";
                    uploadCheck = "N";

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        if (dbRow["Authority"].ToString() == "3")
                        {
                            btSend.Visible = true;
                            btAddFile.Visible = true;
                            downCheck = "Y";
                            uploadCheck = "Y";
                            break;
                        }
                        else if (dbRow["Authority"].ToString() == "2")
                        {
                            btAddFile.Visible = true;
                            uploadCheck = "Y";
                        }
                        else if (dbRow["Authority"].ToString() == "1")
                        {
                            btSend.Visible = true;
                            downCheck = "Y";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : 이지원
        /// 작성일자 : 2017-06-23
        /// 작성목적 : 폴더의 공유권한 정보 가져오기
        /// </summary>
        private void PublicInfo()
        {

            SqlParameter[] parms = new SqlParameter[] {
                                               new SqlParameter("@CompanyID", SqlDbType.Int)
                                              ,new SqlParameter("@UserID", SqlDbType.Int)                                                
                                              ,new SqlParameter("@FolderID", SqlDbType.Int)  
            };
            parms[0].Value = UserInfo.CompanyID;
            parms[1].Value = UserInfo.UserID;
            parms[2].Value = folderID;

            string sql = Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.SQLWebHard, "SEL_PROC_PublicFolder_FolderID");
            DataSet ds = SQLHelper.ExecuteDataSet(SQLHelper.CONN_SOLUTION, CommandType.StoredProcedure, sql, "dsPublic", parms);

            if (ds != null)
            {
                // 개인 권한 우선 
                DataRow[] dr = ds.Tables[0].Select("TargetType = 'M'");

                if (ds.Tables[0].Rows.Count < 1)
                {
                    if (folderName != "ROOT")
                    {
                        FolderTitle = resMng.GetString("NoPermission");
                    }

                    ddlOrder.Visible = false;
                    PageNumMain.Visible = false;
                    btMain.Visible = false;
                }

                btAll.Visible = false;

                if (dr.Length > 0)
                {
                    if (dr[0]["Authority"].ToString() == "3")
                    {
                        btAll.Visible = true;

                        btSend.Visible = true;
                        btDel.Visible = true;
                        downCheck = "Y";
                    }
                    else if (dr[0]["Authority"].ToString() == "2")
                    {
                        btAll.Visible = true;

                        btSend.Visible = false;
                        btDel.Visible = true;
                        downCheck = "N";
                    }
                    else if (dr[0]["Authority"].ToString() == "1")
                    {
                        btAll.Visible = true;

                        btSend.Visible = true;
                        btAddFile.Visible = false;
                        downCheck = "Y";
                        uploadCheck = "N";
                    }
                    else
                    {
                        btAll.Visible = false;

                        btSend.Visible = false;
                        btAddFile.Visible = false;
                        downCheck = "N";
                        uploadCheck = "N";
                    }
                }
                else
                {
                    // 부서 권한    
                    btAll.Visible = false;
                    btAddFile.Visible = false;
                    btSend.Visible = false;
                    downCheck = "N";
                    uploadCheck = "N";

                    foreach (DataRow dbRow in ds.Tables[0].Rows)
                    {
                        if (dbRow["Authority"].ToString() == "3")
                        {
                            btAll.Visible = true;
                            btDel.Visible = true;
                            btAddFile.Visible = true;
                            btSend.Visible = true;
                            downCheck = "Y";
                            uploadCheck = "Y";
                            break;
                        }
                        else if (dbRow["Authority"].ToString() == "2")
                        {
                            btAll.Visible = true;
                            btDel.Visible = true;
                            btAddFile.Visible = true;
                            btSend.Visible = false;
                            uploadCheck = "Y";
                        }
                        else if (dbRow["Authority"].ToString() == "1")
                        {
                            btAll.Visible = true;

                            btSend.Visible = true;
                            downCheck = "Y";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 작성자 : famo23
        /// 작성일자 : 2015-10-27
        /// 작성목적 : 게시물 총 갯수
        /// </summary>
        private void GetListCount()
        {
            Ws.DAL.List dal = new Ws.DAL.List();
            int Total = dal.GetListCount(CompanyID, UserID, folderID);

            int recsInLastPage = Total % 10;
            int TotalPage = 0;

            //마지막 페이지의 리스트 개수 구하기
            if (recsInLastPage == 0)
            {
                TotalPage = (Total / 10);
            }
            else
            {
                TotalPage = (Total / 10) + 1;
            }

            if (TotalPage < pageNo)
            {
                pageNo = 1;
            }

            TotalNum.InnerText = TotalPage.ToString();
            PageNum.InnerText = pageNo.ToString();
        }

        private string getDSFileID(string fileID)
        {
            if (fileID.Length > 0)
            {
                DocStream docStream = new DocStream();
                DataTable dt = docStream.DSFileInfo(Convert.ToInt32(CompanyID), Convert.ToInt32(fileID), "WHD_FILE");
                
                if (dt.Rows.Count > 0)
                {   
                    if (String.IsNullOrEmpty(dt.Rows[0]["FileID"].ToString()))
                    {   
                        return fileID;
                    }
                    return "";
                }
                return fileID;
            }
            return "";
        }

        public string FolderTitle { get; set; }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : SSL 사용 유무      
        /// </summary>
        public string httpType
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "HTTP_TYPE");
            }
        }

        /// <summary>
        /// 작성자   : famo23
        /// 작성일자 : 2019-03-19
        /// 작성목적 : 도메인 정보        
        /// </summary>
        public string MobileDOMAIN
        {
            get
            {
                return Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "MobileDOMAIN");
            }
        }
    }
}