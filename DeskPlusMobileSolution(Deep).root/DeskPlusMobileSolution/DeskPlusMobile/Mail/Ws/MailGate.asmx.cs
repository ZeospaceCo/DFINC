using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using Zeospace.DeskPlusMobile.Web.Helper;
using System.IO;
using System.Xml;
using System.Text;
using HtmlAgilityPack;
using System.Text.RegularExpressions;


namespace Zeospace.DeskPlusMobile.Web.Mail.Ws
{
    /// <summary>
    /// MailGate의 요약 설명입니다.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // ASP.NET AJAX를 사용하여 스크립트에서 이 웹 서비스를 호출하려면 다음 줄의 주석 처리를 제거합니다. 
    //[System.Web.Script.Services.ScriptService]
    public class MailGate : System.Web.Services.WebService
    {
        #region Struct  리턴용 구조체 만들기

        #region 1. 조직도
        public class organization
        {
            public struct data1
            {
                public string returnCode;
                public string returnMsg;

                public Item1[] result;
            }

            public struct Item1
            {
                public string type;
                public string parentkey;
                public string key;
                public string email;
                public string name;
                public string rank;
            }
        }
        #endregion

        #region 2. 계정 SSO (ID 기준)
        public class sso
        {
            public struct data2
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 3. 계정 SSO (사번 기준) - 설치 시, 사번 마이그레이션을 해야 하므로 DEEPSoft 와 사전 협의 필요
        public class ssoByStaffNo
        {
            public struct data3
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 4. 계정 로그아웃
        public class logout
        {
            public struct data4
            {
                public string returnCode;
                public string returnUrl;
            }
        }
        #endregion

        #region 5. 계정 추가
        public class add
        {
            public struct data5
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 6. 계정 수정
        public class modify
        {
            public struct data6
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 7. 계정 삭제
        public class delete
        {
            public struct data7
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 8. 계정 존재여부
        public class isExist
        {
            public struct data8
            {
                public string returnCode;
                public string returnMsg;

                public string result;
            }
        }
        #endregion

        #region 9. 계정 비밀번호 확인
        public class checkPw
        {
            public struct data9
            {
                public string returnCode;
                public string returnMsg;

                public string result;
            }
        }
        #endregion

        #region 10. 계정 인증 - pw 항목이 DEEPSoft 암호화 알고리즘을 사용해야 하므로 DEEPSoft 와 사전 협의 필요
        public class auth
        {
            public struct data10
            {
                public string returnCode;
                public string returnMsg;

                public string result;
            }

        }
        #endregion

        #region 11. 계정 편지 건수
        public class mailCount
        {
            public struct data11
            {
                public string returnCode;
                public string returnMsg;

                public string result;
            }
        }
        #endregion

        #region 12. 계정 편지 목록
        public class mailList
        {
            public struct data12
            {
                public string returnCode;
                public string returnMsg;

                public Item12[] result;
            }

            //public struct Result12
            //{
            //    public Item12[] item;
            //}

            public struct Item12
            {
                public string uid;
                public string subject;
                public string from;
                public string to;
                public string cc;
                public string bcc;
                public string sendDate;
                public string recvDate;
                public string flag;
                public string isAttach;
                public string isRead;                
            }
        }
        #endregion

        #region 13. 계정 수신확인 목록
        public class confirmList
        {
            public struct data13
            {
                public string returnCode;
                public string returnMsg;

                public Item13[] result;
            }

            //public struct Result13
            //{
            //    public Item13[] item13;
            //}

            public struct Item13
            {
                public string uid;
                public string subject;
                public string sender;
                public string receiver;
                public string sendDate;
                public string recvDate;
                public string lastDate;
                public string status;
            }
        }
        #endregion

        #region 14. 전체계정 중 새편지 있는 계정목록
        public class listHasNew
        {
            public struct data14
            {
                public string returnCode;
                public string returnMsg;

                public Item14[] result;
            }

            //public struct Result14
            //{
            //    public Item14[] item;
            //}

            public struct Item14
            {
                public string id;
            }
        }
        #endregion

