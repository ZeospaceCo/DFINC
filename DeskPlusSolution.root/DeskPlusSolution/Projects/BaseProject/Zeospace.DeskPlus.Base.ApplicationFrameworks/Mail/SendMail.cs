using System;
using System.Web;
using System.Text;
using System.IO;
using System.Threading;
using System.Collections;
using Zeospace.DeskPlus.Base.ApplicationFrameworks.Helper;

namespace Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail
{
    /// <summary>
    /// SendMail에 대한 요약 설명입니다.
    /// </summary>
    public class SendMail
    {
        private string lineEnd = "\r\n";
        private int DATA_BLOCK_SIZE = 100;
        private readonly string BOUDARYSTRING = "----NextPart_ZEOSPACE_DESKPLUS_30";

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
        /// 메일 발송
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <returns></returns>
        public bool Send(MailDataInfo _mailDataInfo)
        {
            try
            {
                if (!(_mailDataInfo.To.Count.Equals(0) && _mailDataInfo.Cc.Count.Equals(0) && _mailDataInfo.Bcc.Count.Equals(0)))
                {
                    bool attach = false;

                    if (!(_mailDataInfo.Attach.Count.Equals(0)))
                    {
                        attach = true;
                    }

                    string emlPath = SaveEml(_mailDataInfo, "/DeskPlusEIP/EmlSpool", attach, "");

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("emlPath : " + emlPath, AppSettingType.COMPONENT);
                    //메일 서비스(윈도우서비스 (DpEpmangr.exe)를 이용하여 메일을 발송할것인지 확인
                    try
                    {                        
                        string useService = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseMailSendByService", "Y");
                        if (useService.ToUpper() != "Y")
                        {
                            if (emlPath == "") return false;
                            SMTP smtp = new SMTP(emlPath);
                            smtp.Send();
                        }
                    }
                    catch
                    {
                        if (emlPath == "") return false;
                        SMTP smtp = new SMTP(emlPath);
                        smtp.Send();
                    }

                    //Send(emlPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMaul[Send]", ex);
                return false;
            }
        }

        /// <summary>
        /// 작성자   : 이현수
        /// 작성일자 : 2008-11-26
        /// 작성목적 : 확장 옵션
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_xExtention"></param>
        /// <returns></returns>
        public bool Send(MailDataInfo _mailDataInfo, string _xExtention)
        {
            try
            {
                if (!(_mailDataInfo.To.Count.Equals(0) && _mailDataInfo.Cc.Count.Equals(0) && _mailDataInfo.Bcc.Count.Equals(0)))
                {
                    string emlPath = SaveEml(_mailDataInfo, "/DeskPlusEIP/EmlSpool", false, _xExtention);

                    Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("emlPath : " + emlPath, AppSettingType.COMPONENT);
                    //메일 서비스(윈도우서비스 (DpEpmangr.exe)를 이용하여 메일을 발송할것인지 확인
                    try
                    {
                        string useService = AppSettingHelper.GetAppSetting(AppSettingType.CONFIG, "UseMailSendByService", "Y");
                        if (useService.ToUpper() != "Y")
                        {
                            if (emlPath == "") return false;
                            SMTP smtp = new SMTP(emlPath);
                            smtp.Send();
                        }
                    }
                    catch
                    {
                        if (emlPath == "") return false;
                        SMTP smtp = new SMTP(emlPath);
                        smtp.Send();
                    }

                    //Send(emlPath);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMaul[Send]", ex);
                return false;
            }
        }

        /// <summary>
        /// mht 생성하기(첨부파일 제외)
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_targetPath"></param>
        /// <param name="_isWebPath"></param>
        /// <param name="_emlSpoolPath"></param>
        /// <returns></returns>
        public bool SaveMht(MailDataInfo _mailDataInfo, string _targetPath, bool _isWebPath, string _emlSpoolPath)
        {
            try
            {
                //eml 만들고 임시경로에 저장함
                string spoolPath = SaveEml(_mailDataInfo, _emlSpoolPath, false, "");

                if (_isWebPath)
                {
                    HttpContext context = HttpContext.Current;
                    _targetPath = context.Server.MapPath(_targetPath);
                }


                int lastIdx = _targetPath.LastIndexOf(@"\");
                //if (!Directory.Exists(_targetPath.Substring(0, lastIdx)))Directory.CreateDirectory(_targetPath.Substring(0, lastIdx));

                //targetpath 값으로 폴더 생성
                DirectoryInfo di = new DirectoryInfo(_targetPath.Substring(0, lastIdx));
                if (di.Exists == false)
                {
                    di.Create();
                }

                //임시폴더에서 targetpath 로 eml 이동
                File.Move(spoolPath, _targetPath);
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail[SaveMht] : ", ex);
                return false;
            }
        }

        /// <summary>
        /// MHT 파일 생성
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_targetPath"></param>
        /// <param name="_isWebPath"></param>
        /// <param name="_emlSpoolPath"></param>
        /// <param name="_addAttach">폴더에 첨부파일 떨구기 여부</param>
        /// <returns></returns>
        public bool SaveMht(MailDataInfo _mailDataInfo, string _targetPath, bool _isWebPath, string _emlSpoolPath, bool _addAttach)
        {
            try
            {
                //eml 만들고 임시경로에 저장함
                string spoolPath = SaveEml(_mailDataInfo, _emlSpoolPath, _addAttach, "");

                if (_targetPath.Length > 0)
                {
                    if (_isWebPath)
                    {
                        HttpContext context = HttpContext.Current;
                        _targetPath = context.Server.MapPath(_targetPath);
                    }


                    int lastIdx = _targetPath.LastIndexOf(@"\");
                    //if (!Directory.Exists(_targetPath.Substring(0, lastIdx)))Directory.CreateDirectory(_targetPath.Substring(0, lastIdx));



                    //targetpath 값으로 폴더 생성
                    DirectoryInfo di = new DirectoryInfo(_targetPath.Substring(0, lastIdx));
                    if (di.Exists == false)
                    {
                        di.Create();
                    }

                    //임시폴더에서 targetpath 로 eml 이동
                    File.Move(spoolPath, _targetPath);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail[SaveMht] : ", ex);
                return false;
            }
        }


        /// <summary>
        /// Saves the HTM.
        /// </summary>
        /// <param name="_mailDataInfo">데이터</param>
        /// <param name="_phisicalPath">물리적경로</param>
        /// <param name="_fileName">저장할 파일명</param>
        /// <returns></returns>
        public bool SaveHtm(MailDataInfo _mailDataInfo, string _phisicalPath, string _fileName)
        {
            try
            {
                string savePath = _phisicalPath + _fileName;

                using (StreamWriter sr = File.CreateText(savePath))
                {
                    sr.WriteLine("<html>");
                    sr.WriteLine("<head>");
                    sr.WriteLine("<title></title>");
                    sr.WriteLine("<meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />");
                    sr.WriteLine("</head>");
                    sr.WriteLine("<body>");
                    sr.WriteLine(_mailDataInfo.Contents);
                    sr.WriteLine("</body>");
                    sr.WriteLine("</html>");
                    sr.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteException("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail.SendMail[SaveHtm] : ", ex);
                return false;
            }
        }



        /// <summary>
        /// eml 생성하기
        /// 첨부파일용도
        /// </summary>
        /// <param name="_mailDataInfo"></param>
        /// <param name="_emlSpoolPath"></param>
        /// <param name="_addAttach">첨부파일 폴더 떨구기 사용여부</param>
        /// <param name="_xExtention">확장여부</param>
        /// <returns></returns>
        public string SaveEml(MailDataInfo _mailDataInfo, string _emlSpoolPath, bool _addAttach, string _xExtention)
        {
            try
            {
                //if (_mailDataInfo.From == "" || _mailDataInfo.From == null) 
                if (_mailDataInfo.MailMessageID == "") _mailDataInfo.MailMessageID = GetGUID() + "@" + _mailDataInfo.From;
                StringBuilder message = new StringBuilder();
                message.Append("Message-ID: " + _mailDataInfo.MailMessageID + lineEnd);

                int endInt = _mailDataInfo.From.IndexOf("<");

                string from = GetEmail(_mailDataInfo.From) + " <" + GetEmail(_mailDataInfo.From) + ">";

                if (endInt > -1)
                {
                    from = "=?" + _mailDataInfo.CharacterSet + "?B?" + Convert.ToBase64String(Encoding.GetEncoding(_mailDataInfo.CharacterSet).GetBytes(_mailDataInfo.From.Substring(0, endInt))) + "?=" + " <" + GetEmail(_mailDataInfo.From) + ">";
                }



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
                            newTo = to.Substring(0, checkIndex);
                        }
                        else
                        {
                            newTo = to;
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

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("To : " + message.ToString(), AppSettingType.MAIL);

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

                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteAssert("cc : " + message.ToString(), AppSettingType.MAIL);

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
                    _emlSpoolPath = "/DeskPlusEIP/EmlSpool";
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
                Zeospace.DeskPlus.Base.ApplicationFrameworks.Exceptions.AppException.WriteMail("Zeospace.DeskPlus.Base.ApplicationFrameworks.Mail[MailHelper]", ex);
                return "";
            }
        }

        /// <summary>
        /// GUID
        /// </summary>
        /// <returns></returns>
        public string GetGUID()
        {
            Guid tmpGUID = Guid.NewGuid();
            string guid = tmpGUID.ToString().Replace("-", "");

            return guid;
        }

        /// <summary>
        /// 메일을 발송할 모든 Email 주소 가져오기
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
        /// 파일 내용을 base64 로 바뀌어 리턴
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
        /// 폴더 삭제
        /// </summary>
        /// <param name="_path"></param>
        public void DeleteFolder(string _path)
        {
            if (Directory.Exists(_path))
            {
                Directory.Delete(_path, true);
            }
        }

        /// <summary>
        /// Email 형식을 리턴한다
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


        /// <summary>
        /// 작성자   : 심재운
        /// 작성일자 : 2010-11-12
        /// 작성목적 : post 전송하기
        /// 수정일자 :
        /// 수정내용 :
        /// </summary>
        /// <param name="_mURL">Post Url</param>
        /// <returns></returns>
        public static string SendPost(string _mURL)
        {
            string getValue = "";

            // Send to server
            System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_mURL);
            httpWebRequest.Method = "get";
            httpWebRequest.Accept = "*/*";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            // Get response
            using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (System.IO.StreamReader responseReader = new System.IO.StreamReader(responseStream))
                        {
                            StringBuilder response = new StringBuilder();

                            char[] read = new Char[256];
                            int count = responseReader.Read(read, 0, 256);

                            while (count > 0)
                            {
                                response.Append(read, 0, count);
                                count = responseReader.Read(read, 0, 256);
                            }
                            responseReader.Close();
                            getValue = response.ToString();
                        }
                    }
                }
            }

            getValue = getValue.Replace("\r\n", "|");
            return getValue;
        }
    }
}
