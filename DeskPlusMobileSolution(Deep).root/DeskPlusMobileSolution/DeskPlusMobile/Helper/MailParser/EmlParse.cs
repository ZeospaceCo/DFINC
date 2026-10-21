using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Security;

namespace Zeospace.DeskPlusMobile.Web.Helper.MailParser
{
    public class EmlParse
    {

        private static char[] sep ={ ' ', '\t', '\r', '\n' };
        private static string line_end = "\r\n";
        private MailMessage mmsg = new MailMessage();
        private bool image = false;
        private bool audio = false;
        private bool message = false;
        private static int DATA_BLOCK_SIZE = 76;
        private bool assertMail = false;
        private string mailQueuePath = "";
        private string mailDropPath = "";

        public EmlParse()
        {
            mmsg.Date = DateTime.Now;
            mmsg.Body_Str = new StringBuilder();
            mmsg.Boundary = new string[4];
            mmsg.X_Receive = new Hashtable();
            mmsg.To = new Hashtable();
            mmsg.Cc = new Hashtable();
            mmsg.Bcc = new Hashtable();
            mmsg.Content_ID = new Hashtable();
            mmsg.Secure = false;
            mmsg.recall = false;
            mmsg.X_Forwarding = false;

            mmsg.Attach = new Hashtable();

            AssertMail = Convert.ToBoolean(Helper.XmlDataBaseHelper.GetAppSetting(AppSettingType.Base, "ASSERT_MAIL"));
        }

        public MailMessage Mmsg
        {
            get { return mmsg; }
            set { mmsg = value; }
        }

        public bool AssertMail
        {
            get { return assertMail; }
            set { assertMail = value; }
        }

        public string MailQueuePath
        {
            get { return mailQueuePath; }
            set { mailQueuePath = value; }
        }

        public string MailDropPath
        {
            get { return mailDropPath; }
            set { mailDropPath = value; }
        }

