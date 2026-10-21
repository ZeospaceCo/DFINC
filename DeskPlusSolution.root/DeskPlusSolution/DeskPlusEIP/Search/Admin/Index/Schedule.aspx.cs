using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Threading;
using System.Globalization;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent;
using System.Collections.Specialized;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Data;

namespace Zeospace.DeskPlus.Search.Web.Admin.Index
{
    public partial class Schedule : System.Web.UI.Page
    {
        #region 변수설정
        protected System.Resources.ResourceManager resMng = null;
        public string ResourceName { get; set; }
        public Assembly AssemblyName { get; set; }
        private string decryString = string.Empty;//복고화 값
        #endregion

        #region Page Process (페이지 처리)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string message = String.Empty;
            
            message += "RequestType : " + Request.RequestType + "\n\r";

            if (Request.RequestType == "POST" && !String.IsNullOrEmpty(Request.Params["p"]))
            {
                string parm = Request.Params["p"];
                message += "p=" + Request.Params["p"].ToString();

                AddIndex(parm); //색인 진행
            }

            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(message);
        }
        #endregion

        #region (함수)
        private void AddIndex(string _parm)
        {
            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Schedule.AddIndex : Start ");

            try
            {
                //게시판 색인
                if (_parm == "INDEX" || _parm == "BOARD") Add("BOARD");  //f=BOARD

                //전자결재 색인
                if (_parm == "INDEX" || _parm == "APPROVAL") Add("APPROVAL");  //f=APPROVAL

                //문서관리 색인
                if (_parm == "INDEX" || _parm == "EDM") Add("EDM");  //f=EDM

                //주소록 색인
                if (_parm == "INDEX" || _parm == "ADDRESS") Add("ADDRESS");  //f=EDM

                //웹하드 색인
                if (_parm == "INDEX" || _parm == "WEBHARD") Add("WEBHARD");  //f=WEBHARD

                //업무협업 색인
                if (_parm == "INDEX" || _parm == "COOP") Add("COOP");  //f=COOP

                //업무협업 색인
                if (_parm == "INDEX" || _parm == "TASK") Add("TASK");  //f=TASK

                //일정관리 색인
                if (_parm == "INDEX" || _parm == "PIM") Add("PIM");  //f=PIM
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Schedule.AddIndex() " + ex.Message);
            }

        }

