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
    /// SetDeptInfo의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    // [System.Web.Script.Services.ScriptService]
    public class SetDeptInfo : System.Web.Services.WebService
    {
        #region struct
        public struct DEPTINFO  // 받는 필드 정의
        {

            public string XSTAT;              // 전송건수
            public string XMSGS;              // 전송건수
            public string XDATS;              // 전송건수
            public string XTIMS;              // 전송건수
            public string XROWS;              // 전송건수

            public DEPTINFODETAIL[] arrDatas;

        }

        public struct DEPTINFODETAIL
        {
            public string DEPTCODE;       //조직코드
            public string DEPTNAME;       //조직명
            public string PARENTDEPT;     //상위조직코드
            public string DEPTORDER;      //순서
            public string SHORTNAME;      //조직줄임명
            public string ENGLISHNAME;    //조직영어명
            public string BEGDATUM;       //조직생성일
            public string ENDDATUM;       //조직종료일
            public string SHORT_D;        //조직레벨
            public string ZESOBJTX;       //상위조직명
            public string BTRTL;          //본사 공장 구분 , 본사의 경우 1000, 공장의 경우 2000            
        }

        #endregion


        #region LagacySetUserInfo : 인사사용자 내용을 XI 로 부터 받습니다.
        /// <summary>
        ///  공통 필드에 대한 사항을 XI 로 부터 받습니다.
        /// </summary>
        /// <param name="sap_datas"></param>
        /// <returns></returns>
        [WebMethod]
        public void LagacySetDeptInfo(DEPTINFO commonData)
        {
            try
            {
                if (commonData.arrDatas != null)
                {
                    using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
                    {
                        SetPersonManage objcode = new SetPersonManage();
                        Account.BE.DeptInfo deptinfo;

                        // 임시테이블 삭제
                        objcode.DelTempDept();

                        Zeospace.DeskPlus.Account.DAL.Dept dept = new Account.DAL.Dept();

                        for (int i = 0; i < commonData.arrDatas.Length; i++)
                        {
                            // 부서코드를 맵핑한다.
                            // 일단 들어오는 데이터 로그처리

                            WsException.WriteAssert("LagacySetDeptInfo DEPTCODE: " + commonData.arrDatas[i].DEPTCODE, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetDeptInfo DEPTNAME: " + commonData.arrDatas[i].DEPTNAME, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetDeptInfo PARENTDEPT: " + commonData.arrDatas[i].PARENTDEPT, AppSettingType.ACCOUNT);
                            WsException.WriteAssert("LagacySetDeptInfo DEPTORDER: " + commonData.arrDatas[i].DEPTORDER, AppSettingType.ACCOUNT);

                            // 임시테이블 interfacecode 입력 
                            objcode.SetTempDept(commonData.arrDatas[i].DEPTCODE);

                            // 있는지 여부 확인
                            int deptid = objcode.IsDept(commonData.arrDatas[i].DEPTCODE, 1);

                            // 부모 deptid 를 미리 구해온다.
                            int dpParentDeptID = objcode.GetDeptID("1", commonData.arrDatas[i].PARENTDEPT);

                            deptinfo = new Account.BE.DeptInfo();
                            deptinfo.ActiveDept = "1";
                            deptinfo.CompanyID = 1;
                            deptinfo.DeptCode = commonData.arrDatas[i].DEPTCODE;
                            deptinfo.DeptOrder = (commonData.arrDatas[i].DEPTORDER == "") ? 0 : Int32.Parse(commonData.arrDatas[i].DEPTORDER);
                            //deptinfo.DeptOrder = i;                        
                            deptinfo.ShortName = commonData.arrDatas[i].SHORTNAME;
                            deptinfo.NameBase = commonData.arrDatas[i].DEPTNAME;
                            deptinfo.NameKo = commonData.arrDatas[i].DEPTNAME;
                            deptinfo.NameEn = commonData.arrDatas[i].ENGLISHNAME;
                            //deptinfo.NameJa = "";
                            //deptinfo.NameFr = "";
                            //deptinfo.NameZhcn = "";
                            //deptinfo.NameZhtw = "";
                            deptinfo.ParentDeptID = dpParentDeptID;
                            deptinfo.WriteUserID = 1;
                            //deptinfo.Address2 = commonData.arrDatas[i].BTRTL; // 해당 부분은 address2 필드 이용                      

                            //if (commonData.arrDatas[i].PARENTDEPT == "")
                            //    deptinfo.RootDept = "1";
                            //else
                            //    deptinfo.RootDept = "0";

                            deptinfo.RootDept = "0";

                            deptinfo.DeptID = deptid;

                            // order 정보를 문자로 넣어주니, 숫자로 따로 
                            deptinfo.Description = deptinfo.DeptOrder.ToString();

                            if (deptid == 0)
                            {
                                if (deptinfo.ShortName == "")
                                {
                                    deptinfo.ShortName = commonData.arrDatas[i].DEPTNAME;
                                }

                                // insert
                                int deptSeq = dept.Add(deptinfo);
                                objcode.ModDeptOrder(deptSeq, deptinfo.DeptOrder);
                            }
                            else
                            {
                                // update
                                WsException.WriteAssert("update : " + deptinfo.DeptCode, AppSettingType.ACCOUNT);

                                var deptList = dept.GetDeptLists(1, deptid, true);
                                foreach (var info in deptList)
                                {
                                    deptinfo.ShortName = info.ShortName;
                                    deptinfo.Phone = info.Phone;
                                    deptinfo.Fax = info.Fax;
                                    deptinfo.Description = info.Description;
                                    deptinfo.ZipCode = info.ZipCode;
                                    deptinfo.Address1 = objcode.ToReverseXSS(info.Address1);
                                    deptinfo.Address2 = objcode.ToReverseXSS(info.Address2);
                                    deptinfo.HomePage = info.HomePage;
                                }

                                dept.Mod(deptinfo);
                                dept.ModParentDeptID(deptinfo.CompanyID, deptinfo.DeptID, deptinfo.ParentDeptID);
                                objcode.ModDeptOrder(deptid, deptinfo.DeptOrder);
                            }
                        }

                        // 임시테이블중 실테이블에 없는 데이터는 삭제 
                        objcode.DelModifyValueDept();

                        // deptid = parentdeptid 일 경우; ParentDeptID 를 null 처리
                        objcode.ModParentDeptID();

                        // ParentDeptID 가 최상위 부서일 경우, RootDept = 1 처리
                        objcode.ModRootDept();

                        ts.Complete();
                    }
                }

            }
            catch (Exception ex)
            {
                WsException.WriteException("LagacySetDeptInfo.asmx[SetUserInfo] : " + ex);       
            }
        }
        #endregion
    }
}