        #region 15. 계정메일 삭제
        public class mailDelete
        {
            public struct data15
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 16. 계정메일 열람 상태 변경
        public class mailStatus
        {
            public struct data16
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 17. 계정메일 이동
        public class mailMove
        {
            public struct data17
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 18. 계정메일 발송
        public class mailSend
        {
            public struct data18
            {
                public string returnCode;
                public string returnMsg;
            }
        }
        #endregion

        #region 19. 계정 메일 읽기
        public class mailRead
        {
            public struct data19
            {
                public string returnCode;
                public string returnMsg;

                public Item19 result;
            }

            public struct Item19
            {                
                public string uid;
                public string prevUid;
                public string nextUid;
                public string subject;
                public string from;
                public string to;
                public string cc;
                public string bcc;
                public string sendDate;
                public string recvDate;
                public string content;
                public string flag;
                public AttachList[] attachList;                
            }

            public struct AttachList
            {
                public string attachFile;
                public string name;
                public string size;
                public string downloadUrl;
            }
        }
        #endregion

        #endregion

        #region 케이스별 서비스 메서드 Sample

        #region 1.조직도
        // 1. 조직도 관련 정보 픽스 데이터 리턴 
        [WebMethod(EnableSession = true)]
        public organization.data1 Organization(string act, string domain, string key, string language)
        {
            organization.data1 rtn = new organization.data1();

            organization.Item1[] arrItem = new organization.Item1[2];

            arrItem[0].type = "0";
            arrItem[0].parentkey = "1";
            arrItem[0].key = "1";
            arrItem[0].email = "seolove777@zeospace.com";
            arrItem[0].name = "서병각";
            arrItem[0].rank = "2";

            arrItem[1].type = "1";
            arrItem[1].parentkey = "1";
            arrItem[1].key = "2";
            arrItem[1].email = "deskplus@zeospace.com";
            arrItem[1].name = "홍길동";
            arrItem[1].rank = "1";

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = arrItem;

            return rtn;
        }
        #endregion

        #region  2.계정 SSO (ID 기준)
        // 2. 계정 SSO (ID 기준)
        [WebMethod(EnableSession = true)]
        public sso.data2 Sso(string act, string domain, string id, string pw, string language, string lp, string mbox, string msgkey, string to, string content, string hideTopMenu)
        {
            // act=sso&amp;domain={0}&amp;id={1}&amp;pw={2}&amp;language={3}&amp;lp={4}&amp;mbox={5}&amp;msgkey={6}&amp;to={7}&amp;content={8}&amp;hideTopMenu={9}
            sso.data2 rtn = new sso.data2();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";

            return rtn;
        }
        #endregion

        #region  3.계정 SSO (사번 기준)
        // 3. 계정 SSO (사번 기준)
        [WebMethod(EnableSession = true)]
        public ssoByStaffNo.data3 SsoByStaffNo(string act, string domain, string staffno, string pw, string language, string lp, string mbox, string msgkey, string noPw)
        {
            ssoByStaffNo.data3 rtn = new ssoByStaffNo.data3();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";

            return rtn;
        }
        #endregion

        #region 4. 계정 로그아웃
        // 4. 계정 로그아웃
        [WebMethod(EnableSession = true)]
        public logout.data4 Logout(string act, string returnUrl)
        {
            logout.data4 rtn = new logout.data4();

            rtn.returnCode = "0";
            rtn.returnUrl = returnUrl;// "Success";

            return rtn;
        }
        #endregion

        #region 5. 계정 추가
        // 5. 계정 추가
        [WebMethod(EnableSession = true)]
        public add.data5 Add(string act, string domain, string id, string pw, string name)
        {
            add.data5 rtn = new add.data5();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";

            return rtn;
        }
        #endregion

        #region 6. 계정 수정
        // 6. 계정 수정
        [WebMethod(EnableSession = true)]
        public modify.data6 Modify(string act, string domain, string id, string pw, string newPw, string newStatus)
        {
            modify.data6 rtn = new modify.data6();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";

            return rtn;
        }
        #endregion

