using System;
using System.Text;
using System.IO;
using System.Web;
using System.Threading;
using System.Collections;

namespace Zeospace.DeskPlusMobile.Web.Helper
{
    public class SendMail
    {
        private string lineEnd = "\r\n";
        private int DATA_BLOCK_SIZE = 100;
        private readonly string BOUDARYSTRING = "----NextPart_ZEOSPACE_DESKPLUSMOBILE";

        /// <summary>
        /// 생성자
        /// </summary>
        public SendMail()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 확장 옵션
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_xExtention"></param>
        /// <param name="_addAttach"></param>
        /// <param name="_name"></param>
        /// <param name="_email"></param>
        /// <returns></returns>
        public bool Send(MailDataInfo _mailDataInfo, bool _xExtention, bool _addAttach, string _name)
        {
            try
            {
                if (!(_mailDataInfo.To.Count.Equals(0) && _mailDataInfo.Cc.Count.Equals(0) && _mailDataInfo.Bcc.Count.Equals(0)))
                {
                    string xExtentionString = String.Empty;

                    if (_xExtention)
                    {
                        string msg = GetEmail(_mailDataInfo.From) + ";no_save_attach=0;alert_message=";
                        xExtentionString = "X-Zmail-Savemail: =?utf-8?B?" + Convert.ToBase64String(Encoding.GetEncoding("utf-8").GetBytes(msg)) + "?=\r\n";
                    }
                    //Email 파일 생성
                    SaveEml(_mailDataInfo, "/DeskPlusMobile/EmlSpool", _addAttach, xExtentionString, _name);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Helper.SendMail[Send]", ex);
                return false;
            }
        }




        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 메일 파일 생성 확장 옵션
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_emlSpoolPath"></param>
        /// <param name="_addAttach"></param>
        /// <param name="_xExtention"></param>
        /// <param name="_name"></param>
        /// <returns></returns>
        public string SaveEml(MailDataInfo _mailDataInfo, string _emlSpoolPath, bool _addAttach, string _xExtention, string _name)
        {
            try
            {
                //if (_mailDataInfo.From == "" || _mailDataInfo.From == null) 
                if (_mailDataInfo.MailMessageID == "") _mailDataInfo.MailMessageID = GetGUID() + "@" + _mailDataInfo.From;
                StringBuilder message = new StringBuilder();
                message.Append("Message-ID: " + _mailDataInfo.MailMessageID + lineEnd);

                int endInt = _mailDataInfo.From.IndexOf("<");
                //int endInt2 = _mailDataInfo.From.IndexOf("(");

                string from = GetEmail(_mailDataInfo.From) + " <" + GetEmail(_mailDataInfo.From) + ">";

                if (endInt > -1)
                {
                    from = "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_mailDataInfo.From.Substring(0, endInt))) + "?=" + " <" + GetEmail(_mailDataInfo.From) + ">";
                }

                /*
                if (endInt < -1)
                {
                    from = "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_mailDataInfo.From.Substring(0, endInt))) + "?=" + " <" + GetEmail(_mailDataInfo.From) + ">";
                }
                else
                {
                    from = "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_name)) + "?=" + " <" + GetEmail(_mailDataInfo.From) + ">";
                }
                */


                message.Append("From: " + from + lineEnd);

                StringBuilder mailTo = new StringBuilder();
                string mailToStr = "";
                int checkIndex = 0;
                string newTo = "";

                if (_mailDataInfo.To != null && _mailDataInfo.To.Count > 0)
                {
                    foreach (string to in _mailDataInfo.To.Keys)
                    {
                        checkIndex = to.IndexOf("<");
                        if (checkIndex > 0)
                        {
                            newTo = "\"" + to.Substring(0, checkIndex) + "\"";
                        }
                        else
                        {
                            newTo = "\"" + to + "\"";
                        }

                        mailTo.Append("\t" + "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(newTo)) + "?=" + " <" + GetEmail(to) + ">, " + lineEnd);
                    }

                    mailToStr = mailTo.ToString().Substring(0, mailTo.Length - 4);

                    message.Append("To: " + mailToStr + lineEnd);

                }
                else
                {
                    return "";
                }

                //Helper.AppException.WriteAssert(AppSettingType.SQLMail, "To : " + message.ToString());

                if (_mailDataInfo.Cc != null && _mailDataInfo.Cc.Count > 0)
                {
                    StringBuilder mailCc = new StringBuilder();
                    string mailCcStr = "";
                    string newCc = "";
                    foreach (string cc in _mailDataInfo.Cc.Keys)
                    {
                        checkIndex = cc.IndexOf("<");
                        if (checkIndex > 0)
                        {
                            newCc = cc.Substring(0, checkIndex);
                        }
                        else
                        {
                            newCc = cc;
                        }

                        mailCc.Append("\t" + "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(newCc)) + "?=" + " <" + GetEmail(cc) + ">, " + lineEnd);
                    }
                    mailCcStr = mailCc.ToString().Substring(0, mailCc.Length - 4);

                    message.Append("Cc: " + mailCcStr + lineEnd);
                }

                //Helper.AppException.WriteAssert(AppSettingType.SQLMail, "cc : " + message.ToString());

                message.Append("Subject: =?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_mailDataInfo.Subject)) + "?=" + lineEnd);
                //날짜값이 현재시간을 가리키지 못해서 변경하였습니다. 
                //message.Append("Date: " + DateTime.Now.ToUniversalTime().ToString("r") + lineEnd);
                message.Append("Date: " + DateTime.Now.AddHours(-9).ToString("r") + lineEnd);
                message.Append("MIME-Version: 1.0" + lineEnd);
                message.Append("Content-Type: multipart/mixed;" + lineEnd);
                message.Append("\tboundary=\"" + BOUDARYSTRING + "\"" + lineEnd);
                message.Append("Content-Transfer-Encoding: base64" + lineEnd);
                message.Append("X-Priority: " + _mailDataInfo.Important + lineEnd);
                message.Append("This is a multi-part message in MIME format" + lineEnd + lineEnd);

                ///////////////////////////////////////////////////////////
                // PLAIN
                // 바운더리 지정. 헤더 다음에 바운더리 그리고 Content-type 이 바로 나와야 한다.
                // 공백 있으면 안됨
                message.Append("--" + BOUDARYSTRING + lineEnd);
                // 메일 바디
                // Content-Type
                message.Append("Content-Type: text/html;charset=\"" + _mailDataInfo.CharacterSet + "\"" + lineEnd);
                // Content-Transfer-Encoding
                message.Append("Content-Transfer-Encoding: base64" + lineEnd);

                if ((_mailDataInfo.Contents == null) || (_mailDataInfo.Contents == ""))
                {
                    _mailDataInfo.Contents = "&nbsp;";
                }

                string contentEncoding = Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_mailDataInfo.Contents));

                byte[] cotentByteArr = Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(contentEncoding);

                StringBuilder contentEncoded = new StringBuilder();
                using (MemoryStream ms = new MemoryStream(cotentByteArr))
                {
                    byte[] buffer = new byte[DATA_BLOCK_SIZE];
                    int bytesRead;

                    do
                    {
                        bytesRead = ms.Read(buffer, 0, buffer.Length);

                        contentEncoded.Append(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetString(buffer, 0, bytesRead) + lineEnd);
                    }
                    while (bytesRead == buffer.Length);

                    ms.Close();
                }

                message.Append(lineEnd + contentEncoded.ToString() + lineEnd + lineEnd);


                if (_mailDataInfo.Attach == null || _mailDataInfo.Attach.Count == 0)
                {
                    message.Append("--" + BOUDARYSTRING + "--" + lineEnd + lineEnd);
                }
                else
                {
                    //첨부파일을 Base64 인코딩하여 가져오기
                    if (_addAttach) message.Append(GetAttachedFile(_mailDataInfo.Attach, _mailDataInfo.CharacterSet));
                }

                string xSender = "";
                xSender = "x-sender: " + GetEmail(_mailDataInfo.From) + lineEnd;

                StringBuilder xReceiver = new StringBuilder();
                Hashtable allReceive = GetAllReceive(_mailDataInfo);

                if (_emlSpoolPath == "")
                {
                    _emlSpoolPath = "/DeskPlusMobile/EmlSpool";
                }

                //eml을 저장할 경로
                HttpContext context = HttpContext.Current;
                string emlSpoolPath = context.Server.MapPath(_emlSpoolPath);

                if (!Directory.Exists(emlSpoolPath)) Directory.CreateDirectory(emlSpoolPath);

                string savePath = emlSpoolPath + @"\" + GetGUID() + ".eml";

                foreach (Hashtable allTemp in allReceive.Keys)
                {
                    //xReceiver.Length = 0;
                    foreach (string temp in allTemp.Keys)
                    {
                        xReceiver.Append("x-receiver: " + temp + lineEnd);
                    }
                }

                using (StreamWriter tw = File.CreateText(savePath))
                {
                    tw.Write(xSender + xReceiver.ToString() + _xExtention + message);
                    tw.Close();
                }

                return savePath;
            }
            catch (Exception ex)
            {
                Helper.AppException.WriteException("Helper.SendMail[SaveEml]", ex);
                return "";
            }
        }


        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : GUID 생성
        /// </summary>
        /// <returns></returns>
        public string GetGUID()
        {
            Guid tmpGUID = Guid.NewGuid();
            string guid = tmpGUID.ToString().Replace("-", "");

            return guid;
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 메일을 발송할 모든 Email 주소 가져오기
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <returns></returns>
        private Hashtable GetAllReceive(MailDataInfo _mailDataInfo)
        {
            Hashtable AllReceive = new Hashtable();
            string strEmail = "";
            string strDomain = "";

            foreach (string tempEmail in _mailDataInfo.To.Keys)
            {
                strEmail = GetEmail(tempEmail);
                strDomain = strEmail.Split('@')[1];

                if (!AllReceive.ContainsKey(strEmail))
                {
                    AllReceive.Add(strEmail, strDomain);
                }
            }

            if (_mailDataInfo.Cc != null && _mailDataInfo.Cc.Count > 0)
            {
                foreach (string tempEmail in _mailDataInfo.Cc.Keys)
                {
                    strEmail = GetEmail(tempEmail);
                    strDomain = strEmail.Split('@')[1];

                    if (!AllReceive.ContainsKey(strEmail))
                    {
                        AllReceive.Add(strEmail, strDomain);
                    }
                }
            }

            if (_mailDataInfo.Bcc != null && _mailDataInfo.Bcc.Count > 0)
            {
                foreach (string tempEmail in _mailDataInfo.Bcc.Keys)
                {
                    strEmail = GetEmail(tempEmail);
                    strDomain = strEmail.Split('@')[1];

                    if (!AllReceive.ContainsKey(strEmail))
                    {
                        AllReceive.Add(strEmail, strDomain);
                    }
                }
            }


            Hashtable resultHash = new Hashtable();
            Hashtable tempHash = null;

            foreach (string domain in AllReceive.Values)
            {
                tempHash = new Hashtable();

                foreach (string email in AllReceive.Keys)
                {
                    if (domain.Equals(email.Split('@')[1]))
                    {
                        if (!tempHash.ContainsKey(email))
                        {
                            tempHash.Add(email, domain);
                        }
                    }
                }

                if (tempHash.Count > 0)
                {
                    if (!resultHash.ContainsValue(domain))
                    {
                        resultHash.Add(tempHash, domain);
                    }
                }
            }

            return resultHash;
        }



        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 첨부 파일을 읽어들여서 인코딩 하고 메일에 추가 한다.
        /// </summary>
        private StringBuilder GetAttachedFile(Hashtable _attach, string _characterSet)
        {
            string fileFullPath = "";

            // 순수 파일 이름을 저장하는 변수
            string ordFileName = null;
            StringBuilder fileString = new StringBuilder();

            int idx;

            /*
            // 첨부파일이 존재. 형식에 맞춰 스트링으로 만듬.
            foreach (string fileName in _attach.Values)
            {
                fileFullPath = fileName;

                // \ 이것이 마지막으로 되는 인덱스를 저장하기 위한 변수
                idx = fileName.LastIndexOf("/");
                // 진짜 파일 이름을 뽑아온다.
                ordFileName = fileName.Substring(idx + 1);

                fileString.Append("--" + BOUDARYSTRING + "" + lineEnd);
                // 파일에 대한 타입 처리
                fileString.Append("Content-Type: application/octet-stream;");
                // 파일 네임 집어 넣는다. name -> 첨부 이름 같음
                fileString.Append("name=\"=?" + _characterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_characterSet).GetBytes(ordFileName)) + "?=\"" + lineEnd);
                // Content-Transfer-Encoding 타입-base 64
                fileString.Append("Content-Transfer-Encoding: base64" + lineEnd);
                // Content-Disposition: attachment 
                fileString.Append("Content-Disposition: attachment; ");
                // 진짜 파일 네임 지정
                fileString.Append("filename=\"=?" + _characterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_characterSet).GetBytes(ordFileName)) + "?=\"" + lineEnd);
                // 한칸 띄움
                fileString.Append(lineEnd);

                StringBuilder EncodingResult = EncodeFileToBase64(fileName, _characterSet);
                if (EncodingResult == null)
                {
                    //MessageBox.Show("컨버팅 에러");
                    continue;
                }

                fileString.Append(EncodingResult);

                fileString.Append(lineEnd);
            }
             */

            // 첨부파일이 존재. 형식에 맞춰 스트링으로 만듬.
            foreach (DictionaryEntry Item in _attach)
            {
                string fileName = Item.Value.ToString();
                string fileRealName = Item.Key.ToString();

                fileFullPath = fileName;

                // \ 이것이 마지막으로 되는 인덱스를 저장하기 위한 변수
                idx = fileRealName.LastIndexOf("\\");
                // 진짜 파일 이름을 뽑아온다.
                ordFileName = fileRealName.Substring(idx + 1);

                fileString.Append("--" + BOUDARYSTRING + "" + lineEnd);
                // 파일에 대한 타입 처리
                fileString.Append("Content-Type: application/octet-stream;");
                // 파일 네임 집어 넣는다. name -> 첨부 이름 같음
                fileString.Append("name=\"=?" + _characterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_characterSet).GetBytes(ordFileName)) + "?=\"" + lineEnd);
                // Content-Transfer-Encoding 타입-base 64
                fileString.Append("Content-Transfer-Encoding: base64" + lineEnd);
                // Content-Disposition: attachment 
                fileString.Append("Content-Disposition: attachment; ");
                // 진짜 파일 네임 지정
                fileString.Append("filename=\"=?" + _characterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_characterSet).GetBytes(ordFileName)) + "?=\"" + lineEnd);
                // 한칸 띄움
                fileString.Append(lineEnd);

                StringBuilder EncodingResult = EncodeFileToBase64(fileName, _characterSet);
                if (EncodingResult == null)
                {
                    //MessageBox.Show("컨버팅 에러");
                    continue;
                }

                fileString.Append(EncodingResult);

                fileString.Append(lineEnd);
            }

