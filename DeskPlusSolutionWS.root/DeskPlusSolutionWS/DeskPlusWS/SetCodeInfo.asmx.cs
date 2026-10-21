using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Zeospace.DeskPlus.Common.BE;
using Zeospace.DeskPlus.Common.BLL;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;
using System.Transactions;
using System.Data.SqlClient;

using System.Net;
using System.IO;
using System.Xml;

namespace Zeospace.DeskPlus.DeskPlusWS.Web
{
    /// <summary>
    /// SetCodeInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class SetCodeInfo : System.Web.Services.WebService
    {

        #region struct
        public struct CODEINFO  // 받는 필드 정의
        {

            public string XSTAT;              // 전송건수
            public string XMSGS;              // 전송건수
            public string XDATS;              // 전송건수
            public string XTIMS;              // 전송건수
            public string XROWS;              // 전송건수

            public CODEINFODETAIL[] arrDatas;

        }

        public struct CODEINFODETAIL
        {
            public string CODEGUBUN;     //구분 코드 직급 1, 직책 2
            public string INTERFACECODE;       //직급직책코드
            public string CODENAME;     //직급직책명
            public string CODENAMEEN;     //직급직책명
            public string CODEORDER;      //순서
        }

        #endregion


        #region LagacySetUserInfo : 인사사용자 내용을 XI 로 부터 받습니다.
        /// <summary>
        ///  공통 필드에 대한 사항을 XI 로 부터 받습니다.
        /// </summary>
        /// <param name="sap_datas"></param>
        /// <returns></returns>
        [WebMethod]
        public void LagacySetCodeInfo(CODEINFO commonData)
        {
            try
            {
                if (commonData.arrDatas != null)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        SetPersonManage objcode = new SetPersonManage();
                        Common.BE.CodeInfo codeinfo;

                        // 임시테이블 삭제
                        objcode.DelTempCode();

                        DataSet dsCodeAll = objcode.GetCodeAll("1");
                        DataView dvCodeAll = dsCodeAll.Tables[0].DefaultView;

                        Zeospace.DeskPlus.Common.DAL.Code code = new Common.DAL.Code();

                        //int tmpTitleOrder = 1;
                        int tmpDutyOrder = 1;

                        for (int i = 0; i < commonData.arrDatas.Length; i++)
                        {
                            // 일단 들어오는 데이터 로그처리
                            WsException.WriteAssert("LagacySetCodeInfo CODEGUBUN: " + commonData.arrDatas[i].CODEGUBUN, AppSettingType.COMMON);
                            WsException.WriteAssert("LagacySetCodeInfo INTERFACECODE: " + commonData.arrDatas[i].INTERFACECODE, AppSettingType.COMMON);
                            WsException.WriteAssert("LagacySetCodeInfo CODENAME: " + commonData.arrDatas[i].CODENAME, AppSettingType.COMMON);
                            WsException.WriteAssert("LagacySetCodeInfo CODEORDER: " + commonData.arrDatas[i].CODEORDER, AppSettingType.COMMON);

                            // 임시테이블 interfacecode 입력 
                            objcode.SetTempCode(commonData.arrDatas[i].INTERFACECODE);
                            //objcode.SetTempCode(commonData.arrDatas[i].CODEGUBUN + commonData.arrDatas[i].INTERFACECODE);

                            // 있는지 여부 확인
                            int codeID = 0;
                            codeID = objcode.GetCodeID(commonData.arrDatas[i].INTERFACECODE, 1);
                            //codeID = objcode.GetCodeID(commonData.arrDatas[i].CODEGUBUN + commonData.arrDatas[i].INTERFACECODE, 1);

                            codeinfo = new CodeInfo();
                            codeinfo.CodeID = codeID;
                            codeinfo.ActiveCode = "1";

                            if (commonData.arrDatas[i].CODEORDER != "")
                            {
                                codeinfo.CodeOrder = Int32.Parse(commonData.arrDatas[i].CODEORDER);
                            }
                            else
                            {
                                codeinfo.CodeOrder = 0;
                            }

                            /*
                            if (commonData.arrDatas[i].CODEGUBUN == "1")
                                codeinfo.CodeOrder = Int32.Parse(commonData.arrDatas[i].CODEORDER) + 1;
                            else if (commonData.arrDatas[i].CODEGUBUN == "2")
                                codeinfo.CodeOrder = tmpDutyOrder++;
                            */

                            codeinfo.CompanyID = 1;
                            codeinfo.Description = "";
                            codeinfo.InterfaceCode = commonData.arrDatas[i].INTERFACECODE;
                            //codeinfo.InterfaceCode = commonData.arrDatas[i].CODEGUBUN + commonData.arrDatas[i].INTERFACECODE;
                            codeinfo.ModifyDate = DateTime.Now;
                            codeinfo.ModifyUserID = 0;
                            codeinfo.NameBase = commonData.arrDatas[i].CODENAME;
                            codeinfo.NameKo = commonData.arrDatas[i].CODENAME;
                            codeinfo.NameEn = commonData.arrDatas[i].CODENAMEEN;

                            //codeinfo.NameEn = "";
                            //codeinfo.NameJa = "";
                            //codeinfo.NameFr = "";
                            //codeinfo.NameZhcn = "";
                            //codeinfo.NameZhtw = "";

                            //codeinfo.ParentCodeID = (commonData.arrDatas[i].CODEGUBUN == "1") ? 1 : 2;

                            if (commonData.arrDatas[i].CODEGUBUN == "1")
                                codeinfo.ParentCodeID = 1;
                            else if (commonData.arrDatas[i].CODEGUBUN == "2")
                                codeinfo.ParentCodeID = 2;


                            if (commonData.arrDatas[i].CODEGUBUN == "1")
                                codeinfo.Scope = "T";
                            else if (commonData.arrDatas[i].CODEGUBUN == "2")
                                codeinfo.Scope = "D";

                            codeinfo.Type = "U";

                            if (codeID > 0)
                            {
                                // 지회 홈페이지 정보
                                dvCodeAll.RowFilter = "interfacecode = '" + codeinfo.InterfaceCode + "'";
                                if (dvCodeAll.Count > 0)
                                {
                                    codeinfo.Extension1 = dvCodeAll[0]["Extension1"].ToString();
                                    codeinfo.Extension2 = dvCodeAll[0]["Extension2"].ToString();
                                    codeinfo.Description = dvCodeAll[0]["Description"].ToString();
                                    codeinfo.AccessID = dvCodeAll[0]["AccessID"].ToString();
                                }

                                // update 
                                code.Mod(codeinfo);
                                objcode.ModCodeOrder(codeID, codeinfo.CodeOrder);
                            }
                            else
                            {
                                // insert
                                codeinfo.AccessID = "0";

                                int codeSeq = code.Add(codeinfo);
                                objcode.ModCodeOrder(codeSeq, codeinfo.CodeOrder);
                            }

                        }

                        // 임시테이블중 실테이블에 없는 데이터는 삭제 
                        objcode.DelModifyValueCode();

                        ts.Complete();
                    }
                }

            }
            catch (Exception ex)
            {
                WsException.WriteException("LagacySetUserInfo.asmx[SetUserInfo] : " + ex);
            }

        }

        #endregion
    }
}