        #region 7. 계정 삭제
        // 7. 계정 삭제
        [WebMethod(EnableSession = true)]
        public delete.data7 Delete(string act, string domain, string id, string pw)
        {
            delete.data7 rtn = new delete.data7();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";

            return rtn;
        }
        #endregion

        #region 8. 계정 존재여부
        // 8. 계정 존재여부
        [WebMethod(EnableSession = true)]
        public isExist.data8 IsExist(string act, string domain, string id)
        {
            isExist.data8 rtn = new isExist.data8();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = "1";

            return rtn;
        }
        #endregion

        #region 9. 계정 비밀번호 확인
        // 9. 계정 비밀번호 확인
        [WebMethod(EnableSession = true)]
        public checkPw.data9 CheckPw(string act, string domain, string id, string pw)
        {
            checkPw.data9 rtn = new checkPw.data9();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = "1";

            return rtn;
        }
        #endregion

        #region 10. 계정 인증 - pw 항목이 DEEPSoft 암호화 알고리즘을 사용해야 하므로 DEEPSoft 와 사전 협의 필요
        // 10. 계정 인증 - pw 항목이 DEEPSoft 암호화 알고리즘을 사용해야 하므로 DEEPSoft 와 사전 협의 필요
        [WebMethod(EnableSession = true)]
        public auth.data10 Auth(string act, string domain, string id, string pw)
        {
            auth.data10 rtn = new auth.data10();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = "1";

            return rtn;
        }
        #endregion

        #region 11. 계정 편지 건수
        // 11. 계정 편지 건수
        [WebMethod(EnableSession = true)]
        public mailCount.data11 MailCount(string act, string domain, string id, string pw, string mbox, string type)
        {
            mailCount.data11 rtn = new mailCount.data11();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = "100";

            return rtn;
        }
        #endregion

        #region 12. 계정 편지 목록
        // 12. 계정 편지 목록
        [WebMethod(EnableSession = false)]
        public mailList.data12 MailList(string act, string domain, string id, string pw, string mbox, string type, string count, string offset)
        {
            mailList.data12 rtn = new mailList.data12();

            mailList.Item12[] arrItem = new mailList.Item12[2];            

            arrItem[0].uid = "1234";
            arrItem[0].subject = "테스트 메일입니다.";
            arrItem[0].from = "송신자이름(혹은 메일주소)";
            arrItem[0].to = "수신자이름(혹은 메일즈소)";
            arrItem[0].cc = "참조자 이름(혹은 메일주소)";
            arrItem[0].bcc = "숨은 참조자 이름(혹은 메일주소)";
            arrItem[0].sendDate = "2013/01/01 00:00:00";
            arrItem[0].recvDate = "2013/01/01 00:00:00";
            arrItem[0].flag = "1";
            arrItem[0].isAttach = "true";
            arrItem[0].isRead = "0";

            arrItem[1].uid = "1234";
            arrItem[1].subject = "테스트 메일입니다.";
            arrItem[1].from = "송신자이름(혹은 메일주소)";
            arrItem[1].to = "수신자이름(혹은 메일즈소)";
            arrItem[1].cc = "참조자 이름(혹은 메일주소)";
            arrItem[1].bcc = "숨은 참조자 이름(혹은 메일주소)";
            arrItem[1].sendDate = "2013/01/01 00:00:00";
            arrItem[1].recvDate = "2013/01/01 00:00:00";
            arrItem[1].flag = "1";
            arrItem[1].isAttach = "true";
            arrItem[1].isRead = "0";

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = arrItem;

            return rtn;
        }
        #endregion