            int endPoint = fileFullPath.LastIndexOf(@"\");

            fileString.Append("--" + BOUDARYSTRING + "--" + lineEnd);

            //DeleteFolder(fileFullPath.Substring(0,endPoint));//첨부파일 삭제하지 않기

            return fileString;
        }


        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 파일 내용을 base64 로 바뀌어 리턴
        /// </summary>
        private StringBuilder EncodeFileToBase64(string filename, string _characterSet)
        {
            //FileStream inFile;      // File Stream 객체    
            byte[] binaryData;  // 바이트 배열로 파일 내용 읽어들이는 참조 변수이다.

            //int sizeReceived;
            int bytesRead;

            byte[] buffer = new byte[DATA_BLOCK_SIZE];

            // base64로 인코딩된 것을 저장하는 스트링 
            string base64String = null;

            try
            {
                int try_count = 0;

                long bytesRead1;


                while (try_count < 5)
                {

                    try
                    {
                        // 파일 오픈 
                        using (FileStream inFile = File.OpenRead(filename))
                        {
                            // 파일 크기 만큼 내용 공간 확보 
                            binaryData = new Byte[inFile.Length];
                            // 읽어 들임 
                            bytesRead1 = inFile.Read(binaryData, 0, (int)inFile.Length);

                            inFile.Close();
                            try_count = 21;
                        }

                        base64String = Convert.ToBase64String(binaryData, 0, binaryData.Length);

                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        //Console.WriteLine(er.Message);
                    }
                    finally
                    {
                        try_count++;
                    }
                }

            }
            catch (System.Exception)
            {
                return null;
            }


            StringBuilder sbEncodeFile = new StringBuilder();


            try
            {

                byte[] byteArr = Encoding.GetEncoding(_characterSet).GetBytes(base64String);

                using (MemoryStream ms = new MemoryStream(byteArr))
                {
                    do
                    {
                        bytesRead = ms.Read(buffer, 0, buffer.Length);

                        sbEncodeFile.Append(Encoding.GetEncoding(_characterSet).GetString(buffer, 0, bytesRead) + lineEnd);
                    }
                    while (bytesRead == buffer.Length);

                    ms.Close();
                }

            }
            catch (System.ArgumentNullException)
            {
                return null;
            }


            binaryData = null;


            // 인코딩된 결과 스트링을 리턴한다.
            return sbEncodeFile;
        }


        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : Email 형식을 리턴한다
        /// </summary>
        /// <param name="_email"></param>
        /// <returns></returns>
        public static string GetEmail(string _email)
        {
            try
            {
                StringBuilder email = new StringBuilder();
                string[] emailArr = _email.Trim().Split(',');
                string temp = "";

                if (emailArr.Length > 1)
                {
                    for (int j = 0; j < emailArr.Length; j++)
                    {
                        string[] emailArr1 = emailArr[j].Trim().Split('<');
                        email.Append(emailArr1[emailArr1.Length - 1].Substring(0, emailArr1[emailArr1.Length - 1].Length) + ",");
                    }
                }
                else
                {
                    string[] emailArr2 = _email.Split('<');

                    temp = emailArr2[emailArr2.Length - 1].Substring(0, emailArr2[emailArr2.Length - 1].Length);

                    email.Append(temp + ",");

                }

                email.Replace("<", "");
                email.Replace(">", "");

                return email.ToString().Substring(0, email.Length - 1).Trim(); ;
            }
            catch
            {
                if (_email.IndexOf("<") > -1)
                {
                    int startIdx = _email.IndexOf("<");

                    int endIdx = _email.IndexOf(">");

                    return _email.Substring(startIdx + 1, endIdx - startIdx - 1);
                }
                else
                {
                    return _email;
                }
            }
        }
    }

