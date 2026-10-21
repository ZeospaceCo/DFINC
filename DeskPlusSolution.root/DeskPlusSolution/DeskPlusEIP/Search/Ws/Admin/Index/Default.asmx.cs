using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using DateHelpers = Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper.WebComponent.DateHelper;
using System.Data;
using Zeospace.DeskPlus.Search.BLL;
using System.Collections;

namespace Zeospace.DeskPlus.Search.Web.Ws.Admin.Index
{
    /// <summary>
    /// List의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    [System.Web.Script.Services.ScriptService]
    public class Default : Base.BaseWSSearch
    {
        #region Optimize
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-22
        /// 작성목적 : 옵티마이져
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Optimize(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            try
            {
                SetDecryptString(p);
                string func = GetDecryptParam("f", "");
                string ids = GetDecryptParam("i", "");

                string[] funcArr = func.Split(',');
                string[] idArr = ids.Split(',');

                for (int i = 0; i < funcArr.Length; i++)
                {
                    //IndexLog Detail Message
                    Zeospace.DeskPlus.Search.BE.IndexLogDetailInfo indexDetailInfo = new BE.IndexLogDetailInfo();
                    indexDetailInfo.CompanyID = CurrentUserInfo.CompanyID;
                    indexDetailInfo.LogID = Convert.ToInt32(idArr[i]);

                    Zeospace.DeskPlus.Search.DAL.IndexLog dalIndexLog = new DAL.IndexLog();
                    Zeospace.DeskPlus.Search.DAL.IndexLogDetail dalIndexDetailLog = new DAL.IndexLogDetail();


                    switch (funcArr[i].ToUpper())
                    {
                        case "BOARD":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.Board.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "APPROVAL":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.Approval.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "EDM":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[EDM:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.EDM.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[EDM:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[EDM:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "ADDRESS":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[ADDRESS:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.Address.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[ADDRESS:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[ADDRESS:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "WEBHARD":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.WebHard.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "COOP":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[COOP:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.Coop.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[COOP:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[COOP:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "TASK":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[TASK:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.TAS.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[TASK:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "PIM":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[PIM:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.Pim.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[PIM:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[PIM:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        case "RM":
                            try
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[RM:OPTIMIZE START] " + GetResource("MSG_OPT_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.BLL.RM.Index.Optimize();

                                //Detail Log
                                indexDetailInfo.Message = "[RM:OPTIMIZE COMPLETE] " + GetResource("MSG_OPT_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                //Detail Log
                                indexDetailInfo.Message = "[RM:OPTIMIZE ERROR] " + GetResource("MSG_OPT_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                                return false;
                            }

                            break;
                        default:
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Optimize : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Index Add
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-22
        /// 작성목적 : 인덱스 생성
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Add(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            try
            {
                SetDecryptString(p);
                string func = GetDecryptParam("f", "");

                string[] funcArr = func.Split(',');

                for (int i = 0; i < funcArr.Length; i++)
                {
                    //IndexLog BE 초기화
                    Zeospace.DeskPlus.Search.BE.IndexLogInfo indexInfo = new BE.IndexLogInfo();
                    indexInfo.CompanyID = CurrentUserInfo.CompanyID;
                    indexInfo.LogID = 0;
                    indexInfo.FTRTextCount = 0;
                    indexInfo.IndexCount = 0;
                    indexInfo.Status = "P";
                    indexInfo.StartDate = DateTime.Now;
                    indexInfo.EndDate = DateTime.Now;
                    indexInfo.WriteUserID = CurrentUserInfo.UserID;


                    //IndexLog Detail Message
                    Zeospace.DeskPlus.Search.BE.IndexLogDetailInfo indexDetailInfo = new BE.IndexLogDetailInfo();
                    indexDetailInfo.CompanyID = CurrentUserInfo.CompanyID;

                    Zeospace.DeskPlus.Search.DAL.IndexLog dalIndexLog = new DAL.IndexLog();
                    Zeospace.DeskPlus.Search.DAL.IndexLogDetail dalIndexDetailLog = new DAL.IndexLogDetail();

                    int logID = 0;
                    int ftrTextCount = 0;
                    int indexCount = 0;

                    switch (funcArr[i].ToUpper())
                    {
                        case "BOARD":
                            #region BOARD
                            try
                            {
                                indexInfo.Category = "BOARD";
                                
                                logID  = dalIndexLog.Add(indexInfo); // 인덱스 생성시작
                                
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
                            }

                            break;
                            #endregion
                        case "TASK":
                            #region TASK
                            try
                            {
                                indexInfo.Category = "TASK";

                                logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                                indexInfo.LogID = logID;
                                indexDetailInfo.LogID = logID;

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:START] " + GetResource("MSG_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.DAL.TAS task = new Zeospace.DeskPlus.Search.DAL.TAS();
                                List<BE.TAS.AttachFileInfo> attachList = task.GetAttachAll();  // 첨부파일 리스트


                                //TXT 변환
                                ftrTextCount = Zeospace.DeskPlus.Search.BLL.TAS.Index.GetTextFile(attachList); //TEXT 파일 변환
                                indexInfo.FTRTextCount = ftrTextCount;

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                //File DB처리
                                Zeospace.DeskPlus.Search.BLL.TAS.Index.AddFTRIndex(attachList);

                                //List DB처리
                                indexCount = Zeospace.DeskPlus.Search.BLL.TAS.Index.AddIndex(task.GetListAll());
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
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

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
                            }

                            break;
                            #endregion
                        case "RM":
                            #region RM
                            try
                            {
                                indexInfo.Category = "RM";

                                logID = dalIndexLog.Add(indexInfo); // 인덱스 생성시작

                                indexInfo.LogID = logID;
                                indexDetailInfo.LogID = logID;

                                //Detail Log
                                indexDetailInfo.Message = "[RM:START] " + GetResource("MSG_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Search.DAL.RM rm = new Zeospace.DeskPlus.Search.DAL.RM();
                                List<BE.RM.AttachFileInfo> attachList = rm.GetAttachAll();  // 첨부파일 리스트


                                //TXT 변환
                                ftrTextCount = Zeospace.DeskPlus.Search.BLL.RM.Index.GetTextFile(attachList); //TEXT 파일 변환
                                indexInfo.FTRTextCount = ftrTextCount;

                                //Detail Log
                                indexDetailInfo.Message = "[RM:FTRTEXTCOUNT-" + ftrTextCount + "] " + GetResource("MSG_TXT");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                //File DB처리
                                Zeospace.DeskPlus.Search.BLL.RM.Index.AddFTRIndex(attachList);

                                //List DB처리
                                indexCount = Zeospace.DeskPlus.Search.BLL.RM.Index.AddIndex(rm.GetListAll());
                                indexInfo.IndexCount = indexCount;

                                //Detail Log
                                indexDetailInfo.Message = "[RM:INDEXCOUNT-" + indexCount + "] " + GetResource("MSG_CONTENT");
                                dalIndexDetailLog.Add(indexDetailInfo);


                                indexInfo.EndDate = DateTime.Now;
                                indexInfo.Status = "C";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[RM:COMPLETE] " + GetResource("MSG_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[RM:ERROR] " + GetResource("MSG_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                                return false;
                            }

                            break;
                            #endregion
                        default:
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Add : " + ex.Message, ex);
                return false;
            }
        }
        #endregion

        #region Index Delete
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-22
        /// 작성목적 : 인덱스 삭제
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public bool Del(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            try
            {
                SetDecryptString(p);
                string func = GetDecryptParam("f", "");
                string ids = GetDecryptParam("i", "");

                string[] funcArr = func.Split(',');
                string[] idArr = ids.Split(',');

                for (int i = 0; i < funcArr.Length; i++)
                {
                    //IndexLog BE 초기화
                    Zeospace.DeskPlus.Search.BE.IndexLogInfo indexInfo = new BE.IndexLogInfo();
                    indexInfo.CompanyID = CurrentUserInfo.CompanyID;
                    indexInfo.LogID = 0;
                    indexInfo.FTRTextCount = 0;
                    indexInfo.IndexCount = 0;
                    indexInfo.Status = "D";
                    indexInfo.StartDate = DateTime.Now;
                    indexInfo.EndDate = DateTime.Now;
                    indexInfo.WriteUserID = CurrentUserInfo.UserID;


                    //IndexLog Detail Message
                    Zeospace.DeskPlus.Search.BE.IndexLogDetailInfo indexDetailInfo = new BE.IndexLogDetailInfo();
                    indexDetailInfo.CompanyID = CurrentUserInfo.CompanyID;

                    Zeospace.DeskPlus.Search.DAL.IndexLog dalIndexLog = new DAL.IndexLog();
                    Zeospace.DeskPlus.Search.DAL.IndexLogDetail dalIndexDetailLog = new DAL.IndexLogDetail();

                    indexInfo.LogID = Convert.ToInt32(idArr[i]);
                    indexDetailInfo.LogID = Convert.ToInt32(idArr[i]);

                    switch (funcArr[i].ToUpper())
                    {
                        case "BOARD":
                            #region BOARD
                            try
                            {
                                indexInfo.Category = "BOARD";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.Board.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[BOARD:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "APPROVAL":
                            #region APPROVAL
                            try
                            {
                                indexInfo.Category = "APPROVAL";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.Approval.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[APPROVAL:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "EDM":
                            #region EDM
                            try
                            {
                                indexInfo.Category = "EDM";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[EDM:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.EDM.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[EDM:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[EDM:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "ADDRESS":
                            #region ADDRESS
                            try
                            {
                                indexInfo.Category = "ADDRESS";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[ADDRESS:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.Address.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[ADDRESS:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[EDM:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "WEBHARD":
                            #region WEBHARD
                            try
                            {
                                indexInfo.Category = "WEBHARD";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.WebHard.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[WEBHARD:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "COOP":
                            #region COOP
                            try
                            {
                                indexInfo.Category = "COOP";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[COOP:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.Coop.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[COOP:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[COOP:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "TASK":
                            #region TASK
                            try
                            {
                                indexInfo.Category = "TASK";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.TAS.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[TASK:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "PIM":
                            #region PIM
                            try
                            {
                                indexInfo.Category = "PIM";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[PIM:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.Pim.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[PIM:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[PIM:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        case "RM":
                            #region RM
                            try
                            {
                                indexInfo.Category = "RM";

                                dalIndexLog.Mod(indexInfo); // 로그 작성시작

                                //Detail Log
                                indexDetailInfo.Message = "[RM:DELETE START] " + GetResource("MSG_DEL_START");
                                dalIndexDetailLog.Add(indexDetailInfo);

                                bool rtnVal = Zeospace.DeskPlus.Search.BLL.RM.Index.DelIndex();

                                //Detail Log
                                indexDetailInfo.Message = "[RM:DELETE COMPLETE] " + GetResource("MSG_DEL_COMPLETE");
                                dalIndexDetailLog.Add(indexDetailInfo);
                            }
                            catch (Exception ex)
                            {
                                indexInfo.Status = "E";
                                dalIndexLog.Mod(indexInfo);

                                //Detail Log
                                indexDetailInfo.Message = "[RM:DELETE ERROR] " + GetResource("MSG_DEL_ERROR") + " " + ex.Message;
                                dalIndexDetailLog.Add(indexDetailInfo);

                                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                                return false;
                            }

                            break;
                            #endregion
                        default:
                            break;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Admin.Index.Default.Del : " + ex.Message);
                return false;
            }
        }
        #endregion

        #region GET
        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : Category 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetCategory(string p)
        {
            #region 다국어 리소스 로드/Title 정보
            this.ResourceName = "Zeospace.DeskPlus.Search.Web.Resource.GlobalString.string";		            //리소스 명 선언
            this.AssemblyName = typeof(Zeospace.DeskPlus.Search.Web.Ws.Admin.Index.Default).Assembly;	//어셈블리 선언
            this.GetResourceManager();
            #endregion

            try
            {
                SetDecryptString(p);

                DAL.IndexLog dal = new DAL.IndexLog();

                DataTableReader dtr = dal.GetCategory(CurrentUserInfo.CompanyID);

                List<object> listobj = new List<object>();

                //Index 기본값
                ArrayList indexArr = new ArrayList();
                indexArr.Add("BOARD");
                indexArr.Add("APPROVAL");
                indexArr.Add("EDM");
                indexArr.Add("ADDRESS");
                indexArr.Add("PIM");
                indexArr.Add("RM");
                indexArr.Add("SNS");
                indexArr.Add("WEBHARD");
                indexArr.Add("COOP");
                indexArr.Add("TASK");

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.IndexLogInfo info = new BE.IndexLogInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.LogID = dtr["LogID"].ToString().ToNumber<int>();
                            info.Category = dtr["Category"].ToString();

                            indexArr.Remove(dtr["Category"].ToString()); // 사용된 색인은 삭제함

                            DateTime dt;
                            if (DateTime.TryParse(dtr["StartDate"].ToString(), out dt))
                                info.StartDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.StartDateStr = string.Empty;


                            if (DateTime.TryParse(dtr["EndDate"].ToString(), out dt))
                                info.EndDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.EndDateStr = string.Empty;


                            info.StartDate = (dtr["StartDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dtr["StartDate"];
                            info.EndDate = (dtr["EndDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)dtr["EndDate"];

                            info.FTRTextCount = dtr["FTRTextCount"].ToString().ToNumber<int>();
                            info.IndexCount = dtr["IndexCount"].ToString().ToNumber<int>();
                            info.Status = dtr["Status"].ToString();
                            //switch (dtr["Status"].ToString())
                            //{
                            //    case "P":
                            //        info.Status = this.GetResource("StatusP");
                            //        break;
                            //    case "C":
                            //        info.Status = GetResource("StatusC");
                            //        break;
                            //    case "E":
                            //        info.Status = GetResource("StatusE");
                            //        break;
                            //    case "D":
                            //        info.Status = GetResource("StatusD");
                            //        break;
                            //    default :
                            //        info.Status = GetResource("StatusN");
                            //        break;
                            //}

                            if (dtr["EndDate"] != DBNull.Value)
                            {
                                info.ExecTime = info.EndDate.Subtract(info.StartDate).ToString("g");
                            }
                            else
                            {
                                info.ExecTime = string.Empty;
                            }

                            listobj.Add(info);
                        }


                    }
                }

                for (int i = 0; i < indexArr.Count; i++)
                {
                    InitCategory(ref listobj, indexArr[i].ToString());
                }


                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : Log detail 가져오기
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [System.Web.Script.Services.ScriptMethod(UseHttpGet = false)]
        public List<object> GetLogDetail(string p)
        {
            try
            {
                SetDecryptString(p);

                string logIDs = GetDecryptParam("logID", "");
                if (logIDs == "undefined") return new List<object>();
                DAL.IndexLogDetail dal = new DAL.IndexLogDetail();

                DataTableReader dtr = dal.GetLog(CurrentUserInfo.CompanyID, logIDs);

                List<object> listobj = new List<object>();

                if (dtr != null)
                {
                    if (dtr.HasRows)
                    {
                        while (dtr.Read())
                        {
                            BE.IndexLogDetailInfo info = new BE.IndexLogDetailInfo();

                            info.CompanyID = dtr["CompanyID"].ToString().ToNumber<int>();
                            info.LogID = dtr["LogID"].ToString().ToNumber<int>();
                            info.LogDetailID = dtr["LogDetailID"].ToString().ToNumber<int>();
                            info.Message = dtr["Message"].ToString();

                            DateTime dt;
                            if (DateTime.TryParse(dtr["WriteDate"].ToString(), out dt))
                                info.WriteDateStr = DateHelpers.GetCultureDate(dt, CurrentUserInfo.PreferredLanguage, true);
                            else
                                info.WriteDateStr = string.Empty;                            


                            listobj.Add(info);
                        }
                    }
                }

                return listobj;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// 작성자 : freeman
        /// 작성일자 : 2013-11-28
        /// 작성목적 : Category 초기값 설정
        /// </summary>
        /// <param name="_listobj"></param>
        /// <param name="_category"></param>
        private void InitCategory(ref List<object> _listobj, string _category)
        {
            BE.IndexLogInfo appInfo = new BE.IndexLogInfo();
            appInfo.Category = _category;
            appInfo.Status = GetResource("StatusN");
            appInfo.StartDateStr = string.Empty;
            appInfo.EndDateStr = string.Empty;
            appInfo.ExecTime = string.Empty;
            _listobj.Add(appInfo);
        }
        #endregion
    }
}