        #region 13. 계정 수신확인 목록
        // 13. 계정 수신확인 목록
        [WebMethod(EnableSession = true)]
        public confirmList.data13 ConfirmList(string act, string domain, string id, string date)
        {
            confirmList.data13 rtn = new confirmList.data13();

            confirmList.Item13[] arrItem = new confirmList.Item13[2];

            arrItem[0].uid = "34klkl4j54l54l352";
            arrItem[0].subject = "첫번째 송신 메일";
            arrItem[0].sender = "deskplus@zeospace.com";
            arrItem[0].receiver = "seolove777@zeospace.com";
            arrItem[0].sendDate = "2013/08/01 00:00:00";
            arrItem[0].recvDate = "2013/08/01 00:00:00";
            arrItem[0].lastDate = "2013/08/01 00:00:00";
            arrItem[0].status = "3";

            arrItem[1].uid = "sdf789s7fdfdfg";
            arrItem[1].subject = "두번째 송신 메일";
            arrItem[1].sender = "deskplus@zeospace.com";
            arrItem[1].receiver = "withsuki@zeospace.com";
            arrItem[1].sendDate = "2013/08/02 00:00:00";
            arrItem[1].recvDate = "2013/08/02 00:00:00";
            arrItem[1].lastDate = "2013/08/02 00:00:00";
            arrItem[1].status = "2";

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = arrItem;

            return rtn;
        }
        #endregion

        #region 14. 전체계정 중 새편지 있는 계정목록
        // 14. 전체계정 중 새편지 있는 계정목록
        [WebMethod(EnableSession = true)]
        public listHasNew.data14 ListHasNew(string act, string domain, string lastTime)
        {
            listHasNew.data14 rtn = new listHasNew.data14();

            listHasNew.Item14[] arrItem = new listHasNew.Item14[2];

            arrItem[0].id = "master";
            arrItem[1].id = "babo";

            rtn.returnCode = "0";
            rtn.returnMsg = "Success";
            rtn.result = arrItem;

            return rtn;
        }
        #endregion

        #region 15. 계정메일 삭제
        // 15. 계정메일 삭제
        [WebMethod(EnableSession = true)]
        public mailDelete.data15 MailDelete(string act, string domain, string id, string pw, string mbox, string uid)
        {
            mailDelete.data15 rtn = new mailDelete.data15();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success to delete a mail";

            return rtn;
        }
        #endregion

        #region 16. 계정메일 열람 상태 변경
        // 16. 계정메일 열람 상태 변경
        [WebMethod(EnableSession = true)]
        public mailStatus.data16 MailStatus(string act, string domain, string id, string pw, string mbox, string uid)
        {
            mailStatus.data16 rtn = new mailStatus.data16();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success to modify a mail status";

            return rtn;
        }
        #endregion

        #region 17. 계정 메일 이동
        // 17. 계정 메일 이동
        [WebMethod(EnableSession = true)]
        public mailMove.data17 MailMove(string act, string domain, string id, string pw, string mbox, string uid, string targetMbox)
        {
            mailMove.data17 rtn = new mailMove.data17();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success to move a mail";

            return rtn;
        }
        #endregion

        #region 18. 계정 메일 발송
        // 18. 계정 메일 발송
        [WebMethod(EnableSession = true)]
        public mailSend.data18 MailSend(string act, string domain, string id, string pw, string to, string cc, string bcc, string subject, string mbox, string forward, string content)
        {
            mailSend.data18 rtn = new mailSend.data18();

            rtn.returnCode = "0";
            rtn.returnMsg = "Success to Send a mail";

            return rtn;
        }
        #endregion

        #region 19. 계정 메일 읽기
        // 19. 계정 메일 읽기
        [WebMethod(EnableSession = true)]
        public mailRead.data19 MailRead()
        {
            mailRead.data19 rtn = new mailRead.data19();

            mailRead.Item19 item = new mailRead.Item19();
            mailRead.AttachList[] attachList = new mailRead.AttachList[2];

            item.uid = "47";
            item.prevUid = "46";
            item.nextUid = "48";
            item.subject = "테스트 메일입니다.";
            item.from = "송신자 이름(혹은 메일주소)";
            item.to = "수신자 이름(혹은 메일주소)";
            item.cc = "참조자 이름(혹은 메일주소)";
            item.bcc = "숨은참조자 이름(혹은 메일주소)";
            item.sendDate = "2013/01/01 00:00:00";
            item.recvDate = "2013/01/01 00:00:00";
            item.content = "메일 내용";
            item.flag = "1";

            //첨부파일 목록
            //attachList[0].

            return rtn;
        }
        #endregion

        #endregion
    }
}