        #region function WriteLog
        private void WriteLog(string msg)
        {
            if (AssertMail)
            {
                HttpContext context = HttpContext.Current;
                string nowYear = DateTime.Now.Year.ToString();
                string nowMon = DateTime.Now.ToString("MM");
                string nowDay = DateTime.Now.ToString("dd");
                string nowHour = DateTime.Now.ToString("HH");

                string emlLogpath = context.Server.MapPath("/DeskPlusMobile/Data/Logs/Mail");
                if (!Directory.Exists(emlLogpath)) Directory.CreateDirectory(emlLogpath);


                using (System.IO.StreamWriter tw = new System.IO.StreamWriter(emlLogpath + @"\" + nowYear + nowMon + nowDay + nowHour + ".log", true, Encoding.Default))
                {
                    string msg_1 = "================================================================" + line_end;
                    msg_1 += "=================================================================" + line_end;
                    msg_1 += "=================================================================" + line_end;

                    msg_1 += "Error : " + msg + line_end;
                    msg_1 += "File_Name : " + mmsg.Full_File_Name + line_end;
                    msg_1 += "From : " + mmsg.From + line_end;

                    foreach (string to in mmsg.To.Values)
                    {
                        msg_1 += "To : " + to + line_end;
                    }

                    foreach (string cc in mmsg.Cc.Values)
                    {
                        msg_1 += "Cc : " + cc + line_end;
                    }

                    msg_1 += "Attach Count : " + mmsg.Attach_Count + line_end;
                    msg_1 += "Date : " + mmsg.Date + line_end;
                    msg_1 += "Important : " + mmsg.Important + line_end;
                    msg_1 += "Subject : " + mmsg.Subject + line_end;
                    msg_1 += "Contents Type : " + mmsg.ContentType + line_end;
                    msg_1 += "Contents Encoding : " + mmsg.C_Encoding + line_end;


                    foreach (string c_id in mmsg.Content_ID.Values)
                    {
                        msg_1 += "C_ID : " + c_id + line_end;
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        if (mmsg.Boundary[i] != null)
                        {
                            msg_1 += "Boundary " + i + " : " + mmsg.Boundary[i] + line_end;
                        }
                    }

                    foreach (string file in mmsg.Attach.Values)
                    {
                        msg_1 += "Attach : " + file + line_end;
                    }
                    msg_1 += "=====================================================================" + line_end;
                    msg_1 += "BODY : " + mmsg.Body_Str + line_end;
                    msg_1 += "=====================================================================" + line_end;
                    msg_1 += "HTML : " + mmsg.Contents_Html + line_end;
                    msg_1 += "=====================================================================" + line_end;
                    msg_1 += "Text : " + mmsg.Contents_Text + line_end;
                    msg_1 += "=====================================================================" + line_end;
                    msg_1 += "=====================================================================" + line_end;
                    msg_1 += "=====================================================================" + line_end + line_end + line_end;

                    tw.Write(msg_1);
                    tw.Close();
                }
            }
        }
        #endregion

        #region function Start_Parsing

        public void Start_Parsing()
        {
            FileInfo fi = new FileInfo(mmsg.Full_File_Name);
            mmsg.Size = fi.Length;

            if (mmsg.Size.Equals(0))
            {
                throw new Exception("0 byte");
            }

            try
            {
                Mail_Head_Parsing();

                //WriteLog("After Mail_Head_Parsing : " + DateTime.Now);

                //WriteLog("mmsg.Subject : " + mmsg.Subject);

                //WriteLog(mmsg.Body_Str.ToString().Substring(0,500));

                //WriteLog("From : " + QuotedCoding.DecodeOne("\'=?euc-kr?B?V2hhdCdzIE5FWFQ/IEZvcnVt?=\'") + line_end);

                //WriteLog("mmsg.Attach_Count : " + mmsg.Attach_Count);

                //WriteLog("mmsg.Content_CharSet : ." + mmsg.Content_CharSet + ".");


                Mail_Body_Parsing();

                //WriteLog("After Mail_Body_Parsing : " + DateTime.Now);


                if (mmsg.From == null)
                {
                    mmsg.From = "Sender Unspecified";
                }

                //								for (int i=0; i<3; i++)
                //								{
                //									if (mmsg.Boundary[i] != null)
                //									{
                //										WriteLog("Boundary " + i + " : ." + mmsg.Boundary[i] + ".");
                //									}
                //								}


                //
                //				for (int i=0; i<3; i++)
                //				{
                //					if (mmsg.Boundary[i] != null)
                //					{
                //						WriteLog("Boundary " + i + " : " + mmsg.Boundary[i] + line_end);
                //					}
                //				}



                if (mmsg.Boundary[0] == null)
                {
                    mmsg.Attach_Count = 0;
                }
                else
                {
                    if (mmsg.Attach_Count > 0 && mmsg.Boundary[0] != null)
                    {
                        Mail_Attach_Parsing();
                    }
                }

                if (mmsg.Contents_Html == null)
                {
                    mmsg.Contents_Html = "";
                }

                if (mmsg.Contents_Text == null)
                {
                    mmsg.Contents_Text = "";
                }

                if (mmsg.Important == null)
                {
                    mmsg.Important = "3";
                }

                if (mmsg.Subject == null)
                {
                    mmsg.Subject = "";
                }

                if (mmsg.MsgID == null)
                {
                    mmsg.MsgID = "";
                }

                if (mmsg.Notification == null)
                {
                    mmsg.Notification = "";
                }


                //WriteLog(mmsg.Date);

                if (AssertMail)
                {
                    WriteLog("테스팅");
                }

                if (AssertMail)
                {
                    string msg_2 = "File_Name : " + mmsg.Full_File_Name;
                    WriteLog(msg_2);

                    msg_2 = "From : " + mmsg.From;
                    WriteLog(msg_2);


                    foreach (string to in mmsg.To.Keys)
                    {
                        WriteLog("to : " + to);

                    }

                    WriteLog(mmsg.Contents_Html);

                }
            }
            catch (Exception er)
            {
                if (AssertMail)
                {
                    WriteLog(er.Message);
                }
            }
            finally
            {
                //Dispose();
            }

        }
        #endregion

        #region function New_Mail_Head_Parsing

        private void To_Cc_Head_Parsing(string msg)
        {
            Hashtable ht_to = new Hashtable();
            StringBuilder Header_Str = new StringBuilder();

            string msg_1 = "to:";

            if (msg_1.Equals(msg))
            {
                msg_1 = "cc:";
            }

            try
            {
                using (StreamReader reader = new StreamReader(mmsg.Full_File_Name, Encoding.Default))
                {
                    string line = reader.ReadLine();
                    string msg_to = "";
                    
                    bool ok = false;

                    while (line != null)
                    {
                        #region To or Cc
                        if (line.ToLower().StartsWith(msg))
                        {
                            if (!ok)
                            {
                                msg_to = line.Substring(3).Trim();

                                if (msg_to.TrimEnd(sep).EndsWith(","))
                                {
                                    msg_to = msg_to.Substring(0, msg_to.Length - 1);
                                }

                                if (!ht_to.ContainsKey(msg_to.Trim()))
                                {
                                    ht_to.Add(msg_to.Trim(), null);
                                }

                                //WriteLog(msg_to);


                                while (line.Trim().TrimEnd(sep).EndsWith(","))
                                {
                                    line = reader.ReadLine().Trim();

                                    //WriteLog(line);

                                    if (line.Trim().ToLower().StartsWith("subject:") || line.Trim().ToLower().StartsWith(msg_1))
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        if (line.TrimEnd(sep).EndsWith(","))
                                        {
                                            msg_to = line.Trim().TrimEnd(sep).Substring(0, line.Length - 1);
                                        }
                                        else
                                        {
                                            msg_to = line.Trim().TrimEnd(sep);
                                        }


                                        //WriteLog(msg + msg_to);

                                        if (!ht_to.ContainsKey(msg_to))
                                        {
                                            ht_to.Add(msg_to, null);
                                        }
                                    }
                                }

                                if (msg.Equals("to:"))
                                {
                                    New_Receive_Process(ht_to, true);
                                }
                                else
                                {
                                    New_Receive_Process(ht_to, false);
                                }
                            }

                            //WriteLog(line + line_end);
                        }
                        #endregion

                        Header_Str.Append(line + line_end);

                        #region Body_Start
                        if (line.ToLower().TrimStart(sep).TrimEnd(sep) == "")
                        {
                            mmsg.Header_Str = Header_Str;
                            ok = true;
                            break;
                        }
                        #endregion


                        line = reader.ReadLine();
                    }
                    reader.Close();
                }
            }
            catch (Exception er)
            {
                WriteLog("To_Cc_Header : " + er.Message);
            }
            finally
            {
                ht_to.Clear();
            }

        }



        private void Subject_Head_Parsing()
        {
            try
            {
                using (StreamReader reader = new StreamReader(mmsg.Full_File_Name, Encoding.Default))
                {
                    string line = reader.ReadLine();
                    StringBuilder sb = new StringBuilder();
                    bool ok = false;

                    while (line != null)
                    {
                        #region Subject
                        if (line.Trim().ToLower().StartsWith("subject:"))
                        {
                            if (!ok)
                            {
                                sb.Append(line.Substring(8).TrimStart(sep).TrimEnd(sep));

                                do
                                {
                                    line = reader.ReadLine();

                                    if (line.StartsWith("\t") || line.TrimStart(sep).StartsWith("=?"))
                                    {
                                        if (line.TrimStart(sep).StartsWith("=?"))
                                        {
                                            sb.Append(QuotedCoding.DecodeOne(line.TrimStart(sep).TrimEnd(sep)));
                                        }
                                        else
                                        {
                                            sb.Append(line.TrimStart(sep).TrimEnd(sep));
                                        }

                                    }
                                } while (line.StartsWith("\t") || line.TrimStart(sep).StartsWith("=?"));


                                if (line.TrimStart(sep).StartsWith("=?"))
                                {
                                    sb.Append(QuotedCoding.DecodeOne(line.TrimStart(sep).TrimEnd(sep)));
                                    //mmsg.Subject += line.TrimStart(sep).TrimEnd(sep);
                                }


                                mmsg.Subject = QuotedCoding.Decode(sb.ToString());

                                //WriteLog(line + line_end);
                            }
                        }
                        #endregion


                        #region Body_Start
                        if (line.ToLower().TrimStart(sep).TrimEnd(sep) == "")
                        {
                            ok = true;
                            break;
                        }
                        #endregion

                        line = reader.ReadLine();
                    }
                    reader.Close();
                }
            }
            catch (Exception er)
            {
                WriteLog("Subject_Header : " + er.Message);
            }
            //			finally
            //			{
            //				WriteLog(mmsg.Subject);
            //			}
        }

        #region function New_Receive_Process
        private void New_Receive_Process(Hashtable line, bool bool_to)
        {

            int lastidx = 0;
            string name = null;
            string[] NudeStr;
            string to_email = null;
            int startidx = -1;
            string to = null;

            foreach (string data in line.Keys)
            {
                try
                {
                    // From 문자열 찾아냄
                    lastidx = data.TrimStart(sep).LastIndexOf("<");

                    if (lastidx > 0)
                    {
                        // From 을 base64에서 원래 문자열로 바꿈
                        NudeStr = data.Split('?');

                        if (NudeStr.Length != 1 && data.IndexOf("?") > -1)
                        {

                            // "=?문자세트?인코딩방법?인코딩된 편지제목?="  형식
                            // "예로 =?ks_c_5601-1987?B?wMyx4sW5?="
                            // 이거 인데 이걸 ? 기준으로 split하면 된다.
                            // split 한 2번째 스트링은 -> 한글 코드
                            // 4번째가 인코딩된 문자열 
                            // 2번째 B : base64, or Q면: Quoted-Printable
                            // =,= 는 처음과 끝을 나타냄 
                            if (NudeStr[2].ToLower().CompareTo("b") == 0)
                            {
                                name = System.Text.Encoding.Default.GetString(Convert.FromBase64String(NudeStr[3])).TrimEnd(sep).Replace(@"\", "").Trim();
                            }
                            else if (NudeStr[2].ToLower().CompareTo("q") == 0)
                            {
                                // Quoted_Printable
                                name = Q_Decoding(NudeStr[3], NudeStr[1].ToLower()).TrimEnd(sep);
                            }
                        }
                        else
                        {
                            name = data.Substring(0, lastidx).TrimEnd(sep);
                        }

                        //WriteLog("Name :" + name + line_end);
                    }
                    startidx = data.IndexOf("<");

                    if (startidx > -1)
                    {
                        lastidx = data.LastIndexOf(">");

                        if (name == null)
                        {
                            to_email = data.Substring(startidx + 1, lastidx - startidx - 1).TrimEnd(sep);
                        }
                        else
                        {
                            to_email = name + " <" + data.Substring(startidx + 1, lastidx - startidx - 1).TrimEnd(sep) + ">";
                        }

                        if (bool_to)
                        {
                            if (!mmsg.To.ContainsKey(to_email))
                            {
                                mmsg.To.Add(to_email, to_email);

                                //WriteLog("To_Email : " +  to_email + line_end);
                            }
                        }
                        else //cc
                        {
                            if (!mmsg.Cc.ContainsKey(to_email))
                            {
                                mmsg.Cc.Add(to_email, to_email);

                                //WriteLog("To_Email : " +  to_email + line_end);
                            }
                        }
                    }
                    else
                    {
                        to = QuotedCoding.DecodeOne(data.TrimStart(sep).TrimEnd(sep));

                        if (bool_to)
                        {
                            if (!mmsg.To.ContainsKey(to))
                            {
                                mmsg.To.Add(to, to);

                                //WriteLog("To_Email : " +  to + line_end);
                            }
                        }
                        else //cc
                        {
                            if (!mmsg.Cc.ContainsKey(to))
                            {
                                mmsg.Cc.Add(to, to);

                                //WriteLog("To_Email : " +  to + line_end);
                            }
                        }

                    }
                }
                catch (Exception er)
                {
                    WriteLog("TO or Cc : " + er.Message);
                }
            }
        }
        #endregion



        #endregion

        #region function Mail_Head_Parsing

        private void Mail_Head_Parsing()
        {
            try
            {
                using (FileStream reader = File.OpenRead(mmsg.Full_File_Name))
                {
                    byte[] buffer = new byte[reader.Length];
                    reader.Read(buffer, 0, buffer.Length);
                    mmsg.Full_Eml = buffer;

                    reader.Close();
                }

                To_Cc_Head_Parsing("to:");
                To_Cc_Head_Parsing("cc:");
                Subject_Head_Parsing();

                using (StreamReader reader = new StreamReader(mmsg.Full_File_Name, Encoding.Default))
                {
                    string line = reader.ReadLine();

                    int i = 0;
                    bool ok = false;
                    int body_start = 0;
                    int a_count = 0;
                    string file_name = "";

                    string x_receiver = null;
                    string x_forwarding = null;
                    //					string Subject = null;
                    string read_line = null;
                    string boundary = null;

                    bool end_boundary = false; //Content-Type: text/html; 이후에 나오는  boundary는 무효..

                    while (line != null)
                    {
                        #region Date_1
                        /*
						if(line.ToLower().StartsWith("received: "))
						{
							line = reader.ReadLine();

							int startidx = line.IndexOf(";");

							if (startidx > -1)
							{
								mmsg.Date = line.Substring(startidx+1).TrimStart(sep).TrimEnd(sep);

								//WriteLog("Date : " + mmsg.Date + line_end);
							}

							if (line.StartsWith("Date: "))
							{
								// Date: 일 때
								mmsg.Date = line.Substring(6);

								//WriteLog("Date : " + mmsg.Date + line_end);
							}
						}
						*/
                        #endregion

                        #region x_receiver
                        if (line.ToLower().StartsWith("x-receiver:"))
                        {
                            if (!ok)
                            {
                                x_receiver = line.Substring(11).TrimStart(sep).TrimEnd(sep);

                                if (!mmsg.X_Receive.ContainsValue(x_receiver))
                                {
                                    mmsg.X_Receive.Add(x_receiver, x_receiver);

                                    //WriteLog("X-Receiver : " + data + line_end);
                                }
                            }
                        }
                        #endregion

                        #region x-forwarding
                        if (line.ToLower().StartsWith("x-forwarding:"))
                        {
                            if (!ok)
                            {
                                x_forwarding = line.Substring(13).TrimStart(sep).TrimEnd(sep);

                                if (x_forwarding.ToLower().Equals("ok"))
                                {
                                    mmsg.X_Forwarding = true;
                                }
                            }
                        }
                        #endregion

                        #region From
                        else if (line.ToLower().StartsWith("from:"))
                        {
                            if (!ok)
                            {
                                From_Process(line);
                            }
                        }
                        #endregion

                        #region Date
                        else if (line.ToLower().StartsWith("date:"))
                        {
                            if (!ok)
                            {
                                Date_Process(line);
                            }
                        }
                        #endregion

                        #region Secure
                        else if (line.ToLower().StartsWith("secure:"))
                        {
                            if (!ok)
                            {
                                if (line.Substring(8).Equals("ok"))
                                {
                                    mmsg.Secure = true;
                                }
                            }
                        }
                        #endregion

                        #region X-Priority
                        else if (line.ToLower().StartsWith("x-priority:"))
                        {
                            if (!ok)
                            {
                                XPre_Process(line);
                            }
                        }
                        #endregion

                        #region Attach_Count & Contents-Type & Boundary
                        else if (line.ToLower().StartsWith("content-type:"))
                        {
                            //WriteLog(line);
                            //WriteLog(a_count);

                            if (line.ToLower().IndexOf("content-type: text/html") > -1)
                            {
                                end_boundary = true;
                            }

                            if (a_count == 0)
                            {
                                if (line.TrimEnd(sep).EndsWith(";"))
                                {
                                    read_line = reader.ReadLine().TrimStart(sep).TrimEnd(sep);

                                    i++;

                                    if (read_line.ToLower().TrimStart(sep).TrimEnd(sep) == "")
                                    {
                                        if (body_start == 0)
                                        {
                                            body_start = i;
                                            ok = true;
                                        }
                                    }

                                    line += read_line;
                                }

                                if (line.TrimEnd(sep).EndsWith(";"))
                                {
                                    read_line = reader.ReadLine().TrimStart(sep).TrimEnd(sep);

                                    line += read_line;
                                }

                                //WriteLog(line);

                                // Content-Type: message/rfc822
                                //X-HM-IDENT: attach

                                if (line.ToLower().IndexOf("name=\"") > -1 || line.ToLower().IndexOf("name = \"") > -1 || line.ToLower().IndexOf("message") > -1)
                                {
                                    if (ok)
                                    {
                                        if (i > body_start)
                                        {
                                            file_name = line.Substring(line.ToLower().IndexOf("name=\"") + 6).Replace("\"", "").Replace(";", "");
                                            //
                                            //WriteLog("Temp File Name : " + file_name);

                                            //WriteLog("a_count 1 : " + a_count);

                                            a_count += 1;

                                            //WriteLog("a_count 2 : " + a_count);
                                        }
                                    }
                                }

                                if (line.ToLower().IndexOf("charset=") > -1)
                                {
                                    //int start_idx = line.ToLower().IndexOf("charset=");

                                    mmsg.Content_CharSet = line.Substring(line.ToLower().IndexOf("charset=") + 8).Replace("\"", "");
                                }

                                if (!ok) //아직 헤드에서...
                                {
                                    //WriteLog("Boundary");

                                    //WriteLog("ok : " + ok.ToString());

                                    Contents_Boundary_Process(line);
                                }
                                else //본문에서..
                                {
                                    if (!end_boundary)
                                    {
                                        if (mmsg.Boundary[0] != null) //최소 헤드에 한개의 boundary가 있을 경우..
                                        {
                                            Contents_Boundary_Process(line);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Contents-ID
                        else if (line.ToLower().StartsWith("content-id:"))
                        {
                            //WriteLog("c-id : " + line);

                            if (a_count == 0)
                            {
                                a_count++;
                            }
                        }
                        #endregion

                        #region Content-Disposition
                        else if (line.ToLower().StartsWith("content-disposition:"))
                        {
                            //WriteLog("c-d : " + line);

                            if (ok)
                            {
                                if (line.TrimEnd(sep).EndsWith(";"))
                                {
                                    line += reader.ReadLine();
                                    i++;
                                }

                                //WriteLog("ok : " + line);

                                if (line.ToLower().IndexOf("filename=") > -1 || line.ToLower().IndexOf("name=") > -1)
                                {
                                    //WriteLog("filename : " + line);

                                    //a_count ++;


                                    //WriteLog("a_count : " + a_count);



                                    if (i > body_start)
                                    {
                                        if (line.Substring(line.ToLower().IndexOf("name=\"") + 6).Replace("\"", "").Replace(";", "") != file_name)
                                        {

                                            //WriteLog("Temp File Name : " + file_name + line_end);


                                            a_count++;
                                        }
                                    }

                                    //WriteLog("a_count : " + a_count);
                                }

                            }
                        }
                        #endregion

                        #region Content-Transfer-Encoding
                        else if (line.ToLower().StartsWith("content-transfer-encoding:"))
                        {
                            if (!ok && mmsg.C_Encoding == null)
                            {
                                C_Encoding_Process(line);
                            }
                        }
                        #endregion

                        #region request-recall
                        else if (line.ToLower().StartsWith("request-recall: true"))
                        {
                            //Request-Recall: True
                            //WriteLog("request-recall: 찾음");
                            if (!ok)
                            {
                                //WriteLog("request-recall: 찾음");
                                mmsg.recall = true;
                            }
                        }
                        #endregion

                        #region Disposition-Notification-To
                        else if (line.ToLower().StartsWith("disposition-notification-to:"))
                        {
                            if (!ok)
                            {
                                Notification_Process(line);
                            }
                        }
                        #endregion


                        #region Message-ID
                        else if (line.ToLower().StartsWith("message-id:"))
                        {
                            //WriteLog("message-id: 찾음");
                            if (!ok)
                            {
                                //WriteLog("message-id: 찾음");
                                MID_Process(line);
                            }
                        }
                        #endregion

                        #region Boundary
                        else if ((line.ToLower().TrimStart(sep)).StartsWith("boundary="))
                        {
                            bool still_bound = false;

                            if (!ok)
                            {
                                if (mmsg.Boundary[0] != null)
                                {
                                    still_bound = true;
                                }
                            }

                            if (still_bound)
                            {
                                if (a_count == 0)
                                {
                                    // 왼쪽 공백 지우고 난 것이 boundary="로 시작하면
                                    // 바운더리 문자열만 얻어온다.  boundary=" 로부터 11자부터 뽑아온다.
                                    //string bstr=line.TrimStart(sep);
                                    boundary = line.TrimStart(sep).Substring(9).Replace("\"", "").TrimStart(sep).TrimEnd(sep);

                                    if (mmsg.Boundary[0] == null)
                                    {
                                        mmsg.Boundary[0] = boundary;

                                        //WriteLog("Boundary 0: " + boundary + line_end);
                                    }
                                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] == null)
                                    {
                                        mmsg.Boundary[1] = boundary;

                                        //WriteLog("Boundary 1: " + boundary + line_end);
                                    }
                                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] != boundary && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2] == null)
                                    {
                                        mmsg.Boundary[2] = boundary;

                                        //WriteLog("Boundary 2: " + boundary + line_end);
                                    }
                                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] != boundary && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2] != boundary && mmsg.Boundary[2].Length > 0 && mmsg.Boundary[3] == null)
                                    {
                                        mmsg.Boundary[3] = boundary;

                                        //WriteLog("Boundary 3: " + boundary + line_end);
                                    }
                                }
                            }

                            //WriteLog("Boundary : " + boundary + line_end);
                        }
                        #endregion

                        #region Body_Start
                        else if (line.ToLower().TrimStart(sep).TrimEnd(sep) == "")
                        {
                            if (body_start == 0)
                            {
                                body_start = i;
                                ok = true;
                            }
                        }
                        #endregion

                        #region Body_Start 2
                        else if (line.ToLower().TrimStart(sep).TrimEnd(sep).StartsWith("--"))
                        {
                            if (body_start == 0)
                            {
                                body_start = i;
                                ok = true;
                            }
                        }
                        #endregion

                        if (ok)
                        {
                            if (i >= body_start)
                            {
                                mmsg.Body_Str.Append(line + line_end);
                            }
                        }

                        line = reader.ReadLine();

                        i++;
                    }
                    reader.Close();
                    mmsg.Attach_Count = a_count;


                    //WriteLog("a_count : " + a_count);

                    //WriteLog(mmsg.Body_Str.ToString());


                    //mmsg.Body = new MemoryStream(Encoding.Default.GetBytes(mmsg.Body_Str.ToString()));
                }
            }
            catch (Exception er)
            {
                WriteLog("헤더 : " + er.Message);
            }
        }