        public void Add(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            try
            {
                //IndexLog BE 초기화
                Zeospace.DeskPlus.Search.BE.IndexLogInfo indexInfo = new BE.IndexLogInfo();
                indexInfo.CompanyID = 1;
                indexInfo.LogID = 0;
                indexInfo.FTRTextCount = 0;
                indexInfo.IndexCount = 0;
                indexInfo.Status = "P";
                indexInfo.StartDate = DateTime.Now;
                indexInfo.EndDate = DateTime.Now;
                indexInfo.WriteUserID = 0;


                //IndexLog Detail Message
                Zeospace.DeskPlus.Search.BE.IndexLogDetailInfo indexDetailInfo = new BE.IndexLogDetailInfo();
                indexDetailInfo.CompanyID = 1;

                Zeospace.DeskPlus.Search.DAL.IndexLog dalIndexLog = new DAL.IndexLog();
                Zeospace.DeskPlus.Search.DAL.IndexLogDetail dalIndexDetailLog = new DAL.IndexLogDetail();

                int logID = 0;
                int ftrTextCount = 0;
                int indexCount = 0;

                switch (p.ToUpper())
                {
                    case "BOARD":
                        #region BOARD
                        try
                        {
                            indexInfo.Category = "BOARD";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[BOARD:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.Board boa = new Zeospace.DeskPlus.Search.DAL.Board();
                            List<BE.Board.AttachFileInfo> attachList = boa.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.Board.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[BOARD:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.Board.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.Board.Index.AddIndex(boa.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[BOARD:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[BOARD:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[BOARD:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "APPROVAL":
                        #region APPROVAL
                        try
                        {
                            indexInfo.Category = "APPROVAL";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[APPROVAL:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.Approval app = new Zeospace.DeskPlus.Search.DAL.Approval();
                            List<BE.Approval.AttachFileInfo> attachList = app.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.Approval.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[APPROVAL:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.Approval.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.Approval.Index.AddIndex(app.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[APPROVAL:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[APPROVAL:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[" + indexInfo.Category + ":ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "EDM":
                        #region EDM
                        try
                        {
                            indexInfo.Category = "EDM";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[EDM:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.EDM app = new Zeospace.DeskPlus.Search.DAL.EDM();
                            List<BE.EDM.AttachFileInfo> attachList = app.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.EDM.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[EDM:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.EDM.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.EDM.Index.AddIndex(app.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[EDM:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[EDM:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[" + indexInfo.Category + ":ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "ADDRESS":
                        #region ADDRESS
                        try
                        {
                            indexInfo.Category = "ADDRESS";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[ADDRESS:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.Address app = new Zeospace.DeskPlus.Search.DAL.Address();

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.Address.Index.AddIndex(app.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[ADDRESS:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[ADDRESS:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[" + indexInfo.Category + ":ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "WEBHARD":
                        #region WEBHARD
                        try
                        {
                            indexInfo.Category = "WEBHARD";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[WEBHARD:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.WebHard whd = new Zeospace.DeskPlus.Search.DAL.WebHard();
                            List<BE.WebHard.ListInfo> attachList = whd.GetListAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.WebHard.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[WEBHARD:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //List DB처리
                            Zeospace.DeskPlus.Search.BLL.WebHard.Index.AddIndex(attachList);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[WEBHARD:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[WEBHARD:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "COOP":
                        #region COOP
                        try
                        {
                            indexInfo.Category = "COOP";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[COOP:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.Coop coop = new Zeospace.DeskPlus.Search.DAL.Coop();
                            List<BE.Coop.AttachFileInfo> attachList = coop.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.Coop.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[COOP:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.Coop.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.Coop.Index.AddIndex(coop.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[COOP:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[COOP:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[COOP:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "TASK":
                        #region TAS
                        try
                        {
                            indexInfo.Category = "TASK";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[TASK:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.TAS tas = new Zeospace.DeskPlus.Search.DAL.TAS();
                            List<BE.TAS.AttachFileInfo> attachList = tas.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.TAS.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[TASK:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.TAS.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.TAS.Index.AddIndex(tas.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[TASK:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[TASK:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[TASK:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion
                    case "PIM":
                        #region PIM
                        try
                        {
                            indexInfo.Category = "PIM";

                            logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                            indexInfo.LogID = logID;
                            indexDetailInfo.LogID = logID;

                            //Detail Log
                            indexDetailInfo.Message = "[PIM:START] " + GetResource("MSG_START");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Search.DAL.Pim pim = new Zeospace.DeskPlus.Search.DAL.Pim();
                            List<BE.Pim.AttachFileInfo> attachList = pim.GetAttachAll();  // 첨부파일 리스트


                            //TXT 변환
                            ftrTextCount = Zeospace.DeskPlus.Search.BLL.Pim.Index.GetTextFile(attachList); //TEXT 파일 변환
                            indexInfo.FTRTextCount = ftrTextCount;

                            //Detail Log
                            indexDetailInfo.Message = "[PIM:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                            dalIndexDetailLog.Add(indexDetailInfo);

                            //File DB처리
                            Zeospace.DeskPlus.Search.BLL.Pim.Index.AddFTRIndex(attachList);

                            //List DB처리
                            indexCount = Zeospace.DeskPlus.Search.BLL.Pim.Index.AddIndex(pim.GetListAll());
                            indexInfo.IndexCount = indexCount;

                            //Detail Log
                            indexDetailInfo.Message = "[PIM:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                            dalIndexDetailLog.Add(indexDetailInfo);


                            indexInfo.EndDate = DateTime.Now;
                            indexInfo.Status = "C";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[PIM:COMPLETE] " + GetResource("MSG_COMPLETE");
                            dalIndexDetailLog.Add(indexDetailInfo);
                        }
                        catch (Exception ex)
                        {
                            indexInfo.Status = "E";
                            dalIndexLog.Mod(indexInfo);

                            //Detail Log
                            indexDetailInfo.Message = "[PIM:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                            dalIndexDetailLog.Add(indexDetailInfo);

                            Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
                        }

                        break;
                        #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message);
            }
        }
        #endregion

        #region NO Auth 처리 
        /// <summary>
        /// 다국어 리소스 인스턴스 메소드
        /// </summary>
        /// <remarks></remarks>
        private void GetResourceManager()
        {
            resMng = new System.Resources.ResourceManager(ResourceName, AssemblyName);
        }

        /// <summary>
        /// 다국어 리소스 값 반환 메소드
        /// </summary>
        /// <param name="_resName">Name of the _res.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public string GetResource(string _resName)
        {
            return new System.Resources.ResourceManager(ResourceName, AssemblyName).GetString(_resName);
        }

        /// <summary>
        /// 파라미터 명시
        /// </summary>
        private void LoadParam()
        {
            ViewState["CompanyID"] = GetDecryptParam("companyID", "0").ToNumber<int>();
            ViewState["CategoryID"] = GetDecryptParam("categoryID", "");
            ViewState["ListID"] = GetDecryptParam("listID", "");
        }

        /// <summary>
        /// 다국어 설정
        /// </summary>
        private void SetLanguage()
        {
            this.GetResourceManager();
            if (!this.resMng.Equals(null))
            {
            }
        }

        /// <summary>
        /// 메서드명 :
        /// 작성목적 : Request 파라미터 STRING 가져오기
        /// 작성자   : 심재운
        /// </summary>
        /// <param name="_parmName">Name of the _parm.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetParm(string _parmName, string _errorReturn)
        {
            try
            {
                return ((string)(Request.Params[_parmName] ?? _errorReturn) == "") ? _errorReturn : (string)(Request.Params[_parmName] ?? _errorReturn);
            }
            catch
            {
                return (Request.Params[_parmName] != null) ? Convert.ToString(Request.Params[_parmName]) : Convert.ToString(_errorReturn);
            }
        }

        private void SetDecryptString()
        {
            string param = GetParm("p", "!NULL");
            if (!string.IsNullOrEmpty(param) && param != "!NULL")
            {
                param = Server.UrlDecode(param);//디코딩
                decryString = Utility.DecryptString(param);
                decryString = Server.UrlDecode(decryString);//디코딩
            }
        }

        /// <summary>
        /// 작성자   : shimpark
        /// 작성목적 : code behind 단에서 복고화 하여 파라미터값 가져오기
        /// </summary>
        /// <param name="_keyName">Name of the _key.</param>
        /// <param name="_errorReturn">The _error return.</param>
        /// <returns></returns>
        protected string GetDecryptParam(string _keyName, string _errorReturn)
        {
            if (!string.IsNullOrEmpty(decryString))
            {
                return UtilHelper.GetParmforParameter(decryString, _keyName, _errorReturn);
            }

            return _errorReturn;

        }

        #endregion

    }
}