    [Serializable]
    public class MailDataInfo
    {
        #region 생성자

        /// <summary>
        /// Creates a new <see cref="MailDataInfo"/> instance.
        /// </summary>
        public MailDataInfo()
        {
            to = new Hashtable();
            cc = new Hashtable();
            bcc = new Hashtable();
            attach = new Hashtable();
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        #endregion

        #region 필드

        /// <summary>
        /// characterSet
        /// </summary>
        private string characterSet = "utf-8";

        /// <summary>
        /// to
        /// </summary>
        private Hashtable to = null;

        /// <summary>
        /// cc
        /// </summary>
        private Hashtable cc = null;

        /// <summary>
        /// bcc
        /// </summary>
        private Hashtable bcc = null;

        /// <summary>
        /// attach
        /// </summary>
        private Hashtable attach = null;

        /// <summary>
        /// from
        /// </summary>
        private string from;

        /// <summary>
        /// important
        /// </summary>
        private string important = "3";

        /// <summary>
        /// subject
        /// </summary>
        private string subject;

        /// <summary>
        /// contents
        /// </summary>
        private string contents;

        /// <summary>
        /// mailMessageID
        /// </summary>
        private string mailMessageID;
        #endregion

        #region 프로퍼티
        /// <summary>
        /// CharacterSet.
        /// </summary>
        public string CharacterSet
        {
            get { return characterSet; }
            set { characterSet = value; }
        }

        /// <summary>
        /// To.
        /// </summary>
        public Hashtable To
        {
            get { return to; }
            set { to = value; }
        }

        /// <summary>
        /// Cc.
        /// </summary>
        public Hashtable Cc
        {
            get { return cc; }
            set { cc = value; }
        }

        /// <summary>
        /// Bcc.
        /// </summary>
        public Hashtable Bcc
        {
            get { return bcc; }
            set { bcc = value; }
        }

        /// <summary>
        /// attach
        /// </summary>
        public Hashtable Attach
        {
            get { return attach; }
            set { attach = value; }
        }

        /// <summary>
        /// from
        /// </summary>
        public string From
        {
            get { return from; }
            set { from = value; }
        }

        /// <summary>
        /// important
        /// </summary>
        public string Important
        {
            get { return important; }
            set { important = value; }
        }

        /// <summary>
        /// subject
        /// </summary>
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        /// <summary>
        /// contents
        /// </summary>
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }

        /// <summary>
        /// mailMessageID
        /// </summary>
        public string MailMessageID
        {
            get { return mailMessageID; }
            set { mailMessageID = value; }
        }
        #endregion
    }

}