        #region function From_Process
        private void From_Process(string line)
        {
            try
            {
                string from_name = null;

                line = line.Substring(5).TrimStart(sep).TrimEnd(sep);

                line = line.Replace("\"", "");

                // From 문자열 찾아냄
                int startidx = line.IndexOf("<");

                string From = null;
                string[] NudeStr;
                int lastidx = -1;

                // <>가 있는 유형
                if (startidx > 0)
                {
                    From = line.Substring(0, startidx);

                    //WriteLog("From : " + From);

                    // From 을 base64에서 원래 문자열로 바꿈
                    NudeStr = From.Split('?');

                    if (NudeStr.Length != 1 && From.IndexOf("=?") > -1)
                    {

                        // "=?문자세트?인코딩방법?인코딩된 편지제목?="  형식
                        // "예로 =?ks_c_5601-1987?B?wMyx4sW5?="
                        // 이거 인데 이걸 ? 기준으로 split하면 된다.
                        // split 한 2번째 스트링은 -> 한글 코드
                        // 4번째가 인코딩된 문자열 
                        // 2번째 B : base64, or Q면: Quoted-Printable
                        // =,= 는 처음과 끝을 나타냄 
                        if (NudeStr[2].ToLower().Equals("b"))
                        {
                            from_name = System.Text.Encoding.Default.GetString(Convert.FromBase64String(NudeStr[3])).Replace(@"\", "").TrimStart(sep).TrimEnd(sep);
                        }
                        else if (NudeStr[2].ToLower().Equals("q"))
                        {
                            // Quoted_Printable
                            from_name = Q_Decoding(NudeStr[3], NudeStr[1].ToLower()).TrimStart(sep).TrimEnd(sep);
                        }
                    }
                    else
                    {
                        from_name = NudeStr[0].TrimStart(sep).TrimEnd(sep);

                        if (from_name.IndexOf("<") > -1 && from_name.IndexOf(">") > -1)
                        {
                            from_name = from_name.Substring(0, from_name.Length - from_name.IndexOf("<"));
                        }
                    }
                }

                if (startidx > -1)
                {
                    lastidx = line.LastIndexOf(">");

                    if (from_name == "" || from_name == null)
                    {
                        mmsg.From = line.Substring(startidx + 1, lastidx - startidx - 1).TrimStart(sep).TrimEnd(sep);
                    }
                    else
                    {
                        mmsg.From = from_name + " <" + line.Substring(startidx + 1, lastidx - startidx - 1) + ">";
                    }
                }
                else
                {
                    mmsg.From = QuotedCoding.DecodeOne(line).TrimStart().TrimEnd(sep);
                }

                //WriteLog("From : " + mmsg.From + line_end);
            }
            catch (Exception er)
            {
                WriteLog("FROM : " + er.Message);
            }
        }
        #endregion

        #region function Date_Process
        private void Date_Process(string line)
        {
            try
            {
                string date_1 = line.Substring(5).TrimStart(sep).TrimEnd(sep); //"Tue, 08 Jul 2003 11:51:00";// +0900";

                //				WriteLog(date_1);

                string date_add = "";
                string date_minus = "";
                int index = date_1.IndexOf("+");

                if (index > -1)
                {
                    date_add = date_1.Substring(index + 1).Trim();
                    date_1 = date_1.Substring(0, index).TrimEnd(sep) + " GMT";
                }

                index = date_1.IndexOf("-");

                if (index > -1)
                {
                    date_minus = date_1.Substring(index + 1).Trim();
                    date_1 = date_1.Substring(0, index).TrimEnd(sep) + "GMT";
                }

                //				WriteLog("date_add : " + date_add);
                //				WriteLog("date_minus : " + date_minus);


                //System.Globalization.DateTimeFormatInfo dFormat = System.Globalization.CultureInfo.CreateSpecificCulture("ko-KR").DateTimeFormat;
                System.Globalization.DateTimeFormatInfo dFormat = System.Globalization.CultureInfo.CreateSpecificCulture("en").DateTimeFormat;
                dFormat.FullDateTimePattern = dFormat.RFC1123Pattern;

                DateTime date_2 = Convert.ToDateTime(date_1, dFormat);
                //string date_3 = date_2.ToString((dFormat.RFC1123Pattern,dFormat);


                mmsg.Date = date_2;

                //				WriteLog(mmsg.Date);


                if (date_add.Length > 0)
                {
                    mmsg.Date = mmsg.Date.AddHours(-Convert.ToDouble(date_add.Substring(0, 2)));
                    mmsg.Date = mmsg.Date.AddMinutes(Convert.ToDouble(date_add.Substring(2, 2)));
                }

                if (date_minus.Length > 0)
                {
                    mmsg.Date = mmsg.Date.AddHours(Convert.ToDouble(date_minus.Substring(0, 2)));
                    mmsg.Date = mmsg.Date.AddMinutes(Convert.ToDouble(date_minus.Substring(2, 2)));
                }
                //
                //
                //				WriteLog(mmsg.Date);

            }
            catch //(Exception er)
            {
                //				WriteLog(er.Message);
                mmsg.Date = DateTime.Now;
            }
        }
        #endregion

        #region function MID_Process
        private void MID_Process(string line)
        {
            try
            {
                mmsg.MsgID = line.Substring(11).TrimStart(sep).TrimEnd(sep);

                //				WriteLog("Msg ID : " + mmsg.MsgID + line_end);	
                //				WriteLog("Msg ID : " + line + line_end);	
            }
            catch (Exception er)
            {
                WriteLog("MID : " + er.Message);
            }
        }
        #endregion

        #region function Notification_Process
        private void Notification_Process(string line)
        {
            try
            {
                mmsg.Notification = line.Substring(28).TrimStart(sep).TrimEnd(sep).Replace("<", "").Replace(">", "");

            }
            catch (Exception er)
            {
                WriteLog("Notification : " + er.Message);
            }
        }
        #endregion

        #region function C_Encoding_Process
        private void C_Encoding_Process(string line)
        {
            try
            {
                mmsg.C_Encoding = line.Substring(26).TrimStart(sep).TrimEnd(sep);
                //WriteLog("Encoding : " + mmsg.C_Encoding + line_end);
            }
            catch (Exception er)
            {
                WriteLog("C ENCODING : " + er.Message);
            }
        }
        #endregion

        #region function XPre_Process
        private void XPre_Process(string line)
        {
            try
            {
                string important = line.Substring(11).TrimStart(sep).TrimEnd(sep);

                string[] important_1 = important.Split(' ');

                if (important_1.Length > 1)
                {
                    mmsg.Important = important_1[0].ToString().TrimStart(sep).TrimEnd(sep);
                }
                else
                {
                    mmsg.Important = important;
                }

                //WriteLog("Important : " + mmsg.Important + line_end);
            }
            catch (Exception er)
            {
                WriteLog("XPRE PROCESS : " + er.Message);
            }
        }
        #endregion

        #region function Contents_Boundary_Process
        private void Contents_Boundary_Process(string line)
        {
            line = line.Replace("\"", "");


            try
            {
                if (line.TrimEnd(sep).EndsWith(";"))
                {
                    line = line.TrimEnd(sep).Substring(0, line.Length - 1);
                }

                line = line.Substring(13).TrimStart(sep).TrimEnd(sep);

                //WriteLog("Bound Temp : " + line + line_end);


                if (mmsg.ContentType == null)
                {
                    // Content-Type 일때 파싱 
                    int first_idx = line.IndexOf(";");

                    if (first_idx > -1)
                    {
                        mmsg.ContentType = line.Substring(0, first_idx).TrimStart(sep).TrimEnd(sep);
                        //WriteLog("Content Type 1 : " + mmsg.ContentType + line_end);
                    }
                    else
                    {
                        mmsg.ContentType = line;
                        //WriteLog("Content Type 2 : " + mmsg.ContentType + line_end);
                    }
                }

                int idx = line.ToLower().IndexOf("boundary=");

                if (idx == -1)
                {
                    idx = line.ToLower().IndexOf("boundary = ");
                }

                // boundary=가 있으면 바운드리에 포함
                if (idx > -1)
                {
                    string boundary = line.Substring(idx + 8).Trim();

                    if (boundary.StartsWith("="))
                    {
                        boundary = boundary.Substring(1).Trim();
                    }

                    //WriteLog("Bound Temp 1 : " + boundary + line_end);

                    if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0] == null)
                    {
                        mmsg.Boundary[0] = boundary;

                        if (boundary.Split(';').Length > 1)
                        {
                            mmsg.Boundary[0] = boundary.Split(';')[0];
                        }

                        //WriteLog("Boundary 0: " + boundary + line_end);
                    }
                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] == null)
                    {
                        mmsg.Boundary[1] = boundary;

                        if (boundary.Split(';').Length > 1)
                        {
                            mmsg.Boundary[1] = boundary.Split(';')[0];
                        }

                        //WriteLog("Boundary 1: " + boundary + line_end);
                    }
                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] != boundary && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2] == null)
                    {
                        mmsg.Boundary[2] = boundary;

                        if (boundary.Split(';').Length > 1)
                        {
                            mmsg.Boundary[2] = boundary.Split(';')[0];
                        }

                        //WriteLog("Boundary 2: " + boundary + line_end);
                    }
                    else if (mmsg.Boundary[0] != boundary && mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] != boundary && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2] != boundary && mmsg.Boundary[2].Length > 0 && mmsg.Boundary[3] == null)
                    {
                        mmsg.Boundary[3] = boundary;

                        if (boundary.Split(';').Length > 1)
                        {
                            mmsg.Boundary[3] = boundary.Split(';')[0];
                        }

                        //WriteLog("Boundary 3: " + boundary + line_end);
                    }
                }
            }
            catch (Exception er)
            {
                WriteLog("BOUNDARY : " + er.Message);
            }
        }
        #endregion

        #endregion

        #region function Mail_Body_Parsing
        /// <summary>
        /// 메일 본문 파싱
        /// </summary>
        private void Mail_Body_Parsing()
        {
            StringBuilder msgbody = new StringBuilder();

            try
            {
                //string msgbody = null;

                string type = "";

                string line = null;

                int encodetype = 5;
                string charset = "";

                //WriteLog("원본 : " + mmsg.Body_Str.ToString().TrimStart(sep).TrimEnd(sep));


                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(mmsg.Body_Str.ToString().TrimStart(sep).TrimEnd(sep))))
                {
                    using (StreamReader reader = new StreamReader((Stream)ms, Encoding.Default))
                    {
                        line = reader.ReadLine();

                        //WriteLog(line.TrimStart(sep).TrimEnd(sep));

                        if (mmsg.Boundary[0] == null)
                        {
                            if (mmsg.C_Encoding != null)
                            {
                                encodetype = Encoding_Type(mmsg.C_Encoding.ToLower());
                            }
                        }

                        int i = 0;
                        bool start = false;
                        //WriteLog("boundary : " + mmsg.Boundary[0].Length.ToString());


                        string encodingtype = null;

                        while (line != null)
                        {
                            line = line.TrimStart(sep).TrimEnd(sep);
                            //WriteLog(line);

                            #region boundary count = null
                            // 바운더리 스트링이 없는 경우 
                            if (mmsg.Boundary[0] == null)
                            {
                                if (line.ToLower().StartsWith("content-type:"))
                                {
                                }
                                else
                                {
                                    try
                                    {
                                        if ((mmsg.ContentType.Equals("text/html") || mmsg.ContentType.Equals("text/plain")) && encodetype == 4)
                                        {
                                            if (line.TrimEnd(sep).EndsWith("="))
                                            {
                                                msgbody.Append(line.Substring(0, line.Length - 1));
                                            }
                                            else
                                            {
                                                msgbody.Append(line + " " + line_end);
                                            }
                                        }
                                        else
                                        {
                                            msgbody.Append(line + line_end);
                                        }
                                    }
                                    catch
                                    {
                                        msgbody.Append(line + line_end);
                                    }
                                }
                            }
                            #endregion

                            #region boundary count = 1
                            else if (mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1] == null)
                            {
                                if (line.ToLower().StartsWith("content-transfer-encoding:"))
                                {
                                    // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                    encodingtype = line.Substring(26).ToLower().TrimStart(sep).TrimEnd(sep);

                                    encodetype = Encoding_Type(encodingtype);
                                }
                                else if (line.TrimStart(sep).StartsWith("--" + mmsg.Boundary[0]))
                                {
                                    //									WriteLog("i :" + i.ToString());
                                    //									WriteLog(line);

                                    if (msgbody.Length > 0)
                                    {
                                        if (i == 0)
                                        {
                                            msgbody.Length = 0;
                                        }
                                        else if (i == 1)
                                        {
                                            //WriteLog(type + " : " + msgbody.TrimStart(sep).TrimEnd(sep) + line_end + line_end);
                                            //Decoding_Body_Msg(encodetype,msgbody.TrimStart(sep).TrimEnd(sep),type, charset);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            charset = "";
                                        }
                                        else if (i == 2)
                                        {
                                            //WriteLog(type + " : " + msgbody.TrimStart(sep).TrimEnd(sep) + line_end + line_end);
                                            //Decoding_Body_Msg(encodetype,msgbody.TrimStart(sep).TrimEnd(sep),type, charset);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            charset = "";
                                            break;
                                        }
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-disposition:"))
                                {
                                    if (line.ToLower().IndexOf("attachment") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }

                                    if (line.ToLower().IndexOf("filename") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-length:"))
                                {

                                }
                                else if (line.ToLower().StartsWith("content-description:"))
                                {

                                }
                                else if (line.ToLower().StartsWith("\tcharset="))
                                {
                                    charset = line.ToLower().TrimStart(sep).TrimEnd(sep).Substring(8).Replace("\"", "").Trim();
                                }
                                else if (line.ToLower().StartsWith("charset="))
                                {
                                    charset = line.ToLower().TrimStart(sep).TrimEnd(sep).Substring(8).Replace("\"", "").Trim();
                                }
                                else if (line.ToLower().StartsWith("reply-type="))
                                {

                                }
                                else if (line.ToLower().StartsWith("content-type:"))
                                {
                                    //WriteLog("3line : " + line + line_end);

                                    while (line.TrimEnd(sep).EndsWith(";"))
                                    {
                                        line += reader.ReadLine();
                                    }

                                    if (line.IndexOf("charset=") > -1)
                                    {
                                        charset = line.Substring(line.IndexOf("charset=") + 8).Replace("\"", "").Trim();
                                        //WriteLog("charset : " + "." + charset + "."  + line_end);
                                    }

                                    type = line.Substring(13).TrimEnd(sep).TrimStart(sep);
                                    //WriteLog("type : " + "." + type + "." + line_end);

                                    if (type.EndsWith(";"))
                                    {
                                        type = type.Substring(0, type.Length - 1);
                                    }

                                    //WriteLog("type : " + "." + type + "." + line_end);

                                    int check_idx_1 = type.ToLower().IndexOf("content-transfer-encoding:");

                                    if (check_idx_1 > -1)
                                    {
                                        encodetype = Encoding_Type(type.Substring(check_idx_1 + 26).TrimStart(sep).TrimEnd(sep));
                                        //WriteLog("ENCODING : " + type.Substring(check_idx_1+26).TrimStart(sep).TrimEnd(sep) + line_end);
                                    }

                                    //WriteLog("type : " + "." + type + "." + line_end);

                                    int check_idx = type.IndexOf(";");

                                    if (check_idx > -1)
                                    {
                                        type = type.Substring(0, check_idx);

                                    }

                                    if (type.ToLower().StartsWith("multipart"))
                                    {

                                    }
                                    else if (line.ToLower().IndexOf("name=\"") > -1)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else if (type.ToLower().StartsWith("text/plain") || type.ToLower().StartsWith("text/html"))
                                    {
                                        //WriteLog("Type : " + "." + type + "." + line_end);
                                        i++;
                                    }
                                    else if (type.ToLower().StartsWith("application"))
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else
                                    {
                                        msgbody.Length = 0;
                                        break;
                                    }

                                    //WriteLog("Type : " + "." + type + "." + line_end);
                                }
                                else if (line.ToLower().TrimStart(sep).StartsWith("charset="))
                                {
                                    if (start)
                                    {
                                        msgbody.Append(line + line_end);
                                    }
                                }
                                else if (line.ToLower().TrimStart(sep).StartsWith("name=\""))
                                {
                                    //WriteLog("Break : " + "." + s.TrimStart(sep) + "." + line_end);
                                    msgbody.Length = 0;
                                    //break;
                                }
                                else if ((line.TrimStart(sep)).ToLower().StartsWith("charset=") && (line.TrimStart(sep)).LastIndexOf("3D") == -1)
                                {
                                    // 역시 이것도 무시
                                    //continue;
                                }
                                else if (line.TrimStart(sep).StartsWith("content-id:"))
                                {
                                    // 역시 이것도 무시
                                    //continue;
                                }
                                else if (line.ToLower().StartsWith("x-mime"))
                                {
                                    //WriteLog(line);
                                    // 한메일, 드림위에서 요런 것들이 생김
                                    // X-MIME-Autoconverted: from 8bit to quoted-printable by smail-6.hanmail.net id h62AsXWO018208
                                    //continue;
                                }
                                else if (line.TrimStart(sep).TrimEnd(sep).Equals(""))
                                {
                                    //WriteLog("공백 열...");
                                    start = true;
                                }
                                else
                                {
                                    if (start)
                                    {
                                        //WriteLog(line);

                                        if (line.TrimEnd(sep).TrimStart(sep) != "")
                                        {
                                            //WriteLog(line);
                                            //WriteLog("encodetype : " + encodetype);

                                            if (encodetype == 4) // quotedcoding...
                                            {
                                                if (line.TrimEnd(sep).EndsWith("="))
                                                {
                                                    line = line.TrimEnd(sep).TrimStart(sep);
                                                    msgbody.Append(line.Substring(0, line.Length - 1));
                                                }
                                                else
                                                {
                                                    msgbody.Append(line + line_end);
                                                }
                                            }
                                            else if (type.Equals("text/html"))
                                            {
                                                //WriteLog("type : " + "." + type  + "." + line_end);
                                                msgbody.Append(line.TrimEnd(sep).TrimStart(sep) + line_end);
                                            }
                                            else
                                            {
                                                //WriteLog(line);
                                                msgbody.Append(line + line_end);
                                            }
                                        }
                                    }

                                    //WriteLog("line 1 : " + line + line_end);
                                }
                            }
                            //}

                            #endregion

                            #region boundary count = 2
                            else if (mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2] == null)
                            {
                                if (line.ToLower().StartsWith("content-transfer-encoding:"))
                                {
                                    // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                    encodingtype = (line.Substring(26).TrimStart(sep).TrimEnd(sep)).ToLower();
                                    encodetype = Encoding_Type(encodingtype);

                                    //WriteLog("encodetype : " + encodetype + line_end);

                                }
                                else if (line.StartsWith("--" + mmsg.Boundary[0]) || line.StartsWith("--" + mmsg.Boundary[1]))
                                {
                                    if (msgbody.Length > 0)
                                    {
                                        if (i == 0)
                                        {
                                            msgbody.Length = 0;
                                            //continue;
                                        }
                                        else if (i == 1)
                                        {
                                            //											WriteLog("i : " + i);
                                            //											WriteLog("type : " + type);
                                            //											WriteLog("encodetype : " + encodetype);
                                            //											WriteLog("msgbody : " + msgbody);

                                            //WriteLog(type + " : " + msgbody + line_end + line_end);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            type = "";
                                            charset = "";
                                        }
                                        else if (i == 2)
                                        {
                                            //WriteLog("i : " + i);
                                            //WriteLog("type : " + type);
                                            //WriteLog(type + " : " + msgbody + line_end + line_end);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            charset = "";
                                            type = "";
                                            break;
                                        }
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-disposition:"))
                                {
                                    if (line.ToLower().IndexOf("attachment") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }

                                    if (line.ToLower().IndexOf("filename") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-length:"))
                                {
                                    //continue;
                                }
                                else if (line.ToLower().StartsWith("content-description:"))
                                {

                                }
                                else if (line.ToLower().StartsWith("content-type:"))
                                {
                                    while (line.TrimEnd(sep).EndsWith(";"))
                                    {
                                        line += reader.ReadLine();
                                    }

                                    //WriteLog("line : " + "." + line + "."  + line_end);

                                    if (line.IndexOf("charset=") > -1)
                                    {
                                        charset = line.Substring(line.IndexOf("charset=") + 8).Replace("\"", "").Trim();

                                        if (AssertMail)
                                        {
                                            WriteLog("charset : " + "." + charset + ".");
                                        }
                                    }

                                    type = line.Substring(13).TrimEnd(sep).TrimStart(sep);

                                    if (type.EndsWith(";"))
                                    {
                                        type = type.Substring(0, type.Length - 1);
                                    }

                                    int check_idx_1 = type.ToLower().IndexOf("content-transfer-encoding:");

                                    if (check_idx_1 > -1)
                                    {

                                        encodetype = Encoding_Type(type.Substring(check_idx_1 + 26).TrimStart(sep).TrimEnd(sep));
                                        //WriteLog("ENCODING : " + "." + type.Substring(check_idx_1+26).TrimStart(sep).TrimEnd(sep) + "." + line_end);
                                    }


                                    int check_idx = type.IndexOf(";");

                                    if (check_idx > -1)
                                    {
                                        type = type.Substring(0, check_idx);
                                    }

                                    if (type.ToLower().StartsWith("multipart"))
                                    {

                                    }
                                    else if (line.ToLower().IndexOf("name=\"") > -1)
                                    {
                                        //WriteLog("line : " + "." + line + "." + line_end);
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else if (type.ToLower().StartsWith("text/plain") || type.ToLower().StartsWith("text/html"))
                                    {
                                        //WriteLog("Type : " + "." + type + "." + line_end);
                                        i++;
                                    }
                                    else if (type.ToLower().StartsWith("application"))
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else
                                    {
                                        msgbody.Length = 0;
                                        break;
                                    }
                                    //WriteLog("Type : " + "." + type + "." + line_end);
                                }
                                else if (line.ToLower().TrimStart(sep).StartsWith("name=\""))
                                {
                                    msgbody.Length = 0;
                                    //break;
                                }
                                else if ((line.TrimStart(sep)).ToLower().StartsWith("charset=") && (line.TrimStart(sep)).LastIndexOf("3D") == -1)
                                {
                                    if (start)
                                    {
                                        msgbody.Append(line + line_end);
                                    }
                                }
                                else if (line.TrimStart(sep).StartsWith("content-id:"))
                                {
                                    // 역시 이것도 무시
                                    //continue;
                                }
                                else if (line.ToLower().StartsWith("x-mime"))
                                {
                                    // 한메일에서 요런 것들이 생김
                                    // X-MIME-Autoconverted: from 8bit to quoted-printable by smail-6.hanmail.net id h62AsXWO018208
                                    //continue;
                                }
                                else if (line.TrimStart(sep).TrimEnd(sep).Equals(""))
                                {
                                    start = true;
                                }
                                else
                                {
                                    if (start)
                                    {
                                        if (encodetype == 4)
                                        {
                                            if (line.TrimEnd(sep).EndsWith("="))
                                            {
                                                line = line.TrimEnd(sep).TrimStart(sep);
                                                msgbody.Append(line.Substring(0, line.Length - 1));
                                            }
                                            else
                                            {
                                                msgbody.Append(line + line_end);
                                            }
                                        }
                                        else if (type.Equals("text/html"))
                                        {
                                            if (encodetype == 1)
                                            {
                                                msgbody.Append(line.TrimEnd(sep).TrimStart(sep));
                                            }
                                            else
                                            {
                                                //WriteLog("type : " + "." + type  + "." + line_end);
                                                msgbody.Append(line.TrimEnd(sep).TrimStart(sep) + line_end);
                                            }
                                        }
                                        else
                                        {
                                            msgbody.Append(line + line_end);
                                        }
                                    }
                                }
                            }
                            //}
                            #endregion

                            #region boundary count = 3
                            else if (mmsg.Boundary[0].Length > 0 && mmsg.Boundary[1].Length > 0 && mmsg.Boundary[2].Length > 0)
                            {
                                if (line.ToLower().StartsWith("content-transfer-encoding:"))
                                {
                                    // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                    encodingtype = (line.Substring(26).TrimStart(sep).TrimEnd(sep)).ToLower();
                                    encodetype = Encoding_Type(encodingtype);

                                    //WriteLog("encodetype : " + encodetype + line_end);

                                }
                                else if (line.StartsWith("--" + mmsg.Boundary[0]) || line.StartsWith("--" + mmsg.Boundary[1]) || line.StartsWith("--" + mmsg.Boundary[2]))
                                {
                                    if (msgbody.Length > 0)
                                    {
                                        if (i == 0)
                                        {
                                            msgbody.Length = 0;
                                            //continue;
                                        }
                                        else if (i == 1)
                                        {
                                            //WriteLog(type + " : " + msgbody + line_end + line_end);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            type = "";
                                            charset = "";
                                        }
                                        else if (i == 2)
                                        {
                                            //WriteLog(type + " : " + msgbody + line_end + line_end);
                                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                                            msgbody.Length = 0;
                                            encodetype = 5;
                                            type = "";
                                            charset = "";
                                            break;
                                        }
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-disposition:"))
                                {
                                    if (line.ToLower().IndexOf("attachment") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }

                                    if (line.ToLower().IndexOf("filename") > 0)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                }
                                else if (line.ToLower().StartsWith("content-length:"))
                                {
                                    //continue;
                                }
                                else if (line.ToLower().StartsWith("content-description:"))
                                {

                                }
                                else if (line.ToLower().StartsWith("content-type:"))
                                {
                                    while (line.TrimEnd(sep).EndsWith(";"))
                                    {
                                        line += reader.ReadLine();
                                    }


                                    if (line.IndexOf("charset=") > -1)
                                    {
                                        charset = line.Substring(line.IndexOf("charset=") + 8).Replace("\"", "").Trim();
                                        //WriteLog("charset : " + "." + charset + "."  + line_end);
                                    }

                                    type = line.Substring(13).TrimEnd(sep).TrimStart(sep);

                                    if (type.EndsWith(";"))
                                    {
                                        type = type.Substring(0, type.Length - 1);
                                    }

                                    //WriteLog("C_TYPE : " + type + line_end);

                                    int check_idx_1 = type.ToLower().IndexOf("content-transfer-encoding:");

                                    if (check_idx_1 > -1)
                                    {

                                        encodetype = Encoding_Type(type.Substring(check_idx_1 + 26).TrimStart(sep).TrimEnd(sep));
                                        //WriteLog("ENCODING : " + type.Substring(check_idx_1+26).TrimStart(sep).TrimEnd(sep) + line_end);
                                    }


                                    int check_idx = type.IndexOf(";");

                                    if (check_idx > -1)
                                    {
                                        type = type.Substring(0, check_idx);
                                    }

                                    if (type.ToLower().StartsWith("multipart"))
                                    {
                                        //continue;
                                    }
                                    else if (line.ToLower().IndexOf("name=\"") > -1)
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else if (type.ToLower().StartsWith("text/plain") || type.ToLower().StartsWith("text/html"))
                                    {
                                        //WriteLog("Type : " + "." + type + "." + line_end);
                                        i++;
                                    }
                                    else if (type.ToLower().StartsWith("application"))
                                    {
                                        msgbody.Length = 0;
                                        //break;
                                    }
                                    else
                                    {
                                        msgbody.Length = 0;
                                        break;
                                    }
                                    //WriteLog("Type : " + "." + type + "." + line_end);
                                }
                                else if (line.ToLower().TrimStart(sep).StartsWith("name=\""))
                                {
                                    msgbody.Length = 0;
                                    //break;
                                }
                                else if ((line.TrimStart()).ToLower().StartsWith("charset=") && (line.TrimStart()).LastIndexOf("3D") == -1)
                                {
                                    if (start)
                                    {
                                        msgbody.Append(line + line_end);
                                    }
                                }
                                else if (line.TrimStart(sep).StartsWith("boundary="))
                                {
                                    // 역시 이것도 무시
                                    //continue;
                                }
                                else if (line.TrimStart(sep).StartsWith("content-id:"))
                                {
                                    // 역시 이것도 무시
                                    //continue;
                                }
                                else if (line.ToLower().StartsWith("x-mime"))
                                {
                                    // 한메일에서 요런 것들이 생김
                                    // X-MIME-Autoconverted: from 8bit to quoted-printable by smail-6.hanmail.net id h62AsXWO018208
                                    //continue;
                                }
                                else if (line.TrimStart(sep).TrimEnd(sep).Equals(""))
                                {
                                    start = true;
                                }
                                else
                                {
                                    if (start)
                                    {
                                        if (encodetype == 4)
                                        {
                                            if (line.TrimEnd(sep).EndsWith("="))
                                            {
                                                line = line.TrimEnd(sep).TrimStart(sep);
                                                msgbody.Append(line.Substring(0, line.Length - 1));
                                            }
                                            else
                                            {
                                                msgbody.Append(line + line_end);
                                            }
                                        }
                                        else if (type.Equals("text/html"))
                                        {
                                            //WriteLog("type : " + "." + type  + "." + line_end);
                                            msgbody.Append(line.TrimEnd(sep).TrimStart(sep) + line_end);
                                        }
                                        else
                                        {
                                            msgbody.Append(line + line_end);
                                        }
                                    }
                                }
                            }
                            //}

                            #endregion

                            line = reader.ReadLine();
                        }
                        reader.Close();
                    }
                    ms.Close();
                }
                if (mmsg.Boundary[0] == null)
                {
                    //WriteLog("++++++++++ : " + msgbody.ToString());
                    Decoding_Body_Msg(encodetype, msgbody, mmsg.ContentType, charset);
                }
                else
                {
                    try
                    {
                        if (msgbody.Length > 0)
                        {
                            //WriteLog("encodetype : " + encodetype);
                            //WriteLog("charset : " + charset);

                            //Decoding_Body_Msg(encodetype,msgbody.TrimStart(sep).TrimEnd(sep),type, charset);
                            Decoding_Body_Msg(encodetype, msgbody, type, charset);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch (Exception er)
            {
                string dummy = er.Message;

                if (AssertMail)
                {
                    WriteLog("Body : " + dummy);
                }
                //WriteLog("BODY : " + er.Message);
            }
            finally
            {
                msgbody = null;
            }

        }
        #endregion

        #region function Mail_Attach_Parsing
        /// <summary>
        /// 메일 첨부 파일 파싱
        /// </summary>
        private void Mail_Attach_Parsing()
        {
            StringBuilder file_data = new StringBuilder();

            try
            {
                image = false;
                audio = false;
                message = false;

                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(mmsg.Body_Str.ToString())))
                {
                    using (StreamReader reader = new StreamReader((Stream)ms, Encoding.Default))
                    {
                        string line = reader.ReadLine();
                        int encodetype = 3;
                        string file_name = "";

                        bool ok = false;
                        string boundarystr = null;

                        string encodingtype = null;

                        while (line != null)
                        {

                            #region Start Boundary

                            if (line.StartsWith("--" + mmsg.Boundary[0]))
                            {
                                boundarystr = mmsg.Boundary[0];
                                //WriteLog("Boundary 0: " + boundarystr);
                                //WriteLog("Line : " + line + line_end);
                            }

                            else if (mmsg.Boundary[1] != null && line.StartsWith("--" + mmsg.Boundary[1]))
                            {
                                boundarystr = mmsg.Boundary[1];

                                //WriteLog("Boundary 1: " + boundarystr);
                            }
                            else if (mmsg.Boundary[2] != null && line.StartsWith("--" + mmsg.Boundary[2]))
                            {
                                boundarystr = mmsg.Boundary[1];

                                //WriteLog("Boundary 2: " + boundarystr);
                            }
                            else if (mmsg.Boundary[3] != null && line.StartsWith("--" + mmsg.Boundary[3]))
                            {
                                boundarystr = mmsg.Boundary[1];

                                //WriteLog("Boundary 3: " + boundarystr);
                            }
                            #endregion

                            #region Start Point : content-type
                            else if (line.ToLower().TrimStart(sep).StartsWith("content-type:"))
                            {
                                if (line.TrimEnd(sep).EndsWith(";"))
                                {
                                    line += reader.ReadLine();
                                }

                                //WriteLog("3 : " + line);

                                if (line.ToLower().IndexOf("image") > -1)
                                {
                                    image = true;
                                    ok = true;

                                    //WriteLog("Image : " + line + line_end);
                                }

                                if (line.ToLower().IndexOf("audio") > -1)
                                {
                                    audio = true;
                                    ok = true;
                                }

                                //Content-Type: message/rfc822 -- 다움에서 전달을 하게 되면..첨부 형태로 들어옴
                                //X-HM-IDENT: attach

                                if (line.ToLower().IndexOf("message/rfc822") > -1)
                                {
                                    message = true;
                                    ok = true;
                                }

                                if (line.ToLower().IndexOf("application") > -1)
                                {
                                    ok = true;
                                }

                                //WriteLog("message/rfc822\t" + message.ToString() );


                                int startidx = line.ToLower().IndexOf("name=");


                                if (startidx == -1)
                                {
                                    startidx = line.ToLower().IndexOf("name = ");
                                }



                                //WriteLog(line);


                                if (startidx > -1)
                                {
                                    string full_file_name = "";
                                    file_name = line.Substring(startidx + 4).Trim();

                                    //WriteLog("file_name 1 : " + file_name);

                                    if (file_name.StartsWith("="))
                                    {
                                        file_name = file_name.Substring(1).Trim();
                                    }

                                    //WriteLog("file_name 2 : " + file_name);

                                    full_file_name = file_name;


                                    if (file_name.IndexOf(";") > -1) // 계속 줄줄이 붙어 있으면...구글 메일...
                                    {
                                        int end_file_name_idx = file_name.IndexOf(";");
                                        file_name = file_name.Substring(0, end_file_name_idx);

                                        //WriteLog("file_name 3 : " + file_name);

                                        file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);
                                        file_name = QuotedCoding.DecodeOne(file_name);

                                        //WriteLog("file_name 4 : " + file_name);

                                        if (line.Split('\"').Length == 2) // " 한개만 있을 경우.. 다음 라인 읽기
                                        {
                                            file_name += QuotedCoding.DecodeOne(reader.ReadLine().Trim());
                                        }

                                        //WriteLog("file_name 5 : " + file_name);


                                        file_name = Check_File_Name(file_name);

                                        ok = true;

                                        if (image && file_name.IndexOf(".") == -1 && file_name.Length > 0)
                                        {
                                            file_name = file_name + ".png";
                                        }

                                        //WriteLog("File_Name 11 : " + file_name);


                                        if (full_file_name.ToLower().IndexOf("content-transfer-encoding:") > -1)
                                        {

                                            //WriteLog(full_file_name.Substring(end_file_name_idx));

                                            // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                            encodingtype = (full_file_name.Substring(end_file_name_idx + 1 + 26).Replace(":", "").TrimStart(sep).TrimEnd(sep)).ToLower();
                                            encodetype = Encoding_Type(encodingtype);

                                            //WriteLog(encodingtype);

                                            line = reader.ReadLine();
                                            line = reader.ReadLine();

                                            file_data.Length = 0;

                                            while (!line.TrimStart(sep).StartsWith("--" + boundarystr))
                                            {
                                                //WriteLog("MSG : " + line + line_end);

                                                file_data.Append(line.TrimStart(sep).TrimEnd(sep) + line_end);
                                                //file_data += line + line_end;

                                                line = reader.ReadLine();
                                            }

                                            //WriteLog("Last Line : " + line + line_end);


                                            if (image)
                                            {
                                                if (file_name.IndexOf(".") == -1)
                                                {
                                                    file_name = file_name + ".png";
                                                }
                                            }


                                            //WriteLog(file_name);
                                            //WriteLog(encodetype);
                                            //WriteLog("Start : " + file_data.ToString().Substring(0,10));
                                            //WriteLog("End : " + file_data.ToString().Substring(file_data.ToString().Length-10));


                                            Convert_File(file_name, encodetype, file_data);

                                            file_data.Length = 0;

                                            boundarystr = null;

                                            image = false;
                                            audio = false;
                                            ok = false;
                                            file_name = "";


                                            encodetype = 3;
                                        }

                                    }
                                    else
                                    {
                                        if (file_name.IndexOf("\"") > -1) // 라인 끝에 " 큰 따옴표가 있는데 줄바꿈 되어 있다면..
                                        {
                                            if (!line.ToLower().EndsWith("\"")) // 다음줄이 더 있을 경우
                                            {

                                                //WriteLog("filename 66 : " + file_name);

                                                file_name = file_name.Replace("\"", "");

                                                if (file_name.EndsWith("?="))
                                                {
                                                    file_name = QuotedCoding.DecodeOne(file_name);
                                                }

                                                //WriteLog("filename 77 : " + file_name);

                                                line = reader.ReadLine().Replace("\"", "").TrimEnd(sep);

                                                if (line.TrimEnd(sep).EndsWith("?="))
                                                {
                                                    file_name += QuotedCoding.DecodeOne(line.Replace("\"", "").Trim(sep));
                                                }
                                                else
                                                {
                                                    //string file_name_add = line.Replace("\"","").TrimStart(sep).TrimEnd(sep);
                                                    file_name += QuotedCoding.DecodeOne(line.Replace("\"", "").TrimStart(sep).TrimEnd(sep));
                                                }

                                                //WriteLog("filename 88 : " + file_name);
                                            }
                                            else
                                            {
                                                file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);
                                                file_name = QuotedCoding.DecodeOne(file_name);
                                            }
                                        }
                                        else
                                        {

                                            file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);
                                            file_name = QuotedCoding.DecodeOne(file_name);
                                        }

                                        file_name = Check_File_Name(file_name);

                                        //WriteLog(file_name);

                                        ok = true;

                                        if (image && file_name.IndexOf(".") == -1 && file_name.Length > 0)
                                        {
                                            file_name = file_name + ".png";
                                        }

                                    }
                                }
                                else
                                {
                                    if (message)
                                    {
                                        file_name = "ATT" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Millisecond.ToString() + ".eml";
                                    }
                                }
                            }
                            #endregion

                            #region X-HM-IDENT
                            else if (line.TrimStart(sep).ToLower().StartsWith("x-hm-ident:"))
                            {

                                //Content-Type: message/rfc822
                                //X-HM-IDENT: attach

                                if (line.TrimStart(sep).ToLower().IndexOf("attach") > -1)
                                {
                                    //WriteLog("Line : " + line);

                                    if (message)
                                    {
                                        line = reader.ReadLine();
                                        line = reader.ReadLine();

                                        file_data.Length = 0;

                                        while (!line.TrimStart(sep).StartsWith("--" + boundarystr))
                                        {
                                            if (line.TrimEnd(sep).ToLower().StartsWith("subject:"))
                                            {
                                                file_name = QuotedCoding.Decode(line.TrimEnd(sep).Substring(8).Trim()) + ".eml";
                                            }

                                            file_data.Append(line.TrimEnd(sep) + line_end);

                                            //file_data += line + line_end;

                                            line = reader.ReadLine();
                                        }

                                        if (file_name.Length.Equals(0))
                                        {
                                            file_name = mmsg.Subject + ".eml";
                                        }

                                        Convert_File(file_name, encodetype, file_data);

                                        file_data.Length = 0;
                                    }
                                }
                            }
                            #endregion

                            #region Content-Disposition
                            else if (line.TrimStart(sep).ToLower().StartsWith("content-disposition:"))
                            {
                                //WriteLog(line);

                                if (line.EndsWith(";"))
                                {
                                    line += reader.ReadLine();
                                }

                                //WriteLog("1 : " + line);

                                if (line.ToLower().IndexOf("inline") > -1)
                                {
                                    //WriteLog("첨부 파일 아님" + line_end);
                                }
                                else
                                {

                                    //WriteLog("첨부 파일 맞음");

                                    //WriteLog("LIne 2 : " + line);

                                    try
                                    {
                                        //WriteLog("첨부 파일 맞음" + line_end);

                                        int startidx = line.ToLower().IndexOf("filename=");

                                        if (startidx == -1)
                                        {
                                            startidx = line.ToLower().IndexOf("filename = ");
                                        }

                                        file_name = line.Substring(startidx + 8).Trim();


                                        if (file_name.StartsWith("="))
                                        {
                                            file_name = file_name.Substring(1).Trim();
                                        }


                                        //WriteLog("File_Name 1 : " + file_name + ".");


                                        if (file_name.IndexOf("\"") > -1) // 라인 끝에 " 큰 따옴표가 있는데 줄바꿈 되어 있다면..
                                        {
                                            if (!line.ToLower().EndsWith("\""))
                                            {
                                                file_name = file_name.Replace("\"", "");

                                                if (file_name.EndsWith("?="))
                                                {
                                                    file_name = QuotedCoding.DecodeOne(file_name);
                                                }

                                                line = reader.ReadLine();
                                                //string file_name_add = line.Replace("\"","").TrimStart(sep).TrimEnd(sep);
                                                file_name += QuotedCoding.DecodeOne(line.Replace("\"", "").TrimStart(sep).TrimEnd(sep));
                                            }
                                            else
                                            {
                                                file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);
                                                file_name = QuotedCoding.DecodeOne(file_name);
                                            }
                                        }
                                        else
                                        {

                                            file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);
                                            file_name = QuotedCoding.DecodeOne(file_name);
                                        }

                                        //WriteLog("File_Name 2 : " + file_name + ".");


                                        file_name = Check_File_Name(file_name);

                                        if (image && file_name.IndexOf(".") == -1)
                                        {
                                            file_name = file_name + ".png";
                                        }

                                        if (file_name.Length > 0)
                                        {
                                            ok = true;
                                        }
                                    }
                                    catch //(Exception er)
                                    {
                                        //WriteLog(er.Message);
                                    }

                                    //WriteLog("final file name : " + file_name);


                                    while (line.TrimStart(sep).TrimEnd(sep) != "")
                                    {
                                        line = reader.ReadLine();

                                        if (line.ToLower().TrimStart(sep).StartsWith("content-transfer-encoding:"))
                                        {
                                            //WriteLog(line);

                                            if (ok)
                                            {
                                                // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                                encodingtype = (line.Substring(26).TrimStart(sep).TrimEnd(sep)).ToLower();

                                                encodetype = Encoding_Type(encodingtype);

                                                if (AssertMail)
                                                {
                                                    WriteLog("encodingtype : " + encodingtype);
                                                }
                                            }
                                        }
                                    }

                                    line = reader.ReadLine();

                                    file_data.Length = 0;

                                    while (!line.TrimStart(sep).StartsWith("--" + boundarystr))
                                    {
                                        //WriteLog("MSG : " + line);
                                        file_data.Append(line.TrimEnd(sep) + line_end);
                                        //file_data += line + line_end;

                                        line = reader.ReadLine();
                                    }


                                    //WriteLog(file_data.ToString().Substring(0,100));

                                    //WriteLog("3");
                                    Convert_File(file_name, encodetype, file_data);

                                    file_data.Length = 0;


                                    boundarystr = null;

                                    image = false;

                                    encodetype = 3;
                                }

                            }
                            #endregion

                            #region content-id
                            else if (line.ToLower().TrimStart(sep).StartsWith("content-id:"))
                            {
                                if (image || audio)
                                {
                                    int start_idx = line.IndexOf("<");
                                    int end_idx = line.IndexOf(">");

                                    string C_ID = line.Substring(start_idx + 1, end_idx - start_idx - 1);

                                    if (file_name == "")
                                    {
                                        if (image)
                                        {
                                            file_name = C_ID + ".png";
                                        }
                                        else
                                        {
                                            file_name = C_ID;
                                        }
                                    }

                                    //WriteLog(file_name);

                                    if (!mmsg.Content_ID.ContainsKey(C_ID))
                                    {
                                        mmsg.Content_ID.Add(C_ID, C_ID);
                                        mmsg.Contents_Html = mmsg.Contents_Html.Replace("cid:" + C_ID, "cid:" + C_ID + file_name);
                                    }

                                    //WriteLog("C ID : " + C_ID);
                                }
                            }
                            #endregion

                            #region Content-Description
                            else if (line.ToLower().TrimStart(sep).StartsWith("content-description:"))
                            {

                            }
                            #endregion

                            #region content-transfer-encoding
                            else if (line.ToLower().TrimStart(sep).StartsWith("content-transfer-encoding:"))
                            {
                                //WriteLog("2 : " + line);
                                //WriteLog("2 boundstr : " + boundarystr);

                                if (AssertMail)
                                {
                                    WriteLog("ok : " + ok.ToString());
                                }


                                // 이 메세지 이면..인코딩 타입을 알아와야 한다.
                                encodingtype = (line.Substring(26).TrimStart(sep).TrimEnd(sep)).ToLower();

                                if (AssertMail)
                                {
                                    WriteLog("encodingtype : " + encodingtype);
                                }

                                encodetype = Encoding_Type(encodingtype);


                                if (ok)
                                {

                                    //WriteLog("2-1 : " + line);


                                    //WriteLog(encodetype);


                                    line = reader.ReadLine();

                                    #region Content-Description
                                    if (line.TrimStart(sep).ToLower().StartsWith("content-description:"))
                                    {
                                        line = reader.ReadLine();
                                    }
                                    #endregion

                                    //WriteLog("Line : " + line);

                                    // 전형적인 첨부 파일 형태 || 아웃룩에서 일부 메일들에서 생기는 현상
                                    #region content-disposition
                                    if (line.TrimStart(sep).ToLower().StartsWith("content-disposition:"))
                                    {
                                        if (line.EndsWith(";"))
                                        {
                                            line += reader.ReadLine();
                                        }

                                        line = line.Substring(20).TrimStart(sep).TrimEnd(sep);

                                        //WriteLog("LIne 2 : " + line + line_end);

                                        try
                                        {
                                            //WriteLog("첨부 파일 맞음" + line_end);

                                            int startidx = line.ToLower().IndexOf("filename=\"");

                                            if (startidx > -1)
                                            {
                                                file_name = line.Substring(startidx + 10);
                                                file_name = file_name.Replace("\"", "").TrimEnd(sep).TrimStart(sep);

                                                file_name = QuotedCoding.DecodeOne(file_name);

                                                //WriteLog("NEW File_Name : " + file_name);

                                                if (!line.ToLower().EndsWith("\""))
                                                {
                                                    line = reader.ReadLine();
                                                    //string file_name_add = line.Replace("\"","").TrimStart(sep).TrimEnd(sep);
                                                    file_name += QuotedCoding.DecodeOne(line.Replace("\"", "").TrimStart(sep).TrimEnd(sep));
                                                }

                                                file_name = Check_File_Name(file_name);

                                                //WriteLog("File_Name 2 : " + file_name);

                                            }

                                            line = reader.ReadLine();


                                            if (image && file_name.IndexOf(".") == -1 && file_name.Length > 0)
                                            {
                                                file_name = file_name + ".png";
                                            }

                                            if (line.TrimStart(sep).ToLower().StartsWith("content-id:"))
                                            {
                                                if (image || audio)
                                                {
                                                    int start_idx = line.IndexOf("<");
                                                    int end_idx = line.IndexOf(">");

                                                    string C_ID = line.Substring(start_idx + 1, end_idx - start_idx - 1);

                                                    if (file_name == "")
                                                    {
                                                        if (image)
                                                        {
                                                            file_name = C_ID + ".png";
                                                        }
                                                        else
                                                        {
                                                            file_name = C_ID;
                                                        }
                                                    }

                                                    //WriteLog(file_name);

                                                    if (!mmsg.Content_ID.ContainsKey(C_ID))
                                                    {
                                                        mmsg.Content_ID.Add(C_ID, C_ID);
                                                        mmsg.Contents_Html = mmsg.Contents_Html.Replace("cid:" + C_ID, "cid:" + C_ID + file_name);
                                                    }

                                                    //WriteLog("C ID : " + C_ID + line_end);
                                                }
                                            }
                                        }
                                        catch //(Exception er)
                                        {
                                            //WriteLog(er.Message);
                                        }


                                        //WriteLog("ATTACH : " + file_name + line_end);

                                    }

                                    #endregion

                                    // 아웃룩에서 일부 메일들에서 생기는 현상
                                    #region content-location
                                    else if (line.TrimStart(sep).ToLower().StartsWith("content-location:"))
                                    {
                                        //string loc = line.Substring(17).TrimStart(sep).TrimEnd(sep);

                                        //WriteLog("LOC : " + loc + line_end);

                                    }
                                    #endregion

                                    // 본문 내용에 이미지를 다이렉트로 드레그했을때 생김
                                    #region content-id
                                    else if (line.TrimStart(sep).ToLower().StartsWith("content-id:"))
                                    {
                                        //if (image || audio)
                                        //{
                                        if (image && file_name.IndexOf(".") == -1 && file_name.Length > 0)
                                        {
                                            file_name = file_name + ".png";
                                        }

                                        int start_idx = line.IndexOf("<");
                                        int end_idx = line.IndexOf(">");

                                        string C_ID = line.Substring(start_idx + 1, end_idx - start_idx - 1);

                                        //WriteLog(C_ID);
                                        if (file_name == "")
                                        {
                                            if (image)
                                            {
                                                file_name = C_ID + ".png";
                                            }
                                            else
                                            {
                                                file_name = C_ID;
                                            }
                                        }

                                        //WriteLog(file_name);

                                        if (!mmsg.Content_ID.ContainsKey(C_ID))
                                        {
                                            mmsg.Content_ID.Add(C_ID, C_ID);
                                            mmsg.Contents_Html = mmsg.Contents_Html.Replace("cid:" + C_ID, "cid:" + C_ID + file_name);
                                        }

                                        //WriteLog("C ID : " + mmsg.Content_ID + line_end);
                                        //}
                                    }
                                    #endregion




                                    //WriteLog("다음줄 : " + line);


                                    while (line.TrimStart(sep).TrimEnd(sep) != "")
                                    {
                                        line = reader.ReadLine();
                                    }

                                    line = reader.ReadLine();

                                    file_data.Length = 0;

                                    while (!line.TrimStart(sep).StartsWith("--" + boundarystr))
                                    {
                                        //WriteLog("MSG : " + line);

                                        file_data.Append(line.TrimEnd(sep) + line_end);
                                        //file_data += line + line_end;

                                        line = reader.ReadLine();
                                    }

                                    //WriteLog("Last Line 1 : " + line + line_end);


                                    if (image)
                                    {
                                        if (file_name.IndexOf(".") == -1)
                                        {
                                            file_name = file_name + ".png";
                                        }
                                    }

                                    //WriteLog("Last Line 2 : " + line + line_end);


                                    Convert_File(file_name, encodetype, file_data);

                                    //WriteLog("Last Line 3 : " + line + line_end);

                                    file_data.Length = 0;

                                    //WriteLog("Last Line 4 : " + line + line_end);

                                    boundarystr = null;

                                    image = false;
                                    audio = false;
                                    ok = false;
                                    file_name = "";

                                    //WriteLog("Last Line 5 : " + line + line_end);


                                    encodetype = 3;

                                    //WriteLog("Last Line 6 : " + line + line_end);
                                }

                            }
                            #endregion

                            #region message/rfc822
                            else if (message)
                            {
                                //StringBuilder file_data = new StringBuilder();

                                file_data.Length = 0;

                                line = reader.ReadLine();

                                if (line.TrimStart(sep).TrimEnd(sep).Length > 0)
                                {
                                    file_data.Append(line.TrimStart(sep).TrimEnd(sep) + line_end);
                                }

                                while (!line.TrimStart(sep).StartsWith("--" + boundarystr))
                                {
                                    if (line.TrimStart(sep).TrimEnd(sep).ToLower().StartsWith("subject:"))
                                    {
                                        file_name = QuotedCoding.Decode(line.TrimStart(sep).TrimEnd(sep).Substring(8).Trim()) + ".eml";
                                    }

                                    file_data.Append(line.TrimStart(sep).TrimEnd(sep) + line_end);
                                    //file_data += line + line_end;

                                    line = reader.ReadLine();
                                }

                                if (file_name.Length.Equals(0))
                                {
                                    file_name = mmsg.Subject + ".eml";
                                }

                                Convert_File(file_name, encodetype, file_data);

                                file_data.Length = 0;

                                //WriteLog("message/rfc822\t" + file_name + message.ToString() );
                            }
                            #endregion

                            line = reader.ReadLine();
                        }

                        reader.Close();
                    }
                    ms.Close();
                }
            }
            catch (Exception er)
            {
                //WriteLog("ATTACH 33: " + er.Message);
                if (AssertMail)
                {
                    WriteLog("ATTACH 33: " + er.Message);
                }
            }
            finally
            {
                if (file_data != null)
                {
                    file_data = null;
                }
            }

        }
        #endregion

        #region function Decoding_Body_Msg

        public void Decoding_Body_Msg(int encodetype, StringBuilder msgbody, string type, string character_set)
        {
            try
            {

                if (character_set.Equals(""))
                {
                    if (mmsg.Content_CharSet != null && mmsg.Content_CharSet.Length > 0)
                    {
                        character_set = mmsg.Content_CharSet;
                    }
                }


                //WriteLog("encodetype : " + "." + encodetype + ".");
                //WriteLog("type : " + "." + type + ".");
                //WriteLog("character_set : " + character_set);


                if (msgbody.Length > 0)
                {
                    if (type == null)
                    {
                        type = "text/plain";
                    }

                    string decoding_msg = Decoding_Common(msgbody, encodetype, character_set.ToLower());

                    ////					WriteLog("encodetype : " + "." + encodetype + "." + "\t" + "type : " + "." + type + "." + line_end);
                    ////					WriteLog("character_set : " + character_set);
                    //					WriteLog("msgbody : " + "." + decoding_msg + "." + line_end);
                    //

                    if (type.ToLower().Equals("text/html"))
                    {
                        mmsg.Contents_Html = decoding_msg;
                    }
                    else
                    {
                        if (mmsg.Contents_Text == null)
                        {
                            mmsg.Contents_Text = decoding_msg;
                        }
                    }
                }
            }
            catch (Exception er)
            {
                if (AssertMail)
                {
                    WriteLog("Decoding_Body_Msg Error : " + er.Message);
                    //Guid ttl = Guid.NewGuid();
                    //File.Copy(mmsg.Full_File_Name, @"C:\web_mail_data\샘플\error\" + ttl.ToString() + ".eml");
                    //WriteLog("DECODING BODY MSG : " + er.Message);
                }
            }
        }

        #endregion

        #region function Decoding_Common

        private string Decoding_Common(StringBuilder msgbody, int encodetype, string character_set)
        {
            string decoding_msg = null;

            if (character_set.Trim() == "")
            {
                character_set = mmsg.Content_CharSet;
            }
            if (AssertMail)
            {
                WriteLog("msgbody : " + msgbody.ToString().Substring(0, 100));
                //				WriteLog();
                //				WriteLog();
                //				WriteLog();
                WriteLog("character_set : " + "." + character_set + ".");
                WriteLog("encodetype : " + encodetype);
            }

            try
            {
                switch (encodetype)
                {
                    case 1:  // base64
                        {

                            try
                            {
                                decoding_msg = Encoding.GetEncoding(character_set).GetString(Convert.FromBase64String(msgbody.ToString().TrimEnd(sep)));
                            }
                            catch
                            {
                                decoding_msg = Encoding.Default.GetString(Convert.FromBase64String(msgbody.ToString().TrimEnd(sep)));
                            }


                            //						if (character_set.Equals("utf-8"))
                            //						{
                            //							decoding_msg = Encoding.UTF8.GetString(Convert.FromBase64String(msgbody.ToString()));
                            //						}
                            //						else
                            //						{
                            //							decoding_msg = Encoding.Default.GetString(Convert.FromBase64String(msgbody.ToString()));
                            //						}
                            break;
                        }
                    case 2:  // 7bit
                        {
                            try
                            {
                                if (character_set.Trim() == "")
                                {
                                    character_set = "iso-2022-kr";
                                }
                                decoding_msg = Encoding.GetEncoding(character_set).GetString(Encoding.GetEncoding(character_set).GetBytes(msgbody.ToString().TrimEnd(sep)));
                            }
                            catch
                            {
                                character_set = "iso-8859-1";

                                decoding_msg = Encoding.GetEncoding(character_set).GetString(Encoding.GetEncoding(character_set).GetBytes(msgbody.ToString().TrimEnd(sep)));

                            }

                            break;

                        }
                    case 3:  // 8bit
                        {
                            //decoding_msg = Encoding.Default.GetString(Encoding.ASCII.GetBytes(msgbody));
                            decoding_msg = msgbody.ToString().TrimEnd(sep);
                            break;
                        }
                    case 4:  // quoted-printable
                        {
                            decoding_msg = Q_Decoding(msgbody.ToString().TrimEnd(sep), character_set);
                            break;
                        }
                    case 5:    // 현재 파싱하는 것 외에...
                        {

                            //WriteLog("character_set : " + character_set);

                            if (character_set == "iso-2022-kr")
                            {
                                decoding_msg = Encoding.GetEncoding(character_set).GetString(Encoding.GetEncoding(character_set).GetBytes(msgbody.ToString().TrimEnd(sep)));
                            }
                            else
                            {
                                decoding_msg = msgbody.ToString().TrimEnd(sep);
                            }
                            break;
                        }

                }
            }
            catch (Exception er)
            {
                if (AssertMail)
                {
                    //WriteLog("msgbody : " + msgbody);
                    WriteLog("Decoding_Common : " + er.Message);
                    WriteLog(er.Message);
                }

                decoding_msg = "";
            }

            //WriteLog(decoding_msg);
            return decoding_msg;

        }
        #endregion

        #region function Q_Decoding

        private string Q_Decoding(string str, string character_set)
        {
            try
            {

                if (AssertMail)
                {
                    WriteLog("character_set : " + character_set);
                }


                byte[] temp_byte = QuotedCoding.GetByteArray(str);


                if (character_set != "")
                {
                    try
                    {
                        return Encoding.GetEncoding(character_set).GetString(temp_byte);
                    }
                    catch
                    {
                        return Encoding.Default.GetString(temp_byte);
                    }
                }
                else
                {
                    return Encoding.Default.GetString(temp_byte);
                }
            }
            catch (Exception er)
            {
                //string dummy = er.Message;
                if (AssertMail)
                {
                    WriteLog("Q_DECODING : " + er.Message);
                    WriteLog("Q_DECODING : " + er.Message);
                }
                return "";
            }
        }

        #endregion

        #region function Encoding_Type
        private int Encoding_Type(string encodingtype)
        {
            int encodetype;

            encodingtype = encodingtype.ToLower();


            if (encodingtype.Equals("base64"))
            {
                // base64 인코딩 타입이냐?
                encodetype = 1;
            }
            else if (encodingtype.Equals("7bit"))
            {
                // 아니면 7bit 이냐?
                encodetype = 2;
            }
            else if (encodingtype.Equals("8bit"))
            {
                // 아니면 8bit 인코딩이냐?
                encodetype = 3;
            }
            else if (encodingtype.Equals("quoted-printable"))
            {
                encodetype = 4;
            }
            else
            {
                // 아니면 알 수 없는 타입이냐?
                encodetype = 5;
            }

            return encodetype;

        }
        #endregion

        #region function Convert_File

        private void Convert_File(string file_name, int encodetype, StringBuilder msg)
        {
            try
            {
                if (AssertMail)
                {
                    //WriteLog("msg : ." + msg.ToString().TrimEnd(sep) + "." + line_end);
                    WriteLog("msg : ." + msg.ToString().Substring(0, 100));
                    WriteLog("File Name : " + file_name + "\t" + "encodetype : " + encodetype.ToString());
                }


                if (!mmsg.Attach.ContainsKey(file_name))
                {
                    mmsg.Attach.Add(file_name, file_name);
                }


                byte[] d_msg;

                if (encodetype == 1)
                {
                    d_msg = Convert.FromBase64String(msg.ToString().TrimEnd(sep));
                }
                else
                {
                    d_msg = Encoding.Default.GetBytes(Decoding_Common(msg, encodetype, "iso"));
                }

                //				WriteLog(msg);


                //				using (StreamWriter sw = new StreamWriter(mmsg.Attach_Save_Path + @"\" + file_name,false,Encoding.Default))
                //				{
                //					sw.Write(d_msg);
                //					sw.Close();
                //				}

                using (FileStream sw = File.Create(mmsg.Attach_Save_Path + @"\" + file_name)) //new StreamWriter(mmsg.Attach_Save_Path + @"\" + file_name,false,Encoding.Default))
                {
                    sw.Write(d_msg, 0, d_msg.Length);
                    sw.Close();
                }
            }
            catch (Exception er)
            {
                WriteLog("CONVERT FILE : " + er.Message);
            }
        }

        #endregion

        #region function Set_Default
        public void Set_Default()
        {
            mmsg.Full_Eml = null;
            mmsg.Full_File_Name = null;
            mmsg.Attach_Save_Path = null;
            mmsg.MsgID = null;
            mmsg.Secure = false;
            mmsg.recall = false;


            mmsg.Boundary = null;
            mmsg.Boundary = new string[4];

            mmsg.ContentType = null;
            mmsg.C_Encoding = null;
            mmsg.X_Receive.Clear();
            mmsg.To.Clear();
            mmsg.Cc.Clear();
            mmsg.Bcc.Clear();
            mmsg.From = null;
            mmsg.Subject = null;
            mmsg.Contents_Html = null;
            mmsg.Contents_Text = null;
            //mmsg.Date = null;
            mmsg.Date = DateTime.Now;
            mmsg.Important = null;
            mmsg.Attach.Clear();
            mmsg.Attach_Count = 0;
            mmsg.Size = 0;
            //mmsg.Body = null;
            mmsg.Body_Str.Remove(0, mmsg.Body_Str.Length);
            mmsg.Content_ID.Clear();

            image = false;
        }
        #endregion

        #region function Forward_Process

        public void Forward_Process(string email, bool local)
        {
            try
            {
                using (StreamReader reader = new StreamReader(mmsg.Full_File_Name, Encoding.Default))
                {
                    string line = reader.ReadLine();

                    StringBuilder sb = new StringBuilder();
                    while (line != null)
                    {

                        #region x_receiver
                        if (line.ToLower().StartsWith("x-receiver:"))
                        {

                        }
                        #endregion

                        #region x-sender
                        else if (line.ToLower().StartsWith("x-sender:"))
                        {
                            sb.Append(line + line_end);
                            sb.Append("x-receiver: " + email + line_end);
                            sb.Append("x-forwarding: ok" + line_end);
                        }
                        #endregion

                        #region else
                        else
                        {
                            sb.Append(line + line_end);
                        }
                        #endregion

                        line = reader.ReadLine();
                    }

                    string path = MailQueuePath;

                    if (local)
                    {
                        path = MailDropPath;
                    }

                    using (StreamWriter sw = new StreamWriter(path + mmsg.Full_File_Name.Substring(mmsg.Full_File_Name.LastIndexOf("\\")) + "." + Get_Random_Value(10000, 99999).ToString() + ".eml", false, Encoding.Default))
                    {
                        sw.WriteLine(sb);
                        sw.Close();
                    }
                    sb = null;
                }
            }
            catch //(Exception er)
            {
                //WriteLog(er.Message + "1");
            }

        }
        #endregion



        #region function Make_RcvChkCode
        /// <summary>
        /// 수신확인 테그를 본문에 삽입하고 MemmoryStream 객체로 전체 본문을 반환한다.
        /// </summary>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public MemoryStream Make_RcvChkCode(string emlFile, string toEmail)
        {
            Set_Default();
            Mmsg.Full_File_Name = emlFile;
            Start_Parsing();
            
            StringBuilder ContentHtml = new StringBuilder();            
            StringBuilder Body_Str = new StringBuilder();
            StringBuilder newEml = new StringBuilder();
            string charset = "";
            string type = "";
            bool IsHtmlContent = false;

            #region findHtml
            using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(Mmsg.Body_Str.ToString())))
            {
                using (StreamReader reader = new StreamReader((Stream)ms, Encoding.Default))
                {
                    string line = reader.ReadLine();
                    int i = 0;
                    int body_start = 0;
                    bool ok = false;

                    #region do
                    while (line != null)
                    {
                        line = line.TrimStart(sep).TrimEnd(sep);
                        //WriteLog(line);

                        if (line.TrimStart(sep).StartsWith("--" + Mmsg.Boundary[0]))
                        {
                            ok = false;
                            body_start = 0;

                        }
                        else if (line.ToLower().StartsWith("content-type:"))
                        {
                            ok = false;
                            body_start = 0;
                            while (line.TrimEnd(sep).EndsWith(";"))
                            {
                                line += reader.ReadLine();
                            }

                            if (line.IndexOf("charset=") > -1)
                            {
                                charset = line.Substring(line.IndexOf("charset=") + 8).Replace("\"", "").Trim();
                                //WriteLog("charset : " + "." + charset + "."  + line_end);
                            }


                            type = line.Substring(13).TrimEnd(sep).TrimStart(sep);
                            //WriteLog("type : " + "." + type + "." + line_end);

                            if (type.EndsWith(";"))
                            {
                                type = type.Substring(0, type.Length - 1);
                            }

                            int check_idx = type.IndexOf(";");

                            if (check_idx > -1)
                            {
                                type = type.Substring(0, check_idx);
                            }


                            if (type.ToLower().StartsWith("text/html"))
                            {
                                IsHtmlContent = true;
                            }
                            else
                            {
                                IsHtmlContent = false;
                            }


                        }
                        else if (line.ToLower().StartsWith("content-transfer-encoding:"))
                        {
                            ok = false;
                            body_start = 0;
                        }
                        else if (line.ToLower().TrimStart(sep).TrimEnd(sep) == "")
                        {
                            ok = true;
                            body_start = i;
                        }
                        if (ok)
                        {
                            if (i >= body_start)
                            {
                                if (IsHtmlContent)
                                {
                                    Body_Str.Append(line + line_end);
                                }
                            }

                        }

                        i++;
                        line = reader.ReadLine();

                    }
                    #endregion

                    string tts = Convert.ToBase64String(Encoding.GetEncoding(charset).GetBytes(ContentHtml.ToString()));
                    byte[] ttl = Encoding.GetEncoding(charset).GetBytes(tts);

                    StringBuilder sb_contents = new StringBuilder();

                    using (MemoryStream ms1 = new MemoryStream(ttl))
                    {
                        byte[] buffer = new byte[DATA_BLOCK_SIZE];
                        int bytesRead;

                        do
                        {
                            bytesRead = ms1.Read(buffer, 0, buffer.Length);

                            sb_contents.Append(Encoding.GetEncoding(charset).GetString(buffer, 0, bytesRead) + line_end);
                        }
                        while (bytesRead == buffer.Length);

                        ms1.Close();
                    }

                    newEml.Append(Mmsg.Header_Str.ToString());
                    newEml.Append(Mmsg.Body_Str.ToString().TrimStart(sep).TrimEnd(sep).Replace(Body_Str.ToString().TrimStart(sep).TrimEnd(sep), sb_contents.ToString().TrimStart(sep).TrimEnd(sep)).ToString().TrimStart(sep).TrimEnd(sep));
                }

            }
            #endregion


            MemoryStream memStream = new MemoryStream(Encoding.Default.GetBytes(newEml.ToString()));

            return memStream; 
        }


        public string Check_File_Name(string str)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(str);
            sb.Replace(@"\", "");
            sb.Replace("/", "");
            sb.Replace(":", "");
            sb.Replace("?", "");
            sb.Replace("\"", "");
            sb.Replace("<", "");
            sb.Replace(">", "");
            sb.Replace("|", "");
            sb.Replace("*", "");

            return sb.ToString();
        }

        public int Get_Random_Value(int start, int end)
        {
            Random rd = new Random();

            int result;
            result = rd.Next(start, end);

            return result;
        }
        #endregion



    }
}
